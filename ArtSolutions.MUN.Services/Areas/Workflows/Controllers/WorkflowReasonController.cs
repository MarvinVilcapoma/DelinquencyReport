using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/WorkflowReason")]
    public class WorkflowReasonController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";
        [HttpGet]
        public IHttpActionResult Get(int id = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowReasonModel().Get(companyID, language, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowReasonModel.Get,", language, ex));
            }
        }

        [HttpGet]
        [Route("GetByPaging")]
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
          int sortDirection, string filterText)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowReasonModel().Get(new WorkflowReasonViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    FilterText = filterText
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowReasonModel.Get,", language, ex));
            }
        }

        [HttpPost]
        public IHttpActionResult Post(WorkflowReasonViewModel model)
        {
            string language = "en";
            Guid token = Guid.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());


                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.Edit);

                model.CompanyID = companyID;
                model.Language = language;
                var result = new WorkflowReasonModel().Update(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Edit.ToString(), model.Name, "Update Reason");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowReasonModel.Update", language, ex));
            }
        }

        [HttpGet]
        [Route("GetByWorkflowID")]
        public IHttpActionResult GetByWorkflowID(int currentIndex, int pageSize, string sortColumn,
           int sortDirection, int workflowID, int workflowVersionID)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowReasonModel().GetByWorkflowID(new WorkflowReasonViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    WorkflowID = workflowID,
                    WorkflowVersionID = workflowVersionID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowReasonModel.GetByWorkflowID,", language, ex));
            }
        }

        [Route("StatusReasonGet")]
        [HttpGet]
        public IHttpActionResult StatusReasonGet(int workflowID, string version, int statusID, int reasonID)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                var model = new WorkflowReasonModel().StatusReasonGet(workflowID, version, language, statusID, reasonID).ToList();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowReasonModel.StatusReasonGet", language, ex));
            }
        }
    }
}

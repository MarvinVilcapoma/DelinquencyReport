using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/WorkflowStatus")]
    public class WorkflowStatusController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";
        [HttpGet]
        [Route("GetByPaging")]
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
           int sortDirection, int workflowID)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowStatusModel().Get(new WorkflowStatusViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    WorkflowID = workflowID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowStatusModel.Get,", language, ex));
            }
        }

        [Route("Post")]
        [HttpPost]
        public IHttpActionResult Post(WorkflowStatusViewModel model)
        {
            string language = "en";
            Guid token = Guid.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());


                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                model.CompanyID = companyID;
                model.Language = language;
                var result = new WorkflowStatusModel().Insert(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.New.ToString(), model.Name, "Insert Status");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowStatusModel.Insert", language, ex));
            }
        }

        [HttpGet]
        [Route("StatusInGroupGet")]
        public IHttpActionResult StatusInGroupGet(int statusID = 0, int groupID = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowSecurityStatusInGroupModel().Get(new WorkflowSecurityStatusInGroupViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    StatusID = statusID,
                    GroupID = groupID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowSecurityStatusInGroupModel.Get,", language, ex));
            }
        }

        [Route("Put")]
        [HttpPost]
        public IHttpActionResult Put(WorkflowStatusViewModel model)
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
                var result = new WorkflowStatusModel().Update(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Edit.ToString(), model.Name, "Update Status");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowStatusModel.Update", language, ex));
            }
        }

        [Route("SequencePost")]
        [HttpPut]
        public IHttpActionResult SequencePost(WorkflowStatusViewModel model)
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
                var result = new WorkflowStatusModel().SequenceUpdate(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Edit.ToString(), model.ID.ToString(), "Update Status Sequence");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowStatusModel.SequenceUpdate", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int workflowID, int id = 0)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var workFlowStatus = new WorkflowStatusModel().WorkflowStatusGet(new WorkflowStatusViewModel
                {
                    WorkflowID = workflowID,
                    CompanyID = companyID,
                    Language = language,
                    ID = id,
                    IsPublic = false,
                    WorkflowVersionID = "1"
                });

                return Ok(workFlowStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowStatusModel.WorkflowStatusGet", language, ex));
            }
        }
    }
}

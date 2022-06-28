using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/WorkflowGroup")]
    public class WorkflowGroupController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get(int id = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowSecurityGroupModel().Get(companyID, language, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CS.Core.WorkflowModels.WorkflowSecurityGroupModel.Get,", language, ex));
            }
        }
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
          int sortDirection, string filterText)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowSecurityGroupModel().Get(new WorkflowSecurityGroupViewModel
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
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupModel.Get,", language, ex));
            }
        }

        [HttpPost]
        public IHttpActionResult Post(WorkflowSecurityGroupViewModel model)
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
                var result = new WorkflowSecurityGroupModel().Insert(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.New.ToString(), model.Name, "Insert Group");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupModel.Insert", language, ex));
            }
        }

        [HttpPut]
        public IHttpActionResult Put(WorkflowSecurityGroupViewModel model)
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
                var result = new WorkflowSecurityGroupModel().Update(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Edit.ToString(), model.Name, "Update Group");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupModel.Insert", language, ex));
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

                var result = new WorkflowSecurityGroupModel().GetByWorkflowID(new WorkflowSecurityGroupViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    WorkFlowID = workflowID,
                    WorkflowVersionID = workflowVersionID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupModel.GetByWorkflowID,", language, ex));
            }
        }

        [HttpGet]
        [Route("GroupUsersGetByPaging")]
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
            int sortDirection, int workflowID, int workflowVersionID)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowSecurityGroupUsersModel().GetByPaging(new WorkflowSecurityGroupUsersViewModel
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
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupUsersModel.GetByPaging,", language, ex));
            }
        }

        [HttpPost]
        [Route("GroupUserInsert")]
        public IHttpActionResult GroupUserInsert(WorkflowSecurityGroupUsersViewModel model)
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
                var result = new WorkflowSecurityGroupUsersModel().Insert(model);
                Common.InsertActivity(token, companyID, _featureID, Actions.New.ToString(), null, "Insert GroupUser");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupUsersModel.Insert", language, ex));
            }
        }

        [Route("GroupUserGet")]
        [HttpGet]
        public IHttpActionResult GroupUserGet(int groupID, int statusID = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowSecurityGroupUsersModel().Get(statusID, companyID, groupID);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkflowModels.WorkflowSecurityGroupUsersModel.Get,", language, ex));
            }
        }
    }
}

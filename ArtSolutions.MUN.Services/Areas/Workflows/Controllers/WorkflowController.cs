using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Areas.Workflows.Models;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/Workflow")]
    public class WorkflowController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";

        [HttpGet]
        [Route("GetByPaging")]
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
           int sortDirection, string filterText, int documentTypeID = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowModel().Get(new WorkflowViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    FilterText = filterText,
                    DocumentTypeID = documentTypeID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkFlowModel.Get,", language, ex));
            }
        }

        [HttpGet]
        [Route("DocumentTypeGet")]
        public IHttpActionResult DocumentTypeGet(int id = 0, bool isMunicipalityOnly=false)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new DocumentTypeModel().Get(id, isMunicipalityOnly, language);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.HR.Core.WorkFlowModels.DocumentTypeModel.Get,", language, ex));
            }
        }

        [Route("ActiveStatusUpdate")]
        [HttpPut]
        public IHttpActionResult ActiveStatusUpdate(WorkflowViewModel model)
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

                var result = new WorkflowModel().ActiveStatusUpdate(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Edit.ToString(), model.IsActive.ToString(), "Update Active Status");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkFlowModel.ActiveStatusUpdate", language, ex));
            }
        }

        [Route("Post")]
        [HttpPost]
        public IHttpActionResult Post(WorkflowViewModel model)
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

                var result = new WorkflowModel().Insert(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.New.ToString(), model.Name, "Insert Workflow");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.CSR.Core.WorkFlowModels.WorkFlowModel.Insert", language, ex));
            }
        }

        [Route("WorkflowSecurityGroupUsersUpdateProfile")]
        [HttpPost]
        public IHttpActionResult WorkflowSecurityGroupUsersUpdateProfile(UserProfile userProfileModel)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();

                List<MUNDOCDocumentWorkflowSecurityGroupUsersUpdateProfile_Result> lstWorkflow = new WorkflowSecurityGroupUsersModel().DocumentWorkflowSecurityGroupUsersUpdateProfile(companyID, userProfileModel.UserID, userProfileModel.FirstName, userProfileModel.LastName).ToList();
                return Ok(lstWorkflow == null ? 0 : 1);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.HR.Core.WorkflowModel.DocumentWorkflowGetByDocumentTypeIDs", language, ex));
            }
        }

        [Route("DocumentWorkflowHistoryLogGetForSplit")]
        [HttpGet]
        public IHttpActionResult DocumentWorkflowHistoryLogGetForSplit(int TransferID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new WorkflowModel().DocumentWorkflowHistoryLogGetForSplit(companyId, TransferID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.Finance.Core.WorkFlowModel.DocumentWorkflowHistoryLogGetForRequisition", language, ex));
            }
        }

    }
}

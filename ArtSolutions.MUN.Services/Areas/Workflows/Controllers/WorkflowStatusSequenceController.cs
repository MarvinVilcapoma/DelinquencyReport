using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/WorkflowStatusSequence")]
    public class WorkflowStatusSequenceController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";
        [HttpPost]
        public IHttpActionResult Post(WorkflowStatusSequenceViewModel model)
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

                var result = new WorkflowStatusSequenceModel().Insert(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.New.ToString(), model.WorkflowStatusID.ToString(), "Insert Status Sequence");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowStatusSequenceModel.Insert", language, ex));
            }
        }
        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int workflowID = 0, int workflowVersionID = 0, int statusID = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowStatusSequenceModel().Get(new WorkflowStatusSequenceViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    WorkflowStatusID = statusID,
                    WorkflowID = workflowID,
                    WorkflowVersionID = workflowVersionID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowStatusSequenceModel.Get,", language, ex));
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(WorkflowStatusSequenceViewModel model)
        {
            string language = "en";
            Guid token = Guid.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());

                language = Request.Headers.GetValues("Language").First().ToString();

                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.Delete);

                model.CompanyID = companyID;

                var result = new WorkflowStatusSequenceModel().Delete(model);

                Common.InsertActivity(token, companyID, _featureID, Actions.Delete.ToString(), model.WorkflowStatusID.ToString(), "Delete Status Sequence");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowStatusSequenceModel.Delete", language, ex));
            }
        }

        [HttpGet]
        [Route("GetByPaging")]
        public IHttpActionResult GetByPaging(int workflowID = 0, int workflowVersionID = 0)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowStatusSequenceModel().GetByPaging(new WorkflowStatusSequenceViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    WorkflowID = workflowID,
                    WorkflowVersionID = workflowVersionID
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowStatusSequenceModel.GetByPaging,", language, ex));
            }
        }
    }
}

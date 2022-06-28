using ArtSolutions.MUN.Core.SchedulerEmailModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.SchedulerEmailModels.Controllers
{
    [RoutePrefix("api/SchedulerEmail")]
    public class SchedulerEmailController : ApiController
    {
        const string _featureID = "00000000-0009-0002-0001-000000000000";

        #region Service  

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new SchedulerEmail().Get(companyId, language, id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.SchedulerEmailModels.SchedulerEmail.Get", language, ex));
            }
        }

        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult GetByPaging(int? schedulerEmailID, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new SchedulerEmail().GetByPaging(schedulerEmailID, companyId, language, filterText, pageIndex, pageSize, sortColumn, sortOrder);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.SchedulerEmailModels.SchedulerEmail.GetByPaging", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(SchedulerEmailModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);
                model.CompanyID = companyId;
                int schedulerEmailID = new SchedulerEmail().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), schedulerEmailID.ToString(), string.Empty);
                return Ok(schedulerEmailID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.SchedulerEmailModels.SchedulerEmail.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(SchedulerEmailModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.CompanyID = companyId;
                int schedulerEmailID = new SchedulerEmail().Update(model);
                
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), schedulerEmailID.ToString(), string.Empty);
                return Ok(schedulerEmailID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.SchedulerEmailModels.SchedulerEmail.Update", language, ex));
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IHttpActionResult Delete(SchedulerEmailModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Delete);

                new SchedulerEmail().Delete(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.SchedulerEmailModels.SchedulerEmail.Delete", language, ex));
            }
        }

        #endregion


    }
}

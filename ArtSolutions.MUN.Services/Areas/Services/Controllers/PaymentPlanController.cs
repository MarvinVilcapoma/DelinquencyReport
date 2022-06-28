using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.ServiceModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Services.Controllers
{
    [RoutePrefix("api/PaymentPlan")]
    public class PaymentPlanController : ApiController
    {
        const string _featureID = "00000000-0009-0002-0001-000000000000";

        #region Service Payment Plan

        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult GetByPaging(string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                PaymentPlanListModel list = new PaymentPlan().GetByPaging(companyID, language, filterText, pageIndex, pageSize, sortColumn, sortOrder);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.PaymentPlan.GetByPaging", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? id, DateTime? date, bool? isActive)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                List<MUNSERPaymentPlanGet_Result> list = new PaymentPlan().Get(companyID, language, id, date, isActive);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.PaymentPlan.Get", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(PaymentPlanModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Locale = language;
                model.CompanyID = companyId;
                int paymentPlanID = new PaymentPlan().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), paymentPlanID.ToString(), string.Empty);
                return Ok(paymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.PaymentPlan.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(PaymentPlanModel model)
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
                model.Locale = language;
                int paymentPlanID = new PaymentPlan().Update(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), paymentPlanID.ToString(), string.Empty);
                return Ok(paymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.PaymentPlan.Update", language, ex));
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public IHttpActionResult UpdateStatus(PaymentPlanModel model)
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
                int paymentPlanID = new PaymentPlan().UpdateStatus(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), paymentPlanID.ToString(), string.Empty);
                return Ok(paymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.PaymentPlan.UpdateStatus", language, ex));
            }
        }

        #endregion
    }
}
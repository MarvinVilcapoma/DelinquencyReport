using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/AccountPaymentPlan")]
    public class AccountPaymentPlanController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";

        #region Account Payment Plan
        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult GetByPaging(string filterText, int? accountID, int? accountPropertyID, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                ListAccountPaymentPlanModel list = new AccountPaymentPlan().GetByPaging(companyID, language, filterText, accountID, accountPropertyID, pageIndex, pageSize, sortColumn, sortOrder);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.GetByPaging", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? id, int? accountID, bool? isActive)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                List<MUNSERAccountPaymentPlanGet_Result> list = new AccountPaymentPlan().Get(companyID, accountID, id, isActive, language);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.Get", language, ex));
            }
        }

        [Route("GetNotPaid")]
        [HttpGet]
        public IHttpActionResult GetNotPaid(int? id, int? accountID, DateTime? date, bool? isActive)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                List<MUNSERAccountPaymentPlanGetNotPaid_Result> list = new AccountPaymentPlan().GetNotPaid(companyID, accountID, id, date, isActive, language);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.GetNotPaid", language, ex));
            }
        }
        [Route("IsAccountHavePaymentPlan")]
        [HttpGet]
        public IHttpActionResult IsAccountHavePaymentPlan(int accountID)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                bool result = new AccountPaymentPlan().IsAccountHavePaymentPlan(companyID, accountID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.IsAccountHavePaymentPlan", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(AccountPaymentPlanModel model)
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
                int accountPaymentPlanID = new AccountPaymentPlan().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), accountPaymentPlanID.ToString(), string.Empty);
                return Ok(accountPaymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.Insert", language, ex));
            }
        }


        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(AccountPaymentPlanModel model)
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
                int accountPaymentPlanID = new AccountPaymentPlan().Update(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), accountPaymentPlanID.ToString(), string.Empty);
                return Ok(accountPaymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.Update", language, ex));
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IHttpActionResult Delete(AccountPaymentPlanModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Delete);

                new AccountPaymentPlan().Delete(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Delete.ToString(), model.ID.ToString(), string.Empty);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.Delete", language, ex));
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public IHttpActionResult UpdateStatus(AccountPaymentPlanModel model)
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
                int accountPaymentPlanID = new AccountPaymentPlan().UpdateStatus(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), accountPaymentPlanID.ToString(), string.Empty);
                return Ok(accountPaymentPlanID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.UpdateStatus", language, ex));
            }
        }
        #endregion

        #region Account Payment Plan Detail
        [Route("GetAccountPaymentPlanDetail")]
        [HttpGet]
        public IHttpActionResult GetAccountPaymentPlanDetail(int? id, string accountPaymentPlanIDs, bool? isActive, bool? isPaid, bool? isRemoveInterest)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                List<MUNSERAccountPaymentPlanDetailGet_Result> list = new AccountPaymentPlan().GetAccountPaymentPlanDetail(companyID, id, accountPaymentPlanIDs, isActive, isPaid, language, isRemoveInterest);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.GetAccountPaymentPlanDetail", language, ex));
            }
        }
        #endregion

        #region Account Service Payment Plan
        [Route("GetAccountServicePaymentPlan")]
        [HttpGet]
        public IHttpActionResult GetAccountServicePaymentPlan(int? accountID, int? accountPaymentPlanID, int? accountServiceID)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                List<MUNSERAccountServicePaymentPlanGet_Result> list = new AccountPaymentPlan().GetAccountServicePaymentPlan(companyID, language, accountID, accountPaymentPlanID, accountServiceID);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.HR.Core.AccountModels.AccountPaymentPlan.GetAccountServicePaymentPlan", language, ex));
            }
        }

        [Route("GetAccountServicePaymentPlanSummary")]
        [HttpGet]
        public IHttpActionResult GetAccountServicePaymentPlanSummary(int accountPaymentPlanID)
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                List<MUNSERAccountServicePaymentPlanSummaryGet_Result> list = new AccountPaymentPlan().GetAccountServicePaymentPlanSummary(companyID, language, accountPaymentPlanID);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.HR.Core.AccountModels.AccountPaymentPlan.GetAccountServicePaymentPlanSummary", language, ex));
            }
        }
        #endregion

        #region Print
        [Route("Print")]
        [HttpGet]
        public IHttpActionResult Print(int accountID, int AccountPaymentPlanID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountPaymentPlan().Print(companyId, language, accountID, AccountPaymentPlanID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.Print", language, ex));
            }
        }
        #endregion

        #region Export
        [Route("GetExportPaymentPlan")]
        [HttpGet]
        public IHttpActionResult GetExportPaymentPlan()
        {
            Guid token = Guid.Empty;
            string language = string.Empty;
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                PaymentPlanExportList list = new AccountPaymentPlan().GetExportpaymentPlan(companyID, language);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountPaymentPlan.GetExportPaymentPlan", language, ex));
            }

        }
        #endregion
    }
}

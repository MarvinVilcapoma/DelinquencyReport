using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using ArtSolutions.MUN.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/Payment")]
    public class PaymentController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0002-000000000000";
        const string _manualreceiptfeatureID = "00000000-0009-0008-0006-000000000000";

        #region Payment Receipt Get & Print

        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(int? AccountID, int? ID, int? year, string FilterText, bool? IsVoid, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Payment().GetWithPaging(CompanyID, AccountID, ID, year, FilterText, IsVoid, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetWithPaging", language, ex));
            }
        }

        [Route("GetForClosingEntry")]
        [HttpGet]
        public IHttpActionResult GetForClosingEntry(int? AccountID, int? ID, Guid? CashierID, DateTime? ClosingDate, bool? OnlyPostedReceipts, int? PaymentOptionID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Payment().GetForClosingEntry(CompanyID, AccountID, ID, CashierID, ClosingDate, language, OnlyPostedReceipts, PaymentOptionID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetForClosingEntry", language, ex));
            }
        }

        [Route("GetAsObject")]
        [HttpGet]
        public IHttpActionResult GetAsObject(int? AccountID, int? ID, string FilterText, bool? IsVoid, bool? IsPost)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Payment().GetAsObject(CompanyID, AccountID, ID, FilterText, IsVoid, language, IsPost);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetAsObject", language, ex));
            }
        }

        [Route("GetPrint")]
        [HttpGet]
        public IHttpActionResult GetPrint(int ID, int? ServiceTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Payment().GetPrint(ID, ServiceTypeID, CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetPrint", language, ex));
            }
        }

        [Route("GetPaymentPlanPrint")]
        [HttpGet]
        public IHttpActionResult GetPaymentPlanPrint(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Payment().GetPaymentPlanPrint(ID, CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetPaymentPlanPrint", language, ex));
            }
        }

        [Route("PrintTemplateGet")]
        [HttpGet]
        public IHttpActionResult PrintTemplateGet(int? ID, int? DocumentTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var result = new PrintTemplate().Get(ID, companyId, DocumentTypeID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PrintTemplate.Get", language, ex));
            }
        }

        #endregion

        #region Payment Receipt Insert

        [Route("InsertByInvoice")]
        [HttpPost]
        public IHttpActionResult InsertByInvoice(PaymentModel model)
        {
            Guid token = Guid.Empty;
            //string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.Language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                model.NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, model.Language);
                model.IsOfficialReceipt = model.NumberPrefix == "RO" ? true : false;
                if (model.IsManual)
                    model.NumberPrefix = null;

                int paymentId = new Payment().InsertByInvoice(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), paymentId.ToString(), "ID");
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.InsertByInvoice", model.Language, ex));
            }
        }

        [Route("InsertByOtherService")]
        [HttpPost]
        public IHttpActionResult InsertByOtherService(PaymentModel model)
        {
            Guid token = Guid.Empty;
            //string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.Language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                model.NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, model.Language);
                model.IsOfficialReceipt = model.NumberPrefix == "RO" ? true : false;
                if (model.IsManual)
                    model.NumberPrefix = null;

                int paymentId = new Payment().InsertByOtherService(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), paymentId.ToString(), "ID");
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.InsertByOtherService", model.Language, ex));
            }
        }

        [Route("InsertByService")]
        [HttpPost]
        public IHttpActionResult InsertByService(PaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                model.IsOfficialReceipt = model.NumberPrefix == "RO" ? true : false;
                if (model.IsManual)
                    model.NumberPrefix = null;

                int paymentId = new Payment().InsertByService(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), paymentId.ToString(), "ID");
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.InsertByService", language, ex));
            }
        }

        [Route("InsertByPaymentPlan")]
        [HttpPost]
        public IHttpActionResult InsertByPaymentPlan(PaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                model.IsOfficialReceipt = model.NumberPrefix == "RO" ? true : false;

                int paymentId = new Payment().InsertByPaymentPlan(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), paymentId.ToString(), "ID");
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.InsertByPaymentPlan", language, ex));
            }
        }

        [Route("InsertByDebitNote")]
        [HttpPost]
        public IHttpActionResult InsertByDebitNote(PaymentModel model)
        {
            Guid token = Guid.Empty;
            //string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.Language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                model.NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, model.Language);
                model.IsOfficialReceipt = model.NumberPrefix == "RO" ? true : false;
                if (model.IsManual)
                    model.NumberPrefix = null;

                int paymentId = new Payment().InsertByDebitNote(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), paymentId.ToString(), "ID");
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.InsertByOtherService", model.Language, ex));
            }
        }

        #endregion

        #region Payment Receipt Void

        [Route("Void")]
        [HttpPost]
        public IHttpActionResult Void(PaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                var result = new Payment().Void(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "ID");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.Void", language, ex));
            }
        }

        [Route("PaymentPlanVoid")]
        [HttpPost]
        public IHttpActionResult PaymentPlanVoid(PaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                var result = new Payment().PaymentPlanVoid(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "ID");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.PaymentPlanVoid", language, ex));
            }
        }

        [Route("GetAvailableCreditBalance")]
        [HttpGet]
        public IHttpActionResult GetAvailableCreditBalance(int AccountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);
                var result = new Payment().GetAvailableCreditBalance(AccountID, CompanyID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.GetAvailableCreditBalance", language, ex));
            }
        }

        #endregion

        #region Manual Payment Receipt

        [Route("ValidateUserPermissionForManualReceipt")]
        [HttpGet]
        public IHttpActionResult ValidateUserPermissionForManualReceipt()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                try
                {
                    Common.ValidateUserPermission(token, companyId, Guid.Parse(_manualreceiptfeatureID), Actions.New);
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Areas.Collections.Controllers.ValidateUserPermissionForManualReceipt", language, ex));
            }
        }

        [Route("GetAccountDebitNoteExist")]
        [HttpGet]
        public IHttpActionResult GetAccountDebitNoteExist(int AccountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);
                var result = new DebitNote().GetAccountDebitNoteExists(CompanyID, AccountID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.DebitNote.MUNCOLDebitNoteExistByAccount", language, ex));
            }
        }
        #endregion

        #region Security Calls

        [Route("SalesCashierGet")]
        [HttpGet]
        public IHttpActionResult SalesCashierGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<SalesCashierModel> model = new SecuirtyAPIModel().SalesCashierGet(token, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Models.SecuirtyAPIModel.SalesCashierGet", language, ex));
            }
        }

        #endregion

        #region Finance Calls

        [Route("PaymentOptionsGet")]
        [HttpGet]
        public IHttpActionResult PaymentOptionsGet(int? CashierId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new FinanceAPIModel().PaymentOptionGet(token, companyId, language, CashierId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.PaymentOptionsModel.PaymentOptionsGet", language, ex));
            }
        }

        #endregion

        #region Import Bank Payments
        [Route("ValidateImportBankPayments")]
        [HttpPost]
        public IHttpActionResult ValidateImportBankPayments(BankPaymentsImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                var data = new Payment().ImportBankPaymentsValidation(companyID, language, model);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Import Bank Payments");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.ImportBankPaymentsValidation", language, ex));
            }
        }

        [Route("InsertImportBankPayments")]
        [HttpPost]
        public IHttpActionResult InsertImportBankPayments(BankPaymentsImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                int result = new Payment().ImportBankPaymentsInsert(companyId, language, model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Bank Payments");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Payment.ImportBankPaymentsInsert", language, ex));
            }
        }
        #endregion
    }
}
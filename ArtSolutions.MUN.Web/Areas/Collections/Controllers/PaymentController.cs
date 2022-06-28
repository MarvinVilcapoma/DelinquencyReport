using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Reports.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class PaymentController : BaseController
    {
        bool status = false;
        string Msg = string.Empty, htmlString = string.Empty, nextHtmlString = string.Empty;

        #region List Payment Receipt
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, int? year, string filterText)
        {
            JsonResult jResult = new JsonResult();
            DateTime outDate;
            decimal outDecimal;

            try
            {
                if (Decimal.TryParse(filterText, out outDecimal))
                {
                    try
                    {
                        filterText = Convert.ToDecimal(filterText).ToString(CultureInfo.InvariantCulture);
                    }
                    catch { }
                }

                if (DateTime.TryParse(filterText, out outDate))
                {
                    try
                    {
                        filterText = Convert.ToDateTime(filterText).ToString("d", CultureInfo.InvariantCulture);
                    }
                    catch { }
                }

                PaymentListModel list = new Payment().GetWithPaging(jqdtParams, year, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.PaymentList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region New Payment Receipt
        [HttpGet]
        public ActionResult Add(string ActionType, int? InvoiceID, int? accountID, int? serviceTypeID, int? accountPaymentPlanID)
        {
            PaymentModel model = new PaymentModel();
            try
            {
                if (ActionType == "invoice" && InvoiceID != null)
                {
                    model = new Payment().GetByInvoice(InvoiceID);
                    return View("AddByInvoice", model);
                }
                else if (ActionType == "service")
                {
                    model = new Payment().GetByService(accountID, serviceTypeID);
                    return View("AddByService", model);
                }
                else if (ActionType == "propertytax")
                {
                    model = new Payment().GetByPropertyTax();
                    return View("AddByPropertyTax", model);
                }
                else if (ActionType == "paymentplan")
                {
                    model = new Payment().GetByPaymentPlan(accountID, accountPaymentPlanID);
                    return View("AddByPaymentPlan", model);
                }
                else if (ActionType == "otherservice")
                {
                    model = new Payment().GetByOtherService(accountID, serviceTypeID);
                    return View("AddByOtherService", model);
                }
                else if (ActionType == "debitnote")
                {
                    model = new Payment().GetByDebitNote(accountID);
                    return View("AddByDebitNote", model);
                }
                throw new Exception(Resources.Global.GeneralErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PaymentModel model, string actionType)
        {
            bool status = false;
            string ErrorMsg = "";
            int paymentId = 0;
            JsonResult jsonResult = new JsonResult();

            Payment _payment = new Payment();
            try
            {
                if (model == null)
                {
                    status = false;
                    ErrorMsg = Resources.Global.ValNull;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }

                if (actionType == "invoicesave" || actionType == "invoicesaveandcontinue")
                {
                    _payment.ValidateInvoicePayment(model);
                    paymentId = _payment.InsertByInvoice(model);
                }
                else if (actionType == "servicesave" || actionType == "servicesaveandcontinue")
                {
                    //if (model.ServiceTypeID == 1) // Business License 
                    //{
                    //    model.AccountServiceCollectionDetailList = new BusinessLicenseAccountServiceModel().GetCollectionDetailList(model.BusinessLicenseAccountServiceList);
                    //}
                    _payment.ValidateServicePayment(model);
                    paymentId = _payment.InsertByService(model);
                }
                else if (actionType == "paymentplansave" || actionType == "paymentplansaveandcontinue")
                {
                    _payment.ValidatePaymentPlanPayment(model);
                    paymentId = _payment.InsertByPaymentPlan(model);
                }
                else if (actionType == "otherservicesave" || actionType == "otherservicesaveandcontinue")
                {
                    _payment.ValidateOtherServicePayment(model);
                    paymentId = _payment.InsertByOtherService(model);
                }
                else if (actionType == "debitnotesave" || actionType == "debitnotesaveandcontinue")
                {
                    _payment.ValidateDebitNotePayment(model);
                    paymentId = _payment.InsertByDebitNote(model);
                }

                if (paymentId > 0)
                {
                    status = true;
                    ErrorMsg = Resources.Global.SavedSuccessfullyMessage;
                    TempData["SuccessMsg"] = ErrorMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            return Json(new { status = status, message = ErrorMsg, data = "", paymentID = paymentId, serviceTypeId = model.ServiceTypeID, actionType = actionType }, JsonRequestBehavior.AllowGet);
        }

        #region Payment option methods
        public ActionResult GetPaymentOption()
        {
            PaymentOptionModel model = new PaymentOptionModel();
            try
            {
                model.PaymentOptions = new SelectList(new AccountModel().GetSupportValues(17), "ID", "Name").OrderBy(x => x.Text);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Collections/Views/Payment/_PaymentOptionList.cshtml", model);
        }
        #endregion

        #region Invoice methods
        public ActionResult GetPostedInvoices(int accountID, string type)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            try
            {
                if (type == "invoice")
                {
                    new DebitNote().GetAccountDebitNoteExist(accountID);
                    AccountModel accountModel = new AccountModel().Get(accountID.ToString(), null, null, null, null, null, true).FirstOrDefault();
                    if (accountModel.IsInJudicialCollection == null)
                    {
                        model = new InvoiceModel().Get(null, accountID, null, true, true);
                        return PartialView("~/Areas/Collections/Views/Payment/_ManualInvoiceList.cshtml", model);
                    }
                    else
                    {
                        return Json(new { status = false, message = Resources.Global.AccountUnderJudicialCollectionMsg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    throw new Exception(Resources.Global.GeneralErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Service methods 
        public ActionResult GetAccountServiceType(int accountID)
        {
            try
            {
                var model = new ServiceTypeModel().GetNotPaid(accountID, true);
                return PartialView("~/Areas/Collections/Views/Payment/_AccountServiceTypeList.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAccountServiceCollectionDetail(int accountId, DateTime? paymentDate, string selectedItemIds, bool IsIvaApply = false, bool applybyAmnesty = false)
        {
            try
            {
                //if (servicetypeId == 1)
                //    return PartialView("~/Areas/Collections/Views/Payment/_BusinessLicenseAccountServiceList.cshtml", new Payment().GetAccountServiceCollectionDetailForBusinessLicense(accountId, servicetypeId, paymentDate, TotalAmount, selectedItemIds));
                //else                    
                new DebitNote().GetAccountDebitNoteExist(accountId);
                return PartialView("~/Areas/Collections/Views/Payment/_AccountServiceCollectionDetailList.cshtml", new Payment().GetAccountServiceCollectionDetail(accountId, paymentDate, selectedItemIds, IsIvaApply, applybyAmnesty));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetAccountServiceCollectionDetailOnly(int accountId, DateTime? paymentDate, bool IsIvaApply = false, bool applybyAmnesty = false)
        {
            try
            {
                new DebitNote().GetAccountDebitNoteExist(accountId);
                AccountModel accountModel = new AccountModel().Get(accountId.ToString(), null, null, null, null, null, true).FirstOrDefault();
                if (accountModel.IsInJudicialCollection == null)
                {
                    List<AccountServiceCollectionDetailModel> model = new Payment().GetAccountServiceCollectionDetailOnly(accountId, paymentDate, IsIvaApply, applybyAmnesty);
                    ViewBag.AccountServiceCollectionDetailListJson = JsonConvert.SerializeObject(model); ;
                    return PartialView("~/Areas/Collections/Views/Payment/_AccountServiceCollectionDetailList.cshtml", model);
                }
                else
                {
                    return Json(new { status = false, message = Resources.Global.AccountUnderJudicialCollectionMsg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
     
        #region Get Debit Notes
        [HttpPost]
        public ActionResult GetDebitNotes(int AccountID, string selectedItemIds)
        {
            try
            {
                return PartialView("~/Areas/Collections/Views/Payment/_AccountDebitNoteList.cshtml", new Payment().GetDebitNotes(AccountID, selectedItemIds));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDebitNotesOnly(int accountId)
        {
            try
            {
                AccountModel accountModel = new AccountModel().Get(accountId.ToString(), null, null, null, null, null, true).FirstOrDefault();

                if (accountModel.IsInJudicialCollection == null)
                {
                    List<DebitNoteModel> model = new Payment().GetDebitNotesOnly(accountId);
                    ViewBag.DebitNoteJsonDetail = JsonConvert.SerializeObject(model);
                    return PartialView("~/Areas/Collections/Views/Payment/_AccountDebitNoteList.cshtml", model);
                }
                else
                {
                    return Json(new { status = false, message = Resources.Global.AccountUnderJudicialCollectionMsg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    
        #region Property Tax methods 
        public ActionResult GetAccountPropertyRight(int accountPropertyID)
        {
            try
            {
                var model = new AccountProperty().GetNotPaid(accountPropertyID, true);
                return PartialView("~/Areas/Collections/Views/Payment/_AccountPropertyRightList.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAccountServiceCollectionPropertyTaxDetail(int accountPropertyID, int accountPropertyRightID, DateTime? paymentDate, decimal TotalAmount, string selectedItemIds)
        {
            try
            {
                return PartialView("~/Areas/Collections/Views/Payment/_AccountServicePropertyTaxCollectionDetailList.cshtml", new Payment().GetAccountServiceCollectionPropertyTaxDetail(accountPropertyID, accountPropertyRightID, paymentDate, TotalAmount, selectedItemIds));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Payment Plan methods
        public ActionResult GetAccountPaymentPlan(int accountID, DateTime receiptDate)
        {
            try
            {
                AccountModel accountModel = new AccountModel().Get(accountID.ToString(), null, null, null, null, null, true).FirstOrDefault();
                if (accountModel.IsInJudicialCollection == null)
                {
                    new DebitNote().GetAccountDebitNoteExist(accountID);
                    var model = new Accounts.Models.AccountPaymentPlan().GetNotPaid(null, accountID, true, receiptDate);
                    return PartialView("~/Areas/Collections/Views/Payment/_AccountPaymentPlanList.cshtml", model);
                }
                else
                {
                    return Json(new { status = false, message = Resources.Global.AccountUnderJudicialCollectionMsg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAccountPaymentPlanDetail(int accountId, string accountpaymentplanIds, string AccountPaymentPlanDetailIDs, bool isRemoveInterest)
        {
            try
            {
                List<AccountPaymentPlanDetailModel> model = new List<AccountPaymentPlanDetailModel>();
                if (!string.IsNullOrEmpty(accountpaymentplanIds))
                {
                    model = new AccountPaymentPlanDetail().Get(null, accountpaymentplanIds, true, false, isRemoveInterest);
                    if (model != null && model.Count() > 0)
                    {
                        List<int> CollectionID = new List<int>();
                        if (!string.IsNullOrEmpty(AccountPaymentPlanDetailIDs))
                        {
                            CollectionID = AccountPaymentPlanDetailIDs.Split(',').Select(int.Parse).ToList();
                        }

                        model.ForEach(x =>
                        {
                            if (CollectionID.Where(d => d == x.ID).Count() > 0)
                            {
                                x.SelectedItem = true;
                            }
                        });
                    }
                }
                return PartialView("~/Areas/Collections/Views/Payment/_AccountPaymentPlanDetailList.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region View & Void Payment Receipt
        public ActionResult View(int? ID, string Type, int? TypeID = null)
        {
            if (ID == null || string.IsNullOrEmpty(Type))
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            PaymentPrintModel model = new PaymentPrintModel();
            try
            {
                if (Type == "PaymentPlan")
                {
                    model = new Payment().GetPaymentPlanPrint(ID.Value);
                }
                else
                {
                    model = new Payment().GetPrint(ID.Value, TypeID);
                }

                if (model.Payment == null || model.Payment.ID == 0)
                {
                    TempData["ErrorMsg"] = Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    model.PrintTemplate.TemplateContent = new Payment().PreparePrintTemplate(model, Type, TypeID, true);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }

        public FileResult ExportPaymentReceipt(int paymentId, int exportType, string Type, int? serviceTypeId)
        {
            PaymentPrintModel model = new Payment().GetPrint(paymentId, serviceTypeId);
            model.PrintTemplate.TemplateContent = new Payment().PreparePrintTemplate(model, Type, serviceTypeId, false);

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Payment/ExportTemplate/_PaymentReceiptByService.cshtml", model);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Global.PaymentReceipt, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public ActionResult Print(int? Id, string Type, int? TypeId)
        {
            if (Id == null || string.IsNullOrEmpty(Type))
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            try
            {
                PaymentPrintModel model = new PaymentPrintModel();
                if (Type == "PaymentPlan")
                {
                    model = new Payment().GetPaymentPlanPrint(Id.Value);
                }
                else
                {
                    model = new Payment().GetPrint(Id.Value, TypeId);
                }

                if (model.Payment == null)
                {
                    TempData["ErrorMsg"] = Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    if (Type != "PaymentPlan" && model.PrintTemplate.ID != 6 && model.PrintTemplate.ID != 10 && model.PrintTemplate.ID != 11)
                    {
                        return View("~/Areas/Collections/Views/Payment/RollPrint.cshtml", model);
                    }
                    else if (Type == "PaymentPlan")
                    {
                        return View("~/Areas/Collections/Views/Payment/PaymentPlanPrint.cshtml", model);
                    }
                    else
                    {
                        //model.PrintTemplate.TemplateContent = new Payment().PreparePrintTemplate(model, Type, TypeId, false);
                        //return View("~/Areas/Collections/Views/Payment/ExportTemplate/_PaymentReceiptByService.cshtml", model);
                        return View("~/Areas/Collections/Views/Payment/InvoicePrint.cshtml", model);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("Index");
        }

        [HttpGet]
        public ActionResult RollPrint(int? Id, string Type, int? TypeId)
        {
            if (Id == null || string.IsNullOrEmpty(Type))
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            try
            {
                PaymentPrintModel model = new PaymentPrintModel();
                if (Type == "PaymentPlan")
                {
                    model = new Payment().GetPaymentPlanPrint(Id.Value);
                }
                else
                {
                    model = new Payment().GetPrint(Id.Value, TypeId);
                }

                if (model.Payment == null)
                {
                    TempData["ErrorMsg"] = Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    if (Type != "PaymentPlan" && model.PrintTemplate.ID != 6 && model.PrintTemplate.ID != 10 && model.PrintTemplate.ID != 11)
                    {
                        return View("~/Areas/Collections/Views/Payment/RollPrint.cshtml", model);
                    }
                    else if (Type == "PaymentPlan")
                    {
                        return View("~/Areas/Collections/Views/Payment/PaymentPlanPrint.cshtml", model);
                    }
                    else
                    {
                        return View("~/Areas/Collections/Views/Payment/InvoicePrint.cshtml", model);
                    }
                    //return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        public JsonResult VoidPayment(PaymentPrintModel model)
        {
            try
            {
                Payment _model = new Payment();
                if (_model.ValidatePaymentForVoid(model.Payment.FINJournalID, model.Payment.DocumentTypeID))
                {
                    return Json(new { status = false, message = Resources.COLPayment.VoidFinanceMsg }, JsonRequestBehavior.AllowGet);
                }

                string type = !string.IsNullOrEmpty(model.Payment.PaymentPlanName) ? "PaymentPlan" : "Service";
                if (type == "PaymentPlan")
                {
                    _model.PaymentPlanVoid(model.Payment);
                }
                else
                {
                    _model.Void(model.Payment);
                }

                return Json(new { status = true, message = Resources.COLPayment.VoidSuccessMsg, ID = model.Payment.ID, Type = type, ServiceTypeID = model.Payment.ServiceTypeID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DebitNoteAlreadyExist(int AccountID)
        {
            try
            {
                new DebitNote().GetAccountDebitNoteExist(AccountID);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Credit Note
        [HttpGet]
        public ActionResult CreditNote(int PaymentID)
        {
            bool status;
            string msg = "";
            string htmlString = "";

            CreditNoteModel model = new CreditNoteModel();
            model.PaymentID = PaymentID;
            try
            {
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Payment/_CreditNote.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreditNote(CreditNoteModel model)
        {
            string Message = "";
            bool Status = false;
            int CreditNoteID = 0;
            try
            {
                CreditNoteID = new CreditNote().Insert(model);
                if (CreditNoteID > 0)
                {
                    Message = Resources.COLPayment.CreditNoteSuccessMsg;
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;

            }
            return Json(new { status = Status, message = Message, ID = CreditNoteID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAvailableCreditBalance(int accountID)
        {
            try
            {
                return Json(new { status = true, Balance = new Payment().GetAvailableCreditBalance(accountID) }, JsonRequestBehavior.AllowGet);
                //PaymentModel model = new Payment().GetAvailableCreditBalance(accountID);
                //return Json(new { status = true, CreditNoteAmount = model.CreditNoteAmount,CreditAmount = model.CreditAmount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsAccountHavePaymentPlan(int accountID)
        {
            try
            {

                return Json(new { status = true, IsAccountPayemntPlan = new AccountPaymentPlan().IsAccountHavePaymentPlan(accountID) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Debit Note
        [HttpGet]
        public ActionResult DebitNote(int PaymentID)
        {
            bool status;
            string msg = "";
            string htmlString = "";

            DebitNoteModel model = new DebitNoteModel();
            model.PaymentID = PaymentID;
            try
            {
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Payment/_DebitNote.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DebitNote(DebitNoteModel model)
        {
            string Message = "";
            bool Status = false;
            int DebitNoteID = 0;
            try
            {
                if (model.Amount <= 0)
                {
                    return Json(new { status = false, message = Resources.ColDebitNote.InvalidDebitAmount }, JsonRequestBehavior.AllowGet);
                }

                DebitNoteID = new DebitNote().Insert(model);
                if (DebitNoteID > 0)
                {
                    Message = Resources.COLPayment.DebitNoteSuccessMsg;
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;

            }
            return Json(new { status = Status, message = Message, ID = DebitNoteID }, JsonRequestBehavior.AllowGet);
        }
        #endregion
      
        #region Import Bank Payments
        [HttpGet]
        public ActionResult ImportBankPayments()
        {
            ImportBankPaymentsFieldModel model = new ImportBankPaymentsFieldModel();
            model.PaymentOptions = new SelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank"), "ID", "Name").OrderBy(x => x.Text);
            return View("~/Areas/Collections/Views/ImportBankPayments/ImportBankPayments.cshtml", model);
        }

        [HttpPost]
        public ActionResult UploadFile(ImportBankPaymentsFieldModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
            }

            if (model.UploadFile == null || model.UploadFile.ContentLength <= 0)
            {
                return Json(new { status = status, message = Resources.Global.EmptyFileMsg }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string filepath, extension;
                extension = System.IO.Path.GetExtension(model.UploadFile.FileName).ToLower();
                if (!Common.ImportTextFileTypes.Contains(extension))
                {
                    return Json(new { status = status, message = string.Format(Resources.ImportAccount.ValFileFormat, string.Join(", ", Common.ImportTextFileTypes)), data = "" }, JsonRequestBehavior.AllowGet);
                }

                //model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString(), extension);
                //model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString() + "_" + DateTime.Now.Date.ToLongDateString(), extension);

                model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), Guid.NewGuid().ToString(), extension);


                //GET THE COMPLETE FOLDER PATH AND STORE THE FILE INSIDE IT.
                string path = Server.MapPath(Common.ImportFilePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                filepath = System.IO.Path.Combine(path, model.FileName);
                model.UploadFile.SaveAs(filepath);

                BankPaymentReadImportFileModel readImportFileModel = ReadImportFile(model.FileName, true);

                if (readImportFileModel != null && readImportFileModel.IsValidFile)
                {
                    BankPaymentsMappingViewModel bankPaymentsMappingModel = new BankPaymentsMappingViewModel(model.FileName, readImportFileModel);
                    ValidBankPaymentsModel validImportmodel = new ValidBankPaymentsModel(readImportFileModel.BankPaymentList, bankPaymentsMappingModel);

                    if (validImportmodel.ImportList != null)
                    {
                        string contract = validImportmodel.ImportList.Rows[0][1].ToString();

                        if
                        (
                             validImportmodel.ImportList.AsEnumerable().Where(x => x.Field<string>("Contract").Trim() == contract.Trim()).Count()
                                    ==
                             validImportmodel.ImportList.Rows.Count
                        )
                        {
                            InvalidBankPaymentsModel _model = new InvalidBankPaymentsModel();
                            _model.Note = readImportFileModel.Note;
                            _model.Date = model.Date;
                            _model.PaymentOptionID = model.PaymentOptionID;
                            _model.FileName = model.FileName;
                            _model.TotalLinesInFileReceived = readImportFileModel.TotalLinesInFileReceived;
                            _model.TotalLinesWithPayments = readImportFileModel.TotalLinesWithPayments;
                            _model.TotalCommission = readImportFileModel.TotalCommission;
                            _model.TotalAmount = readImportFileModel.TotalAmount;
                            _model.IsValid = false;
                            _model.ValidBankPaymentsStatement = new ImportBankPaymentsModel().ImportBankPaymentsValid(validImportmodel);
                            _model.Valid = (_model.ValidBankPaymentsStatement.Rows.Count == 0 || _model.ValidBankPaymentsStatement == null);


                            ////Distinct Record                        
                            //if (_model.ValidBankPaymentsStatement != null && _model.ValidBankPaymentsStatement.Rows.Count>0)
                            //{

                            //    DataView view = new DataView(_model.ValidBankPaymentsStatement);
                            //    _model.ValidBankPaymentsStatement = view.ToTable(true, "ImportID", "PERIODOREC", "PaymentDate", "Contract", "TaxNumber", "Amount", "Reason");
                            //}

                            ////


                            status = true;
                            nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/ImportBankPayments/_ImportValidation.cshtml", _model);
                        }
                        else
                        {
                            status = false;
                            Msg = Resources.COLPayment.SameAgreementMsg;
                        }
                    }
                    else
                    {
                        status = false;
                        Msg = Resources.COLPayment.ImportBankPaymentsFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    Msg = Resources.COLPayment.FileIsNotInCorrectFormatMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, data = htmlString, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportValidation(BankPaymentsMappingViewModel model)
        {
            try
            {
                BankPaymentReadImportFileModel readImportFileModel = ReadImportFile(model.FileName, false);
                BankPaymentsMappingViewModel bankPaymentsMappingModel = new BankPaymentsMappingViewModel(model.FileName, readImportFileModel);

                if (readImportFileModel.BankPaymentList != null)
                {
                    ValidBankPaymentsModel validBankPaymentsModel = new ValidBankPaymentsModel();
                    validBankPaymentsModel.ImportList = readImportFileModel.BankPaymentList;
                    validBankPaymentsModel.IsValid = true;
                    validBankPaymentsModel.CreatedDate = validBankPaymentsModel.ModifiedDate = Common.CurrentDateTime;
                    validBankPaymentsModel.CreatedUser = validBankPaymentsModel.ModifiedUser = UserSession.Current.Id;

                    FinishViewBankPaymentsModel finishViewBankPaymentsModel = new FinishViewBankPaymentsModel(readImportFileModel.BankPaymentList, bankPaymentsMappingModel);
                    finishViewBankPaymentsModel.ImportList = new ImportBankPaymentsModel().ImportBankPaymentsValid(validBankPaymentsModel);
                    status = true;
                    nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/ImportBankPayments/_FinishImport.cshtml", finishViewBankPaymentsModel);
                }
                else
                {
                    status = false;
                    Msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FinishImport(BankPaymentsMappingViewModel model)
        {
            try
            {
                BankPaymentReadImportFileModel readImportFileModel = ReadImportFile(model.FileName, false);

                if (readImportFileModel.BankPaymentList != null)
                {

                    ValidBankPaymentsModel validImportmodel = new ValidBankPaymentsModel(readImportFileModel.BankPaymentList, model);
                    int result = new ImportBankPaymentsModel().ImportBankPaymentsInsert(validImportmodel);

                    if (result > 0)
                    {
                        status = true;
                        Msg = Resources.Global.SavedSuccessfullyMessage;
                        TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;

                        ClearImportFile(model.FileName);
                    }
                    else
                    {
                        status = false;
                        Msg = Resources.COLPayment.ImportBankPaymentsFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    Msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, nextStepData = nextHtmlString, Acctype = 1 }, JsonRequestBehavior.AllowGet);
        }

        #region Common Import
        private BankPaymentReadImportFileModel ReadImportFile(string fileName, bool isFirstTime)
        {
            BankPaymentReadImportFileModel model = new BankPaymentReadImportFileModel();

            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportBankPaymentsModel.S_ImportBankPaymentsData]))))
            {
                model.BankPaymentList = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportBankPaymentsModel.S_ImportBankPaymentsData]));
            }
            else
            {
                //Read File From The Folder
                string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
                string extension = System.IO.Path.GetExtension(path).ToLower();

                model = new ImportModel().GetDataTableFromTxtForBankPayments(path);
                Session[ImportBankPaymentsModel.S_ImportBankPaymentsData] = JsonConvert.SerializeObject(model.BankPaymentList);
            }
            return model;
        }
        private void ClearImportFile(string fileName)
        {
            //REMOVE FILE FROM SESSION
            Session.Remove(ImportBankPaymentsModel.S_ImportBankPaymentsData);

            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        #endregion

        #endregion
    }
}
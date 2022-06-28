using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class ClosingEntryController : BaseController
    {
        #region List Closing Entry
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText)
        {
            JsonResult jResult = new JsonResult();
            DateTime outDate;
            try
            {
                if (DateTime.TryParse(filterText, out outDate))
                {
                    try
                    {
                        filterText = Convert.ToDateTime(filterText).ToString("d", CultureInfo.InvariantCulture);
                    }
                    catch { }
                }
                ClosingEntryListModel list = new ClosingEntry().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.ClosingEntryList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region New Closing Entry
        [HttpGet]
        public ActionResult Add()
        {
            ClosingEntryModel model = new ClosingEntryModel();
            try
            {
                model = new ClosingEntry().Get();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult GetPostedPaymentReceipts(Guid? cashierID, DateTime? closingDate, int? paymentOptionID)
        {
            List<PaymentModel> model = new List<PaymentModel>();
            try
            {
                if (cashierID != null)
                    model = new Payment().GetForClosingEntry(null, null, cashierID, closingDate, true, paymentOptionID);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                html = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/ClosingEntry/_PaymentReceiptList.cshtml", model),
                netClosing = model.Sum(d => d.NetPayment),
                receiptCount = model.Count(),
                totalCash = model.Sum(d => d.TotalCash),
                totalChequedelBancoNacional = model.Sum(d => d.TotalChequedelBancoNacional),
                totalCreditCard = model.Sum(d => d.TotalCreditCard),
                totalBankTransfer = model.Sum(d => d.TotalBankTransfer),
                totalChequedelBancodeCostaRica = model.Sum(d => d.TotalChequedelBancodeCostaRica),
                totalAdjustment = model.Sum(d => d.TotalAdjustment),
                totalBank = model.Sum(d => d.TotalBank),
                totalBankTransferByBancoNacionaldeCostaRica = model.Sum(d => d.TotalBankTransferByBancoNacionaldeCostaRica)
            }
            , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ClosingEntryModel model)
        {
            bool status = false;
            string ErrorMsg = "";
            int closingentryID = 0;
            JsonResult jsonResult = new JsonResult();
            try
            {
                if (model == null)
                {
                    status = false;
                    ErrorMsg = Resources.Global.ValNull;
                    return Json(new { status = status, message = ErrorMsg }, JsonRequestBehavior.AllowGet);
                }

                //Get only selected receipt from the list
                model.PostedPaymentReceiptList = model.PostedPaymentReceiptList.Where(x => x.SelectedReceipt == true).ToList();
                if (model.PostedPaymentReceiptList.Count() <= 0)
                {
                    status = false;
                    ErrorMsg = Resources.COLClosing.ValMinOneReceipt;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }

                if (model.NetClosing <= 0)
                {
                    status = false;
                    ErrorMsg = Resources.COLClosing.ValClosingAmount;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }

                closingentryID = new ClosingEntry().Insert(model);
                if (closingentryID > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage; ;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            jsonResult = Json(new { status = status, message = ErrorMsg, data = "", closingentryID = closingentryID }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region View & Void Closing Entry
        public ActionResult View(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            ClosingEntryPrintModel model = new ClosingEntryPrintModel();
            try
            {
                ViewBag.IsViewMode = true;
                model = new ClosingEntry().GetPrint(ID.Value);
                if (model.ClosingEntry == null || model.ClosingEntry.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Collections/Views/ClosingEntry/Add.cshtml", model.ClosingEntry);
        }

        public ActionResult Print(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            string msg = string.Empty;
            bool status = true;
            try
            {
                ClosingEntryPrintModel model = new ClosingEntry().GetPrint(ID.Value);
                if (model.ClosingEntry == null)
                {
                    status = false;
                    msg = Resources.Global.NoDataMessage;
                }

                ViewBag.CompanyName = new ReportCompanyModel().Get().Name;
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/ClosingEntry/ExportTemplate/_PaymentReceiptTemplate.cshtml", model.ClosingEntry);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Void(ClosingEntryModel model)
        {
            JsonResult jsonResult = new JsonResult();
            try
            {
                new ClosingEntry().Void(model);
                TempData["SuccessMsg"] = Resources.COLClosing.VoidSuccessMsg;
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
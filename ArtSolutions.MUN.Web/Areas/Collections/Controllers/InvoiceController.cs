using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class InvoiceController : BaseController
    {
        #region List Invoice
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText, int? accountID)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                InvoiceListModel list = new InvoiceModel().GetWithPaging(jqdtParams, filterText, accountID);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.InvoiceList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region Add/Edit Invoice
        public ActionResult Add(int? accountID)
        {
            try
            {
                InvoiceModel model = new InvoiceModel();
                model.NewInvoice(accountID);
                return View("AddEdit", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult IsSponsorByAccount(int AccountId)
        {
            bool IsSponspor = false;
            try
            {
                IsSponspor = new AccountModel().IsSponsorByAccount(AccountId, 0) == true;
            }
            catch (Exception ex)
            {

                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = true, IsSponspor = IsSponspor }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServiceTypeOption()
        {
            InvoiceDetailModel model = new InvoiceDetailModel();
            try
            {
                model.InvoiceTypeList = new SelectList(new AccountModel().GetSupportValues(16), "ID", "Name");
                model.ServiceList = new Service().GetForNoFiling();
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Collections/Views/Invoice/_ServiceTypetOptionList.cshtml", model);
        }

        public ActionResult Edit(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                InvoiceModel model = new InvoiceModel();
                model = model.EditInvoice(ID.Value);
                return View("AddEdit", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public JsonResult Save(InvoiceModel model, int actionType)
        {
            bool status = true;
            string ErrorMsg = string.Empty;
            int invoiceId = 0;
            int AccountType = 0;
            try
            {
                var data = new InvoiceModel().Save(model);
                invoiceId = data.ID;
                AccountType = data.AccountTypeID;               
                TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            JsonResult jsonResult = Json(new { status = status, message = ErrorMsg, actionType = actionType, data = "", AccountTypeID= AccountType, InvoiceId = invoiceId }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public JsonResult GetCollectionTemplate()
        {
            try
            {
                List<ServiceCollectionTemplateModel> list = new ServiceCollectionTemplateModel().Get(null, true, null);
                string collectionTemplate = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Invoice/_CollectionTemplateOptionList.cshtml", list);
                return Json(new { status = true, collectiontemlate = collectionTemplate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGrantList(DateTime date, int? AccountId, string FormValue)
        {
            try
            {
                List<FinClassGrantModel> data = new List<FinClassGrantModel>();

                if (AccountId > 0)
                {
                    data = new InvoiceModel().FinGrantDetailList(date, AccountId.Value);
                }

                try
                {
                    List<ServiceCollectionTemplateModel> list = new ServiceCollectionTemplateModel().Get(null, true, null);
                    ViewBag.FormValue = FormValue;
                    string collectionTemplate = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Invoice/_CollectionTemplateOptionList.cshtml", data);
                    return Json(new { status = true, collectiontemlate = collectionTemplate }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCheckBookList(int grantId, string FormValue)
        {
            FinGrantAccountModel model = new FinGrantAccountModel();
            try
            {
                model = new FinGrantAccountModel().Get(grantId, EnumUtility.FINAccountType.AR);
                ViewBag.FormValue = FormValue;
                string collectionTemplate = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Invoice/_CheckbookOptionList.cshtml", model.CheckbookAccountList);
                return Json(new { status = true, collectiontemlate = collectionTemplate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                if (model.CheckbookAccountList.Count() <= 0)
                    Message = ArtSolutions.MUN.Web.Resources.ServiceCollectionTemplate.NotAvailableCheckbook;
                return Json(new { status = false, message = Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetService(string FormValue)
        {
            try
            {
                List<ServiceModel> list = new Service().GetForNoFiling();
                ViewBag.FormValue = FormValue;
                string service = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/Invoice/_ServiceOptionList.cshtml", list);
                return Json(new { status = true, service = service }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region View & Void Invoice
        public ActionResult View(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            InvoicePrintModel model = new InvoicePrintModel();
            try
            {
                model = new InvoiceModel().GetPrint(ID.Value);
                if (model.Invoice.ID <= 0)
                {
                    TempData["ErrorMsg"] = Resources.COLInvoice.InvalidNumber;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Print(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                InvoicePrintModel model = new InvoiceModel().GetPrint(ID.Value);
                if (model.Invoice.ID <= 0)
                    TempData["ErrorMsg"] = Resources.Global.NoDataMessage;
                else
                    return View("~/Areas/Collections/Views/Invoice/ExportTemplate/_InvoiceTemplate.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToAction("View", new { ID = ID });
        }

        [HttpPost]
        public ActionResult Void(InvoiceModel model)
        {
            try
            {
                new InvoiceModel().Void(model);
                TempData["SuccessMsg"] = Resources.COLInvoice.VoidSuccessMsg;
                return Json(new { status = true, message = Resources.COLInvoice.VoidSuccessMsg, ID = model.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
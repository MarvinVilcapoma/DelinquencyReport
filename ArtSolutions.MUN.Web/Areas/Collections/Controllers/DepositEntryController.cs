using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class DepositEntryController : BaseController
    {
        #region List Deposit Entry
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
                DepositEntryListModel list = new DepositEntry().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.DepositEntryList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region Add Deposit Entry
        [HttpGet]
        public ActionResult Add()
        {
            DepositEntryModel model = new DepositEntryModel();
            try
            {
                model = new DepositEntry().Get();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DepositEntryModel model)
        {
            bool status = false;
            string ErrorMsg = "";
            int depositentryID = 0;
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
                model.ClosedEntryList = model.ClosedEntryList.Where(x => x.SelectedClosedEntry == true).ToList();
                if (model.ClosedEntryList.Count() <= 0)
                {
                    status = false;
                    ErrorMsg = Resources.COLDeposit.ValMinOneCloseEntry;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }

                if (model.NetDeposit <= 0)
                {
                    status = false;
                    ErrorMsg = Resources.COLDeposit.ValDepositAmount;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }

                depositentryID = new DepositEntry().Insert(model);
                if (depositentryID > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            jsonResult = Json(new { status = status, message = ErrorMsg, data = "", depositentryID = depositentryID }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region View Deposit Entry
        public ActionResult View(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            DepositEntryPrintModel model = new DepositEntryPrintModel();
            try
            {
                ViewBag.IsViewMode = true;
                model = new DepositEntry().GetPrint(ID.Value);
                if (model.DepositEntry == null || model.DepositEntry.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Collections/Views/DepositEntry/Add.cshtml", model.DepositEntry);
        }
        public ActionResult Print(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            string msg = string.Empty;
            bool status = true;
            try
            {
                DepositEntryPrintModel model = new DepositEntry().GetPrint(ID.Value);
                if (model.DepositEntry == null)
                {
                    status = false;
                    msg = Resources.Global.NoDataMessage;
                }

                ViewBag.CompanyName = new ReportCompanyModel().Get().Name;
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/DepositEntry/ExportTemplate/_ClosingEntryTemplate.cshtml", model.DepositEntry);

                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Void(int ID, String VoidReason)
        {
            string msg = string.Empty;
            bool status = true;
            try
            {
                DepositEntryPrintModel depositEntryPrintModel = new DepositEntry().GetPrint(ID);
                depositEntryPrintModel.DepositEntry.VoidReason = VoidReason;
                new DepositEntry().Void(depositEntryPrintModel.DepositEntry);
                TempData["SuccessMsg"] = Resources.COLDeposit.VoidSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
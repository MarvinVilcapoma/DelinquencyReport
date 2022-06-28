using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Reports.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Controllers
{
    [Filters.IsCompanyConfigured]
    public class AccountPaymentPlanController : BaseController
    {
        #region List Account Payment Plan

        public ActionResult List()
        {
            return View("~/Areas/Accounts/Views/AccountPaymentPlan/List.cshtml");
        }

        [HttpPost]
        public JsonResult List(JQDTParams jqdtParams, int? accountID, int? accountPropertyID, string filterText = null)
        {
            JsonResult jResult = new JsonResult();
            decimal outDecimal;
            DateTime outDate;
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

                AccountPaymentPlanListModel list = new AccountPaymentPlanListModel().GetByPaging(filterText, accountID, accountPropertyID, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir));
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.AccountPaymentPlanList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public JsonResult ActiveDeactive(bool isActive, int id)
        {
            bool status = false;
            string msg = string.Empty;

            try
            {
                int ID = new Accounts.Models.AccountPaymentPlan().UpdateStatus(isActive, id);
                if (ID > 0)
                {
                    status = true;
                    msg = isActive ? Global.ActiveMessage : Global.DeActiveMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Account Payment Plan 

        public ActionResult Add(int? accountPaymentPlanID)
        {
            AccountPaymentPlanModel model = new AccountPaymentPlanModel();
            try
            {
                if (accountPaymentPlanID > 0)
                    ViewBag.ActionType = 2;
                else
                    ViewBag.ActionType = 1;

                model = new Accounts.Models.AccountPaymentPlan().Get(accountPaymentPlanID);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/AccountPaymentPlan/Add.cshtml", model);
        }

        [HttpPost]
        public ActionResult Add(AccountPaymentPlanModel model)
        {
            bool status = false;
            string msg = string.Empty;
            int result = 0;

            try
            {
                ViewBag.ActionType = 1;

                if (model.ID > 0)
                {
                    //if (string.IsNullOrEmpty(model.AccountServiceCollectionDetailIDs) && string.IsNullOrEmpty(model.AccountPaymentPlanDetailJson))
                    if (string.IsNullOrEmpty(model.AccountServiceCollectionDetailIDs) || string.IsNullOrEmpty(model.AccountPaymentPlanDetailJson))
                    {
                        AccountPaymentPlanModel summaryModel = new Models.AccountPaymentPlan().GetPaymentPlanSummary
                        (
                                model.ServicePaymentPlanID, model.TotalDebt, model.StartDate, model.InstalmentPercentage,
                                model.InterestPercentage, model.LateInterestPercentage, model.Months, model.AccountID,
                                model.AccountPaymentPlanServiceIDs, model.AccountPaymentPlanServiceCollectionDetailIDs,
                                true, model.ApplybyAmnesty
                        );
                        model.MonthlyInterest = summaryModel.MonthlyInterest;
                        model.AccountPaymentPlanDetailJson = summaryModel.AccountPaymentPlanDetailJson;
                        model.AccountServiceCollectionDetailIDs = model.AccountPaymentPlanServiceCollectionDetailIDs;
                    }
                    result = new Accounts.Models.AccountPaymentPlan().Update(model);
                }
                else
                    result = new Accounts.Models.AccountPaymentPlan().Insert(model);

                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult View(int? accountPaymentPlanID)
        {
            if (accountPaymentPlanID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            AccountPaymentPlanModel model = new AccountPaymentPlanModel();
            try
            {
                ViewBag.ActionType = 3;
                model = new Accounts.Models.AccountPaymentPlan().Get(accountPaymentPlanID);
                if (model.ID == 0)
                {
                    TempData["ErrorMsg"] = Resources.AccountPaymentPlan.InvalidAccountPaymentPlan;
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/AccountPaymentPlan/View.cshtml", model);
        }

        public JsonResult Delete(int id, string Reason, bool? isWindowReload = null)
        {
            bool status = false;
            string msg = string.Empty;
            try
            {
                new Accounts.Models.AccountPaymentPlan().Delete(id, Reason);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.AccountService.DeleteAccountServiceSuccMsg;

                if (isWindowReload == true)
                    TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                TempData["ErrorMsg"] = msg;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAccountServiceCollectionDetail(int? actionType, int? accountId, string selectedItemIds, bool applybyAmnesty, bool forEdit)
        {
            bool status = false;
            string Msg = string.Empty, htmlString = string.Empty;

            List<AccountServiceCollectionDetailModel> model = new List<AccountServiceCollectionDetailModel>();
            try
            {
                if (accountId > 0)
                {
                    ViewBag.ActionType = actionType;

                    if (actionType == 1) // Add
                        model = new AccountServiceCollectionDetailModel().GetNotPaidSummary(accountId.Value, null, false, applybyAmnesty).ToList();
                    else // Edit
                        model = new AccountServiceCollectionDetailModel().GetNotPaidSummary(accountId.Value, selectedItemIds, forEdit, applybyAmnesty).ToList();
                    
                    List<int> CollectionID = new List<int>();
                    if (!string.IsNullOrEmpty(selectedItemIds))
                        CollectionID = selectedItemIds.Split(',').Select(int.Parse).ToList();
                    model.ForEach(x =>
                    {
                        if
                        (
                            CollectionID.Where(d => d == x.ID).Count() > 0
                                ||
                            actionType == 2
                         )
                        {
                            x.SelectedItem = true;
                        }
                    });
                }

                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountPaymentPlan/_AccountServiceCollectionDetailList.cshtml", model);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, data = htmlString, accountServiceIDs = string.Join(",", model.Distinct().Select(i => i.AccountServiceID).Distinct()) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAccountPaymentPlanDetail(int paymentPlanId, decimal totalDebt, DateTime startDate, decimal InstalmentPercentage, decimal InterestPercentage, decimal LateInterestPercentage, int Months, int accountId, string accountServiceIds, string ServiceCollectionDetailIDs, bool forEdit, bool applybyAmnesty)
        {
            bool status = false;
            string Msg = string.Empty, htmlString = string.Empty;

            try
            {
                if (totalDebt > 0)
                {
                    htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountPaymentPlan/_AccountPaymentPlanDetail.cshtml", new Accounts.Models.AccountPaymentPlan().GetPaymentPlanSummary(paymentPlanId, totalDebt, startDate, InstalmentPercentage, InterestPercentage, LateInterestPercentage, Months, accountId, accountServiceIds, ServiceCollectionDetailIDs, forEdit, applybyAmnesty));
                    status = true;
                }
                else
                {
                    status = false;
                    Msg = Resources.COLPayment.ValTotalAmountRequired;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, data = htmlString }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print
        [HttpGet]
        public ActionResult Print(int accountID, int AccountPaymentPlanID)
        {
            AccountPaymentPlanPrintModel model = new AccountPaymentPlanPrintModel();

            try
            {
                model = new AccountPaymentPlanPrint().Print(accountID, AccountPaymentPlanID);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/AccountPaymentPlan/Print.cshtml", model);
        }
        #endregion

        #region Ajax Calls
        public JsonResult GetServiceType(int accountID)
        {
            return Json(HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetNotPaid(accountID, false), "ID", "Name", null, true, true, Global.DropDownSelectMessage), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPaymentPlan(DateTime startDate)
        {
            return Json(HMTLHelperExtensions.CreateSelectList(new Services.Models.PaymentPlan().Get(null, startDate, true), "ID", "Name", null, true, true, Global.DropDownSelectMessage), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalDebtForAccountPaymentPlan(decimal totalDebt)
        {
            return Json(totalDebt.ToString("C"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountPaymentPlan(int paymentPlanId)
        {
            bool status = false;
            string Msg = string.Empty, htmlString = string.Empty;
            PaymentPlanModel model = new PaymentPlanModel();
            try
            {
                if (paymentPlanId > 0)
                {
                    model = new Accounts.Models.AccountPaymentPlan().GetPaymentPlan(paymentPlanId);
                    status = true;
                }
                else
                {
                    status = false;
                    Msg = Resources.COLPayment.ValTotalAmountRequired;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, data = model }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountPaymentPlanByAccountID(int accountID)
        {
            //List<SelectListItem> resultList = HMTLHelperExtensions.CreateSelectList(new Models.AccountPaymentPlan().Get(null, accountID, true), "ID", "PaymentPlanNameWithNumber", null, true, true, Resources.Global.DropDownSelectMessage);
            List<SelectListItem> resultList = HMTLHelperExtensions.CreateSelectList(new Models.AccountPaymentPlan().Get(null, accountID, true), "ID", "PaymentPlanNameWithNumber");
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export
        [HttpGet]
        public ActionResult PaymentPlanExport(int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountPaymentPlan/Export/_PaymentPlanListExport.cshtml",
                new AccountPaymentPlanListModel().GetExportPaymentPlan());
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.AccountPaymentPlan.ListAccountPaymentPlan, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion
    }
}
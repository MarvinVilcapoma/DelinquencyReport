using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Controllers
{
    [Filters.IsCompanyConfigured]
    public class AccountServiceController : BaseController
    {
        AccountServiceModel Model = new AccountServiceModel();
        bool status = false;
        string msg = string.Empty, htmlString = string.Empty, nextHtmlString = string.Empty;

        string[] ServiceCode = { "01", "02", "03", "04", "05" };
        string[] arrNumbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        int[] arrValidateErrorCode = new int[] { 50350, 50351, 50352, 50359, 50354, 50398, 50353, 50361 };
        int[] arrDBErrorCode = new int[] { 50293, 50288, 50289, 50362, 50372, 50154, 50371 };

        #region List Account Service 
        public ActionResult List()
        {
            AccountServiceList list = new AccountServiceList();
            try
            {
                list = list.Get();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/AccountService/List.cshtml", list);
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText, string ServiceTypeIDs, int? accountId, int? accountPropertyID, string serviceIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                AccountServiceList list = new AccountServiceList().Get(filterText, ServiceTypeIDs, accountId, accountPropertyID, serviceIDs, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir));
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.AccountServiceModelList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public JsonResult ActiveDeactive(bool isActive, int id, string Reason)
        {
            try
            {
                Model.ID = Model.Update(isActive, id, Reason).ID;
                if (Model.ID > 0)
                    status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id, string Reason, bool? isWindowReload = null)
        {
            try
            {
                Model.Delete(id, Reason);
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

        public JsonResult DeleteNote(int id, string Reason, bool IsAccountServiceNote, bool? isWindowReload = null)
        {
            try
            {
                Model.DeleteNote(id, Reason, IsAccountServiceNote);
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

        public ActionResult View(int? accountServiceID)
        {
            if (accountServiceID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                status = true;
                Model = Model.Get(0, 0, 0, accountServiceID.Value, null, null);

                if (Model.ID == 0)
                {
                    status = false;
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.InvalidAccountService;
                }
                else
                {
                    try
                    {
                        Model.HasPrintTemplate = new Service().Get(null, null, Model.ServiceID, false, null).FirstOrDefault().PrintTemplateID == null ? false : true;
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            if (status)
                return View("~/Areas/Accounts/Views/AccountService/View.cshtml", Model);
            else
                return RedirectToAction("List");
        }

        public JsonResult GetInitialPlan(int accountID, int accountServiceID)
        {
            return Json(new AccountServicePaymentPlanModel().Get(accountID, null, accountServiceID).OrderBy(x => x.AccountPaymentPlanID).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region New Account Service 

        [HttpGet]
        public ActionResult Add(int? accountId, int? accountTypeId, int? serviceType, int? ownerID, int? accountPropertyID, int? isFromRight)
        {
            if (accountId == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            AccountServiceModel model = new AccountServiceModel();
            try
            {
                ViewBag.IsFromRight = isFromRight;
                model = model.Get(accountId.Value, accountTypeId.Value, serviceType.Value, 0, ownerID, accountPropertyID);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/AccountService/Add.cshtml", model);
        }

        [HttpPost]
        public ActionResult Add(AccountServiceModel model)
        {
            int id = 0;
            DateTime outDate;

            try
            {
                DateTime dateRangeFrom = new DateTime(1753, 1, 1, 12, 00, 00);
                DateTime dateToFrom = new DateTime(9999, 12, 31, 11, 59, 59);

                bool isValidDateTime = false;
                if (DateTime.TryParse(Request["hfCustomDateField"], out outDate))
                    isValidDateTime = true;

                DateTime hfCustomDateField = new DateTime();
                if (isValidDateTime)
                    hfCustomDateField = DateTime.Parse(Request["hfCustomDateField"]);

                bool isValidDateTime1 = true;
                if (model.IsDateFieldCustomField1 == true)
                {
                    if (!string.IsNullOrEmpty(model.CustomField1))
                    {
                        isValidDateTime1 = false;
                        if (DateTime.TryParse(model.CustomField1, out outDate))
                            isValidDateTime1 = true;

                        DateTime hfCustomDateField1 = new DateTime();
                        if (isValidDateTime1)
                        {
                            hfCustomDateField1 = DateTime.Parse(model.CustomField1);
                            if (hfCustomDateField1 >= dateRangeFrom && hfCustomDateField1 <= dateToFrom)
                                isValidDateTime1 = true;
                            else
                                isValidDateTime1 = false;
                        }
                    }
                }

                if ((
                        string.IsNullOrEmpty(Request["hfCustomDateField"])
                            ||
                        ((isValidDateTime == true) && (hfCustomDateField >= dateRangeFrom && hfCustomDateField <= dateToFrom))
                 ) && isValidDateTime1 == true)
                {
                    id = model.Insert(model);
                    if (id > 0)
                    {
                        status = true;
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                    }
                }
                else
                {
                    status = false;
                    msg = ArtSolutions.MUN.Web.Resources.Global.WrongDateValidationMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, id = id }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Service Support Methods

        public PartialViewResult GetServiceType(int groupId)
        {
            return PartialView("~/Areas/Accounts/Views/AccountService/_ServiceType.cshtml", new ServiceTypeModel().GetServiceType(groupId));
        }

        public PartialViewResult GetService(int serviceTypeId, int groupId, int AccountTypeID)
        {
            return PartialView("~/Areas/Accounts/Views/AccountService/_Service.cshtml", new Service().GetForFlexibleDatesAndFixDates(serviceTypeId, groupId, null, true, AccountTypeID));
        }

        public PartialViewResult GetSummary()
        {
            return PartialView("~/Areas/Accounts/Views/AccountService/_Summary.cshtml");
        }

        public JsonResult GetAdditionalInfo(int serviceID, int fiscalyear, DateTime initialDate, DateTime endDate, int? accountID, int? serviceGroupID)
        {
            string jsonResult = null;

            fiscalyear = initialDate.Month == 1 && endDate.Month == 12 ? fiscalyear : fiscalyear - 1;
            DateTime iniDate = new DateTime(fiscalyear, initialDate.Month, initialDate.Day);

            if (serviceID > 0 && fiscalyear > 0 && accountID > 0 && serviceGroupID == 1)
            {
                List<AccountServiceModel> accServiceList = new AccountServiceModel().Get(0, null, accountID, fiscalyear - 1, null).Where(x => x.IsActive == true && x.IsDeleted == false && x.ServiceID == serviceID).ToList();

                if (accServiceList != null && accServiceList.Any() && accServiceList.Count == 1)
                {
                    AccountServiceModel accServiceModel = accServiceList.FirstOrDefault();

                    var customDateField = accServiceModel.CustomDateField == null ? null : accServiceModel.CustomDateField.Value.ToString("d");

                    jsonResult = "{\"InitialDate\": \"" + iniDate.ToString("d") + "\"," +
                              "\"EndDate\": \"" + iniDate.AddYears(1).AddDays(-1).ToString("d") + "\"," +
                              "\"CustomField1\": \"" + accServiceModel.CustomField1 + "\"," +
                              "\"CustomField2\": \"" + accServiceModel.CustomField2 + "\"," +
                              "\"CustomField3\": \"" + accServiceModel.CustomField3 + "\"," +
                              "\"CustomField4\": \"" + accServiceModel.CustomField4 + "\"," +
                              "\"CustomField5\": \"" + accServiceModel.CustomField5 + "\"," +
                              "\"CustomDateField\": \"" + customDateField + "\"}";
                }
                else
                {
                    jsonResult = "{\"InitialDate\": \"" + iniDate.ToString("d") + "\"," +
                          "\"EndDate\": \"" + iniDate.AddYears(1).AddDays(-1).ToString("d") + "\"}";
                }
            }
            else
            {
                jsonResult = "{\"InitialDate\": \"" + iniDate.ToString("d") + "\"," +
                              "\"EndDate\": \"" + iniDate.AddYears(1).AddDays(-1).ToString("d") + "\"}";
            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFiscalYear(int serviceID, int accountID)
        {
            IEnumerable<SelectListItem> list = new FiscalYearModel().GetFiscalYearByServiceNotInAccount(serviceID, accountID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceException(int serviceID)
        {
            IEnumerable<SelectListItem> resultList = HMTLHelperExtensions.CreateSelectList(new ServiceExceptionModel().Get(null, serviceID), "ID", "FormattedExceptionValue", null, true, true, Resources.Global.DropDownSelectMessage);
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountPropertyRightByOwner(int ownerID, int serviceID, int fiscalYearID, int? id, bool? forAccountStatementReport, bool? IsTransferByRight)
        {
            List<SelectListItem> resultList = new List<SelectListItem>();
            if (forAccountStatementReport == true)
            {
                List<PropertyModel> list = new List<PropertyModel>();
                list.Add(new PropertyModel() { Property = Resources.Global.DropDownSelectMessage, AccountPropertyID = 0 });
                list.Add(new PropertyModel() { Property = Resources.Global.All, AccountPropertyID = -1 });
                list.Add(new PropertyModel() { Property = Resources.Report.Unassigned, AccountPropertyID = -2 });
                list.AddRange(new AccountProperty().GetAccountPropertyRightByOwner(ownerID, serviceID, fiscalYearID, id));
                resultList = HMTLHelperExtensions.CreateSelectList(list, "AccountPropertyID", "Property");
            }
            else
                resultList = HMTLHelperExtensions.CreateSelectList(new AccountProperty().GetAccountPropertyRightByOwner(ownerID, serviceID, fiscalYearID, id, IsTransferByRight), "AccountPropertyID", "Property", null, true, true, Resources.Global.DropDownSelectMessage);
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountServiceForTimbre(int fiscalYearID, int accountID, int ServiceID, int? accountPropertyID)
        {
            var list = new AccountServiceList().GetAccountServiceForTimbre(fiscalYearID, accountID, ServiceID, accountPropertyID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Filing Get

        public ActionResult CollectionDebtGet(int accountServiceId, int accountServiceCollectionDetailId)
        {
            try
            {
                List<AccountServiceCollectionDebtModel> list = new AccountServiceCollectionDebtModel().Get(accountServiceId, accountServiceCollectionDetailId, null, null);
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_CollectionDebt.cshtml", list);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CollectionDiscountGet(int accountServiceId, int accountServiceCollectionDetailId, int FilingFormID)
        {
            try
            {
                List<AccountServiceCollectionDiscountModel> list = new List<AccountServiceCollectionDiscountModel>();
                if (FilingFormID != (int)EnFilingForm.PropertyTax)
                {
                    list = new AccountServiceCollectionDiscountModel().Get(accountServiceId, accountServiceCollectionDetailId);
                }

                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_CollectionDiscount.cshtml", list);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Filling(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingLicenseModel fillingModel = new AccountServiceCollectionFillingLicenseModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditFilling(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingLicenseModel fillingModel = new AccountServiceCollectionFillingLicenseModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Filing License

        [HttpPost]
        public ActionResult FillingLicense(AccountServiceFillingModel collectionDetail, int filingFormID, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            if (isViewMode)
            {
                ViewBag.IsViewMode = true;
                collectionDetail.CollectionFillingModel = new AccountServiceCollectionFillingLicenseModel().Get(collectionDetail.AccountServiceCollectionDetailId, filingFormID, 1);
                collectionDetail.FillingDate = collectionDetail.CollectionFillingModel.FillingDate;
                collectionDetail.ReFillingDate = collectionDetail.CollectionFillingModel.ReFillingDate;
            }
            return View("~/Areas/Accounts/Views/AccountService/_FillingLicense.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult EditFillingLicense(AccountServiceFillingModel collectionDetail, int filingFormID, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;

            collectionDetail.CollectionFillingModel = new AccountServiceCollectionFillingLicenseModel().Get(collectionDetail.AccountServiceCollectionDetailId, filingFormID, 1);
            collectionDetail.FillingDate = collectionDetail.CollectionFillingModel.FillingDate;

            return View("~/Areas/Accounts/Views/AccountService/_EditFillingLicense.cshtml", collectionDetail);
        }

        public JsonResult DeleteFillingLicense(int ID, string Reason)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceCollectionFillingLicenseModel model = new AccountServiceCollectionFillingLicenseModel();
                    model.Delete(ID, Reason);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.DeleteFillingSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFillingLicenseRule(int AccountServiceCollectionDetailID)
        {
            AccountServiceCollectionFillingUnitModel fillingModel = new AccountServiceCollectionFillingUnitModel();
            decimal Percentage = 0;
            try
            {
                Percentage = fillingModel.GetFillingUnitRule(AccountServiceCollectionDetailID);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, RuleValue = Percentage }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Filing Unit

        [HttpPost]
        public ActionResult FillingUnit(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            if (isViewMode)
            {
                ViewBag.IsViewMode = true;
                collectionDetail.CollectionFillingUnitModel = new AccountServiceCollectionFillingUnitModel().Get(collectionDetail.AccountServiceCollectionDetailId, 1);
                collectionDetail.FillingDate = collectionDetail.CollectionFillingUnitModel.FillingDate;
                collectionDetail.ReFillingDate = collectionDetail.CollectionFillingUnitModel.ReFillingDate;
            }
            return View("~/Areas/Accounts/Views/AccountService/_FillingUnit.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult EditFillingUnit(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;

            collectionDetail.CollectionFillingUnitModel = new AccountServiceCollectionFillingUnitModel().Get(collectionDetail.AccountServiceCollectionDetailId, 1);
            collectionDetail.FillingDate = collectionDetail.CollectionFillingUnitModel.FillingDate;

            return View("~/Areas/Accounts/Views/AccountService/_EditFillingUnit.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult SaveFillingUnit(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingUnitModel fillingModel = new AccountServiceCollectionFillingUnitModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFillingUnitRule(int AccountServiceCollectionDetailID)
        {
            AccountServiceCollectionFillingUnitModel fillingModel = new AccountServiceCollectionFillingUnitModel();
            decimal Amount = 0;
            try
            {
                Amount = fillingModel.GetFillingUnitRule(AccountServiceCollectionDetailID);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, RuleValue = Amount.ToString(Common.DecimalPoints) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveFillingUnitEdit(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingUnitModel fillingModel = new AccountServiceCollectionFillingUnitModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFillingUnit(int ID, string Reason)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceCollectionFillingUnitModel model = new AccountServiceCollectionFillingUnitModel();
                    model.Delete(ID, Reason);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.DeleteFillingSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Filing Auto Filing

        [HttpPost]
        public ActionResult AutoFilling(AccountServiceFillingModel collectionDetail, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            string ServiceName = collectionDetail.ServiceName;
            string ServicePeriod = collectionDetail.ServicePeriod;
            collectionDetail = new AccountServiceFillingModel().GetAutoFiling(collectionDetail.AccountServiceCollectionDetailId);
            collectionDetail.ServiceName = ServiceName;
            collectionDetail.ServicePeriod = ServicePeriod;
            return View("~/Areas/Accounts/Views/AccountService/_AutoFilling.cshtml", collectionDetail);
        }

        public JsonResult DeleteFillingAutoFiling(int ID, string Reason)
        {
            try
            {
                if (ID > 0)
                {
                    AccountServiceFillingModel model = new AccountServiceFillingModel();
                    model.DeleteFillingAutoFiling(ID, Reason);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.DeleteFillingSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Filing Measured Water

        [HttpPost]
        public ActionResult FillingMeasuredWater(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            if (isViewMode)
            {
                ViewBag.IsViewMode = true;
                collectionDetail.CollectionFillingMeasuredWaterModel = new AccountServiceCollectionFillingMeasuredWaterModel().Get(collectionDetail.AccountServiceCollectionDetailId);
                collectionDetail.FillingDate = collectionDetail.CollectionFillingMeasuredWaterModel.FillingDate;
                collectionDetail.ReFillingDate = collectionDetail.CollectionFillingMeasuredWaterModel.ReFillingDate;
            }
            else
            {
                decimal PreviosMeasureWater = new AccountServiceCollectionFillingMeasuredWaterModel().PreviousMeasureGet(collectionDetail.AccountServiceCollectionDetailId);
                collectionDetail.CollectionFillingMeasuredWaterModel = new AccountServiceCollectionFillingMeasuredWaterModel();
                collectionDetail.CollectionFillingMeasuredWaterModel.PreviousMeasure = PreviosMeasureWater;

            }
            return View("~/Areas/Accounts/Views/AccountService/_FillingMeasuredWater.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult EditFillingMeasuredWater(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            collectionDetail.CollectionFillingMeasuredWaterModel = new AccountServiceCollectionFillingMeasuredWaterModel().Get(collectionDetail.AccountServiceCollectionDetailId);
            collectionDetail.FillingDate = collectionDetail.CollectionFillingMeasuredWaterModel.FillingDate;
            return View("~/Areas/Accounts/Views/AccountService/_EditFillingMeasuredWater.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult SaveFillingMeasuredWater(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingMeasuredWaterModel fillingModel = new AccountServiceCollectionFillingMeasuredWaterModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveFillingMeasuredWaterEdit(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingMeasuredWaterModel fillingModel = new AccountServiceCollectionFillingMeasuredWaterModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFillingMeasuredWater(int ID, string Reason)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceCollectionFillingMeasuredWaterModel model = new AccountServiceCollectionFillingMeasuredWaterModel();
                    model.Delete(ID, Reason);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.DeleteFillingSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Filing Property Tax

        [HttpPost]
        public ActionResult FillingPropertyTax(int AccountPropertyID, AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            if (isViewMode)
            {
                ViewBag.IsViewMode = true;
                collectionDetail.CollectionFillingPropertyTaxModel = new AccountServiceCollectionFillingPropertyTaxModel().Get(collectionDetail.AccountServiceCollectionDetailId);
                collectionDetail.FillingDate = collectionDetail.CollectionFillingPropertyTaxModel.FillingDate;
                collectionDetail.ReFillingDate = collectionDetail.CollectionFillingPropertyTaxModel.ReFillingDate;
            }
            else
            {
                collectionDetail.CollectionFillingPropertyTaxModel.MovementTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(39), "ID", "Name");
            }
            collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList = new AccountProperty().GetForFilling(AccountPropertyID, collectionDetail.AccountServiceCollectionDetailId);
            collectionDetail.CollectionFillingPropertyTaxModel.Area = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().Area;
            collectionDetail.CollectionFillingPropertyTaxModel.TerrainValue = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().TerrainValue;
            collectionDetail.CollectionFillingPropertyTaxModel.PropertyAccountID = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().ID;
            collectionDetail.CollectionFillingPropertyTaxModel.PropertyNumber = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().PropertyNumber;
            collectionDetail.CollectionFillingPropertyTaxModel.RigthNumber = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().RigthNumber;

            return View("~/Areas/Accounts/Views/AccountService/_FillingPropertyTax.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult EditFillingPropertyTax(int AccountPropertyID, AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            collectionDetail.CollectionFillingPropertyTaxModel = new AccountServiceCollectionFillingPropertyTaxModel().Get(collectionDetail.AccountServiceCollectionDetailId);
            collectionDetail.FillingDate = collectionDetail.CollectionFillingPropertyTaxModel.FillingDate;
            collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList = new AccountProperty().GetForFilling(AccountPropertyID, collectionDetail.AccountServiceCollectionDetailId);
            collectionDetail.CollectionFillingPropertyTaxModel.Area = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().Area;
            collectionDetail.CollectionFillingPropertyTaxModel.TerrainValue = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().TerrainValue;
            collectionDetail.CollectionFillingPropertyTaxModel.PropertyAccountID = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().ID;
            collectionDetail.CollectionFillingPropertyTaxModel.PropertyNumber = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().PropertyNumber;
            collectionDetail.CollectionFillingPropertyTaxModel.RigthNumber = collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().RigthNumber;
            return View("~/Areas/Accounts/Views/AccountService/_EditFillingPropertyTax.cshtml", collectionDetail);
        }


        [HttpPost]
        public ActionResult SaveFillingPropertyTax(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingPropertyTaxModel fillingModel = new AccountServiceCollectionFillingPropertyTaxModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveFillingPropertyTaxEdit(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingPropertyTaxModel fillingModel = new AccountServiceCollectionFillingPropertyTaxModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFillingPropertyTax(int ID, string Reason)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceCollectionFillingPropertyTaxModel model = new AccountServiceCollectionFillingPropertyTaxModel();
                    model.Delete(ID, Reason);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.DeleteFillingSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Filing IVU Tax

        [HttpPost]
        public ActionResult FillingTax(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;
            if (isViewMode)
            {
                ViewBag.IsViewMode = true;
                collectionDetail.CollectionFillingTaxModel = new AccountServiceCollectionFillingTaxModel().Get(collectionDetail.AccountServiceCollectionDetailId, 2);
                collectionDetail.FillingDate = collectionDetail.CollectionFillingTaxModel.FillingDate;
                collectionDetail.ReFillingDate = collectionDetail.CollectionFillingTaxModel.ReFillingDate;
            }
            return View("~/Areas/Accounts/Views/AccountService/_FillingTax.cshtml", collectionDetail);
        }
        [HttpPost]
        public ActionResult EditFillingTax(AccountServiceFillingModel collectionDetail, bool isViewMode, bool? isSummary)
        {
            ViewBag.IsSummary = isSummary;

            ViewBag.IsViewMode = true;
            collectionDetail.CollectionFillingTaxModel = new AccountServiceCollectionFillingTaxModel().Get(collectionDetail.AccountServiceCollectionDetailId, 2);
            collectionDetail.FillingDate = collectionDetail.CollectionFillingTaxModel.FillingDate;

            return View("~/Areas/Accounts/Views/AccountService/_EditFillingTax.cshtml", collectionDetail);
        }

        [HttpPost]
        public ActionResult SaveFillingTax(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingTaxModel fillingModel = new AccountServiceCollectionFillingTaxModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveFillingTaxEdit(AccountServiceFillingModel model)
        {
            string summaryHtmlString = string.Empty, discountInfoString = string.Empty;
            AccountServiceCollectionFillingTaxModel fillingModel = new AccountServiceCollectionFillingTaxModel();

            try
            {
                fillingModel = fillingModel.Filling(model);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString, returnSummaryData = summaryHtmlString, accountServiceId = fillingModel.AccountServiceID, discountInfoMessage = discountInfoString }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Payment       
        public ActionResult CollectionPaymentHistoryGet(int accountServiceId, int accountServiceCollectionDetailId)
        {
            try
            {
                List<AccountServiceCollectionPaymentHistoryModel> list = new AccountServiceCollectionPaymentHistoryModel().Get(accountServiceId, accountServiceCollectionDetailId, null);
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_CollectionPaymentHistory.cshtml", list);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Notes
        public JsonResult AddNote(AccountServiceNoteModel model)
        {
            try
            {
                model.Insert();
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_NoteList.cshtml", new AccountServiceNoteModel().Get(model.AccountServiceID, null));
                msg = ArtSolutions.MUN.Web.Resources.AccountService.AddNoteSuccessMessage;
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Suspension Order        
        [HttpGet]
        public ActionResult SuspensionOrder(int id)
        {
            try
            {
                AccountServicePrintModel accountServicePrintModel = new AccountServicePrintModel();
                accountServicePrintModel = accountServicePrintModel.GetPrintModel(id, 4);
                return View(accountServicePrintModel.PrintTemplate);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("View");
        }
        #endregion

        #region Issue        
        [HttpPost]
        public JsonResult Issue(int id, string rowVersion64)
        {
            try
            {
                status = new AccountServiceModel().Issue(id, Convert.FromBase64String(rowVersion64));
                if (status)
                {
                    // Get Print Template
                    AccountServicePrintModel model = new AccountServicePrintModel().GetPrintModel(id, null);
                    htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_PrintTemplatePopup.cshtml", model);
                    msg = ArtSolutions.MUN.Web.Resources.AccountService.IssueSuccessMsg;
                }
                else
                    msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, htmlContent = htmlString }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print - RePrint

        [HttpGet]
        public ActionResult RePrint(int id, string rowVersion64, bool isPrinted, int? serviceTypeID)
        {
            try
            {
                ViewBag.ServiceTypeID = serviceTypeID;

                AccountServicePrintModel accountServicePrintModel = new AccountServicePrintModel();
                if (!isPrinted)
                {
                    accountServicePrintModel.Print(id, Convert.FromBase64String(rowVersion64));
                }
                status = accountServicePrintModel.RePrintLog(id);
                if (status)
                {
                    accountServicePrintModel = accountServicePrintModel.GetPrintModel(id, null);
                    return View(accountServicePrintModel.PrintTemplate);
                }
                else
                    TempData["ErrorMsg"] = Resources.Global.GeneralErrorMessage;
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("View");
        }
        #endregion

        #region Lock
        public JsonResult LockService(AccountServiceModel model)
        {
            try
            {
                model.IsLock = !model.IsLock;
                status = new AccountServiceModel().Lock(model);
                if (status)
                {
                    if (model.IsLock)
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.LockSuccessMsg;
                    else
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.UnlockSuccessMsg;
                }
                else
                    throw new Exception(ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, isLock = model.IsLock }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Adjustment
        public ActionResult LoadAdjustmentPopup(int accountServiceID, int serviceID)
        {
            try
            {
                AccountServiceAdjustmentModel model = new AccountServiceAdjustmentModel().Get(accountServiceID, serviceID);
                return PartialView("~/Areas/Accounts/Views/AccountService/_AdjustmentPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AdjustmentSave(AccountServiceAdjustmentModel model)
        {
            try
            {
                model.Insert();
                status = true;
                TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.AdjustmentSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdjustmentDelete(int ID)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceAdjustmentModel model = new AccountServiceAdjustmentModel();
                    model.Delete(ID);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.AdjustmentDeleteSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Exception
        public ActionResult LoadExemptionPopup(int accountServiceID)
        {
            try
            {
                AccountServiceExemptionModel model = new AccountServiceExemptionModel().Get(accountServiceID, 0);
                //model.CollectionDetail = model.CollectionDetail.Where(d => (d.Balance != d.PaidAmount + d.Discount)).ToList();
                return PartialView("~/Areas/Accounts/Views/AccountService/_ExemptionPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ExemptionSave(AccountServiceExemptionModel model)
        {
            try
            {
                model.Insert();
                status = true;
                TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.ExemptionSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadEditExemptionPopup(int accountServiceID, int CollectionID, int ID, decimal Debt, string Reason)
        {
            try
            {
                AccountServiceExemptionModel model = new AccountServiceExemptionModel().Get(accountServiceID, 0);
                model.Amount = Debt;
                model.Reason = Reason;
                model.ID = ID;
                model.CollectionDetail.ForEach(d =>
                {
                    if (d.ID == CollectionID)
                        d.Balance = d.Balance + Debt;
                });
                return PartialView("~/Areas/Accounts/Views/AccountService/_EditExemptionPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ExemptionUpdate(AccountServiceExemptionModel model)
        {
            try
            {
                model.Insert();
                status = true;
                TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.ExemptionSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExemptionDelete(int ID)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceExemptionModel model = new AccountServiceExemptionModel();
                    model.Delete(ID);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.ExemptionDeleteSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExemptionDeleteAll(int ID)
        {
            try
            {

                if (ID > 0)
                {
                    AccountServiceExemptionModel model = new AccountServiceExemptionModel();
                    model.DeleteAll(ID);
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.ExemptionDeleteSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExemptionHistory(int ID)
        {
            string msg = "";
            try
            {
                List<AccountServiceExemptionModel> model = new List<AccountServiceExemptionModel>();
                string Message = Resources.Global.Deleted;
                model = new AccountServiceExemptionModel().GetExemptionHistory(ID);
                model.ForEach(d =>
                {
                    d.Reason = (d.Mode == 3 ? Message : d.Reason);
                });
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountService/_ExemptionHistory.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Correction
        public ActionResult LoadCorrectionPopup(int accountServiceID, int AccountPropertyID, int AccountServiceCollectionDetailId)
        {
            try
            {
                AccountServiceCorrectiontModel model = new AccountServiceCorrectiontModel();
                model.AccountServiceId = accountServiceID;
                model.collectionDetail = new AccountServiceFillingModel();
                model.collectionDetail.CollectionFillingPropertyTaxModel.MovementTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(39), "ID", "Name");
                model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList = new AccountProperty().GetForFilling(AccountPropertyID, AccountServiceCollectionDetailId);
                model.collectionDetail.CollectionFillingPropertyTaxModel.Area = model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().Area;
                model.collectionDetail.CollectionFillingPropertyTaxModel.TerrainValue = model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().TerrainValue;
                model.collectionDetail.CollectionFillingPropertyTaxModel.PropertyAccountID = model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().ID;
                model.collectionDetail.CollectionFillingPropertyTaxModel.PropertyNumber = model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().PropertyNumber;
                model.collectionDetail.CollectionFillingPropertyTaxModel.RigthNumber = model.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList.FirstOrDefault().RigthNumber;
                return PartialView("~/Areas/Accounts/Views/AccountService/_CorrectionPropertyTax.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveCorrectionPropertyTax(AccountServiceCorrectiontModel model)
        {
            try
            {
                model.Insert();
                status = true;
                TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.CorrectionSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Filing Extension
        public ActionResult FilingExtensionGet(int accountserviceID, DateTime filingDueDate)
        {
            try
            {
                AccountServiceExtensionModel model = new AccountServiceExtensionModel().Get(accountserviceID, filingDueDate);
                return PartialView("~/Areas/Accounts/Views/AccountService/_FilingExtensionPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FilingExtensionView(int ID, int accountserviceID)
        {
            try
            {
                AccountServiceExtensionModel model = new AccountServiceExtensionModel().Get(ID, accountserviceID);
                ViewBag.IsViewMode = true;
                return PartialView("~/Areas/Accounts/Views/AccountService/_FilingExtensionPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FilingExtensionSave(AccountServiceExtensionModel model)
        {
            try
            {
                int id = new AccountServiceExtensionModel().Insert(model);
                if (id == 0)
                    throw new Exception(Resources.Global.GeneralErrorMessage);
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEndDate(DateTime startDate, int month)
        {
            try
            {
                return Json(new { status = true, endDate = startDate.AddMonths(month).ToShortDateString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ImportAccountService

        [HttpGet]
        public ActionResult ImportAccountService()
        {
            ImportAccountServiceFilingFieldModel model = new ImportAccountServiceFilingFieldModel();
            return View("~/Areas/Accounts/Views/ImportAccountServiceFiling/ImportAccountServiceFiling.cshtml", model);
        }

        [HttpPost]
        public ActionResult UploadFile(ImportAccountServiceFilingFieldModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
            }

            if (model.UploadFile == null || model.UploadFile.ContentLength <= 0)
            {
                return Json(new { status = status, message = Resources.ImportAccount.ValSelectFile }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                string filepath, extension;
                extension = System.IO.Path.GetExtension(model.UploadFile.FileName).ToLower();
                if (!Common.ImportFileTypes.Contains(extension))
                {
                    return Json(new { status = status, message = string.Format(Resources.ImportAccount.ValFileFormat, string.Join(", ", Common.ImportFileTypes)), data = "" }, JsonRequestBehavior.AllowGet);
                }

                model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString(), extension);

                //GET THE COMPLETE FOLDER PATH AND STORE THE FILE INSIDE IT.
                string path = Server.MapPath(Common.ImportFilePath);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                filepath = System.IO.Path.Combine(path, model.FileName);
                model.UploadFile.SaveAs(filepath);



                DataTable dt = new ImportModel().GetDataTableFromImportFile(filepath, extension);

                if (dt == null || new ImportModel().CheckDataTableEmptyRow(dt))
                {
                    status = false;
                    return Json(new { status = status, message = Resources.Global.ValidEmptyExcelFile }, JsonRequestBehavior.AllowGet);
                }

                DataTable importDataTable = ReadImportFile(model.FileName, true);

                if (importDataTable == null)
                {
                    status = false;
                    return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
                }

                AccountServiceFilingMappingViewModel MappingModel = new AccountServiceFilingMappingViewModel(model.FileName, importDataTable);
                nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountServiceFiling/_ColumnMapping.cshtml", MappingModel);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, data = htmlString, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ColumnMapping(AccountServiceFilingMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    ValidAccountServiceFilingModel validImportmodel = new ValidAccountServiceFilingModel(importDataTable, model);

                    if (validImportmodel.ImportList != null)
                    {
                        InvalidAccountServiceFilingModel _model = new InvalidAccountServiceFilingModel();
                        _model.FileName = model.FileName;
                        _model.ValidAccountStatement = new ImportAccountServiceFilingModel().ImportAccountServiceFilingValid(validImportmodel);
                        _model.Valid = (_model.ValidAccountStatement.Count == 0 || _model.ValidAccountStatement == null);

                        status = true;
                        nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountServiceFiling/_ImportValidation.cshtml", _model);
                    }
                    else
                    {
                        status = false;
                        msg = Resources.Account.ImportAccountFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    msg = Resources.Account.ImportAccountFailedMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportValidation(AccountServiceFilingMappingViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    FinishViewAccountServiceFilingModel finishViewAccountServiceFilingModel = new FinishViewAccountServiceFilingModel(importDataTable, model);

                    status = true;
                    nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountServiceFiling/_FinishImport.cshtml", finishViewAccountServiceFilingModel);
                }
                else
                {
                    status = false;
                    msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FinishImport(AccountServiceFilingMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    ValidAccountServiceFilingModel validImportmodel = new ValidAccountServiceFilingModel(importDataTable, model)
                    {
                        IsValidate = false
                    };

                    int result = new ImportAccountServiceFilingModel().ImportAccountServiceFilingInsert(validImportmodel);
                    if (result == 2)
                    {
                        status = true;
                        msg = Resources.Global.SavedSuccessfullyMessage;
                        TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;

                        ClearImportFile(model.FileName);
                    }
                    else
                    {
                        status = false;
                        msg = Resources.ImportAccount.ImportAccountServiceFilingFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, nextStepData = nextHtmlString, Acctype = 1 }, JsonRequestBehavior.AllowGet);
        }

        #region Common Import
        private DataTable ReadImportFile(string fileName, bool isFirstTime)
        {
            DataTable importDataTable = new DataTable();
            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportAccountServiceFilingModel.S_ImportAccountData]))))
            {
                importDataTable = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportAccountServiceFilingModel.S_ImportAccountData]));
            }
            else
            {
                //Read File From The Folder
                string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
                string extension = System.IO.Path.GetExtension(path).ToLower();

                importDataTable = new ImportModel().GetDataTableFromImportFile(path, extension);

                Session[ImportAccountServiceFilingModel.S_ImportAccountData] = JsonConvert.SerializeObject(importDataTable);
            }
            return importDataTable;
        }

        private void ClearImportFile(string fileName)
        {
            //REMOVE FILE FROM SESSION
            Session.Remove(ImportAccountServiceFilingModel.S_ImportAccountData);

            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        #endregion

        #endregion

        #region CustomField
        public PartialViewResult CustomField(AccountServiceModel accountServiceModel, int customField)
        {
            ViewBag.CustomField = customField;
            return PartialView("~/Areas/Accounts/Views/AccountService/_CustomField.cshtml", accountServiceModel);
        }

        [HttpPost]
        public ActionResult UpdateCustomField(AccountServiceModel model)
        {
            bool status = false;
            string msg = string.Empty;
            DateTime outDate;

            try
            {
                DateTime dateRangeFrom = new DateTime(1753, 1, 1, 12, 00, 00);
                DateTime dateToFrom = new DateTime(9999, 12, 31, 11, 59, 59);

                bool isValidDateTime = false;
                if (DateTime.TryParse(Request["hfNewCustomDateField"], out outDate))
                    isValidDateTime = true;

                DateTime hfNewCustomDateField = new DateTime();
                if (isValidDateTime)
                    hfNewCustomDateField = DateTime.Parse(Request["hfNewCustomDateField"]);

                bool isValidDateTime1 = true;
                if (model.IsDateFieldCustomField1 == true)
                {
                    if (!string.IsNullOrEmpty(model.CustomField1NewValue))
                    {
                        isValidDateTime1 = false;
                        if (DateTime.TryParse(model.CustomField1NewValue, out outDate))
                            isValidDateTime1 = true;

                        DateTime hfCustomDateField1 = new DateTime();
                        if (isValidDateTime1)
                        {
                            hfCustomDateField1 = DateTime.Parse(model.CustomField1NewValue);
                            if (hfCustomDateField1 >= dateRangeFrom && hfCustomDateField1 <= dateToFrom)
                                isValidDateTime1 = true;
                            else
                                isValidDateTime1 = false;
                        }
                    }
                }

                if ((
                        string.IsNullOrEmpty(Request["hfNewCustomDateField"])
                            ||
                        ((isValidDateTime == true) && (hfNewCustomDateField >= dateRangeFrom && hfNewCustomDateField <= dateToFrom))
                 ) && isValidDateTime1 == true)
                {
                    int result = new AccountServiceModel().UpdateForCustomField(model);
                    if (result > 0)
                    {
                        status = true;
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                    }
                }
                else
                {
                    status = false;
                    msg = ArtSolutions.MUN.Web.Resources.Global.WrongDateValidationMsg;
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

        #region Add Right
        public ActionResult AddRight(AccountServiceModel model)
        {
            if (model.AccountID > 0)
                model.PropertyList = HMTLHelperExtensions.CreateSelectList(new AccountProperty().GetAccountPropertyRightByOwner(model.AccountID, model.ServiceID, model.Year, null), "AccountPropertyID", "Property");
            else
                model.PropertyList = new List<SelectListItem>();
            return PartialView("~/Areas/Accounts/Views/AccountService/_AddRight.cshtml", model);
        }
        public JsonResult UpdateRight(AccountServiceModel model)
        {
            bool status = false;
            string msg = string.Empty;
            try
            {
                int result = new AccountServiceModel().UpdateForRight(model);
                if (result > 0)
                {
                    status = true;
                    msg = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
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

        #region Edit License Number
        public ActionResult EditLicenseNumber(int accountServiceID)
        {
            try
            {
                AccountServiceModel model = new AccountServiceModel().Get(accountServiceID, null, null, null, null).FirstOrDefault();
                return PartialView("~/Areas/Accounts/Views/AccountService/_EditLicenseNumberPopup.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateLicenseNumber(AccountServiceModel model)
        {
            bool status = false;
            string msg = string.Empty;
            bool? isDuplicateLicenseNumberExist = null;

            try
            {
                new AccountServiceModel().UpdateForLicenseNumber(model);
                status = true;
                TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.AccountService.LicenseNumberUpdateSuccessMsg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;

                if (msg == Resources.AccountService.LicenseNumberAlreadyExistsMsg)
                    isDuplicateLicenseNumberExist = true;
            }
            return Json(new { status = status, message = msg, isDuplicateLicenseNumber = isDuplicateLicenseNumberExist }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Validate Measured Water Filing
        [HttpGet]
        public ActionResult ValidateMeasuredWaterFiling()
        {
            ImportMeasuredWaterFilingFieldModel model = new ImportMeasuredWaterFilingFieldModel();
            ValidMeasuredWaterFilingModel _model = new ImportMeasuredWaterFilingModel().GetValidateMeasuredWaterFilingForFixFile(null, null, false, false, false, null);
            if (model != null)
            {
                model.ProcessDate = _model.ProcessDate;
                model.PeriodYear = _model.PeriodYear;
                model.PeriodMonth = _model.PeriodMonth;
            }
            return View("~/Areas/Accounts/Views/ValidateMeasuredWaterFiling/Validate.cshtml", model);
        }
        public JsonResult UploadFileForValidateMeasuredWaterFiling()
        {
            try
            {
                string newFileName = "";
                var request = new RestRequest("", Method.POST);

                foreach (string fileName in Request.Files)
                {
                    request.AddHeader("Content-Type", "multipart/form-data");
                    HttpPostedFileBase file = Request.Files[fileName];

                    string filepath, extension;
                    extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    newFileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(file.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString(), extension);

                    //GET THE COMPLETE FOLDER PATH AND STORE THE FILE INSIDE IT.
                    string path = Server.MapPath(Common.ImportFilePath);
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    filepath = System.IO.Path.Combine(path, newFileName);

                    ClearMeasuredWaterImportFile(fileName);//Clear Existing File
                    file.SaveAs(filepath);
                }
                return Json(new { id = 0, message = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage, newFileName = newFileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { id = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ProcessAlreadySavedData(int periodYear, int periodMonth, string filterText)
        {
            string msg = string.Empty, htmlString = string.Empty;
            ValidMeasuredWaterFilingModel validImportmodel = new ValidMeasuredWaterFilingModel();

            try
            {
                ValidMeasuredWaterFilingModel _model = new ImportMeasuredWaterFilingModel().GetValidateMeasuredWaterFilingForFixFile(periodYear, periodMonth, false, false, false, filterText);
                MeasuredWaterFilingMappingViewModel measuredWaterFilingMappingModel = new MeasuredWaterFilingMappingViewModel(null, periodYear, periodMonth, _model.FileDataList);
                validImportmodel.MeasuredWaterFilingList = new ImportModel().GetMeasuredWaterFilingListFromValidateDatatTable(_model.FileDataList, measuredWaterFilingMappingModel);
                if (validImportmodel.MeasuredWaterFilingList != null && validImportmodel.MeasuredWaterFilingList.Count > 0)
                    validImportmodel.Valid = validImportmodel.MeasuredWaterFilingList.Where(x => x.IsValid.ToLower() == "false").Count() > 0 ? false : true;

                status = true;
                msg = Resources.Global.ValidatedSuccessfullyMessage;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ValidateMeasuredWaterFiling/_ErrorList.cshtml", validImportmodel);
                return Json(new { status = status, message = msg, data = htmlString, valid = validImportmodel.Valid, searchText = filterText }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, data = htmlString, valid = validImportmodel.Valid, searchText = filterText }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidateMeasuredWaterFiling(ImportMeasuredWaterFilingFieldModel model)
        {
            string msg = string.Empty, htmlString = string.Empty;

            ValidMeasuredWaterFilingModel validImportmodel = new ValidMeasuredWaterFilingModel();
            validImportmodel.CompanyID = UserSession.Current.CompanyID;
            validImportmodel.PeriodYear = model.PeriodDate.Year;
            validImportmodel.PeriodMonth = model.PeriodDate.Month;
            validImportmodel.UploadedFileNames = model.UploadedFileNames;

            try
            {
                var arrFilePath = model.UploadedFileNames.Split(',').Select(s => System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), s)).ToArray();
                MeasuredWaterFilingReadImportFileModel readImportFileModel = new ImportModel().GetDataTableForMeasuredWaterFiling(arrFilePath, true);

                if (!readImportFileModel.IsValidData)
                {
                    status = false;
                    msg = Resources.Global.InValidDataFileMsg;
                }
                else
                {
                    validImportmodel.MeasuredWaterFilingList = GetValidateMeasuredWaterFilingList(validImportmodel, readImportFileModel.MeasuredWaterFilingList, null, false, false);
                    if (validImportmodel.MeasuredWaterFilingList != null && validImportmodel.MeasuredWaterFilingList.Count > 0)
                        validImportmodel.Valid = validImportmodel.MeasuredWaterFilingList.Where(x => x.IsValid.ToLower() == "false").Count() > 0 ? false : true;

                    status = true;
                    msg = Resources.Global.ValidatedSuccessfullyMessage;
                    htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ValidateMeasuredWaterFiling/_ErrorList.cshtml", validImportmodel);
                    return Json(new { status = status, message = msg, data = htmlString, valid = validImportmodel.Valid }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, data = htmlString, valid = validImportmodel.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReValidateMeasuredWaterFiling(ImportMeasuredWaterFilingFieldModel model)
        {
            string msg = string.Empty, htmlString = string.Empty;
            ValidMeasuredWaterFilingModel reValidImportmodel = new ValidMeasuredWaterFilingModel();
            reValidImportmodel.CompanyID = UserSession.Current.CompanyID;
            reValidImportmodel.PeriodYear = model.PeriodDate.Year;
            reValidImportmodel.PeriodMonth = model.PeriodDate.Month;
            reValidImportmodel.UploadedFileNames = model.UploadedFileNames;

            try
            {
                MeasuredWaterFilingReadImportFileModel readImportFileModel = new MeasuredWaterFilingReadImportFileModel();
                readImportFileModel.MeasuredWaterFilingList = model.MeasuredWaterFilingList.Where(x => x.IsValid == "false").ToList().ToDataTable();
                DataTable validMeasuredWaterFilingList = model.MeasuredWaterFilingList.Where(x => x.IsValid == "true").ToList().ToDataTable();

                if (readImportFileModel.MeasuredWaterFilingList != null && readImportFileModel.MeasuredWaterFilingList.Rows.Count > 0)
                {
                    foreach (DataRow mrow in readImportFileModel.MeasuredWaterFilingList.Rows)
                    {
                        mrow["Note"] = "";
                        mrow["IsValid"] = true;
                    }
                }

                reValidImportmodel.MeasuredWaterFilingList = GetValidateMeasuredWaterFilingList(reValidImportmodel, readImportFileModel.MeasuredWaterFilingList, validMeasuredWaterFilingList, true, false);
                if (reValidImportmodel.MeasuredWaterFilingList != null && reValidImportmodel.MeasuredWaterFilingList.Count > 0)
                    reValidImportmodel.Valid = reValidImportmodel.MeasuredWaterFilingList.Where(x => x.IsValid.ToLower() == "false").Count() > 0 ? false : true;

                status = true;
                msg = Resources.Global.ValidatedSuccessfullyMessage;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ValidateMeasuredWaterFiling/_ErrorList.cshtml", reValidImportmodel);
                return Json(new { status = status, message = msg, data = htmlString, valid = reValidImportmodel.Valid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, data = htmlString, valid = reValidImportmodel.Valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult EditValidateMeasuredWaterFiling(ImportMeasuredWaterFilingFieldModel model)
        {
            return PartialView("~/Areas/Accounts/Views/ValidateMeasuredWaterFiling/_Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult SaveValidateMeasuredWaterFiling(ImportMeasuredWaterFilingFieldModel model)
        {
            try
            {
                if (model.MeasuredWaterFilingModel != null)
                {
                    int currentReading = Convert.ToInt32(model.MeasuredWaterFilingModel.LECTURAACT);
                    int lastReading = Convert.ToInt32(model.MeasuredWaterFilingModel.LastReading);
                    int difference = Convert.ToInt32(model.MeasuredWaterFilingModel.Difference);

                    //50372 =  Current Reading is 0.
                    if (currentReading == 0)
                    {
                        status = false;
                        msg = Resources.AccountService.ValidationCurrentReadingZero;
                    }
                    //50154 =  Current reading must be greater than last reading
                    else if (lastReading > currentReading)
                    {
                        status = false;
                        msg = Resources.AccountService.ValidationCurrentReadingMustBeGreaterThanLastReading;
                    }
                    //50371 =  Water consumption is grater then 100.
                    else if (difference > 100)
                    {
                        status = false;
                        msg = Resources.AccountService.ValidationWaterConsumprionGreaterThanZero;
                    }
                    else
                    {
                        ValidMeasuredWaterFilingModel validModel = new ValidMeasuredWaterFilingModel();
                        validModel.CompanyID = UserSession.Current.CompanyID;
                        validModel.PeriodYear = model.PeriodDate.Year;
                        validModel.PeriodMonth = model.PeriodDate.Month;

                        List<MeasuredWaterFilingFixFileModel> MeasuredWaterFilingList = new List<MeasuredWaterFilingFixFileModel>();
                        MeasuredWaterFilingList.Add(model.MeasuredWaterFilingModel);
                        validModel.FileDataList = MeasuredWaterFilingList.ToDataTable();
                        DataTable dtFixFile = new ImportMeasuredWaterFilingModel().UpdateValidateMeasuredWaterFilingForFixFile(validModel);

                        if (dtFixFile != null && dtFixFile.Rows.Count > 0)
                        {
                            status = true;
                            msg = Resources.Global.UpdatedSuccessfullyMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, data = model.MeasuredWaterFilingModel }, JsonRequestBehavior.AllowGet);
        }
        public List<ValidImportMeasuredWaterFilingFixFileModel> GetValidateMeasuredWaterFilingList(ValidMeasuredWaterFilingModel model, DataTable dtMeasuredWaterFiling, DataTable dtValidMeasuredWaterFilingList, bool isRevalidate, bool isValidateImport)
        {
            if (dtMeasuredWaterFiling != null && dtMeasuredWaterFiling.Rows.Count > 0)
            {
                model.FileDataList = dtMeasuredWaterFiling.AsEnumerable().CopyToDataTable();
                ValidateMeasuredWaterAccountServiceFilingModel accountServiceDetailList = new ImportMeasuredWaterFilingModel().GetAccountServiceForValidateMeasuredWaterFiling(model);
                var accountServiceList = (from sr in dtMeasuredWaterFiling.AsEnumerable()
                                          join kv in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                          on new { a = sr["RowNo"].ToString() } equals new { a = kv["RowNo"].ToString() }
                                          select new { DataRow = sr, NewValue = kv }).ToList();


                foreach (var x in accountServiceList)
                {
                    if (!isRevalidate) //17-May-2022
                        x.DataRow["LastReading"] = x.NewValue["LastReading"];

                    //27-May-2022 = Current Reading  = Last Reading when Current Reading is Zero
                    if (Convert.ToDecimal(x.DataRow["LECTURAACT"]) == 0)
                        x.DataRow["LECTURAACT"] = x.NewValue["LastReading"];
                }

                var validDetailList = (from sr in dtMeasuredWaterFiling.AsEnumerable()
                                       join kv in accountServiceDetailList.ValidDetailList.AsEnumerable()
                                       on new { a = sr["RowNo"].ToString() } equals new { a = kv["RowNo"].ToString() }
                                       select new { DataRow = sr, NewValue = kv }).ToList();

                if (!isValidateImport)
                {
                    foreach (var x in validDetailList)
                    {
                        x.DataRow["TaxNumber"] = x.NewValue["TaxNumber"];
                    }
                }

                foreach (DataRow row in dtMeasuredWaterFiling.Rows)
                {
                    string UBICACION = row["UBICACION"].ToString();
                    string CODIGOM = row["CODIGOM"].ToString();
                    string CATEGORIA = row["CATEGORIA"].ToString();
                    string FECHA = row["FECHA"].ToString();
                    string LECTURAACT = row["LECTURAACT"].ToString();
                    string TaxNumber = row["TaxNumber"].ToString();
                    int LastReading = String.IsNullOrEmpty(Convert.ToString(row["LastReading"])) ? 0 : Convert.ToInt32(row["LastReading"]);
                    int Difference = (String.IsNullOrEmpty(LECTURAACT) ? 0 : Convert.ToInt32(LECTURAACT)) - LastReading;

                    if (accountServiceDetailList.ValidDetailList != null && accountServiceDetailList.ValidDetailList.Rows.Count > 0)
                    {
                        var drValidDetail = (from x in accountServiceDetailList.ValidDetailList.AsEnumerable()
                                             where x.Field<string>("UBICACION") == row["UBICACION"].ToString() && x.Field<string>("CODIGOM") == row["CODIGOM"].ToString()
                                             select x).ToList();

                        if (drValidDetail.Count == 0)
                        {
                            TaxNumber = "";
                            LastReading = 0;
                            Difference = (String.IsNullOrEmpty(LECTURAACT) ? 0 : Convert.ToInt32(LECTURAACT)) - LastReading;
                            row["TaxNumber"] = TaxNumber;
                        }
                    }

                    row["LastReading"] = LastReading;
                    row["Difference"] = Difference;

                    foreach (var rerror in arrValidateErrorCode)
                    {
                        // Ubicacion cannot be empty.
                        if (rerror == 50350 && string.IsNullOrEmpty(UBICACION))
                        {
                            row["Note"] = Resources.AccountService.ValidationUbicacionRequired;
                            row["IsValid"] = false;
                            break;
                        }
                        //Codigom cannot be empty.
                        else if (rerror == 50351 && string.IsNullOrEmpty(CODIGOM))
                        {
                            row["Note"] = Resources.AccountService.ValidationCodigomRequired;
                            row["IsValid"] = false;
                            break;
                        }
                        //Category cannot be empty. 
                        else if (rerror == 50352 && string.IsNullOrEmpty(CATEGORIA))
                        {
                            row["Note"] = Resources.AccountService.ValidationCategoryRequired;
                            row["IsValid"] = false;
                            break;
                        }
                        // Category does not exist.
                        else if (rerror == 50359 && !string.IsNullOrEmpty(CATEGORIA) && !ServiceCode.Contains(CATEGORIA))
                        {
                            row["Note"] = Resources.AccountService.ValidationCategoryNotExist;
                            row["IsValid"] = false;
                            break;
                        }
                        //Date cannot be empty.
                        else if (rerror == 50354 && string.IsNullOrEmpty(FECHA))
                        {
                            row["Note"] = Resources.AccountService.ValidationDateRequired;
                            row["IsValid"] = false;
                            break;
                        }
                        //Format of Date is invalid.Provide in DDMMYYYY format.
                        else if
                        (
                            rerror == 50398
                                &&
                            !string.IsNullOrEmpty(FECHA)
                                &&
                            (
                                FECHA.Length != 8
                                    ||
                                (
                                   FECHA.Length == 8
                                       &&
                                   (
                                        (
                                             !arrNumbers.Contains(FECHA.Substring(0, 1)) || !arrNumbers.Contains(FECHA.Substring(1, 1)) ||
                                             !arrNumbers.Contains(FECHA.Substring(2, 1)) || !arrNumbers.Contains(FECHA.Substring(3, 1)) ||
                                             !arrNumbers.Contains(FECHA.Substring(4, 1)) || !arrNumbers.Contains(FECHA.Substring(5, 1)) ||
                                             !arrNumbers.Contains(FECHA.Substring(6, 1)) || !arrNumbers.Contains(FECHA.Substring(7, 1))
                                        )
                                                ||
                                        !(

                                            (Convert.ToInt32(FECHA.Substring(0, 2)) >= 1 && Convert.ToInt32(FECHA.Substring(0, 2)) <= 31)
                                                     &&
                                            (Convert.ToInt32(FECHA.Substring(2, 2)) >= 1 && Convert.ToInt32(FECHA.Substring(2, 2)) <= 12)
                                                     &&
                                            (FECHA.Substring(4, 4).Length == 4 && Convert.ToInt32(FECHA.Substring(4, 4)) > 0)
                                        )
                                    )
                                )
                            )
                        )
                        {
                            row["Note"] = Resources.AccountService.ValidationInvalidDateFormat;
                            row["IsValid"] = false;
                            break;
                        }
                        //Reading cannot be empty.
                        else if (rerror == 50353 && string.IsNullOrEmpty(LECTURAACT))
                        {
                            row["Note"] = Resources.AccountService.ValidationReadingRequired;
                            row["IsValid"] = false;
                            break;
                        }
                        //Ubicacion Or Codigom does not belong to the account record.
                        // 50361 =>50156=>50293,50288,50289,50362,50154,50371
                        else if (rerror == 50361 && !string.IsNullOrEmpty(UBICACION) && !string.IsNullOrEmpty(CODIGOM))
                        {
                            //50361 = Ubicacion Or Codigom does not belong to the account record.
                            if (string.IsNullOrEmpty(TaxNumber))
                            {
                                row["Note"] = Resources.AccountService.ValidationUbicacionOrCodigomNotBelongToAccount;
                                row["IsValid"] = false;
                                break;
                            }
                            else
                            {
                                var drAccountService = (from x in accountServiceDetailList.AccountServiceNotExistList.AsEnumerable()
                                                        where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM
                                                        select x).ToList();

                                //50156 = Service is not available for this year
                                if (drAccountService != null && drAccountService.Any())
                                {
                                    row["Note"] = Resources.AccountService.ValidationServiceNotAvailableForThisYear;
                                    row["IsValid"] = false;
                                    break;
                                }
                                //50293,50288,50289,50362,50154,50371
                                else
                                {
                                    foreach (var derror in arrDBErrorCode)
                                    {
                                        //Already exists a filing for same service and month.
                                        if (derror == 50293 && accountServiceDetailList.AccountServiceList != null && accountServiceDetailList.AccountServiceList.Rows.Count > 0)
                                        {
                                            var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                             where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && !string.IsNullOrEmpty(Convert.ToString(x["FillingDate"]))
                                                             select x).ToList();
                                            if (drInValid != null && drInValid.Any())
                                            {
                                                row["Note"] = Resources.AccountService.ValidationAlreadyExistFillingForSameServiceAndMonth;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                        }
                                        //This service is locked.
                                        else if (derror == 50288 && accountServiceDetailList.AccountServiceList != null && accountServiceDetailList.AccountServiceList.Rows.Count > 0)
                                        {
                                            var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                             where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && Convert.ToBoolean(x["IsLock"]) == true
                                                             select x).ToList();
                                            if (drInValid != null && drInValid.Any())
                                            {
                                                row["Note"] = Resources.AccountService.ValidationServiceLocked;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                        }
                                        //This service is inactive.
                                        else if (derror == 50289 && accountServiceDetailList.AccountServiceList != null && accountServiceDetailList.AccountServiceList.Rows.Count > 0)
                                        {
                                            var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                             where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && Convert.ToBoolean(x["IsActive"]) == false
                                                             select x).ToList();
                                            if (drInValid != null && drInValid.Any())
                                            {
                                                row["Note"] = Resources.AccountService.ValidationServiceInactive;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                        }
                                        // Period is less than Service Start Period.
                                        else if (derror == 50362 && accountServiceDetailList.AccountServiceList != null && accountServiceDetailList.AccountServiceList.Rows.Count > 0)
                                        {
                                            var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                             where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && model.PeriodMonth < Convert.ToInt32(x["ServiceStartPeriod"])
                                                             select x).ToList();
                                            if (drInValid != null && drInValid.Any())
                                            {
                                                row["Note"] = Resources.AccountService.ValidationPeriodLessThanServiceStartPeriod;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                        }
                                        //Current Reading is 0.
                                        else if
                                        (
                                            derror == 50372
                                                &&
                                           (
                                               (!isValidateImport && Convert.ToInt32(LECTURAACT) == 0)
                                                    ||
                                                (isValidateImport && row["IsValid"].ToString().ToLower() == "no" && Convert.ToInt32(LECTURAACT) == 0)
                                           )
                                        )
                                        {
                                            row["Note"] = Resources.AccountService.ValidationCurrentReadingZero;
                                            row["IsValid"] = false;
                                            break;
                                        }
                                        //Current reading must be greater than last reading
                                        else if (derror == 50154)
                                        {
                                            if (isValidateImport && Difference < 0)
                                            {
                                                row["Note"] = Resources.AccountService.ValidationCurrentReadingMustBeGreaterThanLastReading;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                            else
                                            {
                                                if
                                                (
                                                    accountServiceDetailList.AccountServiceList != null
                                                        &&
                                                    accountServiceDetailList.AccountServiceList.Rows.Count > 0
                                                        &&
                                                    Convert.ToInt32(LastReading) > Convert.ToInt32(LECTURAACT)
                                                )
                                                {
                                                    //var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                    //                 where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && Convert.ToInt32(x["LastReading"]) > Convert.ToInt32(LECTURAACT)
                                                    //                 select x).ToList();
                                                    //if (drInValid != null && drInValid.Any())
                                                    //{
                                                    row["Note"] = Resources.AccountService.ValidationCurrentReadingMustBeGreaterThanLastReading;
                                                    row["IsValid"] = false;
                                                    break;
                                                    //}
                                                }
                                            }
                                        }
                                        //Water consumption is grater then 100.
                                        else if
                                        (
                                            derror == 50371
                                                &&
                                            (
                                               !isValidateImport
                                                   ||
                                               (isValidateImport && row["IsValid"].ToString().ToLower() == "no")
                                            )
                                        )
                                        {
                                            if (isValidateImport && Difference > 100)
                                            {
                                                row["Note"] = Resources.AccountService.ValidationWaterConsumprionGreaterThanZero;
                                                row["IsValid"] = false;
                                                break;
                                            }
                                            else
                                            {
                                                if (accountServiceDetailList.AccountServiceList != null && accountServiceDetailList.AccountServiceList.Rows.Count > 0)
                                                {
                                                    var drInValid = (from x in accountServiceDetailList.AccountServiceList.AsEnumerable()
                                                                     where x.Field<string>("UBICACION") == UBICACION && x.Field<string>("CODIGOM") == CODIGOM && Difference > 100
                                                                     select x).ToList();
                                                    if (drInValid != null && drInValid.Any())
                                                    {
                                                        row["Note"] = Resources.AccountService.ValidationWaterConsumprionGreaterThanZero;
                                                        row["IsValid"] = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (model.FileDataList != null && model.FileDataList.Rows.Count > 0)
                model.FileDataList.Rows.Clear();


            MeasuredWaterFilingMappingViewModel measuredWaterFilingMappingModel = new MeasuredWaterFilingMappingViewModel(model.UploadedFileNames, model.PeriodYear, model.PeriodMonth, dtMeasuredWaterFiling);

            if (!isValidateImport)
            {
                if (dtMeasuredWaterFiling != null && dtMeasuredWaterFiling.Rows.Count > 0)
                    model.FileDataList = dtMeasuredWaterFiling.AsEnumerable().CopyToDataTable();
                else
                    model.FileDataList = new DataTable();

                DataTable dtFixFile = new DataTable();

                if (!isRevalidate)
                    dtFixFile = new ImportMeasuredWaterFilingModel().InsertValidateMeasuredWaterFilingForFixFile(model);
                else
                {
                    //Merge IsValid Record with Existing list
                    if (dtValidMeasuredWaterFilingList != null && dtValidMeasuredWaterFilingList.Rows.Count > 0)
                        model.FileDataList.Merge(dtValidMeasuredWaterFilingList);
                    dtFixFile = new ImportMeasuredWaterFilingModel().UpdateValidateMeasuredWaterFilingForFixFile(model);
                }
                return new ImportModel().GetMeasuredWaterFilingListFromValidateDatatTable(dtFixFile, measuredWaterFilingMappingModel);
            }
            else
            {
                if (!dtMeasuredWaterFiling.AsEnumerable().Where(rows => rows.Field<string>("IsValid").ToLower() == "false").Any())
                    model.FileDataList = dtMeasuredWaterFiling.AsEnumerable().CopyToDataTable();
                else
                    model.FileDataList = dtMeasuredWaterFiling.AsEnumerable().Where(rows => rows.Field<string>("IsValid").ToLower() == "false").CopyToDataTable();
                return new ImportModel().GetMeasuredWaterFilingListFromValidateDatatTable(model.FileDataList, measuredWaterFilingMappingModel);
            }
        }
        public ActionResult DownloadValidateMeasuredWaterFiling(DateTime periodDate)
        {
            string fileName = Resources.AccountService.ValidateMeasuredWaterFiling + "_" + UserSession.Current.CompanyID.ToString() + "_" + DateTime.Now.Date.ToLongDateString() + ".xlsx";

            //GET THE COMPLETE FOLDER PATH For Download.
            string downloadFilePath = Server.MapPath(Common.DownloadFilePath);
            if (!System.IO.Directory.Exists(downloadFilePath))
                System.IO.Directory.CreateDirectory(downloadFilePath);
            string validateFilePath = System.IO.Path.Combine(downloadFilePath, fileName);

            ValidMeasuredWaterFilingModel model = new ImportMeasuredWaterFilingModel().GetValidateMeasuredWaterFilingForFixFile(periodDate.Year, periodDate.Month, true, false, false, null);
            if (model != null && model.FileDataList != null && model.FileDataList.Rows.Count > 0)
                GenerateExcelFileForValidateMeasuredWaterFiling(model.FileDataList, validateFilePath,false);

            byte[] byteArray = System.IO.File.ReadAllBytes(validateFilePath);

            MemoryStream stream = new MemoryStream(byteArray);
            Response.AppendHeader("Set-Cookie", "fileDownload=true; path=/");
            return File(stream, "text/plain", fileName);

        }
        private void GenerateExcelFileForValidateMeasuredWaterFiling(DataTable dt, string path,bool isReport)
        {
            //Delete if file exist
            if (System.IO.File.Exists(path))
                DeleteDownloadedFile(path);
            //

            //Remove Note,IsValid column for Report
            if (isReport)
            {
                dt.Columns.RemoveAt(10);
                dt.Columns.RemoveAt(10);
            }
            //            
            
            //Rename datatable column name in spanish before generate
            dt.Columns[0].ColumnName = "Año";
            dt.Columns[1].ColumnName = "Mes";
            dt.Columns[2].ColumnName = "Categoria";
            dt.Columns[3].ColumnName = "Cédula";
            dt.Columns[4].ColumnName = "Ubicacion";
            dt.Columns[5].ColumnName = "Codigom";
            dt.Columns[6].ColumnName = "Lectura Actual";
            dt.Columns[7].ColumnName = "Última Lectura";
            dt.Columns[8].ColumnName = "Diferencia";
            dt.Columns[9].ColumnName = "Fecha";

            if (!isReport)
            {
                dt.Columns[10].ColumnName = "Nota";
                dt.Columns[11].ColumnName = "Es Válido";
            }

            dt.AcceptChanges();
            //

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(Resources.AccountService.ValidateMeasuredWaterFiling);
            workSheet.Cells[1, 1].LoadFromDataTable(dt, true);
            workSheet.Column(10).Style.Numberformat.Format = "@";
            byte[] result = excel.GetAsByteArray();
            System.IO.File.WriteAllBytes(path, result);
        }
        public JsonResult RemoveUploadImportFile(string fileName)
        {
            try
            {
                status = true;
                ClearMeasuredWaterImportFile(fileName);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        private void DeleteDownloadedFile(string fileName)
        {
            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.DownloadFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        #endregion

        public ActionResult DownloadValidateMeasuredWaterFilingInconsistencies(DateTime periodDate)
        {
            string fileName = Resources.AccountService.Inconsistencies + "_" + UserSession.Current.CompanyID.ToString() + "_" + DateTime.Now.Date.ToLongDateString() + ".xlsx";

            //GET THE COMPLETE FOLDER PATH For Download.
            string downloadFilePath = Server.MapPath(Common.DownloadFilePath);
            if (!System.IO.Directory.Exists(downloadFilePath))
                System.IO.Directory.CreateDirectory(downloadFilePath);
            string validateFilePath = System.IO.Path.Combine(downloadFilePath, fileName);

            ValidMeasuredWaterFilingModel model = new ImportMeasuredWaterFilingModel().GetValidateMeasuredWaterFilingForFixFile(periodDate.Year, periodDate.Month, false, true, false, null);
            if (model != null && model.FileDataList != null && model.FileDataList.Rows.Count > 0)
                GenerateExcelFileForValidateMeasuredWaterFiling(model.FileDataList, validateFilePath,true);

            byte[] byteArray = System.IO.File.ReadAllBytes(validateFilePath);

            MemoryStream stream = new MemoryStream(byteArray);
            Response.AppendHeader("Set-Cookie", "fileDownload=true; path=/");
            return File(stream, "text/plain", fileName);

        }
        public ActionResult DownloadValidateMeasuredWaterFilingHighConsumption(DateTime periodDate)
        {
            string fileName = Resources.AccountService.HighConsumption + "_" + UserSession.Current.CompanyID.ToString() + "_" + DateTime.Now.Date.ToLongDateString() + ".xlsx";

            //GET THE COMPLETE FOLDER PATH For Download.
            string downloadFilePath = Server.MapPath(Common.DownloadFilePath);
            if (!System.IO.Directory.Exists(downloadFilePath))
                System.IO.Directory.CreateDirectory(downloadFilePath);
            string validateFilePath = System.IO.Path.Combine(downloadFilePath, fileName);

            ValidMeasuredWaterFilingModel model = new ImportMeasuredWaterFilingModel().GetValidateMeasuredWaterFilingForFixFile(periodDate.Year, periodDate.Month, false, false, true, null);
            if (model != null && model.FileDataList != null && model.FileDataList.Rows.Count > 0)
                GenerateExcelFileForValidateMeasuredWaterFiling(model.FileDataList, validateFilePath,true);

            byte[] byteArray = System.IO.File.ReadAllBytes(validateFilePath);

            MemoryStream stream = new MemoryStream(byteArray);
            Response.AppendHeader("Set-Cookie", "fileDownload=true; path=/");
            return File(stream, "text/plain", fileName);

        }

        #region Import Measured Water Filing 
        [HttpPost]
        public ActionResult ImportMeasuredWaterFilingSave(DateTime periodDate)
        {
            try
            {
                ValidMeasuredWaterFilingModel validImportmodel = new ValidMeasuredWaterFilingModel();
                validImportmodel.PeriodYear = periodDate.Year;
                validImportmodel.PeriodMonth = periodDate.Month;

                int result = new ImportMeasuredWaterFilingModel().ImportMeasuredWaterFilingInsert(validImportmodel);
                if (result == 2)
                {
                    status = true;
                    msg = Resources.Global.SavedSuccessfullyMessage;
                    //TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;
                }
                else
                {
                    status = false;
                    msg = Resources.AccountService.ImportMeasuredWaterFailedMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult ImportMeasuredWaterFiling() // Done
        //{
        //    ImportMeasuredWaterFilingFieldModel model = new ImportMeasuredWaterFilingFieldModel();
        //    return View("~/Areas/Accounts/Views/ImportMeasuredWaterFiling/ImportMeasuredWaterFiling.cshtml", model);
        //}

        //[HttpPost]
        //public ActionResult UploadMeasuredWaterFile(ImportMeasuredWaterFilingFieldModel model)
        //{
        //    if (!ModelState.IsValid || model == null)
        //    {
        //        return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (model.UploadFile == null || model.UploadFile.ContentLength <= 0)
        //    {
        //        return Json(new { status = status, message = Resources.Global.EmptyFileMsg }, JsonRequestBehavior.AllowGet);
        //    }
        //    try
        //    {
        //        string filepath, extension;
        //        extension = System.IO.Path.GetExtension(model.UploadFile.FileName).ToLower();
        //        if (!Common.ImportFileTypes[0].Contains(extension))
        //        {
        //            return Json(new { status = status, message = string.Format(Resources.ImportAccount.ValFileFormat, string.Join(", ", Common.ImportFileTypes[0])), data = "" }, JsonRequestBehavior.AllowGet);
        //        }

        //        model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString(), extension);

        //        //GET THE COMPLETE FOLDER PATH AND STORE THE FILE INSIDE IT.
        //        string path = Server.MapPath(Common.ImportFilePath);
        //        if (!System.IO.Directory.Exists(path))
        //            System.IO.Directory.CreateDirectory(path);
        //        filepath = System.IO.Path.Combine(path, model.FileName);
        //        model.UploadFile.SaveAs(filepath);

        //        MeasuredWaterFilingReadImportFileModel readImportFileModel = ReadMeasuredWaterImportFile(model.FileName, true);

        //        if (!readImportFileModel.IsValidData)
        //        {
        //            status = false;

        //            if (readImportFileModel.MeasuredWaterFilingList.Columns.Count == 0)
        //                msg = Resources.Global.EmptyFileMsg;
        //            else
        //                msg = Resources.Global.ColumnDoesntMatchWithSampleFileMsg;
        //        }
        //        else
        //        {
        //            if (readImportFileModel.MeasuredWaterFilingList.Rows.Count == 0)
        //                msg = Resources.Global.EmptyFileMsg;
        //            else
        //            {
        //                if (readImportFileModel.Year == model.PeriodDate.Year && readImportFileModel.Month == model.PeriodDate.Month)
        //                {
        //                    ValidMeasuredWaterFilingModel validImportmodel = new ValidMeasuredWaterFilingModel();
        //                    validImportmodel.CompanyID = UserSession.Current.CompanyID;
        //                    validImportmodel.PeriodYear = model.PeriodDate.Year;
        //                    validImportmodel.PeriodMonth = model.PeriodDate.Month;
        //                    validImportmodel.UploadedFileNames = model.FileName;
        //                    validImportmodel.FileName = model.FileName;

        //                    validImportmodel.MeasuredWaterFilingList = GetValidateMeasuredWaterFilingList(validImportmodel, readImportFileModel.MeasuredWaterFilingList, null, false, true);
        //                    if (validImportmodel.MeasuredWaterFilingList != null && validImportmodel.MeasuredWaterFilingList.Count > 0)
        //                        validImportmodel.Valid = validImportmodel.MeasuredWaterFilingList.Where(x => x.IsValid.ToLower() == "false").Count() > 0 ? false : true;

        //                    if (validImportmodel.Valid)
        //                        validImportmodel.MeasuredWaterFilingList = null;

        //                    status = true;
        //                    nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportMeasuredWaterFiling/_ImportValidation.cshtml", validImportmodel);
        //                }
        //                else
        //                {
        //                    status = false;
        //                    msg = Resources.AccountService.PeriodNotMatchWithSelectedPeriod;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //        msg = ex.Message;
        //    }
        //    return Json(new { status = status, message = msg, data = htmlString, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult ImportMeasuredWaterValidation(MeasuredWaterFilingMappingViewModel model)
        //{
        //    try
        //    {
        //        DataTable importDataTable = ReadMeasuredWaterImportFile(model.FileName, false).MeasuredWaterFilingList;
        //        MeasuredWaterFilingMappingViewModel measuredWaterFilingMappingModel = new MeasuredWaterFilingMappingViewModel(model.FileName, model.PeriodYear, model.PeriodMonth, importDataTable);

        //        if (importDataTable != null)
        //        {
        //            FinishViewMeasuredWaterFilingModel finishViewMeasuredWaterFilingModel = new FinishViewMeasuredWaterFilingModel(importDataTable, measuredWaterFilingMappingModel);
        //            status = true;
        //            nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportMeasuredWaterFiling/_FinishImport.cshtml", finishViewMeasuredWaterFilingModel);
        //        }
        //        else
        //        {
        //            status = false;
        //            msg = Resources.Global.ValNull;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //        msg = ex.Message;
        //    }
        //    return Json(new { status = status, message = msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult FinishMeasuredWaterImport(MeasuredWaterFilingMappingViewModel model)
        //{
        //    try
        //    {
        //        MeasuredWaterFilingReadImportFileModel readImportFileModel = ReadMeasuredWaterImportFile(model.FileName, false);
        //        MeasuredWaterFilingMappingViewModel measuredWaterFilingMappingModel = new MeasuredWaterFilingMappingViewModel(model.FileName, model.PeriodYear, model.PeriodMonth, readImportFileModel.MeasuredWaterFilingList);

        //        if (readImportFileModel.MeasuredWaterFilingList != null)
        //        {
        //            ValidMeasuredWaterFilingModel validImportmodel = new ValidMeasuredWaterFilingModel(readImportFileModel.MeasuredWaterFilingList, measuredWaterFilingMappingModel, false, false, null)
        //            {
        //                IsValidate = false
        //            };

        //            int result = new ImportMeasuredWaterFilingModel().ImportMeasuredWaterFilingInsert(validImportmodel);
        //            if (result == 2)
        //            {
        //                status = true;
        //                msg = Resources.Global.SavedSuccessfullyMessage;
        //                TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;

        //                ClearMeasuredWaterImportFile(model.FileName);
        //            }
        //            else
        //            {
        //                status = false;
        //                msg = Resources.AccountService.ImportMeasuredWaterFailedMsg;
        //            }
        //        }
        //        else
        //        {
        //            status = false;
        //            msg = Resources.Global.ValNull;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //        msg = ex.Message;
        //    }
        //    return Json(new { status = status, message = msg, nextStepData = nextHtmlString, Acctype = 1 }, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Common Import
        private MeasuredWaterFilingReadImportFileModel ReadValidateMeasuredWaterFilingFixFile(string[] arrFilePath, int year, int month)
        {
            MeasuredWaterFilingReadImportFileModel model = new MeasuredWaterFilingReadImportFileModel();
            model = new ImportModel().GetDataTableForMeasuredWaterFilingForFixFile(arrFilePath, year, month); // TXT   
            return model;
        }
        private MeasuredWaterFilingReadImportFileModel ReadValidateMeasuredWaterFilingFile(string[] arrFilePath, bool isFirstTime)
        {
            MeasuredWaterFilingReadImportFileModel model = new MeasuredWaterFilingReadImportFileModel();

            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData]))))
            {
                model.MeasuredWaterFilingList = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData]));
            }
            else
            {
                //Read File From The Folder              
                model = new ImportModel().GetDataTableForMeasuredWaterFiling(arrFilePath, false); // TXT               
                Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData] = JsonConvert.SerializeObject(model.MeasuredWaterFilingList);
            }
            return model;
        }
        private MeasuredWaterFilingReadImportFileModel ReadMeasuredWaterImportFile(string fileName, bool isFirstTime)
        {
            MeasuredWaterFilingReadImportFileModel model = new MeasuredWaterFilingReadImportFileModel();

            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData]))))
            {
                model.MeasuredWaterFilingList = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData]));
            }
            else
            {
                //Read File From The Folder
                string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
                string extension = System.IO.Path.GetExtension(path).ToLower();

                model.MeasuredWaterFilingList = new ImportModel().GetDataTableFromExcelForImportMeasuredWaterFiling(path); // XLSX

                if (
                    model.MeasuredWaterFilingList != null
                        &&
                    model.MeasuredWaterFilingList.Columns.Count > 0
                        &&
                    (
                      (model.MeasuredWaterFilingList.Columns[0].ColumnName == "Año")
                                    &&
                       (model.MeasuredWaterFilingList.Columns[1].ColumnName == "Mes")
                                    &&
                       (model.MeasuredWaterFilingList.Columns[2].ColumnName == "Categoria")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[3].ColumnName == "Cédula")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[4].ColumnName == "Ubicacion")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[5].ColumnName == "Codigom")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[6].ColumnName == "Lectura Actual")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[7].ColumnName == "Última Lectura")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[8].ColumnName == "Diferencia")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[9].ColumnName == "Fecha")
                                     &&
                       (model.MeasuredWaterFilingList.Columns[10].ColumnName == "Es Válido" || model.MeasuredWaterFilingList.Columns[11].ColumnName == "Es Válido")
                    )
                )
                {
                    model.IsValidData = true;

                    //Rename Datatable column 
                    model.MeasuredWaterFilingList.Columns[0].ColumnName = "Year";
                    model.MeasuredWaterFilingList.Columns[1].ColumnName = "Month";
                    model.MeasuredWaterFilingList.Columns[2].ColumnName = "CATEGORIA";
                    model.MeasuredWaterFilingList.Columns[3].ColumnName = "TaxNumber";
                    model.MeasuredWaterFilingList.Columns[4].ColumnName = "UBICACION";
                    model.MeasuredWaterFilingList.Columns[5].ColumnName = "CODIGOM";
                    model.MeasuredWaterFilingList.Columns[6].ColumnName = "LECTURAACT";
                    model.MeasuredWaterFilingList.Columns[7].ColumnName = "LastReading";
                    model.MeasuredWaterFilingList.Columns[8].ColumnName = "Difference";
                    model.MeasuredWaterFilingList.Columns[9].ColumnName = "FECHA";

                    if (model.MeasuredWaterFilingList.Columns[10].ColumnName == "Es Válido")
                        model.MeasuredWaterFilingList.Columns[10].ColumnName = "IsValid";
                    if (model.MeasuredWaterFilingList.Columns[11].ColumnName == "Es Válido")
                        model.MeasuredWaterFilingList.Columns[11].ColumnName = "IsValid";

                    if (model.MeasuredWaterFilingList.Columns[10].ColumnName == "Nota")
                        model.MeasuredWaterFilingList.Columns[10].ColumnName = "Note";
                    //

                    DataTable copyDataTable;
                    copyDataTable = model.MeasuredWaterFilingList.Copy();
                    //copyDataTable.Columns.Remove("Nota");
                    if (copyDataTable != null && copyDataTable.Rows.Count > 0)
                    {
                        model.Year = Convert.ToInt32(copyDataTable.Rows[0][0]);
                        model.Month = Convert.ToInt32(copyDataTable.Rows[0][1]);
                    }

                    copyDataTable.Columns.Remove("Year");
                    copyDataTable.Columns.Remove("Month");

                    copyDataTable.Columns.Add("RowNo");

                    int rowNo = 1;
                    foreach (DataRow dr in copyDataTable.Rows)
                    {
                        dr["RowNo"] = rowNo++;
                        dr["Note"] = null;
                        dr["IsValid"] = "Yes";
                    }
                    model.MeasuredWaterFilingList = copyDataTable;
                }
                else
                {
                    model.IsValidData = false;
                }

                Session[ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData] = JsonConvert.SerializeObject(model.MeasuredWaterFilingList);
            }
            return model;
        }
        private void ClearMeasuredWaterImportFile(string fileName)
        {
            //REMOVE FILE FROM SESSION
            Session.Remove(ImportMeasuredWaterFilingModel.S_ImportMeasuredWaterFilingData);

            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        #endregion

        #region Export Measure Water Account Service
        public ActionResult ExportMeasureWater(int? AccountID)
        {
            string Filename = "ExportMeasureWater" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
            MemoryStream stream = new AccountServiceCollectionFillingMeasuredWaterModel().CollectionFillingMeasuredWaterExport(AccountID);
            //Response.SetCookie(new HttpCookie(Filename, "true") { Path = "/" });
            Response.AppendHeader("Set-Cookie", "fileDownload=true; path=/");
            return File(stream, "text/plain", Filename);

        }
        #endregion
    }
}
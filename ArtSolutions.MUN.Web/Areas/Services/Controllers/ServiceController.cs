using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Services.Controllers
{
    [Filters.IsCompanyConfigured]
    public class ServiceController : BaseController
    {
        ServiceModel Model = new ServiceModel();
        bool status = false;
        string msg = string.Empty;
        string htmlstring = string.Empty;

        #region List Service
        public ActionResult List()
        {
            ServiceList list = new ServiceList();
            try
            {
                list.ServiceModelList = new List<ServiceModel>();
                ViewBag.ServiceTypeGroupList = new ServiceTypeGroupModel().GetServiceTypeGroup().Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Services/Views/Service/List.cshtml", list);
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText, Nullable<int> GroupID, Nullable<int> ServiceTypeID)
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

                ServiceList list = new Service().Get(null, filterText, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir), GroupID, ServiceTypeID);
                list.ServiceModelList.ForEach(a =>
                {
                    a.EffectiveFromByCulture = a.EffectiveFrom.ToString("d");
                    a.EffectiveToByCulture = a.EffectiveTo.ToString("d");
                });
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.ServiceModelList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        [HttpGet]
        public ActionResult View(int? serviceId)
        {
            if (serviceId == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                Model = new Service().Get(serviceId.Value);
                if (Model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Service.InvalidServiceNumber;
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
            return View("~/Areas/Services/Views/Service/View.cshtml", Model);
        }
        #endregion

        #region New/Edit Service 
        [HttpGet]
        public ActionResult Add()
        {
            //Service service = new Service();
            //service.SetInitialDropDown(Model);
            //service.SetInitialRadioButton(Model);
            //return View("~/Areas/Services/Views/Service/Add.cshtml", Model);

            try
            {
                Service service = new Service();
                service.SetInitialDropDown(Model);
                service.SetInitialRadioButton(Model);
                return View("~/Areas/Services/Views/Service/Add.cshtml", Model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Add(ServiceModel model, int actionType)
        {
            try
            {
                model.StartDate = DateTime.Parse(Request["hdnFormattedStartDate"]);
                model.EndDate = DateTime.Parse(Request["hdnFormattedEndDate"]);
                ValidateDates(model);

                model.EndDate = model.StartDate.AddYears(1).AddDays(-1);
                var serviceCalculationDates = new JavaScriptSerializer().Deserialize<List<ServiceCalculationDateModel>>(model.ServiceCalculationDateListJson);
                if ((serviceCalculationDates == null || serviceCalculationDates.Count <= 0) && (model.FilingTypeID == 4 || model.FilingTypeID == 5))
                {
                    BindCalculationDateList(model);
                }

                int result = new Service().Insert(model);
                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, actionType = actionType }, JsonRequestBehavior.AllowGet);
        }

        private void ValidateDates(ServiceModel model)
        {
            if (model.EffectiveTo < model.EffectiveFrom.AddYears(1).AddDays(-1))
            {
                throw new Exception(ArtSolutions.MUN.Web.Resources.Service.CompareEffectiveDatesValidationMsg);
            }
        }

        [HttpGet]
        public ActionResult Edit(int serviceID)
        {
            try
            {
                ServiceModel model = new Service().Edit(serviceID);
                if (model.IsLocked)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Service.ServiceLockedMsg;
                    return RedirectToAction("List");
                }
                else if (model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Service.InvalidServiceNumber;
                    return RedirectToAction("List");
                }
                return View("~/Areas/Services/Views/Service/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Edit(ServiceModel model)
        {
            try
            {
                model.StartDate = DateTime.Parse(Request["hdnFormattedStartDate"]);
                model.EndDate = DateTime.Parse(Request["hdnFormattedEndDate"]);
                //ValidateDates(model);

                if (model.StartDate != DateTime.MinValue)
                    model.EndDate = model.StartDate.AddYears(1).AddDays(-1);
                var serviceCalculationDates = new JavaScriptSerializer().Deserialize<List<ServiceCalculationDateModel>>(model.ServiceCalculationDateListJson);
              
                if ((serviceCalculationDates == null || serviceCalculationDates.Count <= 0) && (model.FilingTypeID == 4 || model.FilingTypeID == 5))
                {
                    BindCalculationDateList(model);
                }
               
                int result = new Service().Update(model);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ServiceCollectionRule

        [HttpGet]
        public PartialViewResult AddRule()
        {
            ServiceCollectionRuleModel model = new ServiceCollectionRuleModel();
            model.SetInitialDropDown();
            return PartialView("~/Areas/Services/Views/Service/_ServiceCollectionRuleAdd.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult EditRule(ServiceCollectionRuleModel collectionRule)
        {
            collectionRule.SetInitialDropDown();
            return PartialView("~/Areas/Services/Views/Service/_ServiceCollectionRuleAdd.cshtml", collectionRule);
        }
        public PartialViewResult AddRuleDetailrow()
        {
            return PartialView("~/Areas/Services/Views/Service/_ServiceCollectionRuleDetailRow.cshtml", new ServiceCollectionRuleDetailModel());
        }

        public PartialViewResult AddRuleDetailrowForExceed()
        {
            return PartialView("~/Areas/Services/Views/Service/_ServiceCollectionRuleDetailRowForExceed.cshtml", new ServiceCollectionRuleDetailModel());
        }

        [HttpPost]
        public ActionResult AddRule(List<ServiceCollectionRuleModel> collectionRuleList)
        {
            string EffectiveApplyTo = DateTime.Now.AddDays(-1).ToString();

            try
            {
                //collectionRuleList.ForEach(d =>
                //    {
                //        if (d.IsEffectiveApply == true)
                //        {
                //            d.EffectiveTo = DateTime.Now.AddDays(-1).ToString();
                //        }
                //    });

                //test
                collectionRuleList.ForEach(d =>
                {
                    if (d.IsEffectiveApply == true)
                    {
                        ////test
                        //string test = d.Name;
                        ////

                        //d.EffectiveTo = DateTime.Parse(collectionRuleList.Where(x => x.CollectionTypeID == d.CollectionTypeID && x.IsActive == true).FirstOrDefault().EffectiveFrom).AddDays(-1).ToString("d");
                        //EffectiveApplyTo = DateTime.Parse(collectionRuleList.Where(x => x.CollectionTypeID == d.CollectionTypeID && x.IsActive == true).FirstOrDefault().EffectiveFrom).AddDays(-1).ToString();

                        d.EffectiveTo = DateTime.Parse(collectionRuleList.Where(x => x.CollectionTypeID == d.CollectionTypeID && x.IsActive == true).OrderByDescending(x => x.ID).FirstOrDefault().EffectiveFrom).AddDays(-1).ToString("d");
                        EffectiveApplyTo = DateTime.Parse(collectionRuleList.Where(x => x.CollectionTypeID == d.CollectionTypeID && x.IsActive == true).OrderByDescending(x => x.ID).FirstOrDefault().EffectiveFrom).AddDays(-1).ToString();

                    }
                    //if (d.IsNew == false)
                    //{
                    //    d.EffectiveFrom = DateTime.Parse(d.EffectiveFrom).ToString("d");
                    //    d.EffectiveTo = DateTime.Parse(d.EffectiveTo).ToString("d");
                    //}
                });
                //

                htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCollectionRuleList.cshtml", collectionRuleList);
                status = true;
            }
            catch (Exception ex)
            {

                status = false;
                msg = ex.Message;
            }
            //return Json(new { status = status, message = msg, returnData = htmlstring, EffectiveTo = DateTime.Now.AddDays(-1).ToString() }, JsonRequestBehavior.AllowGet);
            return Json(new { status = status, message = msg, returnData = htmlstring, EffectiveTo = EffectiveApplyTo }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public PartialViewResult CopyRule(ServiceCollectionRuleModel collectionRule, bool isViewMode)
        {
            ViewBag.CopyRuleID = collectionRule.ID;

            if (!isViewMode)
            {
                collectionRule.ID = 0;
                collectionRule.Name = collectionRule.Name;
                collectionRule.IsNew = collectionRule.IsActive = true;
                //collectionRule.EffectiveFrom = DateTime.Now.ToString();
                collectionRule.EffectiveFrom = new DateTime(Convert.ToDateTime(collectionRule.EffectiveTo).Year, Convert.ToDateTime(collectionRule.EffectiveTo).Month, Convert.ToDateTime(collectionRule.EffectiveTo).Day).AddDays(1).ToString("d");
                collectionRule.EffectiveTo = null;
                collectionRule.ServiceCollectionRuleDetailList = collectionRule.ServiceCollectionRuleDetailList.Select(c => { c.ID = 0; return c; }).ToList();
            }
            collectionRule.SetInitialDropDown();
            return PartialView("~/Areas/Services/Views/Service/_ServiceCollectionRuleAdd.cshtml", collectionRule);
        }
        #endregion

        #region Service Calculation Dates
        [HttpPost]
        public ActionResult AddCalculationDates(ServiceModel service, int dateType)
        {
            try
            {
                ValidateDates(service);
                BindCalculationDateList(service);
                ViewBag.DateType = dateType;
                if (dateType == (int)EnDateTypes.FillingDueDates)
                {
                    ViewBag.DueType = service.FilingDueType;
                }
                else if (dateType == (int)EnDateTypes.PaymentDueDates)
                {
                    ViewBag.DueType = service.PaymentDueType;
                }
                else if (dateType == (int)EnDateTypes.DiscountDates)
                {
                    ViewBag.DueType = service.DiscountDueType;
                }
                else if (dateType == (int)EnDateTypes.PaymentGraceDates)
                {
                    ViewBag.DueType = service.PaymentGraceType;
                }
                status = true;
                htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCalculationDatesAdd.cshtml", service);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            JsonResult jr = Json(new { status = status, message = msg, returnData = htmlstring, datesList = service.ServiceCalculationDateList }, JsonRequestBehavior.AllowGet);
            jr.MaxJsonLength = Int32.MaxValue;
            return jr;
        }

        [HttpPost]
        public ActionResult AddCalculationDueDates(ServiceModel service, int dateType, int dueType)
        {
            try
            {
                ValidateDates(service);
                BindCalculationDateList(service);
                if (dateType == (int)EnDateTypes.FillingDueDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.FillingDueDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.PaymentDueDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.PaymentDueDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.DiscountDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.DiscountDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.PaymentGraceDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.PaymentGraceDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                ViewBag.DateType = dateType;
                ViewBag.DueType = dueType;
                status = true;
                if (dueType == (int)EnDueTypes.FixDate)
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCalculationDueDatesAdd.cshtml", service);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            JsonResult jr = Json(new { status = status, message = msg, returnData = htmlstring, datesList = service.ServiceCalculationDateList }, JsonRequestBehavior.AllowGet);
            jr.MaxJsonLength = Int32.MaxValue;
            return jr;
        }

        [HttpPost]
        public ActionResult EditCalculationDates(ServiceModel service, int dateType)
        {
            try
            {
                ViewBag.DateType = dateType;
                if (dateType == (int)EnDateTypes.FillingDueDates)
                {
                    ViewBag.DueType = service.FilingDueType;
                }
                else if (dateType == (int)EnDateTypes.PaymentDueDates)
                {
                    ViewBag.DueType = service.PaymentDueType;
                }
                else if (dateType == (int)EnDateTypes.DiscountDates)
                {
                    ViewBag.DueType = service.DiscountDueType;
                }
                else if (dateType == (int)EnDateTypes.PaymentGraceDates)
                {
                    ViewBag.DueType = service.PaymentGraceType;
                }
                status = true;
                htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCalculationDatesEdit.cshtml", service);
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            JsonResult jsonResult = Json(new { status = status, message = msg, returnData = htmlstring, datesList = service.ServiceCalculationDateList, datesList_append = service.ServiceCalculationDateList_Append }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult EditCalculationDueDates(ServiceModel service, int dateType, int dueType)
        {
            try
            {
                BindCalculationDateList(service);
                if (dateType == (int)EnDateTypes.FillingDueDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.FillingDueDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.PaymentDueDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.PaymentDueDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.DiscountDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.DiscountDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                else if (dateType == (int)EnDateTypes.PaymentGraceDates)
                {
                    service.ServiceCalculationDateList.ForEach(d =>
                    {
                        d.PaymentGraceDate = dueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : dueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
                    });
                }
                ViewBag.DateType = dateType;
                ViewBag.DueType = dueType;
                status = true;
                htmlstring = "";
                if (dueType == (int)EnDueTypes.FixDate)
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCalculationDueDatesEdit.cshtml", service);
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            JsonResult jsonResult = Json(new { status = status, message = msg, returnData = htmlstring, datesList = service.ServiceCalculationDateList, datesList_append = service.ServiceCalculationDateList_Append }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        public ActionResult RegenerateCalculationDateList(ServiceModel service)
        {

            BindCalculationDateList(service);

            //// New Code (CO-1349)
            //if (service.ServiceCalculationDateList_Append.Count == 0)
            //    service.ServiceCalculationDateList_Append = service.ServiceCalculationDateList;
            ////

            service.ServiceCalculationDateList_Append.ForEach(d =>
        {
            d.FillingDueDate = service.FilingDueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : service.FilingDueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
            d.PaymentDueDate = service.PaymentDueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : service.PaymentDueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
            d.DiscountDate = service.DiscountDueType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : service.DiscountDueType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
            d.PaymentGraceDate = service.PaymentGraceType == (int)EnDueTypes.StartPeriodDate ? d.CurrentPeriodStartDate : service.PaymentGraceType == (int)EnDueTypes.EndPeriodDate ? d.CurrentPeriodEndDate : null;
        });

            status = true;
            JsonResult jr = Json(new { status = status, datesList = service.ServiceCalculationDateList, datesList_append = service.ServiceCalculationDateList_Append }, JsonRequestBehavior.AllowGet);
            jr.MaxJsonLength = Int32.MaxValue;
            return jr;
        }

        private void BindCalculationDateList(ServiceModel service)
        {
            if (service.EffectiveTo < service.EffectiveTo_Original)
                new ServiceCalculationDate().RemoveCalculationDates_OutOfEffectiveDateRange(service);
            if (service.ServiceCalculationDateList.Count == 0)
                new ServiceCalculationDate().GenerateCalculationDateTable(service);
            if (service.ServiceCalculationDateList_Append.Count == 0)
                new ServiceCalculationDate().GenerateCalculationDateTable_Append(service);
        }

        [HttpPost]
        public ActionResult ViewCalculationDates(ServiceModel model, int dateType)
        {
            try
            {
                ViewBag.DateType = dateType;
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/Service/_ServiceCalculationDatesView.cshtml", model) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Service Exception
        [HttpPost]
        public ActionResult GetException(ServiceExceptionViewModel model, bool? readonlyMode = false)
        {
            try
            {
                if (model.ServiceExceptionList == null)
                    model.ServiceExceptionList = new List<ServiceExceptionModel>();
                if (readonlyMode == true)
                    ViewBag.ViewMode = true;
                return PartialView("~/Areas/Services/Views/Service/_ServiceExceptionAddEdit.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddExceptionDetailrow(ServiceExceptionModel model)
        {
            try
            {
                return PartialView("~/Areas/Services/Views/Service/_ServiceExceptionDetailRow.cshtml", model);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ajax Calls
        public JsonResult GetServiceTypesByGroup(int groupId)
        {
            return Json(new Service().GetServiceTypeListByGroupId(groupId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceByServiceTypes(string serviceTypeIds)
        {
            return Json(new Service().GetServiceListListByServiceTypeId(serviceTypeIds), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsServiceExist(string Name, int Id)
        {
            try
            {
                return Json(!new Service().IsServiceExist(Name, Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
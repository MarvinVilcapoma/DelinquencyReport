using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Reports.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult Report()
        {
            Server.MapPath("~/Content/printStyle.css");
            return View("~/Areas/Reports/Views/Report/Report.cshtml");
        }

        #region Account

        #region Account Statement
        [HttpGet]
        public ActionResult AccountStatement(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Account/AccountStatement.cshtml", new AccountStatement().Get(accountID));
        }

        [HttpPost]
        public ActionResult AccountStatement(JQDTParams jqdtParams, int accountId, int? accountPropertyID, int? tillYear, int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport)
        {
            JsonResult jResult = new JsonResult();
            AccountStatementModel model = new AccountStatementModel();
            try
            {
                model = new AccountStatement().GetExportLayout(accountId, accountPropertyID, tillYear, tillPeriod, tillDate, accountServiceCollectionDetailIDs, isExport);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AccountStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring, accountPaymentPlanNames = model.AccountPaymentPlanNames });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAccountStatement(AccountStatementModel model, int exportType)
        {
            //ViewBag.exportType = exportType;
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountStatementTemplate.cshtml",
                new AccountStatement().GetExportLayout(model.AccountId, model.AccountPropertyID, model.Year, model.Period, model.Date, model.AccountServiceCollectionDetailIDs, true));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AccountStatementTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintAccountStatement(int accountId, int? accountPropertyID, int? tillYear, int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                //ViewBag.exportType = 2;
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountStatementTemplate.cshtml",
                    new AccountStatement().GetExportLayout(accountId, accountPropertyID, tillYear, tillPeriod, tillDate, accountServiceCollectionDetailIDs, isExport));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Summary Account Statement
        [HttpGet]
        public ActionResult SummaryAccountStatement()
        {
            return View("~/Areas/Reports/Views/Report/Account/SummaryAccountStatement.cshtml", new SummaryAccountStatement().Get());
        }

        [HttpPost]
        public ActionResult SummaryAccountStatement(JQDTParams jqdtParams, int accountId, int? accountPropertyID, string commaSeperatedYearIDs)
        {
            JsonResult jResult = new JsonResult();
            SummaryAccountStatementModel model = new SummaryAccountStatementModel();
            try
            {
                model = new SummaryAccountStatement().GetExportLayout(accountId, accountPropertyID, commaSeperatedYearIDs);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_SummaryAccountStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportSummaryAccountStatement(SummaryAccountStatementModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_SummaryAccountStatementTemplate.cshtml",
                new SummaryAccountStatement().GetExportLayout(model.AccountId, model.AccountPropertyID, model.FilterYearID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.SummaryAccountStatementTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintSummaryAccountStatement(int accountId, int? accountPropertyID, string commaSeperatedYearIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_SummaryAccountStatementTemplate.cshtml",
                    new SummaryAccountStatement().GetExportLayout(accountId, accountPropertyID, commaSeperatedYearIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region NOTICE OF ADMINISTRATIVE COLLECTION
        [HttpGet]
        public ActionResult AdministrativeCollectionNotice()
        {
            return View("~/Areas/Reports/Views/Report/Account/AdministrativeCollectionNotice.cshtml", new AdministrativeCollectionNotice().Get());
        }

        [HttpPost]
        public ActionResult AdministrativeCollectionNotice(JQDTParams jqdtParams, int accountId, DateTime? notificationExpirationDate, string representativesName)
        {
            JsonResult jResult = new JsonResult();
            AdministrativeCollectionNoticeModel model = new AdministrativeCollectionNoticeModel();
            try
            {
                model = new AdministrativeCollectionNotice().GetExportLayout(accountId, true, notificationExpirationDate, null, representativesName);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AdministrativeCollectionNoticeDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAdministrativeCollectionNotice(AdministrativeCollectionNoticeModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AdministrativeCollectionNoticeTemplate.cshtml",
                new AdministrativeCollectionNotice().GetExportLayout(model.AccountId, true, model.NotificationExpirationDate, null, model.RepresentativesName));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileLetter((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AdministrativeCollectionFirstNoticeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintAdministrativeCollectionNotice(int accountId, DateTime? notificationExpirationDate, string representativesName)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AdministrativeCollectionNoticeTemplate.cshtml",
                    new AdministrativeCollectionNotice().GetExportLayout(accountId, true, notificationExpirationDate, null, representativesName));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SECOND NOTICE OF ADMINISTRATIVE COLLECTION
        [HttpGet]
        public ActionResult AdministrativeCollectionSecondNotice()
        {
            return View("~/Areas/Reports/Views/Report/Account/AdministrativeCollectionSecondNotice.cshtml", new AdministrativeCollectionNotice().Get());
        }

        [HttpPost]
        public ActionResult AdministrativeCollectionSecondNotice(JQDTParams jqdtParams, int accountId, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            JsonResult jResult = new JsonResult();
            AdministrativeCollectionNoticeModel model = new AdministrativeCollectionNoticeModel();
            try
            {
                model = new AdministrativeCollectionNotice().GetExportLayout(accountId, false, notificationExpirationDate, notificationDate, representativesName);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AdministrativeCollectionSecondNoticeDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAdministrativeCollectionSecondNotice(AdministrativeCollectionNoticeModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AdministrativeCollectionSecondNoticeTemplate.cshtml",
                new AdministrativeCollectionNotice().GetExportLayout(model.AccountId, false, model.NotificationExpirationDate, model.NotificationDate, model.RepresentativesName));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AdministrativeCollectionFirstNoticeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintAdministrativeCollectionSecondNotice(int accountId, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AdministrativeCollectionSecondNoticeTemplate.cshtml",
                    new AdministrativeCollectionNotice().GetExportLayout(accountId, false, notificationExpirationDate, notificationDate, representativesName));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Payment Plan By Taxpayer
        [HttpGet]
        public ActionResult PaymentPlanByTaxpayer(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Account/PaymentPlanByTaxpayer.cshtml", new PaymentPlanByTaxpayer().Get(accountID));
        }

        [HttpPost]
        public ActionResult PaymentPlanByTaxpayer(JQDTParams jqdtParams, int accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            JsonResult jResult = new JsonResult();
            PaymentPlanByTaxpayerModel model = new PaymentPlanByTaxpayerModel();
            try
            {
                ViewBag.IsTermDetail = isTermDetail;
                model = new PaymentPlanByTaxpayer().GetExportLayout(accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_PaymentPlanByTaxpayerDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring, minMOROSAS = model.MinMOROSAS, minAPager = model.MinAPager });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message, minMOROSAS = model.MinMOROSAS, minAPager = model.MinAPager });
            }
            return jResult;
        }

        [HttpPost]
        public ActionResult PaymentPlanByTaxpayerTermDetail(JQDTParams jqdtParams, int accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            JsonResult jResult = new JsonResult();
            PaymentPlanByTaxpayerModel model = new PaymentPlanByTaxpayerModel();
            try
            {
                ViewBag.IsTermDetail = isTermDetail;
                model = new PaymentPlanByTaxpayer().GetExportLayout(accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_PaymentPlanByTaxpayerTermDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportPaymentPlanByTaxpayer(PaymentPlanByTaxpayerModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_PaymentPlanByTaxpayerTemplate.cshtml",
                new PaymentPlanByTaxpayer().GetExportLayout(model.AccountId, model.AccountPaymentPlanIDs, model.Year, model.Date, model.QuotasToCalculate, model.RowNos, null));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.PaymentPlanByTaxpayer, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintPaymentPlanByTaxpayer(int accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_PaymentPlanByTaxpayerTemplate.cshtml",
                    new PaymentPlanByTaxpayer().GetExportLayout(accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Services Missing Filing
        [HttpGet]
        public ActionResult AccountServicesMissingFiling()
        {
            return View("~/Areas/Reports/Views/Report/Account/AccountServicesMissingFiling.cshtml", new AccountServicesMissingFiling().Get());
        }

        [HttpPost]
        public ActionResult AccountServicesMissingFiling(int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            AccountServicesMissingFilingModel model = new AccountServicesMissingFilingModel();
            try
            {
                model = new AccountServicesMissingFiling().GetList(accountId, commaSeperatedServiceIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AccountServicesMissingFilingDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AccountServicesMissingFilingDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.AccountServicesMissingFilingList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAccountServicesMissingFiling(AccountServicesMissingFilingModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountServicesMissingFilingTemplate.cshtml",
                new AccountServicesMissingFiling().GetList(model.AccountId, model.FilterServiceIDs, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AccountServicesMissingFiling, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion

        #region General Movements
        public ActionResult GeneralMovements()
        {
            return View("~/Areas/Reports/Views/Report/Account/GeneralMovements.cshtml", new GeneralMovements().Get());
        }

        [HttpPost]
        public ActionResult GeneralMovements(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            GeneralMovementsModel model = new GeneralMovementsModel();
            try
            {
                model = new GeneralMovements().GetList(userID, accountID, fromDate, toDate, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_GeneralMovementsDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_GeneralMovementsDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.GeneralMovementsList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportGeneralMovements(GeneralMovementsModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_GeneralMovementsTemplate.cshtml",
               new GeneralMovements().GetList(model.UserID, model.AccountId, model.FromDate, model.ToDate, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.GeneralMovementsReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintGeneralMovements(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_GeneralMovementsTemplate.cshtml",
                    new GeneralMovements().GetList(userID, accountID, fromDate, toDate, 1, Int32.MaxValue));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account By Service Type
        [HttpGet]
        public ActionResult AccountByServiceType()
        {
            return View("~/Areas/Reports/Views/Report/Account/AccountByServiceType.cshtml", new AccountByServiceType().Get());
        }
        [HttpPost]
        public ActionResult AccountByServiceType(int serviceTypeID, bool isNotAssignServices)
        {
            AccountByServiceTypeModel model = new AccountByServiceTypeModel();
            try
            {
                string htmlstring = string.Empty;
                model = new AccountByServiceType().GetExportLayout(serviceTypeID, isNotAssignServices);
                if (!isNotAssignServices)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AccountByServiceTypeDetail.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_AccountNotAssignByServiceType.cshtml", model);
                }
                return Json(new { status = true, data = htmlstring, accountPaymentPlanNames = model });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = ex.Message });
            }

        }
        [HttpPost]
        public FileResult ExportAccountByServiceType(AccountByServiceTypeModel model, int exportType)
        {
            string htmlString = string.Empty;
            if (!model.isNotAssignServices)
            {
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountByServiceTypeTemplate.cshtml",
                    new AccountByServiceType().GetExportLayout(model.ServiceTypeID, model.isNotAssignServices));
            }
            else
            {
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountNotAssignByServiceTypeTemplate.cshtml",
                    new AccountByServiceType().GetExportLayout(model.ServiceTypeID, model.isNotAssignServices));
            }
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AccountByServiceTypeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintAccountByServiceType(AccountByServiceTypeModel model)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (!model.isNotAssignServices)
                {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountByServiceTypeTemplate.cshtml",
                    new AccountByServiceType().GetExportLayout(model.ServiceTypeID, true));
                }
                else
                {
                    msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_AccountNotAssignByServiceTypeTemplate.cshtml",
                    new AccountByServiceType().GetExportLayout(model.ServiceTypeID, true));
                }
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DelinquencyReport

        public ActionResult DeliquencyReport()
        {
            return View("~/Areas/Reports/Views/Report/Account/DelinquencyReport.cshtml", new DelinquencyReport().Get());
        }

        #endregion
        #endregion

        #region Certificate

        #region SalesTax Debit

        [HttpGet]
        public ActionResult SalesTaxDebitStatement()
        {
            return View("~/Areas/Reports/Views/Report/Certification/SalesTaxDebitStatement.cshtml", new IVUStatement().Get());
        }

        [HttpPost]
        public ActionResult SalesTaxDebitStatement(JQDTParams jqdtParams, int accountId, decimal? balanceFrom, decimal? balanceTo, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUStatementModel model = new IVUStatement().GetByPaging(jqdtParams, accountId, balanceFrom, balanceTo, futureDate);
                model.FutureDate = futureDate;
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.IVUStatementList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public ActionResult SalesTaxDebtDetailGet(int accountId, decimal? balanceFrom, decimal? balanceTo, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUStatementModel model = new IVUStatementModel();
                model = new IVUStatement().GetAccountDetail(accountId, balanceFrom ?? null, balanceTo ?? null);
                model.FutureDate = futureDate;
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_SalesTaxDebitStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        #endregion

        #region SalesTaxFillingCertificate

        [HttpGet]
        public ActionResult SalesTaxFillingCertificate()
        {
            return View("~/Areas/Reports/Views/Report/Certification/SalesTaxFillingCertificate.cshtml", new IVUFilledCertificate().Get());
        }

        [HttpPost]
        public ActionResult SalesTaxFillingCertificate(JQDTParams jqdtParams, int accountId, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUFilledCertificateModel model = new IVUFilledCertificate().GetByPaging(jqdtParams, accountId, futureDate);
                model.FutureDate = futureDate;
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.IVUFilledCertificateList, companyModel = model.ReportCompanyModel });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public ActionResult SalesTaxFillingDetailGet(int accountId, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUFilledCertificateModel model = new IVUFilledCertificateModel();
                model = new IVUFilledCertificate().GetAccountDetail(accountId);
                model.FutureDate = futureDate;
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_SalesTaxFillingCertificateDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpGet]
        public JsonResult PrintSalesTaxFilingCertification(int accountId, decimal? balanceFrom, decimal? balanceTo, DateTime futureDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_SalesTaxFilingCertificationTemplate.cshtml", new IVUFilledCertificate().GetExportLayout(accountId, futureDate));
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BusinessLicense Debit

        [HttpGet]
        public ActionResult BusinessLicenseDebitStatement()
        {
            return View("~/Areas/Reports/Views/Report/Certification/BusinessLicenseDebitStatement.cshtml", new BusinessLicenseCertificate().Get());
        }

        [HttpPost]
        public ActionResult BusinessLicenseDebitStatement(JQDTParams jqdtParams, int accountId, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                BusinessLicenseCertificateModel model = new BusinessLicenseCertificate().GetByPaging(jqdtParams, accountId, futureDate);
                model.FutureDate = futureDate;
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.BusinessLicenseCertificateModelList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public ActionResult BusinessLicenseDebtDetailGet(int accountId, decimal? balanceFrom, decimal? balanceTo, DateTime futureDate)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                BusinessLicenseCertificateModel model = new BusinessLicenseCertificateModel();
                model = new BusinessLicenseCertificate().GetAccountDetail(accountId);
                model.FutureDate = futureDate;
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_BusinessLicenseDebitStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpGet]
        public JsonResult PrintBusinessLicenseDebtDetail(int accountId, decimal? balanceFrom, decimal? balanceTo, string statementType, DateTime futureDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_BusinessLicenseDebitStatementTemplate.cshtml", new BusinessLicenseCertificate().GetExportLayout(accountId, futureDate));
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Debt Certification
        [HttpGet]
        public ActionResult DebtCertification()
        {
            return View("~/Areas/Reports/Views/Report/Certification/DebtCertification.cshtml", new DebtCertification().Get());
        }

        [HttpPost]
        public ActionResult DebtCertification(JQDTParams jqdtParams, int accountId)
        {
            JsonResult jResult = new JsonResult();
            DebtCertificationModel model = new DebtCertificationModel();
            try
            {
                model = new DebtCertification().GetExportLayout(accountId);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_DebtCertificationDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportDebtCertification(DebtCertificationModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_DebtCertificationTemplate.cshtml",
                new DebtCertification().GetExportLayout(model.AccountId));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DebtCertificationTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintDebtCertification(int accountId)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_DebtCertificationTemplate.cshtml",
                    new DebtCertification().GetExportLayout(accountId));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region No Debt Certification
        [HttpGet]
        public ActionResult NoDebtCertification()
        {
            return View("~/Areas/Reports/Views/Report/Certification/NoDebtCertification.cshtml", new NoDebtCertification().Get());
        }

        [HttpPost]
        public ActionResult NoDebtCertification(JQDTParams jqdtParams, int accountId, string note)
        {
            JsonResult jResult = new JsonResult();
            NoDebtCertificationModel model = new NoDebtCertificationModel();
            try
            {
                model = new NoDebtCertification().GetExportLayout(accountId, note);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_NoDebtCertificationDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportNoDebtCertification(NoDebtCertificationModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_NoDebtCertificationTemplate.cshtml",
                new NoDebtCertification().GetExportLayout(model.AccountId, model.Note,model.Observations));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileLetter((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.NoDebtCertification, htmlString);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintNoDebtCertification(NoDebtCertificationModel model)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_NoDebtCertificationTemplate.cshtml",
                    new NoDebtCertification().GetExportLayout(model.AccountId, model.Note,model.Observations));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Constancy Report
        [HttpGet]
        public ActionResult Constancy()
        {
            return View("~/Areas/Reports/Views/Report/Certification/Constancy.cshtml", new NoDebtCertification().Get());
        }

        [HttpPost]
        public ActionResult Constancy(JQDTParams jqdtParams, string taxNumber, string note)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            NoDebtCertificationModel model = new NoDebtCertificationModel();
            try
            {
                int? accountId = null;
                var accountModel = new Accounts.Models.AccountModel().Get(null, null, null, taxNumber.Trim(), null, null, true).Where(x => x.TaxNumber.Trim() == taxNumber.Trim()).FirstOrDefault();
                if (accountModel != null)
                {
                    accountId = accountModel.ID;
                }

                if (accountId > 0)
                {
                    model = new NoDebtCertification().GetExportLayout(accountId.Value, note);
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_NoDebtCertificationDetail.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_ConstancyDetail.cshtml", model);
                }

                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportConstancy(NoDebtCertificationModel model, int exportType)
        {
            int? accountId = null;
            NoDebtCertificationModel debtCertificationModel = new NoDebtCertification().Get();

            var accountModel = new Accounts.Models.AccountModel().Get(null, null, null, model.TaxNumber.Trim(), null, null, true).Where(x => x.TaxNumber.Trim() == model.TaxNumber.Trim()).FirstOrDefault();
            if (accountModel != null)
            {
                accountId = accountModel.ID;
            }

            if (accountId > 0)
            {
                debtCertificationModel = new NoDebtCertification().GetExportLayout(accountId.Value, model.Note);
                debtCertificationModel.AccountId = accountId.Value;
            }
            else
            {
                debtCertificationModel.UserName = model.UserName;
                debtCertificationModel.UserID = model.UserID;
                debtCertificationModel.Description1Detail = model.Description1Detail;
                debtCertificationModel.Description2Detail = model.Description2Detail;
            }

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_ConstancyTemplate.cshtml", debtCertificationModel);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileLetter((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.CONSTANCY, htmlString);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintConstancy(NoDebtCertificationModel model)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                int? accountId = null;
                NoDebtCertificationModel debtCertificationModel = new NoDebtCertification().Get();

                var accountModel = new Accounts.Models.AccountModel().Get(null, null, null, model.TaxNumber.Trim(), null, null, true).Where(x => x.TaxNumber.Trim() == model.TaxNumber.Trim()).FirstOrDefault();
                if (accountModel != null)
                {
                    accountId = accountModel.ID;
                }

                if (accountId > 0)
                {
                    debtCertificationModel = new NoDebtCertification().GetExportLayout(accountId.Value, model.Note);
                    debtCertificationModel.AccountId = accountId.Value;
                }
                else
                {
                    debtCertificationModel.UserName = model.UserName;
                    debtCertificationModel.UserID = model.UserID;
                    debtCertificationModel.Description1Detail = model.Description1Detail;
                    debtCertificationModel.Description2Detail = model.Description2Detail;
                }
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_ConstancyTemplate.cshtml", debtCertificationModel);
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Projected Account Statement
        [HttpGet]
        public ActionResult ProjectedAccountStatement(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Certification/ProjectedAccountStatement.cshtml", new ProjectedAccountStatement().Get(accountID));
        }

        [HttpPost]
        public ActionResult ProjectedAccountStatement(JQDTParams jqdtParams, int accountId, DateTime? tillDate)
        {
            JsonResult jResult = new JsonResult();
            ProjectedAccountStatementModel model = new ProjectedAccountStatementModel();
            try
            {
                model = new ProjectedAccountStatement().GetExportLayout(accountId, tillDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/_ProjectedAccountStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportProjectedAccountStatement(ProjectedAccountStatementModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_ProjectedAccountStatementTemplate.cshtml",
                new ProjectedAccountStatement().GetExportLayout(model.AccountId, model.Date));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ProjectedAccountStatement, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintProjectedAccountStatement(int accountId, DateTime? tillDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Certification/ExportTemplate/_ProjectedAccountStatementTemplate.cshtml",
                    new ProjectedAccountStatement().GetExportLayout(accountId, tillDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region IVU

        #region IVU BalanceSheet
        [HttpGet]
        public ActionResult IVUBalanceSheet()
        {
            return View("~/Areas/Reports/Views/Report/IVU/IVUBalanceSheet.cshtml", new IVUBalanceSheet().Get());
        }

        [HttpPost]
        public ActionResult IVUBalanceSheet(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            IVUBalanceSheetModel model = new IVUBalanceSheetModel();
            try
            {
                model = new IVUBalanceSheet().Get(startPeriod, endPeriod, accountIDs, balanceFrom, balanceTo, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.IVUBalanceSheetList });
        }

        [HttpPost]
        public FileResult ExportIVUBalanceSheet(IVUBalanceSheetModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUBalanceSheetTemplate.cshtml",
                new IVUBalanceSheet().GetExportLayout(model.StartPeriod, model.EndPeriod
                , model.FilterAccountID
                , model.BalanceFrom, model.BalanceTo));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.IVUBalanceSheetTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintIVUBalanceSheet(DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUBalanceSheetTemplate.cshtml",
                    new IVUBalanceSheet().GetExportLayout(startPeriod, endPeriod, accountIDs, balanceFrom, balanceTo));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IVU Forms Not Filed
        [HttpGet]
        public ActionResult IVUFormsNotFiled()
        {
            return View("~/Areas/Reports/Views/Report/IVU/IVUFormsNotFiled.cshtml", new IVUFormsNotFiled().Get());
        }

        [HttpPost]
        public ActionResult IVUFormsNotFiled(JQDTParams jqdtParams, string accountIDs, DateTime? since, DateTime? till)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            IVUFormsNotFiledModel model = new IVUFormsNotFiledModel();
            try
            {
                model = new IVUFormsNotFiled().Get(accountIDs, since, till, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.IVUFormsNotFiledList });
        }

        [HttpPost]
        public FileResult ExportIVUFormsNotFiled(IVUFormsNotFiledModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUFormsNotFiledTemplate.cshtml",
                new IVUFormsNotFiled().GetExportLayout((model.AccountIDs == null ? null : string.Join(",", model.AccountIDs)), model.Since, model.Till));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.IVUFormsNotFiledTitle, htmlString, false);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintIVUFormsNotFiled(string accountIDs, DateTime? Since, DateTime? Till)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUFormsNotFiledTemplate.cshtml",
                    new IVUFormsNotFiled().GetExportLayout(accountIDs, Since, Till));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IVU Statement

        [HttpGet]
        public ActionResult IVUStatement()
        {
            return View("~/Areas/Reports/Views/Report/IVU/IVUStatement.cshtml", new IVUStatement().Get());
        }

        [HttpPost]
        public ActionResult IVUStatement(JQDTParams jqdtParams, int accountId, decimal? balanceFrom, decimal? balanceTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUStatementModel model = new IVUStatement().GetByPaging(jqdtParams, accountId, balanceFrom, balanceTo);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.IVUStatementList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        [HttpPost]
        public ActionResult AccountDetailGet(int accountId, decimal? balanceFrom, decimal? balanceTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                IVUStatementModel model = new IVUStatementModel();
                new IVUStatement().GetAccountDetail(accountId, balanceFrom ?? null, balanceTo ?? null);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/_IVUStatementDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportIVUStatement(IVUStatementModel model, int exportType, string statementType = null)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUStatementTemplate.cshtml", new IVUStatement().GetExportLayout(model.AccountId.Value, model.BalanceFrom, model.BalanceTo, statementType));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.IVUStatementTitle, htmlString, false);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintIVUStatement(int accountId, decimal? balanceFrom, decimal? balanceTo, string statementType, DateTime? futureDate = null)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUStatementTemplate.cshtml", new IVUStatement().GetExportLayout(accountId, balanceFrom, balanceTo, statementType, futureDate));
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IVU Collection Detail

        [HttpGet]
        public ActionResult IVUCollectionDetail()
        {
            return View("~/Areas/Reports/Views/Report/IVU/IVUCollectionDetail.cshtml", new IVUCollectionDetail().Get());
        }

        [HttpPost]
        public JsonResult IVUCollectionDetail(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate)
        {
            bool status = true;
            IVUCollectionDetailModel model = new IVUCollectionDetailModel();
            try
            {
                model = new IVUCollectionDetail().Get(jqdtParams, startPeriodDate, endPeriodDate);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, recordsTotal = model.TotalRecord, filingonTotal = model.IVUCollectionDetailList.Count, data = model.IVUCollectionDetailList });
        }

        [HttpPost]
        public FileResult ExportIVUCollectionDetail(IVUCollectionDetailModel model, int exportType)
        {
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.IVUCollectionDetailTitle,
                HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUCollectionDetailTemplate.cshtml", new IVUCollectionDetail().GetExportLayout(model.StartPeriodDate, model.EndPeriodDate)), true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintIVUCollectionDetail(DateTime startPeriodDate, DateTime endPeriodDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/IVU/ExportTemplate/_IVUCollectionDetailTemplate.cshtml", new IVUCollectionDetail().GetExportLayout(startPeriodDate, endPeriodDate));
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Collections - Daily Cash Register

        #region Closing Summary

        [HttpGet]
        public ActionResult CollectionClosingSummary()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/ClosingSummary.cshtml", new CollectionClosingSummary().Get());
        }

        [HttpPost]
        public ActionResult CollectionClosingSummary(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, Guid[] cashierIds, decimal? NetClosingFrom, decimal? NetClosingTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionClosingSummaryModel model = new CollectionClosingSummary().Get(jqdtParams, startPeriod, endPeriod, cashierIds, NetClosingFrom, NetClosingTo);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalClosingRecord, recordsTotal = model.TotalClosingRecord, data = model.ClosingEntryList, closingTotal = model.ClosingEntryList.Sum(x => x.NetClosing).ToString("C") });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public ActionResult GetPaymentReceiptHtml(string paymentReceiptListJSON)
        {
            try
            {
                List<PaymentModel> model = JsonConvert.DeserializeObject<List<PaymentModel>>(paymentReceiptListJSON);
                return PartialView("~/Areas/Reports/Views/Report/CashRegister/_PaymentReceiptList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportClosingSummary(CollectionClosingSummaryModel model, int exportType)
        {
            string htmlString = GetClosingSummaryExportLayout(model.StartPeriodDate, model.EndPeriodDate, (string.IsNullOrEmpty(model.FilterCashierID) ? model.CashierIDs : model.FilterCashierID.Split(',').Select(Guid.Parse).ToArray()), model.NetClosingFrom, model.NetClosingTo);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ClosingSummaryTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingSummary(DateTime startPeriod, DateTime endPeriod, Guid[] cashierIds, decimal? NetClosingFrom, decimal? NetClosingTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (NetClosingFrom == 0 && NetClosingTo == 0)
                {
                    NetClosingFrom = null;
                    NetClosingTo = null;
                }
                msg = GetClosingSummaryExportLayout(startPeriod, endPeriod, cashierIds, NetClosingFrom, NetClosingTo);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        public string GetClosingSummaryExportLayout(DateTime startPeriod, DateTime endPeriod, Guid[] cashierIds, decimal? NetClosingFrom, decimal? NetClosingTo)
        {
            CollectionClosingSummaryModel model = new CollectionClosingSummaryModel();
            model = new CollectionClosingSummary().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, cashierIds, NetClosingFrom, NetClosingTo);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_ClosingSummaryTemplate.cshtml", model);
        }
        #endregion

        #region Closing Summary By Service Type

        [HttpGet]
        public ActionResult ClosingSummaryServiceType()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/ClosingSummaryServiceType.cshtml", new ClosingSummaryServiceType().Get());
        }

        [HttpPost]
        public ActionResult ClosingSummaryByServiceType(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string serviceTypeIds, decimal? netClosingFrom, decimal? netClosingTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                ClosingSummaryServiceTypeModel model = new ClosingSummaryServiceType().Get(jqdtParams, startPeriod, endPeriod, serviceTypeIds, netClosingFrom, netClosingTo);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalClosingRecord,
                    recordsTotal = model.TotalClosingRecord,
                    data = model.ClosingEntryList,
                    closingTotal = model.ClosingEntryList.Sum(x => x.NetClosing).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetPaymentReceiptByServiceTypeHtml(string closingListJSON)
        {
            try
            {
                List<PaymentModel> model = JsonConvert.DeserializeObject<List<PaymentModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/CashRegister/_PaymentReceiptByServiceTypeList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportClosingSummaryServiceType(ClosingSummaryServiceTypeModel model, int exportType)
        {
            string htmlString = GetClosingSummaryServiceTypeExportLayout(model.StartPeriodDate.Value, model.EndPeriodDate.Value, (model.FilterServiceTypeID == null ? null : string.Join(",", model.FilterServiceTypeID)), model.NetClosingFrom, model.NetClosingTo);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Global.ClosingSummaryServiceTypeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingSummaryServiceType(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netClosingFrom == 0 && netClosingTo == 0)
                {
                    netClosingFrom = null;
                    netClosingTo = null;
                }
                msg = GetClosingSummaryServiceTypeExportLayout(startPeriod, endPeriod, serviceTypeIDs, netClosingFrom, netClosingTo);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        public string GetClosingSummaryServiceTypeExportLayout(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            ClosingSummaryServiceTypeModel model = new ClosingSummaryServiceTypeModel();
            model = new ClosingSummaryServiceType().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, serviceTypeIDs, netClosingFrom, netClosingTo);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_ClosingSummaryServiceTypeTemplate.cshtml", model);
        }
        #endregion

        #region Closing Summary By Payment Type

        [HttpGet]
        public ActionResult ClosingSummaryByPaymentType()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/ClosingSummaryByPaymentType.cshtml", new ClosingSummaryPaymentType().Get());
        }

        [HttpPost]
        public ActionResult ClosingSummaryByPaymentType(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string paymentTypeIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentType().Get(jqdtParams, startPeriod, endPeriod, netClosingFrom, netClosingTo, paymentTypeIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalClosingRecord,
                    recordsTotal = model.TotalClosingRecord,
                    data = model.ClosingEntryList,
                    closingTotal = model.ClosingEntryList.Sum(x => x.NetClosing).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetPaymentReceiptByPaymentTypeHtml(string closingListJSON)
        {
            try
            {
                List<PaymentModel> model = JsonConvert.DeserializeObject<List<PaymentModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/CashRegister/_PaymentReceiptByPaymentTypeList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportClosingSummaryByPaymentType(ClosingSummaryPaymentTypeModel model, int exportType)
        {
            string htmlString = GetClosingSummaryByPaymentTypeExportLayout(model.StartPeriodDate.Value, model.EndPeriodDate.Value, model.NetClosingFrom, model.NetClosingTo, (model.FilterPaymentTypeID == null ? null : string.Join(",", model.FilterPaymentTypeID)));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Global.ClosingSummaryByPaymentType, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingSummaryByPaymentType(DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string paymentTypeIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netClosingFrom == 0 && netClosingTo == 0)
                {
                    netClosingFrom = null;
                    netClosingTo = null;
                }
                msg = GetClosingSummaryByPaymentTypeExportLayout(startPeriod, endPeriod, netClosingFrom, netClosingTo, paymentTypeIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetClosingSummaryByPaymentTypeExportLayout(DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string paymentTypeIDs)
        {
            ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
            model = new ClosingSummaryPaymentType().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, netClosingFrom, netClosingTo, paymentTypeIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_ClosingSummaryByPaymentTypeTemplate.cshtml", model);
        }
        #endregion

        #region Closing Summary By Invoice

        [HttpGet]
        public ActionResult ClosingSummaryByInvoice()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/ClosingSummaryByInvoice.cshtml", new ClosingSummaryInvoice().Get());
        }

        [HttpPost]
        public ActionResult ClosingSummaryByInvoice(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string OtherServiceIDs, string GrantIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                ClosingSummaryInvoiceModel model = new ClosingSummaryInvoice().Get(jqdtParams, startPeriod, endPeriod, netClosingFrom, netClosingTo, OtherServiceIDs, GrantIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalClosingRecord,
                    recordsTotal = model.TotalClosingRecord,
                    data = model.ClosingEntryList,
                    closingTotal = model.ClosingEntryList.Sum(x => x.NetClosing).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetPaymentReceiptByInvoiceHtml(string closingListJSON)
        {
            try
            {
                List<PaymentModel> model = JsonConvert.DeserializeObject<List<PaymentModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/CashRegister/_PaymentReceiptByInvoiceList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportClosingSummaryByInvoice(ClosingSummaryInvoiceModel model, int exportType)
        {
            string htmlString = GetClosingSummaryByInvoiceExportLayout(model.StartPeriodDate.Value, model.EndPeriodDate.Value, model.NetClosingFrom, model.NetClosingTo, model.FilterOtherServiceID, model.FilterGrantID);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Global.CashRegisterSummaryByInvoice, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingSummaryByInvoice(DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string OtherServiceIDs, string GrantIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netClosingFrom == 0 && netClosingTo == 0)
                {
                    netClosingFrom = null;
                    netClosingTo = null;
                }
                msg = GetClosingSummaryByInvoiceExportLayout(startPeriod, endPeriod, netClosingFrom, netClosingTo, OtherServiceIDs, GrantIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetClosingSummaryByInvoiceExportLayout(DateTime startPeriod, DateTime endPeriod, decimal? netClosingFrom, decimal? netClosingTo, string OtherServiceIDs, string GrantIDs)
        {
            ClosingSummaryInvoiceModel model = new ClosingSummaryInvoiceModel();
            model = new ClosingSummaryInvoice().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, netClosingFrom, netClosingTo, OtherServiceIDs, GrantIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_ClosingSummaryByInvoiceTemplate.cshtml", model);
        }
        #endregion

        #region Closing Summary By Payment Plan

        [HttpGet]
        public ActionResult ClosingSummaryByPaymentPlan()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/ClosingSummaryByPaymentPlan.cshtml", new ClosingSummaryPaymentPlan().Get());
        }

        [HttpPost]
        public ActionResult ClosingSummaryByPaymentPlan(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                ClosingSummaryPaymentPlanModel model = new ClosingSummaryPaymentPlan().Get(jqdtParams, startPeriod, endPeriod, commaSeperatedPaymentPlanIDs, netClosingFrom, netClosingTo);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalClosingRecord,
                    recordsTotal = model.TotalClosingRecord,
                    data = model.ClosingEntryList,
                    closingTotal = model.ClosingEntryList.Sum(x => x.NetClosing).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetPaymentReceiptByPaymentPlanHtml(string closingListJSON)
        {
            try
            {
                List<PaymentModel> model = JsonConvert.DeserializeObject<List<PaymentModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/CashRegister/_PaymentReceiptByPaymentPlanList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportClosingSummaryByPaymentPlan(ClosingSummaryPaymentPlanModel model, int exportType)
        {
            string htmlString = GetClosingSummaryByPaymentPlanExportLayout(model.StartPeriodDate.Value, model.EndPeriodDate.Value, (model.FilterPaymentPlanID == null ? null : string.Join(",", model.FilterPaymentPlanID)), model.NetClosingFrom, model.NetClosingTo);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Global.CashRegisterSummaryByPaymentPlan, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingSummaryByPaymentPlan(DateTime startPeriod, DateTime endPeriod, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netClosingFrom == 0 && netClosingTo == 0)
                {
                    netClosingFrom = null;
                    netClosingTo = null;
                }
                msg = GetClosingSummaryByPaymentPlanExportLayout(startPeriod, endPeriod, commaSeperatedPaymentPlanIDs, netClosingFrom, netClosingTo);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetClosingSummaryByPaymentPlanExportLayout(DateTime startPeriod, DateTime endPeriod, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            ClosingSummaryPaymentPlanModel model = new ClosingSummaryPaymentPlanModel();
            model = new ClosingSummaryPaymentPlan().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, commaSeperatedPaymentPlanIDs, netClosingFrom, netClosingTo);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_ClosingSummaryByPaymentPlanTemplate.cshtml", model);
        }
        #endregion

        #region Cash Receipt Control        
        [HttpGet]
        public ActionResult CashReceiptControl()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/CashReceiptControl.cshtml", new CashReceiptControl().Get());
        }

        [HttpPost]
        public ActionResult CashReceiptControl(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            JsonResult jResult = new JsonResult();
            CashReceiptControlModel model = new CashReceiptControlModel();
            try
            {
                model = new CashReceiptControl().GetExportLayout(startDate, endDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_CashReceiptControlDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportCashReceiptControl(CashReceiptControlModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_CashReceiptControlTemplate.cshtml",
                new CashReceiptControl().GetExportLayout(model.StartDate, model.EndDate));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.CashReceiptControl, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintCashReceiptControl(DateTime? startDate, DateTime? endDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_CashReceiptControlTemplate.cshtml",
                    new CashReceiptControl().GetExportLayout(startDate, endDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Daily Income State
        [HttpGet]
        public ActionResult CollectionDailyIncomeState()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/DailyIncomeState.cshtml", new CollectionDailyIncomeState().Get());
        }

        [HttpPost]
        public ActionResult CollectionDailyIncomeState(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            CollectionDailyIncomeStateModel model = new CollectionDailyIncomeStateModel();
            try
            {
                model = new CollectionDailyIncomeState().GetList(startPeriod, serviceIds, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_DailyIncomeStateDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_DailyIncomeStateDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.DailyIncomeStateList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportCollectionDailyIncomeState(CollectionDailyIncomeStateModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_DailyIncomeStateTemplate.cshtml",
                new CollectionDailyIncomeState().GetList(model.StartPeriodDate, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DailyIncomeStateTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintDailyIncomeState(DateTime startPeriod, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetDailyIncomeStateExportLayout(startPeriod, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDailyIncomeStateExportLayout(DateTime startPeriod, string serviceIds)
        {
            CollectionDailyIncomeStateModel model = new CollectionDailyIncomeStateModel();
            model = new CollectionDailyIncomeState().GetList(startPeriod, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_DailyIncomeStateTemplate.cshtml", model);
        }
        #endregion

        #region Monthly Incom eStatement By Services
        [HttpGet]
        public ActionResult MonthlyIncomeStatementByServices()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/MonthlyIncomeStatementByServices.cshtml", new MonthlyIncomeStatementByServices().Get());
        }

        [HttpPost]
        public ActionResult MonthlyIncomeStatementByServices(DateTime closingDate, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            MonthlyIncomeStatementByServicesModel model = new MonthlyIncomeStatementByServicesModel();
            try
            {
                model = new MonthlyIncomeStatementByServices().GetList(closingDate, serviceIds, pageIndex, int.MaxValue);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_MonthlyIncomeStatementByServicesDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_MonthlyIncomeStatementByServicesDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.MonthlyIncomeStatementByServicesList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportMonthlyIncomeStatementByServices(MonthlyIncomeStatementByServicesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_MonthlyIncomeStatementByServicesTemplate.cshtml",
                new MonthlyIncomeStatementByServices().GetList(model.ClosingDate, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.MonthlyIncomeStatementByServices, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintMonthlyIncomeStatementByServices(DateTime closingDate, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetMonthlyIncomeStatementByServicesExportLayout(closingDate, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetMonthlyIncomeStatementByServicesExportLayout(DateTime closingDate, string serviceIds)
        {
            MonthlyIncomeStatementByServicesModel model = new MonthlyIncomeStatementByServicesModel();
            model = new MonthlyIncomeStatementByServices().GetList(closingDate, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_MonthlyIncomeStatementByServicesTemplate.cshtml", model);
        }
        #endregion

        #region Quarterly Incom eStatement By Services
        [HttpGet]
        public ActionResult QuarterlyIncomeStatementByServices()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/QuarterlyIncomeStatementByServices.cshtml", new QuarterlyIncomeStatementByServices().Get());
        }

        [HttpPost]
        public ActionResult QuarterlyIncomeStatementByServices(int Quarter, int Year, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            QuarterlyIncomeStatementByServicesModel model = new QuarterlyIncomeStatementByServicesModel();
            try
            {
                model = new QuarterlyIncomeStatementByServices().GetList(Quarter, Year, serviceIds, pageIndex, int.MaxValue);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_QuarterlyIncomeStatementByServicesDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_QuarterlyIncomeStatementByServicesDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.MonthlyIncomeStatementByServicesList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportQuarterlyIncomeStatementByServices(QuarterlyIncomeStatementByServicesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_QuarterlyIncomeStatementByServicesTemplate.cshtml",
                new QuarterlyIncomeStatementByServices().GetList(model.Quarter, model.Year, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.QuarterlyIncomeStatementByServices, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintQuarterlyIncomeStatementByServices(int Quarter, int Year, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetQuarterlyIncomeStatementByServicesExportLayout(Quarter, Year, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetQuarterlyIncomeStatementByServicesExportLayout(int Quarter, int Year, string serviceIds)
        {
            QuarterlyIncomeStatementByServicesModel model = new QuarterlyIncomeStatementByServicesModel();
            model = new QuarterlyIncomeStatementByServices().GetList(Quarter, Year, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_QuarterlyIncomeStatementByServicesTemplate.cshtml", model);
        }
        #endregion

        #region Yearly Incom eStatement By Services
        [HttpGet]
        public ActionResult YearlyIncomeStatementByServices()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/YearlyIncomeStatementByServices.cshtml", new YearlyIncomeStatementByServices().Get());
        }

        [HttpPost]
        public ActionResult YearlyIncomeStatementByServices(int Year, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            YearlyIncomeStatementByServicesModel model = new YearlyIncomeStatementByServicesModel();
            try
            {
                model = new YearlyIncomeStatementByServices().GetList(Year, serviceIds, pageIndex, int.MaxValue);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_YearlyIncomeStatementByServicesDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_YearlyIncomeStatementByServicesDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.YearlyIncomeStatementByServicesList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportYearlyIncomeStatementByServices(YearlyIncomeStatementByServicesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_YearlyIncomeStatementByServicesTemplate.cshtml",
                new YearlyIncomeStatementByServices().GetList(model.Year, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.YearlyIncomeStatementByServices, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintYearlyIncomeStatementByServices(int Year, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetYearlyIncomeStatementByServicesExportLayout(Year, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetYearlyIncomeStatementByServicesExportLayout(int Year, string serviceIds)
        {
            YearlyIncomeStatementByServicesModel model = new YearlyIncomeStatementByServicesModel();
            model = new YearlyIncomeStatementByServices().GetList(Year, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_YearlyIncomeStatementByServicesTemplate.cshtml", model);
        }
        #endregion

        #region Revenue By Daily Collection
        [HttpGet]
        public ActionResult RevenueByDailyCollection()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/RevenueByDailyCollection.cshtml", new CollectionRevenueDailyCollection().Get());
        }

        [HttpPost]
        public ActionResult RevenueByDailyCollection(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            CollectionRevenueDailyCollectionModel model = new CollectionRevenueDailyCollectionModel();
            try
            {
                model = new CollectionRevenueDailyCollection().GetList(startPeriod, serviceIds, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_RevenueByDailyCollectionDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_RevenueByDailyCollectionDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.RevenueDailyCollectionList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportRevenueByDailyCollection(CollectionRevenueDailyCollectionModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_RevenueByDailyCollectionTemplate.cshtml",
                new CollectionRevenueDailyCollection().GetList(model.StartPeriodDate, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.RevenueDailyCollectionTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintRevenueByDailyCollection(DateTime startPeriod, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetRevenueByDailyCollectionExportLayout(startPeriod, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetRevenueByDailyCollectionExportLayout(DateTime startPeriod, string serviceIds)
        {
            CollectionRevenueDailyCollectionModel model = new CollectionRevenueDailyCollectionModel();
            model = new CollectionRevenueDailyCollection().GetList(startPeriod, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_RevenueByDailyCollectionTemplate.cshtml", model);
        }
        #endregion

        #region Historical Payment Report
        public ActionResult HistoricalPaymentReport()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/HistoricalPaymentReport.cshtml", new HistoricalPaymentReport().Get());
        }

        [HttpPost]
        public ActionResult HistoricalPaymentReport(DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            HistoricalPaymentReportModel model = new HistoricalPaymentReportModel();

            try
            {
                model = new HistoricalPaymentReport().GetExportLayout(fromDate, toDate, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_HistoricalPaymentDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_HistoricalPaymentDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.HistoricalPaymentList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportHistoricalPaymentReport(HistoricalPaymentReportModel model, int exportType)
        {
            model.FromDate = DateTime.Parse(model.FormattedFromDate);
            model.ToDate = DateTime.Parse(model.FormattedToDate);

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_HistoricalPaymentTemplate.cshtml",
                new HistoricalPaymentReport().GetExportLayout(model.FromDate, model.ToDate, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.HistoricalPaymentReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion


        #region Amnesty Movement Report       
        [HttpGet]
        public ActionResult AmnestyMovementReport()
        {
            return View("~/Areas/Reports/Views/Report/CashRegister/AmnestyMovementReport.cshtml", new AmnestyMovementReport().Get());
        }

        [HttpPost]
        public ActionResult AmnestyMovementReport(DateTime fromDate, DateTime toDate)
        {
            JsonResult jResult = new JsonResult();
            AmnestyMovementReportModel model = new AmnestyMovementReportModel();
            try
            {
                model = new AmnestyMovementReport().GetExportLayout(fromDate, toDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/_AmnestyMovementDetailList.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAmnestyMovementReport(DateTime fromDate, DateTime toDate, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_AmnestyMovementTemplate.cshtml",
                new AmnestyMovementReport().GetExportLayout(fromDate, toDate));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AmnestyMovementReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintAmnestyMovementReport(DateTime fromDate, DateTime toDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/CashRegister/ExportTemplate/_AmnestyMovementTemplate.cshtml",
                    new AmnestyMovementReport().GetExportLayout(fromDate, toDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

        #region Collections - Deposit Entry

        #region Deposit Summary

        [HttpGet]
        public ActionResult CollectionDepositSummary()
        {
            return View("~/Areas/Reports/Views/Report/DepositeEntry/DepositSummary.cshtml", new CollectionDepositSummary().Get());
        }

        [HttpPost]
        public ActionResult CollectionDepositSummary(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionDepositSummaryModel model = new CollectionDepositSummary().Get(jqdtParams, startPeriod, endPeriod, NetDepositFrom, NetDepositTo, bankAccountIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalDepositRecord,
                    recordsTotal = model.TotalDepositRecord,
                    data = model.DepositEntryList,
                    depositTotal = model.DepositEntryList.Sum(x => x.NetDeposit).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetClosingHtml(string closingListJSON)
        {
            try
            {
                List<ClosingEntryModel> model = JsonConvert.DeserializeObject<List<ClosingEntryModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/DepositeEntry/_ClosingList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportDepositSummary(CollectionDepositSummaryModel model, int exportType)
        {
            string htmlString = GetDepositSummaryExportLayout(model.StartPeriodDate, model.EndPeriodDate, model.NetDepositFrom, model.NetDepositTo, (model.FilterBankAccountID == null ? null : string.Join(",", model.FilterBankAccountID)));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DepositSummaryTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintDepositSummary(DateTime startPeriod, DateTime endPeriod, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (NetDepositFrom == 0 && NetDepositTo == 0)
                {
                    NetDepositFrom = null;
                    NetDepositTo = null;
                }
                msg = GetDepositSummaryExportLayout(startPeriod, endPeriod, NetDepositFrom, NetDepositTo, bankAccountIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        public string GetDepositSummaryExportLayout(DateTime startPeriod, DateTime endPeriod, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            CollectionDepositSummaryModel model = new CollectionDepositSummaryModel();
            model = new CollectionDepositSummary().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, NetDepositFrom, NetDepositTo, bankAccountIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/DepositeEntry/ExportTemplate/_DepositSummaryTemplate.cshtml", model);
        }
        #endregion

        #region Deposite By Payment Type
        [HttpGet]
        public ActionResult CollectionDepositSummaryByPaymentType()
        {
            return View("~/Areas/Reports/Views/Report/DepositeEntry/DepositSummaryByPaymentType.cshtml", new CollectionDepositSummaryByPaymentType().Get());
        }

        [HttpPost]
        public ActionResult CollectionDepositSummaryByPaymentType(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string paymentTypeIDs, string bankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionDepositSummaryByPaymentTypeModel model = new CollectionDepositSummaryByPaymentType().Get(jqdtParams, startPeriod, endPeriod, paymentTypeIDs, bankAccountIDs, NetDepositFrom, NetDepositTo);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalDepositRecord,
                    recordsTotal = model.TotalDepositRecord,
                    data = model.DepositEntryList,
                    depositTotal = model.DepositEntryList.Sum(x => x.NetDeposit).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetClosingPaymentTypeHtml(string closingListJSON)
        {
            try
            {
                List<ClosingEntryModel> model = JsonConvert.DeserializeObject<List<ClosingEntryModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/DepositeEntry/_ClosingPaymentTypeList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportDepositSummaryByPaymentType(CollectionDepositSummaryByPaymentTypeModel model, int exportType)
        {
            string htmlString = GetDepositSummaryPaymentTypeExportLayout(model.StartPeriodDate, model.EndPeriodDate, (model.FilterPaymentTypeID == null ? null : string.Join(",", model.FilterPaymentTypeID)), (model.FilterBankAccountID == null ? null : string.Join(",", model.FilterBankAccountID)), model.NetDepositFrom, model.NetDepositTo);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DepositSummaryPaymentTypeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        public JsonResult PrintDepositSummaryByPaymentType(DateTime startPeriod, DateTime endPeriod, string paymentTypeIDs, string bankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (NetDepositFrom == 0 && NetDepositTo == 0)
                {
                    NetDepositFrom = null;
                    NetDepositTo = null;
                }
                msg = GetDepositSummaryPaymentTypeExportLayout(startPeriod, endPeriod, paymentTypeIDs, bankAccountIDs, NetDepositFrom, NetDepositTo);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDepositSummaryPaymentTypeExportLayout(DateTime startPeriod, DateTime endPeriod, string PaymentTypeIDs, string bankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo)
        {
            CollectionDepositSummaryByPaymentTypeModel model = new CollectionDepositSummaryByPaymentTypeModel();
            model = new CollectionDepositSummaryByPaymentType().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, PaymentTypeIDs, bankAccountIDs, NetDepositFrom, NetDepositTo);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/DepositeEntry/ExportTemplate/_DepositSummaryByPaymentTypeTemplate.cshtml", model);
        }
        #endregion

        #region Deposite By Service Type        
        [HttpGet]
        public ActionResult CollectionDepositSummaryByServiceType()
        {
            return View("~/Areas/Reports/Views/Report/DepositeEntry/DepositSummaryByServiceType.cshtml", new CollectionDepositSummaryByServiceType().Get());
        }

        [HttpPost]
        public ActionResult CollectionDepositSummaryByServiceType(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionDepositSummaryByServiceTypeModel model = new CollectionDepositSummaryByServiceType().Get(jqdtParams, startPeriod, endPeriod, serviceTypeIDs, NetDepositFrom, NetDepositTo, bankAccountIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalDepositRecord,
                    recordsTotal = model.TotalDepositRecord,
                    data = model.DepositEntryList,
                    depositTotal = model.DepositEntryList.Sum(x => x.NetDeposit).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetClosingServiceTypeHtml(string closingListJSON)
        {
            try
            {
                List<ClosingEntryModel> model = JsonConvert.DeserializeObject<List<ClosingEntryModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/DepositeEntry/_ClosingServiceTypeList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportDepositSummaryByServiceType(CollectionDepositSummaryByServiceTypeModel model, int exportType)
        {
            string htmlString = GetDepositSummaryServiceTypeExportLayout(model.StartPeriodDate, model.EndPeriodDate, (model.FilterServiceTypeID == null ? null : string.Join(",", model.FilterServiceTypeID)), model.NetDepositFrom, model.NetDepositTo, (model.FilterBankAccountID == null ? null : model.FilterBankAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DepositSummaryPaymentTypeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        public JsonResult PrintDepositSummaryByServiceType(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (NetDepositFrom == 0 && NetDepositTo == 0)
                {
                    NetDepositFrom = null;
                    NetDepositTo = null;
                }
                msg = GetDepositSummaryServiceTypeExportLayout(startPeriod, endPeriod, serviceTypeIDs, NetDepositFrom, NetDepositTo, bankAccountIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDepositSummaryServiceTypeExportLayout(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string bankAccountIDs)
        {
            CollectionDepositSummaryByServiceTypeModel model = new CollectionDepositSummaryByServiceTypeModel();
            model = new CollectionDepositSummaryByServiceType().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, serviceTypeIDs, netDepositFrom, netDepositTo, bankAccountIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/DepositeEntry/ExportTemplate/_DepositSummaryByServiceTypeTemplate.cshtml", model);
        }
        #endregion

        #region Deposite By Invoice      
        [HttpGet]
        public ActionResult CollectionDepositSummaryByInvoice()
        {
            return View("~/Areas/Reports/Views/Report/DepositeEntry/DepositSummaryByInvoice.cshtml", new CollectionDepositSummaryByInvoice().Get());
        }

        [HttpPost]
        public ActionResult CollectionDepositSummaryByInvoice(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionDepositSummaryByInvoiceModel model = new CollectionDepositSummaryByInvoice().Get(jqdtParams, startPeriod, endPeriod, serviceTypeIDs, netDepositFrom, netDepositTo, bankAccountIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = model.TotalDepositRecord,
                    recordsTotal = model.TotalDepositRecord,
                    data = model.DepositEntryList,
                    depositTotal = model.DepositEntryList.Sum(x => x.NetDeposit).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetClosingInvoiceHtml(string closingListJSON)
        {
            try
            {
                List<ClosingEntryModel> model = JsonConvert.DeserializeObject<List<ClosingEntryModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/DepositeEntry/_ClosingInvoiceList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportDepositSummaryByInvoice(CollectionDepositSummaryByInvoiceModel model, int exportType)
        {
            string htmlString = GetDepositSummaryInvoiceExportLayout(model.StartPeriodDate.Value, model.EndPeriodDate.Value, (model.FilterServiceTypeID == null ? null : string.Join(",", model.FilterServiceTypeID)), model.NetDepositFrom, model.NetDepositTo, (model.FilterBankAccountID == null ? null : model.FilterBankAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DepositSummaryReportByInvoice, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        public JsonResult PrintDepositSummaryByInvoice(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netDepositFrom == 0 && netDepositTo == 0)
                {
                    netDepositFrom = null;
                    netDepositTo = null;
                }
                msg = GetDepositSummaryInvoiceExportLayout(startPeriod, endPeriod, serviceTypeIDs, netDepositFrom, netDepositTo, bankAccountIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDepositSummaryInvoiceExportLayout(DateTime startPeriod, DateTime endPeriod, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string bankAccountIDs)
        {
            CollectionDepositSummaryByInvoiceModel model = new CollectionDepositSummaryByInvoiceModel();
            model = new CollectionDepositSummaryByInvoice().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, serviceTypeIDs, netDepositFrom, netDepositTo, bankAccountIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/DepositeEntry/ExportTemplate/_DepositSummaryByInvoiceTemplate.cshtml", model);
        }
        #endregion

        #region Deposite By Payment Plan      
        [HttpGet]
        public ActionResult CollectionDepositSummaryByPaymentPlan()
        {
            return View("~/Areas/Reports/Views/Report/DepositeEntry/DepositSummaryByPaymentPlan.cshtml", new CollectionDepositSummaryByPaymentPlan().Get());
        }

        [HttpPost]
        public ActionResult CollectionDepositSummaryByPaymentPlan(JQDTParams jqdtParams, DateTime startPeriod, DateTime endPeriod, decimal? netDepositFrom, decimal? netDepositTo, string paymentPlanIDs, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CollectionDepositSummaryByPaymentPlanModel model = new CollectionDepositSummaryByPaymentPlan().Get(jqdtParams, startPeriod, endPeriod, netDepositFrom, netDepositTo, paymentPlanIDs, bankAccountIDs);
                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    data = model.DepositEntryList,
                    depositTotal = model.DepositEntryList.Sum(x => x.NetDeposit).ToString("C")
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult GetClosingPaymentPlanHtml(string closingListJSON)
        {
            try
            {
                List<ClosingEntryModel> model = JsonConvert.DeserializeObject<List<ClosingEntryModel>>(closingListJSON);
                return PartialView("~/Areas/Reports/Views/Report/DepositeEntry/_ClosingPaymentPlanList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportDepositSummaryByPaymentPlan(CollectionDepositSummaryByPaymentPlanModel model, int exportType)
        {
            string htmlString = GetDepositSummaryPaymentPlanExportLayout(model.StartPeriodDate, model.EndPeriodDate, model.NetDepositFrom, model.NetDepositTo, (model.FilterPaymentPlanID == null ? null : string.Join(",", model.FilterPaymentPlanID)), (model.FilterBankAccountID == null ? null : model.FilterBankAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DepositSummaryReportByInvoice, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        public JsonResult PrintDepositSummaryByPaymentPlan(DateTime startPeriod, DateTime endPeriod, decimal? netDepositFrom, decimal? netDepositTo, string paymentPlanIDs, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                if (netDepositFrom == 0 && netDepositTo == 0)
                {
                    netDepositFrom = null;
                    netDepositTo = null;
                }
                msg = GetDepositSummaryPaymentPlanExportLayout(startPeriod, endPeriod, netDepositFrom, netDepositTo, paymentPlanIDs, bankAccountIDs);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDepositSummaryPaymentPlanExportLayout(DateTime startPeriod, DateTime endPeriod, decimal? netDepositFrom, decimal? netDepositTo, string paymentPlanIDs, string bankAccountIDs)
        {
            CollectionDepositSummaryByPaymentPlanModel model = new CollectionDepositSummaryByPaymentPlanModel();
            model = new CollectionDepositSummaryByPaymentPlan().Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, endPeriod, netDepositFrom, netDepositTo, paymentPlanIDs, bankAccountIDs);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/DepositeEntry/ExportTemplate/_DepositSummaryByPaymentPlanTemplate.cshtml", model);
        }
        #endregion
        #endregion

        #region Collections - Payment Receipts

        #region Receipt Register
        [HttpGet]
        public ActionResult ReceiptRegister()
        {
            if (UserSession.Current.CountryID == 52)
            {
                return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptRegisterForCR.cshtml", new ReceiptRegister().Get());
            }
            else
            {
                return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptRegister.cshtml", new ReceiptRegister().Get());
            }
        }

        [HttpPost]
        public ActionResult ReceiptRegister(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptRegisterModel model = new ReceiptRegisterModel();
            try
            {
                model = new ReceiptRegister().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptRegisterList });
        }

        [HttpPost]
        public FileResult ExportReceiptRegister(ReceiptRegisterModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptRegisterTemplate.cshtml",
                new ReceiptRegister().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptRegister, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptRegister(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptRegisterTemplate.cshtml",
                    new ReceiptRegister().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt Register For CR        
        public ActionResult ReceiptRegisterForCR()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptRegisterForCR.cshtml", new ReceiptRegister().Get());
        }
        [HttpPost]
        public ActionResult ReceiptRegisterForCR(DateTime? startDate, DateTime? endDate, int? paymentReceiptTypeID, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            string message = "";
            ReceiptRegisterModel model = new ReceiptRegisterModel();
            try
            {
                model = new ReceiptRegister().GetForCR(startDate, endDate, paymentReceiptTypeID, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
                model.ReceiptRegisterList = new List<ReceiptRegisterList>();
            }
            return Json(new { recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptRegisterList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportReceiptRegisterForCR(ReceiptRegisterModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptRegisterForCRTemplate.cshtml",
                new ReceiptRegister().GetExportLayoutForCR(model.StartDate, model.EndDate, model.PaymentReceiptTypeID, model.PrintTemplateID, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptRegister, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptRegisterForCR(DateTime? startDate, DateTime? endDate, int? paymentReceiptTypeID, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptRegisterForCRTemplate.cshtml",
                    new ReceiptRegister().GetExportLayoutForCR(startDate, endDate, paymentReceiptTypeID, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt Detail
        [HttpGet]
        public ActionResult ReceiptDetail()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptDetail.cshtml", new ReceiptDetail().Get());
        }

        [HttpPost]
        public ActionResult ReceiptDetail(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs)
        {
            JsonResult jResult = new JsonResult();
            ReceiptDetailModel model = new ReceiptDetailModel();
            try
            {
                model = new ReceiptDetail().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_ReceiptDetail.cshtml", model.ReceiptDetailList);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportReceiptDetail(ReceiptDetailModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptDetailTemplate.cshtml",
                new ReceiptDetail().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID, model.FilterAccountServiceTypeID, model.FilterInvoiceTypeID, model.FilterPaymentPlanTypeID, model.FilterBankAccountID, model.FilterGrantID, model.FilterCheckbookID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptDetail, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion

        #region Receipt Commercial Facilities Rental

        [HttpGet]
        public ActionResult ReceiptCommercialFacilitiesRental()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptCommercialFacilitiesRental.cshtml", new ReceiptCommercialFacilitiesRental().Get());
        }

        [HttpPost]
        public ActionResult ReceiptCommercialFacilitiesRental(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptCommercialFacilitiesRentalModel model = new ReceiptCommercialFacilitiesRentalModel();
            try
            {
                model = new ReceiptCommercialFacilitiesRental().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptCommercialFacilitiesRentalList });
        }

        [HttpPost]
        public FileResult ExportReceiptCommercialFacilitiesRental(ReceiptCommercialFacilitiesRentalModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptCommercialFacilitiesRentalTemplate.cshtml",
                new ReceiptCommercialFacilitiesRental().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptCommercialFacilitiesRental, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptCommercialFacilitiesRental(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptCommercialFacilitiesRentalTemplate.cshtml",
                    new ReceiptCommercialFacilitiesRental().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By IVU

        [HttpGet]
        public ActionResult ReceiptByIVU()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByIVU.cshtml", new ReceiptIVU().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByIVU(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptIVUModel model = new ReceiptIVUModel();
            try
            {
                model = new ReceiptIVU().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.IVUReceiptDetailList });
        }

        [HttpPost]
        public FileResult ExportReceiptByIVU(ReceiptIVUModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByIVUTemplate.cshtml",
                new ReceiptIVU().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptIVUTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByIVU(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByIVUTemplate.cshtml",
                    new ReceiptIVU().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Business License

        [HttpGet]
        public ActionResult ReceiptByBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByBusinessLicense.cshtml", new ReceiptBusinessLicense().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptBusinessLicenseModel model = new ReceiptBusinessLicenseModel();
            try
            {
                model = new ReceiptBusinessLicense().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseReceiptDetailList });
        }

        [HttpPost]
        public FileResult ExportReceiptByBusinessLicense(ReceiptBusinessLicenseModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByBusinessLicenseTemplate.cshtml",
                new ReceiptBusinessLicense().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByBusinessLicense(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByBusinessLicenseTemplate.cshtml",
                    new ReceiptBusinessLicense().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Payment Plan

        [HttpGet]
        public ActionResult ReceiptByPaymentPlan()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByPaymentPlan.cshtml", new ReceiptPaymentPlan().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByPaymentPlan(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string paymentPlanIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptPaymentPlanModel model = new ReceiptPaymentPlanModel();
            try
            {
                model = new ReceiptPaymentPlan().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, paymentPlanIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.PaymentPlanReceiptDetailList });
        }

        [HttpPost]
        public FileResult ExportReceiptByPaymentPlan(ReceiptPaymentPlanModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPaymentPlanTemplate.cshtml",
                new ReceiptPaymentPlan().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID
              , model.FilterPaymentPlanID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptsByPaymentPlanTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByPaymentPlan(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string paymentPlanIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPaymentPlanTemplate.cshtml",
                     new ReceiptPaymentPlan().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, paymentPlanIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Other Concept

        [HttpGet]
        public ActionResult ReceiptByOtherConcept()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByOtherConcept.cshtml", new ReceiptOtherConcept().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByOtherConcept(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string paymentPlanIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptOtherConceptModel model = new ReceiptOtherConceptModel();
            List<ReceiptOtherConceptModelDataList> receiptOtherConceptModelDataList = new List<ReceiptOtherConceptModelDataList>();
            try
            {
                model = new ReceiptOtherConcept().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;

                foreach (string item in model.OtherConceptReceiptDetailList.OrderBy(g => g.CollectionConceptName).Select(s => s.CollectionConceptName).Distinct())
                {
                    ReceiptOtherConceptModelDataList receiptOtherConceptModelData = new ReceiptOtherConceptModelDataList();
                    receiptOtherConceptModelData.CollectionConcept = item;
                    receiptOtherConceptModelData.collectionList = model.OtherConceptReceiptDetailList.Where(e => e.CollectionConceptName == item).OrderBy(g => g.Date).ToList();
                    receiptOtherConceptModelDataList.Add(receiptOtherConceptModelData);
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = receiptOtherConceptModelDataList, dataconceptlist = model.OtherConceptReceiptDetailList });
        }

        public ActionResult GetOtherConceptHtml(string otherConceptListJSON)
        {
            try
            {
                List<ReceiptOtherConceptModelList> model = JsonConvert.DeserializeObject<List<ReceiptOtherConceptModelList>>(otherConceptListJSON);
                return PartialView("~/Areas/Reports/Views/Report/PaymentReceipt/_OtherConceptList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public FileResult ExportReceiptByOtherConcept(ReceiptOtherConceptModel model, int exportType, decimal? BalanceFrom, decimal? BalanceTo, string AccountIDs, string CashierIDs)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByOtherConceptTemplate.cshtml",
                new ReceiptOtherConcept().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , (model.FilterAccountID == null ? null : string.Join(",", model.FilterAccountID))
              , (model.FilterCashierID == null ? null : string.Join(",", model.FilterCashierID))));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptsByOtherConceptTitle, htmlString, false);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByOtherConcept(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string paymentPlanIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByOtherConceptTemplate.cshtml",
                    new ReceiptOtherConcept().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt Void

        [HttpGet]
        public ActionResult VoidReceipts()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/VoidReceipts.cshtml", new VoidReceiptsModel().Get());
        }

        [HttpPost]
        public ActionResult VoidReceipts(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;

            VoidReceiptsModel model = new VoidReceiptsModel();
            try
            {
                model = model.Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptEntryList });
        }

        [HttpPost]
        public FileResult ExportVoidReceipt(VoidReceiptsModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_VoidReceiptTemplate.cshtml",
                model.GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , (model.FilterAccountID == null ? null : string.Join(",", model.FilterAccountID))
              , (model.FilterCashierID == null ? null : string.Join(",", model.FilterCashierID))));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.VoidReceipt, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintVoidRegister(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_VoidReceiptTemplate.cshtml",
                    new VoidReceiptsModel().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Receipt By Permission Fees

        [HttpGet]
        public ActionResult ReceiptByPermissionFees()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByPermissionFees.cshtml", new ReceiptPermissionFees().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByPermissionFees(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptPermissionFeesModel model = new ReceiptPermissionFeesModel();
            try
            {
                model = new ReceiptPermissionFees().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptPermissionFeesList });
        }

        [HttpPost]
        public FileResult ExportReceiptByPermissionFees(ReceiptPermissionFeesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPermissionFeesTemplate.cshtml",
                new ReceiptPermissionFees().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptPermissionFessTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByPermissionFees(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPermissionFeesTemplate.cshtml",
                    new ReceiptPermissionFees().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Permission Expenses

        [HttpGet]
        public ActionResult ReceiptByPermissionExpenses()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByPermissionExpenses.cshtml", new ReceiptPermissionExpenses().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByPermissionExpenses(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptPermissionExpensesModel model = new ReceiptPermissionExpensesModel();
            try
            {
                model = new ReceiptPermissionExpenses().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptPermissionExpensesList });
        }

        [HttpPost]
        public FileResult ExportReceiptByPermissionExpenses(ReceiptPermissionExpensesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPermissionExpensesTemplate.cshtml",
                new ReceiptPermissionExpenses().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptPermissionExpensesTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByPermissionExpenses(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByPermissionExpensesTemplate.cshtml",
                    new ReceiptPermissionExpenses().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Street Vendors

        [HttpGet]
        public ActionResult ReceiptByStreetVendors()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByStreetVendors.cshtml", new ReceiptPermissionStreetVendors().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByStreetVendors(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptPermissionStreetVendorsModel model = new ReceiptPermissionStreetVendorsModel();
            try
            {
                model = new ReceiptPermissionStreetVendors().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptPermissionStreetVendorsList });
        }

        [HttpPost]
        public FileResult ExportReceiptByStreetVendors(ReceiptPermissionStreetVendorsModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByStreetVendorsTemplate.cshtml",
                new ReceiptPermissionStreetVendors().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterCashierID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptStreetVendorTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByStreetVendors(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByStreetVendorsTemplate.cshtml",
                    new ReceiptPermissionStreetVendors().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt By Security Bank Account

        [HttpGet]
        public ActionResult ReceiptBySecurityBankAccount()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptBySecurityBankAccount.cshtml", new ReceiptBySecurityBankAccount().Get());
        }

        [HttpPost]
        public ActionResult ReceiptBySecurityBankAccount(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptBySecurityBankAccountModel model = new ReceiptBySecurityBankAccountModel();
            try
            {
                model = new ReceiptBySecurityBankAccount().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { recordsFiltered = model.TotalRecord, status = status, data = model.SecurityBankAccountList });
        }

        [HttpPost]
        public FileResult ExportReceiptBySecurityBankAccount(ReceiptBySecurityBankAccountModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptBySecurityBankAccountTemplate.cshtml",
                new ReceiptBySecurityBankAccount().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterBankAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.SecurityDetailBankAccountTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptBySecurityBankAccount(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptBySecurityBankAccountTemplate.cshtml",
                    new ReceiptBySecurityBankAccount().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceiptBySecurityBankAccountHtml(string DataListJSON)
        {
            try
            {
                List<ReceiptBySecurityBankAccountModelList> model = JsonConvert.DeserializeObject<List<ReceiptBySecurityBankAccountModelList>>(DataListJSON);
                return PartialView("~/Areas/Reports/Views/Report/PaymentReceipt/_ReceiptBySecurityBankAccountList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Receipt By Bank

        [HttpGet]
        public ActionResult ReceiptByBank()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/ReceiptByBank.cshtml", new ReceiptByBank().Get());
        }

        [HttpPost]
        public ActionResult ReceiptByBank(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            ReceiptByBankModel model = new ReceiptByBankModel();
            try
            {
                model = new ReceiptByBank().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { recordsFiltered = model.TotalRecord, status = status, data = model.ReceiptByBankList });
        }

        [HttpPost]
        public FileResult ExportReceiptByBank(ReceiptByBankModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByBankTemplate.cshtml",
                new ReceiptByBank().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID
              , model.FilterBankAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptByBankTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptByBank(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_ReceiptByBankTemplate.cshtml",
                    new ReceiptByBank().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceiptByBankHtml(string DataListJSON)
        {
            try
            {
                List<ReceiptByBankList> model = JsonConvert.DeserializeObject<List<ReceiptByBankList>>(DataListJSON);
                return PartialView("~/Areas/Reports/Views/Report/PaymentReceipt/_ReceiptByBankList.cshtml", model);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Migration Validation Factu
        [HttpGet]
        public ActionResult MigrationValidationFactu()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/MigrationValidationFactu.cshtml", new MigrationValidationFactu().Get());
        }

        [HttpPost]
        public ActionResult MigrationValidationFactu(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            MigrationValidationFactuModel model = new MigrationValidationFactuModel();
            try
            {
                model = new MigrationValidationFactu().GetList(typeID, serviceCodes, accountIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationFactuDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationFactuDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.MigrationValidationFactuList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportMigrationValidationFactu(MigrationValidationFactuModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_MigrationValidationFactuTemplate.cshtml",
                new MigrationValidationFactu().GetList(model.TypeID, model.FilterServiceCodes, model.FilterAccountID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.MigrationValidationFactuTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion

        #region Migration Validation Cobros
        [HttpGet]
        public ActionResult MigrationValidationCobros()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/MigrationValidationCobros.cshtml", new MigrationValidationCobros().Get());
        }

        [HttpPost]
        public ActionResult MigrationValidationCobros(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            MigrationValidationCobrosModel model = new MigrationValidationCobrosModel();
            try
            {
                model = new MigrationValidationCobros().GetList(typeID, serviceCodes, accountIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationCobrosDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationCobrosDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.MigrationValidationCobrosList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportMigrationValidationCobros(MigrationValidationCobrosModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_MigrationValidationCobrosTemplate.cshtml",
                new MigrationValidationCobros().GetList(model.TypeID, model.FilterServiceCodes, model.FilterAccountID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.MigrationValidationCobrosTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion

        #region Migration Validation Bienes
        [HttpGet]
        public ActionResult MigrationValidationBienes()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/MigrationValidationBienes.cshtml", new MigrationValidationBienes().Get());
        }

        [HttpPost]
        public ActionResult MigrationValidationBienes(int? typeID, string FincaIDs, string accountIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            MigrationValidationBienesModel model = new MigrationValidationBienesModel();
            try
            {
                model = new MigrationValidationBienes().GetList(typeID, FincaIDs, accountIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationBienesDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_MigrationValidationBienesDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.MigrationValidationBienesList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportMigrationValidationBienes(MigrationValidationBienesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_MigrationValidationBienesTemplate.cshtml",
                new MigrationValidationBienes().GetList(model.TypeID, model.FilterFincaIDs, model.FilterAccountID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.MigrationValidationBienesTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion

        #region Payments Made By The Taxpayer       
        [HttpGet]
        public ActionResult PaymentsMadeByTheTaxpayer()
        {
            return View("~/Areas/Reports/Views/Report/PaymentReceipt/PaymentsMadeByTheTaxpayer.cshtml", new PaymentsMadeByTheTaxpayer().Get());
        }

        [HttpPost]
        public ActionResult PaymentsMadeByTheTaxpayer(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            PaymentsMadeByTheTaxpayerModel model = new PaymentsMadeByTheTaxpayerModel();
            try
            {
                model = new PaymentsMadeByTheTaxpayer().GetExportLayout(startDate, endDate, accountIDs);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/_PaymentsMadeByTheTaxpayerDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportPaymentsMadeByTheTaxpayer(PaymentsMadeByTheTaxpayerModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_PaymentsMadeByTheTaxpayerTemplate.cshtml",
                new PaymentsMadeByTheTaxpayer().GetExportLayout(model.StartDate, model.EndDate, model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ListOfPaymentsMadeByTheTaxpayer, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintPaymentsMadeByTheTaxpayer(DateTime? startDate, DateTime? endDate, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/PaymentReceipt/ExportTemplate/_PaymentsMadeByTheTaxpayerTemplate.cshtml",
                    new PaymentsMadeByTheTaxpayer().GetExportLayout(startDate, endDate, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Accounting

        #region Journal Detail
        [HttpGet]
        public ActionResult JournalDetail(int? ID, int? DocumentTypeID)
        {
            JournalDetailModel Modal = new JournalDetail().Get();
            if (ID == null || ID <= 0 || DocumentTypeID == null || DocumentTypeID <= 0)
            {
                return View("~/Areas/Reports/Views/Report/Journal/JournalDetail.cshtml", Modal);
            }

            var objModal = new JournalDetail().Get(null, null, DocumentTypeID.ToString(), null, null, null, null, null, null, null, null, null, ID);
            Modal.JournalDetailListModel = objModal.JournalDetailListModel;
            Modal.ReportCompanyModel = objModal.ReportCompanyModel;
            return View("~/Areas/Reports/Views/Report/Journal/JournalDetail.cshtml", Modal);

        }


        [HttpPost]
        public ActionResult JournalDetail(DateTime startDate, DateTime endDate, string DocumentTypeIDs, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            JsonResult jResult = new JsonResult();
            JournalDetailModel model = new JournalDetailModel();
            try
            {
                model = new JournalDetail().Get(startDate, endDate, DocumentTypeIDs, accountIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo, null);

                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/_JournalDetail.cshtml", model.JournalDetailListModel);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportJournalDetail(JournalDetailModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/ExportTemplate/_JournalDetailTemplate.cshtml",
                new JournalDetail().GetExportLayout(model.StartDate, model.EndDate, (model.FilterDocumentTypeIDs == null ? null : model.FilterDocumentTypeIDs), model.FilterAccountID, model.FilterAccountServiceTypeID, model.FilterInvoiceTypeID, model.FilterPaymentPlanTypeID, model.FilterBankAccountID, model.FilterGrantID, model.FilterCheckbookID, model.BalanceFrom, model.BalanceTo));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.JournalDetailTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        //[HttpGet]
        //public JsonResult PrintJournalDetail(DateTime? startDate, DateTime? endDate, string DocumentTypeIDs, string accountIDs, string serviceTypeIDs, string collectionTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo)
        //{
        //    bool status = true;
        //    string msg = string.Empty;
        //    try
        //    {
        //        JournalDetailModel model = new JournalDetailModel();
        //        msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/ExportTemplate/_JournalDetailTemplate.cshtml",
        //            new JournalDetail().GetExportLayout(startDate, endDate, DocumentTypeIDs, accountIDs, serviceTypeIDs, collectionTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo));
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //        msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
        //    }
        //    return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Journal
        [HttpGet]
        public ActionResult Journal()
        {
            JournalModel Modal = new Journal().Get();
            return View("~/Areas/Reports/Views/Report/Journal/Journal.cshtml", Modal);
        }

        [HttpPost]
        public ActionResult Journal(DateTime startDate, DateTime endDate, string DocumentTypeIDs)
        {
            JsonResult jResult = new JsonResult();
            JournalModel model = new JournalModel();
            try
            {
                model.JournalListModel = new Journal().Get(startDate, endDate, DocumentTypeIDs);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/_Journal.cshtml", model.JournalListModel);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportJournal(JournalModel model, int exportType)
        {
            var objdata = new Journal().Get();
            model.ReportCompanyModel = objdata.ReportCompanyModel;
            model.lstDocumentType = objdata.lstDocumentType;
            model.JournalListModel = new Journal().GetExportLayout(model.StartDate, model.EndDate, (model.FilterDocumentTypeIDs == null ? null : model.FilterDocumentTypeIDs));
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/ExportTemplate/_JournalTemplate.cshtml", model);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.JournalTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintJournal(DateTime? startDate, DateTime? endDate, string DocumentTypeIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                JournalModel model = new Journal().Get();
                model.JournalListModel = new Journal().GetExportLayout(startDate, endDate, DocumentTypeIDs);
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Journal/ExportTemplate/_JournalTemplate.cshtml", model);
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Business License

        #region Balance By Business License

        [HttpGet]
        public ActionResult BalanceByBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/BalanceByBusinessLicense.cshtml", new BusinessLicenseBalance().Get());
        }

        [HttpPost]
        public ActionResult BalanceByBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseBalanceModel model = new BusinessLicenseBalanceModel();
            try
            {
                model = new BusinessLicenseBalance().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseBalanceList });
        }

        [HttpPost]
        public FileResult ExportBalanceByBusinessLicense(BusinessLicenseBalanceModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BalanceByBusinessLicenseTemplate.cshtml",
                new BusinessLicenseBalance().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.BalanceBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintBalanceByBusinessLicense(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BalanceByBusinessLicenseTemplate.cshtml",
                    new BusinessLicenseBalance().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Business License Filing By Filing Type

        [HttpGet]
        public ActionResult BusinessLicenseFilingByFilingType()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/BusinessLicenseFilingByFilingType.cshtml", new BusinessLicenseFilingByFilingType().Get());
        }

        [HttpPost]
        public ActionResult BusinessLicenseFilingByFilingType(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseFilingByFilingTypeModel model = new BusinessLicenseFilingByFilingTypeModel();
            try
            {
                model = new BusinessLicenseFilingByFilingType().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseFilingByFilingTypeList });
        }

        [HttpPost]
        public FileResult ExportBusinessLicenseFilingByFilingType(BusinessLicenseFilingByFilingTypeModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BusinessLicenseFilingByFilingTypeTemplate.cshtml",
                new BusinessLicenseFilingByFilingType().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.BusinessLicenseFilingByFilingTypeTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintBusinessLicenseFilingByFilingType(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BusinessLicenseFilingByFilingTypeTemplate.cshtml",
                    new BusinessLicenseFilingByFilingType().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Credit Balance By Business License

        [HttpGet]
        public ActionResult CreditBalanceByBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/CreditBalanceByBusinessLicense.cshtml", new BusinessLicenseCreditBalance().Get());
        }

        [HttpPost]
        public ActionResult CreditBalanceByBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseCreditBalanceModel model = new BusinessLicenseCreditBalanceModel();
            try
            {
                model = new BusinessLicenseCreditBalance().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseCreditBalanceList });
        }

        [HttpPost]
        public FileResult ExportCreditBalanceByBusinessLicense(BusinessLicenseCreditBalanceModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_CreditBalanceByBusinessLicenseTemplate.cshtml",
                new BusinessLicenseCreditBalance().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.CreditBalanceBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintCreditBalanceByBusinessLicense(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_CreditBalanceByBusinessLicenseTemplate.cshtml",
                    new BusinessLicenseCreditBalance().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Closing Balance By Business License

        [HttpGet]
        public ActionResult ClosingBalanceByBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/ClosingBalanceByBusinessLicense.cshtml", new BusinessLicenseBalanceClosing().Get());
        }

        [HttpPost]
        public ActionResult ClosingBalanceByBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseBalanceClosingModel model = new BusinessLicenseBalanceClosingModel();
            try
            {
                model = new BusinessLicenseBalanceClosing().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseBalanceByClosingList });
        }

        [HttpPost]
        public FileResult ExportClosingBalanceByBusinessLicense(BusinessLicenseBalanceClosingModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_ClosingBalanceByBusinessLicenseTemplate.cshtml",
                new BusinessLicenseBalanceClosing().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ClosingBalanceBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintClosingBalanceByBusinessLicense(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_ClosingBalanceByBusinessLicenseTemplate.cshtml",
                    new BusinessLicenseBalanceClosing().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transaction By Business License
        [HttpGet]
        public ActionResult TransactionByBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/TransactionByBusinessLicense.cshtml", new BusinessLicenseTransaction().Get());
        }

        [HttpPost]
        public ActionResult TransactionByBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, int searchStatus)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseTransactionModel model = new BusinessLicenseTransactionModel();
            try
            {
                model = new BusinessLicenseTransaction().Get(startDate, endDate, searchStatus, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseTransactionList });
        }

        [HttpPost]
        public FileResult ExportTransactionByBusinessLicense(BusinessLicenseTransactionModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_TransactionByBusinessLicenseTemplate.cshtml",
               new BusinessLicenseTransaction().GetExportLayout(model.StartDate, model.EndDate, model.searchStatusID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.TransactionBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintTransactionByBusinessLicense(DateTime? startDate, DateTime? endDate, int searchStatus)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_TransactionByBusinessLicenseTemplate.cshtml",
                    new BusinessLicenseTransaction().GetExportLayout(startDate, endDate, searchStatus));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Business Licence Sanitary Permit Due Date
        [HttpGet]
        public ActionResult BusinessLicenceSanitaryPermitDueDate()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/BusinessLicenceSanitaryPermitDueDate.cshtml", new BusinessLicenceSanitaryPermitDueDate().Get());
        }

        [HttpPost]
        public ActionResult BusinessLicenceSanitaryPermitDueDate(JQDTParams jqdtParams, DateTime? fromDate, DateTime? toDate, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenceSanitaryPermitDueDateModel model = new BusinessLicenceSanitaryPermitDueDateModel();
            try
            {
                model = new BusinessLicenceSanitaryPermitDueDate().Get(fromDate, toDate, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenceSanitaryPermitDueDateList });
        }

        [HttpPost]
        public FileResult ExportBusinessLicenceSanitaryPermitDueDate(BusinessLicenceSanitaryPermitDueDateModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BusinessLicenceSanitaryPermitDueDateTemplate.cshtml",
                new BusinessLicenceSanitaryPermitDueDate().GetExportLayout(model.FromDate, model.ToDate, model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.SanitaryPermitDueDateBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintBusinessLicenceSanitaryPermitDueDate(DateTime? fromDate, DateTime? toDate, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_BusinessLicenceSanitaryPermitDueDateTemplate.cshtml",
                    new BusinessLicenceSanitaryPermitDueDate().GetExportLayout(fromDate, toDate, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region General Business License

        [HttpGet]
        public ActionResult GeneralBusinessLicense()
        {
            return View("~/Areas/Reports/Views/Report/BusinessLicense/GeneralBusinessLicense.cshtml", new BusinessLicenseGeneral().Get());
        }

        [HttpPost]
        public ActionResult GeneralBusinessLicense(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;
            BusinessLicenseGeneralModel model = new BusinessLicenseGeneralModel();
            try
            {
                model = new BusinessLicenseGeneral().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, jqdtParams);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.BusinessLicenseGeneralList });
        }

        [HttpPost]
        public FileResult ExportGeneralBusinessLicense(BusinessLicenseBalanceModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_GeneralBusinessLicenseTemplate.cshtml",
                new BusinessLicenseGeneral().GetExportLayout(model.StartDate, model.EndDate, model.BalanceFrom, model.BalanceTo
                , model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.BalanceBusinessLicenseTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintGeneralBusinessLicense(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/BusinessLicense/ExportTemplate/_GeneralBusinessLicenseTemplate.cshtml",
                    new BusinessLicenseGeneral().GetExportLayout(startDate, endDate, balanceFrom, balanceTo, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ViewJournal
        [HttpGet]
        public ActionResult ViewJournal(int id, int DocumentTypeID, string ReceiptNumber)
        {
            bool status = true;
            string ErrorMsg = "";
            ViewBag.ReceiptNumber = ReceiptNumber;
            List<JournalDetailListModel> JournalDetailListModel = new List<JournalDetailListModel>();
            try
            {
                JournalDetailListModel = new JournalDetail().Get(null, null, DocumentTypeID.ToString(), null, null, null, null, null, null, null, null, null, id).JournalDetailListModel;
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            return Json(new { status = status, message = ErrorMsg, RedirectUrl = HMTLHelperExtensions.RenderPartialToString(this, "~/Views/Shared/_ViewJournal.cshtml", JournalDetailListModel) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Water

        #region Historical Readings by Meter
        public ActionResult HistoricalReadingsByMeter(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Water/HistoricalReadingsByMeter.cshtml", new HistoricalReadingsMeter().Get(accountID));
        }

        [HttpPost]
        public ActionResult HistoricalReadingsByMeter(JQDTParams jqdtParams, string meter, int? accountID)
        {
            JsonResult jResult = new JsonResult();
            HistoricalReadingsMeterModel model = new HistoricalReadingsMeterModel();
            try
            {
                if (!string.IsNullOrEmpty(meter) || accountID > 0)
                {
                    model = new HistoricalReadingsMeter().GetExportLayout(meter, accountID);
                }

                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_HistoricalReadingsByMeterDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportHistoricalReadingsByMeter(HistoricalReadingsMeterModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_HistoricalReadingsByMeterTemplate.cshtml",
                new HistoricalReadingsMeter().GetExportLayout(model.Meter, model.AccountId));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReportOfHistoricalReadingsByMeter, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintHistoricalReadingsByMeter(string meter, int? accountID)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_HistoricalReadingsByMeterTemplate.cshtml",
                    new HistoricalReadingsMeter().GetExportLayout(meter, accountID));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Consumption By Range
        public ActionResult ConsumptionByRange()
        {
            return View("~/Areas/Reports/Views/Report/Water/ConsumptionByRange.cshtml", new ConsumptionByRange().Get());
        }

        [HttpPost]
        public ActionResult ConsumptionByRange(DateTime? fromDate, DateTime? toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            ConsumptionByRangeModel model = new ConsumptionByRangeModel();
            try
            {
                model = new ConsumptionByRange().GetExportLayout(fromDate, toDate, from, to, taxNumber, meter, conditionFrom, conditionTo, conditionType, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_ConsumptionRangeDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_ConsumptionByRangeDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.ConsumptionRangeList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportConsumptionByRange(ConsumptionByRangeModel model, int exportType)
        {
            model.FromDate = DateTime.Parse(model.FormattedFromDate);
            model.ToDate = DateTime.Parse(model.FormattedToDate);

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_ConsumptionByRangeTemplate.cshtml",
                new ConsumptionByRange().GetExportLayout(model.FromDate, model.ToDate, model.From, model.To, model.TaxNumber, model.Meter, model.ddlFrom, model.ddlTo, model.ConditionTypeID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ConsumptionByRangeReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintConsumptionByRange(DateTime fromDate, DateTime toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_ConsumptionByRangeTemplate.cshtml",
                    new ConsumptionByRange().GetExportLayout(fromDate, toDate, from, to, taxNumber, meter, conditionFrom, conditionTo, conditionType, 1, Int32.MaxValue));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Inconsistence Reading

        [HttpGet]
        public ActionResult InconsistenceReading()
        {
            return View("~/Areas/Reports/Views/Report/Water/InconsistenceReading.cshtml", new InconsistenceReading().Get());
        }

        [HttpPost]
        public ActionResult InconsistenceReading(DateTime? period, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            InconsistenceReadingModel model = new InconsistenceReadingModel();
            try
            {
                model = new InconsistenceReading().GetExportLayout(period, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_InconsistenceReadingDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_InconsistenceReadingDetail.cshtml", model);
                }
                jResult = Json(new
                {
                    status = true,
                    data = htmlstring,
                    RowCount = model.InconsistenceReadingList.Count,
                    lastReadingTotal = model.InconsistenceReadingList.Sum(x => x.LastReading).ToString(Common.DecimalPoints),
                    currentReadingTotal = model.InconsistenceReadingList.Sum(x => x.CurrentReading).ToString(Common.DecimalPoints),
                    TotalConsumption = model.TotalConsumption.ToString(Common.DecimalPoints),
                    TotalAmount = model.TotalAmount.ToString(Common.DecimalPoints)
                });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportInconsistenceReading(InconsistenceReadingModel model, int exportType)
        {
            model.Period = DateTime.Parse(model.FormattedPeriod);

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_InconsistenceReadingTemplate.cshtml",
                new InconsistenceReading().GetExportLayout(model.Period, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.InconsistenceReadingReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintInconsistenceReading(DateTime? period)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_InconsistenceReadingTemplate.cshtml",
                    new InconsistenceReading().GetExportLayout(period, 1, Int32.MaxValue));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Accounts Consumption And Collection
        [HttpGet]
        public ActionResult AccountsConsumptionAndCollection()
        {
            return View("~/Areas/Reports/Views/Report/Water/AccountsConsumptionAndCollection.cshtml", new AccountsConsumptionAndCollection().Get());
        }

        [HttpPost]
        public ActionResult AccountsConsumptionAndCollection(JQDTParams jqdtParams, DateTime Period)
        {
            JsonResult jResult = new JsonResult();
            AccountsConsumptionAndCollectionModel model = new AccountsConsumptionAndCollectionModel();
            try
            {
                model = new AccountsConsumptionAndCollection().GetExportLayout(Period);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_AccountsConsumptionAndCollectionDetail.cshtml", model);
                jResult = Json(new { status = true, Period = Period.ToString("MMMM yyyy").First().ToString().ToUpper() + Period.ToString("MMMM yyyy").Substring(1), data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAccountsConsumptionAndCollection(AccountsConsumptionAndCollectionModel model, int exportType)
        {
            model.Period = DateTime.Parse(model.FormattedPeriod);
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_AccountsConsumptionAndCollectionTemplate.cshtml",
                new AccountsConsumptionAndCollection().GetExportLayout(model.Period));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AccountsConsumptionAndCollectionTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintAccountsConsumptionAndCollection(DateTime Period)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_AccountsConsumptionAndCollectionTemplate.cshtml",
                    new AccountsConsumptionAndCollection().GetExportLayout(Period));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Duplicated Meter Number
        [HttpGet]
        public ActionResult DuplicatedMeterNumber()
        {
            return View("~/Areas/Reports/Views/Report/Water/DuplicatedMeterNumber.cshtml", new DuplicatedMeterNumber().Get());
        }

        [HttpPost]
        public ActionResult DuplicatedMeterNumber(JQDTParams jqdtParams)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;

            DuplicatedMeterNumberModel model = new DuplicatedMeterNumberModel();
            try
            {
                model = new DuplicatedMeterNumber().GetExportLayout();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.DuplicatedMeterNumberList });
        }

        [HttpPost]
        public FileResult ExportDuplicatedMeterNumber(DuplicatedMeterNumberModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_DuplicatedMeterNumberTemplate.cshtml",
                new DuplicatedMeterNumber().GetExportLayout());
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DuplicatedMeterNumberTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintDuplicatedMeterNumber()
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_DuplicatedMeterNumberTemplate.cshtml",
                    new DuplicatedMeterNumber().GetExportLayout());
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Routa Missing Services
        [HttpGet]
        public ActionResult RoutaMissingServices()
        {
            return View("~/Areas/Reports/Views/Report/Water/RoutaMissingServices.cshtml", new RoutaMissingServices().Get());
        }

        [HttpPost]
        public ActionResult RoutaMissingServices(JQDTParams jqdtParams)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;

            RoutaMissingServicesModel model = new RoutaMissingServicesModel();
            try
            {
                model = new RoutaMissingServices().GetExportLayout();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.RoutaMissingServicesList });
        }

        [HttpPost]
        public FileResult ExportRoutaMissingServices(RoutaMissingServicesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_RoutaMissingServicesTemplate.cshtml",
                new RoutaMissingServices().GetExportLayout());
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.RoutaMissingServices, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintRoutaMissingServices()
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_RoutaMissingServicesTemplate.cshtml",
                    new RoutaMissingServices().GetExportLayout());
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Suspension Order For Water Service
        [HttpGet]
        public ActionResult SuspensionOrderForWaterService(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Water/SuspensionOrderForWaterService.cshtml", new SuspensionOrderForWaterService().Get(accountID));
        }

        [HttpPost]
        public ActionResult SuspensionOrderForWaterService(JQDTParams jqdtParams, int accountId)
        {
            JsonResult jResult = new JsonResult();
            SuspensionOrderForWaterServiceModel model = new SuspensionOrderForWaterServiceModel();
            try
            {
                model = new SuspensionOrderForWaterService().GetExportLayout(accountId);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/_SuspensionOrderForWaterServiceDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportSuspensionOrderForWaterService(SuspensionOrderForWaterServiceModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_SuspensionOrderForWaterServiceTemplate.cshtml",
                new SuspensionOrderForWaterService().GetExportLayout(model.AccountId));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.AccountService.SuspensionOrder, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintSuspensionOrderForWaterService(int accountId)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Water/ExportTemplate/_SuspensionOrderForWaterServiceTemplate.cshtml",
                    new SuspensionOrderForWaterService().GetExportLayout(accountId));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Properties 

        #region Properties and Movements List by Account

        [HttpGet]
        public ActionResult Property(int? accountID)
        {
            return View("~/Areas/Reports/Views/Report/Property/PropertyDetail.cshtml", new Property().Get(accountID));
        }

        [HttpPost]
        public ActionResult Property(JQDTParams jqdtParams, int accountId, string accountPropertyIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            JsonResult jResult = new JsonResult();
            PropertyModel model = new PropertyModel();
            try
            {
                model = new Property().GetExportLayout(accountId, accountPropertyIDs, balanceFrom, balanceTo);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_PropertyDetailList.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }
        [HttpPost]
        public FileResult ExportProperty(PropertyModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_PropertyDetailTemplate.cshtml",
                new Property().GetExportLayout(model.AccountId, model.AccountPropertyID, model.BalanceFrom, model.BalanceTo));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.PropertyTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintProperty(int accountId, string accountPropertyID, decimal? balanceFrom, decimal? balanceTo)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_PropertyDetailTemplate.cshtml",
                    new Property().GetExportLayout(accountId, accountPropertyID, balanceFrom, balanceTo));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Transfers Report                
        [HttpGet]
        public ActionResult TransfersReport()
        {
            return View("~/Areas/Reports/Views/Report/Property/TransfersReport.cshtml", new TransfersReport().Get());
        }

        [HttpPost]
        public ActionResult TransfersReport(JQDTParams jqdtParams, DateTime? fromDate, DateTime? toDate, int? transferTypeID)
        {
            JsonResult jResult = new JsonResult();
            TransfersReportModel model = new TransfersReportModel();
            try
            {
                model = new TransfersReport().GetExportLayout(fromDate, toDate, transferTypeID);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_TransfersReportDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportTransfersReport(TransfersReportModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_TransfersReportTemplate.cshtml",
                new TransfersReport().GetExportLayout(model.FromDate, model.ToDate, model.TransferTypeId));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.TransfersReportTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintTransfersReport(DateTime? fromDate, DateTime? toDate, int? transferTypeID)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_TransfersReportTemplate.cshtml",
                    new TransfersReport().GetExportLayout(fromDate, toDate, transferTypeID));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Exclusion Form
        [HttpGet]
        public ActionResult AccountExclusionForm()
        {
            return View("~/Areas/Reports/Views/Report/Property/AccountExclusionForm.cshtml", new AccountExclusionForm().Get());
        }

        [HttpPost]
        public ActionResult AccountExclusionForm(JQDTParams jqdtParams, int? accountId, string observations)
        {
            JsonResult jResult = new JsonResult();
            AccountExclusionFormModel model = new AccountExclusionFormModel();
            try
            {
                model = new AccountExclusionForm().GetExportLayout(accountId, observations);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_AccountExclusionFormDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAccountExclusionForm(AccountExclusionFormModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_AccountExclusionFormTemplate.cshtml",
                new AccountExclusionForm().GetExportLayout(model.AccountId, model.Observations));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.AccountExclusionFormReportTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintAccountExclusionForm(AccountExclusionFormModel model)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_AccountExclusionFormTemplate.cshtml",
                    new AccountExclusionForm().GetExportLayout(model.AccountId, model.Observations));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Park Maintenance Missing Services             
        [HttpGet]
        public ActionResult ParkMaintenanceMissingServices()
        {
            return View("~/Areas/Reports/Views/Report/Property/ParkMaintenanceMissingServices.cshtml", new ParkMaintenanceMissingServices().Get());
        }

        [HttpPost]
        public ActionResult ParkMaintenanceMissingServices(JQDTParams jqdtParams, int? year)
        {
            JsonResult jResult = new JsonResult();
            bool status = true;

            ParkMaintenanceMissingServicesModel model = new ParkMaintenanceMissingServicesModel();
            try
            {
                model = new ParkMaintenanceMissingServices().GetExportLayout(year);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, status = status, data = model.ParkMaintenanceMissingServicesList });
        }

        [HttpPost]
        public FileResult ExportParkMaintenanceMissingServices(ParkMaintenanceMissingServicesModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_ParkMaintenanceMissingServicesTemplate.cshtml",
                new ParkMaintenanceMissingServices().GetExportLayout(model.Year));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ParkMaintenanceMissingServicesTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintParkMaintenanceMissingServices(int? year)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_ParkMaintenanceMissingServicesTemplate.cshtml",
                    new ParkMaintenanceMissingServices().GetExportLayout(year));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Services Linked To Right
        [HttpGet]
        public ActionResult ServicesLinkedToRight()
        {
            return View("~/Areas/Reports/Views/Report/Property/ServicesLinkedToRight.cshtml", new ServicesLinkedToRight().Get());
        }

        [HttpPost]
        public ActionResult ServicesLinkedToRight(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            ServicesLinkedToRightModel model = new ServicesLinkedToRightModel();
            try
            {
                model = new ServicesLinkedToRight().GetList(typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_ServicesLinkedToRightDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_ServicesLinkedToRightDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.ServicesLinkedToRightList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpGet]
        public FileResult ExportServicesLinkedToRight(int? TypeID, string FilterServiceTypeID, string FilterServiceID, string FilterAccountID, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_ServicesLinkedToRightTemplate.cshtml",
                new ServicesLinkedToRight().GetList(TypeID, FilterServiceTypeID, FilterServiceID, FilterAccountID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ServicesLinkedToRightTitle, htmlString, true);
            Response.AppendHeader("Set-Cookie", "fileDownload=true; path=/");
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintServicesLinkedToRight(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_ServicesLinkedToRightTemplate.cshtml",
                    new ServicesLinkedToRight().GetList(typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, 1, Int32.MaxValue));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RightsByOwnersID
        [HttpGet]
        public ActionResult RightsByOwnersID()
        {
            return View("~/Areas/Reports/Views/Report/Property/RightsByOwnersID.cshtml", new RightsByOwnersID().Get());
        }

        [HttpPost]
        public ActionResult RightsByOwnersID(JQDTParams jqdtParams, int? ownerID)
        {
            JsonResult jResult = new JsonResult();
            RightsByOwnersIDModel model = new RightsByOwnersIDModel();
            try
            {
                model = new RightsByOwnersID().GetExportLayout(ownerID);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_RightsByOwnersIDDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportRightsByOwnersID(RightsByOwnersIDModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_RightsByOwnersIDTemplate.cshtml",
                new RightsByOwnersID().GetExportLayout(model.OwnerID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ListOfRightsByOwnersIDReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpPost]
        public JsonResult PrintRightsByOwnersID(int? ownerID)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_RightsByOwnersIDTemplate.cshtml",
                    new RightsByOwnersID().GetExportLayout(ownerID));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Historical Account Service Removed
        public ActionResult HistoricalAccountServiceRemoved()
        {
            return View("~/Areas/Reports/Views/Report/Account/HistoricalAccountServiceRemoved.cshtml", new HistoricalAccountServiceRemoved().Get());
        }

        [HttpPost]
        public ActionResult HistoricalAccountServiceRemoved(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            JsonResult jResult = new JsonResult();
            HistoricalAccountServiceRemovedModel model = new HistoricalAccountServiceRemovedModel();
            try
            {

                model = new HistoricalAccountServiceRemoved().GetExportLayout(startDate, endDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/_HistoricalAccountServiceRemovedDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportHistoricalAccountServiceRemoved(HistoricalAccountServiceRemovedModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_HistoricalAccountServiceRemovedTemplate.cshtml",
                new HistoricalAccountServiceRemoved().GetExportLayout(model.StartDate, model.EndDate));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReportOfHistoricalAccountServiceRemoved, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintHistoricalAccountServiceRemoved(DateTime? startDate, DateTime? endDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Account/ExportTemplate/_HistoricalAccountServiceRemovedTemplate.cshtml",
                    new HistoricalAccountServiceRemoved().GetExportLayout(startDate, endDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Property Removed
        public ActionResult AccountPropertyRemoved()
        {
            return View("~/Areas/Reports/Views/Report/Property/AccountPropertyRemoved.cshtml", new AccountPropertyRemoved().Get());
        }

        [HttpPost]
        public ActionResult AccountPropertyRemoved(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            JsonResult jResult = new JsonResult();
            AccountPropertyRemovedModel model = new AccountPropertyRemovedModel();
            try
            {

                model = new AccountPropertyRemoved().GetExportLayout(startDate, endDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/_AccountPropertyRemovedDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportAccountPropertyRemoved(AccountPropertyRemovedModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_AccountPropertyRemovedTemplate.cshtml",
                new AccountPropertyRemoved().GetExportLayout(model.StartDate, model.EndDate));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReportOfAccountPropertyRemoved, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintAccountPropertyRemoved(DateTime? startDate, DateTime? endDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Property/ExportTemplate/_AccountPropertyRemovedTemplate.cshtml",
                    new AccountPropertyRemoved().GetExportLayout(startDate, endDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bank

        #region Export Bank Payments
        public ActionResult ExportBankPayments()
        {
            return View("~/Areas/Reports/Views/Report/Bank/ExportBankPayments.cshtml", new ExportBankPayments().Get());
        }

        [HttpPost]
        public ActionResult ExportBankPayments(DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            ExportBankPaymentsModel model = new ExportBankPaymentsModel();
            try
            {
                model = new ExportBankPayments().GetExportLayout(dueDate, commaSeperatedContractIDs, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_ExportBankPaymentsDetailList.cshtml", model.ExportBankPaymentsList);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_ExportBankPaymentsDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.ExportBankPaymentsList.Rows.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpGet]
        public ActionResult DownloadExportFile()
        {
            if (!string.IsNullOrWhiteSpace(Convert.ToString(Session[Common.S_ExportData])))
            {
                ExportFileViewModel exportFileViewModel = JsonConvert.DeserializeObject<ExportFileViewModel>(Convert.ToString(Session[Common.S_ExportData]));
                Session.Remove(Common.S_ExportData);
                return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
            }
            return Json(new { });
        }

        [HttpGet]
        public ActionResult ExportBankPaymentsToDBF(DateTime dueDate, string commaSeperatedContractIDs)
        {
            bool status = false;
            string message = string.Empty;
            try
            {
                ExportBankPaymentsModel model = new ExportBankPayments().GetExportLayout(dueDate, commaSeperatedContractIDs, 1, int.MaxValue);
                if (model != null && model.ExportBankPaymentsList != null && model.ExportBankPaymentsList.Rows.Count > 0)
                {
                    ExportFileViewModel exportFileviewModel = new ExportFileViewModel()
                    {
                        FileName = string.Format("BankPaymentsDBF_{0}.dbf", DateTime.Now.ToLongTimeString().Replace(":", "").Replace("AM", "").Replace("PM", "")),
                        Data = CreateBankPaymentsDBFFile(model.ExportBankPaymentsList, model.ColumnSchema)
                    };
                    Session[Common.S_ExportData] = JsonConvert.SerializeObject(exportFileviewModel);
                    status = exportFileviewModel.Data != null;
                }
                else
                {
                    status = false;
                    message = Resources.Global.NoDataMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }

        private byte[] CreateBankPaymentsDBFFile(DataTable dtBankPayments, Dictionary<string, int> ColumnSchema)
        {
            OleDbConnection dBaseConnection = null;
            string filepath = null;
            filepath = Server.MapPath("~/SampleImportFile/");// ConfigurationManager.AppSettings["DbfExportPath"];
            string TableName = "T" + DateTime.Now.ToLongTimeString().Replace(":", "").Replace("AM", "").Replace("PM", "");
            try
            {
                using (dBaseConnection = new OleDbConnection(string.Format(ConfigurationManager.AppSettings["AceOledbConnectionString"], filepath)))
                {
                    StringBuilder queryBuilder = new StringBuilder();

                    if (dBaseConnection.State.Equals(ConnectionState.Closed))
                    {
                        dBaseConnection.Open();
                    }

                    using (OleDbCommand olecommand = dBaseConnection.CreateCommand())
                    {
                        queryBuilder.Append("CREATE TABLE [" + TableName + "] (");
                        queryBuilder.Append(String.Join(", ", ColumnSchema.Select(x => string.Format("[{0}] Character({1})", x.Key, x.Value)).ToArray()));
                        queryBuilder.Append(")");

                        olecommand.CommandText = queryBuilder.ToString();
                        olecommand.ExecuteNonQuery();
                        queryBuilder.Clear();
                    }
                    using (OleDbCommand oleinsertCommand = dBaseConnection.CreateCommand())
                    {
                        string insrtstmnt = "INSERT INTO [" + TableName + "] VALUES(";
                        int colcount = dtBankPayments.Columns.Count - 1;
                        string colval = string.Empty;

                        for (int row = 0; row < dtBankPayments.Rows.Count; row++)
                        {
                            queryBuilder.Append(insrtstmnt);
                            for (int col = 0; col <= colcount; col++)
                            {
                                colval = Convert.ToString(dtBankPayments.Rows[row][col]).Replace("'", "''");
                                if (col == colcount) //SINGLE OR LAST RECORD
                                {
                                    queryBuilder.Append(string.Format("'{0}')", colval));
                                }
                                else
                                {
                                    queryBuilder.Append(string.Format("'{0}', ", colval));
                                }
                            }
                            oleinsertCommand.CommandText = queryBuilder.ToString();
                            oleinsertCommand.ExecuteNonQuery();
                            queryBuilder.Clear();
                        }
                    }
                }

                return System.IO.File.ReadAllBytes(filepath + "" + TableName + ".dbf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dBaseConnection.State.Equals(ConnectionState.Open))
                {
                    dBaseConnection.Close();
                    dBaseConnection.Dispose();
                }
                if ((System.IO.File.Exists(filepath + "" + TableName + ".dbf")))
                {
                    System.IO.File.Delete(filepath + "" + TableName + ".dbf");
                }
            }
        }
        #endregion

        #region Revenue Bank Collection
        [HttpGet]
        public ActionResult RevenuefromBankCollection()
        {
            return View("~/Areas/Reports/Views/Report/Bank/RevenuefromBankCollection.cshtml", new CollectionRevenueBankCollection().Get());
        }

        [HttpPost]
        public ActionResult RevenuefromBankCollection(DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize, bool isLoadMore)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            CollectionRevenueBankCollectionModel model = new CollectionRevenueBankCollectionModel();
            try
            {
                model = new CollectionRevenueBankCollection().GetList(startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize);
                if (isLoadMore == true)
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_RevenuefromBankCollectionDetailList.cshtml", model);
                }
                else
                {
                    htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_RevenuefromBankCollectionDetail.cshtml", model);
                }
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.RevenueBankCollectionList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportRevenueByBankCollection(CollectionRevenueBankCollectionModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_RevenuefromBankCollectionTemplate.cshtml",
                new CollectionRevenueBankCollection().GetList(model.StartPeriodDate, model.FilterContractID, model.FilterBankID, model.FilterServiceID, 1, Int32.MaxValue));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.RevenueBankCollectionTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintRevenueByBankCollection(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetRevenueByBankCollectionExportLayout(startPeriod, contractIds, bankIds, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetRevenueByBankCollectionExportLayout(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            CollectionRevenueBankCollectionModel model = new CollectionRevenueBankCollectionModel();
            model = new CollectionRevenueBankCollection().GetList(startPeriod, contractIds, bankIds, serviceIds, 0, Int32.MaxValue);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_RevenuefromBankCollectionTemplate.cshtml", model);
        }
        #endregion

        #region Daily Income Prpoduce Bank
        [HttpGet]
        public ActionResult CollectionDailyIncomeProduceBank()
        {
            return View("~/Areas/Reports/Views/Report/Bank/DailyIncomeProduceBank.cshtml", new CollectionDailyIncomeProduceBank().Get());
        }

        [HttpPost]
        public ActionResult CollectionDailyIncomeProduceBank(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            string htmlstring = null;
            JsonResult jResult = new JsonResult();
            CollectionDailyIncomeProduceBankModel model = new CollectionDailyIncomeProduceBankModel();
            try
            {
                model = new CollectionDailyIncomeProduceBank().GetList(startPeriod, contractIds, bankIds, serviceIds);
                htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_DailyIncomeProduceBankDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring, RowCount = model.DailyIncomeProduceBankList.Count });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportCollectionDailyIncomeProduceBank(CollectionDailyIncomeProduceBankModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_DailyIncomeProduceBankTemplate.cshtml",
                new CollectionDailyIncomeProduceBank().GetList(model.StartPeriodDate, model.FilterContractID, model.FilterBankID, model.FilterServiceID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.DailyIncomeStateTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintDailyIncomeProduceBank(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = GetDailyIncomeProduceBankExportLayout(startPeriod, contractIds, bankIds, serviceIds);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;

            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        public string GetDailyIncomeProduceBankExportLayout(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            CollectionDailyIncomeProduceBankModel model = new CollectionDailyIncomeProduceBankModel();
            model = new CollectionDailyIncomeProduceBank().GetList(startPeriod, contractIds, bankIds, serviceIds);
            return HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_DailyIncomeProduceBankTemplate.cshtml", model);
        }
        #endregion

        #region Statistics of Receipts Collected

        [HttpGet]
        public ActionResult StatisticsofReceiptsCollected()
        {
            return View("~/Areas/Reports/Views/Report/Bank/StatisticsofReceiptsCollected.cshtml", new StatisticsofReceiptsCollected().Get());
        }

        [HttpPost]
        public ActionResult StatisticsofReceiptsCollected(JQDTParams jqdtParams, DateTime? date, int? bankId, string contract)
        {
            JsonResult jResult = new JsonResult();
            StatisticsofReceiptsCollectedModel model = new StatisticsofReceiptsCollectedModel();
            try
            {
                model = new StatisticsofReceiptsCollected().GetExportLayout(date, bankId, contract);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_StatisticsofReceiptsCollectedDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportStatisticsofReceiptsCollected(StatisticsofReceiptsCollectedModel model, int exportType)
        {
            StatisticsofReceiptsCollectedModel _model = new StatisticsofReceiptsCollected().GetExportLayout(model.Date, model.BankID, model.Contract);
            _model.Date = model.Date;
            _model.BankID = model.BankID;
            _model.Contract = model.Contract;

            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_StatisticsofReceiptsCollectedTemplate.cshtml",
                _model);
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.StatisticsOfReceiptsCollected, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintStatisticsofReceiptsCollected(DateTime? date, int? bankId, string contract)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                StatisticsofReceiptsCollectedModel _model = new StatisticsofReceiptsCollected().GetExportLayout(date, bankId, contract);
                _model.Date = date;
                _model.BankID = bankId;
                _model.Contract = contract;

                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_StatisticsofReceiptsCollectedTemplate.cshtml",
                    _model);
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Receipt Collected By Bank
        [HttpGet]
        public ActionResult ReceiptCollectedByBank()
        {
            return View("~/Areas/Reports/Views/Report/Bank/ReceiptCollectedByBank.cshtml", new ReceiptCollectedByBank().Get());
        }

        [HttpPost]
        public ActionResult ReceiptCollectedByBank(JQDTParams jqdtParams, DateTime? startDate, string contractIds, string bankIds, string accountIDs)
        {
            JsonResult jResult = new JsonResult();
            ReceiptCollectedByBankModel model = new ReceiptCollectedByBankModel();
            try
            {
                model = new ReceiptCollectedByBank().GetExportLayout(startDate, contractIds, bankIds, accountIDs);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_ReceiptCollectedByBankDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportReceiptCollectedByBank(ReceiptCollectedByBankModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_ReceiptCollectedByBankTemplate.cshtml",
                new ReceiptCollectedByBank().GetExportLayout(model.StartDate, model.FilterContractID, model.FilterBankID, model.FilterAccountID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ReceiptCollectedByBankTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintReceiptCollectedByBank(DateTime? startDate, string contractIds, string bankIds, string accountIDs)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_ReceiptCollectedByBankTemplate.cshtml",
                    new ReceiptCollectedByBank().GetExportLayout(startDate, contractIds, bankIds, accountIDs));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Control Of Payments Made In The Bank
        [HttpGet]
        public ActionResult ControlOfPaymentsMadeInTheBank()
        {
            return View("~/Areas/Reports/Views/Report/Bank/ControlOfPaymentsMadeInTheBank.cshtml", new ControlOfPaymentsMadeInTheBank().Get());
        }

        [HttpPost]
        public ActionResult ControlOfPaymentsMadeInTheBank(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            JsonResult jResult = new JsonResult();
            ControlOfPaymentsMadeInTheBankModel model = new ControlOfPaymentsMadeInTheBankModel();
            try
            {
                model = new ControlOfPaymentsMadeInTheBank().GetExportLayout(startPeriod, contractIds, bankIds, serviceIds);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_ControlOfPaymentsMadeInTheBankDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportControlOfPaymentsMadeInTheBank(ControlOfPaymentsMadeInTheBankModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_ControlOfPaymentsMadeInTheBankTemplate.cshtml",
              new ControlOfPaymentsMadeInTheBank().GetExportLayout(model.StartPeriodDate, model.FilterContractID, model.FilterBankID, model.FilterServiceID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ControlofPaymentsmadeintheBank, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintControlOfPaymentsMadeInTheBank(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_ControlOfPaymentsMadeInTheBankTemplate.cshtml",
                    new ControlOfPaymentsMadeInTheBank().GetExportLayout(startPeriod, contractIds, bankIds, serviceIds));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Inconsistencies In The Bank Receipts
        [HttpGet]
        public ActionResult InconsistenciesInTheBankReceipts()
        {
            return View("~/Areas/Reports/Views/Report/Bank/InconsistenciesInTheBankReceipts.cshtml", new InconsistenciesInTheBankReceipts().Get());
        }

        [HttpPost]
        public ActionResult InconsistenciesInTheBankReceipts(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            JsonResult jResult = new JsonResult();
            InconsistenciesInTheBankReceiptsModel model = new InconsistenciesInTheBankReceiptsModel();
            try
            {
                model = new InconsistenciesInTheBankReceipts().GetExportLayout(startPeriod, contractIds, bankIds, serviceIds);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/_InconsistenciesInTheBankReceiptsDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportInconsistenciesInTheBankReceipts(InconsistenciesInTheBankReceiptsModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_InconsistenciesInTheBankReceiptsTemplate.cshtml",
              new InconsistenciesInTheBankReceipts().GetExportLayout(model.StartPeriodDate, model.FilterContractID, model.FilterBankID, model.FilterServiceID));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFile((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ListofInconsistenciesintheApplicationofBankReceiptsReport, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        [HttpGet]
        public JsonResult PrintInconsistenciesInTheBankReceipts(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Bank/ExportTemplate/_InconsistenciesInTheBankReceiptsTemplate.cshtml",
                    new InconsistenciesInTheBankReceipts().GetExportLayout(startPeriod, contractIds, bankIds, serviceIds));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Exception Report                
        [HttpGet]
        public ActionResult ExceptionReport()
        {
            return View("~/Areas/Reports/Views/Report/Exception/Exception.cshtml", new ExceptionReport().Get());
        }

        [HttpPost]
        public ActionResult ExceptionReport(JQDTParams jqdtParams, DateTime? Date)
        {
            JsonResult jResult = new JsonResult();
            ExceptionReportModel model = new ExceptionReportModel();
            try
            {
                model = new ExceptionReport().GetExportLayout(Date);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Exception/_ExceptionDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportExceptionReport(ExceptionReportModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Exception/ExportTemplate/_ExceptionTemplate.cshtml",
                new ExceptionReport().GetExportLayout(model.Date));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.ExceptionTitle, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintExceptionReport(DateTime? Date)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Exception/ExportTemplate/_ExceptionTemplate.cshtml",
                    new ExceptionReport().GetExportLayout(Date));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Job Scheduler History
        public ActionResult JobSchedulerHistory()
        {
            return View("~/Areas/Reports/Views/Report/Scheduler/JobSchedulerHistory.cshtml", new JobSchedulerHistory().Get());
        }

        [HttpPost]
        public ActionResult JobSchedulerHistory(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            JsonResult jResult = new JsonResult();
            JobSchedulerHistoryModel model = new JobSchedulerHistoryModel();
            try
            {

                model = new JobSchedulerHistory().GetExportLayout(startDate, endDate);
                string htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Scheduler/_JobSchedulerHistoryDetail.cshtml", model);
                jResult = Json(new { status = true, data = htmlstring });
                jResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                jResult = Json(new { status = false, data = ex.Message });
            }
            return jResult;
        }

        [HttpPost]
        public FileResult ExportJobSchedulerHistory(JobSchedulerHistoryModel model, int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Scheduler/ExportTemplate/_JobSchedulerHistoryTemplate.cshtml",
                new JobSchedulerHistory().GetExportLayout(model.StartDate, model.EndDate));
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Report.JobSchedulerHistory, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }

        [HttpGet]
        public JsonResult PrintJobSchedulerHistory(DateTime? startDate, DateTime? endDate)
        {
            bool status = true;
            string msg = string.Empty;
            try
            {
                msg = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Reports/Views/Report/Scheduler/ExportTemplate/_JobSchedulerHistoryTemplate.cshtml",
                    new JobSchedulerHistory().GetExportLayout(startDate, endDate));
            }
            catch (Exception)
            {
                status = false;
                msg = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage;
            }
            return Json(new { status = status, data = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
    }
}
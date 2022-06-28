using ArtSolutions.MUN.Core.ReportModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Reports.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";

        #region Account

        [Route("AccountStatementGet")]
        [HttpGet]
        public IHttpActionResult AccountStatementGet(int accountId, int? accountPropertyID, int? tillYear,int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AccountStatementGet(companyId, language, accountId, accountPropertyID, tillYear, tillPeriod, tillDate, accountServiceCollectionDetailIDs, isExport, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountStatementGet", language, ex));
            }
        }

        [Route("HistoricalAccountServiceRemovedGet")]
        [HttpGet]
        public IHttpActionResult HistoricalAccountServiceRemovedGet(Guid removedByaccountID, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().HistoricalAccountServiceRemovedGet(companyId, language, removedByaccountID, startDate, endDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.HistoricalAccountServiceRemovedGet", language, ex));
            }
        }

        [Route("AccountPropertyRemovedGet")]
        [HttpGet]
        public IHttpActionResult AccountPropertyRemovedGet(Guid removedByaccountID, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AccountPropertyRemovedGet(companyId, language, removedByaccountID, startDate, endDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountPropertyRemovedGet", language, ex));
            }
        }

        [Route("SummaryAccountStatementGet")]
        [HttpGet]
        public IHttpActionResult SummaryAccountStatementGet(int accountId, int? accountPropertyID, string commaSeperatedYearIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().SummaryAccountStatementGet(companyId, language, accountId, accountPropertyID, commaSeperatedYearIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.SummaryAccountStatementGet", language, ex));
            }
        }

        [Route("AdministrativeCollectionNoticeGet")]
        [HttpGet]
        public IHttpActionResult AdministrativeCollectionNoticeGet(int accountId, bool isFirstNotice, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AdministrativeCollectionNoticeGet(companyId, language, accountId, isFirstNotice, notificationExpirationDate, notificationDate, representativesName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AdministrativeCollectionNoticeGet", language, ex));
            }
        }

        [Route("PaymentPlanByTaxpayerGet")]
        [HttpGet]
        public IHttpActionResult PaymentPlanByTaxpayerGet(int accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().PaymentPlanByTaxpayerGet(companyId, language, accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.PaymentPlanByTaxpayerGet", language, ex));
            }
        }

        [Route("AccountServicesMissingFilingGet")]
        [HttpGet]
        public IHttpActionResult AccountServicesMissingFilingGet(int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AccountServicesMissingFilingGet(companyID, language, accountId, commaSeperatedServiceIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountServicesMissingFilingGet", language, ex));
            }
        }

        [Route("GeneralMovementsGet")]
        [HttpGet]
        public IHttpActionResult GeneralMovementsGet(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().GeneralMovementsGet(companyId, language, userID, accountID, fromDate, toDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.GeneralMovementsGet", language, ex));
            }
        }
        
        [Route("AccountByServiceType")]
        [HttpGet]
        public IHttpActionResult AccountByServiceType(int serviceTypeID, bool isNotAssignServices)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AccountByServiceType(companyId, language, serviceTypeID, isNotAssignServices);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountByServiceType", language, ex));
            }
        }
        #endregion

        #region "Certification"

        [Route("BusinessLicenseStatementGet")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseStatementGet(int AccountId, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseStatementGet(companyId, AccountId, PageIndex, PageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseStatementGet", language, ex));
            }
        }

        [Route("BusinessLicenseStatementGetForHP")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseStatementGetForHP(int AccountId, int PageIndex, int PageSize, DateTime FutureDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseStatementGetForHP(companyId, AccountId, PageIndex, PageSize, FutureDate);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseStatementGetForHP", language, ex));
            }
        }

        [Route("DebtCertification")]
        [HttpGet]
        public IHttpActionResult DebtCertification(int accountID, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().DebtCertification(companyId, language, accountID, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.DebtCertification", language, ex));
            }
        }

        [Route("NoDebtCertification")]
        [HttpPost]
        public IHttpActionResult NoDebtCertification(NoDebtCertificationModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new Report().NoDebtCertification(companyId, language, model.AccountId, model.PageIndex, model.PageSize);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.NoDebtCertification", language, ex));
            }
        }

        [Route("ProjectedAccountStatementGet")]
        [HttpGet]
        public IHttpActionResult ProjectedAccountStatementGet(int accountID, DateTime? tillDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ProjectedAccountStatementGet(companyId, language, accountID, tillDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ProjectedAccountStatementGet", language, ex));
            }
        }

        #endregion

        #region "IVU"

        [Route("IVUBalanceSheetGet")]
        [HttpGet]
        public IHttpActionResult IVUBalanceSheetGet(DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? rangeFrom, decimal? rangeTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUBalanceSheetGet(companyId, startPeriod, accountIDs, endPeriod, rangeFrom, rangeTo, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUBalanceSheetGet", language, ex));
            }
        }

        [Route("IVUFormsNotFiledGet")]
        [HttpGet]
        public IHttpActionResult IVUFormsNotFiledGet(string accountIDs, int? sinceMonth, int? sinceYear, int? tillMonth, int? tilleYear, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUFormsNotFiledGet(companyId, accountIDs, sinceMonth, sinceYear, tillMonth, tilleYear, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUFormsNotFiledGet", language, ex));
            }
        }

        [Route("IVUStatementGet")]
        [HttpGet]
        public IHttpActionResult IVUStatementGet(int AccountId, decimal? BalanceFrom, decimal? BalanceTo, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUStatementGet(companyId, AccountId, BalanceFrom, BalanceTo, PageIndex, PageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUStatementGet", language, ex));
            }
        }

        [Route("IVUStatementGetForHP")]
        [HttpGet]
        public IHttpActionResult IVUStatementGetForHP(int AccountId, decimal? BalanceFrom, decimal? BalanceTo, int PageIndex, int PageSize, DateTime FutureDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUStatementGetForHP(companyId, AccountId, BalanceFrom, BalanceTo, PageIndex, PageSize, FutureDate);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUStatementGetForHP", language, ex));
            }
        }

        [Route("IVUFilledCertificateGet")]
        [HttpGet]
        public IHttpActionResult IVUFilledCertificateGet(int AccountId, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUFilledCertificateGet(companyId, AccountId, language, PageIndex, PageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUFilledCertificateGet", language, ex));
            }
        }

        [Route("IVUFilledCertificateGetForHP")]
        [HttpGet]
        public IHttpActionResult IVUFilledCertificateGetForHP(int AccountId, int PageIndex, int PageSize, DateTime FutureDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUFilledCertificateGetForHP(companyId, AccountId, language, PageIndex, PageSize, FutureDate);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUFilledCertificateGetForHP", language, ex));
            }
        }

        [Route("IVUCollectionDetailGet")]
        [HttpGet]
        public IHttpActionResult IVUCollectionDetailGet(DateTime StartPeriodDate, DateTime EndPeriodDate, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().IVUCollectionDetailGet(companyId, StartPeriodDate, EndPeriodDate, PageIndex, PageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.IVUCollectionDetailGet", language, ex));
            }
        }

        #endregion

        #region Water
        [Route("HistoricalReadingsMeterGet")]
        [HttpGet]
        public IHttpActionResult HistoricalReadingsMeterGet(string meter, int? accountID, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().HistoricalReadingsMeterGet(companyId, language, meter, accountID, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.HistoricalReadingsMeterGet", language, ex));
            }
        }

        [Route("ConsumptionByRangeGet")]
        [HttpGet]
        public IHttpActionResult ConsumptionByRangeGet(DateTime fromDate, DateTime toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ConsumptionByRangeGet(companyId, language, fromDate, toDate, from, to, taxNumber, meter, conditionFrom, conditionTo, conditionType, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ConsumptionByRangeGet", language, ex));
            }
        }

        [Route("InconsistenceReadingGet")]
        [HttpGet]
        public IHttpActionResult InconsistenceReadingGet(int month, int year, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().InconsistenceReadingGet(companyId, language, month, year, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.InconsistenceReadingGet", language, ex));
            }
        }

        [Route("AccountsConsumptionAndCollectionGet")]
        [HttpGet]
        public IHttpActionResult AccountsConsumptionAndCollectionGet(int month, int year)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AccountsConsumptionAndCollectionGet(companyId, month, year);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountsConsumptionAndCollectionGet", language, ex));
            }
        }

        [Route("DuplicatedMeterNumberGet")]
        [HttpGet]
        public IHttpActionResult DuplicatedMeterNumberGet(int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().DuplicatedMeterNumberGet(companyId, language, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.DuplicatedMeterNumberGet", language, ex));
            }
        }

        [Route("RoutaMissingServicesGet")]
        [HttpGet]
        public IHttpActionResult RoutaMissingServicesGet(int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().RoutaMissingServicesGet(companyId, language, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.RoutaMissingServicesGet", language, ex));
            }
        }

        [Route("SuspensionOrderForWaterServiceGet")]
        [HttpGet]
        public IHttpActionResult SuspensionOrderForWaterServiceGet(int accountID, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().SuspensionOrderForWaterServiceGet(companyId, language, accountID, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.SuspensionOrderForWaterServiceGet", language, ex));
            }
        }
        #endregion

        #region Patent Report

        [Route("BusinessLicenseBalance")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseBalance(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseBalance(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseBalance", language, ex));
            }
        }

        [Route("BusinessLicenseFilingbyFilingType")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseFilingbyFilingType(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseFilingbyFilingType(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseFilingbyFilingType", language, ex));
            }
        }

        [Route("BusinessLicenseCreditBalance")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseCreditBalance(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseCreditBalance(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseCreditBalance", language, ex));
            }
        }

        [Route("BusinessLicenseBalanceByClosingOperation")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseBalanceByClosingOperation(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseBalanceByClosingOperation(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseBalanceByClosingOperation", language, ex));
            }
        }

        [Route("BusinessLicenseTransaction")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseTransaction(DateTime? startDate, DateTime? endDate, string commaSeperatedAccountIds, int pageIndex, int pageSize,int? searchStatus)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseTransaction(companyID, startDate, endDate, commaSeperatedAccountIds, pageIndex, pageSize, language, searchStatus);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseBalance", language, ex));
            }
        }

        [Route("BusinessLicenceSanitaryPermitDueDate")]
        [HttpGet]
        public IHttpActionResult BusinessLicenceSanitaryPermitDueDate(DateTime? fromDate, DateTime? toDate, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenceSanitaryPermitDueDate(companyID, language, fromDate, toDate, commaSeperatedAccountIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenceSanitaryPermitDueDate", language, ex));
            }
        }

        [Route("BusinessLicenseGeneral")]
        [HttpGet]
        public IHttpActionResult BusinessLicenseGeneral(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().BusinessLicenseGeneral(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.BusinessLicenseGeneral", language, ex));
            }
        }
        #endregion

        #region "Payment Receipts"

        [Route("ReceiptRegisterGet")]
        [HttpGet]
        public IHttpActionResult ReceiptRegisterGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptRegisterGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptRegisterGet", language, ex));
            }
        }

        [Route("ReceiptRegisterGetForCR")]
        [HttpGet]
        public IHttpActionResult ReceiptRegisterGetForCR(DateTime? startDate, DateTime? endDate, int? type, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptRegisterGetForCR(companyID, startDate, endDate, type, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptRegisterGetForCR", language, ex));
            }
        }

        [Route("ReceiptDetailGet")]
        [HttpGet]
        public IHttpActionResult ReceiptDetailGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptDetailGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptDetailGet", language, ex));
            }
        }

        [Route("ReceiptCommercialFacilitiesRentalGet")]
        [HttpGet]
        public IHttpActionResult ReceiptCommercialFacilitiesRentalGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptCommercialFacilitiesRentalGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptCommercialFacilitiesRentalGet", language, ex));
            }
        }

        [Route("ReceiptByIVUGet")]
        [HttpGet]
        public IHttpActionResult ReceiptByIVUGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptByIVUGet(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptByIVUGet", language, ex));
            }
        }

        [Route("ReceiptByBusinessLicenseGet")]
        [HttpGet]
        public IHttpActionResult ReceiptByBusinessLicenseGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptByBusinessLicenseGet(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptByBusinessLicenseGet", language, ex));
            }
        }

        [Route("ReceiptByPaymentPlanGet")]
        [HttpGet]
        public IHttpActionResult ReceiptByPaymentPlanGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, string commaSeperatedPaymentPlanIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptByPaymentPlanGet(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, commaSeperatedPaymentPlanIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptByPaymentPlanGet", language, ex));
            }
        }

        [Route("ReceiptByOtherConceptGet")]
        [HttpGet]
        public IHttpActionResult ReceiptByOtherConceptGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptByOtherConceptGet(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptByOtherConceptGet", language, ex));
            }
        }

        [Route("ReceiptVoidGet")]
        [HttpGet]
        public IHttpActionResult ReceiptVoidGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptVoidGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptVoidGet", language, ex));
            }
        }

        [Route("ReceiptBySecurityBankAccountGet")]
        [HttpGet]
        public IHttpActionResult ReceiptBySecurityBankAccountGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptBySecurityBankAccountGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptBySecurityBankAccountGet", language, ex));
            }
        }

        [Route("ReceiptBankGet")]
        [HttpGet]
        public IHttpActionResult ReceiptBankGet(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptBankGet(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptBankGet", language, ex));
            }
        }

        [Route("ReceiptPermissionFees")]
        [HttpGet]
        public IHttpActionResult ReceiptPermissionFees(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptPermissionFees(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptPermissionFees", language, ex));
            }
        }

        [Route("ReceiptPermissionExpenses")]
        [HttpGet]
        public IHttpActionResult ReceiptPermissionExpenses(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptPermissionExpenses(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptPermissionExpenses", language, ex));
            }
        }

        [Route("ReceiptStreetVendors")]
        [HttpGet]
        public IHttpActionResult ReceiptStreetVendors(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptStreetVendors(companyID, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ReceiptStreetVendors", language, ex));
            }
        }



        [Route("MigrationValidationFactuGet")]
        [HttpGet]
        public IHttpActionResult MigrationValidationFactuGet(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().MigrationValidationFactuGet(companyID, language, typeID, serviceCodes, accountIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.MigrationValidationFactuGet", language, ex));
            }
        }
        [Route("MigrationValidationCobrosGet")]
        [HttpGet]
        public IHttpActionResult MigrationValidationCobrosGet(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().MigrationValidationCobrosGet(companyID, language, typeID, serviceCodes, accountIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.MigrationValidationCobrosGet", language, ex));
            }
        }

        [Route("MigrationValidationBienesGet")]
        [HttpGet]
        public IHttpActionResult MigrationValidationBienesGet(int? typeID, string fincaIDs, string accountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().MigrationValidationBienesGet(companyID, language, typeID, fincaIDs, accountIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.MigrationValidationBienesGet", language, ex));
            }
        }

        [Route("PaymentsMadeByTheTaxpayerGet")]
        [HttpGet]
        public IHttpActionResult PaymentsMadeByTheTaxpayerGet(DateTime? startDate, DateTime? endDate, string accountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().PaymentsMadeByTheTaxpayerGet(companyID, startDate, endDate, accountIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.PaymentsMadeByTheTaxpayerGet", language, ex));
            }
        }

        #endregion

        #region "Daily Cash Register"

        [Route("CollectionClosingSummaryGet")]
        [HttpGet]
        public IHttpActionResult CollectionClosingSummaryGet(DateTime StartPeriodDate, DateTime EndPeriodDate, string CommaSeperatedCashierIds, decimal? NetClosingFrom, decimal? NetClosingTo, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionClosingSummaryGet(companyId, StartPeriodDate, EndPeriodDate, CommaSeperatedCashierIds, NetClosingFrom, NetClosingTo, PageIndex, PageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionClosingSummaryGet", language, ex));
            }
        }

        [Route("ClosingSummaryByServiceType")]
        [HttpGet]
        public IHttpActionResult ClosingSummaryByServiceType(DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedServiceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CashRegisterSummaryByServiceType(companyId, startPeriodDate, endPeriodDate, commaSeperatedServiceTypeIDs, netClosingFrom, netClosingTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CashRegisterSummaryByServiceType", language, ex));
            }
        }

        [Route("ClosingSummaryByPaymentType")]
        [HttpGet]
        public IHttpActionResult ClosingSummaryByPaymentType(DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentTypeIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ClosingSummaryByPaymentType(companyId, startPeriodDate, endPeriodDate, commaSeperatedPaymentTypeIDs, netClosingFrom, netClosingTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ClosingSummaryByPaymentType", language, ex));
            }
        }

        [Route("ClosingSummaryByInvoice")]
        [HttpGet]
        public IHttpActionResult ClosingSummaryByInvoice(DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string commaSeperatedOtherServiceIDs, string commaSeperatedGrantIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ClosingSummaryByInvoice(companyId, startPeriodDate, endPeriodDate, netClosingFrom, netClosingTo, commaSeperatedOtherServiceIDs, commaSeperatedGrantIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ClosingSummaryByInvoice", language, ex));
            }
        }

        [Route("ClosingSummaryByPaymentPlan")]
        [HttpGet]
        public IHttpActionResult ClosingSummaryByPaymentPlan(DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ClosingSummaryByPaymentPlan(companyId, startPeriodDate, endPeriodDate, commaSeperatedPaymentPlanIDs, netClosingFrom, netClosingTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ClosingSummaryByPaymentPlan", language, ex));
            }
        }

        [Route("CashReceiptControlGet")]
        [HttpGet]
        public IHttpActionResult CashReceiptControlGet(DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CashReceiptControlGet(companyId, language, startDate, endDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CashReceiptControlGet", language, ex));
            }
        }

        [Route("CollectionDailyIncomeStateGet")]
        [HttpGet]
        public IHttpActionResult CollectionDailyIncomeStateGet(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDailyIncomeStateGet(companyID, language, startPeriod, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDailyIncomeStateGet", language, ex));
            }
        }

        [Route("MonthlyIncomeStatementByServicesGet")]
        [HttpGet]
        public IHttpActionResult MonthlyIncomeStatementByServicesGet(DateTime closingDate, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().MonthlyIncomeStatementByServicesGet(companyId, language, closingDate, commaSeperatedServiceIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.MonthlyIncomeStatementByServicesGet", language, ex));
            }
        }

        [Route("QuarterlyIncomeStatementByServicesGet")]
        [HttpGet]
        public IHttpActionResult QuarterlyIncomeStatementByServicesGet(int quarter, int year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().QuarterlyIncomeStatementByServicesGet(companyId, language, quarter, year, commaSeperatedServiceIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.QuarterlyIncomeStatementByServicesGet", language, ex));
            }
        }

        [Route("YearlyIncomeStatementByServicesGet")]
        [HttpGet]
        public IHttpActionResult YearlyIncomeStatementByServicesGet(int year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().YearlyIncomeStatementByServicesGet(companyId, language, year, commaSeperatedServiceIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.YearlyIncomeStatementByServicesGet", language, ex));
            }
        }

        [Route("RevenueByDailyCollectionGet")]
        [HttpGet]
        public IHttpActionResult RevenueByDailyCollectionGet(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().RevenueByDailyCollectionGet(companyID, language, startPeriod, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.RevenueByDailyCollectionGet", language, ex));
            }
        }

        [Route("HistoricalPaymentReportGet")]
        [HttpGet]
        public IHttpActionResult HistoricalPaymentReportGet(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().HistoricalPaymentReportGet(companyId, language, fromDate, toDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.HistoricalPaymentReportGet", language, ex));
            }
        }

        [Route("AmnestyMovementReportGet")]
        [HttpGet]
        public IHttpActionResult AmnestyMovementReportGet(DateTime fromDate, DateTime toDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().AmnestyMovementReportGet(companyId, language, fromDate, toDate);
                return Ok(model); 
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AmnestyMovementReportGet", language, ex));
            }
        }


        #endregion

        #region "Deposit Entry"

        [Route("CollectionDepositSummaryGet")]
        [HttpGet]
        public IHttpActionResult CollectionDepositSummaryGet(DateTime StartPeriodDate, DateTime EndPeriodDate, decimal? NetDepositFrom, decimal? NetDepositTo, string commaSeperatedBankAccountIDs, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDepositSummaryGet(companyId, StartPeriodDate, EndPeriodDate, NetDepositFrom, NetDepositTo, commaSeperatedBankAccountIDs, PageIndex, PageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDepositSummaryGet", language, ex));
            }
        }

        [Route("CollectionDepositSummaryByPaymentTypeGet")]
        [HttpGet]
        public IHttpActionResult CollectionDepositSummaryByPaymentTypeGet(DateTime StartPeriodDate, DateTime EndPeriodDate, string commaSeperatedPaymentTypeIDs, string commaSeperatedBankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDepositSummaryByPaymentTypeGet(companyId, StartPeriodDate, EndPeriodDate, commaSeperatedPaymentTypeIDs, commaSeperatedBankAccountIDs, NetDepositFrom, NetDepositTo, PageIndex, PageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDepositSummaryByPaymentTypeGet", language, ex));
            }
        }

        [Route("CollectionDepositSummaryByServiceTypeGet")]
        [HttpGet]
        public IHttpActionResult CollectionDepositSummaryByServiceTypeGet(DateTime StartPeriodDate, DateTime EndPeriodDate, string commaSeperatedServiceTypeIDs, string commaSeperatedBankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo, int PageIndex, int PageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDepositSummaryByServiceTypeGet(companyId, StartPeriodDate, EndPeriodDate, commaSeperatedServiceTypeIDs, commaSeperatedBankAccountIDs, NetDepositFrom, NetDepositTo, PageIndex, PageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDepositSummaryByServiceTypeGet", language, ex));
            }
        }

        [Route("CollectionDepositSummaryByInvoiceGet")]
        [HttpGet]
        public IHttpActionResult CollectionDepositSummaryByInvoiceGet(DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedBankAccountIds, decimal? netDepositFrom, decimal? netDepositTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDepositSummaryByInvoiceGet(companyId, startPeriodDate, endPeriodDate, commaSeperatedBankAccountIds, netDepositFrom, netDepositTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDepositSummaryByInvoiceGet", language, ex));
            }
        }

        [Route("CollectionDepositSummaryByPaymentPlanGet")]
        [HttpGet]
        public IHttpActionResult CollectionDepositSummaryByPaymentPlanGet(DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIds, string commaSeperatedBankAccountIds, decimal? netDepositFrom, decimal? netDepositTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDepositSummaryByPaymentPlanGet(companyId, startPeriodDate, endPeriodDate, commaSeperatedPaymentPlanIds, commaSeperatedBankAccountIds, netDepositFrom, netDepositTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDepositSummaryByPaymentPlanGet", language, ex));
            }
        }

        #endregion

        #region JournalDetail 

        [Route("JournalDetailGet")]
        [HttpGet]
        public IHttpActionResult JournalDetailGet(DateTime? StartPeriodDate, DateTime? EndPeriodDate, string DocumentTypeID, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo, int? ReferrenceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().JournalDetailGet(companyId, StartPeriodDate, EndPeriodDate, DocumentTypeID, accountIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo, ReferrenceID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.JournalDetailGet", language, ex));
            }
        }

        #endregion

        #region Journal 

        [Route("JournalGet")]
        [HttpGet]
        public IHttpActionResult JournalGet(DateTime StartPeriodDate, DateTime EndPeriodDate, string DocumentTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().JournalGet(companyId, StartPeriodDate, EndPeriodDate, DocumentTypeID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.JournalDetailGet", language, ex));
            }
        }

        #endregion

        #region Property
        [Route("PropertyMovementGet")]
        [HttpGet]
        public IHttpActionResult PropertyMovementGet(int accountId, string accountPropertyID, decimal? rangeFrom, decimal? rangeTo, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().PropertyMovementGet(companyId, accountId, accountPropertyID, rangeFrom, rangeTo, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.PropertyMovementGet", language, ex));
            }
        }

        [Route("TransfersReport")]
        [HttpGet]
        public IHttpActionResult TransfersReport(DateTime? fromDate, DateTime? toDate, int? transferTypeID, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().TransfersReport(companyID, language, fromDate, toDate, transferTypeID, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.TransfersReport", language, ex));
            }
        }

        [Route("AccountExclusionForm")]
        [HttpPost]
        public IHttpActionResult AccountExclusionForm(AccountExclusionFormModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new Report().AccountExclusionForm(companyId, language, model.AccountId);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.AccountExclusionForm", language, ex));
            }
        }

        [Route("ParkMaintenanceMissingServicesGet")]
        [HttpGet]
        public IHttpActionResult ParkMaintenanceMissingServicesGet(int year, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ParkMaintenanceMissingServicesGet(companyID, language, year, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ParkMaintenanceMissingServicesGet", language, ex));
            }
        }

        [Route("ServicesLinkedToRightGet")]
        [HttpGet]
        public IHttpActionResult ServicesLinkedToRightGet(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ServicesLinkedToRightGet(companyID, language, typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ServicesLinkedToRightGet", language, ex));
            }
        }

        [Route("RightsByOwnersID")]
        [HttpGet]
        public IHttpActionResult RightsByOwnersID(int? ownerID, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new Report().RightsByOwnersID(companyId, language, ownerID, pageIndex, pageSize);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.RightsByOwnersID", language, ex));
            }
        }
        #endregion

        #region Bank
        [Route("ExportBankPaymentsGet")]
        [HttpGet]
        public IHttpActionResult ExportBankPaymentsGet(DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ExportBankPaymentsGet(companyId, language, dueDate, commaSeperatedContractIDs, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ExportBankPaymentsGet", language, ex));
            }
        }

        [Route("RevenueByBankCollectionGet")]
        [HttpGet]
        public IHttpActionResult RevenueByBankCollectionGet(DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().RevenueByBankCollectionGet(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.RevenueByDailyCollectionGet", language, ex));
            }
        }

        [Route("CollectionDailyIncomeProduceBankGet")]
        [HttpGet]
        public IHttpActionResult CollectionDailyIncomeProduceBankGet(DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().CollectionDailyIncomeProduceBankGet(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.CollectionDailyIncomeStateGet", language, ex));
            }
        }

        [Route("StatisticsofReceiptsCollectedGet")]
        [HttpGet]
        public IHttpActionResult StatisticsofReceiptsCollectedGet(DateTime date, int bankId, string contract, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().StatisticsofReceiptsCollectedGet(companyID, language, date, bankId, contract, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.StatisticsofReceiptsCollectedGet", language, ex));
            }
        }

        [Route("ReceiptCollectedByBankGet")]
        [HttpGet]
        public IHttpActionResult ReceiptCollectedByBankGet(DateTime? startDate, string contractIds, string bankIds, string accountIDs, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ReceiptCollectedByBankGet(companyID, startDate, contractIds, bankIds, accountIDs, pageIndex, pageSize, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.PaymentsMadeByTheTaxpayerGet", language, ex));
            }
        }

        [Route("ControlOfPaymentsMadeInTheBankGet")]
        [HttpGet]
        public IHttpActionResult ControlOfPaymentsMadeInTheBankGet(DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ControlOfPaymentsMadeInTheBankGet(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ControlOfPaymentsMadeInTheBankGet", language, ex));
            }
        }

        [Route("InconsistenciesInTheBankReceiptsGet")]
        [HttpGet]
        public IHttpActionResult InconsistenciesInTheBankReceiptsGet(DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var model = new Report().InconsistenciesInTheBankReceiptsGet(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.InconsistenciesInTheBankReceiptsGet", language, ex));
            }
        }
        #endregion

        #region Exception
        [Route("ExceptionGet")]
        [HttpGet]
        public IHttpActionResult ExceptionGet(DateTime Date, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().ExceptionsGet(companyId, language, Date, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.ExceptionGet", language, ex));
            }
        }
        #endregion

        #region Job Scheduler
        [Route("JobSchedulerHistoryGet")]
        [HttpGet]
        public IHttpActionResult JobSchedulerHistoryGet(DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Report().JobSchedulerHistoryGet(language, startDate, endDate, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ReportModels.Report.JobSchedulerHistoryGet", language, ex));
            }
        }
        #endregion
    }
}

using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.ReportModels
{
    public class Report
    {
        #region Account    
        public AccountStatementModel AccountStatementGet(int companyId, string language, int accountId, int? accountPropertyID, int? tillYear, int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport, int pageIndex, int pageSize)
        {

            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountStatementModel model = new AccountStatementModel();

                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyId",Value=companyId},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@AccountId",Value=accountId},
                    new SqlParameter { ParameterName = "@AccountPropertyID",Value = (accountPropertyID == null ? (object)DBNull.Value : accountPropertyID)},
                    new SqlParameter { ParameterName = "@TillYear",Value = (tillYear == null ? (object)DBNull.Value : tillYear)},
                    new SqlParameter { ParameterName = "@TillPeriod",Value = (tillPeriod == null ? (object)DBNull.Value : tillPeriod)},
                    new SqlParameter { ParameterName = "@TillDate",Value = (tillDate == null ? (object)DBNull.Value : tillDate)},
                    new SqlParameter { ParameterName = "@AccountServiceCollectionDetailIDs",Value=(string.IsNullOrEmpty(accountServiceCollectionDetailIDs) ? "" : accountServiceCollectionDetailIDs)},
                    new SqlParameter { ParameterName = "@IsExport",Value=isExport},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[11].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPAccountStatement");
                model.IsInJudicialCollection = ds.Tables[0].ToList<AccountStatement>().FirstOrDefault().IsInJudicialCollection ?? null;
                model.AccountPaymentPlanNames = ds.Tables[1].ToList<AccountStatement>().FirstOrDefault().AccountPaymentPlanNames ?? null;
                model.AccountStatementList = ds.Tables[2].ToList<AccountStatement>() ?? new List<AccountStatement>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[10].Value);
                return model;
            }
        }
        public HistoricalAccountServiceRemovedModel HistoricalAccountServiceRemovedGet(int companyId, string language, Guid removedByaccountID, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                HistoricalAccountServiceRemovedModel model = new HistoricalAccountServiceRemovedModel();
                model.HistoricalAccountServiceRemovedList = context.MUNSERREPAccountServiceRemove(companyId, startDate, endDate, pageIndex, pageSize, language, totalRecord).ToList() ?? new List<MUNSERREPAccountServiceRemove_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public AccountPropertyRemovedModel AccountPropertyRemovedGet(int companyId, string language, Guid removedByaccountID, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountPropertyRemovedModel model = new AccountPropertyRemovedModel();
                model.AccountPropertyRemovedList = context.MUNREPAccountPropertyRemove(companyId, startDate, endDate, pageIndex, pageSize, language, totalRecord).ToList() ?? new List<MUNREPAccountPropertyRemove_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public SummaryAccountStatementModel SummaryAccountStatementGet(int companyId, string language, int accountId, int? accountPropertyID, string commaSeperatedYearIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                SummaryAccountStatementModel model = new SummaryAccountStatementModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyId",Value=companyId},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@AccountId",Value=accountId},
                    new SqlParameter { ParameterName = "@AccountPropertyID",Value = (accountPropertyID == null ? (object)DBNull.Value : accountPropertyID)},
                    new SqlParameter { ParameterName = "@CommaSeperatedYearIDs",Value=(string.IsNullOrEmpty(commaSeperatedYearIDs) ? "" : commaSeperatedYearIDs)},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[7].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPSummaryAccountStatement");
                model.IsInJudicialCollection = ds.Tables[0].ToList<SummaryAccountStatement>().FirstOrDefault().IsInJudicialCollection ?? null;
                model.SummaryAccountStatementList = ds.Tables[1].ToList<SummaryAccountStatement>() ?? new List<SummaryAccountStatement>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[7].Value);
                return model;
            }
        }
        public AdministrativeCollectionNoticeModel AdministrativeCollectionNoticeGet(int companyId, string language, int accountId, bool isFirstNotice, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                AdministrativeCollectionNoticeModel model = new AdministrativeCollectionNoticeModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyId},
                    new SqlParameter { ParameterName = "@Language",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value= accountId},
                    new SqlParameter { ParameterName = "@IsFirstNotice",Value=isFirstNotice},
                    new SqlParameter { ParameterName = "@NotificationExpirationDate",Value= notificationExpirationDate != null?notificationExpirationDate : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NotificationDate",Value= notificationDate != null?notificationDate : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@RepresentativesName",Value=(!string.IsNullOrEmpty(representativesName)) ? representativesName : (object)DBNull.Value},
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPNoticeOfAdministrativeCollection");
                model.AccountPropertyList = ds.Tables[0].ToList<AccountProperty>();
                model.SummaryAccountStatementList = ds.Tables[1].ToList<SummaryAccountStatement>();
                return model;
            }
        }
        public PaymentPlanByTaxpayerModel PaymentPlanByTaxpayerGet(int companyID, string language, int accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                PaymentPlanByTaxpayerModel model = new PaymentPlanByTaxpayerModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Locale ",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@AccountPaymentPlanIDs",Value=accountPaymentPlanIDs!=null ? accountPaymentPlanIDs : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@TillYear",Value=tillYear!=null ? tillYear : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@TillDate",Value=tillDate!=null ? tillDate : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@QuotasToCalculate",Value=quotasToCalculate!=null ? quotasToCalculate : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@RowNos",Value=rowNos!=null ? rowNos : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@IsTermDetail",Value=isTermDetail!=null ? isTermDetail : (object)DBNull.Value},
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPPaymentPlanByTaxpayer");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();

                if (isTermDetail == false)
                {
                    model.AccountPaymentPlanServiceDetailList = ds.Tables[1].ToList<AccountPaymentPlanServiceDetailList>() ?? new List<AccountPaymentPlanServiceDetailList>();
                }
                else
                {
                    model.AccountPaymentPlanServiceDetailList = new List<AccountPaymentPlanServiceDetailList>();
                }

                if (isTermDetail == true)
                {
                    model.PaymentPlanByTaxpayerList = ds.Tables[1].ToList<PaymentPlanByTaxpayerList>() ?? new List<PaymentPlanByTaxpayerList>();
                }
                else
                {
                    model.PaymentPlanByTaxpayerList = new List<PaymentPlanByTaxpayerList>();
                }

                if (isTermDetail == null)
                {
                    model.AccountPaymentPlanServiceDetailList = ds.Tables[1].ToList<AccountPaymentPlanServiceDetailList>() ?? new List<AccountPaymentPlanServiceDetailList>();
                    model.PaymentPlanByTaxpayerList = ds.Tables[2].ToList<PaymentPlanByTaxpayerList>() ?? new List<PaymentPlanByTaxpayerList>();
                    model.MinMOROSAS = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
                    model.IsInJudicialCollection = ds.Tables[4].ToList<PaymentPlanByTaxpayerList>().FirstOrDefault().IsInJudicialCollection ?? null;
                    model.MinAPager = Convert.ToInt32(ds.Tables[5].Rows[0][0]);
                }
                else
                {
                    model.MinMOROSAS = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    model.IsInJudicialCollection = ds.Tables[3].ToList<PaymentPlanByTaxpayerList>().FirstOrDefault().IsInJudicialCollection ?? null;
                    model.MinAPager = Convert.ToInt32(ds.Tables[4].Rows[0][0]);
                }


                return model;
            }
        }
        public AccountServicesMissingFilingModel AccountServicesMissingFilingGet(int companyID, string language, int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountServicesMissingFilingModel list = new AccountServicesMissingFilingModel();
                list.AccountServicesMissingFilingList = context.MUNSERREPAccountServicesMissingFiling(companyID, language, accountId, commaSeperatedServiceIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public GeneralMovementsModel GeneralMovementsGet(int companyId, string language, Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 10000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                GeneralMovementsModel model = new GeneralMovementsModel();
                model.GeneralMovementsList = context.MUNSERREPGeneralMovements(companyId, language, userID, accountID, fromDate, toDate, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNSERREPGeneralMovements_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public AccountByServiceTypeModel AccountByServiceType(int companyID, string language, int servicetypeID,bool isNotAssignServices)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                AccountByServiceTypeModel model = new AccountByServiceTypeModel();
                model.AccountByServiceTypeList = context.MUNREPAccountByServicesType(companyID,language,servicetypeID, isNotAssignServices).ToList() ?? new List<MUNREPAccountByServicesType_Result>();
                return model;
            }
        }
        #endregion

        #region "Certification"
        public BusinessLicenseStatementModel BusinessLicenseStatementGet(int CompanyId, int AccountId, int PageIndex, int PageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseStatementModel model = new BusinessLicenseStatementModel();
                model.BusinessLicenseCertificateModelList = context.MUNSERREPBusinessLicenseStatement(CompanyId, AccountId, PageIndex, PageSize, totalRecord).ToList() ?? new List<MUNSERREPBusinessLicenseStatement_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseStatementModel BusinessLicenseStatementGetForHP(int CompanyId, int AccountId, int PageIndex, int PageSize, DateTime FutureDate)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseStatementModel model = new BusinessLicenseStatementModel();
                model.BusinessLicenseCertificateModelList = context.MUNSERREPBusinessLicenseStatementForHP(CompanyId, AccountId, PageIndex, PageSize, totalRecord, FutureDate).ToList() ?? new List<MUNSERREPBusinessLicenseStatement_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public DebtCertificationModel DebtCertification(int companyID, string language, int accountID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                DebtCertificationModel model = new DebtCertificationModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPDebtCertification");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.AccountPropertyList = ds.Tables[1].ToList<AccountPropertyModel>() ?? new List<AccountPropertyModel>();
                model.DebtCertificationList = ds.Tables[2].ToList<DebtCertificationDetailModel>() ?? new List<DebtCertificationDetailModel>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        public NoDebtCertificationModel NoDebtCertification(int companyID, string language, int accountID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                NoDebtCertificationModel model = new NoDebtCertificationModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPNoDebtCertification");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.NoDebtCertificationPeriodModel = ds.Tables[1].ToList<NoDebtCertificationPeriodModel>().FirstOrDefault() ?? new NoDebtCertificationPeriodModel();
                model.NoDebtCertificationList = ds.Tables[2].ToList<NoDebtCertificationDetailModel>() ?? new List<NoDebtCertificationDetailModel>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        public ProjectedAccountStatementModel ProjectedAccountStatementGet(int companyID, string language, int accountID, DateTime? tillDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ProjectedAccountStatementModel model = new ProjectedAccountStatementModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@TillDate",Value=tillDate},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[6].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPProjectedAccountStatement");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.ProjectedAccountStatementList = ds.Tables[1].ToList<MUNREPProjectedAccountStatement_Result>() ?? new List<MUNREPProjectedAccountStatement_Result>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[6].Value);
                return model;
            }
        }
        #endregion

        #region "IVU"
        public IVUBalanceSheetModel IVUBalanceSheetGet(int companyID, DateTime startPeriod, string accountIDs, DateTime endPeriod, decimal? rangeFrom, decimal? rangeTo, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUBalanceSheetModel list = new IVUBalanceSheetModel();

                list.IVUBalanceSheetList = context.MUNSERREPIVUBalanceSheet(companyID, startPeriod, endPeriod, accountIDs, rangeFrom, rangeTo, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public IVUFormsNotFiledModel IVUFormsNotFiledGet(int companyID, string accountIDs, int? sinceMonth, int? sinceYear, int? tillMonth, int? tilleYear, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUFormsNotFiledModel list = new IVUFormsNotFiledModel();

                list.IVUFormsNotFiledList = context.MUNSERREPIVUFormsNotFiled(companyID, accountIDs, sinceMonth, sinceYear, tillMonth, tilleYear, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public IVUStatementModel IVUStatementGet(int CompanyId, int AccountId, decimal? BalanceFrom, decimal? BalanceTo, int PageIndex, int PageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUStatementModel model = new IVUStatementModel();
                model.IVUStatementList = context.MUNSERREPIVUStatement(CompanyId, AccountId, BalanceFrom, BalanceTo, PageIndex, PageSize, totalRecord).ToList() ?? new List<MUNSERREPIVUStatement_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public IVUStatementModel IVUStatementGetForHP(int CompanyId, int AccountId, decimal? BalanceFrom, decimal? BalanceTo, int PageIndex, int PageSize, DateTime FutureDate)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUStatementModel model = new IVUStatementModel();
                model.IVUStatementList = context.MUNSERREPIVUStatementForHP(CompanyId, AccountId, BalanceFrom, BalanceTo, PageIndex, PageSize, totalRecord, FutureDate).ToList() ?? new List<MUNSERREPIVUStatement_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public IVUFillingCertificateModel IVUFilledCertificateGet(int CompanyId, int AccountId, string Language, int PageIndex, int PageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUFillingCertificateModel model = new IVUFillingCertificateModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyId",Value=CompanyId},
                    new SqlParameter { ParameterName = "@AccountId",Value=AccountId},
                    new SqlParameter { ParameterName = "@Language",Value=Language},
                    new SqlParameter { ParameterName = "@PageIndex",Value=PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=PageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue},
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPIVUFilingCertificate");
                model.ReportCompanyModel = ds.Tables[0].ToList<ReportCompanyModel>().FirstOrDefault() ?? new ReportCompanyModel();
                model.IVUFilledCertificateList = ds.Tables[1].ToList<IVUFillingCertificateDetailModel>() ?? new List<IVUFillingCertificateDetailModel>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        public IVUFillingCertificateModel IVUFilledCertificateGetForHP(int CompanyId, int AccountId, string Language, int PageIndex, int PageSize, DateTime FutureDate)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUFillingCertificateModel model = new IVUFillingCertificateModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyId",Value=CompanyId},
                    new SqlParameter { ParameterName = "@AccountId",Value=AccountId},
                    new SqlParameter { ParameterName = "@Language",Value=Language},
                    new SqlParameter { ParameterName = "@PageIndex",Value=PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=PageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue},
                    new SqlParameter { ParameterName = "@FutureDate",Value=FutureDate}
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPIVUFilingCertificateForHP");
                model.ReportCompanyModel = ds.Tables[0].ToList<ReportCompanyModel>().FirstOrDefault() ?? new ReportCompanyModel();
                model.IVUFilledCertificateList = ds.Tables[1].ToList<IVUFillingCertificateDetailModel>() ?? new List<IVUFillingCertificateDetailModel>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        public IVUCollectionDetailModel IVUCollectionDetailGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, int PageIndex, int PageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUCollectionDetailModel model = new IVUCollectionDetailModel();
                model.IVUCollectionDetailList = context.MUNSERREPIVUCollectionDetail(CompanyId, StartPeriodDate, EndPeriodDate, PageIndex, PageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        #endregion

        #region Water         
        public HistoricalReadingsMeterModel HistoricalReadingsMeterGet(int companyId, string language, string meter, int? accountID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                HistoricalReadingsMeterModel model = new HistoricalReadingsMeterModel();
                model.HistoricalReadingsMeterList = context.MUNREPHistoricalReadingsByMeter(companyId, language, meter, accountID, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNREPHistoricalReadingsByMeter_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public ConsumptionRangeModel ConsumptionByRangeGet(int companyId, string language, DateTime fromDate, DateTime toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ConsumptionRangeModel model = new ConsumptionRangeModel();
                model.ConsumptionRangeList = context.MUNREPConsumptionByRange(companyId, language, fromDate, toDate, conditionFrom, from, conditionType, conditionTo, to, taxNumber, meter, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNREPConsumptionByRange_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public InconsistenceReadingModel InconsistenceReadingGet(int companyId, string language, int month, int year, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 5500;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ObjectParameter totalConsumption = new ObjectParameter("TotalConsumption", typeof(Int32));
                ObjectParameter totalAmount = new ObjectParameter("TotalAmount", typeof(decimal));
                InconsistenceReadingModel model = new InconsistenceReadingModel();
                model.InconsistenceReadingList = context.MUNREPInconsistenceReading(companyId, month, year, pageIndex, pageSize, language, totalRecord, totalConsumption, totalAmount).ToList() ?? new List<MUNREPInconsistenceReading_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                model.TotalConsumption = Convert.ToInt32(totalConsumption.Value);
                model.TotalAmount = Convert.ToDecimal(totalAmount.Value);
                return model;
            }
        }
        public AccountsConsumptionAndCollectionModel AccountsConsumptionAndCollectionGet(int companyID, int month, int year)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                AccountsConsumptionAndCollectionModel model = new AccountsConsumptionAndCollectionModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Month",Value=month},
                    new SqlParameter { ParameterName = "@Year",Value=year}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPAccountsConsumptionAndCollection");

                model.ColumnList = ds.Tables[0].ToList<AccountsConsumptionAndCollectionColumnModel>() ?? new List<AccountsConsumptionAndCollectionColumnModel>();

                model.DomesticWaterServiceList = ds.Tables[1];
                model.DomesticWaterServiceList.Columns.Remove("ServiceID");

                model.OrdinaryWaterServiceList = ds.Tables[2];
                model.OrdinaryWaterServiceList.Columns.Remove("ServiceID");

                model.ReproductiveWaterServiceList = ds.Tables[3];
                model.ReproductiveWaterServiceList.Columns.Remove("ServiceID");

                model.PreferedWaterServiceList = ds.Tables[4];
                model.PreferedWaterServiceList.Columns.Remove("ServiceID");

                model.GovernmentWaterServiceList = ds.Tables[5];
                model.GovernmentWaterServiceList.Columns.Remove("ServiceID");

                model.SummaryList = ds.Tables[6];

                return model;
            }
        }
        public DuplicatedMeterNumberModel DuplicatedMeterNumberGet(int companyId, string language, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                DuplicatedMeterNumberModel model = new DuplicatedMeterNumberModel();
                model.DuplicatedMeterNumberList = context.MUNREPDuplicatedMeterNumber(companyId, language, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNREPDuplicatedMeterNumber_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public RoutaMissingServicesModel RoutaMissingServicesGet(int companyId, string language, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                RoutaMissingServicesModel model = new RoutaMissingServicesModel();
                model.RoutaMissingServicesList = context.MUNREPRoutaMissingServices(companyId, language, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNREPRoutaMissingServices_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public SuspensionOrderForWaterServiceModel SuspensionOrderForWaterServiceGet(int companyID, string language, int accountID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                SuspensionOrderForWaterServiceModel model = new SuspensionOrderForWaterServiceModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPSuspensionOrderForWaterService");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.SuspensionOrderForWaterServiceList = ds.Tables[1].ToList<SuspensionOrderForWaterServiceList>() ?? new List<SuspensionOrderForWaterServiceList>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        #endregion

        #region Patent Report Detail
        public BusinessLicenseBalanceModel BusinessLicenseBalance(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseBalanceModel model = new BusinessLicenseBalanceModel();
                model.BusinessLicenseBalanceList = context.MUNSERREPBusinessLicenceBalance(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseFilingByFilingTypeModel BusinessLicenseFilingbyFilingType(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseFilingByFilingTypeModel model = new BusinessLicenseFilingByFilingTypeModel();
                model.BusinessLicenseFilingByFilingTypeList = context.MUNSERREPBusinessLicenceFilingByFiling(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, PageIndex, PageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseCreditBalanceModel BusinessLicenseCreditBalance(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseCreditBalanceModel model = new BusinessLicenseCreditBalanceModel();
                model.BusinessLicenseCreditBalanceList = context.MUNSERREPBusinessLicenceCreditBalance(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseBalanceByClosingModel BusinessLicenseBalanceByClosingOperation(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseBalanceByClosingModel model = new BusinessLicenseBalanceByClosingModel();
                model.BusinessLicenseBalanceByClosingList = context.MUNSERREPBusinessLicenceBalanceByClosing(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseTransactionModel BusinessLicenseTransaction(int CompanyId, DateTime? startDate, DateTime? endDate, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language, int? searchStatus)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseTransactionModel model = new BusinessLicenseTransactionModel();
                model.BusinessLicenseTransactionList = context.MUNSERREPBusinessLicenceTransaction(CompanyId, startDate, endDate, commaSeperatedAccountIds, PageIndex, PageSize, Language, searchStatus, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenceSanitaryPermitDueDateModel BusinessLicenceSanitaryPermitDueDate(int companyID, string language, DateTime? fromDate, DateTime? toDate, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenceSanitaryPermitDueDateModel model = new BusinessLicenceSanitaryPermitDueDateModel();
                model.BusinessLicenceSanitaryPermitDueDateList = context.MUNREPBusinessLicenceSanitaryPermitDueDate(companyID, language, fromDate, toDate, commaSeperatedAccountIDs, pageIndex, pageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseGeneralModel BusinessLicenseGeneral(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseGeneralModel model = new BusinessLicenseGeneralModel();
                model.BusinessLicenseGeneralList = context.MUNREPBusinessLicenceGeneral(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        #endregion

        #region Payment Receipts
        public ReceiptRegisterModel ReceiptRegisterGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptRegisterModel list = new ReceiptRegisterModel();
                list.ReceiptRegisterList = context.MUNSERREPReceiptRegister(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptRegisterForCRModel ReceiptRegisterGetForCR(int companyID, DateTime? startDate, DateTime? endDate, int? type, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptRegisterForCRModel list = new ReceiptRegisterForCRModel();
                list.ReceiptRegisterList = context.MUNSERREPReceiptRegisterForCR(companyID, startDate, endDate, type, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptDetailModel ReceiptDetailGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptDetailModel list = new ReceiptDetailModel();
                list.ReceiptDetailList = context.MUNSERREPReceiptDetail(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptCommercialFacilitiesRentalModel ReceiptCommercialFacilitiesRentalGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptCommercialFacilitiesRentalModel list = new ReceiptCommercialFacilitiesRentalModel();
                list.ReceiptCommercialFacilitiesRentalList = context.MUNSERREPReceiptCommercialFacilitiesRental(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public IVUReceiptDetailModel ReceiptByIVUGet(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                IVUReceiptDetailModel model = new IVUReceiptDetailModel();
                model.IVUReceiptDetailList = context.MUNSERREPReceiptIVU(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public BusinessLicenseReceiptDetailModel ReceiptByBusinessLicenseGet(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                BusinessLicenseReceiptDetailModel model = new BusinessLicenseReceiptDetailModel();
                model.BusinessLicenseReceiptDetailList = context.MUNSERREPReceiptBusinessLicense(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public PaymentPlanReceiptDetailModel ReceiptByPaymentPlanGet(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, string commaSeperatedPaymentPlanIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PaymentPlanReceiptDetailModel model = new PaymentPlanReceiptDetailModel();
                model.PaymentPlanReceiptDetailList = context.MUNSERREPReceiptPaymentPlan(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, commaSeperatedPaymentPlanIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public OtherConceptReceiptDetailModel ReceiptByOtherConceptGet(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                OtherConceptReceiptDetailModel model = new OtherConceptReceiptDetailModel();
                model.OtherConceptReceiptDetailList = context.MUNSERREPReceiptOtherConcept(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public VoidReceiptsModel ReceiptVoidGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())

            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                VoidReceiptsModel list = new VoidReceiptsModel();
                list.ReceiptEntryList = context.MUNSERREPReceiptVoid(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public SecurityBankAccountModel ReceiptBySecurityBankAccountGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                SecurityBankAccountModel list = new SecurityBankAccountModel();
                list.SecurityBankAccountList = context.MUNSERREPSecuritiesByBank(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptBankModel ReceiptBankGet(int companyID, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptBankModel list = new ReceiptBankModel();
                list.ReceiptByBankList = context.MUNSERREPReceiptBank(companyID, startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs, pageIndex, pageSize, language, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptPermissionFeesModel ReceiptPermissionFees(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptPermissionFeesModel model = new ReceiptPermissionFeesModel();
                model.ReceiptPermissionFeesList = context.MUNSERREPReceiptPermissionFees(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public ReceiptPermissionExpensesModel ReceiptPermissionExpenses(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptPermissionExpensesModel model = new ReceiptPermissionExpensesModel();
                model.ReceiptPermissionExpensesList = context.MUNSERREPReceiptPermissionExpenses(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public ReceiptPermissionStreetVendorsModel ReceiptStreetVendors(int CompanyId, DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptPermissionStreetVendorsModel model = new ReceiptPermissionStreetVendorsModel();
                model.ReceiptPermissionStreetVendorsList = context.MUNSERREPReceiptStreetVendors(CompanyId, startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, PageIndex, PageSize, Language, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public MigrationValidationFactuModel MigrationValidationFactuGet(int companyID, string language, int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                MigrationValidationFactuModel list = new MigrationValidationFactuModel();
                list.MigrationValidationFactuList = context.MUNREPMigrationValidationFactu(companyID, language, typeID, serviceCodes, accountIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }

        public CollectionDailyIncomeStateModel CollectionDailyIncomeStateGet(int companyID, string language, DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CollectionDailyIncomeStateModel list = new CollectionDailyIncomeStateModel();
                list.DailyIncomeStateList = context.MUNSERREPDailyIncomeState(companyID, language, startPeriod, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public CollectionRevenueDailyCollectionModel RevenueByDailyCollectionGet(int companyID, string language, DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CollectionRevenueDailyCollectionModel list = new CollectionRevenueDailyCollectionModel();
                list.RevenueDailyCollectionList = context.MUNSERREPDailyCollection(companyID, language, startPeriod, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public MigrationValidationCobrosModel MigrationValidationCobrosGet(int companyID, string language, int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                MigrationValidationCobrosModel list = new MigrationValidationCobrosModel();
                list.MigrationValidationCobrosList = context.MUNREPMigrationValidationCobros(companyID, language, typeID, serviceCodes, accountIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public MigrationValidationBienesModel MigrationValidationBienesGet(int companyID, string language, int? typeID, string fincaIDs, string accountIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                MigrationValidationBienesModel list = new MigrationValidationBienesModel();
                list.MigrationValidationBienesList = context.MUNREPMigrationValidationBienes(companyID, language, typeID, fincaIDs, accountIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public PaymentsMadeByTheTaxpayerModel PaymentsMadeByTheTaxpayerGet(int companyID, DateTime? startDate, DateTime? endDate, string accountIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PaymentsMadeByTheTaxpayerModel list = new PaymentsMadeByTheTaxpayerModel();
                list.PaymentsMadeByTheTaxpayerList = context.MUNSERREPListOfPaymentsMadeByTheTaxpayer(companyID, language, startDate, endDate, accountIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        #endregion

        #region Daily Cash Register
        public CollectionClosingSummaryModel CollectionClosingSummaryGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, string CommaSeperatedCashierIDs, decimal? netClosingFrom, decimal? netClosingTo, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= CompanyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= StartPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= EndPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedCashierIDs",Value= !string.IsNullOrEmpty(CommaSeperatedCashierIDs)  ? CommaSeperatedCashierIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingFrom",Value= netClosingFrom.HasValue ? netClosingFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingTo",Value= netClosingTo.HasValue ? netClosingTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= PageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=Language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPClosingSummary");
                CollectionClosingSummaryModel model = new CollectionClosingSummaryModel();
                model.ClosingEntryList = ds.Tables[0].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalClosingRecord = int.Parse(ds.Tables[1].Rows[0]["TotalClosingRecord"].ToString());
                }

                model.PaymentList = ds.Tables[2].ToList<PaymentModel>().ToList() ?? new List<PaymentModel>();
                return model;
            }
        }
        public ClosingSummaryServiceTypeModel CashRegisterSummaryByServiceType(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedServiceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize, string locale)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedServiceTypeIDs",Value= !string.IsNullOrEmpty(commaSeperatedServiceTypeIDs)  ? commaSeperatedServiceTypeIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingFrom",Value= netClosingFrom.HasValue ? netClosingFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingTo",Value= netClosingTo.HasValue ? netClosingTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=locale}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPClosingSummaryByServiceType");
                ClosingSummaryServiceTypeModel model = new ClosingSummaryServiceTypeModel();
                model.ClosingEntryList = ds.Tables[0].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalClosingRecord = int.Parse(ds.Tables[1].Rows[0]["TotalClosingRecord"].ToString());
                }

                model.PaymentList = ds.Tables[2].ToList<PaymentModel>().ToList() ?? new List<PaymentModel>();
                return model;
            }
        }
        public ClosingSummaryPaymentTypeModel ClosingSummaryByPaymentType(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentTypeIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize, string locale)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedPaymentTypeIDs",Value= !string.IsNullOrEmpty(commaSeperatedPaymentTypeIDs)  ? commaSeperatedPaymentTypeIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingFrom",Value= netClosingFrom.HasValue ? netClosingFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingTo",Value= netClosingTo.HasValue ? netClosingTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=locale}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPClosingSummaryByPaymentType");
                ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
                model.ClosingEntryList = ds.Tables[0].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalClosingRecord = int.Parse(ds.Tables[1].Rows[0]["TotalClosingRecord"].ToString());
                }

                model.PaymentList = ds.Tables[2].ToList<PaymentModel>().ToList() ?? new List<PaymentModel>();
                return model;
            }
        }
        public ClosingSummaryPaymentTypeModel ClosingSummaryByInvoice(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string commaSeperatedOtherServiceIDs, string commaSeperatedGrantIDs, int pageIndex, int pageSize, string locale)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@NetClosingFrom",Value= netClosingFrom.HasValue ? netClosingFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingTo",Value= netClosingTo.HasValue ? netClosingTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CommaSeperatedOtherServiceIDs",Value= (!string.IsNullOrEmpty(commaSeperatedOtherServiceIDs) ? commaSeperatedOtherServiceIDs : (object)DBNull.Value)},
                    new SqlParameter { ParameterName = "@CommaSeperatedGrantIDs",Value= (!string.IsNullOrEmpty(commaSeperatedGrantIDs) ? commaSeperatedGrantIDs : (object)DBNull.Value)},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=locale}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPClosingSummaryByInvoice");
                ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
                model.ClosingEntryList = ds.Tables[0].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalClosingRecord = int.Parse(ds.Tables[1].Rows[0]["TotalClosingRecord"].ToString());
                }

                model.PaymentList = ds.Tables[2].ToList<PaymentModel>().ToList() ?? new List<PaymentModel>();
                return model;
            }
        }
        public ClosingSummaryPaymentTypeModel ClosingSummaryByPaymentPlan(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo, int pageIndex, int pageSize, string locale)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedPaymentPlanIDs",Value= !string.IsNullOrEmpty(commaSeperatedPaymentPlanIDs)  ? commaSeperatedPaymentPlanIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingFrom",Value= netClosingFrom.HasValue ? netClosingFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetClosingTo",Value= netClosingTo.HasValue ? netClosingTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=locale}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPClosingSummaryByPaymentPlan");
                ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
                model.ClosingEntryList = ds.Tables[0].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalClosingRecord = int.Parse(ds.Tables[1].Rows[0]["TotalClosingRecord"].ToString());
                }

                model.PaymentList = ds.Tables[2].ToList<PaymentModel>().ToList() ?? new List<PaymentModel>();
                return model;
            }
        }
        public CashReceiptControlModel CashReceiptControlGet(int companyID, string language, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CashReceiptControlModel list = new CashReceiptControlModel();

                list.CashReceiptControlList = context.MUNREPCashReceiptControl(companyID, language, startDate, endDate, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public QuarterlyIncomeStatementByServicesModel QuarterlyIncomeStatementByServicesGet(int companyID, string language, int quarter, int year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                QuarterlyIncomeStatementByServicesModel list = new QuarterlyIncomeStatementByServicesModel();
                list.QuarterlyIncomeStatementByServicesList = context.MUNREPQuarterlyIncomeStatementByServices(companyID, language, quarter, year, commaSeperatedServiceIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public YearlyIncomeStatementByServicesModel YearlyIncomeStatementByServicesGet(int companyID, string language, int year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                YearlyIncomeStatementByServicesModel list = new YearlyIncomeStatementByServicesModel();
                list.YearlyIncomeStatementByServicesList = context.MUNREPYearlyIncomeStatementByServices(companyID, language, year, commaSeperatedServiceIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public MonthlyIncomeStatementByServicesModel MonthlyIncomeStatementByServicesGet(int companyID, string language, DateTime closingDate, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                MonthlyIncomeStatementByServicesModel list = new MonthlyIncomeStatementByServicesModel();
                list.MonthlyIncomeStatementByServicesList = context.MUNREPMonthlyIncomeStatementByServices(companyID, language, closingDate, commaSeperatedServiceIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public HistoricalPaymentReportModel HistoricalPaymentReportGet(int companyID, string language, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                HistoricalPaymentReportModel model = new HistoricalPaymentReportModel();
                model.HistoricalPaymentList = context.MUNSERREPOneTimePayment(companyID, language, fromDate, toDate, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNSERREPOneTimePayment_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public AmnestyMovementReportModel AmnestyMovementReportGet(int companyID, string language, DateTime? fromDate, DateTime? toDate)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AmnestyMovementReportModel model = new AmnestyMovementReportModel();
                model.AmnestyMovementReportList = context.MUNSERREPReceiptAmnestyMovement(companyID, fromDate, toDate, language, totalRecord).ToList() ?? new List<MUNSERREPReceiptAmnestyMovement_Result>();
                model.TotalRecord = 0;
                return model;
            }
        }


        #endregion

        #region Deposit Entry
        public CollectionDepositSummaryModel CollectionDepositSummaryGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, decimal? netDepositFrom, decimal? netDepositTo, string commaSeperatedBankAccountIDs, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= CompanyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= StartPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= EndPeriodDate},
                    new SqlParameter { ParameterName = "@NetDepositFrom",Value= netDepositFrom.HasValue ? netDepositFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositTo",Value= netDepositTo.HasValue ? netDepositTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CommaSeperatedBankAccountIds",Value= (!string.IsNullOrEmpty(commaSeperatedBankAccountIDs)) ? commaSeperatedBankAccountIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= PageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=Language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPDepositSummary");
                CollectionDepositSummaryModel model = new CollectionDepositSummaryModel();
                model.DepositEntryList = ds.Tables[0].ToList<DepositEntryModel>().ToList() ?? new List<DepositEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalDepositRecord = int.Parse(ds.Tables[1].Rows[0]["TotalDepositRecord"].ToString());
                }

                model.ClosingEntryList = ds.Tables[2].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        public CollectionDepositSummaryPaymentTypeModel CollectionDepositSummaryByPaymentTypeGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, string commaSeperatedPaymentTypeIDs, string commaSeperatedBankAccountIDs, decimal? netDepositFrom, decimal? netDepositTo, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= CompanyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= StartPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= EndPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedPaymentTypeIDs",Value= !string.IsNullOrEmpty(commaSeperatedPaymentTypeIDs)  ? commaSeperatedPaymentTypeIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositFrom",Value= netDepositFrom.HasValue ? netDepositFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositTo",Value= netDepositTo.HasValue ? netDepositTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CommaSeperatedBankAccountIds",Value= (!string.IsNullOrEmpty(commaSeperatedBankAccountIDs)) ? commaSeperatedBankAccountIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= PageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=Language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPDepositSummaryByPaymentType");
                CollectionDepositSummaryPaymentTypeModel model = new CollectionDepositSummaryPaymentTypeModel();
                model.DepositEntryList = ds.Tables[0].ToList<DepositEntryModel>().ToList() ?? new List<DepositEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalDepositRecord = int.Parse(ds.Tables[1].Rows[0]["TotalDepositRecord"].ToString());
                }

                model.ClosingEntryList = ds.Tables[2].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        public CollectionDepositSummaryServiceTypeModel CollectionDepositSummaryByServiceTypeGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, string commaSeperatedServiceTypeIDs, string commaSeperatedBankAccountIDs, decimal? netDepositFrom, decimal? netDepositTo, int PageIndex, int PageSize, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= CompanyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= StartPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= EndPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedServiceTypeIDs",Value= !string.IsNullOrEmpty(commaSeperatedServiceTypeIDs)  ? commaSeperatedServiceTypeIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@commaSeperatedBankAccountIDs",Value= !string.IsNullOrEmpty(commaSeperatedBankAccountIDs)  ? commaSeperatedBankAccountIDs : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositFrom",Value= netDepositFrom.HasValue ? netDepositFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositTo",Value= netDepositTo.HasValue ? netDepositTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= PageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= PageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=Language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPDepositSummaryByServiceType");
                CollectionDepositSummaryServiceTypeModel model = new CollectionDepositSummaryServiceTypeModel();
                model.DepositEntryList = ds.Tables[0].ToList<DepositEntryModel>().ToList() ?? new List<DepositEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalDepositRecord = int.Parse(ds.Tables[1].Rows[0]["TotalDepositRecord"].ToString());
                }

                model.ClosingEntryList = ds.Tables[2].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        public CollectionDepositSummaryByInvoiceModel CollectionDepositSummaryByInvoiceGet(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedBankAccountIds, decimal? netDepositFrom, decimal? netDepositTo, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedBankAccountIds",Value= !string.IsNullOrEmpty(commaSeperatedBankAccountIds)  ? commaSeperatedBankAccountIds : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositFrom",Value= netDepositFrom.HasValue ? netDepositFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositTo",Value= netDepositTo.HasValue ? netDepositTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPDepositSummaryByInvoice");
                CollectionDepositSummaryByInvoiceModel model = new CollectionDepositSummaryByInvoiceModel();
                model.DepositEntryList = ds.Tables[0].ToList<DepositEntryModel>().ToList() ?? new List<DepositEntryModel>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalDepositRecord = int.Parse(ds.Tables[1].Rows[0]["TotalDepositRecord"].ToString());
                }

                model.ClosingEntryList = ds.Tables[2].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        public CollectionDepositSummaryByPaymentPlanModel CollectionDepositSummaryByPaymentPlanGet(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIds, string commaSeperatedBankAccountIds, decimal? netDepositFrom, decimal? netDepositTo, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@StartPeriod",Value= startPeriodDate},
                    new SqlParameter { ParameterName = "@EndPeriod",Value= endPeriodDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedPaymentPlanIds",Value= !string.IsNullOrEmpty(commaSeperatedPaymentPlanIds)  ? commaSeperatedPaymentPlanIds : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CommaSeperatedBankAccountIds",Value= !string.IsNullOrEmpty(commaSeperatedBankAccountIds)  ? commaSeperatedBankAccountIds : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositFrom",Value= netDepositFrom.HasValue ? netDepositFrom.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@NetDepositTo",Value= netDepositTo.HasValue ? netDepositTo.Value : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PageIndex",Value= pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value= pageSize},
                    new SqlParameter { ParameterName = "@Locale",Value=language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERREPDepositSummaryByPaymentPlan");
                CollectionDepositSummaryByPaymentPlanModel model = new CollectionDepositSummaryByPaymentPlanModel();
                model.DepositEntryList = ds.Tables[0].ToList<DepositEntryModel>().ToList() ?? new List<DepositEntryModel>();
                model.ClosingEntryList = ds.Tables[2].ToList<ClosingEntryModel>().ToList() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        #endregion

        #region JournalDetail 
        public List<MUNACCJournalDetailGet_Result> JournalDetailGet(int CompanyId, DateTime? StartPeriodDate, DateTime? EndPeriodDate, string DocumentTypeID, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo, int? ReferrenceID, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                return context.MUNACCJournalDetailGet(CompanyId, accountIDs, Language, StartPeriodDate, EndPeriodDate, DocumentTypeID, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo, ReferrenceID).ToList();
            }
        }
        #endregion

        #region Journal 
        public List<MUNACCJournalGet_Result> JournalGet(int CompanyId, DateTime StartPeriodDate, DateTime EndPeriodDate, string DocumentTypeID, string Language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                return context.MUNACCJournalGet(CompanyId, Language, StartPeriodDate, EndPeriodDate, DocumentTypeID).ToList();
            }
        }
        #endregion

        #region Property         
        public PropertyModel PropertyMovementGet(int companyId, int accountId, string accountPropertyID, decimal? rangeFrom, decimal? rangeTo, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PropertyModel model = new PropertyModel();
                model.PropertyMovementList = context.MUNREPPropertyMovementByAccount(companyId, accountId, accountPropertyID, rangeFrom, rangeTo, pageIndex, pageSize, language, totalRecord).ToList() ?? new List<MUNREPPropertyMovementByAccount_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public TransfersReportModel TransfersReport(int companyID, string language, DateTime? fromDate, DateTime? toDate, int? transferTypeID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                TransfersReportModel model = new TransfersReportModel();
                model.TransfersReportList = context.MUNREPTransfersReport(companyID, language, fromDate, toDate, transferTypeID, pageIndex, pageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public AccountExclusionFormModel AccountExclusionForm(int companyID, string language, int accountID)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                AccountExclusionFormModel model = new AccountExclusionFormModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPAccountExclusionForm");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.PropertyTaxList = ds.Tables[1].ToList<PropertyTaxList>() ?? new List<PropertyTaxList>();
                model.WaterMeasureList = ds.Tables[2].ToList<WaterMeasureList>() ?? new List<WaterMeasureList>();
                model.OtherServicesList = ds.Tables[3].ToList<OtherServicesList>() ?? new List<OtherServicesList>();
                model.HistoricalList = ds.Tables[4].ToList<HistoricalList>() ?? new List<HistoricalList>();
                return model;
            }
        }
        public ParkMaintenanceMissingServicesModel ParkMaintenanceMissingServicesGet(int companyID, string language, int year, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ParkMaintenanceMissingServicesModel model = new ParkMaintenanceMissingServicesModel();
                model.ParkMaintenanceMissingServicesList = context.MUNREPParkMaintenanceMissingServices(companyID, language, year, pageIndex, pageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public ServicesLinkedToRightModel ServicesLinkedToRightGet(int companyID, string language, int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ServicesLinkedToRightModel model = new ServicesLinkedToRightModel();
                model.ServicesLinkedToRightList = context.MUNREPServicesLinkedToRight(companyID, language, typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, pageIndex, pageSize, totalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        public RightsByOwnersIDModel RightsByOwnersID(int companyID, string language, int? ownerID, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                RightsByOwnersIDModel model = new RightsByOwnersIDModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@OwnerID",Value=ownerID},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    new SqlParameter { ParameterName = "@TotalRecord",Value=Int32.MinValue}
                };
                sqlparameters[5].Direction = ParameterDirection.Output;
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPListOfRightsByOwnersID");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.RightsByOwnersIDDetailModel = ds.Tables[1].ToList<RightsByOwnersIDDetailModel>().FirstOrDefault() ?? new RightsByOwnersIDDetailModel();
                model.RightsByOwnersIDList = ds.Tables[2].ToList<RightsByOwnersIDList>() ?? new List<RightsByOwnersIDList>();
                model.TotalRecord = Convert.ToInt32(sqlparameters[5].Value);
                return model;
            }
        }
        #endregion

        #region Bank
        public ExportBankPaymentsModel ExportBankPaymentsGet(int companyId, string language, DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 30000;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@TotalRecord", Value = 5, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

                ExportBankPaymentsModel model = new ExportBankPaymentsModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyId",Value=companyId},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@DueDate",Value=dueDate},
                    new SqlParameter { ParameterName = "@CommaSeperatedContractIDs",Value= (string.IsNullOrEmpty(commaSeperatedContractIDs) ? "" : commaSeperatedContractIDs),SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@PageIndex",Value=pageIndex},
                    new SqlParameter { ParameterName = "@PageSize",Value=pageSize},
                    outputparam
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNREPExportBankPayments", true);
                if (ds.Tables.Count > 0)
                {
                    model.ExportBankPaymentsList = ds.Tables[0];
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            model.ColumnSchema.Add(col.ColumnName, col.MaxLength);
                        }
                    }
                }
                else
                {
                    model.ExportBankPaymentsList = new DataTable();
                }

                model.TotalRecord = Convert.ToInt32(sqlparameters[6].Value);

                return model;
            }
        }
        public CollectionRevenueBankCollectionModel RevenueByBankCollectionGet(int companyID, string language, DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CollectionRevenueBankCollectionModel list = new CollectionRevenueBankCollectionModel();
                list.RevenueBankCollectionList = context.MUNSERREPBankCollection(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public CollectionDailyIncomeProduceBankModel CollectionDailyIncomeProduceBankGet(int companyID, string language, DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CollectionDailyIncomeProduceBankModel list = new CollectionDailyIncomeProduceBankModel();
                list.DailyIncomeProduceBankList = context.MUNSERREPDailyIncomeProduceBank(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public StatisticsofReceiptsCollectedModel StatisticsofReceiptsCollectedGet(int companyID, string language, DateTime date, int bankId, string contract, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                StatisticsofReceiptsCollectedModel list = new StatisticsofReceiptsCollectedModel();
                list.StatisticsofReceiptsCollectedList = context.MUNSERREPStatisticsofReceiptsCollected(companyID, language, date, bankId, contract, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ReceiptCollectedByBankModel ReceiptCollectedByBankGet(int companyID, DateTime? startDate, string contractIds, string bankIds, string accountIDs, int pageIndex, int pageSize, string language)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ReceiptCollectedByBankModel list = new ReceiptCollectedByBankModel();
                list.ReceiptCollectedByBankList = context.MUNSERREPListOfReceiptCollectedBank(companyID, language, startDate, contractIds, bankIds, accountIDs, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public ControlOfPaymentsMadeInTheBankModel ControlOfPaymentsMadeInTheBankGet(int companyID, string language, DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ControlOfPaymentsMadeInTheBankModel list = new ControlOfPaymentsMadeInTheBankModel();
                list.ControlOfPaymentsMadeInTheBankList = context.MUNSERREPControlOfPaymentsMadeInTheBank(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        public InconsistenciesInTheBankReceiptsModel InconsistenciesInTheBankReceiptsGet(int companyID, string language, DateTime startPeriod, string contractIds, string bankIds, string serviceIds, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                InconsistenciesInTheBankReceiptsModel list = new InconsistenciesInTheBankReceiptsModel();
                list.InconsistenciesInTheBankReceiptsList = context.MUNSERREPInconsistenciesInTheBankReceipts(companyID, language, startPeriod, contractIds, bankIds, serviceIds, pageIndex, pageSize, totalRecord).ToList();
                list.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return list;
            }
        }
        #endregion

        #region Exception
        public ExceptionModel ExceptionsGet(int companyId, string language, DateTime Date, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ExceptionModel model = new ExceptionModel();
                model.ExceptionList = context.MUNREPException(companyId, Date, pageIndex, pageSize, language, totalRecord).ToList() ?? new List<MUNREPException_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        #endregion

        #region Job Scheduler
        public JobSchedulerHistoryModel JobSchedulerHistoryGet(string language, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            using (ReportDataModelContainer context = new ReportDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                JobSchedulerHistoryModel model = new JobSchedulerHistoryModel();
                model.JobSchedulerHistoryList = context.MUNREPJobSchedulerHistory(language, startDate, endDate, pageIndex, pageSize, totalRecord).ToList() ?? new List<MUNREPJobSchedulerHistory_Result>();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return model;
            }
        }
        #endregion
    }
}
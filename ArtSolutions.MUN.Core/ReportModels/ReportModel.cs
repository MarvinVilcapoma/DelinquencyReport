using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;

namespace ArtSolutions.MUN.Core.ReportModels
{
    public class ReportModel
    {
    }

    #region Account
    public class AccountStatementModel
    {
        public bool? IsInJudicialCollection { get; set; }
        public string AccountPaymentPlanNames { get; set; }
        public List<AccountStatement> AccountStatementList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountStatement
    {
        public int ServiceID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int AccountServiceCollectionDetailID { get; set; }
        public string ConsecutiveNumber { get; set; }
        public string MeterNumber { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public string RightNumber { get; set; }
        public decimal Principal { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Discount { get; set; }
        public decimal PendingAmount { get; set; }
        public Nullable<bool> HasActivePaymentPlan { get; set; }
        public int AccountID { get; set; }
        public string PeriodName { get; set; }
        public Nullable<int> IsMonthly { get; set; }
        public Nullable<decimal> PreviousMeasure { get; set; }
        public Nullable<decimal> ActualMeasure { get; set; }
        public Nullable<decimal> WaterConsumption { get; set; }
        public string AccountPaymentPlanNames { get; set; }
        public bool? IsInJudicialCollection { get; set; }
    }
    public class HistoricalAccountServiceRemovedModel
    {
        public List<MUNSERREPAccountServiceRemove_Result> HistoricalAccountServiceRemovedList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountPropertyRemovedModel
    {
        public List<MUNREPAccountPropertyRemove_Result> AccountPropertyRemovedList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class SummaryAccountStatementModel
    {
        public bool? IsInJudicialCollection { get; set; }
        public List<SummaryAccountStatement> SummaryAccountStatementList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AdministrativeCollectionNoticeModel
    {
        public List<SummaryAccountStatement> SummaryAccountStatementList { get; set; }
        public List<AccountProperty> AccountPropertyList { get; set; }
    }
    public class SummaryAccountStatement
    {
        public string Service { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> PendingAmount { get; set; }
        public string Period { get; set; }
        public Nullable<int> IsMonthly { get; set; }
        public Nullable<System.DateTime> MINDueDate { get; set; }
        public Nullable<System.DateTime> MAXDueDate { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public bool? IsInJudicialCollection { get; set; }
    }
    public class AccountProperty
    {
        public string FincaID { get; set; }
        public decimal TotalValue { get; set; }
    }
    public class PaymentPlanByTaxpayerModel
    {
        public bool? IsInJudicialCollection { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public List<AccountPaymentPlanServiceDetailList> AccountPaymentPlanServiceDetailList { get; set; }
        public List<PaymentPlanByTaxpayerList> PaymentPlanByTaxpayerList { get; set; }
        public int MinMOROSAS { get; set; }
        public int MinAPager { get; set; }
    }
    public class AccountPaymentPlanServiceDetailList
    {
        public int RowNo { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string Periods { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public string PaymentPlanNameWithNumber { get; set; }
    }
    public class PaymentPlanByTaxpayerList
    {
        public int ID { get; set; }
        public string PaymentPlanNameWithNumber { get; set; }
        public decimal MONTOTOTAL { get; set; }
        public decimal? CUOTAMENSUAL { get; set; }
        public int APagar { get; set; }
        public int CANCELADAS { get; set; }
        public int QuotasToCalculate { get; set; }
        public int MOROSAS { get; set; }
        public DateTime? FECHAINICIAL { get; set; }
        public DateTime? FECHAFINAL { get; set; }
        public DateTime? ULTIMOPAGO { get; set; }
        public decimal SubTotal { get; set; }
        public decimal InterestTotal { get; set; }
        public decimal Total { get; set; }
        public bool? IsInJudicialCollection { get; set; }
    }
    public class AccountServicesMissingFilingModel
    {
        public List<MUNSERREPAccountServicesMissingFiling_Result> AccountServicesMissingFilingList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class GeneralMovementsModel
    {
        public List<MUNSERREPGeneralMovements_Result> GeneralMovementsList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountByServiceTypeModel
    {
        public List<MUNREPAccountByServicesType_Result> AccountByServiceTypeList { get; set; }
    }
    #endregion

    #region "Certification"
    public class BusinessLicenseStatementModel
    {
        public List<MUNSERREPBusinessLicenseStatement_Result> BusinessLicenseCertificateModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class DebtCertificationModel
    {
        public List<DebtCertificationDetailModel> DebtCertificationList { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public List<AccountPropertyModel> AccountPropertyList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class DebtCertificationDetailModel
    {
        public int ID { get; set; }
        public string Service { get; set; }
        public string CustomField { get; set; }
        public string Period { get; set; }
        public DateTime DueDate { get; set; }
        public int IsMonthly { get; set; }
        public decimal PenaltyPercentage { get; set; }
        public decimal Penalties { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal Interest { get; set; }
        public decimal Principal { get; set; }
        public decimal Total { get; set; }
    }
    public class NoDebtCertificationModel
    {
        public int AccountId { get; set; }
        public string Note { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public List<NoDebtCertificationDetailModel> NoDebtCertificationList { get; set; }
        public NoDebtCertificationPeriodModel NoDebtCertificationPeriodModel { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class NoDebtCertificationDetailModel
    {
        public string Right { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal Area { get; set; }
    }
    public class NoDebtCertificationPeriodModel
    {
        public int? IsPayed { get; set; }
        public int? IsPropertyTaxPayed { get; set; }
        public string PropertyTaxPeriod { get; set; }
        public int? PropertyTaxYear { get; set; }
        public string OtherPeriod { get; set; }
        public int? OtherYear { get; set; }
    }
    public class ProjectedAccountStatementModel
    {
        public List<MUNREPProjectedAccountStatement_Result> ProjectedAccountStatementList { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region "IVU"
    public class IVUBalanceSheetModel
    {
        public List<MUNSERREPIVUBalanceSheet_Result> IVUBalanceSheetList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class IVUFormsNotFiledModel
    {
        public List<MUNSERREPIVUFormsNotFiled_Result> IVUFormsNotFiledList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class IVUStatementModel
    {
        public List<MUNSERREPIVUStatement_Result> IVUStatementList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class IVUFillingCertificateModel
    {
        public List<IVUFillingCertificateDetailModel> IVUFilledCertificateList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class IVUFillingCertificateDetailModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }
        public bool IsFilled { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
    }
    public class IVUCollectionDetailModel
    {
        public List<MUNSERREPIVUCollectionDetail_Result> IVUCollectionDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Water
    public class HistoricalReadingsMeterModel
    {
        public List<MUNREPHistoricalReadingsByMeter_Result> HistoricalReadingsMeterList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ConsumptionRangeModel
    {
        public List<MUNREPConsumptionByRange_Result> ConsumptionRangeList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class InconsistenceReadingModel
    {
        public List<MUNREPInconsistenceReading_Result> InconsistenceReadingList { get; set; }
        public Int32 TotalRecord { get; set; }
        public Int32 TotalConsumption { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class AccountsConsumptionAndCollectionModel
    {
        public List<AccountsConsumptionAndCollectionColumnModel> ColumnList { get; set; }
        public DataTable DomesticWaterServiceList { get; set; }
        public DataTable OrdinaryWaterServiceList { get; set; }
        public DataTable ReproductiveWaterServiceList { get; set; }
        public DataTable PreferedWaterServiceList { get; set; }
        public DataTable GovernmentWaterServiceList { get; set; }
        public DataTable SummaryList { get; set; }
    }
    public class AccountsConsumptionAndCollectionColumnModel
    {
        public string ColumnName { get; set; }
    }
    public class DuplicatedMeterNumberModel
    {
        public List<MUNREPDuplicatedMeterNumber_Result> DuplicatedMeterNumberList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class RoutaMissingServicesModel
    {
        public List<MUNREPRoutaMissingServices_Result> RoutaMissingServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class SuspensionOrderForWaterServiceModel
    {
        public List<SuspensionOrderForWaterServiceList> SuspensionOrderForWaterServiceList { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class SuspensionOrderForWaterServiceList
    {
        public string ServiceName { get; set; }
        public decimal? TotalPrincipal { get; set; }
        public decimal? TotalInterest { get; set; }
        public decimal? Total { get; set; }
        public string Meter { get; set; }
        public string FincaID { get; set; }
        public int ServiceTypeID { get; set; }
    }
    #endregion

    #region Patent Report
    public class BusinessLicenseBalanceModel
    {
        public List<MUNSERREPBusinessLicenceBalance_Result> BusinessLicenseBalanceList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenseFilingByFilingTypeModel
    {
        public List<MUNSERREPBusinessLicenceFilingByFiling_Result> BusinessLicenseFilingByFilingTypeList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenseCreditBalanceModel
    {
        public List<MUNSERREPBusinessLicenceCreditBalance_Result> BusinessLicenseCreditBalanceList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenseBalanceByClosingModel
    {
        public List<MUNSERREPBusinessLicenceBalanceByClosing_Result> BusinessLicenseBalanceByClosingList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenseTransactionModel
    {
        public List<MUNSERREPBusinessLicenceTransaction_Result> BusinessLicenseTransactionList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenceSanitaryPermitDueDateModel
    {
        public List<MUNREPBusinessLicenceSanitaryPermitDueDate_Result> BusinessLicenceSanitaryPermitDueDateList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    public class BusinessLicenseGeneralModel
    {
        public List<MUNREPBusinessLicenceGeneral_Result> BusinessLicenseGeneralList { get; set; }
        public Int32 TotalRecord { get; set; }

    }
    #endregion

    #region Payment Receipts
    public class IVUReceiptDetailModel
    {
        public List<MUNSERREPReceiptIVU_Result> IVUReceiptDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class BusinessLicenseReceiptDetailModel
    {
        public List<MUNSERREPReceiptBusinessLicense_Result> BusinessLicenseReceiptDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptPermissionFeesModel
    {
        public List<MUNSERREPReceiptPermissionFees_Result> ReceiptPermissionFeesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptPermissionExpensesModel
    {
        public List<MUNSERREPReceiptPermissionExpenses_Result> ReceiptPermissionExpensesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptPermissionStreetVendorsModel
    {
        public List<MUNSERREPReceiptStreetVendors_Result> ReceiptPermissionStreetVendorsList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class PaymentPlanReceiptDetailModel
    {
        public List<MUNSERREPReceiptPaymentPlan_Result> PaymentPlanReceiptDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class OtherConceptReceiptDetailModel
    {
        public List<MUNSERREPReceiptOtherConcept_Result> OtherConceptReceiptDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptRegisterModel
    {
        public List<MUNSERREPReceiptRegister_Result> ReceiptRegisterList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptRegisterForCRModel
    {
        public List<MUNSERREPReceiptRegisterForCR_Result> ReceiptRegisterList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptDetailModel
    {
        public List<MUNSERREPReceiptDetail_Result> ReceiptDetailList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptBankModel
    {
        public List<MUNSERREPReceiptBank_Result> ReceiptByBankList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class SecurityBankAccountModel
    {
        public List<MUNSERREPSecuritiesByBank_Result> SecurityBankAccountList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ReceiptCommercialFacilitiesRentalModel
    {
        public List<MUNSERREPReceiptCommercialFacilitiesRental_Result> ReceiptCommercialFacilitiesRentalList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class VoidReceiptsModel
    {
        public List<MUNSERREPReceiptVoid_Result> ReceiptEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class MigrationValidationFactuModel
    {
        public List<MUNREPMigrationValidationFactu_Result> MigrationValidationFactuList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class CollectionDailyIncomeStateModel
    {
        public List<MUNSERREPDailyIncomeState_Result> DailyIncomeStateList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class CollectionRevenueDailyCollectionModel
    {
        public List<MUNSERREPDailyCollection_Result> RevenueDailyCollectionList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class HistoricalPaymentReportModel
    {
        public List<MUNSERREPOneTimePayment_Result> HistoricalPaymentList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AmnestyMovementReportModel
    {
        public List<MUNSERREPReceiptAmnestyMovement_Result> AmnestyMovementReportList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class MigrationValidationCobrosModel
    {
        public List<MUNREPMigrationValidationCobros_Result> MigrationValidationCobrosList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class MigrationValidationBienesModel
    {
        public List<MUNREPMigrationValidationBienes_Result> MigrationValidationBienesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class PaymentsMadeByTheTaxpayerModel
    {
        public List<MUNSERREPListOfPaymentsMadeByTheTaxpayer_Result> PaymentsMadeByTheTaxpayerList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Daily Cash Register
    public class CollectionClosingSummaryModel
    {
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
    }
    public class ClosingSummaryServiceTypeModel
    {
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
    }
    public class ClosingSummaryPaymentTypeModel
    {
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
    }
    public class ServiceTypePaymentModel
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CashierName { get; set; }
        public string ClosingTypeName { get; set; }
        public string Description { get; set; }
        public int ReceiptCount { get; set; }
        public decimal NetClosing { get; set; }
    }
    public class CashReceiptControlModel
    {
        public List<MUNREPCashReceiptControl_Result> CashReceiptControlList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class MonthlyIncomeStatementByServicesModel
    {
        public List<MUNREPMonthlyIncomeStatementByServices_Result> MonthlyIncomeStatementByServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class QuarterlyIncomeStatementByServicesModel
    {
        public List<MUNREPQuarterlyIncomeStatementByServices_Result> QuarterlyIncomeStatementByServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class YearlyIncomeStatementByServicesModel
    {
        public List<MUNREPYearlyIncomeStatementByServices_Result> YearlyIncomeStatementByServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Deposit Entry
    public class CollectionDepositSummaryModel
    {
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalDepositRecord { get; set; }
    }
    public class CollectionDepositSummaryPaymentTypeModel
    {
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalDepositRecord { get; set; }
    }
    public class CollectionDepositSummaryServiceTypeModel
    {
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalDepositRecord { get; set; }
    }
    public class CollectionDepositSummaryByInvoiceModel
    {
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalDepositRecord { get; set; }
    }
    public class CollectionDepositSummaryByPaymentPlanModel
    {
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
    }
    #endregion

    #region Property
    public class PropertyModel
    {
        public List<MUNREPPropertyMovementByAccount_Result> PropertyMovementList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class TransfersReportModel
    {
        public List<MUNREPTransfersReport_Result> TransfersReportList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountExclusionFormModel
    {
        public int AccountId { get; set; }
        public string Observations { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public List<PropertyTaxList> PropertyTaxList { get; set; }
        public List<WaterMeasureList> WaterMeasureList { get; set; }
        public List<OtherServicesList> OtherServicesList { get; set; }
        public List<HistoricalList> HistoricalList { get; set; }
    }
    public class PropertyTaxList
    {
        public int Year { get; set; }
        public string RightNumber { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal Principal { get; set; }
        public string Service { get; set; }
        public decimal Penalties { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmountByService { get; set; }
    }
    public class WaterMeasureList
    {
        public string Service { get; set; }
        public string CustomField1 { get; set; }
        public decimal PreviousMeasure { get; set; }
        public decimal ActualMeasure { get; set; }
        public string CustomField2 { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Balance { get; set; }
    }
    public class OtherServicesList
    {
        public string Service { get; set; }
        public string RightNumber { get; set; }
        public decimal Principal { get; set; }
        public int PendingPeriod { get; set; }
        public string Segrega { get; set; }
        public int FiscalYearID { get; set; }
        public decimal Balance { get; set; }
    }
    public class HistoricalList
    {
        public int FiscalYearID { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
    public class ParkMaintenanceMissingServicesModel
    {
        public List<MUNREPParkMaintenanceMissingServices_Result> ParkMaintenanceMissingServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ServicesLinkedToRightModel
    {
        public List<MUNREPServicesLinkedToRight_Result> ServicesLinkedToRightList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class RightsByOwnersIDModel
    {
        public int OwnerID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public ReportAccountModel AccountModel { get; set; }
        public List<RightsByOwnersIDList> RightsByOwnersIDList { get; set; }
        public RightsByOwnersIDDetailModel RightsByOwnersIDDetailModel { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class RightsByOwnersIDList
    {
        public string PropertyCode { get; set; }
        public string PropertyNumber { get; set; }
        public string RightNumber { get; set; }
        public decimal? TotalArea { get; set; }
        public decimal? CurrentValue { get; set; }
        public string CurrentCode { get; set; }
        public DateTime? CurrentFecha { get; set; }
        public string CurrentNumber { get; set; }
        public decimal? PreviousValue { get; set; }
        public string PreviousCode { get; set; }
        public DateTime? PreviousFecha { get; set; }
        public string PreviousNumber { get; set; }
        public string ExonerationCode { get; set; }
        public string ExonerationType { get; set; }
        public decimal? ExonerationPercent { get; set; }
    }
    public class RightsByOwnersIDDetailModel
    {
        public int TotalFincas { get; set; }
        public int TotalOwner { get; set; }
    }
    #endregion

    #region Bank

    #region Export Bank Payments
    public class ExportBankPaymentsModel
    {
        public ExportBankPaymentsModel()
        {
            ColumnSchema = new Dictionary<string, int>();
        }
        public DataTable ExportBankPaymentsList { get; set; }
        public Int32 TotalRecord { get; set; }
        public Dictionary<string, int> ColumnSchema { get; set; }
    }
    #endregion
    public class CollectionRevenueBankCollectionModel
    {
        public List<MUNSERREPBankCollection_Result> RevenueBankCollectionList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class CollectionDailyIncomeProduceBankModel
    {
        public List<MUNSERREPDailyIncomeProduceBank_Result> DailyIncomeProduceBankList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class ReceiptCollectedByBankModel
    {
        public List<MUNSERREPListOfReceiptCollectedBank_Result> ReceiptCollectedByBankList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class StatisticsofReceiptsCollectedModel
    {
        public List<MUNSERREPStatisticsofReceiptsCollected_Result> StatisticsofReceiptsCollectedList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ControlOfPaymentsMadeInTheBankModel
    {
        public List<MUNSERREPControlOfPaymentsMadeInTheBank_Result> ControlOfPaymentsMadeInTheBankList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class InconsistenciesInTheBankReceiptsModel
    {
        public List<MUNSERREPInconsistenciesInTheBankReceipts_Result> InconsistenciesInTheBankReceiptsList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Company Detail For Report
    public class ReportCompanyModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string WorkPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string StateName { get; set; }
        public int LogoID { get; set; }
    }
    #endregion

    #region Account Detail For Report
    public class ReportAccountModel
    {
        public int ID { get; set; }
        public string RegisterNumber { get; set; }
        public string DisplayName { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    #endregion

    #region Exception 
    public class ExceptionModel
    {
        public List<MUNREPException_Result> ExceptionList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Job Scheduler
    public class JobSchedulerHistoryModel
    {
        public List<MUNREPJobSchedulerHistory_Result> JobSchedulerHistoryList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

}

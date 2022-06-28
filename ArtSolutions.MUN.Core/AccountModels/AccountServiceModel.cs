using ArtSolutions.MUN.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ServiceID { get; set; }
        public int Year { get; set; }
        public int AccountID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public System.DateTime IssueDate { get; set; }
        public bool IsIssued { get; set; }
        public System.Guid IssuedBy { get; set; }
        public bool IsPrint { get; set; }
        public System.DateTime PrintDate { get; set; }
        public bool IsLock { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string LicenseNumber { get; set; }
        public int LockReasonTableValue { get; set; }
        public int LockActionTableValue { get; set; }
        public string LockComment { get; set; }
        public string SSEIN { get; set; }
        public int IssueNumber { get; set; }
        public int? ServiceExceptionID { get; set; }
        public decimal? ServiceExceptionValue { get; set; }
        public System.DateTime? FilingDueDate { get; set; }
        public System.DateTime? PaymentDueDate { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }

        public Nullable<System.DateTime> CustomDateField { get; set; }
        public string CustomField1NewValue { get; set; }
        public string CustomField2NewValue { get; set; }
        public string CustomField3NewValue { get; set; }
        public string CustomField4NewValue { get; set; }
        public string CustomField5NewValue { get; set; }
        public DateTime? CustomField1UpdateDate { get; set; }

        public DateTime? CustomDateFieldNewValue { get; set; }
        public int? AccountPropertyID { get; set; }
        public int? ServiceStartPeriod { get; set; }
        public int? LicenseAccountServiceID { get; set; }
        public int AccountTypeID { get; set; }
        public string LicenseType { get; set; }
        public string ServiceName { get; set; }
        public bool IsActive { get; set; }
        public bool IsVoid { get; set; }
        public string Notes { get; set; }
        public Nullable<int> AccountPaymentPlanID { get; set; }
        public int ServiceTypeID { get; set; }
        public string Property { get; set; }
        public Nullable<decimal> AnnualIncome { get; set; }
        public int FromAccountID { get; set; }
        public string ReasonForDeleted { get; set; }
        public string CreatedBy { get; set; }
        public bool IsAccountServiceNote { get; set; }
        public decimal? PreviousMeasure { get; set; }
        public string BusinessDescription { get; set; }
    }
    public class AccountServiceTransferModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ServiceID { get; set; }
        public int Year { get; set; }
        public int AccountID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public System.DateTime IssueDate { get; set; }
        public bool IsIssued { get; set; }
        public System.Guid IssuedBy { get; set; }
        public bool IsPrint { get; set; }
        public System.DateTime PrintDate { get; set; }
        public bool IsLock { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AccountName { get; set; }
        public string LicenseNumber { get; set; }
        public int LockReasonTableValue { get; set; }
        public string LockComment { get; set; }
        public string SSEIN { get; set; }
        public int IssueNumber { get; set; }
        public int? ServiceExceptionID { get; set; }
        public decimal? ServiceExceptionValue { get; set; }
        public System.DateTime? FilingDueDate { get; set; }
        public System.DateTime? PaymentDueDate { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public Nullable<System.DateTime> CustomDateField { get; set; }
        public string CustomField1NewValue { get; set; }
        public string CustomField2NewValue { get; set; }
        public string CustomField3NewValue { get; set; }
        public DateTime? CustomDateFieldNewValue { get; set; }
        public int? AccountPropertyID { get; set; }
        public int? ServiceStartPeriod { get; set; }
        public int AccountTypeID { get; set; }
        public string LicenseType { get; set; }
        public string ServiceName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> AccountPaymentPlanID { get; set; }
        public int ServiceTypeID { get; set; }
        public int GroupID { get; set; }
        public string Property { get; set; }
        public Nullable<decimal> AnnualIncome { get; set; }
        public int FromAccountID { get; set; }
        public string ReasonForDeleted { get; set; }


        public string ServiceNameWithYear { get; set; }
        public int AccountServiceCollectionDetailId { get; set; }
        public decimal SalesVolume { get; set; }
        public decimal Principal { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public decimal Discounts { get; set; }
        public decimal Debts { get; set; }
        public decimal PaidAmount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public int FiscalYearID { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public Nullable<int> FilingFormID { get; set; }
        public int SequenceID { get; set; }
    }
    public class AccountServicePrintModel
    {
        public CompanyModel Company { get; set; }
        public AccountServiceModel AccountService { get; set; }
        public AccountAddressForPrint AccountAddress { get; set; }
        public PrintTemplateModel PrintTemplate { get; set; }
        public AccountBusinessModel AccountBusiness { get; set; }
        public AccountServiceCollectionPrintModel AccountServiceCollection { get; set; }
        public List<AccountServiceCollectionPrintModel> AccountServiceDebtList { get; set; }

    }
    public class AccountServiceStatusModel
    {
        public int CompanyID { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public bool IsIssued { get; set; }
        public Nullable<System.Guid> IssuedBy { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public bool IsLock { get; set; }
        public bool IsVoid { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
    public class AccountServiceCollectionPrintModel
    {
        public string ServiceName { get; set; }
        public decimal TotalPrincipal { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal Total { get; set; }
    }

    #region AccountServiceCollection 
    public class AccountServiceCollectionFillingModel
    {
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        public decimal GrossVolume { get; set; }
        public decimal ExemptionAmount { get; set; }
        public decimal? PercentageValue { get; set; }
        public decimal Total { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal PurchasesubjectToTax { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public string CommaSeperatedImageIDs { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public System.Guid FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool ApplyToAllYear { get; set; }

        //Extra column for tax
        public decimal FormIVUTreasury { get; set; }
        public decimal PurchasesImportsResale { get; set; }
        public decimal ImportsUse { get; set; }
        public decimal UseofInventory { get; set; }
        public decimal TotalUseSubjectIVU { get; set; }
        public decimal TaxableFurnitureSale { get; set; }
        public decimal TaxableServicesSale { get; set; }
        public decimal TaxableAdmissions { get; set; }
        public decimal TaxableItemsReturns { get; set; }
        public decimal TotalTaxableSales { get; set; }
        public decimal ExemptFurnitureSale { get; set; }
        public decimal ExemptServicesSale { get; set; }
        public decimal ExemptAdmissions { get; set; }
        public decimal ExemptReturns { get; set; }
        public decimal TotalExemptSales { get; set; }
        public decimal CreditSaleProperty { get; set; }
        public decimal CreditUncollectibleAccounts { get; set; }
        public decimal TaxCreditPaid { get; set; }

        public int? ID { get; set; }

        //Extra For Unit
        //public decimal Length { get; set; }
        //public decimal Width { get; set; }
        public decimal Value { get; set; }
        public decimal UnitCost { get; set; }

        //Extra For Measured Water
        public decimal PreviousMeasure { get; set; }
        public decimal ActualMeasure { get; set; }
        public decimal WaterConsumption { get; set; }

        //Extra For Property Tax
        public string PropertyTaxJson { get; set; }
        public Nullable<int> MovementTypeID { get; set; }
        public Nullable<decimal> Area { get; set; }
        public Nullable<decimal> TerrainValue { get; set; }
        public int PropertyAccountID { get; set; }
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public decimal TotalValue { get; set; }
        public string Notes { get; set; }
        public AccountServiceCollectionFillingModel CollectionFillingPropertyTaxModel { get; set; }
    }

    public class AccountServiceList
    {
        public List<MUNSERAccountServiceGetWithPaging_Result> AccountServiceModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region Import Account Service Filing

    public class AccountServiceFilingImportViewModel
    {
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string CustomField1 { get; set; }
        public string LastReading { get; set; }
        public string CurrentReading { get; set; }
        public string Difference { get; set; }
        public string TaxNumber { get; set; }
    }
    public class AccountServiceFilingImportModel
    {
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<AccountServiceFilingImportViewModel> ImportList { get; set; }
    }

    public class ImportAccountServiceFilingModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string FincaID { get; set; }
        public List<ImportAccountServiceFilingDetailModel> AccountServiceFilingDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ImportAccountServicePropertyForPreviousYearModel
    {
        public string FincaID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int Year { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public decimal Principal { get; set; }
        public decimal Exemption { get; set; }
        public int ExemptionPeriod { get; set; }
        public decimal Payment { get; set; }
        public int PaymentPeriod { get; set; }
        public DateTime FillingDate { get; set; }
        public string FillingeReference { get; set; }

    }

    public class ImportAccountServiceAutoCreationPreviousYearModel
    {
        public string FincaID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int Year { get; set; }
        public string Segrega { get; set; }
        public int PreviousYearPeriod { get; set; }
    }

    public class ImportAccountServiceUnitPreviousYearModel
    {
        public string FincaID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int Year { get; set; }
        public string Segrega { get; set; }
        public int PreviousYearPeriod { get; set; }
        public decimal AmountSubjectToTax { get; set; }
    }

    public class ImportAccountServiceLicenseFilingModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string FincaID { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomDateField { get; set; }
        public List<ImportAccountServiceLicenseFilingDetailModel> AccountServiceFilingDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ImportAccountServiceMeasureWaterFilingModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string FincaID { get; set; }
        public List<ImportAccountServiceMeasureWaterFilingDetailModel> AccountServiceFilingDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ImportAccountServiceUnitFilingModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string FincaID { get; set; }
        public string Segrega { get; set; }
        public List<ImportAccountServiceUnitFilingDetailModel> AccountServiceFilingDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CURRENT_YEAR_PERIODS_DEBT { get; set; }
    }

    public class ImportAccountServiceAutoFilingModel
    {
        public int CompanyID { get; set; }
        public string FincaID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public int Year { get; set; }
        public string Segrega { get; set; }
        //public List<ImportAccountServiceFilingDetailModel> AccountServiceFilingDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CURRENT_YEAR_PERIODS_DEBT { get; set; }
    }
    public class ImportAccountServiceFilingPaymentModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public List<ImportAccountServicePaymentDetailModel> AccountServicePaymentDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }


    public class ImportAccountServiceFilingPaymentForSelectedYearModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public string ServiceCode { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public Nullable<int> FilingTypeID { get; set; }
        public List<ImportAccountServicePaymentDetailForSelectedYearModel> AccountServicePaymentDetail { get; set; }
        public string Language { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ImportAccountServiceFilingDetailModel
    {
        public int Year { get; set; }
        public Nullable<DateTime> FilingDueDate { get; set; }
        public Nullable<DateTime> PaymentDueDate { get; set; }
        public string FormattedFilingDueDate
        {
            get
            {
                return FilingDueDate.HasValue ? FilingDueDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public string FormattedPaymentDueDate
        {
            get
            {
                return PaymentDueDate.HasValue ? PaymentDueDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public Nullable<DateTime> FillingDate { get; set; }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public string FillingeReference { get; set; }
        public Nullable<int> ExemptionPeriod { get; set; }
        public Nullable<decimal> Exemption { get; set; }
    }

    public class ImportAccountServiceLicenseFilingDetailModel
    {
        public int Year { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public Nullable<DateTime> FillingDate { get; set; }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
    }

    public class ImportAccountServiceMeasureWaterFilingDetailModel
    {
        public int Year { get; set; }
        public Nullable<DateTime> FillingDate { get; set; }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public string FillingeReference { get; set; }
        public Nullable<decimal> PreviousMeasure { get; set; }
        public Nullable<decimal> ActualMeasure { get; set; }
        public Nullable<decimal> WaterConsumption { get; set; }
        public Nullable<int> Month { get; set; }
        public bool IsPaid { get; set; }
    }

    public class ImportAccountServiceUnitFilingDetailModel
    {
        public int Year { get; set; }
        public Nullable<DateTime> FilingDueDate { get; set; }
        public Nullable<DateTime> PaymentDueDate { get; set; }
        public string FormattedFilingDueDate
        {
            get
            {
                return FilingDueDate.HasValue ? FilingDueDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public string FormattedPaymentDueDate
        {
            get
            {
                return PaymentDueDate.HasValue ? PaymentDueDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public decimal AmountSubjectToTax { get; set; }
        public Nullable<DateTime> FillingDate { get; set; }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d", CultureInfo.InvariantCulture) : null;
            }
        }
        public string FillingeReference { get; set; }
    }
    public class ImportAccountServicePaymentDetailModel
    {
        public DateTime PaymentDate { get; set; }
        public string FormattedPaymentDate
        {
            get
            {
                return PaymentDate.ToString("d", CultureInfo.InvariantCulture);
            }
        }
        public decimal Payment { get; set; }
        public decimal Penalty { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetPayment { get; set; }
        public string Notes { get; set; }
        public string PaymentNumber { get; set; }
        public int PaymentPeriod { get; set; }
        public string Segrega { get; set; }
        //public string OwnerID { get; set; }
    }

    public class ImportAccountServicePaymentDetailForSelectedYearModel
    {
        public DateTime PaymentDate { get; set; }
        public string FormattedPaymentDate
        {
            get
            {
                return PaymentDate.ToString("d", CultureInfo.InvariantCulture);
            }
        }
        public decimal Payment { get; set; }
        public decimal Penalty { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetPayment { get; set; }
        public string Notes { get; set; }
        public string PaymentNumber { get; set; }
        public int PaymentPeriod { get; set; }
        public string Segrega { get; set; }
        public int StartQuarter { get; set; }
        public int EndQuarter { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        //public string OwnerID { get; set; }
    }

    public class AccountServiceImportModel
    {
        public int CompanyID { get; set; }
        public string PropertyNumber { get; set; }
        public int ServiceID { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string ReferenceNumber { get; set; }
        public int Year { get; set; }
        public bool IsIssued { get; set; }
        public Nullable<Guid> IssuedBy { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<DateTime> PrintDate { get; set; }
        public bool IsLock { get; set; }
        public Nullable<int> LockReason { get; set; }
        public bool IsVoid { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> FilingDueDate { get; set; }
        public Nullable<DateTime> PaymentDueDate { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime FillingDate { get; set; }
        public string FillingeReference { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetPayment { get; set; }
        public string Notes { get; set; }
        public string PaymentNumber { get; set; }
        public string Language { get; set; }
        public string NumberPrefix { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ValidImportAccountServiceFilingModel
    {
        public string GroupCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CustomField1 { get; set; }
        public string TaxNumber { get; set; }
        public string Reason { get; set; }
    }
    public class ImportAccountServicePaymentPlanModel
    {
        public int CompanyID { get; set; }
        public string TaxNumber { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public string TermsAndConditions { get; set; }
        public int TotalEmiPeriod { get; set; }
        public int PaidEmiPeriod { get; set; }
        public int RemainingEmiPeriod { get; set; }
        public List<ImportInvoiceDetailModel> InvoiceDetail { get; set; }

    }
    public class ImportInvoiceDetailModel
    {
        public string Description { get; set; }
        public string ServiceCode { get; set; }
        public decimal PaidEmiAmount { get; set; }
        public decimal RemainingEmiAmount { get; set; }
        public decimal DownPaymentAmount { get; set; }
    }
    #endregion

    #region Import Measured Water Filing
    public class MeasuredWaterFilingImportViewModel
    {
        public int RowNo { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CATEGORIA { get; set; }
        public string TaxNumber { get; set; }
        public string UBICACION { get; set; }
        public string CODIGOM { get; set; }
        public string LECTURAACT { get; set; }
        public string LastReading { get; set; }
        public string Difference { get; set; }
        public string FECHA { get; set; }
        public string IsValid { get; set; }
    }
    public class MeasuredWaterFilingImportModel
    {
        public int CompanyID { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEdit { get; set; }
        public bool IsValidate { get; set; }
        public bool IsForDownload { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<MeasuredWaterFilingImportViewModel> ImportList { get; set; }
        public DataTable FileDataList { get; set; }
        public int? ErrorCode { get; set; }
        public bool IsRevalidate { get; set; }
        public bool? IsValid { get; set; }
        public bool ValidateCustomField { get; set; }
        public int PageSize { get; set; }
        public int? RowNo { get; set; }
        public int? CurrentReading { get; set; }
        public DateTime? ProcessDate { get; set; }
    }
    public class ValidImportMeasuredWaterFilingModel
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string CATEGORIA { get; set; }
        public string TaxNumber { get; set; }
        public string UBICACION { get; set; }
        public string CODIGOM { get; set; }
        public decimal? LECTURAACT { get; set; }
        public decimal? LastReading { get; set; }
        public decimal? Difference { get; set; }
        public string Note { get; set; }
        public bool IsValid { get; set; }
        public string FECHA { get; set; }
    }
    public class ValidateMeasuredWaterAccountServiceFilingModel
    {
        #region public properties
        public DataTable AccountServiceList { get; set; }
        //public DataTable InvalidDetailList { get; set; }
        public DataTable ValidDetailList { get; set; }
        public DataTable AccountServiceNotExistList { get; set; }
        #endregion
    }
    #endregion
}
using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class InvoiceModel
    {
        #region Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Language { get; set; }
        public int DocumentID { get; set; }
        public int DocumentTypeID { get; set; }
        public int SequenceID { get; set; }
        public System.DateTime Date { get; set; }
        public int FiscalYearID { get; set; }
        public string FiscalYearPeriodID { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public int AccountServiceID { get; set; }
        public Nullable<int> AccountContactID { get; set; }
        public Nullable<int> AccountAddressID { get; set; }
        public Nullable<int> PaymentOptionID { get; set; }
        public int PrintTemplateID { get; set; }
        public string Notes { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsPost { get; set; }
        public bool IsVoid { get; set; }
        public bool IsDeleted { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Payments { get; set; }
        public decimal Balance { get; set; }
        public decimal SubTotalCompanyCurrency { get; set; }
        public decimal DiscountTotalCompanyCurrency { get; set; }
        public decimal TotalCompanyCurrency { get; set; }
        public decimal PaymentsCompanyCurrency { get; set; }
        public decimal BalanceCompanyCurrency { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsManual { get; set; }
        public List<InvoiceDetailModel> InvoiceDetail { get; set; }
        public string VoidReason { get; set; }
        public DateTime VoidDate { get; set; }

        #region Extra Properties
        public string ServiceName { get; set; }

        public string InvoiceDetailJson { get; set; }
        #endregion
        #endregion
    }
    public class InvoiceUTDModel
    {
        public int ID { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public int PaymentLine { get; set; }
        public decimal Payments { get; set; }
        public string DiscountJSON { get; set; }
    }
    public class InvoiceDetailModel
    {
        public int ID { get; set; }
        public int ItemTypeTableID { get; set; }
        public int ItemTypeID { get; set; }
        public int? AccountServiceCollectionDetailID { get; set; }
        public int? ServiceID { get; set; }
        public int? CollectionTemplateID { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public decimal Total { get; set; }
        public byte[] RowVersion { get; set; }

        //Exctra Column For Receivable, Revenu & Cash Account
        public int ReceivableAccountID { get; set; }
        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public int RevenueAccountID { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public int CheckbookID { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public int CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public int CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }
    }
    public class InvoiceListModel
    {
        public List<MUNCOLInvoiceGetWithPaging_Result> InvoiceList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class InvoicePrintModel
    {
        public InvoiceCompanyModel Company { get; set; }
        public InvoiceAccountModel Account { get; set; }
        public InvoiceModel Invoice { get; set; }
        public List<InvoiceDetailModel> InvoiceDetailList { get; set; }
        public List<AccountServiceCollectionDetailModel> AccountServiceCollectionDetailList { get; set; }
        public PrintTemplateModel PrintTemplate { get; set; }
        public int ServiceTypeID { get; set; }
    }
    public class InvoiceCompanyModel
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
    public class InvoiceAccountModel
    {
        public int ID { get; set; }
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StateName { get; set; }
        public string CountyCode { get; set; }
    }
}

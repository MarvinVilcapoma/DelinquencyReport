using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace ArtSolutions.MUN.Core.Collections
{
    #region Payment
    public class PaymentModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int ServiceTypeID { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public int CashierID { get; set; }
        public string CashierName { get; set; }
        public int InvoiceID { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal AmnestyAmount { get; set; }
        public decimal NetPayment { get; set; }
        public string Notes { get; set; }
        public int? AttachmentID { get; set; }
        public Guid UserID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int PrintTemplateID { get; set; }
        public List<InvoiceUTDModel> PostedInvoiceList { get; set; }
        public List<PaymentDetailUTDModel> PaymentDetailList { get; set; }
        public List<PaymentDetailPropertyTaxUTDModel> PaymentDetailPropertyTaxList { get; set; }
        public string FileName { get; set; }
        public bool IsVoid { get; set; }
        public bool IsClosed { get; set; }
        public bool IsPost { get; set; }
        public string AccountDisplayName { get; set; }
        public int ClosingID { get; set; }
        public int ClosingTypeID { get; set; }
        public string Language { get; set; }
        public string PaymentOptionJSON { get; set; }
        public string ServiceTypeName { get; set; }
        public string NumberPrefix { get; set; }
        public bool IsManual { get; set; }
        public bool ApplybyAmnesty { get; set; }
        public string VoidReason { get; set; }
        public string PaymentPlanName { get; set; }
        public bool IsOfficialReceipt { get; set; }
        public decimal ApplyCreditAmount { get; set; }
        public Nullable<int> CreditNoteID { get; set; }
        public Nullable<int> DebitNoteID { get; set; }
        public int DocumentTypeID { get; set; }
        public Nullable<int> FINJournalID { get; set; }
        public string CollectionType { get; set; }
        public string PaymentType { get; set; }
        public List<InvoiceDetailModel> InvoiceDetail { get; set; }
        public string InvoiceDetailJson { get; set; }
        public string PaymentOption { get; set; }
        public bool IVAPayment { get; set; }
        public bool IsIVAApply { get; set; }
        public bool IsRemoveInterest { get; set; }
        public string DebitNotesJson { get; set; }
    }
    public class PaymentListModel
    {
        public List<MUNCOLPaymentGetWithPaging_Result> PaymentList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class PaymentPrintModel
    {
        public PaymentCompanyModel Company { get; set; }
        public PaymentAccountModel Account { get; set; }
        public PaymentModel Payment { get; set; }
        public List<AccountServiceCollectionDetailModel> AccountServiceCollectionDetail { get; set; }
        public List<InvoiceDetailModel> InvoiceDetail { get; set; }
        public List<PaymentOptionModel> PaymentOptionList { get; set; }
        public List<AccountPaymentPlanDetailModel> PaymentPlanDetailList { get; set; }
        public PrintTemplateModel PrintTemplate { get; set; }
        public List<DebitNoteModel> DebitNote { get; set; }
        public List<CreditNoteModel> CreditNote { get; set; }
    }
    public class PaymentDetailUTDModel
    {
        public int? AccountServiceID { get; set; }
        public int? AccountServiceCollectionDetailID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal CollectionDetailDebt { get; set; }
        public int? AccountPaymentPlanID { get; set; }
        public int? AccountPaymentPlanDetailID { get; set; }
        public int? AccountPaymentPlanServiceDetailID { get; set; }
    }
    public class PaymentDetailPropertyTaxUTDModel
    {
        public int? AccountServiceID { get; set; }
        public int? AccountServiceCollectionDetailID { get; set; }
        public int? CollectionFillingPropertyTaxID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal CollectionDetailDebt { get; set; }
        public int? AccountPaymentPlanID { get; set; }
        public int? AccountPaymentPlanDetailID { get; set; }

    }
    public class PaymentCompanyModel
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
        public string Email { get; set; }
    }
    public class PaymentAccountModel
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string RegisterNumber { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StateName { get; set; }
        public string CountyCode { get; set; }
        public string TaxNumber { get; set; }
    }
    #endregion

    #region Import Bank Payments
    public class BankPaymentsImportViewModel
    {
        public int ImportID { get; set; }
        public string Contract { get; set; }
        public string TaxNumber { get; set; }
        public string PERIODOREC { get; set; }
        public string PaymentDate { get; set; }
        public string Amount { get; set; }
        public string ReceiptNumber { get; set; }
    }
    public class BankPaymentsImportModel
    {
        public int CompanyID { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }      
        public int? PaymentOptionID { get; set; }
        public string FileName { get; set; }
        public string Contract { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsValid { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DataTable ImportList { get; set; }
    }
    public class ValidImportBankPaymentsModel
    {
        public string PERIODOREC { get; set; }
        public string Contract { get; set; }
        public string TaxNumber { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
        public int? ServiceID { get; set; }
        public string Segrega  { get; set; }

        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int? PaymentID { get; set; }
        public decimal? Payment { get; set; }
    }
    #endregion
}
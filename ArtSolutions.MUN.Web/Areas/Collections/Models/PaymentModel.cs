using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PaymentModel
    {
        public PaymentModel()
        {
            AccountList = new List<SelectListItem>();
            AccountPropertyList = new List<SelectListItem>();
            PostedInvoiceList = new List<InvoiceModel>();
            ServiceTypeList = new List<ServiceTypeModel>();
            AccountServiceCollectionDetailList = new List<AccountServiceCollectionDetailModel>();
            AccountPaymentPlanList = new List<AccountPaymentPlanModel>();
            AccountPaymentPlanDetailList = new List<AccountPaymentPlanDetailModel>();
            AccountDebitNoteList = new List<DebitNoteModel>();
        }
        public int ID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int AccountID { get; set; }
        public int AccountServiceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? ServiceTypeID { get; set; }
        public int InvoiceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime Date { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Number { get; set; }
        public bool IsVoid { get; set; }
        [StringLength(500, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string VoidReason { get; set; }
        public DateTime VoidDate { get; set; }
        public decimal TotalPayment { get; set; }
        [DecimalNumber(ErrorMessageResourceName = "ValDecimalValue", ErrorMessageResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal NetPayment { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Notes { get; set; }
        public int? AttachmentID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public List<SelectListItem> AccountPropertyList { get; set; }
        public IEnumerable<AccountPropertyRightModel> AccountPropertyRightList { get; set; }
        public List<ServiceTypeModel> ServiceTypeList { get; set; }
        public int PrintTemplateID { get; set; }
        public bool IsClosed { get; set; }
        public bool IsPost { get; set; }
        public int SequenceID { get; set; }
        public List<InvoiceModel> PostedInvoiceList { get; set; }
        public List<AccountServiceCollectionDetailModel> AccountServiceCollectionDetailList { get; set; }
        public List<BusinessLicenseAccountServiceModel> BusinessLicenseAccountServiceList { get; set; }
        public List<PaymentOptionModel> PaymentOptionList { get; set; }
        public List<PaymentDetailModel> PaymentDetailList { get; set; }
        public List<PaymentDetailModel> PaymentDetailPropertyTaxList { get; set; }
        public List<AccountPaymentPlanModel> AccountPaymentPlanList { get; set; }
        public List<AccountPaymentPlanDetailModel> AccountPaymentPlanDetailList { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int AccountPaymentPlanID { get; set; }
        public string PaymentOption { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalChequedelBancoNacional { get; set; }
        public decimal TotalCreditCard { get; set; }
        public decimal TotalBankTransfer { get; set; }
        public decimal TotalBankTransferByBancoNacionaldeCostaRica { get; set; }
        public decimal TotalChequedelBancodeCostaRica { get; set; }
        public decimal TotalAdjustment { get; set; }
        public decimal TotalBank { get; set; }
        public bool IsRemoveInterest { get; set; }
        #region Extra Columns
        public int ClosingID { get; set; }
        public string Cashier { get; set; }
        public string FormattedNetPayment
        {
            get
            {
                return NetPayment.ToString("C");
            }
        }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        public string AccountDisplayName { get; set; }
        public string ItemName { get; set; }
        public string ServiceTypeName { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public bool SelectedReceipt { get; set; }
        public string PaymentOptionJSON { get; set; }
        public int ClosingTypeID { get; set; }
        public int DocumentTypeID { get; set; }
        public string RowVersion64
        {
            get
            {
                if (this.RowVersion != null)
                {
                    return Convert.ToBase64String(this.RowVersion);
                }

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.RowVersion = null;
                }
                else
                {
                    this.RowVersion = Convert.FromBase64String(value);
                }
            }
        }
        public bool AllowManualReceipt { get; set; }
        public bool IsManual { get; set; }
        public bool IVAPayment { get; set; }
        public bool ApplybyAmnesty { get; set; }
        public decimal AmnestyAmount { get; set; }
        public string PaymentPlanName { get; set; }
        public decimal ApplyCreditAmount { get; set; }
        public Nullable<int> CreditNoteID { get; set; }
        public Nullable<int> DebitNoteID { get; set; }
        public Nullable<int> FINJournalID { get; set; }
        public string CollectionType { get; set; }
        public string PaymentType { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? AccountPropertyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? AccountPropertyRightID { get; set; }
        public string AccountPaymentPlanDetailIDs { get; set; }
        public List<InvoiceDetailModel> InvoiceDetail { get; set; }
        public string InvoiceDetailJson { get; set; }
        public int PaymentOptionTableValue { get; set; }
        public bool IsIVAApply { get; set; }
        public List<DebitNoteModel> AccountDebitNoteList { get; set; }
        public string DebitNotesJson { get; set; }
        #endregion
    }
    public class PaymentListModel
    {
        public List<PaymentModel> PaymentList { get; set; }
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
        public PrintTemplateModel PrintTemplate { get; set; }
        public List<AccountPaymentPlanDetailModel> PaymentPlanDetailList { get; set; }

        public List<CreditNoteModel> CreditNote { get; set; }
        public List<DebitNoteModel> DebitNote { get; set; }
    }
    public class PaymentDetailModel
    {
        public int? AccountServiceID { get; set; }
        public int? AccountServiceCollectionDetailID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal AmnestyAmount { get; set; }
        public decimal Total { get; set; }
        public decimal CollectionDetailDebt { get; set; }
        public int? AccountPaymentPlanID { get; set; }
        public int? AccountPaymentPlanDetailID { get; set; }
        public int? AccountPaymentPlanServiceDetailID { get; set; }
        public int? CollectionFillingPropertyTaxID { get; set; }
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
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public string RegisterNumber { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StateName { get; set; }
        public string CountyCode { get; set; }
    }

    //public class ServiceTypePayment
    //{
    //    public string Type { get; set; }
    //    public string CollectionType { get; set; }
    //    public decimal TotalReceived { get; set; }

    //    public string FormattedTotalReceived
    //    {
    //        get
    //        {

    //            return this.TotalReceived > 0 ? this.TotalReceived.ToString("C") : "";
    //        }
    //    }

    //}

    public class PaymentTypePayment
    {
        public string PaymentType { get; set; }
        public decimal TotalReceived { get; set; }

        public string FormattedTotalReceived
        {
            get
            {

                return this.TotalReceived > 0 ? this.TotalReceived.ToString("C") : "";
            }
        }

    }
}
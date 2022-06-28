using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class ClosingEntryModel
    {
        #region public properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public Guid CashierID { get; set; }
        public string CashierName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int ClosingTypeID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        [StringLength(50, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Number { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime Date { get; set; }
        [StringLength(250, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal NetClosing { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int PaymentReceiptCount { get; set; }
        public bool IsDeposited { get; set; }
        public string CommaSeperatedPaymentIds { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalChequedelBancoNacional { get; set; }
        public decimal TotalCreditCard { get; set; }
        public decimal TotalBankTransfer { get; set; }
        public decimal TotalChequedelBancodeCostaRica { get; set; }
        public decimal TotalAdjustment { get; set; }
        public decimal TotalBank { get; set; }
        public decimal TotalBankTransferByBancoNacionaldeCostaRica { get; set; }
        public bool IsVoid { get; set; }
        public string VoidReason { get; set; }
        public bool SelectedClosedEntry { get; set; }
        public string PaymentType { get; set; }
        public string ServiceType { get; set; }
        public string CollectionType { get; set; }
        public int? PaymentOptionID { get; set; }
        public string PaymentOptionName { get; set; }
        public int? FinanceCheckbookID { get; set; }
        public string FinanceCheckbookCode { get; set; }
        public string FinanceCheckbookName { get; set; }
        public int? CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public string PaymentOption { get; set; }
        #endregion

        #region References
        public IEnumerable<SelectListItem> CashierList { get; set; }
        public IEnumerable<SelectListItem> ClosingTypeList { get; set; }
        public List<PaymentModel> PostedPaymentReceiptList { get; set; }
        public IEnumerable<SelectListItem> PaymentOptionList { get; set; }
        public List<SelectListItemViewModel> FinanceCheckbookList { get; set; }
        #endregion

        #region Extra Columns
        public string FormattedNetClosing
        {
            get
            {
                return NetClosing.ToString("C");
            }
        }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        public string FormatingDate { get; set; }
        public string DepositedText { get; set; }
        public string Status
        {
            get
            {
                return IsVoid == true ? Resources.Global.Void : Resources.Global.Posted;
            }
        }
        public string RowVersion64
        {
            get
            {
                if (this.RowVersion != null)
                    return Convert.ToBase64String(this.RowVersion);

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.RowVersion = null;
                else
                    this.RowVersion = Convert.FromBase64String(value);
            }
        }
        public string ClosingTypeName { get; set; }
        public int DepositID { get; set; }
        public int ClosingID { get; set; }
        #endregion

        #region constructor
        public ClosingEntryModel()
        {
            PostedPaymentReceiptList = new List<PaymentModel>();
            PaymentOptionList = new List<SelectListItem>();
            FinanceCheckbookList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ClosingEntryListModel
    {
        #region public properties
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }
    public class ClosingEntryPrintModel
    {
        #region public properties
        public ClosingEntryModel ClosingEntry { get; set; }
        public List<PaymentModel> PaymentReceipts { get; set; }
        #endregion
    }
}
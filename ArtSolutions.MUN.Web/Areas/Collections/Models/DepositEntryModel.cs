using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class DepositEntryModel
    {
        #region public properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SequenceID { get; set; }
        public int? DepositTypeID { get; set; }
        public int? DepositTypeTableID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime Date { get; set; }
        [StringLength(50, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Number { get; set; }
        [StringLength(250, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal NetDeposit { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int ClosingCount { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsVoid { get; set; }
        public byte[] RowVersion { get; set; }
        public string VoidReason { get; set; }
        public string Status
        {
            get
            {
                return IsVoid == true ? Resources.Global.Void : Resources.Global.Posted;
            }
        }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int BankAccountID { get; set; }
        public string BankAccountCode { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public int? AccountID { get; set; }
        public string FinanceAccount { get; set; }
        public int? FinanceAccountID { get; set; }
        public string FinanceAccountCode { get; set; }
        public string FinanceAccountName { get; set; }
        #endregion

        #region References        
        public IEnumerable<SelectListItem> DepositTypeList { get; set; }
        public List<ClosingEntryModel> ClosedEntryList { get; set; }
        public List<FINBankAccountModel> BankAccountList { get; set; }
        #endregion

        #region Extra Columns
        public string DepositTypeName { get; set; }
        public string CommaSeperatedClosingIds { get; set; }
        public string FormattedNetDeposit
        {
            get
            {
                return NetDeposit.ToString("C");
            }
        }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
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
        #endregion

        #region constructor
        public DepositEntryModel()
        {
            ClosedEntryList = new List<ClosingEntryModel>();
        }
        #endregion
    }
    public class DepositEntryListModel
    {
        #region public properties
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }
    public class DepositEntryPrintModel
    {
        #region public properties
        public DepositEntryModel DepositEntry { get; set; }
        public List<ClosingEntryModel> ClosedEntryList { get; set; }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptByBankModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterBankAccountID { get; set; }
        public List<SelectListItemViewModel> BankAccountList { get; set; }
        public List<ReceiptByBankList> ReceiptByBankList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptByBankModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptByBankList
    {
        #region public properties
        public int ID { get; set; }
        public string ReceiptNumber { get; set; }
        public System.DateTime ReceiptDate { get; set; }
        public string AccountDisplayName { get; set; }
        public string ServiceTypeName { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditCard { get; set; }
        public string BankAccount { get; set; }
        public decimal Bank { get; set; }
        public decimal ApplyCreditAmount { get; set; }
        public decimal ChequedelBancoNacional { get; set; }
        public decimal BankTransferByBancodeCostaRica { get; set; }
        public decimal ChequedelBancodeCostaRica { get; set; }
        public decimal Adjustment { get; set; }
        public decimal BankTransferByBancoNacionaldeCostaRica { get; set; }
        public string BankName { get; set; }
        public List<ReceiptByBankList> BankAccountList { get; set; }
        #endregion
    }
}
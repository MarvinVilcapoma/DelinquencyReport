using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptDetailModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<SelectListItemViewModel> AccountServiceTypeList { get; set; }
        public List<SelectListItemViewModel> InvoiceTypeList { get; set; }
        public List<SelectListItemViewModel> PaymentPlanTypeList { get; set; }
        public List<SelectListItemViewModel> BankAccountList { get; set; }
        public List<SelectListItemViewModel> GrantList { get; set; }
        public List<SelectListItemViewModel> CheckbookList { get; set; }
        public string[] AccountIDs { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterCashierID { get; set; }
        public string[] AccountServiceTypeIDs { get; set; }
        public string FilterAccountServiceTypeID { get; set; }
        public string[] InvoiceTypeIDs { get; set; }
        public string FilterInvoiceTypeID { get; set; }
        public string[] PaymentPlanTypeIDs { get; set; }
        public string FilterPaymentPlanTypeID { get; set; }
        public string[] BankAccountIDs { get; set; }
        public string FilterBankAccountID { get; set; }
        public string[] GrantIDs { get; set; }
        public string FilterGrantID { get; set; }
        public string[] CheckbookIDs { get; set; }
        public string FilterCheckbookID { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptDetailList> ReceiptDetailList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptDetailModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptDetailList
    {
        #region public properties
        public int ID { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public string RegisterNumber { get; set; }
        public string DisplayName { get; set; }
        public string ServiceTypeName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClosingNumber { get; set; }
        public string DepositNumber { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }       
        public string GrantCode { get; set; }
        public string GrantName { get; set; }
        public string ChecbookName { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public System.Guid Collector { get; set; }
        public decimal TotalAllocated { get; set; }
        public decimal ReceiptTotalAmount { get; set; }
        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("d");
            }
        }
        public string FormattedPercentage
        {
            get
            {
                return this.Percentage!=null?(this.Percentage.Value / 100).ToString("P"): " ";
            }
        }
        public string FormattedTotalAllocated
        {
            get
            {
                return this.TotalAllocated > 0 ? this.TotalAllocated.ToString("C") : "";
            }
        }
        public string FormattedReceiptTotalAmount
        {
            get
            {
                return this.ReceiptTotalAmount > 0 ? this.ReceiptTotalAmount.ToString("C") : "";
            }
        }
        public string FormattedCashier
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.Collector.ToString()).FirstOrDefault();                
                return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptPermissionFeesModel : DataExportModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterCashierID { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptPermissionFeesModelList> ReceiptPermissionFeesList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }
        #endregion

        #region Constructor
        public ReceiptPermissionFeesModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptPermissionFeesModelList
    {
        #region public properties
        public System.DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string TypeName { get; set; }
        public decimal PaidAmount { get; set; }
        #endregion

        #region custom properties 
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        public string FormattedPaidAmount
        {
            get
            {
                return PaidAmount.ToString("C");
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptIVUModel : DataExportModel
    {
        #region public properties     
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
        public List<ReceiptIVUModelList> IVUReceiptDetailList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptIVUModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptIVUModelList
    {
        #region public properties
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public string Number { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public Nullable<decimal> Charges { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public decimal TotalPayment { get; set; }
        public int EndPeriodID { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.Guid FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public string User { get; set; }        
        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        public string FormattedPeriod
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.EndPeriodID) + "-" + this.DueDate.Year.ToString();
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.Value.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.Value.ToString("C");
            }
        }
        public string FormattedCharges
        {
            get
            {
                return Charges.Value.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.Value.ToString("C");
            }
        }
        public string FormattedDiscount
        {
            get
            {
                return Discount.Value.ToString("C");
            }
        }
        public string FormattedPaymentamount
        {
            get
            {
                return TotalPayment.ToString("C");
            }
        }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d") : "";
            }
        }
        public string FormattedUser
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.FillingBy.ToString()).FirstOrDefault();
                this.User = string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                return this.IsFromPortal == false ? this.User : (this.User + " ( " + Report.MerchantsPortal + " ) ");
            }
        }
        public string Type
        {
            get
            {               
                return this.ReFillingDate == null ? Report.Monthly : Report.Amendment;
            }
        }
        #endregion
    }
}
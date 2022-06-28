using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseBalanceClosingModel : DataExportModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }       
        public string FilterAccountID { get; set; }
        public List<BusinessLicenseBalanceByClosingList> BusinessLicenseBalanceByClosingList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenseBalanceClosingModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class BusinessLicenseBalanceByClosingList
    {
        #region public properties
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public decimal Principal1 { get; set; }
        public decimal Principal2 { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Adjustment { get; set; }
        public string FormattedFiscalYearID { get; set; }
        public decimal TotalPayment { get; set; }
        public Nullable<DateTime> Date { get; set; }
        #endregion

        #region custom properties 
        public string FormattedPrincipal1
        {
            get
            {
                return Principal1.ToString("C");
            }
        }
        public string FormattedPrincipal2
        {
            get
            {
                return Principal2.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.ToString("C");
            }
        }
        public string FormattedCharges
        {
            get
            {
                return Charges.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.ToString("C");
            }
        }

        public string FormattedPaymentamount
        {
            get
            {
                return TotalPayment.ToString("C");
            }
        }
        public string FormattedAdjustment
        {
            get
            {
                return Adjustment.ToString("C");
            }
        }
        public string FormattedDate
        {
            get
            {
                return Date.HasValue ? Date.Value.ToString("d") : "";
            }
        }
        #endregion
    }
}
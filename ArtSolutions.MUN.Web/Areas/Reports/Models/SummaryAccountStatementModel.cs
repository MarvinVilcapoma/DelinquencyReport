using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class SummaryAccountStatementModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public int? AccountPropertyID { get; set; }
        public string FilterYearID { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public string CommaSeperatedYearIDs { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
        public List<SummaryAccountStatementList> SummaryAccountStatementList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        #endregion

        #region Constructor
        public SummaryAccountStatementModel()
        {
            this.YearList = new List<SelectListItem>();
            this.AccountList = new List<AccountSearchModel>();
            this.PropertyList = new List<SelectListItem>();
        }
        #endregion
    }
    public class SummaryAccountStatementList
    {
        #region public properties
        public string Service { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Interest { get; set; }
        public decimal PendingAmount { get; set; }
        public string Period { get; set; }
        public int IsMonthly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MINDueDate { get; set; }
        public DateTime MAXDueDate { get; set; }
        #endregion

        #region custom properties
        public string FormattedService
        {
            get
            {
                return Service.Length > 40 ? Service.Substring(0, 40) + "..." : Service;
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.ToString("C");
            }
        }
        public string FormattedPendingAmount
        {
            get
            {
                return PendingAmount.ToString("C");
            }
        }
        public string FormattedPeriod
        {
            get
            {
                return IsMonthly == 0 ? (String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", MINDueDate) + " - " + String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", MAXDueDate)) : Period;
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DebtCertificationModel : DataExportModel
    {
        #region public properties     
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public List<DebtCertificationList> DebtCertificationList { get; set; }
        public AccountModel AccountModel { get; set; }
        public List<AccountPropertyModel> AccountPropertyList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public DebtCertificationModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class DebtCertificationList
    {
        #region public properties
        public int ID { get; set; }
        public string Service { get; set; }
        public string Period { get; set; }
        public string CustomField { get; set; }
        public DateTime DueDate { get; set; }
        public int IsMonthly { get; set; }
        public decimal PenaltyPercentage { get; set; }
        public decimal Penalties { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal Interest { get; set; }
        public decimal Principal { get; set; }
        public decimal Total { get; set; }
        #endregion

        #region custom properties         
        public string FormattedPeriod
        {
            get
            {
                return IsMonthly == 0 ? String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", DueDate) : Period;
            }
        }
        public string FormattedPenaltyPercentage
        {
            get
            {
                return PenaltyPercentage.ToString("P");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.ToString("C");
            }
        }
        public string FormattedInterestPercentage
        {
            get
            {
                return InterestPercentage.ToString("P");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.ToString("C");
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedTotal
        {
            get
            {
                return Total.ToString("C");
            }
        }
        #endregion
    }
}
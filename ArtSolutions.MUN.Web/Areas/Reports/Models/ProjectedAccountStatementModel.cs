using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ProjectedAccountStatementModel
    {
        #region public properties    
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public DateTime? Date { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<ProjectedAccountStatementList> ProjectedAccountStatementList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public ProjectedAccountStatementModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class ProjectedAccountStatementList
    {
        #region public properties  
        public string RightNumber { get; set; }
        public string ServiceName { get; set; }
        public string Segrega { get; set; }
        public string PeriodName { get; set; }
        public int? IsMonthly { get; set; }
        public DateTime MINDueDate { get; set; }
        public DateTime MAXDueDate { get; set; }
        public string MeterNumber { get; set; }
        public decimal? Principal { get; set; }
        public decimal? Interest { get; set; }
        public decimal? IVA { get; set; }
        public decimal? Total { get; set; }
        #endregion

        #region custom properties
        public string FormattedPeriod
        {
            get
            {
                return IsMonthly == 0 ? (String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", MINDueDate) + " - " + String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", MAXDueDate)) : PeriodName;
            }
        }
        #endregion
    }
}
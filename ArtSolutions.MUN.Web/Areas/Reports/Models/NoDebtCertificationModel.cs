using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class NoDebtCertificationModel
    {
        #region public properties     
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string Note { get; set; }
        public List<NoDebtCertificationList> NoDebtCertificationList { get; set; }
        public AccountModel AccountModel { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public NoDebtCertificationPeriodModel NoDebtCertificationPeriodModel { get; set; }
        public int TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Description1Detail { get; set; }
        public string Description2Detail { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string TaxNumber { get; set; }
        public string Observations { get; set; }
        #endregion

        #region Constructor
        public NoDebtCertificationModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class NoDebtCertificationList
    {
        #region public properties
        public string Right { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal Area { get; set; }
        #endregion

        #region custom properties                
        public string FormattedTotalValue
        {
            get
            {
                return this.TotalValue == null ? null : this.TotalValue.Value.ToString("C");
            }
        }
        public string FormattedArea
        {
            get
            {
                return this.Area.ToString(Common.DecimalPoints);
            }
        }
        #endregion
    }
    public class NoDebtCertificationPeriodModel
    {
        #region public properties
        public int? IsPayed { get; set; }
        public int? IsPropertyTaxPayed { get; set; }
        public string PropertyTaxPeriod { get; set; }
        public int? PropertyTaxYear { get; set; }
        public string OtherPeriod { get; set; }
        public int? OtherYear { get; set; }
        #endregion

        #region custom properties        
        public string FormattedPropertyTaxPeriod
        {
            get
            {
                return this.PropertyTaxPeriod + "  " + Resources.Report.QuarterOfTheYear + "  " + this.PropertyTaxYear;
            }
        }
        public string FormattedOtherPeriod
        {
            get
            {
                return this.OtherPeriod + "  " + Resources.Report.QuarterOfTheYear + "  " + this.OtherYear;
            }
        }
        #endregion
    }
}
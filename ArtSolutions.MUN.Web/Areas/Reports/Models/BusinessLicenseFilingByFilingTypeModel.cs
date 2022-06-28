using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseFilingByFilingTypeModel : DataExportModel
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
        public List<BusinessLicenseFilingByFilingTypeList> BusinessLicenseFilingByFilingTypeList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenseFilingByFilingTypeModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class BusinessLicenseFilingByFilingTypeList
    {
        #region public properties
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string SocialSecurity { get; set; }
        public int FiscalYearID { get; set; }
        public string FormattedFiscalYearID { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public string AccountIdByFilling { get; set; }
        public decimal SalesVolume { get; set; }
        public Nullable<System.Guid> FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string User { get; set; }
        #endregion

        #region custom properties 
        public string FormattedSalesVolume
        {
            get
            {
                return SalesVolume.ToString("C");
            }
        }       
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d") : "";
            }
        }
        public string FormattedDate
        {
            get
            {
                return CreatedDate.ToString("d");
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
        #endregion
    }
}
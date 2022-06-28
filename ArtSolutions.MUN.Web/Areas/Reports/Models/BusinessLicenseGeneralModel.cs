using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseGeneralModel : DataExportModel
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
        public List<BusinessLicenseGeneralList> BusinessLicenseGeneralList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenseGeneralModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }

    public class BusinessLicenseGeneralList
    {
        #region public properties
        public string TaxNumber { get; set; }
        public string PatentNumber { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string District { get; set; }
        public decimal Balance { get; set; }
        #endregion

        #region custom properties 
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        #endregion
    }
}
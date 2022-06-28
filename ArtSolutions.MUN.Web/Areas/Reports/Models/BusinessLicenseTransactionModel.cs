using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseTransactionModel : DataExportModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<BusinessLicenseTransactionList> BusinessLicenseTransactionList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }        
        public int TotalRecord { get; set; }
        public int searchStatusID { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenseTransactionModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }

    public class BusinessLicenseTransactionList
    {
        #region public properties
        public DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public string FormattedFiscalYearID { get; set; }
        public string TransactionType { get; set; }
        public string Note { get; set; }
        public string PatentNumber { get; set; }
        #endregion

        #region custom properties 
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountServicesMissingFilingModel
    {
        #region public properties      
        public List<AccountServicesMissingFilingList> AccountServicesMissingFilingList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int? AccountId { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public string[] ServiceIDs { get; set; }
        public string FilterServiceIDs { get; set; }
        public List<SelectListItem> ServiceList { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AccountServicesMissingFilingModel()
        {
            this.AccountList = new List<AccountSearchModel>();
            this.ServiceList = new List<SelectListItem>();
        }
        #endregion
    }
    public class AccountServicesMissingFilingList
    {
        #region public properties   
        public int AccountServiceID { get; set; }
        public string AccountName { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> IsMonthly { get; set; }
        public System.DateTime DueDate { get; set; }
        public string PeriodName { get; set; }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class HistoricalAccountServiceRemovedModel
    {
        #region public properties          
        public Guid RemovedByAccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<HistoricalAccountServiceRemovedList> HistoricalAccountServiceRemovedList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public HistoricalAccountServiceRemovedModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class HistoricalAccountServiceRemovedList
    {
        #region public properties  
        public string AccountName { get; set; }
        public string ServiceName { get; set; }
        public string RigthNumber { get; set; }
        public string ReasonForDeleted { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public string DeletedByAccountName
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.ModifiedUserID.ToString()).FirstOrDefault();
                return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
              
            }
            #endregion
        }
    }
    }
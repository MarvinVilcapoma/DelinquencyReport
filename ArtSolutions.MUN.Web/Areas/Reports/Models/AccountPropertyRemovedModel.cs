using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountPropertyRemovedModel
    {
        #region public properties          
        public Guid RemovedByAccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<AccountPropertyRemovedList> AccountPropertyRemovedList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AccountPropertyRemovedModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class AccountPropertyRemovedList
    {
        #region public properties  
        public string AccountName { get; set; }
        public string RigthNumber { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public string DeletedByAccountName
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.ModifiedUserID.ToString()).FirstOrDefault();
                return model != null ? string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName : "";

            }

        }
        #endregion
    }
}
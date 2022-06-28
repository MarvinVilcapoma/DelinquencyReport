using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class GeneralMovementsModel
    {
        #region public properties 
        public int? AccountId { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public Guid? UserID { get; set; }
        public List<SelectListItemViewModel> UserList { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime? ToDate { get; set; }
        public List<GeneralMovementsList> GeneralMovementsList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public GeneralMovementsModel()
        {
            this.UserList = new List<SelectListItemViewModel>();
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }

    public class GeneralMovementsList
    {
        #region public properties  
        public string AccountName { get; set; }
        public string ServiceName { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
        public Guid? ModifiedUserID { get; set; }
        #endregion

        #region custom properties
        public string CreatedBy
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.CreatedUserID.ToString()).FirstOrDefault();
                if (model != null)
                {
                    return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                }
                else
                {
                    return null;
                }
            }
        }
        public string FormatedNote
        {
            get
            {
                if (this.ModifiedUserID != Guid.Empty)
                {
                    string modifiedUserName = null;
                    UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.ModifiedUserID.ToString()).FirstOrDefault();
                    if (model != null)
                    {
                        modifiedUserName = string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                        return this.Note.Replace("{}", modifiedUserName);
                    }
                    else
                    {
                        return this.Note;
                    }
                }
                else
                {
                    return "Account deactivated by: System Admin";
                }
            }
        }
        public string FormattedCreatedDate
        {
            get
            {
                return this.CreatedDate.ToString("d");
            }
        }
        public string FormattedCreatedTime
        {
            get
            {
                return this.CreatedDate.ToLongTimeString();
            }
        }
        #endregion
    }
}
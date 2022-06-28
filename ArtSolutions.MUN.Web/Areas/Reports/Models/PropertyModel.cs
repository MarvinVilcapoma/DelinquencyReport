using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class PropertyModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public string AccountPropertyID { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
        public List<PropertyMovementList> PropertyMovementList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public PropertyModel()
        {
            this.AccountList = new List<AccountSearchModel>();
            this.PropertyList = new List<SelectListItem>();
        }
        #endregion
    }

    public class PropertyMovementList
    {
        #region public properties

        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public string CondoType { get; set; }
        public string Condo { get; set; }
        public Nullable<decimal> TotalArea { get; set; }
        public decimal TotalValue { get; set; }
        public string MovementType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> PreviousTotalValue { get; set; }
        public Nullable<int> TotalRight { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public DateTime Date { get; set; }
        public string CreatedUserID { get; set; }
        public string Note { get; set; }
        public string FormattedTotalArea
        {
            get
            {
                return this.TotalArea != null ? this.TotalArea.Value.ToString(Common.DecimalPoints) : null;
            }
        }
        #endregion

        #region custom properties
        public string FormattedTotalValue
        {
            get
            {
                return TotalValue.ToString("C2");
            }
        }
        public string FormattedPreviousTotalValue
        {
            get
            {
                return PreviousTotalValue.HasValue ? PreviousTotalValue.Value.ToString("C2") : null;
            }
        }
        #endregion
        #region custom properties
        public string Who
        {
            get
            {
                if (CreatedUserID != null)
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
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
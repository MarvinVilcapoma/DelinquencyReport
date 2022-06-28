using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionDebtModel
    {
        #region Properties
        public int ID { get; set; }
        public int AccountServiceCollectionDetailID { get; set; }
        public string Description { get; set; }
        public decimal Debt { get; set; }
        public decimal PercentageRate { get; set; }
        public decimal FlatAmount { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalty { get; set; }
        public decimal OfPercentage { get; set; }
        public List<string> CollectionDetailName { get; set; }
        public string ServiceName { get; set; }
        public string AdjustmentReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<int> IsAdjustment { get; set; }
        public Nullable<int> IsExemption { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string ItemDescription { get; set; }
        public int FrequencyID { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public Nullable<bool> IsIVAApply { get; set; }
        #endregion

        #region public methods
        public List<AccountServiceCollectionDebtModel> Get(int accountServiceId, int? accountServiceCollectionDetailId, bool? onlyAdjustment, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailID", Value = accountServiceCollectionDetailId });
            lstParameter.Add(new NameValuePair { Name = "onlyAdjustment", Value = onlyAdjustment });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDebtModel>>(APISelector.Municipality, true, "api/AccountService/CollectionDebtGet", "GET", lstParameter);
        }
        public string DeletedBy
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
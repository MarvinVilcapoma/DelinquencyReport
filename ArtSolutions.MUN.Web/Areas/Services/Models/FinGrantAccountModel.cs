using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FinGrantAccountModel
    {
        #region public properties
        public List<FinAccountModel> ReceivableAccountList { get; set; }
        public List<FinAccountModel> RevenueAccountList { get; set; }
        public List<CheckbookAccountListModel> CheckbookAccountList { get; set; }
        #endregion

        #region public methods
        public FinGrantAccountModel Get(int grantId, EnumUtility.FINAccountType accountType)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "GrantID", Value = grantId });
            lstParameter.Add(new NameValuePair { Name = "AccountType", Value = accountType.ToString() });
            return new RestSharpHandler().RestRequest<FinGrantAccountModel>(APISelector.Municipality, true, "api/CollectionTemplate/FinanceAccountGet", "GET", lstParameter, null);
        }
        #endregion
    }
}
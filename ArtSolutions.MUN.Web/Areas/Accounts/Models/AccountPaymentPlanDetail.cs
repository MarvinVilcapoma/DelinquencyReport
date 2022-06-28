using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPaymentPlanDetail
    {
       #region Public Methods 
        public List<AccountPaymentPlanDetailModel> Get(int? id, string accountpaymentplanIds, bool? isActive , bool? isPaid,bool? isRemoveInterest)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                 new NameValuePair { Name = "id", Value = id },
                 new NameValuePair { Name = "accountPaymentPlanIDs", Value = accountpaymentplanIds },
                 new NameValuePair { Name = "isActive", Value = isActive },
                 new NameValuePair { Name = "isPaid", Value = isPaid },
                 new NameValuePair { Name = "isRemoveInterest", Value = isRemoveInterest },
            };
            return new RestSharpHandler().RestRequest<List<AccountPaymentPlanDetailModel>>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetAccountPaymentPlanDetail", "GET", lstParameter);
        }
        #endregion
    }
}
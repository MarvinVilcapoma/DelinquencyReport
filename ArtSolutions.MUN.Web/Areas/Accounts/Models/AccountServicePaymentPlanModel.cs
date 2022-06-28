using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServicePaymentPlanModel : AccountPaymentPlanModel
    {
        #region public properties 
        public int AccountPaymentPlanID { get; set; }
        public int AccountServiceID { get; set; }
        public int AccountServiceCollectionDetailID { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        #endregion

        #region Public Methods 
        public List<AccountServicePaymentPlanModel> Get(int? accountID,int? accountPaymentPlanID, int? accountServiceID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();             
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "accountPaymentPlanID", Value = accountPaymentPlanID });
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceID });
            return new RestSharpHandler().RestRequest<List<AccountServicePaymentPlanModel>>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetAccountServicePaymentPlan", "GET", lstParameter);
        }

        public List<AccountServiceCollectionDetailModel> GetSummary(int accountPaymentPlanID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountPaymentPlanID", Value = accountPaymentPlanID });
            //lstParameter.Add(new NameValuePair { Name = "serviceTypeID", Value = serviceTypeID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDetailModel>>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetAccountServicePaymentPlanSummary", "GET", lstParameter);
        }
        #endregion
    }
}
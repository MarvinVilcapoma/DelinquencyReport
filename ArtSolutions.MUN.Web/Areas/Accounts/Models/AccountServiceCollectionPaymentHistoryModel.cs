using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionPaymentHistoryModel
    {
        #region public properties       
        public string ItemDescription { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentNumber { get; set; }
        public string ServiceName { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public int PaymentID { get; set; }
        public string PaymentType { get; set; }
        public Nullable<decimal> ApplyCreditAmount { get; set; }
        public Nullable<decimal> AvailableCreditAmount { get; set; }
        public int FrequencyID { get; set; }
        #endregion

        #region public methods
        public List<AccountServiceCollectionPaymentHistoryModel> Get(int accountServiceId, int? accountServiceCollectionDetailId, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailID", Value = accountServiceCollectionDetailId });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionPaymentHistoryModel>>(APISelector.Municipality, true, "api/AccountService/PaymentHistoryGet", "GET", lstParameter);
        }
        #endregion

        #region custom properties        
        public string FormattedApplyCreditAmount
        {
            get
            {
                return ApplyCreditAmount.HasValue? ApplyCreditAmount.Value.ToString("C"):"-";
            }
        }
        public string FormattedAvailableCreditAmount
        {
            get
            {
                return AvailableCreditAmount.HasValue ? AvailableCreditAmount.Value.ToString("C") : "-";
            }
        }
        #endregion
    }
}
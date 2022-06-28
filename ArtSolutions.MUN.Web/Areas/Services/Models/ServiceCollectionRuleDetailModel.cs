using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCollectionRuleDetailModel : ServiceCollectionRuleModel
    {
        #region properties
        public int ServiceCollectionRuleID { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Value { get; set; }
        public Nullable<decimal> SecondValue { get; set; }
        public bool IsEditable { get; set; }
        
        #endregion

        #region public methods
        public List<ServiceCollectionRuleDetailModel> Get(int serviceId, int? id,int? serviceCollectionRuleID, bool? isDeleted)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceId", Value = serviceId });
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "serviceCollectionRuleID", Value = serviceCollectionRuleID });
            lstParameter.Add(new NameValuePair { Name = "isDeleted", Value = isDeleted });
            return new RestSharpHandler().RestRequest<List<ServiceCollectionRuleDetailModel>>(APISelector.Municipality, true, "api/Service/ServiceCollectionRuleDetailGet", "GET", lstParameter);
        }

        public List<ServiceCollectionRuleDetailModel> AppendBlankListItem(List<ServiceCollectionRuleDetailModel> existingDetailList)
        {
            for(int i= existingDetailList.Count + 1; i <= 3; i++)
            {
                existingDetailList.Add(new ServiceCollectionRuleDetailModel());
            }
            return existingDetailList;
        }
        #endregion
    }
}
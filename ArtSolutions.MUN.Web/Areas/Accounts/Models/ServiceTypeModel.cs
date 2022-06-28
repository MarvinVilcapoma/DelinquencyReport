using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class ServiceTypeModel
    {
        #region public properties       
        public int ID { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        #endregion

        #region public methods 
        public List<ServiceTypeModel> GetServiceType(int groupId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "GroupID", Value = groupId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = DBNull.Value });
            return new RestSharpHandler().RestRequest<List<ServiceTypeModel>>(APISelector.Municipality, true, "api/AccountService/ServiceTypeGet", "GET", lstParameter);
        }
        public List<ServiceTypeModel> GetNotPaid(int accountId,bool? checkActivePaymentPlan)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "checkActivePaymentPlan", Value = checkActivePaymentPlan });
            return new RestSharpHandler().RestRequest<List<ServiceTypeModel>>(APISelector.Municipality, true, "api/AccountService/ServiceTypeGetNotPaid", "GET", lstParameter);
        }
        public List<ServiceTypeModel> GetByNoFilingType(bool isNoFilingType)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "isNoFilingType", Value = isNoFilingType });           
            return new RestSharpHandler().RestRequest<List<ServiceTypeModel>>(APISelector.Municipality, true, "api/AccountService/ServiceTypeGetByNoFilingType", "GET", lstParameter);
        }
        #endregion
    }
}
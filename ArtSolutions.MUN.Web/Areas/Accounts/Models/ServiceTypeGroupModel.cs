using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class ServiceTypeGroupModel
    {
        #region public properties       
        public int ID { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        #endregion

        #region public methods  
        public List<ServiceTypeGroupModel> GetServiceTypeGroup()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = DBNull.Value });
            return new RestSharpHandler().RestRequest<List<ServiceTypeGroupModel>>(APISelector.Municipality, true, "api/AccountService/ServiceTypeGroupGet", "GET", lstParameter);
        }
        #endregion
    }
    public enum EnServiceGroup
    {
        License = 1,
        Tax = 2,
        Permit = 3,
        Other = 4,
        Rent = 5
    }
    public enum EnFilingForm
    {
        License = 1,
        IVU = 2,
        Unit = 3,
        MeasuredWater = 4,
        PropertyTax = 5,
        LiquorLicense = 6
    }
}
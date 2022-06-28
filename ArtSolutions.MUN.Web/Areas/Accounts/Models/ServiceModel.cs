using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class ServiceModel
    {
        #region public properties      
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime InitialDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime FillingDate { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public string Description { get; set; }
        #endregion

        #region public methods  
        public List<ServiceModel> GetService(int serviceTypeId, int groupId, int? serviceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ServiceTypeID", Value = serviceTypeId });
            lstParameter.Add(new NameValuePair { Name = "GroupID", Value = groupId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = serviceId });
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(true, true, "api/AccountService/ServiceGet", "GET", lstParameter);
        }
        #endregion
    }
}
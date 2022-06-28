using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FrequencyModel
    {
        #region Public Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Days { get; set; }
        public decimal Hours { get; set; }
        public Nullable<int> Period { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        #endregion

        #region public methods 
        public List<FrequencyModel> Get(bool? IsServiceFrequency = null)
        {
            List<NameValuePair> parameters = new List<NameValuePair>();
            parameters.Add(new NameValuePair() { Name =  "IsServiceFrequency", Value = IsServiceFrequency });
            return new RestSharpHandler().RestRequest<List<FrequencyModel>>(APISelector.Municipality, true, "api/Service/FrequencyGet", "GET", parameters);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Companies.Models
{
    public class CountryModel
    {
        #region public properties        
        public int ID { get; set; }
        public Nullable<bool> AllowsBilling { get; set; }
        public Nullable<bool> AllowsShipping { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public Nullable<int> NumericIsoCode { get; set; }
        public Nullable<bool> SubjectToVat { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        #endregion

        #region public methods   
        public List<StateModel> GetStates(int countryId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CountryID", Value = countryId });
            return new RestSharpHandler().RestRequest<List<StateModel>>(APISelector.Municipality, true, "api/Country/StateGetByCountryID", "GET", lstParameter);
        }

        public List<CityModel> GetCities(int countryId, int stateId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CountryID", Value = countryId });
            lstParameter.Add(new NameValuePair { Name = "StateID", Value = stateId });
            return new RestSharpHandler().RestRequest<List<CityModel>>(APISelector.Municipality, true, "api/Country/CityGetByCountryIdAndStateId", "GET", lstParameter);
        }

        public List<TownModel> GetTowns(int countryId, int stateId, int cityId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CountryID", Value = countryId });
            lstParameter.Add(new NameValuePair { Name = "StateID", Value = stateId });
            lstParameter.Add(new NameValuePair { Name = "CityID", Value = cityId });
            return new RestSharpHandler().RestRequest<List<TownModel>>(APISelector.Municipality, true, "api/Country/TownGetByCountryIdStateIdAndCityId", "GET", lstParameter);
        }
        #endregion
    }
    public class StateModel
    {
        #region public properties        
        public int CountryID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion
    }
    public class CityModel
    {
        #region public properties            
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion
    }
    public class TownModel
    {
        #region public properties        
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion
    }
}
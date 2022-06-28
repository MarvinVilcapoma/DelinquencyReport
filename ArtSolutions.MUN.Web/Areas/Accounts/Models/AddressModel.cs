using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AddressModel
    {
        #region public properties        
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int AccountID { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AddressTypeID { get; set; }
        public string AddressType { get; set; }
        public int? AddressTypeTableID { get; set; }
        public bool IsPrimary { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> CountryID { get; set; }
        public string Country { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> CountryStateID { get; set; }
        public string State { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> CityID { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> TownID { get; set; }
        public string City { get; set; }
        public string Town { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [RegularExpression(@"^([0-9]-?){5,10}$", ErrorMessageResourceName = "Valzipcode", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ZipPostalCode { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem> AddressTypeList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> TownList { get; set; }
        public string TwoLetterIsoCode { get; set; }
        #endregion

        #region Constructor
        public AddressModel()
        {
            StateList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            TownList = new List<SelectListItem>();
        }
        #endregion

        #region public methods   
        public List<AddressModel> Get(int? addressID, int? accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "addressID", Value = addressID });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<List<AddressModel>>(APISelector.Municipality, true, "api/Account/AddressGet", "GET", lstParameter);
        }

        public AddressModel Get()
        {
            AddressModel model = new AddressModel();
            model.AddressTypeList = HMTLHelperExtensions.CreateSelectList(GetAddressTypes(), "ID", "Name");
            model.CountryList = HMTLHelperExtensions.CreateSelectList(GetCountries(), "ID", "Name");
            return model;
        }
        public int GetAddressesForExist(int ID, int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/GetAddressesForExist", "GET", lstParameter);
        }
        public static string GetFullAddress(AddressModel model)
        {
            string address = null;

            if (!string.IsNullOrEmpty(model.AddressLine1))
                address += model.AddressLine1 + ", ";
            if (!string.IsNullOrEmpty(model.AddressLine2))
                address += model.AddressLine2 + ", ";
            if (!string.IsNullOrEmpty(model.City))
                address += model.City + ", ";
            if (!string.IsNullOrEmpty(model.Town))
                address += model.Town + ", ";
            if (!string.IsNullOrEmpty(model.State))
                address += model.State + ", ";
            if (!string.IsNullOrEmpty(model.Country))
                address += model.Country + ", ";
            if (!string.IsNullOrEmpty(model.ZipPostalCode))
                address += model.ZipPostalCode + ", ";

            return address == null ? null : address.TrimEnd(' ').TrimEnd(',');
        }
        #endregion
    
        #region private methods 
        private List<SupportValueModel> GetAddressTypes()
        {
            return new AccountModel().GetSupportValues(6);
        }

        public List<CountryModel> GetCountries()
        {
            return new RestSharpHandler().RestRequest<List<CountryModel>>(APISelector.Municipality, true, "api/Country/Get", "GET", null);
        }

        public IEnumerable<SelectListItem> GetStates(int countryId)
        {
            return HMTLHelperExtensions.CreateSelectList(new CountryModel().GetStates(countryId), "ID", "Name", null, true, true, Global.DropDownSelectMessage);
        }

        public IEnumerable<SelectListItem> GetCities(int countryId, int stateId)
        {
            return HMTLHelperExtensions.CreateSelectList(new CountryModel().GetCities(countryId, stateId), "ID", "Name", null, true, true, Global.DropDownSelectMessage);
        }

        public IEnumerable<SelectListItem> GetTowns(int countryId, int stateId, int cityId)
        {
            return HMTLHelperExtensions.CreateSelectList(new CountryModel().GetTowns(countryId, stateId, cityId), "ID", "Name", null, true, true, Global.DropDownSelectMessage);
        }
        #endregion
    }
}
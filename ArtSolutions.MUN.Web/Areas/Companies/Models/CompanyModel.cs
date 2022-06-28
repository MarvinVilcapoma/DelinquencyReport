using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Companies.Models
{
    public class CompanyModel
    {
        #region public properties       
        public int CompanyID { get; set; }
        public int ID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string LegalName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string WorkPhone { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Url(ErrorMessageResourceName = "ValWebsiteUrl", ErrorMessageResourceType = typeof(Resources.Global))]
        public string URL { get; set; }
        public string Email { get; set; }
        public Nullable<int> LogoID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int CountryID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CompanySize { get; set; }
        public bool IsAgreedTerms { get; set; }
        public System.DateTime AgreeTermsOnDate { get; set; }
        public string TermsDetails { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string Modules { get; set; }
        public List<Guid> ModuleIDs { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public List<ModuleModel> ModuleList { get; set; }
        public CurrencyModel CurrencyModel { get; set; }
        public CompanyParameterModel CompanyParameter { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Code { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Address1 { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Address2 { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public Nullable<int> CountryStateID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        [RegularExpression(@"^([0-9]-?){5,10}$", ErrorMessageResourceName = "Valzipcode", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ZipPostalCode { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public Nullable<int> CompnayCountryID { get; set; }
        public Nullable<int> MUNAccountAddressesID { get; set; }
        #endregion

        #region constructor
        public CompanyModel()
        {
            this.ModuleList = new List<ModuleModel>();
            this.CountryList = new List<SelectListItem>();
            this.CurrencyList = new List<SelectListItem>();
        }
        #endregion

        #region public methods     
        public CompanyModel GetCompanyFromSecurity()
        {
            if (GetCompany() == null)
            {
                //Company not exists in Municipality so get company detail from Security
                CompanyModel securityCompanyModel = GetCompanyModelFromSecurity();
                CompanyModel model = new CompanyModel();
                model.CompanyID = UserSession.Current.CompanyID;
                model.Name = securityCompanyModel.Name;
                model.LegalName = securityCompanyModel.Name;
                model.WorkPhone = securityCompanyModel.WorkPhone;
                model.URL = securityCompanyModel.URL;
                model.CompanySize = securityCompanyModel.CompanySize;
                model.CountryID = securityCompanyModel.CountryID;
                model.CountryList = HMTLHelperExtensions.CreateSelectList(GetCountries(), "ID", "Name");
                model.CurrencyList = HMTLHelperExtensions.CreateSelectList(GetCurrency(), "ID", "Currency", null, false, false, "ALL", true);
                model.ModuleList = GetApplicationModules();
                return model;
            }
            return null;
        }

        public CompanyModel GetCompany()
        {
            return new RestSharpHandler().RestRequest<CompanyModel>(APISelector.Municipality, true, "api/Company/Get", "GET", null);
        }

        public CompanyModel Edit()
        {
            CompanyModel model = this.GetCompany();
            if (string.IsNullOrEmpty(model.Address1))
            {
                string url = "api/Company/GetCompanyAddress";
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "addressType", Value = "4" });
                CompanyAddressModel companyAddressModel = new RestSharpHandler().RestRequest<CompanyAddressModel>(APISelector.Security, true, url, "GET", lstParameter);

                if (string.IsNullOrEmpty(companyAddressModel.Address1))
                {
                    lstParameter = new List<NameValuePair>();
                    lstParameter.Add(new NameValuePair { Name = "addressType", Value = "2" });
                    companyAddressModel = new RestSharpHandler().RestRequest<CompanyAddressModel>(APISelector.Security, true, url, "GET", lstParameter);
                }

                if (!string.IsNullOrEmpty(companyAddressModel.Address1))
                {
                    model.Address1 = companyAddressModel.Address1;
                    model.Address2 = companyAddressModel.Address2;
                    model.City = companyAddressModel.City;
                    model.CountryStateID = companyAddressModel.StateProvinceID;
                    model.CompnayCountryID = companyAddressModel.CountryID;
                    model.ZipPostalCode = companyAddressModel.ZipPostalCode;
                }
            }
            model.CountryList = HMTLHelperExtensions.CreateSelectList(GetCountries(), "ID", "Name");
            // Get Company Parameters
            model.CompanyParameter = new CompanyParameterModel().Get();
            if (model.CompanyParameter == null)
                model.CompanyParameter = new CompanyParameterModel();
            return model;
        }

        public bool IsCompanyCodeExist(string code)
        {
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "Code", Value = code });
                return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Company/IsCompanyCodeExist", "GET", lstParameter);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Insert()
        {
            this.CurrencyID = this.CurrencyModel.ID;
            this.TermsDetails = ArtSolutions.MUN.Web.Resources.Company.Terms;
            this.CreatedUserID = this.ModifiedUserID = UserSession.Current.Id;
            this.AgreeTermsOnDate = this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.IsActive = true;
            this.Modules = string.Join(",", this.ModuleIDs);

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);

            new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Company/Registration", "POST", null, lstObjParameter);
            //mark company as configured
            UserSession.Current.IsCompanyConfigured = true;

            return this.CompanyID;
        }

        public int Update()
        {
            this.ModifiedDate = Common.CurrentDateTime;
            this.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            CompanyModel model = new RestSharpHandler().RestRequest<CompanyModel>(APISelector.Municipality, true, "api/Company/Update", "POST", null, lstObjParameter);
            return model.ID;
        }

        public int UpdateLogo(int companyLogoID)
        {
            CompanyModel model = GetCompany();
            model.CompanyParameter = new CompanyParameterModel().Get(); // Get Company Parameters
            model.LogoID = companyLogoID;
            return Update();
        }
        #endregion

        #region private methods 
        public CompanyModel GetCompanyModelFromSecurity()
        {
            return new RestSharpHandler().RestRequest<CompanyModel>(APISelector.Security, true, "api/Company/Get", "GET", null);
        }

        public List<CountryModel> GetCountries()
        {
            return new RestSharpHandler().RestRequest<List<CountryModel>>(APISelector.Municipality, true, "api/Country/Get", "GET", null);
        }

        private List<CurrencyModel> GetCurrency()
        {
            return new RestSharpHandler().RestRequest<List<CurrencyModel>>(APISelector.Municipality, true, "api/Account/CurrencyGet", "GET", null);
        }

        private List<ModuleModel> GetApplicationModules()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = Common.CurrentApplication });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = "" });

            return new RestSharpHandler().RestRequest<List<ModuleModel>>(APISelector.Security, true, "api/Module/Get", "GET", lstParameter);
        }
        #endregion
    }

    public class CompanyModulesModel
    {
        #region public properties       
        public System.Guid ModuleID { get; set; }
        public int IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion

        #region public methods    
        public List<CompanyModulesModel> Get(Guid? moduleID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "moduleID", Value = moduleID });
            return new RestSharpHandler().RestRequest<List<CompanyModulesModel>>(APISelector.Municipality, true, "api/Company/CompanyModulesGet", "GET", lstParameter);
        }
        #endregion
    }

    public class ReportCompanyModel
    {
        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string WorkPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string CountyCode { get; set; }
        public string StateName { get; set; }
        public string CountyName { get; set; }
        public Nullable<int> LogoID { get; set; }
        public string LogoURL
        {
            get
            {
                if (this.LogoID > 0)
                    return ArtSolutions.MUN.Web.Common.FileURL + this.LogoID;

                return Common.GetAbsoluteUrl("/Content/Images/no_image_available.png");
            }
        }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal? FromRange { get; set; }
        public decimal? ToRange { get; set; }
        public string Period { get; set; }
        public int NumberOfReportColumns { get; set; }
        public bool? ShowUntil { get; set; }
        #endregion

        #region public methods          
        public ReportCompanyModel Get()
        {
            ReportCompanyModel model = new RestSharpHandler().RestRequest<ReportCompanyModel>(APISelector.Municipality, true, "api/Company/CompanyAddressGet", "GET", null);
            return model;
        }

        public ReportCompanyModel Get(string Title, string SubTitle, Nullable<int> NumberOfReportColumns = 4, Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, Nullable<decimal> FromRange = null, Nullable<decimal> ToRange = null, bool? onlyMonthYearInDate = null, Nullable<DateTime> Period = null, bool? showUntil = null)
        {
            ReportCompanyModel model = new RestSharpHandler().RestRequest<ReportCompanyModel>(APISelector.Municipality, true, "api/Company/CompanyAddressGet", "GET", null);
            model.Title = Title;
            model.SubTitle = SubTitle;
            model.NumberOfReportColumns = NumberOfReportColumns ?? 0;
            //model.FromDate = FromDate.HasValue ? FromDate.Value.ToString("d") : "";
            //model.ToDate = ToDate.HasValue ? ToDate.Value.ToString("d") : "";
            model.FromDate = FromDate.HasValue ? (onlyMonthYearInDate == true ? FromDate.Value.ToString("MMMM yyyy") : FromDate.Value.ToString("d")) : "";
            model.ToDate = ToDate.HasValue ? (onlyMonthYearInDate == true ? ToDate.Value.ToString("MMMM yyyy") : ToDate.Value.ToString("d")) : "";
            model.FromRange = FromRange;
            model.ToRange = ToRange;
            model.Period = Period.HasValue ? (onlyMonthYearInDate == true ? Period.Value.ToString("MMMM yyyy") : Period.Value.ToString("d")) : "";
            model.ShowUntil = showUntil;
            return model;
        }
        #endregion
    }

    public class CompanyAddressModel
    {
        #region public properties
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryID { get; set; }
        public int StateProvinceID { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        #endregion
    }
}
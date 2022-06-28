using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private ObjectCache _cache = MemoryCache.Default;
        private object _lock = new object();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            UserSession _userSession = SetClaimData();
            if (UserSession.Current.Token == null || UserSession.Current.Token == Guid.Empty)
            {
                if (_userSession.Token != null && _userSession.Token != Guid.Empty)
                    SetSessionData(_userSession);
            }
            else if (_userSession.Token != UserSession.Current.Token || _userSession.CompanyID != UserSession.Current.CompanyID)
            {
                SetSessionData(_userSession);
            }

            if (UserSession.Current.Token == null || UserSession.Current.Token == Guid.Empty)
                HttpContext.GetOwinContext().Authentication.SignOut("Application");

            // calling CultureHelper class properties for setting    
            CultureInfo ci = new CultureInfo(UserSession.Current.Culture);
            ci.NumberFormat.CurrencyDecimalDigits = UserSession.Current.DecimalPoints;
            ci.NumberFormat.PercentDecimalDigits = UserSession.Current.DecimalPoints;
            ci.NumberFormat.CurrencyDecimalSeparator = ci.NumberFormat.NumberDecimalSeparator = ci.NumberFormat.PercentDecimalSeparator = ".";
            ci.NumberFormat.CurrencyGroupSeparator = ci.NumberFormat.NumberGroupSeparator = ci.NumberFormat.PercentGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(UserSession.Current.Language);

            //Application list
            if (TempData["CompanyApplications"] == null)
                TempData["CompanyApplications"] = GetCompanyApplication();
            //Fill company dropdownlist to display in top navigation bar
            if (TempData["CompanyList"] == null)
                TempData["CompanyList"] = GetCompanyForUser();

            //All Workflow
            if (TempData[Common.T_DocumentWorkflow] == null)
            {
                try
                {
                    TempData[Common.T_DocumentWorkflow] = new Areas.Cases.Models.CaseModel().DocumentWorkFlowGet();
                }
                catch (Exception) { }
            }

            //Is MuniTax Module Exist
            if (TempData["IsMuniTaxModuleExist"] == null)
                TempData["IsMuniTaxModuleExist"] = (new CompanyModulesModel().Get(new Guid("00000000-0009-0007-0000-000000000000")).Count == 0) ? false : true;

            ViewBag.Languages = GetCachedLanguageList();

            base.OnActionExecuting(filterContext);
        }

        private UserSession SetClaimData()
        {
            UserSession _userSession = null;
            if (User.Identity != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userData = identity.FindFirst(ClaimTypes.UserData);

                if (userData != null && (!string.IsNullOrEmpty(userData.Value)))
                {
                    _userSession = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<UserSession>(identity.FindFirst(ClaimTypes.UserData).Value);
                }
            }
            return _userSession;
        }
        private void SetSessionData(UserSession _userSession)
        {
            Common.SetCookie("LoginUserGUID", Convert.ToString(UserSession.Current.Id));
            UserSession.Current.Id = _userSession.Id;
            UserSession.Current.Token = _userSession.Token;
            UserSession.Current.CompanyID = _userSession.CompanyID;
            UserSession.Current.Username = _userSession.Username;
            UserSession.Current.Email = _userSession.Email;
            UserSession.Current.IsOwner = _userSession.IsOwner;
            UserSession.Current.ProfileImage = _userSession.ProfileImage;
            UserSession.Current.Language = _userSession.Language;
            //UserSession.Current.Culture = GetCurrencyLocalization();

            CurrencyModel currencyModel = GetCurrencyLocalization();
            UserSession.Current.Culture = currencyModel.CultureLocalization;
            UserSession.Current.CurrencyName = currencyModel.Description;

            //Check Company Exists Or New Registration
            CompanyModel model = new CompanyModel().GetCompany();
            UserSession.Current.CountryID = (model == null) ? 0 : model.CountryID;
            UserSession.Current.IsCompanyConfigured = (model == null) ? false : true;

            //Get registered company's decimal points
            if (UserSession.Current.IsCompanyConfigured)
            {
                CompanyParameterModel parameterModel = new CompanyParameterModel().Get();
                UserSession.Current.DecimalPoints = (parameterModel == null) ? 2 : parameterModel.Precision;
            }
        }
        //private string GetCurrencyLocalization()
        //{
        //    List<Areas.Companies.Models.CurrencyModel> data = new RestSharpHandler().RestRequest<List<Areas.Companies.Models.CurrencyModel>>(APISelector.Municipality, true, "api/Account/CompanyCurrencyGet", "GET", null);

        //    if (data.Count > 0)
        //        return data[0].CultureLocalization;
        //    return (UserSession.Current.Language);
        //}

        private CurrencyModel GetCurrencyLocalization()
        {
            List<CurrencyModel> data = new RestSharpHandler().RestRequest<List<CurrencyModel>>(APISelector.Municipality, true, "api/Account/CompanyCurrencyGet", "GET", null);

            CurrencyModel model = new CurrencyModel();
            if (data.Count > 0)
            {
                model.CultureLocalization = data[0].CultureLocalization;
                model.Description = data[0].Description;
                
            }
            else
            {
                model.CultureLocalization = UserSession.Current.Language;
                model.Description = "Dollar";               
            }
            return model;
        }

        public List<ApplicationModel> GetCompanyApplication()
        {
            string url = "api/application/GetCompanyApplication";
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "isFramework", Value = null });
            List<ApplicationModel> applicationList = new RestSharpHandler().RestRequest<List<ApplicationModel>>(APISelector.Security, true, url, "GET", lstParameter);
            return applicationList.Where(x => x.ID != Common.CurrentApplication).ToList();
        }

        private List<CompanyDropdownModel> GetCompanyForUser()
        {
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "applicationID", Value = Common.CurrentApplication });
                return new RestSharpHandler().RestRequest<List<CompanyDropdownModel>>(APISelector.Security, true, "api/Company/GetCompanyByToken", "GET", lstParameter);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return new List<CompanyDropdownModel>();
        }

        public List<LanguageModel> GetCachedLanguageList()
        {
            //var key = "GetCachedLanguageList";

            //// Try to get the object from the cache
            //var languageModels = _cache[key] as List<LanguageModel>;

            //// Check whether the value exists
            //if (languageModels == null)
            //{
            //    lock (_lock)
            //    {
            //        // Try to get the object from the cache again
            //        languageModels = _cache[key] as List<LanguageModel>;

            //        // Double-check that another thread did 
            //        // not call the DB already and load the cache
            //        if (languageModels == null)
            //        {
            //            // Get the list from the DB
            //            languageModels = new LanguageList().Get();

            //            // Add the list to the cache
            //            _cache.Set(key, languageModels, DateTimeOffset.Now.AddMinutes(5));
            //        }
            //    }
            //}
            //// Return the cached list
            //return languageModels;
            //Above code commented as we need language list by company. So caching can create issue
            return new LanguageList().Get();
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetActionResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}
using ArtSolutions.MUN.Web.Areas.ChatAndBot.Controllers;
using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Filters.IsCompanyConfiguredAttribute]
        public ActionResult Index()
        {            
            return View(new CompanyModel().GetCompanyModelFromSecurity());
        }

        [Filters.IsCompanyConfiguredAttribute]
        public ActionResult Minor()
        {
            return View();
        }
        public ActionResult NotFoundError()
        {
            return View();
        }

        #region Logoff
        public ActionResult LogOff()
        {
            ChatController objChatController = new ChatController();
            Common.DestroyCookie("LoginUserGUID");
            bool isExist = objChatController.RemoteFileExists(Common.URLCustomerServicePath);
            if (isExist)
            {
                ChatUser objChatUser = new ChatUser();
                List<ChatUser> lstActiveUser = objChatUser.ActiveChatUserGet(UserSession.Current.CompanyID, UserSession.Current.Language, null, true, null);
                if (lstActiveUser.Count > 0)
                {
                    objChatUser = lstActiveUser.Where(x => x.ChatUserID == UserSession.Current.Id).FirstOrDefault();
                }

                if (objChatUser != null && objChatUser.ID > 0)
                {
                    objChatUser.ChatUserStatusUpdate(objChatUser.ID, false, UserSession.Current.CompanyID, UserSession.Current.Language);
                }
            }

            HttpContext.GetOwinContext().Authentication.SignOut("Application");
            Common.AbortSession();
            return RedirectPermanent(System.Configuration.ConfigurationManager.AppSettings["SecurityWebPath"] + "/Account/LogOff");
        }
        #endregion

        #region Last company accessed

        public ActionResult UpdateLastApplicationAccessed(string applicationID, string q)
        {
            q = HttpUtility.UrlDecode(q);
            Uri uriResult;
            bool result = Uri.TryCreate(q, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                //Clear TempDate for this application
                TempData["CompanyApplications"] = null;
                TempData["CompanyList"] = null;

                //Update User LastApplication and LastCompanyVisited
                UpdateLastApplicationAndCompanyAccessed(applicationID);

                //Redirect to appripriate application
                return Redirect(q);
            }
            TempData["WarningMsg"] = ArtSolutions.MUN.Web.Resources.Global.ApplicationURLInValidMessage;
            return RedirectToAction("Index", "Home");
        }

        private void UpdateLastApplicationAndCompanyAccessed(string applicationID)
        {
            string url1 = "api/User/UpdateLastApplication";
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "lastCompanyVisited", Value = UserSession.Current.CompanyID });
            lstParameter.Add(new NameValuePair { Name = "lastApplicationAccessed", Value = applicationID });
            lstParameter.Add(new NameValuePair { Name = "modifiedDate", Value = Common.CurrentDateTime });
            new RestSharpHandler().RestRequest<object>(APISelector.Security, true, url1, "GET", lstParameter);
        }

        #endregion

        #region Change Company

        [HttpPost]
        public ActionResult ChangeCompanyDetails(FormCollection frmCollection)
        {
            try
            {
                List<CompanyDropdownModel> companyList = (List<CompanyDropdownModel>)TempData["CompanyList"];
                CompanyDropdownModel model = companyList.Where(i => i.CompanyID == Convert.ToInt32(frmCollection["UserCompanyID"])).FirstOrDefault();

                if (model != null)
                {
                    Helpers.UserSession.Current.CompanyID = model.CompanyID;
                    Helpers.UserSession.Current.IsOwner = model.IsCompanyOwner;
                    Helpers.UserSession.Current.Language = model.Language ?? "en";
                    Helpers.UserSession.Current.Culture = model.Language ?? "en";
                    SetClaim();

                    //Update User LastApplication and LastCompanyVisited
                    UpdateLastApplicationAndCompanyAccessed(Common.CurrentApplication.ToString());

                    Helpers.UserSession.Current.CompanyID = 0;
                }
            }
            catch (Exception)
            {
                //ErrorMsg = ex.Message;
            }
            //return Redirect(Request.UrlReferrer.PathAndQuery);            
            return RedirectToAction("Index", "Home", new { area = "" });
        }        
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        private void SetClaim()
        {
            Authentication.SignOut("Application");
            var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.UserData, new JavaScriptSerializer().Serialize(Helpers.UserSession.Current))
                        },
                       "Application",
                       ClaimTypes.UserData, ClaimTypes.Role);
            Authentication.SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, identity);

            TempData["CompanyApplications"] = null;
            TempData["CompanyList"] = null;
            TempData["IsMuniTaxModuleExist"] = null;
        }

        #endregion

        #region Change Language
        public ActionResult ChangeLanguage(string language)
        {
            UserSession.Current.Language = language;
            //UserSession.Current.Culture = language;

            new UserProfile().ChangeLanguage(language);

            SetClaim();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        #endregion

        #region Global Search
        public ActionResult ChangeSearchDetails(FormCollection frmCollection)
        {
            try
            {
                int SearchTypeID = Convert.ToInt32(frmCollection["SearchTypeID"]);
                string Search = frmCollection["top-search"];
                TempData["Search"] = Search;
                TempData["SearchTypeID"] = SearchTypeID;
                if (SearchTypeID == (int)Common.SearchTypeEnum.Account)
                {
                    return RedirectToAction("List", "Account", new { area = "Accounts" });
                }
                else if (SearchTypeID == (int)Common.SearchTypeEnum.AccountService)
                {
                    return RedirectToAction("List", "AccountService", new { area = "Accounts" });
                }
                else if (SearchTypeID == (int)Common.SearchTypeEnum.AccountProperty)
                {
                    return RedirectToAction("List", "AccountProperty", new { area = "Accounts" });
                }
            }
            catch (Exception)
            {
                //ErrorMsg = ex.Message;
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        #endregion
    }
}
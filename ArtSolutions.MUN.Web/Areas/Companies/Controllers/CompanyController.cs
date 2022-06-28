using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Companies.Controllers
{
    public class CompanyController : BaseController
    {
        #region Company Registration
        public ActionResult Registration()
        {
            CompanyModel model = new CompanyModel();
            try
            {
                model = model.GetCompanyFromSecurity();
                //If company already created in Municipality, redirect to Dashboard
                if (model == null) return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Companies/Views/Company/Registration.cshtml", model);
        }

        [HttpPost]
        public ActionResult Registration(CompanyModel model)
        {
            bool status = false;
            string msg = string.Empty;

            try
            {
                int result = model.Insert();
                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Company.CompanySucessMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                throw new HttpException(500, ex.Message);
            }
            string str = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Companies/Views/Company/Registration.cshtml", model);
            return Json(new { status = status, message = msg, data = str }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveCompanyImage(int companyLogoID)
        {
            try
            {
                if (companyLogoID > 0)
                {
                    new CompanyModel().UpdateLogo(companyLogoID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Company Edit
        [HttpGet]
        public ActionResult Edit()
        {
            return View("~/Areas/Companies/Views/Company/Edit.cshtml", new CompanyModel().Edit());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyModel model)
        {
            bool status = false;
            try
            {
                int result = model.Update();

                if (result > 0)
                {
                    status = true;
                    UserSession.Current.DecimalPoints = model.CompanyParameter.Precision;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Company.CompanySucessMessage;
                }
                return Json(new { status = status, redirectURL = Url.Action("../../") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                status = false;
                return Json(new { status = status, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region AJAX calls
        [HttpPost]
        public JsonResult IsCompanyCodeExist(string Code)
        {
            try
            {
                return Json(!new CompanyModel().IsCompanyCodeExist(Code), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetStates(string countryId)
        {
            List<SelectListItem> StateList = new List<SelectListItem>();
            if (countryId == "" || countryId == null)
                countryId = "0";
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CountryID", Value = countryId });
            var lstStates = new RestSharpHandler().RestRequest<List<StateModel>>(APISelector.Municipality, true, "api/Country/StateGetByCountryID", "GET", lstParameter);
            if (lstStates != null && lstStates.Count > 0)
                StateList = lstStates.Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Unauthorize page
        public ActionResult UnAuthorizeForRegistration()
        {
            return View("~/Areas/Companies/Views/Company/UnAuthorizeForRegistration.cshtml");
        }
        #endregion
    }
}
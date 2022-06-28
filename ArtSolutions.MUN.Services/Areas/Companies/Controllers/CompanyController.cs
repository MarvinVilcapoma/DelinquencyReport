using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.CompanyModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Companies.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";

        #region Company
        [Route("Registration")]
        [HttpPost]
        public IHttpActionResult Registration(CompanyModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.ID = companyId;
                var companyModel = new Company().Registration(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.ID.ToString(), model.Name);
                return Ok(companyModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.Registration", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Company().Get(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.Get", language, ex));
            }
        }

        [Route("GetWithoutAuthentication")]
        [HttpGet]
        public IHttpActionResult GetWithoutAuthentication()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                var model = new Company().Get(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.Get", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(CompanyModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.ID = companyId;
                var companyModel = new Company().Update(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), model.Name);
                return Ok(companyModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.Update", language, ex));
            }
        }

        [Route("GetCompanyParamter")]
        [HttpGet]
        public IHttpActionResult GetCompanyParamter()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new CompanyParameterModel().Get(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.CompanyParameterModel.Get", language, ex));
            }
        }

        [Route("GetCompanyParamterWithoutAuthentication")]
        [HttpGet]
        public IHttpActionResult GetCompanyParamterWithoutAuthentication()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                var model = new CompanyParameterModel().Get(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.CompanyParameterModel.Get", language, ex));
            }
        }

        [Route("IsCompanyCodeExist")]
        [HttpGet]
        public IHttpActionResult IsCompanyCodeExist(string Code)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                int? ID = new Company().IsCompanyCodeExist(companyId, Code);
                return Ok(ID > 0 ? true : false);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.IsCompanyCodeExist", language, ex));
            }
        }

        [Route("GetLanguagelist")]
        [HttpGet]
        public IHttpActionResult GetLanguagelist()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<MUNLanguageGet_Result> model = new Language().Get(companyId);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Language.Get", language, ex));
            }
        }
        #endregion

        #region Company Modules
        [Route("CompanyModulesGet")]
        [HttpGet]
        public IHttpActionResult CompanyModulesGet(Guid? moduleID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Company().CompanyModulesGet(companyId, moduleID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.CompanyModulesGet", language, ex));
            }
        }
        #endregion

        #region Company Address
        [Route("CompanyAddressGet")]
        [HttpGet]
        public IHttpActionResult CompanyAddressGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Company().CompanyAddressGet(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.CompanyAddressGet", language, ex));
            }
        }
        #endregion
    }
}

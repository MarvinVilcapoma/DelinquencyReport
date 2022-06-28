using ArtSolutions.MUN.Core.ServiceModels;
using ArtSolutions.MUN.Services.Helpers;
using ArtSolutions.MUN.Services.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Services.Controllers
{
    [RoutePrefix("api/CollectionTemplate")]
    public class CollectionTemplateController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0005-000000000000";

        #region Service CollectionTemplate

        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(string FilterText, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new ServiceCollectionTemplate().GetWithPaging(CompanyID,FilterText,language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.GetWithPaging", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(ServiceCollectionTemplateModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Locale = language;
                model.CompanyID = companyId;
                int templateID = new ServiceCollectionTemplate().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), templateID.ToString(), string.Empty);
                return Ok(templateID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(ServiceCollectionTemplateModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.Locale = language;
                model.CompanyID = companyId;
                int templateID = new ServiceCollectionTemplate().Update(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), templateID.ToString(), string.Empty);
                return Ok(templateID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.Update", language, ex));
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public IHttpActionResult UpdateStatus(int id, bool isActive)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                int templateID = new ServiceCollectionTemplate().UpdateStatus(id, companyId, isActive);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), templateID.ToString(), string.Empty);
                return Ok(templateID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.UpdateStatus", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? ID, bool? IsActive,string FilterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionTemplate().Get(ID, companyId,language, IsActive, FilterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.Get", language, ex));
            }
        }

        #endregion

        #region "ServiceCollectionTemplateDetail"

        [Route("DetailGet")]
        [HttpGet]
        public IHttpActionResult DetailGet(int? ID,int CollectionTemplateID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionTemplate().DetailGet(ID,companyId,CollectionTemplateID,language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionTemplate.DetailGet", language, ex));
            }
        }

        #endregion

        #region Finance API Call

        [Route("ClassGrantGet")]
        [HttpGet]
        public IHttpActionResult ClassGrantGet(int? ID, string FilterText, bool? IsActive, bool? IsGrant)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new FinanceAPIModel().ClassGrantGet(token, companyId, language, ID, FilterText, IsActive, IsGrant);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Models.FinanceAPIModel.ClassGrantGet", language, ex));
            }
        }

        [Route("FinanceAccountGet")]
        [HttpGet]
        public IHttpActionResult FinanceAccountGet(int GrantID, string AccountType)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new FinanceAPIModel().AccountGet(token, companyId, language, GrantID, AccountType);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Models.FinanceAPIModel.AccountGet", language, ex));
            }
        }

        #endregion
    }
}
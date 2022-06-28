using ArtSolutions.MUN.Core.ServiceModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Services.Controllers
{
    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {
        const string _featureID = "00000000-0009-0002-0001-000000000000";

        #region Service  

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? serviceTypeId, int? groupId, int? id, bool isGetByEffectiveDate, string filingTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().Get(companyId, language, serviceTypeId, groupId, id, isGetByEffectiveDate, filingTypeID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Get", language, ex));
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

                var model = new Service().Get(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Get", language, ex));
            }
        }

        [Route("GetForFACTU")]
        [HttpGet]
        public IHttpActionResult GetForFACTU()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().GetForFACTU(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.GetForFACTU", language, ex));
            }
        }

        [Route("GetForCobros")]
        [HttpGet]
        public IHttpActionResult GetForCobros()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().GetForCobros(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.GetForCobros", language, ex));
            }
        }

        [Route("GetForNoFiling")]
        [HttpGet]
        public IHttpActionResult GetForNoFiling()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().Get(companyId, language, null, null, null, false, null);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Get", language, ex));
            }
        }

        [Route("GetForFixDates")]
        [HttpGet]
        public IHttpActionResult GetForFixDates(int? serviceTypeId, int? groupId, int? id, bool isGetByEffectiveDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().Get(companyId, language, serviceTypeId, groupId, id, isGetByEffectiveDate, "3");
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Get", language, ex));
            }
        }

        [Route("GetForFlexibleDatesAndFixDates")]
        [HttpGet]
        public IHttpActionResult GetForFlexibleDatesAndFixDates(int? serviceTypeId, int? groupId, int? id, bool isGetByEffectiveDate, int AccountTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().Get(companyId, language, serviceTypeId, groupId, id, isGetByEffectiveDate, "2,3,4,5", AccountTypeID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Get", language, ex));
            }
        }

        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult GetByPaging(int? serviceID, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder, Nullable<int> groupID, Nullable<int> serviceTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().GetByPaging(serviceID, companyId, language, filterText, pageIndex, pageSize, sortColumn, sortOrder, groupID, serviceTypeID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.GetByPaging", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(ServiceModel model)
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
                int serviceID = new Service().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), serviceID.ToString(), string.Empty);
                return Ok(serviceID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(ServiceModel model)
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
                int serviceID = new Service().Update(model);

                //if (serviceID > 0)
                //{
                //    SalesItemInsertUpdate(false, true, token, language, companyId, model, serviceID);
                //}
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), serviceID.ToString(), string.Empty);
                return Ok(serviceID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.Update", language, ex));
            }
        }

        [Route("GetByCollectionField")]
        [HttpGet]
        public IHttpActionResult GetByCollectionField(int CollectionFieldID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().GetByCollectionField(companyId, CollectionFieldID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.GetByCollectionfield", language, ex));
            }
        }

        #endregion

        #region Service Collection Rule

        [Route("ServiceCollectionRuleGet")]
        [HttpGet]
        public IHttpActionResult ServiceCollectionRuleGet(int serviceId, int? id, bool? isDeleted)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().Get(serviceId, companyId, language, id, isDeleted);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionRule.Get", language, ex));
            }
        }

        [Route("CollectionRuleGet")]
        [HttpGet]
        public IHttpActionResult CollectionRuleGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().CollectionRuleGet(language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.CollectionRuleGet", language, ex));
            }
        }

        [Route("CollectionTypeGet")]
        [HttpGet]
        public IHttpActionResult CollectionTypeGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().CollectionTypeGet(language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.CollectionTypeGet", language, ex));
            }
        }

        [Route("CollectionFieldGet")]
        [HttpGet]
        public IHttpActionResult CollectionFieldGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().CollectionFieldGet(language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.CollectionFieldGet", language, ex));
            }
        }

        [Route("FrequencyGet")]
        [HttpGet]
        public IHttpActionResult FrequencyGet(bool? IsServiceFrequency)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().FrequencyGet(language, IsServiceFrequency);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.FrequencyGet", language, ex));
            }
        }

        [Route("IsServiceExist")]
        [HttpGet]
        public IHttpActionResult IsServiceExist(string serviceName, int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().IsServiceExist(serviceName, companyId, language, id);
                return Ok(model > 0 ? model : 0);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.ServiceModels.Service.IsServiceExist", language, ex));
            }
        }

        [Route("IsServiceCollectionRuleExist")]
        [HttpGet]
        public IHttpActionResult IsServiceCollectionRuleExist(string collectionRuleName, int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().IsServiceCollectionRuleExist(collectionRuleName, language, id);
                return Ok(model > 0 ? model : 0);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.ServiceModels.ServiceCollectionRule.IsServiceCollectionRuleExist", language, ex));
            }
        }

        #endregion

        #region  ServiceCollectionRuleDetail

        [Route("ServiceCollectionRuleDetailGet")]
        [HttpGet]
        public IHttpActionResult ServiceCollectionRuleDetailGet(int serviceId, int? id, int? serviceCollectionRuleID, bool? isDeleted)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCollectionRule().DetailGet(serviceId, companyId, language, id, serviceCollectionRuleID, isDeleted);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCollectionRule.DetailGet", language, ex));
            }
        }

        #endregion

        #region ServiceCalculationDateGet

        [Route("ServiceCalculationDateGet")]
        [HttpGet]
        public IHttpActionResult ServiceCalculationDateGet(int? id, int? fiscalYearID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceCalculationDate().Get(id, fiscalYearID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceCalculationDate.Get", language, ex));
            }
        }

        #endregion

        #region Service Print Template

        [Route("PrintTemplateGet")]
        [HttpGet]
        public IHttpActionResult PrintTemplateGet(int? ID, int? DocumentTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServicePrintTemplate().Get(companyId, ID, DocumentTypeID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServicePrintTemplate.Get", language, ex));
            }
        }

        #endregion

        #region Service Exception

        [Route("ExceptionGet")]
        [HttpGet]
        public IHttpActionResult ExceptionGet(int? ID, int? ServiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new ServiceExceptionModel().Get(ID, ServiceID, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.ServiceExceptionModel.Get", language, ex));
            }
        }

        #endregion 

        [Route("GetGrantList")]
        [HttpGet]
        public IHttpActionResult GetGrantList()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Service().GetGrantList(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.ServiceModels.Service.GetGrantList", language, ex));
            }
        }
    }
}

using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Core.ServiceModels;
using ArtSolutions.MUN.Services.Areas.Accounts.Models;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Controllers
{
    [RoutePrefix("api/Finance")]
    public class FinanceController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";

        [Route("GrantGetBySponsor")]
        [HttpGet]
        public IHttpActionResult GrantGetBySponsor(int AccountId, DateTime date)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "AccountId", Value = AccountId });
                lstParameter.Add(new NameValuePair { Name = "date", Value = date.ToString("d", System.Globalization.CultureInfo.InvariantCulture) });

                var model = new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Finance, "api/ClassGrant/GrantGetBySponsor", "GET", lstParameter, null, true, token, language, companyId).ToList();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.GrantGetBySponsor", language, ex));
            }
        }

        [Route("GetSalesCashier")]
        [HttpGet]
        public IHttpActionResult GetSalesCashier(int? id, string filterText, bool? isActive, int? moduleID, int? bankAccountID, int? grantID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "id", Value = id });
                lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
                lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
                lstParameter.Add(new NameValuePair { Name = "moduleID", Value = moduleID });
                lstParameter.Add(new NameValuePair { Name = "bankAccountID", Value = bankAccountID });
                lstParameter.Add(new NameValuePair { Name = "grantID", Value = grantID });
                List<FinClassGrantModel> model = new List<FinClassGrantModel>();
                model = new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Finance, "api/SalesCashier/GetSalesCashier", "GET", lstParameter, null, true, token, language, companyId).ToList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.GetSalesCashier", language, ex));
            }
        }

        [Route("GetSalesCashierDetails")]
        [HttpGet]
        public IHttpActionResult GetSalesCashierDetails(int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "id", Value = id });                
                FinSalesCashierDetailsModel model = new FinSalesCashierDetailsModel();
                model = new RestSharpHandler().RestRequest<FinSalesCashierDetailsModel>(APISelector.Finance, "api/SalesCashier/GetSalesCashierDetails", "GET", lstParameter, null, true, token, language, companyId);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.GetSalesCashierDetails", language, ex));
            }
        }

        [Route("GetAllBankAccount")]
        [HttpGet]
        public IHttpActionResult GetAllBankAccount(int? Id, string filterText, bool? isActive = null, int? cashierID = null)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                if (Id == 0)
                    Id = null;
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
                lstParameter.Add(new NameValuePair { Name = "id", Value = Id });
                lstParameter.Add(new NameValuePair { Name = "IsActive", Value = isActive });
                lstParameter.Add(new NameValuePair { Name = "cashierID", Value = cashierID });
                List<BankAccountModel> model = new List<BankAccountModel>();
                model = new RestSharpHandler().RestRequest<List<BankAccountModel>>(APISelector.Finance, "api/BankAccount/Get", "GET", lstParameter, null, true, token, language, companyId).ToList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.GetAllBankAccount", language, ex));
            }
        }

        [Route("JournalMunicipalityInsert")]
        [HttpPost]
        public IHttpActionResult JournalMunicipalityInsert(FINJournalModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<object> lstObjParameter = new List<object>();
                lstObjParameter.Add(model);
                int FINJournalID = new RestSharpHandler().RestRequest<int>(APISelector.Finance, "api/Journal/JournalMunicipalityInsert", "POST", null, lstObjParameter, true, token, language, companyId);
                return Ok(FINJournalID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.JournalMunicipalityInsert", language, ex));
            }
        }

        [Route("JournalMunicipalityVoid")]
        [HttpGet]
        public IHttpActionResult JournalMunicipalityVoid(int FinJournalID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "FinJournalID", Value = FinJournalID });               
                bool Result = new RestSharpHandler().RestRequest<bool>(APISelector.Finance, "api/Journal/JournalMunicipalityVoid", "GET", lstParameter, null , true, token, language, companyId);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.JournalMunicipalityVoid", language, ex));
            }
        }

        [Route("CheckTransactionIsMatched")]
        [HttpGet]
        public IHttpActionResult CheckTransactionIsMatched(int referenceID, int documentTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "referenceID", Value = referenceID });
                lstParameter.Add(new NameValuePair { Name = "documentTypeID", Value = documentTypeID });
                bool Retval = new RestSharpHandler().RestRequest<bool>(APISelector.Finance, "api/Banking/CheckTransactionIsMatched", "GET", lstParameter, null, true, token, language, companyId);
                return Ok(Retval);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Controllers.FinanceController.CheckTransactionIsMatched", language, ex));
            }
        }

    }
}
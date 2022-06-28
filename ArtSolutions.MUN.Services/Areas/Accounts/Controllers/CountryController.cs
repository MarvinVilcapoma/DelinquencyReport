using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/Country")]
    public class CountryController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";

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

                //Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().CountryGet(language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.CountryGet", language, ex));
            }
        }
        [Route("StateGetByCountryID")]
        [HttpGet]
        public IHttpActionResult StateGetByCountryID(int countryID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().StateGetByCountryID(countryID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.StateGetByCountryID", language, ex));
            }
        }

        [Route("CityGetByCountryIdAndStateId")]
        [HttpGet]
        public IHttpActionResult CityGetByCountryIdAndStateId(int countryID, int StateID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().CityGetByCountryIdAndStateId(countryID, StateID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.CityGetByCountryIdAndStateId", language, ex));
            }
        }

        [Route("TownGetByCountryIdStateIdAndCityId")]
        [HttpGet]
        public IHttpActionResult TownGetByCountryIdStateIdAndCityId(int countryID, int StateID, int CityID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().TownGetByCountryIdStateIdAndCityId(countryID, StateID, CityID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.TownGetByCountryIdStateIdAndCityId", language, ex));
            }
        }
    }
}

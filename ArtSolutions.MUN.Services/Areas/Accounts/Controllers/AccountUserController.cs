using ArtSolutions.MUN.Core.AccountUserModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/AccountUser")]
    public class AccountUserController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? accountID, int? accountTypeID, Guid userID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountUser().Get(companyId, accountID, accountTypeID, userID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountUserModels.AccountUser.Get", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(AccountUserModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.CompanyID = companyId;
                var Model = new AccountUser().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountUserModels.AccountUser.Insert", language, ex));
            }
        }
    }
}

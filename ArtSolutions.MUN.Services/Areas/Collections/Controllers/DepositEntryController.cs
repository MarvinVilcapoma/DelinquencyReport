using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/DepositEntry")]
    public class DepositEntryController : ApiController
    {        
        const string _featureID = "00000000-0009-0008-0004-000000000000";

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(DepositEntryModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                int depositId = new DepositEntry().Insert(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), depositId.ToString(), "MUNCOLDepositEntry.ID");
                return Ok(depositId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.DepositEntry.Insert", language, ex));
            }
        }

        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(int? AccountID, int? ID, string FilterText, bool? IsVoid, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new DepositEntry().GetWithPaging(CompanyID, AccountID, ID, FilterText, IsVoid, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.DepositEntry.GetWithPaging", language, ex));
            }
        }

        [Route("GetPrint")]
        [HttpGet]
        public IHttpActionResult GetPrint(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new DepositEntry().GetPrint(ID, CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.DepositEntry.GetPrint", language, ex));
            }
        }

        [Route("Void")]
        [HttpPost]
        public IHttpActionResult Void(DepositEntryModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new DepositEntry().Void(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.DepositEntry.Void", language, ex));
            }
        }
    }
}

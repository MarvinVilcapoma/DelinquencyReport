using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/ClosingEntry")]
    public class ClosingEntryController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0003-000000000000";

        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(int? AccountID, int? ID, string FilterText, bool? IsDeposited, bool? IsVoid, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new ClosingEntry().GetWithPaging(CompanyID, AccountID, ID, FilterText, IsDeposited, IsVoid, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.ClosingEntry.GetWithPaging", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? AccountID, int? ID, bool? OnlyPostedClosingEntries)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new ClosingEntry().Get(CompanyID, AccountID, ID, language, OnlyPostedClosingEntries);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.ClosingEntry.Get", language, ex));
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

                var result = new ClosingEntry().GetPrint(ID, CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.ClosingEntry.GetPrint", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(ClosingEntryModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                int closingId = new ClosingEntry().Insert(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), closingId.ToString(), "MUNCOLClosingEntry.ID");
                return Ok(closingId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.ClosingEntry.Insert", language, ex));
            }
        }
        [Route("Void")]
        [HttpPost]
        public IHttpActionResult Void(ClosingEntryModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new ClosingEntry().Void(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.ClosingEntry.Void", language, ex));
            }
        }
    }
}

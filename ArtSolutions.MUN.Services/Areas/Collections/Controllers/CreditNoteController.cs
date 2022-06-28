using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/CreditNote")]
    public class CreditNoteController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0002-000000000000";

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(CreditNoteModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                int ID = new CreditNote().Insert(model, language);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), ID.ToString(), "ID");
                return Ok(ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.CreditNote.Insert", language, ex));
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

                var result = new CreditNote().GetWithPaging(CompanyID, AccountID, ID, FilterText, IsVoid, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.CreditNote.GetWithPaging", language, ex));
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

                var result = new CreditNote().GetPrint(ID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.CreditNote.GetPrint", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(CreditNoteModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.Edit);

                int result = new CreditNote().Update(model);
                Common.InsertActivity(token, CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "ID");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.CreditNote.Update", language, ex));
            }
        }
    }
}

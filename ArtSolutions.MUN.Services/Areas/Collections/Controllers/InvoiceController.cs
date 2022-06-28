using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/Invoice")]
    public class InvoiceController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0001-000000000000";

        #region Invoice
        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(int? AccountID, int? ID, string FilterText, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Invoice().GetWithPaging(CompanyID, AccountID, ID, FilterText, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.GetWithPaging", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? AccountID, int? ID, bool? OnlyPendingInvoice, bool? OnlyPostedInvoice)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Invoice().Get(CompanyID, AccountID, ID, OnlyPendingInvoice, OnlyPostedInvoice, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.Get", language, ex));
            }
        }

        [Route("GetAsObject")]
        [HttpGet]
        public IHttpActionResult GetAsObject(int? AccountID, int? ID, string FilterText, bool? IsVoid, bool? IsPost)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Invoice().GetAsObject(CompanyID, AccountID, ID, FilterText, IsVoid, IsPost, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.GetAsObject", language, ex));
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

                var result = new Invoice().GetPrint(ID, CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.GetPrint", language, ex));
            }
        }

        [Route("ManualInsert")]
        [HttpPost]
        public IHttpActionResult ManualInsert(InvoiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.Language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                var result = new Invoice().ManualInsert(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), result.Number, "MUNCOLInvoice.Number");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.ManualInsert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(InvoiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.Language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                int result = new Invoice().Update(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), result.ToString(), "MUNCOLInvoice.ID");
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.Update", language, ex));
            }
        }

        [Route("Void")]
        [HttpPost]
        public IHttpActionResult Void(InvoiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Invoice().Void(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.Void", language, ex));
            }
        }
        #endregion

        #region Invoice Detail

        [Route("DetailGet")]
        [HttpGet]
        public IHttpActionResult DetailGet(int? ID, int? InvoiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new Invoice().DetailGet(CompanyID, ID, InvoiceID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.Invoice.DetailGet", language, ex));
            }
        }

        #endregion
    }
}

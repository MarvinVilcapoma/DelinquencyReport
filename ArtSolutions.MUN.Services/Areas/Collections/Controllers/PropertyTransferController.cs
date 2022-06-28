using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using ArtSolutions.MUN.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/PropertyTransfer")]
    public class PropertyTransferController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0002-000000000000";

        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(string FilterText, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new PropertyTransfer().GetWithPaging(CompanyID,FilterText, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PropertyTransfer.GetWithPaging", language, ex));
            }
        }
        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First().ToString();

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new PropertyTransfer().GetPropertyTransfer(ID,CompanyID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PropertyTransfer.GetWithPaging", language, ex));
            }
        }

        #region Property Transfer Insert

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(PropertyTransferModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                model.language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = model.language;
                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                int transferID = new PropertyTransfer().Insert(model);
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), transferID.ToString(), "ID");
                return Ok(transferID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PropertyTransfer.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(PropertyTransferModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.Edit);

                int result = new PropertyTransfer().Update(model);
                Common.InsertActivity(token, CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.TransferID.ToString(), "ID");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Update", language, ex));
            }
        }

        #endregion
    }
}

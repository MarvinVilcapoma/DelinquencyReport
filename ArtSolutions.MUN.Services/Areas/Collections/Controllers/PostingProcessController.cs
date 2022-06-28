using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Collections.Controllers
{
    [RoutePrefix("api/PostingProcess")]
    public class PostingProcessController : ApiController
    {
        const string _featureID = "00000000-0009-0008-0002-000000000000";
        
        #region Posting Process

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(PostingProcessJournal model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.New);

                int ID = new PostingProcess().Insert(CompanyID, model.JournalID, model.DocuemntTypeDetail, model.AsOfDate
                    , model.WorkflowID, model.WorkflowVerionID, model.WorkflowStatusID
                    , model.CreatedDate, model.CreatedUserID,model.PaymentType);
                Common.InsertActivity(token, CompanyID, _featureID.ToString(), Actions.New.ToString(), ID.ToString(), "ID");
                return Ok(ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Insert", language, ex));
            }
        }
        
        [Route("GetWithPaging")]
        [HttpGet]
        public IHttpActionResult GetWithPaging(int? ID, string FilterText, bool? IsVoid, string DocumentTypeID, int PageIndex, int PageSize, string SortColumn, string SortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new PostingProcess().GetWithPaging(CompanyID, ID, FilterText, IsVoid, DocumentTypeID, language, PageIndex, PageSize, SortColumn, SortOrder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.GetWithPaging", language, ex));
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
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new PostingProcess().Get(CompanyID, ID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Get", language, ex));
            }
        }

        [Route("GetWithGroupSum")]
        [HttpGet]
        public IHttpActionResult GetWithGroupSum(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);

                var result = new PostingProcess().GetWithGroupSum(CompanyID, ID, language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Get", language, ex));
            }
        }

        [Route("LatInsertedGet")]
        [HttpGet]
        public IHttpActionResult LatInsertedGet(int PaymentType)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.View);
                var result = new PostingProcess().LastInsertedGet(CompanyID, PaymentType,language );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Get", language, ex));
            }
        }

        [Route("JournalDetailForPostingProcess")]
        [HttpGet]
        public IHttpActionResult JournalDetailForPostingProcess(DateTime StartPeriodDate, DateTime EndPeriodDate, string DocumentTypeID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new PostingProcess().JournalDetailForPostingProcess(companyId, StartPeriodDate, EndPeriodDate, DocumentTypeID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.JournalDetailForPostingProcess", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(PostingProcessModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.Edit);

                int result = new PostingProcess().Update(model);
                Common.InsertActivity(token, CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "ID");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.Update", language, ex));
            }
        }

        [Route("UpdateFINJournalID")]
        [HttpPost]
        public IHttpActionResult UpdateFINJournalID(JournalSyns model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.Edit);

                bool result = new PostingProcess().UpdateFINJournalID(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.UpdateFINJournalID", language, ex));
            }
        }

        [Route("NewPostingProcessBulkJournalInsert")]
        [HttpPost]
        public IHttpActionResult NewPostingProcessBulkJournalInsert(NewPostingProcessModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, CompanyID, Guid.Parse(_featureID), Actions.Edit);
                var result = new PostingProcess().InsertJournalForPayment(CompanyID,model.PaymentReceiptType,model.AsOfDate,model.CreatedDate,model.CreatedUserID,language);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.Collections.PostingProcess.UpdateFINJournalID", language, ex));
            }
        }

        #endregion
    }
}

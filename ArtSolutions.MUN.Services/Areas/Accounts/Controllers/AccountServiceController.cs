using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Services.Helpers;
using ArtSolutions.MUN.Services.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;


namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/AccountService")]
    public class AccountServiceController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0002-000000000000";
        const string _correctionfeatureID = "00000000-0009-0004-0007-000000000000";

        #region Account Service

        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult Get(string filterText, string serviceTypeIDs, int? accountId, int? accountPropertyID, string serviceIDs, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().Get(companyId, language, filterText, serviceTypeIDs, accountId, accountPropertyID, serviceIDs, pageIndex, pageSize, sortColumn, sortOrder);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Get", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? accountServiceId, string filterText, int? accountId, int? fiscalYearID, bool? isLock = null)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().Get(companyId, accountServiceId, language, filterText, accountId, fiscalYearID, isLock);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Get", language, ex));
            }
        }

        [Route("GetAllAccountService")]
        [HttpGet]
        public IHttpActionResult GetAllAccountService(int accountId, int accountPropertyID, int transferTypeID, int transferID = 0)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().GetAllAccountService(companyId, language, accountId, accountPropertyID, transferTypeID, transferID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Get", language, ex));
            }
        }

        [Route("GetAccountServiceForTimbre")]
        [HttpGet]
        public IHttpActionResult GetAccountServiceForTimbre(int fiscalYearID, int accountID, int serviceID, int? accountPropertyID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().GetAccountServiceForTimbre(companyId, language, accountID, fiscalYearID, serviceID, accountPropertyID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetAllAccountServiceForTimbre", language, ex));
            }
        }

        [Route("GetNotFiled")]
        [HttpGet]
        public IHttpActionResult GetNotFiled(int accountId, int fiscalYearID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().GetNotFiled(companyId, accountId, fiscalYearID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetNotFiled", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(AccountServiceStatusModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                var accountModel = new AccountService().Update(model);

                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Update", language, ex));
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IHttpActionResult Delete(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Delete);

                new AccountService().Delete(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Update", language, ex));
            }
        }

        [Route("DeleteNote")]
        [HttpPost]
        public IHttpActionResult DeleteNote(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Delete);

                new AccountService().DeleteNote(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteNote", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(AccountServiceModel model)
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
                var Model = new AccountService().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                string strmsg = ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Insert", language, ex);

                if (ex.InnerException.Message == "50276")
                    strmsg = strmsg.Replace("{}", model.CustomField1);
                if (ex.InnerException.Message == "50277")
                    strmsg = strmsg.Replace("{}", model.CustomField2);
                if (ex.InnerException.Message == "50278")
                    strmsg = strmsg.Replace("{}", model.CustomField3);
                if (ex.InnerException.Message == "50384")
                    strmsg = strmsg.Replace("{}", model.CustomField4);
                if (ex.InnerException.Message == "50387")
                    strmsg = strmsg.Replace("{}", model.CustomField5);
                return BadRequest(strmsg);
            }
        }

        [Route("GetAsObject")]
        [HttpGet]
        public IHttpActionResult GetAsObject(int? accountServiceId, string filterText, int? licenceGroupId, int? ivuServiceId, int? accountId, int? fiscalYearID, int? id, bool? isPost, bool showAll = false, bool? isLock = null)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().GetAsObject(companyId, accountServiceId, language, filterText, licenceGroupId, ivuServiceId, accountId, fiscalYearID, showAll, isLock, id, isPost);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetAsObject", language, ex));
            }
        }
        #endregion

        #region Notes

        [Route("NoteGet")]
        [HttpGet]
        public IHttpActionResult NoteGet(int AccountServiceID, int? ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().NoteGet(companyId, language, AccountServiceID, ID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.NoteGet", language, ex));
            }
        }

        [Route("NoteInsert")]
        [HttpPost]
        public IHttpActionResult NoteInsert(AccountServiceNoteModel model)
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
                int id = new AccountService().NoteInsert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), id.ToString(), "MUNSERAccountServiceNote.ID");
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.NoteInsert", language, ex));
            }
        }

        #endregion

        #region CustomField         
        [Route("UpdateForCustomField")]
        [HttpPost]
        public IHttpActionResult UpdateForCustomField(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.CompanyID = companyId;
                var accountModel = new AccountService().UpdateForCustomField(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                string strmsg = ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.UpdateForCustomField", language, ex);

                if (ex.InnerException.Message == "50276")
                    strmsg = strmsg.Replace("{}", model.CustomField1NewValue);
                if (ex.InnerException.Message == "50277")
                    strmsg = strmsg.Replace("{}", model.CustomField2NewValue);
                if (ex.InnerException.Message == "50278")
                    strmsg = strmsg.Replace("{}", model.CustomField3NewValue);
                if (ex.InnerException.Message == "50384")
                    strmsg = strmsg.Replace("{}", model.CustomField4NewValue);
                if (ex.InnerException.Message == "50387")
                    strmsg = strmsg.Replace("{}", model.CustomField5NewValue);

                return BadRequest(strmsg);
            }
        }

        #region Right        
        [Route("UpdateForRight")]
        [HttpPost]
        public IHttpActionResult UpdateForRight(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.CompanyID = companyId;
                var accountModel = new AccountService().UpdateForRight(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.UpdateForRight", language, ex));
            }
        }
        #endregion

        #region License Number        
        [Route("UpdateForLicenseNumber")]
        [HttpPost]
        public IHttpActionResult UpdateForLicenseNumber(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                model.CompanyID = companyId;
                var accountModel = new AccountService().UpdateForLicenseNumber(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.UpdateForLicenseNumber", language, ex));
            }
        }
        #endregion

        #endregion

        #region Issue and Re-Print

        [Route("Issue")]
        [HttpPost]
        public IHttpActionResult Issue(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                new AccountService().Issue(companyId, model.ID, model.IssueDate, model.IssuedBy, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Issue", language, ex));
            }
        }
        [Route("PrintGet")]
        [HttpGet]
        public IHttpActionResult PrintGet(int accountServiceID, int? printTemplateId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().GetPrint(companyId, language, accountServiceID, printTemplateId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetPrint", language, ex));
            }
        }
        [Route("Print")]
        [HttpPost]
        public IHttpActionResult Print(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                new AccountService().Print(companyId, model.ID, model.PrintDate, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Print", language, ex));
            }
        }
        [Route("RePrintLog")]
        [HttpPost]
        public IHttpActionResult RePrintLog(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.RePrint.ToString(), model.ID.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Areas.Accounts.Controllers.RePrintLog", language, ex));
            }
        }

        #endregion

        #region Lock

        [Route("Lock")]
        [HttpPost]
        public IHttpActionResult Lock(AccountServiceModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                new AccountService().Lock(companyId, model.ID, model.IsLock, model.LockReasonTableValue, model.LockActionTableValue, model.LockComment, model.CreatedBy, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                string strmsg = ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Lock", language, ex);

                if (ex.InnerException.Message == "50299")
                    strmsg = strmsg.Replace("{}", model.CustomField1);
                if (ex.InnerException.Message == "50300")
                    strmsg = strmsg.Replace("{}", model.CustomField2);
                if (ex.InnerException.Message == "50301")
                    strmsg = strmsg.Replace("{}", model.CustomField3);
                if (ex.InnerException.Message == "50385")
                    strmsg = strmsg.Replace("{}", model.CustomField4);
                if (ex.InnerException.Message == "50388")
                    strmsg = strmsg.Replace("{}", model.CustomField5);
                return BadRequest(strmsg);
            }
        }

        #endregion

        #region Adjustment

        [Route("ProcessAdjustment")]
        [HttpPost]
        public IHttpActionResult ProcessAdjustment(AccountServiceAdjustmentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                new AccountService().ProcessAdjustment(model, companyId, language);
                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ProcessAdjustment", language, ex));
            }
        }
        [Route("AdjustmentsGet")]
        [HttpGet]
        public IHttpActionResult AdjustmentsGet(int AccountServiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().AdjustmentsGet(companyId, AccountServiceID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.AdjustmentsGet", language, ex));
            }
        }

        [Route("DeleteAdjustment")]
        [HttpPost]
        public IHttpActionResult DeleteAdjustment(AccountServiceAdjustmentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                if (model.ID > 0)
                    new AccountService().DeleteAdjustment(model);

                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountServiceCollectionDebt.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteAdjustment", language, ex));
            }
        }

        #endregion

        #region Exception

        [Route("ProcessExemption")]
        [HttpPost]
        public IHttpActionResult ProcessExemption(AccountServiceExemptionModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                if (model.ID > 0)
                    new AccountService().ExemptionUpdate(model, companyId, language);
                else
                    new AccountService().ProcessExemption(model, companyId, language);
                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ProcessExemption", language, ex));
            }
        }

        [Route("DeleteExemption")]
        [HttpPost]
        public IHttpActionResult DeleteExemption(AccountServiceExemptionModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                if (model.ID > 0)
                    new AccountService().DeleteExemption(model);

                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteExemption", language, ex));
            }
        }

        [Route("DeleteAllExemption")]
        [HttpPost]
        public IHttpActionResult DeleteAllExemption(AccountServiceExemptionModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                if (model.AccountServiceId > 0)
                    new AccountService().DeleteAllExemption(model);

                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteAllExemption", language, ex));
            }
        }

        [Route("ExemptionGet")]
        [HttpGet]
        public IHttpActionResult ExemptionGet(int AccountServiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().ExemptionGet(companyId, AccountServiceID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.Exemption", language, ex));
            }
        }

        [Route("ExemptionHistoryGet")]
        [HttpGet]
        public IHttpActionResult ExemptionHistoryGet(int AccountServiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().ExemptionHistoryGet(AccountServiceID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ExemptionHistoryGet", language, ex));
            }
        }

        #endregion

        #region Correction
        [Route("ProcessCorrection")]
        [HttpPost]
        public IHttpActionResult ProcessCorrection(AccountServiceCorrectiontModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                new AccountService().ProcessCorrection(model, companyId, language);
                // Table Update Activity
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceId.ToString(), "MUNSERAccountService.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ProcessCorrection", language, ex));
            }
        }

        [Route("ValidateUserPermissionForCorrection")]
        [HttpGet]
        public IHttpActionResult ValidateUserPermissionForCorrection()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                try
                {
                    Common.ValidateUserPermission(token, companyId, Guid.Parse(_correctionfeatureID), Actions.New);
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Areas.Collections.Controllers.ValidateUserPermissionForCorrection", language, ex));
            }
        }
        #endregion

        #region Extension

        [Route("ExtensionInsert")]
        [HttpPost]
        public IHttpActionResult ExtensionInsert(AccountServiceExtensionModel model)
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
                var Model = new AccountService().ExtensionInsert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ExtensionInsert", language, ex));
            }
        }
        [Route("ExtensionGet")]
        [HttpGet]
        public IHttpActionResult ExtensionGet(int ID, int AccountServiceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var Model = new AccountService().ExtensionGet(companyId, AccountServiceID, ID);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ExtensionGet", language, ex));
            }
        }
        #endregion

        #region Service Type Group

        [Route("ServiceTypeGroupGet")]
        [HttpGet]
        public IHttpActionResult ServiceTypeGroupGet(int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().ServiceTypeGroupGet(companyId, language, id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ServiceTypeGroupGet", language, ex));
            }
        }

        #endregion

        #region Service Type

        [Route("ServiceTypeGet")]
        [HttpGet]
        public IHttpActionResult ServiceTypeGet(int groupId, int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().ServiceTypeGet(companyId, language, groupId, id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ServiceTypeGet", language, ex));
            }
        }
        [Route("ServiceTypeGetNotPaid")]
        [HttpGet]
        public IHttpActionResult ServiceTypeGetNotPaid(int accountID, bool? checkActivePaymentPlan)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().ServiceTypeGetNotPaid(accountID, checkActivePaymentPlan, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ServiceTypeGetNotPaid", language, ex));
            }
        }

        [Route("ServiceTypeGetByNoFilingType")]
        [HttpGet]
        public IHttpActionResult ServiceTypeGetByNoFilingType(bool isNoFilingType)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().ServiceTypeGetByNoFilingType(companyId, language, isNoFilingType);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ServiceTypeGetByNoFilingType", language, ex));
            }
        }
        #endregion

        #region Fiscal Year
        [Route("FiscalYearGet")]
        [HttpGet]
        public IHttpActionResult FiscalYearGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FiscalYearGet();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FiscalYearGet", language, ex));
            }
        }

        [Route("FiscalYearGetByService")]
        [HttpGet]
        public IHttpActionResult FiscalYearGetByService(int serviceId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FiscalYearGetByService(serviceId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FiscalYearGetByService", language, ex));
            }
        }

        [Route("FiscalYearGetByServiceNotInAccount")]
        [HttpGet]
        public IHttpActionResult FiscalYearGetByServiceNotInAccount(int serviceID, int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FiscalYearGetByServiceNotInAccount(serviceID, accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FiscalYearGetByServiceNotInAccount", language, ex));
            }
        }

        [Route("FiscalYearGetByAccount")]
        [HttpGet]
        public IHttpActionResult FiscalYearGetByAccount(int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FiscalYearGetByAccount(accountID, companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FiscalYearGetByAccount", language, ex));
            }
        }

        [Route("FiscalYearGetByAccountNotFiled")]
        [HttpGet]
        public IHttpActionResult FiscalYearGetByAccountNotFiled(int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FiscalYearGetByAccountNotFiled(accountID, companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FiscalYearGetByAccountNotFiled", language, ex));
            }
        }
        #endregion

        #region Collection Detail

        [Route("CollectionDetailGet")]
        [HttpGet]
        public IHttpActionResult CollectionDetailGet(int accountServiceID, int? accountServiceCollectionDetailID, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().CollectionDetailGet(companyId, accountServiceID, accountServiceCollectionDetailID, language, filterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDetailGet", language, ex));
            }
        }

        [Route("CollectionDetailGetNotFiled")]
        [HttpGet]
        public IHttpActionResult CollectionDetailGetNotFiled(int accountServiceID, int? accountServiceCollectionDetailID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().CollectionDetailGetNotFiled(companyId, accountServiceID, accountServiceCollectionDetailID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDetailGetNotFiled", language, ex));
            }
        }
        [Route("CollectionDetailGetNotPaid")]
        [HttpGet]
        public IHttpActionResult CollectionDetailGetNotPaid(int AccountID, DateTime? PaymentDate, bool IsIvaApply, bool applybyAmnesty)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionDetailGetNotPaid(AccountID, PaymentDate, IsIvaApply, applybyAmnesty, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDetailGetNotPaid", language, ex));
            }
        }

        [Route("CollectionPropertyTaxDetailGetNotPaid")]
        [HttpGet]
        public IHttpActionResult CollectionPropertyTaxDetailGetNotPaid(int AccountPropertyID, int AccountPropertyRightID, DateTime? PaymentDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                int ServiceTypeID = 10;
                var model = new AccountService().CollectionPropertyTaxDetailGetNotPaid(AccountPropertyID, ServiceTypeID, AccountPropertyRightID, PaymentDate, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionPropertyTaxDetailGetNotPaid", language, ex));
            }
        }

        [Route("CollectionDetailSummaryGetNotPaid")]
        [HttpGet]
        public IHttpActionResult CollectionDetailSummaryGetNotPaid(int accountID, string collectiondetailIDs, bool forEdit, bool applybyAmnesty)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionDetailSummaryGetNotPaid(companyId, language, accountID, collectiondetailIDs, forEdit, applybyAmnesty);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDetailSummaryGetNotPaid", language, ex));
            }
        }

        [Route("CollectionDetailGetSummary")]
        [HttpGet]
        public IHttpActionResult CollectionDetailGetSummary(string licenseNumber, int? accountID, string filterText, int? accountserviceID, bool? isServiceTypeGroupIsTax)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().CollectionDetailGetSummary(companyId, language, licenseNumber, accountID, filterText, accountserviceID, isServiceTypeGroupIsTax);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDetailGetSummary", language, ex));
            }
        }

        [Route("CollectionFillingLicenseGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingLicenseGet(int accountServiceCollectionID, int? fillingLicenseID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingLicenseGet(companyId, accountServiceCollectionID, fillingLicenseID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingLicenseGet", language, ex));
            }
        }

        [Route("CollectionFillingLicenseImagesGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingLicenseImagesGet(int fillingID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingLicenseImagesGet(fillingID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingLicenseImagesGet", language, ex));
            }
        }


        [Route("DeleteFillingLicense")]
        [HttpPost]
        public IHttpActionResult DeleteFillingLicense(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);
                if (model.AccountServiceCollectionID > 0)
                    new AccountService().DeleteFillingLicense(model, language);

                // Table Update Activity
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceCollectionID.ToString(), "MUNSERAccountServiceCollectionDetail.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteFillingLicense", language, ex));
            }
        }

        [Route("CollectionFillingTaxGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingTaxGet(int accountServiceCollectionID, int? fillingTaxID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingTaxGet(companyId, accountServiceCollectionID, fillingTaxID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingTaxGet", language, ex));
            }
        }

        [Route("CollectionFillingTaxImagesGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingTaxImagesGet(int fillingID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingTaxImagesGet(fillingID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingTaxImagesGet", language, ex));
            }
        }
        [Route("CollectionFillingUnitGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingUnitGet(int accountServiceCollectionID, int? fillingUnitID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingUnitGet(companyId, accountServiceCollectionID, fillingUnitID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingUnitGet", language, ex));
            }
        }

        [Route("CollectionFillingUnitRuleGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingUnitRuleGet(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var Amount = new AccountService().CollectionFillingUnitRuleGet(companyId, ID);
                return Ok(Amount);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingUnitGet", language, ex));
            }
        }

        [Route("CollectionFillingUnitImagesGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingUnitImagesGet(int fillingID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingUnitImagesGet(fillingID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingUnitImagesGet", language, ex));
            }
        }

        [Route("CollectionFillingMeasuredWaterGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingMeasuredWaterGet(int accountServiceCollectionID, int? fillingMeasuredWaterID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingMeasuredWaterGet(companyId, accountServiceCollectionID, fillingMeasuredWaterID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingMeasuredWaterGet", language, ex));
            }
        }

        [Route("CollectionFillingPreviousMeasuredWaterGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingPreviousMeasuredWaterGet(int accountServiceCollectionID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingPreviousMeasuredWaterGet(companyId, accountServiceCollectionID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingPreviousMeasuredWaterGet", language, ex));
            }
        }

        [Route("CollectionFillingMeasuredWaterImagesGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingMeasuredWaterImagesGet(int fillingID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingMeasuredWaterImagesGet(fillingID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingMeasuredWaterImagesGet", language, ex));
            }
        }

        [Route("CollectionFillingPropertyTaxGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingPropertyTaxGet(int accountServiceCollectionID, int? fillingPropertyTaxID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingPropertyTaxGet(companyId, accountServiceCollectionID, fillingPropertyTaxID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingPropertyTaxGet", language, ex));
            }
        }

        [Route("CollectionFillingPropertyTaxImagesGet")]
        [HttpGet]
        public IHttpActionResult CollectionFillingPropertyTaxImagesGet(int fillingID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionFillingPropertyTaxImagesGet(fillingID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingPropertyTaxImagesGet", language, ex));
            }
        }

        [Route("CollectionAutoFillingGet")]
        [HttpGet]
        public IHttpActionResult CollectionAutoFillingGet(int accountServiceCollectionID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountService().CollectionAutoFillingGet(companyId, accountServiceCollectionID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionAutoFillingGet", language, ex));
            }
        }
        #endregion

        #region Filing

        [Route("FillingGet")]
        [HttpGet]
        public IHttpActionResult FillingGet(int accountServiceID, int? accountServiceCollectionDetailID, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().FillingGet(companyId, accountServiceID, accountServiceCollectionDetailID, language, filterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.FillingGet", language, ex));
            }
        }

        #endregion

        #region Filing - BUSINESS LICENSE

        [Route("CollectionFillingLicenseInsert")]
        [HttpPost]
        public IHttpActionResult CollectionFillingLicenseInsert(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new AccountService().CollectionFillingLicenseInsert(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingLicenseInsert", language, ex));
            }
        }

        [Route("CollectionFillingLicenseUpdate")]
        [HttpPost]
        public IHttpActionResult CollectionFillingLicenseUpdate(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                var Model = new AccountService().CollectionFillingLicenseUpdate(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingLicenseUpdate", language, ex));
            }
        }

        #endregion

        #region Filing - Tax IVU

        [Route("CollectionFillingTaxInsert")]
        [HttpPost]
        public IHttpActionResult CollectionFillingTaxInsert(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new AccountService().CollectionFillingTaxInsert(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingTaxInsert", language, ex));
            }
        }

        [Route("CollectionFillingTaxUpdate")]
        [HttpPost]
        public IHttpActionResult CollectionFillingTaxUpdate(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new AccountService().CollectionFillingTaxUpdate(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingTaxUpdate", language, ex));
            }
        }

        #endregion

        #region Filing - UNTI

        [Route("CollectionFillingUnitInsert")]
        [HttpPost]
        public IHttpActionResult CollectionFillingUnitInsert(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new AccountService().CollectionFillingUnitInsert(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingUnitInsert", language, ex));
            }
        }

        [Route("CollectionFillingUnitUpdate")]
        [HttpPost]
        public IHttpActionResult CollectionFillingUnitUpdate(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                var Model = new AccountService().CollectionFillingUnitUpdate(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingUnitUpdate", language, ex));
            }
        }

        [Route("DeleteFillingUnit")]
        [HttpPost]
        public IHttpActionResult DeleteFillingUnit(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);
                if (model.AccountServiceCollectionID > 0)
                    new AccountService().DeleteFillingUnit(model, language);

                // Table Update Activity
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceCollectionID.ToString(), "MUNSERAccountServiceCollectionDetail.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteFillingUnit", language, ex));
            }
        }

        #endregion

        #region Filing - MEASURED WATER

        [Route("CollectionFillingMeasuredWaterInsert")]
        [HttpPost]
        public IHttpActionResult CollectionFillingMeasuredWaterInsert(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);
                var Model = new AccountService().CollectionFillingMeasuredWaterInsert(model, companyId, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingMeasuredWaterInsert", language, ex));
            }
        }

        [Route("CollectionFillingMeasuredWaterUpdate")]
        [HttpPost]
        public IHttpActionResult CollectionFillingMeasuredWaterUpdate(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                var Model = new AccountService().CollectionFillingMeasuredWaterUpdate(model, companyId, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), Model.ID.ToString(), string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingMeasuredWaterUpdate", language, ex));
            }
        }

        [Route("DeleteFillingMeasuredWater")]
        [HttpPost]
        public IHttpActionResult DeleteFillingMeasuredWater(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);
                if (model.AccountServiceCollectionID > 0)
                    new AccountService().DeleteFillingMeasuredWater(model, language);

                // Table Update Activity
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceCollectionID.ToString(), "MUNSERAccountServiceCollectionDetail.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteFillingMeasuredWater", language, ex));
            }
        }

        [Route("CollectionFillingMeasuredWaterExport")]
        [HttpGet]
        public IHttpActionResult CollectionFillingMeasuredWaterExport(int? accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);
                var Model = new AccountService().CollectionFillingMeasuredWaterExport(accountID, companyId, language);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingMeasuredWaterExport", language, ex));
            }
        }

        #endregion

        #region Filing - PROPERTY TAX

        [Route("CollectionFillingPropertyTaxInsert")]
        [HttpPost]
        public IHttpActionResult CollectionFillingPropertyTaxInsert(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try

            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var Model = new AccountService().CollectionFillingPropertyTaxInsert(model, companyId, language);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), string.Empty, string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingPropertyTaxInsert", language, ex));
            }
        }

        [Route("CollectionFillingPropertyTaxUpdate")]
        [HttpPost]
        public IHttpActionResult CollectionFillingPropertyTaxUpdate(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                var Model = new AccountService().CollectionFillingPropertyTaxUpdate(model, companyId, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), string.Empty, string.Empty);
                return Ok(Model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionFillingPropertyTaxUpdate", language, ex));
            }
        }

        [Route("DeleteFillingPropertyTax")]
        [HttpPost]
        public IHttpActionResult DeleteFillingPropertyTax(AccountServiceCollectionFillingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);
                if (model.AccountServiceCollectionID > 0)
                    new AccountService().DeleteFillingPropertyTax(model, language);

                // Table Update Activity
                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.AccountServiceCollectionID.ToString(), "MUNSERAccountServiceCollectionDetail.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteFillingPropertyTax", language, ex));
            }
        }

        #endregion

        #region Filing - AUTO FILING
        [Route("DeleteFillingAutoFiling")]
        [HttpGet]
        public IHttpActionResult DeleteFillingAutoFiling(int accountServiceCollectionID, string notes, Guid createdUserID, DateTime createdDate)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.Edit);
                new AccountService().DeleteFillingAutoFiling(companyID, language, accountServiceCollectionID, notes, createdUserID, createdDate);

                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.Edit.ToString(), accountServiceCollectionID.ToString(), "MUNSERAccountServiceCollectionDetail.ID");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.DeleteFillingAutoFiling", language, ex));
            }
        }
        #endregion

        #region Debt

        [Route("CollectionDebtGet")]
        [HttpGet]
        public IHttpActionResult CollectionDebtGet(int accountServiceID, int? accountServiceCollectionDetailID, bool? onlyAdjustment, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().CollectionDebtGet(companyId, accountServiceID, accountServiceCollectionDetailID, onlyAdjustment, language, filterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDebtGet", language, ex));
            }
        }

        #endregion

        #region Discount

        [Route("CollectionDiscountGet")]
        [HttpGet]
        public IHttpActionResult CollectionDiscountGet(int accountServiceID, int? accountServiceCollectionDetailID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().CollectionDiscountGet(companyId, accountServiceID, accountServiceCollectionDetailID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.CollectionDiscountGet", language, ex));
            }
        }

        #endregion

        #region Payment

        [Route("PaymentHistoryGet")]
        [HttpGet]
        public IHttpActionResult PaymentHistoryGet(int accountServiceID, int? accountServiceCollectionDetailID, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().PaymentHistoryGet(companyId, accountServiceID, accountServiceCollectionDetailID, language, filterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.PaymentHistoryGet", language, ex));
            }
        }

        #endregion

        #region Import Account Service Filing
        [Route("ValidateImportAccountServiceFiling")]
        [HttpPost]
        public IHttpActionResult ValidateImportAccountServiceFiling(AccountServiceFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                var data = new AccountService().ImportAccountServiceFilingValidation(model, language);

                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Import Account Statement");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceFilingValidation", language, ex));
            }
        }

        [Route("InsertImportAccountServiceFiling")]
        [HttpPost]
        public IHttpActionResult InsertImportAccountServiceFiling(AccountServiceFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                int result = new AccountService().ImportAccountServiceFilingInsert(model, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceFilingInsert", language, ex));
            }
        }


        #endregion

        #region Import Measured Water Filing
        [Route("InsertImportMeasuredWaterFiling")]
        [HttpPost]
        public IHttpActionResult InsertImportMeasuredWaterFiling(MeasuredWaterFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);
                //HttpContext.Current.Server.ScriptTimeout = int.MaxValue;
                int result = new AccountService().ImportMeasuredWaterFilingInsert(companyId, language, model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Measured Water Filing");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportMeasuredWaterFilingInsert", language, ex));
            }
        }
        #endregion

        #region Validate Import Measured Water Filing 
        [Route("GetValidateMeasuredWaterFilingForFixFile")]
        [HttpGet]
        public IHttpActionResult GetValidateMeasuredWaterFilingForFixFile(int? periodYear, int? periodMonth, bool isGenerateFile, bool isInconsistencies, bool isHighConsumption, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);
                var data = new AccountService().GetValidateMeasuredWaterFilingForFixFile(companyID, periodYear, periodMonth, isGenerateFile,isInconsistencies,isHighConsumption, filterText);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Validate Measured Water Filing for Fix File");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetValidateMeasuredWaterFilingForFixFile", language, ex));
            }
        }

        [Route("InsertValidateMeasuredWaterFilingForFixFile")]
        [HttpPost]
        public IHttpActionResult InsertValidateMeasuredWaterFilingForFixFile(MeasuredWaterFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);
                var data = new AccountService().InsertValidateMeasuredWaterFilingForFixFile(model, language);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Insert Validate Measured Water Filing for Fix File");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.InsertValidateMeasuredWaterFilingForFixFile", language, ex));
            }
        }

        [Route("UpdateValidateMeasuredWaterFilingForFixFile")]
        [HttpPost]
        public IHttpActionResult UpdateValidateMeasuredWaterFilingForFixFile(MeasuredWaterFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);
                var data = new AccountService().UpdateValidateMeasuredWaterFilingForFixFile(model, language);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Update Validate Measured Water Filing for Fix File");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.UpdateValidateMeasuredWaterFilingForFixFile", language, ex));
            }
        }

        [Route("GetAccountServiceForValidateMeasuredWaterFiling")]
        [HttpPost]
        public IHttpActionResult GetAccountServiceForValidateMeasuredWaterFiling(MeasuredWaterFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);
                var data = new AccountService().GetAccountServiceForValidateMeasuredWaterFiling(model);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Validate Measured Water Filing");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.GetAccountServiceForValidateMeasuredWaterFiling", language, ex));
            }
        }

        [Route("ValidateImportMeasuredWaterFiling")]
        [HttpPost]
        public IHttpActionResult ValidateImportMeasuredWaterFiling(MeasuredWaterFilingImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);
                var data = new AccountService().ValidateImportMeasuredWaterFiling(model, language);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Validate Measured Water Filing");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ValidateImportMeasuredWaterFiling", language, ex));
            }
        }
        #endregion

        #region Migration
        [Route("ImportAccountServicePropertyTax")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePropertyTax(ImportAccountServiceFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                string AccountServiceFilingDetailJson = new JavaScriptSerializer().Serialize(model.AccountServiceFilingDetail);
                result = new AccountService().ImportAccountServicePropertyTax(model, AccountServiceFilingDetailJson);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceFiling", language, ex));
            }
        }

        [Route("ImportAccountServicePropertyTaxPayment")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePropertyTaxPayment(ImportAccountServiceFilingPaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                if (model.AccountServicePaymentDetail != null && model.AccountServicePaymentDetail.Count > 0)
                {
                    string NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                    string AccountServicePaymentDetailJson = new JavaScriptSerializer().Serialize(model.AccountServicePaymentDetail);
                    result = new AccountService().ImportAccountServicePropertyTaxPayment(model, NumberPrefix, AccountServicePaymentDetailJson);
                }

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceFiling", language, ex));
            }
        }

        [Route("ImportAccountServicePropertyTaxPaymentForSelectedYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePropertyTaxPaymentForSelectedYear(ImportAccountServiceFilingPaymentForSelectedYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                if (model.AccountServicePaymentDetail != null && model.AccountServicePaymentDetail.Count > 0)
                {
                    string NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                    string AccountServicePaymentDetailJson = new JavaScriptSerializer().Serialize(model.AccountServicePaymentDetail);
                    result = new AccountService().ImportAccountServicePropertyTaxPaymentForSelectedYear(model, NumberPrefix, AccountServicePaymentDetailJson);
                }

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceFiling", language, ex));
            }
        }

        [Route("ImportAccountServiceMeasureWater")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceMeasureWater(ImportAccountServiceMeasureWaterFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                string AccountServiceFilingDetailJson = new JavaScriptSerializer().Serialize(model.AccountServiceFilingDetail);
                bool result = new AccountService().ImportAccountServiceMeasureWater(model, AccountServiceFilingDetailJson);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Measure Water Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceMeasureWater", language, ex));
            }
        }

        [Route("ImportAccountServiceMeasureWaterManual")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceMeasureWaterManual(ImportAccountServiceMeasureWaterFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                string AccountServiceFilingDetailJson = new JavaScriptSerializer().Serialize(model.AccountServiceFilingDetail);
                bool result = new AccountService().ImportAccountServiceMeasureWaterManual(model, AccountServiceFilingDetailJson);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Measure Water Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceMeasureWater", language, ex));
            }
        }

        [Route("ImportAccountServiceAutoCreationAndAutoFilingServices")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceAutoCreationAndAutoFilingServices(ImportAccountServiceAutoFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = new AccountService().ImportAccountServiceAutoCreationAndAutoFilingServices(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import AutoFiling Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceAutoCreationAndAutoFilingServices", language, ex));
            }
        }

        [Route("ImportAccountServiceLicense")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceLicense(ImportAccountServiceLicenseFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                string AccountServiceFilingDetailJson = new JavaScriptSerializer().Serialize(model.AccountServiceFilingDetail);
                result = new AccountService().ImportAccountServiceLicense(model, AccountServiceFilingDetailJson);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceLicense", language, ex));
            }
        }

        [Route("ImportAccountServiceUnit")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceUnit(ImportAccountServiceUnitFilingModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                string AccountServiceFilingDetailJson = new JavaScriptSerializer().Serialize(model.AccountServiceFilingDetail);
                bool result = new AccountService().ImportAccountServiceUnit(model, AccountServiceFilingDetailJson);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Unit Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceUnit", language, ex));
            }
        }

        [Route("ImportAccountServicePayment")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePayment(ImportAccountServiceFilingPaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                if (model.AccountServicePaymentDetail != null && model.AccountServicePaymentDetail.Count > 0)
                {
                    string NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                    string AccountServicePaymentDetailJson = new JavaScriptSerializer().Serialize(model.AccountServicePaymentDetail);
                    result = new AccountService().ImportAccountServicePayment(model, NumberPrefix, AccountServicePaymentDetailJson);
                }

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Payment From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServicePayment", language, ex));
            }
        }

        [Route("ImportAccountServiceAutoFilingPayment")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceAutoFilingPayment(ImportAccountServiceFilingPaymentModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                if (model.AccountServicePaymentDetail != null && model.AccountServicePaymentDetail.Count > 0)
                {
                    string NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                    string AccountServicePaymentDetailJson = new JavaScriptSerializer().Serialize(model.AccountServicePaymentDetail);
                    result = new AccountService().ImportAccountServiceAutoFilingPayment(model, NumberPrefix, AccountServicePaymentDetailJson);
                }

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Payment From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceAutoFilingPayment", language, ex));
            }
        }

        [Route("ImportAccountServiceAutoFilingPaymentForSelectedYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceAutoFilingPaymentForSelectedYear(ImportAccountServiceFilingPaymentForSelectedYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.Language = language;
                model.CompanyID = companyId;
                bool result = false;
                if (model.AccountServicePaymentDetail != null && model.AccountServicePaymentDetail.Count > 0)
                {
                    string NumberPrefix = new SecuirtyAPIModel().PaymentNumberPrefixByUserRoleGet(token, model.CompanyID, language);
                    string AccountServicePaymentDetailJson = new JavaScriptSerializer().Serialize(model.AccountServicePaymentDetail);
                    result = new AccountService().ImportAccountServiceAutoFilingPaymentForSelectedYear(model, NumberPrefix, AccountServicePaymentDetailJson);
                }

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Payment From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceAutoFilingPayment", language, ex));
            }
        }


        [Route("ImportAccountServicePropertyTaxOwnerSinglePropertyPreviousYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePropertyTaxOwnerSinglePropertyPreviousYear(ImportAccountServicePropertyForPreviousYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                bool result = false;
                result = new AccountService().ImportAccountServicePropertyTaxOwnerSinglePropertyPreviousYear(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServicePropertyTaxOwnerSinglePropertyPreviousYear", language, ex));
            }
        }
        [Route("ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear(ImportAccountServicePropertyForPreviousYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                bool result = false;
                result = new AccountService().ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear", language, ex));
            }
        }

        [Route("ImportAccountServiceAutoCreationPreviousYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceAutoCreationPreviousYear(ImportAccountServiceAutoCreationPreviousYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                bool result = false;
                result = new AccountService().ImportAccountServiceAutoCreationPreviousYear(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServiceAutoCreationPreviousYear", language, ex));
            }
        }

        [Route("ImportAccountServiceUnitPreviousYear")]
        [HttpPost]
        public IHttpActionResult ImportAccountServiceUnitPreviousYear(ImportAccountServiceUnitPreviousYearModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                bool result = false;
                result = new AccountService().ImportAccountServiceUnitPreviousYear(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Filing From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear", language, ex));
            }
        }

        [Route("ImportAccountServicePaymentPlan")]
        [HttpPost]
        public IHttpActionResult ImportAccountServicePaymentPlan(ImportAccountServicePaymentPlanModel model)
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
                bool result = false;
                if (model.InvoiceDetail != null && model.InvoiceDetail.Count > 0)
                {
                    string InvoiceDetailJson = new JavaScriptSerializer().Serialize(model.InvoiceDetail);
                    result = new AccountService().ImportAccountServicePaymentPlan(model, InvoiceDetailJson);
                }


                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Service Payment Plan From Migration");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.ImportAccountServicePaymentPlan", language, ex));
            }
        }
        #endregion

        #region Account Type
        [Route("AccountTypeGet")]
        [HttpGet]
        public IHttpActionResult AccountTypeGet(Guid ApplicationID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountService().AccountTypeGet(ApplicationID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountService.AccountTypeGet", language, ex));
            }
        }
        #endregion
    }
}
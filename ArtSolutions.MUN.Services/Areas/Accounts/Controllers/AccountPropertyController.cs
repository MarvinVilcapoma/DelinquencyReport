using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using static ArtSolutions.MUN.Core.AccountModels.AccountProperty;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/AccountProperty")]
    public class AccountPropertyController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";

        #region Account Property
        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult Get(string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder, int? AccountID = null)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().Get(companyId, language, filterText, pageIndex, pageSize, sortColumn, sortOrder, AccountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Get", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int? id, bool? isActive, int transferID = 0)
        {

            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().Get(companyId, id, isActive, transferID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Get", language, ex));
            }
        }

        [Route("GetForSearch")]
        [HttpGet]
        public IHttpActionResult GetForSearch(string searchText, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().GetForSearch(companyId, searchText, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.GetForSearch", language, ex));
            }
        }

        [Route("GetRightForSearch")]
        [HttpGet]
        public IHttpActionResult GetRightForSearch(string searchText, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().GetRightForSearch(companyId, searchText, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.GetRightForSearch", language, ex));
            }
        }

        [Route("GetAccountPropertyRightByOwner")]
        [HttpGet]
        public IHttpActionResult GetAccountPropertyRightByOwner(int ownerID, int serviceID, int fiscalYearID, int? id, bool? isTransferByRight)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().GetAccountPropertyRightByOwner(companyId, ownerID, serviceID, fiscalYearID, id, isTransferByRight);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.GetAccountPropertyRightByOwner", language, ex));
            }
        }

        [Route("GetAccountPropertyRight")]
        [HttpGet]
        public IHttpActionResult GetAccountPropertyRight()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().GetAccountPropertyRight(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.GetAccountPropertyRight", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(AccountPropertyModel model)
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
                int AccountPropertyID = new AccountProperty().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(AccountPropertyID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Insert", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(AccountPropertyModel model)
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
                new AccountProperty().Update(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(1);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Update", language, ex));
            }
        }

        [Route("DeleteSplit")]
        [HttpGet]
        public IHttpActionResult DeleteSplit(int TransferID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                new AccountProperty().SplitDelete(TransferID, companyId);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), TransferID.ToString(), TransferID.ToString());
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.DeleteSplit", language, ex));
            }
        }

        [Route("AccountPropertyRightUpdate")]
        [HttpPost]
        public IHttpActionResult AccountPropertyRightUpdate(AccountPropertyModel model)
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
                new AccountProperty().AccountPropertyRightUpdate(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(1);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.AccountPropertyRightUpdate", language, ex));
            }
        }

        [Route("MergeCheckNotAssociatedServices")]
        [HttpGet]
        public IHttpActionResult MergeCheckNotAssociatedServices(string commaSeparatedPropertyID, int? AccountID = 0)
        {

            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);
                var model = new AccountProperty().MergeCheckNotAssociatedServices(companyId, language, commaSeparatedPropertyID, AccountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.MergeCheckNotAssociatedServices", language, ex));
            }
        }

        [Route("Split")]
        [HttpPost]
        public IHttpActionResult Split(AccountPropertyModel model)
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
                int TransferID = new AccountProperty().Split(model);
                if (TransferID > 0)
                {
                    Dictionary<string, string> replaceContent = new Dictionary<string, string>
                        {
                            { "[FullName]", model.AssignToName },
                            { "[OldFincaID]", model.Property },
                            { "[SplitDate]", model.SplitDate.ToString("d") },
                            { "[RightSplitUrl]", string.Format(ConfigurationManager.AppSettings["URLMunicipalityWebSitePath"]+"/Collections/PropertyTransfer/{0}?ID={1}",model.workflowStatusID != 43 ? "EditSplit":"ViewSplit", TransferID ) },
                            { "[LogoUrl]", Common.EmailLogoUrl },
                        };

                    Common.SendMailNotification(token, language, companyId, model.AssignToUserEmail, 45, replaceContent, null, null, true, null, 59);
                }
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(TransferID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Split", language, ex));
            }
        }

        [Route("Merge")]
        [HttpPost]
        public IHttpActionResult Merge(AccountPropertyModel model)
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
                int TransferID = new AccountProperty().Merge(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(TransferID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Merge", language, ex));
            }
        }

        [Route("ActiveInactive")]
        [HttpPost]
        public IHttpActionResult ActiveInactive(AccountPropertyModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                int ID = new AccountProperty().ActiveInactive(model.CompanyID, model.ID, model.IsActive, model.ModifiedUserID, model.ModifiedDate);

                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), ID.ToString(), ID.ToString());
                return Ok(ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountPropertyModels.AccountProperty.ActiveInactive", language, ex));
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IHttpActionResult Delete(AccountPropertyModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Delete);

                int ID = new AccountProperty().Delete(model.ID, model.ModifiedUserID, model.ModifiedDate);

                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Delete.ToString(), ID.ToString(), ID.ToString());
                return Ok(ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountPropertyModels.AccountProperty.Delete", language, ex));
            }
        }

        [Route("InsertTemporary")]
        [HttpPost]
        public IHttpActionResult InsertTemporary(AccountPropertyModel model)
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
                int AccountPropertyID = new AccountProperty().InsertTemporary(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(AccountPropertyID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.InsertTemporary", language, ex));
            }
        }
        #endregion

        #region Account Property Filing
        [Route("GetForFilling")]
        [HttpGet]
        public IHttpActionResult GetForFilling(int id, int accountServiceCollectionDetailId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().GetForFilling(companyId, id, accountServiceCollectionDetailId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.GetForFilling", language, ex));
            }
        }

        [Route("PropertyFilingHistoryGet")]
        [HttpGet]
        public IHttpActionResult PropertyFilingHistoryGet(int PropertyAccountID, string PropertyNumber, string RightNumber)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().PropertyFilingHistoryGet(companyId, language, PropertyAccountID, PropertyNumber, RightNumber);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.PropertyFilingHistoryGet", language, ex));
            }
        }

        [Route("AccountPropertyRightHistoryGet")]
        [HttpGet]
        public IHttpActionResult AccountPropertyRightHistoryGet(int AccountPropertyID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().AccountPropertyRightHistoryGet(companyId, language, AccountPropertyID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.PropertyFilingHistoryGet", language, ex));
            }
        }

        [Route("AccountPropertyRightGetNotPaid")]
        [HttpGet]
        public IHttpActionResult AccountPropertyRightGetNotPaid(int accountPropertyID, bool? checkActivePaymentPlan)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().PropertyRightGetNotPaid(accountPropertyID, checkActivePaymentPlan, companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.PropertyRightGetNotPaid", language, ex));
            }
        }

        [Route("AccountPropertyHasDebt")]
        [HttpGet]
        public IHttpActionResult AccountPropertyHasDebt(int ID, int accountID)
        {

            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new AccountProperty().HasDebt(ID, accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.ISPendingAmount", language, ex));
            }
        }
        #endregion

        #region Import Account Property
        [Route("ValidateImportAccountProperty")]
        [HttpPost]
        public IHttpActionResult ValidateImportAccountProperty(AccountPropertyImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                var data = new AccountProperty().ImportAccountPropertyValidation(model, language);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Import Account Property Statement");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ImportAccountPropertyValidation", language, ex));
            }
        }

        [Route("InsertImportAccountProperty")]
        [HttpPost]
        public IHttpActionResult InsertImportAccountProperty(AccountPropertyImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                int result = new AccountProperty().ImportAccountPropertyInsert(model, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account Property");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ImportAccountPropertyInsert", language, ex));
            }
        }
        #endregion

        #region Migrate Account Property

        [Route("MigrationImport")]
        [HttpPost]
        public IHttpActionResult MigrationImport(AccountPropertyModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                //Code added for Import
                if (model.AccountPropertyConstructionDetail != null && model.AccountPropertyConstructionDetail.Count > 0)
                    model.AccountPropertyConstructionDetailJson = new JavaScriptSerializer().Serialize(model.AccountPropertyConstructionDetail);
                model.CompanyID = companyId;
                new AccountProperty().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.PropertyNumber, model.RegisterNumber);
                return Ok(1);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.AccountProperty.Insert", language, ex));
            }
        }

        #endregion
    }
}

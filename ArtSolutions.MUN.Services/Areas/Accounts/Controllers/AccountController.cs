using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.AccountModels;
using ArtSolutions.MUN.Services.Areas.Accounts.Models;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        const string _featureID = "00000000-0009-0001-0001-000000000000";
        const string _blockJudicialCollectionfeatureID = "00000000-0009-0001-0003-000000000000";


        #region Account Get
        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult Get(bool? status, int accountTypeID, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().Get(companyId, language, status, accountTypeID, filterText, pageIndex, pageSize, sortColumn, sortOrder);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.Get", language, ex));
            }
        }

        [Route("GetAccountForExist")]
        [HttpGet]
        public IHttpActionResult GetAccountForExist(string taxNumber, string treasuryNumber, string stateNumber, int? ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().AccountGetForExist(companyId, taxNumber, treasuryNumber, stateNumber, ID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.AccountGetForExist", language, ex));
            }
        }

        [Route("GetAddressesForExist")]
        [HttpGet]
        public IHttpActionResult GetAddressesForExist(int ID, int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().AddressesGetForExist(ID, accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.AddressesGetForExist", language, ex));
            }
        }

        [Route("GetContactsForExist")]
        [HttpGet]
        public IHttpActionResult GetContactsForExist(int ID, int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().ContactsGetForExist(ID, accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ContactsGetForExist", language, ex));
            }
        }

        [Route("GetByRegistrationInformation")]
        [HttpGet]
        public IHttpActionResult GetByRegistrationInformation(string taxNumber, string treasuryNumber, string stateNumber, int? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().GetByRegistrationInformation(taxNumber, treasuryNumber, stateNumber, id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.GetByRegistrationInformation", language, ex));
            }
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(string accountID, int? accountType, string displayName, string taxNumber, string phoneNumber, string filterText, bool? isActive)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().Get(companyId, accountID, accountType, displayName, taxNumber, phoneNumber, filterText, isActive);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.Get", language, ex));
            }
        }

        [Route("GetForSearch")]
        [HttpGet]
        public IHttpActionResult GetForSearch(string accountTypeIDs, int? accountID, string searchText, bool? isActive, int pageIndex, int pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().GetForSearch(companyId, accountTypeIDs, accountID, searchText, isActive, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.GetForSearch", language, ex));
            }
        }

        [Route("IsSponsorByAccount")]
        [HttpGet]
        public IHttpActionResult IsSponsorByAccount(int accountID, int? accountType)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().IsSponsorByAccount(companyId, accountID, accountType);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.IsSponsorByAccount", language, ex));
            }
        }

        [Route("IndividualGet")]
        [HttpGet]
        public IHttpActionResult IndividualGet(int accountId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                AccountIndividualModel model = new Account().IndividualGet(language, accountId, false);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.IndividualGet", language, ex));
            }
        }

        [Route("IndividualGetForEdit")]
        [HttpGet]
        public IHttpActionResult IndividualGetForEdit(int accountId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                AccountIndividualModel model = new Account().IndividualGet(language, accountId, true);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.IndividualGet", language, ex));
            }
        }

        [Route("BusinessGet")]
        [HttpGet]
        public IHttpActionResult BusinessGet(int? accountId, int? parentId, string filterText)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().BusinessGet(accountId, parentId, language, false, filterText);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.BusinessGet", language, ex));
            }
        }
        [Route("BusinessGetForEdit")]
        [HttpGet]
        public IHttpActionResult BusinessGetForEdit(int? accountId, int? parentId)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                List<AccountBusinessModel> model = new Account().BusinessGet(accountId, parentId, language, true, null);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.BusinessGet", language, ex));
            }
        }

        #region Export
        [Route("GetExportAccountList")]
        [HttpGet]
        public IHttpActionResult GetExportAccountList()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                AccountListExport model = new Account().GetAccountListExport(companyId,language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.GetAccountListExport", language, ex));
            }
        }
        #endregion
        #endregion

        #region Account Support Tables
        [Route("AddressGet")]
        [HttpGet]
        public IHttpActionResult AddressGet(int? addressID, int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().AddressGet(addressID, accountID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.AddressGet", language, ex));
            }
        }

        [Route("ContactGet")]
        [HttpGet]
        public IHttpActionResult ContactGet(int? contactID, int? accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().ContactGet(contactID, accountID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ContactGet", language, ex));
            }
        }

        [Route("EmailGet")]
        [HttpGet]
        public IHttpActionResult EmailGet(int? id, int accountID, int? emailTypeID, int? emailTypeTableID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().EmailGet(id, accountID, emailTypeID, emailTypeTableID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.EmailGet", language, ex));
            }
        }

        [Route("FileGet")]
        [HttpGet]
        public IHttpActionResult FileGet(int? fileID, int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().FileGet(fileID, accountID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.FileGet", language, ex));
            }
        }

        [Route("PhoneGet")]
        [HttpGet]
        public IHttpActionResult PhoneGet(int? id, int accountID, int? phoneTypeID, int? phoneTypeTableID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().PhoneGet(id, accountID, phoneTypeID, phoneTypeTableID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.PhoneGet", language, ex));
            }
        }

        [Route("CompanyCurrencyGet")]
        [HttpGet]
        public IHttpActionResult CompanyCurrencyGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().CompanyCurrencyGet(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.CompanyCurrencyGet", language, ex));
            }
        }

        [Route("CompanyCurrencyGetWithoutAuthentication")]
        [HttpGet]
        public IHttpActionResult CompanyCurrencyGetWithoutAuthentication()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                var model = new Account().CompanyCurrencyGet(companyId, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.CompanyCurrencyGet", language, ex));
            }
        }

        [Route("SupportTableValueGet")]
        [HttpGet]
        public IHttpActionResult SupportTableValueGet(int tableID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().SupportTableValueGet(companyId, tableID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.SupportTableValueGet", language, ex));
            }
        }

        [Route("CurrencyGet")]
        [HttpGet]
        public IHttpActionResult CurrencyGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().CurrencyGet(language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.CurrencyGet", language, ex));
            }
        }
        #endregion

        #region Account as People
        [Route("GetAsPeople")]
        [HttpGet]
        public IHttpActionResult GetAsPeople(int accountType, string id, string filterText, Nullable<bool> isActive, Int32 pageIndex, Int32 pageSize)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Account().GetAsPeople(companyId, accountType, id, filterText, isActive, pageIndex, pageSize);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.GetAccountAsPeople", language, ex));
            }
        }
        #endregion

        #region Account Insert
        [Route("IndividualInsert")]
        [HttpPost]
        public IHttpActionResult IndividualInsert(AccountIndividualModel model)
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
                var Result = new Account().IndividualInsert(model);
                int accountId = Result.Item1;
                model.RegisterNumber = Result.Item2;

                //// Code commented for SE-85 issue [Contact to Admin error due to User dont have right for Finance]
                ////Insert record in FINANCE
                //if (accountId > 0)
                //{
                //    model.AccountID = accountId;
                //    SalesAccountIndividualInsertUpdate(true, true, token, language, companyId, model);
                //    ////CO-1295
                //    //AccountSupportTableInsert(model.AccountID, token, language, companyId);
                //}

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), accountId.ToString(), model.DisplayName);
                return Ok(accountId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.IndividualInsert", language, ex));
            }
        }

        [Route("BusinessInsert")]
        [HttpPost]
        public IHttpActionResult BusinessInsert(AccountBusinessModel model)
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
                var Result = new Account().BusinessInsert(model);
                int accountId = Result.Item1;
                model.RegisterNumber = Result.Item2;

                //// Code commented for SE-85 issue [Contact to Admin error due to User dont have right for Finance] 
                ////Insert record in FINANCE
                //if (accountId > 0)
                //{
                //    model.AccountID = accountId;
                //    SalesAccountBusinessInsertUpdate(true, true, token, language, companyId, model);
                //    ////CO-1295
                //    //AccountSupportTableInsert(model.AccountID, token, language, companyId);
                //}

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), accountId.ToString(), model.DisplayName);
                return Ok(accountId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.BusinessInsert", language, ex));
            }
        }

        private int AccountSupportTableInsert(int AccountID, Guid token, string language, int companyId)
        {
            List<MUNAccountGetForSupportTable_Result> munSupportTablelist = new Account().AccountGetForSupportTable(AccountID);
            List<MUNAccountGetForSupportTable_Result> finSupportTableList = AccountGetForSupportTableList(AccountID, token, language, companyId);
            List<AccountSupportTable> list = new List<AccountSupportTable>();

            int index = 0;
            foreach (var item in munSupportTablelist)
            {
                list.Add(new AccountSupportTable(AccountID, item.ID, finSupportTableList[index].ID, item.SupportType));
                index++;
            }
            return new Account().AccountSupportTableInsert(AccountID, list);
        }

        #endregion

        #region Account Update
        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(MUNAccountUpdate_Result model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                var accountModel = new Account().Update(model);

                //Finance Call Commented (Error raise from finance due to new changes in account page,not sure about finance) CO-1010,CO-1112,CO-1091,CO-1017 etc.
                ////Update record in FINANCE
                //if (accountModel != null)
                //    SalesAccountUpdate(token, language, companyId, model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), model.DisplayName);
                return Ok(accountModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.Update", language, ex));
            }
        }

        [Route("IndividualUpdate")]
        [HttpPost]
        public IHttpActionResult IndividualUpdate(AccountIndividualModel model)
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
                var accountId = new Account().IndividualUpdate(model);

                //// Code commented for SE-85 issue [Contact to Admin error due to User dont have right for Finance]
                ////Update record in FINANCE
                //if (accountId > 0)
                //{
                //    SalesAccountIndividualInsertUpdate(false, true, token, language, companyId, model);
                //    ////CO-1295
                //    //AccountSupportTableInsert(model.AccountID, token, language, companyId);
                //}

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), accountId.ToString(), model.DisplayName);
                return Ok(accountId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.IndividualUpdate", language, ex));
            }
        }

        [Route("BusinessUpdate")]
        [HttpPost]
        public IHttpActionResult BusinessUpdate(AccountBusinessModel model)
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
                var accountId = new Account().BusinessUpdate(model);

                //// Code commented for SE-85 issue [Contact to Admin error due to User dont have right for Finance]
                ////Update record in FINANCE
                //if (accountId > 0)
                //{
                //    SalesAccountBusinessInsertUpdate(false, true, token, language, companyId, model);

                //    ////CO-1295
                //    //AccountSupportTableInsert(model.AccountID, token, language, companyId);
                //}

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), accountId.ToString(), model.DisplayName);
                return Ok(accountId);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.BusinessUpdate", language, ex));
            }
        }
        #endregion

        #region Finance Methods
        [Route("FinanceTemplateGet")]
        [HttpGet]
        public IHttpActionResult FinanceTemplateGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "iD", Value = null });
                lstParameter.Add(new NameValuePair { Name = "filterText", Value = null });

                var model = new RestSharpHandler().RestRequest<List<FinanceTemplateModel>>(APISelector.Finance, "api/FinanceTemplate/FinanceTemplateGet", "GET", lstParameter, null, true, token, language, companyId).ToList();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Services.Areas.Accounts.AccountController.FinanceTemplateGet", language, ex));
            }
        }
        private List<MUNAccountGetForSupportTable_Result> AccountGetForSupportTableList(int accountID, Guid token, string language, int companyId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<List<MUNAccountGetForSupportTable_Result>>(APISelector.Finance, "api/Vendor/AccountGetForSupportTable", "GET", lstParameter, null, true, token, language, companyId).ToList();
        }
        private int SalesAccountIndividualInsertUpdate(bool isInsert, bool withAuth, Guid token, string language, int companyId, AccountIndividualModel model)
        {
            ////Note - CO-849 (HardCode data for stateid=846) for fincance issue fix
            //foreach (var item in model.AddressList)
            //{
            //    item.CountryStateID = 846;

            //    //Note - CO-1295 (HardCode data for finance issue fix - AddressTypeID = 5 (Commerce) is not in Finance Table)
            //    if (item.AddressTypeID == 5)
            //        item.AddressTypeID = 1;
            //}
            ////
            ////Note - CO-867 (HardCode data for fincance issue fix)
            //model.SalutationID = 1;
            //model.SalutationTableID = 2;
            ////

            ////Note - CO-849 (HardCode data for fincance issue fix - PhoneType = 3 (Home) is not in Finance Table)
            //foreach (var item in model.PhoneList.Where(x => x.PhoneTypeID == 3))
            //{
            //    item.PhoneTypeID = 1;
            //}
            ////

            ////Note - CO-1091 (HardCode data for fincance issue fix - Position = 5 (Legal Representative) is not in Finance Table)
            //foreach (var item in model.ContactList.Where(x => x.PositionID == 5))
            //{
            //    item.PositionID = 1;
            //}
            ////

            if (!isInsert)
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "accountId", Value = model.AccountID });
                var finModel = new RestSharpHandler().RestRequest<AccountIndividualModel>(APISelector.Finance, "api/Vendor/IndividualGet", "GET", lstParameter, null, true, token, language, companyId);

                if (finModel != null)
                {
                    model.RowVersion = finModel.RowVersion;
                }
                else
                {
                    return SalesAccountIndividualInsertUpdate(true, withAuth, token, language, companyId, model);
                }

                //if (finModel == null)
                //{
                //    foreach (var item in model.EmailList.Where(x => x.ID > 0))
                //    {
                //        item.ID = 0;
                //    }
                //    foreach (var item in model.PhoneList.Where(x => x.ID > 0))
                //    {
                //        item.ID = 0;
                //    }
                //    foreach (var item in model.AddressList.Where(x => x.ID > 0))
                //    {
                //        item.ID = 0;
                //    }
                //    foreach (var item in model.ContactList.Where(x => x.ID > 0))
                //    {
                //        item.ID = 0;
                //    }
                //    return SalesAccountIndividualInsertUpdate(true, withAuth, token, language, companyId, model);
                //}
                //else
                //    model.RowVersion = finModel.RowVersion;
            }

            //10-Jan-2020 Change

            //null for AccountID diff in collection and finance issue fix
            model.ParentID = null;
            //

            if (model.SalutationID == 0 || model.SalutationID == null)
            {
                model.SalutationID = 1;
                model.SalutationTableID = 2;
            }

            //Null all UTD (Emails,Phones,Addresses,Contacts) - for finance data mismatche issue fix (example = AddressType =  Commerce in Municipality but not in Finance)
            //CO-1295 - Address Type = Commerce
            model.EmailList = new List<AccountEmail>();
            model.PhoneList = new List<AccountPhone>();
            model.AddressList = new List<AccountAddress>();
            model.ContactList = new List<AccountContact>();
            model.FileList = new List<AccountFile>();
            //

            model.IsExempt = true;
            model.PaymentTermID = 1;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);

            if (isInsert)
            {
                return new RestSharpHandler().RestRequest<int>(APISelector.Finance, "api/Vendor/IndividualInsert", "POST", null, lstObjParameter, true, token, language, companyId);
            }
            else
            {
                return new RestSharpHandler().RestRequest<int>(APISelector.Finance, "api/Vendor/IndividualUpdate", "POST", null, lstObjParameter, true, token, language, companyId);
            }
        }
        private int SalesAccountBusinessInsertUpdate(bool isInsert, bool withAuth, Guid token, string language, int companyId, AccountBusinessModel model)
        {
            ////Note - CO-849 (HardCode data for stateid=846) for fincance issue fix
            //foreach (var item in model.AddressList)
            //{
            //    item.CountryStateID = 846;

            //    //Note - CO-1295 (HardCode data for finance issue fix - AddressTypeID = 5 (Commerce) is not in Finance Table)
            //    if (item.AddressTypeID == 5)
            //        item.AddressTypeID = 1;
            //}
            ////

            ////Note - CO-1091 (HardCode data for fincance issue fix - Position = 5 (Legal Representative) is not in Finance Table)
            //foreach (var item in model.ContactList.Where(x => x.PositionID == 5))
            //{
            //    item.PositionID = 1;
            //}
            ////

            if (!isInsert)
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "accountId", Value = model.AccountID });
                var finModel = new RestSharpHandler().RestRequest<AccountBusinessModel>(APISelector.Finance, "api/Vendor/BusinessGet", "GET", lstParameter, null, true, token, language, companyId);

                if (finModel != null)
                {
                    model.RowVersion = finModel.RowVersion;
                }
                else
                {
                    return SalesAccountBusinessInsertUpdate(true, withAuth, token, language, companyId, model);
                }

                //if (finModel == null)
                //{
                //foreach (var item in model.EmailList.Where(x => x.ID > 0))
                //{
                //    item.ID = 0;
                //}
                //foreach (var item in model.PhoneList.Where(x => x.ID > 0))
                //{
                //    item.ID = 0;
                //}
                //foreach (var item in model.AddressList.Where(x => x.ID > 0))
                //{
                //    item.ID = 0;
                //}
                //foreach (var item in model.ContactList.Where(x => x.ID > 0))
                //{
                //    item.ID = 0;
                //}
                //return SalesAccountBusinessInsertUpdate(true, withAuth, token, language, companyId, model);
                //}
                //else
                //    model.RowVersion = finModel.RowVersion;
            }

            //10-Jan-2020 Change
            //null for AccountID diff in collection and finance issue fix
            model.ParentID = null;
            //

            //Null all UTD (Emails,Phones,Addresses,Contacts) - for finance data mismatche issue fix (example = AddressType =  Commerce in Municipality but not in Finance)
            //CO-1295 - Address Type = Commerce
            model.EmailList = new List<AccountEmail>();
            model.PhoneList = new List<AccountPhone>();
            model.AddressList = new List<AccountAddress>();
            model.ContactList = new List<AccountContact>();
            model.FileList = new List<AccountFile>();
            //

            model.IsExempt = true;
            model.PaymentTermID = 1;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            if (isInsert)
            {
                return new RestSharpHandler().RestRequest<int>(APISelector.Finance, "api/Vendor/BusinessInsert", "POST", null, lstObjParameter, true, token, language, companyId);
            }
            else
            {
                return new RestSharpHandler().RestRequest<int>(APISelector.Finance, "api/Vendor/BusinessUpdate", "POST", null, lstObjParameter, true, token, language, companyId);
            }
        }
        private MUNAccountUpdate_Result SalesAccountUpdate(Guid token, string language, int companyId, MUNAccountUpdate_Result model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = model.ID });
            lstParameter.Add(new NameValuePair { Name = "active", Value = null });
            lstParameter.Add(new NameValuePair { Name = "accountTypeId", Value = null });
            var finModel = new RestSharpHandler().RestRequest<List<MUNAccountUpdate_Result>>(APISelector.Finance, "api/Vendor/AccountGet", "GET", lstParameter, null, true, token, language, companyId);

            if (finModel != null && finModel.Count > 0)
            {
                model.RowVersion = finModel[0].RowVersion;
                List<object> lstObjParameter = new List<object>();
                lstObjParameter.Add(model);
                return new RestSharpHandler().RestRequest<MUNAccountUpdate_Result>(APISelector.Finance, "api/Vendor/UpdateStatus", "POST", null, lstObjParameter, true, token, language, companyId);
            }
            return new MUNAccountUpdate_Result();
        }
        #endregion

        #region Import Account
        [Route("ValidateImportAccount")]
        [HttpPost]
        public IHttpActionResult ValidateImportAccount(AccountImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                var data = new Account().ImportAccountValidation(model, language);
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), string.Empty, "Import Account Statement");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ImportAccountValidation", language, ex));
            }
        }

        [Route("InsertImportAccount")]
        [HttpPost]
        public IHttpActionResult InsertImportAccount(AccountImportModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                int result = new Account().ImportAccountInsert(model, language);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), result.ToString(), "Import Account");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.ImportAccountInsert", language, ex));
            }
        }
        #endregion

        #region Judicial Collection

        [Route("ValidateUserPermissionForAddInJudicialCollection")]
        [HttpGet]
        public IHttpActionResult ValidateUserPermissionForAddInJudicialCollection()
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
                    Common.ValidateUserPermission(token, companyId, Guid.Parse(_blockJudicialCollectionfeatureID), Actions.New);
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Accounts.Controllers.ValidateUserPermissionForAddInJudicialCollection", language, ex));
            }
        }

        [Route("ValidateUserPermissionForRemoveFromJudicialCollection")]
        [HttpGet]
        public IHttpActionResult ValidateUserPermissionForRemoveFromJudicialCollection()
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
                    Common.ValidateUserPermission(token, companyId, Guid.Parse(_blockJudicialCollectionfeatureID), Actions.Delete);
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Accounts.Controllers.ValidateUserPermissionForRemoveFromJudicialCollection", language, ex));
            }
        }

        [Route("UpdateForJudicialCollection")]
        [HttpPost]
        public IHttpActionResult UpdateForJudicialCollection(AccountForJudicialCollectionModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                model.CompanyID = companyId;
                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);
                new Account().UpdateForJudicialCollection(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), string.Empty);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.Account.UpdateForJudicialCollection", language, ex));
            }
        }
        #endregion

        #region CreditHistory
        [Route("CreditHistoryGet")]
        [HttpGet]
        public IHttpActionResult CreditHistoryGet(int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new CreditHistory().Get(companyId,accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.CreditHistory.Get", language, ex));
            }
        }
        [Route("CreditHistoryInsert")]
        [HttpPost]
        public IHttpActionResult CreditHistoryInsert(CreditHistoryModel model)
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
                int CreditHistoryID = new CreditHistory().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), CreditHistoryID.ToString(), "CreditHistoryID");
                return Ok(CreditHistoryID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.CreditHistory.Insert", language, ex));
            }

        }
        #endregion

        #region DebitHistory
        [Route("DebitHistoryGet")]
        [HttpGet]
        public IHttpActionResult DebitHistoryGet(int accountID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new DebitHistory().Get(accountID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.DebitHistory.Get", language, ex));
            }
        }
        [Route("DebitHistoryInsert")]
        [HttpPost]
        public IHttpActionResult DebitHistoryInsert(DebitHistoryModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                int DebitHistoryID = new DebitHistory().Insert(model);
                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), DebitHistoryID.ToString(), "DebitHistoryID");
                return Ok(DebitHistoryID);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.DebitHistory.Insert", language, ex));
            }

        }
        #endregion

        #region CreditHistoryDetail 
        [Route("CreditHistoryDetailGet")]
        [HttpGet]
        public IHttpActionResult CreditHistoryDetailGet(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new CreditHistoryDetailModel().Get(ID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.CreditHistory.Get", language, ex));
            }
        }
        #endregion
        #region DebitHistoryDetail 
        [Route("DebitHistoryDetailGet")]
        [HttpGet]
        public IHttpActionResult DebitHistoryDetailGet(int ID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new DebitHistory().GetDebitHistory(ID, language);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.AccountModels.CreditHistory.Get", language, ex));
            }
        }
        #endregion
    }
}

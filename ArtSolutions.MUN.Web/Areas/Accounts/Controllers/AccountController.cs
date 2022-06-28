using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Reports.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Controllers
{
    [Filters.IsCompanyConfigured]
    public class AccountController : BaseController
    {
        bool status = false;
        string Msg = string.Empty, htmlString = string.Empty, nextHtmlString = string.Empty;

        #region Account Get

        public ActionResult List()
        {
            AccountList list = new AccountList();
            try
            {
                list = list.Get();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/Account/List.cshtml", list);
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string accountTypeID, bool? status, string filterText)
        {
            JsonResult jResult = new JsonResult();

            try
            {
                AccountList list = new AccountList().Get(accountTypeID, status, filterText, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir));
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.AccountModelList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        public JsonResult ActiveDeactive(bool isActive, int id)
        {
            try
            {
                List<FinClassGrantModel> lstGrant = new List<FinClassGrantModel>();
                if (id > 0)
                {
                    try
                    {
                        lstGrant = new FinClassGrantModel().GrantGetBySponsor(DateTime.Now, id);
                    }
                    catch
                    {

                    }
                }
                if (lstGrant.Count() > 0)
                {
                    status = false;
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Account.ValidateForInactiveAccount;
                }
                else
                {
                    AccountModel Model = new AccountModel();
                    Model.ID = Model.UpdateStatus(isActive, id).ID;
                    if (Model.ID > 0)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult View(int? accountID, int? accountType)
        {
            if (accountID == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            AccountModel model = new AccountModel();
            //CreditBalance
            CreditHistoryModel crBalance = new CreditHistoryModel();
            //DebitBalance
            DebitHistoryModel debitBalance = new DebitHistoryModel();
            try
            {
                model = model.View(accountID.Value, accountType.Value);
                model.CreditBalance = crBalance.Get(accountID.Value).CreditBalance;
                model.DebitBalance = debitBalance.Get(accountID.Value).DebitBalance;
                
                if (model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Account.InvalidAccountNumber;
                    return RedirectToAction("List");
                }

                return View("~/Areas/Accounts/Views/Account/View.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        #region ExportAccount
        public ActionResult ExportAccountList(int exportType)
        {
            string htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/Account/Export/_AccountListExport.cshtml",
                new AccountListExport().GetAccountListExport());
            ExportFileModel exportFileViewModel = new ExportModel().GenerateExportFileA3((ExportType)exportType, ArtSolutions.MUN.Web.Resources.Account.AccountList, htmlString, true);
            return File(exportFileViewModel.Data, System.Net.Mime.MediaTypeNames.Application.Octet, exportFileViewModel.FileName);
        }
        #endregion
        #endregion

        #region Account Add/Edit

        [HttpGet]
        public ActionResult Add(int? accountType, int? accountId = null, int? parentAccountType = null)
        {
            if (accountType == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            AccountModel model = new AccountModel();
            try
            {
                model = model.Get(accountType.Value, accountId.HasValue ? accountId : null);
                TempData["ParentAccountType"] = parentAccountType;
                return View("~/Areas/Accounts/Views/Account/Add.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Add(AccountModel model, int actionType)
        {
            try
            {
                #region Check for duplicate registration info

                var errorMsg = model.GetAccountForExist(model.AccountTypeID, model.TaxNumber, model.TreasuryNumber, model.StateNumber, null);

                if (errorMsg != null)
                {
                    status = false;
                    Msg = errorMsg;
                    throw new Exception(Msg);
                }
                #endregion

                int result = model.Insert(model);
                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            TempData["ParentAccountType"] = TempData["ParentAccountType"];
            return Json(new { status = status, message = Msg, actionType = actionType, parentAccountId = model.ParentID, parentAccountType = TempData["ParentAccountType"] }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int? accountID, int? accountType, int? parentAccountType = null)
        {
            if (accountID == null || accountType == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            try
            {
                AccountModel model = new AccountModel();
                model = model.Edit(accountID.Value, accountType.Value);

                if (model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Account.InvalidAccountNumber;
                    return RedirectToAction("List");
                }
                TempData["ParentAccountType"] = parentAccountType;
                return View("~/Areas/Accounts/Views/Account/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Edit(AccountModel model)
        {
            try
            {
                #region Check for duplicate registration info
                var errorMsg = model.GetAccountForExist(model.AccountTypeID, model.TaxNumber, model.TreasuryNumber, model.StateNumber, model.AccountID);

                if (errorMsg != null)
                {
                    status = false;
                    Msg = errorMsg;
                    throw new Exception(Msg);
                }
                #endregion

                //// Code commented for SE-85 issue [Contact to Admin error due to User dont have right for Finance]
                //#region Check for user have grants during disable the sponsor option
                //if (!model.IsSponsor)
                //{
                //    List<FinClassGrantModel> lstGrant = new List<FinClassGrantModel>();
                //    if (model.AccountID > 0)
                //    {
                //        try
                //        {
                //            lstGrant = new FinClassGrantModel().GrantGetBySponsor(DateTime.Now, model.AccountID);
                //        }
                //        catch
                //        {

                //        }                        
                //    }
                //    if (lstGrant.Count() > 0)
                //    {
                //        status = false;
                //        Msg = ArtSolutions.MUN.Web.Resources.Account.ValidateForInactiveSponsor;
                //        throw new Exception(Msg);
                //    }
                //}
                //#endregion

                model.ID = model.Update(model);
                if (model.ID > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            TempData["ParentAccountType"] = TempData["ParentAccountType"];
            return Json(new { status = status, message = Msg, parentAccountId = model.ParentID, parentAccountType = TempData["ParentAccountType"] }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Email       
        public ActionResult NewEmail()
        {
            EmailModel model = new EmailModel();
            try
            {
                model = model.Get();
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_Email.cshtml", model);
        }
        public PartialViewResult AddEmail(List<EmailModel> emailList)
        {
            return PartialView("~/Areas/Accounts/Views/Account/_EmailList.cshtml", emailList);
        }
        #endregion

        #region Phone
        public ActionResult NewPhone(int AccountTypeID)
        {
            PhoneModel model = new PhoneModel();
            try
            {
                model = model.Get(AccountTypeID);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_Phone.cshtml", model);
        }
        public PartialViewResult AddPhone(List<PhoneModel> phoneList)
        {
            return PartialView("~/Areas/Accounts/Views/Account/_PhoneList.cshtml", phoneList);
        }
        #endregion

        #region Address      
        public ActionResult NewAddress()
        {
            AddressModel model = new AddressModel();
            try
            {
                model = model.Get();
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_Address.cshtml", model);
        }
        public PartialViewResult AddAddress(List<AddressModel> addressList)
        {
            return PartialView("~/Areas/Accounts/Views/Account/_AddressList.cshtml", addressList);
        }

        [HttpGet]
        public ActionResult DeleteAddress(int ID, int accountID)
        {
            try
            {

                int result = new AddressModel().GetAddressesForExist(ID, accountID);
                if (result > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Contact
        public ActionResult NewContact()
        {
            ContactsModel model = new ContactsModel();
            try
            {
                model = model.Get(null);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_Contacts.cshtml", model);
        }
        public PartialViewResult AddContact(List<ContactsModel> contactList)
        {
            return PartialView("~/Areas/Accounts/Views/Account/_ContactsList.cshtml", contactList);
        }
        public ActionResult ViewContact(int? id)
        {
            ContactsModel model = new ContactsModel();
            try
            {
                model = model.Get(id);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_Contacts.cshtml", model);
        }

        [HttpGet]
        public ActionResult DeleteContact(int ID, int accountID)
        {
            try
            {

                int result = new ContactsModel().GetContactsForExist(ID, accountID);
                if (result > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Attachement/File
        public PartialViewResult AddAttachment(List<AttachmentModel> attachmentList)
        {
            return PartialView("~/Areas/Accounts/Views/Account/_AttachmentsFilesList.cshtml", attachmentList);
        }
        public PartialViewResult AddAttchmentRow(int attachmentID, string attachmentName)
        {
            AttachmentModel model = new AttachmentModel();
            model.FileID = attachmentID;
            model.Name = attachmentName;
            model = model.GetTypes();
            return PartialView("~/Areas/Accounts/Views/Account/_AttachmentRow.cshtml", model);
        }
        #endregion

        #region Ajax Calls
        public JsonResult GetStatesByCountry(int countryId)
        {
            return Json(new AddressModel().GetStates(countryId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCitiesByCountryAndState(int countryId, int stateId)
        {
            return Json(new AddressModel().GetCities(countryId, stateId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTowndByCountryStateAndCity(int countryId, int stateId, int cityId)
        {
            return Json(new AddressModel().GetTowns(countryId, stateId, cityId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountForSearch(string accountTypeIDs, int? accountID, string searchText, bool? isActive, int pageIndex, int pageSize)
        {
            return Json(new AccountListForSearch().Get(accountTypeIDs, accountID, searchText, isActive, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        //Test
        public ActionResult GetBusinessAccounts(int? parentId, string filterText)
        {
            AccountModel model = new AccountModel();
            try
            {
                model.BusinessList = new AccountBusinessModel().Get(null, parentId, filterText);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Areas/Accounts/Views/Account/_BusinessList.cshtml", model);
        }
        //
        #endregion

        #region ImportAccount

        [HttpGet]
        public ActionResult ImportAccount(int accountType)
        {
            ImportAccountFieldModel model = new ImportAccountFieldModel();
            model.type = accountType;
            return View("~/Areas/Accounts/Views/ImportAccount/ImportAccount.cshtml", model);
        }

        [HttpPost]
        public ActionResult UploadFile(ImportAccountFieldModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
            }

            if (model.UploadFile == null || model.UploadFile.ContentLength <= 0)
            {
                return Json(new { status = status, message = Resources.ImportAccount.ValSelectFile }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                string filepath, extension;
                extension = System.IO.Path.GetExtension(model.UploadFile.FileName).ToLower();
                if (!Common.ImportFileTypes.Contains(extension))
                {
                    return Json(new { status = status, message = string.Format(Resources.ImportAccount.ValFileFormat, string.Join(", ", Common.ImportFileTypes)), data = "" }, JsonRequestBehavior.AllowGet);
                }

                model.FileName = string.Format("{0}_{1}{2}", System.IO.Path.GetFileNameWithoutExtension(model.UploadFile.FileName).Trim().Replace(" ", ""), UserSession.Current.CompanyID.ToString(), extension);

                //GET THE COMPLETE FOLDER PATH AND STORE THE FILE INSIDE IT.
                string path = Server.MapPath(Common.ImportFilePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                filepath = System.IO.Path.Combine(path, model.FileName);
                model.UploadFile.SaveAs(filepath);

                DataTable importDataTable = ReadImportFile(model.FileName, true);

                if (importDataTable == null)
                {
                    status = false;
                    return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
                }

                AccountMappingViewModel accountMappingModel = new AccountMappingViewModel(model.FileName, model.type, importDataTable);
                nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccount/_ColumnMapping.cshtml", accountMappingModel);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, data = htmlString, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ColumnMapping(AccountMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    ValidAccountModel validImportmodel = new ValidAccountModel(importDataTable, model);

                    if (validImportmodel.ImportList != null)
                    {
                        InvalidAccountModel _model = new InvalidAccountModel();
                        _model.FileName = model.FileName;
                        _model.AccType = model.type;
                        _model.ValidAccountStatement = new ImportAccountModel().ImportAccountValid(validImportmodel);
                        _model.Valid = (_model.ValidAccountStatement.Count == 0 || _model.ValidAccountStatement == null);

                        status = true;
                        nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccount/_ImportValidation.cshtml", _model);
                    }
                    else
                    {
                        status = false;
                        Msg = Resources.Account.ImportAccountFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    Msg = Resources.Account.ImportAccountFailedMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportValidation(AccountMappingViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    FinishViewAccountModel finishViewAccountModel = new FinishViewAccountModel(importDataTable, model);

                    status = true;
                    nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccount/_FinishImport.cshtml", finishViewAccountModel);
                }
                else
                {
                    status = false;
                    Msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, nextStepData = nextHtmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FinishImport(AccountMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);

                if (importDataTable != null)
                {
                    ValidAccountModel validImportmodel = new ValidAccountModel(importDataTable, model)
                    {
                        IsValidate = false
                    };

                    int result = new ImportAccountModel().ImportAccountInsert(validImportmodel);
                    if (result == 2)
                    {
                        status = true;
                        Msg = Resources.Global.SavedSuccessfullyMessage;
                        TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;

                        ClearImportFile(model.FileName);
                    }
                    else
                    {
                        status = false;
                        Msg = Resources.Account.ImportAccountFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    Msg = Resources.Global.ValNull;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, nextStepData = nextHtmlString, Acctype = 1 }, JsonRequestBehavior.AllowGet);
        }

        #region Common Import
        private DataTable ReadImportFile(string fileName, bool isFirstTime)
        {
            DataTable importDataTable = new DataTable();
            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportAccountModel.S_ImportAccountData]))))
            {
                importDataTable = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportAccountModel.S_ImportAccountData]));
            }
            else
            {
                //Read File From The Folder
                string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
                string extension = System.IO.Path.GetExtension(path).ToLower();

                importDataTable = new ImportModel().GetDataTableFromImportFile(path, extension);

                Session[ImportAccountModel.S_ImportAccountData] = JsonConvert.SerializeObject(importDataTable);
            }
            return importDataTable;
        }

        private void ClearImportFile(string fileName)
        {
            //REMOVE FILE FROM SESSION
            Session.Remove(ImportAccountModel.S_ImportAccountData);

            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        #endregion

        #endregion

        #region Judicial Collection
        public JsonResult UpdateForJudicialCollection(AccountModel model)
        {
            string msg = string.Empty;

            try
            {
                status = new AccountModel().UpdateForJudicialCollection(model);

                if (status)
                {
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
                else
                {
                    throw new Exception(ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage);
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult AddTemporaryAccountProperty(int AccountID)
        {
            string msg = string.Empty;

            try
            {
                status = new AccountProperty().InsertTemporary(AccountID) > 0;

                if (status)
                {
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
                else
                {
                    throw new Exception(ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage);
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        #region CreditHistory
        public ActionResult CreditHistory(int accountID)
        {
            CreditHistoryModel model = new CreditHistoryModel();
            List<CreditHistoryModel> lst = new List<CreditHistoryModel>();
            try
            {
                model = model.Get(accountID);
                decimal totalAmount = model.CreditHistoryModelList.Where(x => x.PaymentID == null).Sum(a => a.CreditAmount);
                decimal debitAmount = model.CreditHistoryModelList.Where(a => a.PaymentID != null).Sum(a => a.CreditAmount);
                model.CreditBalance = totalAmount - debitAmount;
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/Account/_CreditHistory.cshtml", model);
        }
        [HttpPost]
        public ActionResult CreditHistory(CreditHistoryModel model)
        {
            string Message = "";
            bool Status = false;
            int CreditHistoryID = 0;
            try
            {
                CreditHistoryID = new CreditHistoryModel().Insert(model);
                Message = Resources.Account.CreditHistorySuccessMsg;
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;
            }
            return Json(new { status = Status, message = Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DebitHistory
        public ActionResult DebitHistory(int accountID)
        {
            DebitHistoryModel model = new DebitHistoryModel();
            List<DebitHistoryModel> lst = new List<DebitHistoryModel>();
            try
            {
                model = model.Get(accountID);
                decimal totalAmount = model.DebitHistoryModelList.Sum(a => a.DebitAmount);
                decimal debitAmount = model.DebitHistoryModelList.Where(a => a.PaymentID != null && a.IsPaid==true).Sum(a => a.DebitAmount);
                model.DebitBalance = totalAmount - debitAmount;
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Accounts/Views/Account/_DebitHistory.cshtml", model);
        }
        [HttpPost]
        public ActionResult DebitHistory(DebitHistoryModel model)
        {
            string Message = "";
            bool Status = false;
            int DebitHistoryID = 0;
            try
            {
                DebitHistoryID = new DebitHistoryModel().Insert(model);
                Message = Resources.Account.DebitHistorySuccessMsg;
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
                Message = ex.Message;
            }
            return Json(new { status = Status, message = Message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
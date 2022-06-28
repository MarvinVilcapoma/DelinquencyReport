using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Controllers
{
    [Filters.IsCompanyConfigured]
    public class AccountPropertyController : BaseController
    {
        AccountPropertyModel Model = new AccountPropertyModel();
        bool status = false;
        string Msg = string.Empty, htmlString = string.Empty, nextHtmlString = string.Empty;

        #region Account Property Get / View / Status Update
        public ActionResult List()
        {
            AccountPropertyList list = new AccountPropertyList();            
            return View("~/Areas/Accounts/Views/AccountProperty/List.cshtml", list);
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText, int? accountID = null)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                AccountPropertyList model = new AccountPropertyList().Get(filterText, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir), accountID);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.AccountPropertyModelList });
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
                Model.ID = new AccountProperty().UpdateStatus(isActive, id);
                if (Model.ID > 0)
                    status = true;

            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AccountPropertyHasDebt(int id, int accountID)
        {
            bool hasDebt = false;
            try
            {
                hasDebt = new AccountProperty().HasDebt(id, accountID);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new { status = status, message = Msg, HasDebt = hasDebt }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult View(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {

                Model = new AccountProperty().View(ID.Value);
                if (Model.ID == 0)
                {
                    TempData["ErrorMsg"] = Resources.AccountProperty.InvalidAccountProperty;
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }

            return View("~/Areas/Accounts/Views/AccountProperty/View.cshtml", Model);
        }

        public JsonResult GetAccountPropertyForSearch(string searchText, int pageIndex, int pageSize)
        {
            return Json(new AccountPropertyListForSearch().Get(searchText, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountPropertyRightForSearch(string searchText, int pageIndex, int pageSize)
        {
            return Json(new AccountPropertyListForSearch().GetRight(searchText, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Account Property Add/Edit
        [HttpGet]
        public ActionResult Add(int? ownerID)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = new AccountProperty().Get(ownerID);
            return View("~/Areas/Accounts/Views/AccountProperty/Add.cshtml", model);
        }
        [HttpPost]
        public ActionResult Add(AccountPropertyModel model, int actionType)
        {
            int AccountPropertyID = 0;
            DateTime outDate;

            try
            {
                DateTime dateRangeFrom = new DateTime(1753, 1, 1, 12, 00, 00);
                DateTime dateToFrom = new DateTime(9999, 12, 31, 11, 59, 59);

                bool isValidDateTime = false;
                if (DateTime.TryParse(Request["hfDateOfMovement"], out outDate))
                    isValidDateTime = true;

                DateTime hfDateOfMovement = new DateTime();
                if (isValidDateTime)
                    hfDateOfMovement = DateTime.Parse(Request["hfDateOfMovement"]);

                if (
                        string.IsNullOrEmpty(Request["hfDateOfMovement"])
                            ||
                        ((isValidDateTime == true) && (hfDateOfMovement >= dateRangeFrom && hfDateOfMovement <= dateToFrom))
                 )
                {
                    AccountPropertyID = new AccountProperty().Insert(model);

                    if (AccountPropertyID > 0)
                    {
                        status = true;
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                    }
                }
                else
                {
                    status = false;
                    Msg = ArtSolutions.MUN.Web.Resources.Global.WrongDateValidationMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }

            return Json(new { status = status, actionType = actionType, ID = AccountPropertyID, ownerID = model.OwnerID, message = Msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                ViewBag.IsCopyMode = false;
                Model = new AccountProperty().Edit(ID.Value);

                if (Model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.AccountProperty.InvalidAccountProperty;
                    return RedirectToAction("List");
                }
                return View("~/Areas/Accounts/Views/AccountProperty/Edit.cshtml", Model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Edit(AccountPropertyModel model, int actionType)
        {
            DateTime outDate;

            try
            {
                DateTime dateRangeFrom = new DateTime(1753, 1, 1, 12, 00, 00);
                DateTime dateToFrom = new DateTime(9999, 12, 31, 11, 59, 59);

                bool isValidDateTime = false;
                if (DateTime.TryParse(Request["hfDateOfMovement"], out outDate))
                    isValidDateTime = true;

                DateTime hfDateOfMovement = new DateTime();
                if (isValidDateTime)
                    hfDateOfMovement = DateTime.Parse(Request["hfDateOfMovement"]);

                if (
                        string.IsNullOrEmpty(Request["hfDateOfMovement"])
                            ||
                        ((isValidDateTime == true) && (hfDateOfMovement >= dateRangeFrom && hfDateOfMovement <= dateToFrom))
                 )
                {
                    int result = 0;
                    if (actionType == 2) // Copy                   
                        result = new AccountProperty().Insert(model);
                    else
                        result = new AccountProperty().Update(model);

                    if (result > 0)
                    {
                        status = true;
                        TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                    }
                }
                else
                {
                    status = false;
                    Msg = ArtSolutions.MUN.Web.Resources.Global.WrongDateValidationMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }

            return Json(new { status = status, message = Msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Copy(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                ViewBag.IsCopyMode = true;
                Model = new AccountProperty().Edit(ID.Value);

                if (Model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.AccountProperty.InvalidAccountProperty;
                    return RedirectToAction("List");
                }
                return View("~/Areas/Accounts/Views/AccountProperty/Edit.cshtml", Model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public JsonResult Delete(int id, bool? isWindowReload = null)
        {
            int result = 0;
            string msg = "";
            try
            {
                result = new AccountProperty().Delete(id);

                if (result > 0)
                {
                    status = true;
                    msg = ArtSolutions.MUN.Web.Resources.AccountProperty.DeleteAccountPropertySuccMsg;
                }
                if (isWindowReload == true)
                    TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                TempData["ErrorMsg"] = msg;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion        

        #region Ajax Call

        public ActionResult NewGeneralDescriptionConstruction()
        {
            AccountPropertyConstructionDetailModel model = new AccountPropertyConstructionDetailModel();
            model = new AccountPropertyConstructionDetail().Get();
            return PartialView("~/Areas/Accounts/Views/AccountProperty/_GeneralDescriptionConstruction.cshtml", model);
        }
        public ActionResult AddRights()
        {
            AccountPropertyModel model = new AccountPropertyModel();
            return PartialView("~/Areas/Accounts/Views/AccountProperty/_AddEditRights.cshtml", model);
        }

        public ActionResult GetAccount(string searchTerm, string page_limit)
        {
            var data = new AccountModel().Get(null, 0, null, null, null, searchTerm, true).OrderBy(x => x.DisplayName).Select(d => new { text = d.TaxNumber + " - " + d.DisplayName, id = d.ID.ToString() }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFillingPropertyTaxHistory(int PropertyAccountID, string PropertyNumber, string RightNumber, string TimeZone)
        {
            string msg = "";
            try
            {
                List<AccountPropertyModel> model = new List<AccountPropertyModel>();
                model = new AccountProperty().GetFillingHistory(PropertyAccountID, PropertyNumber, RightNumber, TimeZone);
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountProperty/_FillingPropertyTaxHistory.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountProperty(int id)
        {
            return Json(new AccountProperty().Get(id, true), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AccountPropertyRightUpdate(int id, string PropertyNumber, string RigthNumber, Nullable<int> CondoID, Nullable<int> CondoTypeID)
        {
            int result = 0;
            string msg = "";
            try
            {
                result = new AccountProperty().AccountPropertyRightUpdate(id, PropertyNumber, RigthNumber, CondoID, CondoTypeID);

                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }                
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;               
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountPropertyRightHistory(int id)
        {
            string msg = "";
            try
            {
                List<MUNAccountPropertyRightHistoryModel> model = new List<MUNAccountPropertyRightHistoryModel>();
                model = new AccountProperty().GetAccountPropertyRightHistory(id);
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/AccountProperty/_AccountPropertyRightHistory.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Import Account Property

        [HttpGet]
        public ActionResult ImportAccountProperty()
        {
            ImportAccountPropertyFieldModel model = new ImportAccountPropertyFieldModel();
            return View("~/Areas/Accounts/Views/ImportAccountProperty/ImportAccountProperty.cshtml", model);
        }

        [HttpPost]
        public ActionResult UploadFile(ImportAccountPropertyFieldModel model)
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
                    System.IO.Directory.CreateDirectory(path);
                filepath = System.IO.Path.Combine(path, model.FileName);
                model.UploadFile.SaveAs(filepath);

                DataTable importDataTable = ReadImportFile(model.FileName, true);

                if (importDataTable == null)
                {
                    status = false;
                    return Json(new { status = status, message = Resources.Global.ValNull }, JsonRequestBehavior.AllowGet);
                }

                AccountPropertyMappingViewModel accountPropertyMappingModel = new AccountPropertyMappingViewModel(model.FileName, model.type, importDataTable);
                nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountProperty/_ColumnMapping.cshtml", accountPropertyMappingModel);
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
        public ActionResult ColumnMapping(AccountPropertyMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);
                if (importDataTable != null)
                {
                    ValidAccountPropertyModel validImportmodel = new ValidAccountPropertyModel(importDataTable, model);

                    if (validImportmodel.ImportViewAccountPropertyDetailWithErrorModel != null)
                    {
                        InvalidAccountPropertyModel _model = new InvalidAccountPropertyModel();
                        _model.FileName = model.FileName;
                        _model.ValidAccountPropertyStatement = new ImportAccountPropertyModel().ImportAccountPropertyValid(validImportmodel);

                        //Concate Error List
                        if (validImportmodel.ImportViewAccountPropertyDetailWithErrorModel.ErrorList != null)
                            _model.ValidAccountPropertyStatement = _model.ValidAccountPropertyStatement.Concat(validImportmodel.ImportViewAccountPropertyDetailWithErrorModel.ErrorList).OrderBy(x => x.GroupCode).ToList();
                        //

                        _model.Valid = (_model.ValidAccountPropertyStatement == null || _model.ValidAccountPropertyStatement.Count == 0);

                        status = true;
                        nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountProperty/_ImportValidation.cshtml", _model);
                    }
                    else
                    {
                        status = false;
                        Msg = Resources.AccountProperty.ImportAccountPropertyFailedMsg;
                    }
                }
                else
                {
                    status = false;
                    Msg = Resources.AccountProperty.ImportAccountPropertyFailedMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
            }
            return Json(new
            {
                status = status,
                message = Msg,
                nextStepData = nextHtmlString
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportValidation(AccountPropertyMappingViewModel model)
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
                    FinishViewAccountPropertyModel finishViewAccountPropertyModel = new FinishViewAccountPropertyModel(importDataTable, model);

                    status = true;
                    nextHtmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Accounts/Views/ImportAccountProperty/_FinishImport.cshtml", finishViewAccountPropertyModel);
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
        public ActionResult FinishImport(AccountPropertyMappingViewModel model)
        {
            try
            {
                DataTable importDataTable = ReadImportFile(model.FileName, false);

                if (importDataTable != null)
                {
                    ValidAccountPropertyModel validImportmodel = new ValidAccountPropertyModel(importDataTable, model)
                    {
                        IsValidate = false
                    };

                    int result = new ImportAccountPropertyModel().ImportAccountPropertyInsert(validImportmodel);
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
                        Msg = Resources.AccountProperty.ImportAccountPropertyFailedMsg;
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
            if (!isFirstTime && (!string.IsNullOrWhiteSpace(Convert.ToString(Session[ImportAccountPropertyModel.S_ImportAccountPropertyData]))))
            {
                importDataTable = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(Session[ImportAccountPropertyModel.S_ImportAccountPropertyData]));
            }
            else
            {
                //Read File From The Folder
                string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
                string extension = System.IO.Path.GetExtension(path).ToLower();

                importDataTable = new ImportModel().GetDataTableFromImportFile(path, extension);

                Session[ImportAccountPropertyModel.S_ImportAccountPropertyData] = JsonConvert.SerializeObject(importDataTable);
            }
            return importDataTable;
        }
        private void ClearImportFile(string fileName)
        {
            //REMOVE FILE FROM SESSION
            Session.Remove(ImportAccountPropertyModel.S_ImportAccountPropertyData);

            //DELETE SAVED FILE
            string path = System.IO.Path.Combine(Server.MapPath(Common.ImportFilePath), fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        #endregion

        #endregion
    }
}
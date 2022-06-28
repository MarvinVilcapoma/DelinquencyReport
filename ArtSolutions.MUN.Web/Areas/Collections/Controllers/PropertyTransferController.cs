using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ArtSolutions.MUN.Web.Common;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class PropertyTransferController : BaseController
    {
        int documentTypeID = (int)DocumentTypeEnum.PropertyTransfer;

        #region List Property Transfer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText)
        {
            JsonResult jResult = new JsonResult();
            DateTime outDate;
            try
            {
                if (DateTime.TryParse(filterText, out outDate))
                {
                    try
                    {
                        filterText = Convert.ToDateTime(filterText).ToString("d", CultureInfo.InvariantCulture);
                    }
                    catch { }
                }

                PropertyTransferListModel list = new PropertyTransfer().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.PropertyTransferList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region New Property Transfer
        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                List<WorkflowViewModel> WorkFlowList = new WorkFlowModel().Get(0, documentTypeID);
                ViewBag.WorkFlowList = WorkFlowList;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(WorkFlowList[0].ID, 0, 1);
                ViewBag.CondoList = new AccountProperty().GetSupportValues(19).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                ViewBag.CondoTypeList = new AccountProperty().GetSupportValues(20).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                PropertyTransferModel model = new PropertyTransferModel();
                model.TransferTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(42), "ID", "Name");
                return View("Add", model);

                throw new Exception(Resources.Global.GeneralErrorMessage);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PropertyTransferModel model)
        {
            bool status = false;
            string ErrorMsg = "";
            int TransferID = 0;
            JsonResult jsonResult = new JsonResult();
            try
            {
                if (model == null)
                {
                    status = false;
                    ErrorMsg = Resources.Global.ValNull;
                    return Json(new { status = status, message = ErrorMsg, data = "" }, JsonRequestBehavior.AllowGet);
                }


                TransferID = new PropertyTransfer().Insert(model);


                if (TransferID > 0)
                {
                    status = true;
                    ErrorMsg = Resources.Global.SavedSuccessfullyMessage;
                    TempData["SuccessMsg"] = ErrorMsg;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            return Json(new { status = status, message = ErrorMsg, data = "", transferID = TransferID }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountService(int accountId, int AccountPropertyID, int transferTypeID)
        {
            try
            {
                return PartialView("~/Areas/Collections/Views/PropertyTransfer/_AddList.cshtml", new PropertyTransfer().GetAllAccountService(accountId, AccountPropertyID, transferTypeID));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult View(int ID)
        {
            if (ID == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            PropertyTransferModel model = new PropertyTransferModel();
            try
            {
                model = new PropertyTransfer().GetPropertyTransfer(ID);
                model.workflowStatusID = model.workflowStatusID;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }
        public ActionResult Update(int ID, int WorkFlowStatusID, bool Ispost = false)
        {
            bool Status = false;
            string Message = "";

            if (ID <= 0 || WorkFlowStatusID <= 0)
            {
                Message = Resources.Global.NoDataMessage;
                return Json(new { status = false, message = Message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                int Retval = new PropertyTransfer().Update(ID, WorkFlowStatusID, Ispost);
                if (Retval == 0)
                {
                    Message = Resources.Global.InvalidAction;
                }
                else if (Retval == 1)
                {
                    Status = true;
                    Message = Resources.Global.SavedSuccessfullyMessage;
                }
                else
                {
                    Message = Resources.Global.ValInternalError;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            return Json(new { status = Status, message = Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetServicePeriodDetail(int AccountServiceID)
        {
            string msg = "";
            bool status;
            string htmlString = "";
            try
            {
                List<AccountServiceFillingModel> model = new List<AccountServiceFillingModel>();
                model = new AccountServiceFillingModel().Get(AccountServiceID, null, null);
                status = true;
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/PropertyTransfer/_ServicePeriodDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlString }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Print(int Id)
        {
            if (Id == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                PropertyTransferModel model = new PropertyTransferModel();
                try
                {
                    model = new PropertyTransfer().GetPropertyTransfer(Id);
                    model.workflowStatusID = model.workflowStatusID;
                    ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = ex.Message;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("Index");
        }
        #endregion

        #region Account Property Split
        [HttpGet]
        public ActionResult NewSplit()
        {
            List<WorkflowViewModel> WorkFlowList = new WorkFlowModel().Get(0, (int)DocumentTypeEnum.SplitRight);
            ViewBag.WorkFlowList = WorkFlowList;
            ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(WorkFlowList[0].ID, 0, 1);
            ViewBag.CondoList = new AccountProperty().GetSupportValues(19).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            ViewBag.CondoTypeList = new AccountProperty().GetSupportValues(20).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            AccountPropertyModel model = new AccountPropertyModel();
            model = new AccountProperty().Get(null);
            return View("~/Areas/Collections/Views/PropertyTransfer/NewSplit.cshtml", model);
        }

        [HttpPost]
        public ActionResult NewSplit(AccountPropertyModel model)
        {
            int TransferID = 0;
            bool status = false;
            string Msg = string.Empty;
            try
            {
                TransferID = new AccountProperty().Split(model);

                if (TransferID > 0)
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

            return Json(new { status = status, ID = TransferID, ownerID = model.OwnerID, message = Msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSplit(int ID)
        {
            List<WorkflowViewModel> WorkFlowList = new WorkFlowModel().Get(0, (int)DocumentTypeEnum.SplitRight);
            PropertyTransferModel model = new PropertyTransferModel();
            model = new PropertyTransfer().GetPropertyTransfer(ID);
            ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(8, model.workflowStatusID, 2);
            ViewBag.CondoList = new AccountProperty().GetSupportValues(19).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            ViewBag.CondoTypeList = new AccountProperty().GetSupportValues(20).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.OldAccountProperty = new AccountProperty().GetAccountPropertyDetail(model.AccountPropertyID.Value, ID);
            model.OldAccountProperty.AccountPropertyConstructionDetailJson = new JavaScriptSerializer().Serialize(model.TransferAccountDetailList.Select(d => new { RigthNumber = d.RigthNumber, OwnerID = d.OwnerID, AccountPropertyID = d.ID }).Distinct().ToList());
            model.TransferID = ID;
            model.AccountServiceList = new PropertyTransfer().GetAllAccountService(model.OldAccountID, model.AccountPropertyID.Value, 2, ID);
            return View("~/Areas/Collections/Views/PropertyTransfer/EditSplit.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditSplit(PropertyTransferModel model)
        {
            int TransferID = 0;
            bool status = false;
            string Msg = string.Empty;
            try
            {
                TransferID = new AccountProperty().EditSplit(model);

                if (TransferID > 0)
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

            return Json(new { status = status, ID = TransferID, message = Msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSplit(int id)
        {
            bool status = false;
            string msg = string.Empty;
            try
            {
                new AccountProperty().DeleteSplit(id);               
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.AccountProperty.VoidAccountPropertySplitSuccMsg;

            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                TempData["ErrorMsg"] = msg;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddComment(int? WorkflowId, int? statusId)
        {
            bool status = false;
            string Htmlpage = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                PropertyTransferModel model = new PropertyTransferModel();
                if (!WorkflowId.HasValue || WorkflowId.Value <= 0 || !statusId.HasValue || statusId.Value <= 0)
                {
                    TempData["ErrorMsg"] = Resources.Global.NoDataMessage;
                    return RedirectToActionPermanent("List");
                }
                else
                {
                    model = new PropertyTransfer().InitNewComment(statusId.Value, WorkflowId.Value);
                    //model.StatuIdSource = statusId.Value;
                    Htmlpage = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/PropertyTransfer/_Comment.cshtml", model);
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
                Errormsg = ex.Message;
            }
            return Json(new { status = status, message = Errormsg, RedirectUrl = Htmlpage }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DocumentWorkflowHistoryLogGetForSplit(int transferID)
        {
            JsonResult jResult = new JsonResult();
            string msg = string.Empty;
            bool status = true;
            try
            {
                var result = new AccountProperty().DocumentWorkflowHistoryLogGetForSplit(transferID);               

                jResult = Json(new { status = status, Message = msg, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                status = false;
                jResult = Json(new { status = status, Message = msg, data = "" }, JsonRequestBehavior.AllowGet);
            }

            return jResult;
        }

        [HttpGet]
        public ActionResult GetAccountPropertyDetail(int ID)
        {
            AccountPropertyModel Model = new AccountPropertyModel();
            bool status = false;
            string Msg = string.Empty;
            try
            {
                ViewBag.IsCopyMode = false;
                Model = new AccountProperty().GetAccountPropertyDetail(ID);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;
                return Json(new { status = status, message = Msg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = status, message = Msg, FincaID = Model.FincaID, PropertyNumber = Model.PropertyNumber, Condo = Model.Condo, CondoType = Model.CondoType, CondoID = Model.CondoID, CondoTypeID = Model.CondoTypeID,  Owner = Model.Owner, RigthNumber = Model.RigthNumber, OwnerID = Model.OwnerID, Percentage = Model.Percentage, TerrainValue = Model.TerrainValue, Model.TotalTerrainValue, Area = Model.Area, RigthCodes = Model.RigthCodes, AccountPropertyConstructionDetailJson = Model.AccountPropertyConstructionDetailJson, TotalValue = Model.TotalValue, CurrentValueFromPropertyTax = Model.CurrentValueFromPropertyTax }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountServiceForSplit(int accountId, int AccountPropertyID, int WorkflowStatusID, int transferTypeID)
        {
            try
            {
                ViewBag.WorkflowStatusID = WorkflowStatusID;
                return PartialView("~/Areas/Accounts/Views/AccountProperty/_ServiceList.cshtml", new PropertyTransfer().GetAllAccountService(accountId, AccountPropertyID, transferTypeID, 0));
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewSplit(int ID)
        {
            if (ID == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            PropertyTransferModel model = new PropertyTransferModel();
            try
            {
                model = new PropertyTransfer().GetPropertyTransfer(ID);
                model.workflowStatusID = model.workflowStatusID;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SplitPrint(int Id)
        {
            if (Id == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                PropertyTransferModel model = new PropertyTransferModel();
                try
                {
                    model = new PropertyTransfer().GetPropertyTransfer(Id);
                    model.workflowStatusID = model.workflowStatusID;
                    ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = ex.Message;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("Index");
        }        

        #endregion

        #region Account Property Merge
        [HttpGet]
        public ActionResult NewMerge()
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = new AccountProperty().Get(null);
            //model.AccountPropertyList = HMTLHelperExtensions.CreateSelectList(new AccountProperty().GetAccountPropertyRight(), "AccountPropertyID", "Property");
            return View("~/Areas/Collections/Views/PropertyTransfer/NewMerge.cshtml", model);
        }

        [HttpPost]
        public ActionResult NewMerge(AccountPropertyModel model, int actionType)
        {
            int TransferID = 0;
            bool status = false;
            string Msg = string.Empty;
            try
            {
                TransferID = new AccountProperty().Merge(model);

                if (TransferID > 0)
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

            return Json(new { status = status, actionType = actionType, ID = TransferID, ownerID = model.OwnerID, message = Msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewMerge(int ID)
        {
            if (ID == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            PropertyTransferModel model = new PropertyTransferModel();
            try
            {
                model = new PropertyTransfer().GetPropertyTransfer(ID);
                model.workflowStatusID = model.workflowStatusID;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult MergePrint(int Id)
        {
            if (Id == 0)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            try
            {
                PropertyTransferModel model = new PropertyTransferModel();
                try
                {
                    model = new PropertyTransfer().GetPropertyTransfer(Id);
                    model.workflowStatusID = model.workflowStatusID;
                    ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(7, model.workflowStatusID, 2);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = ex.Message;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToActionPermanent("Index");
        }
        public ActionResult GetAccountServiceForMerge(int AccountPropertyID, int transferTypeID, string commaSeparatedPropertyID)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            bool status = false;
            string msg = string.Empty;
            string htmlstring = string.Empty;

            try
            {
                model = new AccountProperty().GetAccountPropertyOnly(AccountPropertyID);
                htmlstring = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/PropertyTransfer/_ServiceMergeList.cshtml", new PropertyTransfer().GetAllAccountService(model.OwnerID, AccountPropertyID, transferTypeID));
                status = true;
            }
            catch (Exception ex)
            {

                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, returnData = htmlstring, isNotAssociatedServices = new AccountProperty().GetMergeCheckNotAssociatedServices(commaSeparatedPropertyID) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult isNotAssociatedServices(string commaSeparatedPropertyID)
        {
            bool status = false;
            string msg = string.Empty;
            int? isNotAssociatedServices = null;

            try
            {
                isNotAssociatedServices = new AccountProperty().GetMergeCheckNotAssociatedServices(commaSeparatedPropertyID);
                status = true;
            }
            catch (Exception ex)
            {

                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, isNotAssociatedServices = isNotAssociatedServices }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult isNotAssociatedServicesByAccount(int AccountID)
        {
            bool status = false;
            string msg = string.Empty;
            int? isNotAssociatedServices = null;

            try
            {
                isNotAssociatedServices = new AccountProperty().GetMergeCheckNotAssociatedServices(null, AccountID);
                status = true;
            }
            catch (Exception ex)
            {

                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, isNotAssociatedServices = isNotAssociatedServices }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountPropertyForRightDetail(int ID)
        {
            AccountPropertyModel Model = new AccountPropertyModel();
            bool status = false;
            string Msg = string.Empty;
            try
            {
                ViewBag.IsCopyMode = false;
                Model = new AccountProperty().GetAccountPropertyDetail(ID);
                return PartialView("~/Areas/Collections/Views/PropertyTransfer/_PropertyRights.cshtml", Model);
            }
            catch (Exception ex)
            {
                status = false;
                Msg = ex.Message;

            }
            return Json(new { status = status, message = Msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
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
using static ArtSolutions.MUN.Web.Common;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class PostingProcessController : BaseController
    {
        int documentTypeID = (int)DocumentTypeEnum.PostingProcess;

        #region Posting Process Get
        public ActionResult Index()
        {
            ViewBag.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText, string DocumentTypeID)
        {
            JsonResult jResult = new JsonResult();

            try
            {
                PostingProcessListModel list = new PostingProcess().GetWithPaging(jqdtParams, filterText, DocumentTypeID);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.PostingProcessList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }

        [HttpGet]
        public ActionResult GetJournalListForPosting(DateTime? AsOfDate, string DocumentTypeID)
        {
            try
            {
                List<JournalListForPosting> model = new List<JournalListForPosting>();

                if (AsOfDate.HasValue)
                    model = new PostingProcess().GetJournalListForPosting(AsOfDate.Value, DocumentTypeID);

                string HtmlView = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/PostingProcess/_AddList.cshtml", model);
                return Json(new { status = true, data = HtmlView }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Posting Process Insert/Update/View
        [HttpGet]
        public ActionResult Add()
        {
            List<WorkflowViewModel> WorkFlowList = new WorkFlowModel().Get(0, documentTypeID);
            ViewBag.WorkFlowList = WorkFlowList;
            ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(WorkFlowList[0].ID, 0, 1);
            ViewBag.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(string JournalID, string DocumentTypeDetail, DateTime AsOfDate, int workflowID, int workflowStatusID)
        {
            bool status = false;
            string ErrorMsg = "";
            int PostingProcessID = 0;
            JsonResult jsonResult = new JsonResult();
            try
            {
                if (string.IsNullOrWhiteSpace(JournalID))
                {
                    status = false;
                    ErrorMsg = Resources.Global.ValNull;
                    return Json(new { status = status, message = ErrorMsg }, JsonRequestBehavior.AllowGet);
                }

                PostingProcessID = new PostingProcess().Insert(JournalID, DocumentTypeDetail, AsOfDate, workflowID, workflowStatusID);
                if (PostingProcessID > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            return Json(new { status = status, message = ErrorMsg, data = "", postingProcessID = PostingProcessID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            ViewBag.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            List<JournalListForPosting> model = new List<JournalListForPosting>();
            try
            {
                model = new PostingProcess().Get(ID.Value);
                if (model == null || model.Count == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }

                ViewBag.WorkflowStatusID = model.FirstOrDefault().WorkflowStatusID ?? 0;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(2, model.FirstOrDefault().WorkflowStatusID ?? 0, 2);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Collections/Views/PostingProcess/View.cshtml", model);
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
                int Retval = new PostingProcess().Update(ID, WorkFlowStatusID, Ispost);
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
        #endregion

        #region NewPost

        public ActionResult List()
        {
            ViewBag.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return View();
        }
        public ActionResult NewPost(int? id = null)
        {
            try
            {
                NewPostingProcessModel model = new NewPostingProcessModel();
                List<WorkflowViewModel> WorkFlowList = new WorkFlowModel().Get(0, documentTypeID);
                model.PaymentReceiptTypeList = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                var lastPostingProcessDetails = new PostingProcess().LatInsertedGet(1);
                if (lastPostingProcessDetails != null)
                {
                    model.LastAsOfDate = lastPostingProcessDetails.AsOfDate;
                }
                if (id.HasValue)
                {

                    //test
                    model.JournalForPostingList = new PostingProcess().Get(id.Value);
                    //

                    //var postingDetails = new PostingProcess().Get(id.Value).FirstOrDefault();

                    //if (postingDetails != null)
                    if (model.JournalForPostingList != null)
                    {
                        var postingDetails = model.JournalForPostingList.FirstOrDefault();
                        if (postingDetails != null)
                        {
                            model.workflowStatusID = postingDetails.WorkflowStatusID.Value;
                            model.PaymentReceiptType = postingDetails.PaymentType;
                            model.AsOfDate = postingDetails.AsOfDate;
                            model.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(2, postingDetails.WorkflowStatusID ?? 0, 2);

                            //model.JournalIds = Convert.ToString(postingDetails.ID);
                        }
                    }
                    model.NewPostedList = new PostingProcess().GetWithGroupSum(id.Value);
                }
                else
                {

                    model.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(WorkFlowList[0].ID, 0, 1);
                }
                model.WorkFlowList = WorkFlowList;
                model.ID = id;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public JsonResult NewPost(NewPostingProcessModel model)
        {
            bool result = false;
            string Message = "";
            var htmlString = "";
            try
            {
                var newPostingProcessModel = new PostingProcess().NewPostingProcessBulkJournalInsert(model);
                htmlString = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Collections/Views/PostingProcess/_NewPostList.cshtml", newPostingProcessModel);
                result = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(new { result = result, message = Message, data = htmlString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddNewPost(NewPostingProcessModel model)
        {
            bool status = false;
            string ErrorMsg = "";
            int PostingProcessID = 0;
            JsonResult jsonResult = new JsonResult();
            try
            {
                if (string.IsNullOrWhiteSpace(model.JournalIds))
                {
                    status = false;
                    ErrorMsg = Resources.Global.ValNull;
                    return Json(new { status = status, message = ErrorMsg }, JsonRequestBehavior.AllowGet);
                }

                PostingProcessID = new PostingProcess().Insert(model.JournalIds, "Municipality Posting Process", model.AsOfDate, model.workflowID, model.workflowStatusID, model.PaymentReceiptType);
                if (PostingProcessID > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                ErrorMsg = ex.Message;
            }
            return Json(new { status = status, message = ErrorMsg, data = "", postingProcessID = PostingProcessID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewNewPost(int? ID)
        {
            if (ID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            ViewBag.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            List<JournalListForPosting> model = new List<JournalListForPosting>();
            try
            {
                model = new PostingProcess().Get(ID.Value);
                if (model == null || model.Count == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                    return RedirectToAction("Index");
                }

                ViewBag.WorkflowStatusID = model.FirstOrDefault().WorkflowStatusID ?? 0;
                ViewBag.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(2, model.FirstOrDefault().WorkflowStatusID ?? 0, 2);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Collections/Views/PostingProcess/View.cshtml", model);
        }

        public JsonResult GetLastAsOfDate(int PaymentRecieptType)
        {
            var lastPostingProcessDetails = new PostingProcess().LatInsertedGet(PaymentRecieptType);
            string LastAsOfDate = lastPostingProcessDetails == null ? "-" : lastPostingProcessDetails.FormattedAsOfDate;
            return Json(new { data = LastAsOfDate }, JsonRequestBehavior.AllowGet);
        }
        #endregion 
    }
}
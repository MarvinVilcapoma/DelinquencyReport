using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Controllers
{
    public class WorkflowReasonController : BaseController
    {
        // GET: Workflows/WorkflowReason
        [HttpGet]
        public ActionResult List(Boolean IsBack = false)
        {
            if (IsBack)
            {
                ViewBag.IsBack = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, WorkflowReasonViewModel model)
        {
            model = new WorkflowReasonModel().Get(jqdtParams, model);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.ReasonList });
        }
        public ActionResult Get(int id, int actionType, int workflowID, bool isEditor = false)
        {
            var model = new WorkflowReasonModel().Get(id, actionType, workflowID);
            ViewBag.ActionType = actionType;
            if (isEditor)
            {
                return PartialView("_AddReason", model);
            }
            return View("Add", model);
        }
        [HttpPost]
        public ActionResult Add(WorkflowReasonViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkflowReasonModel().Update(model) > 0;
                response.Message = response.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult GetByWorkflowID(JQDTParams jqdtParams, WorkflowReasonViewModel model)
        {
            model = new WorkflowReasonModel().GetByWorkflowID(jqdtParams, model);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.ReasonList });
        }
    }
}
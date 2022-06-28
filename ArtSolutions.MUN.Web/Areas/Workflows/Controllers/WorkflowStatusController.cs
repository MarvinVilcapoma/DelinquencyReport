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
    public class WorkflowStatusController : BaseController
    {
        // GET: Workflows/WorkflowStatus
        public ActionResult Get(int id, int actionType, string name = "", int workflowID = 0)
        {
            var model = new WorkflowStatusModel().Get(id, actionType, name, workflowID);
            ViewBag.ActionType = actionType;
            return PartialView("_AddStatus", model);
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, int workflowID)
        {
            var model = new WorkflowStatusModel().Get(jqdtParams, new WorkflowStatusViewModel
            {
                WorkflowID = workflowID
            });
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.StatusList });
        }

        [HttpPost]
        public ActionResult Add(WorkflowStatusViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                if (model.ID == 0)
                    response.Status = new WorkflowStatusModel().Insert(model) > 0;
                else
                    response.Status = new WorkflowStatusModel().Update(model) > 0;
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
        public ActionResult SequenceUpdate(WorkflowStatusViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkflowStatusModel().SequenceUpdate(model) > 0;
                response.Message = response.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
    }
}
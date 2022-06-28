using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Controllers
{
    public class WorkflowGroupController : BaseController
    {
        // GET: Workflows/WorkflowGroup
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
        public ActionResult List(JQDTParams jqdtParams, WorkflowSecurityGroupViewModel groupModel)
        {
            var model = new WorkflowSecurityGroupModel().Get(jqdtParams, groupModel);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.SecurityGroupList });
        }
        public ActionResult Get(int id, int actionType)
        {
            var model = new WorkflowSecurityGroupModel().Get(id, actionType);
            ViewBag.ActionType = actionType;
            return View("Add", model);
        }
        [HttpGet]
        public ActionResult Add(WorkflowSecurityGroupViewModel model, int actionType, bool IsBack = false)
        {
            if (IsBack)
            {
                ViewBag.IsBack = true;
            }
            ViewBag.ActionType = actionType;
            return View(model);
        }
        [HttpPost]
        public ActionResult Add(WorkflowSecurityGroupViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                if (model.ID == 0)
                    response.Status = new WorkflowSecurityGroupModel().Insert(model) > 0;
                else
                    response.Status = new WorkflowSecurityGroupModel().Update(model) > 0;
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
        public ActionResult GetByWorkflowID(JQDTParams jqdtParams, WorkflowSecurityGroupViewModel groupModel)
        {
            var model = new WorkflowSecurityGroupModel().GetByWorkflowID(jqdtParams, groupModel);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.SecurityGroupList });
        }
        public ActionResult UserAdd(int id, int workflowID, int documentTypeID)
        {
            var model = new WorkflowSecurityGroupUsersModel().UserGet(id, workflowID, documentTypeID);
            return View(model);
        }
        [HttpPost]
        public ActionResult UserAdd(WorkflowSecurityGroupUsersViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkflowSecurityGroupUsersModel().Insert(model) > 0;
                response.Message = response.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult UserGet(JQDTParams jqdtParams, WorkflowSecurityGroupUsersViewModel model)
        {
            model = new WorkflowSecurityGroupUsersModel().Get(jqdtParams, model);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.SecurityUserList });
        }
    }
}
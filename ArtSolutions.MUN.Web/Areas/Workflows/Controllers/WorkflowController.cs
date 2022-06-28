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
    public class WorkflowController : BaseController
    {
        // GET: Workflows/Workflow
        public ActionResult List(Boolean IsBack = false)
        {
            if (IsBack)
            {
                ViewBag.IsBack = true;
            }
            var model = new WorkflowViewModel
            {
                DocumentTypeList = new DocumentTypeModel().Get()
            };
            model.DocumentTypeList.Insert(0, new DocumentTypeViewModel
            {
                ID = 0,
                Name = Resources.Global.All
            });
            return View(model);
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, WorkflowViewModel model, Boolean IsBack = false)
        {
            model = new WorkFlowModel().Get(jqdtParams, model);
            return Json(new { recordsFiltered = model.TotalRecord, recordsTotal = model.TotalRecord, data = model.WorkflowList });
        }
        public ActionResult Active(WorkflowViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkFlowModel().ActiveStatusUpdate(model) > 0;
                response.Message = response.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Add()
        {
            var model = new WorkflowViewModel
            {
                DocumentTypeList = new DocumentTypeModel().Get(),
                WorkflowReasonList = new WorkflowReasonModel().Get()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Add(WorkflowViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkFlowModel().Insert(model) > 0;
                response.Message = response.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Editor(int? id, int? documentTypeID)
        {
            if (id == null || documentTypeID == null)
                return HttpNotFound();
            var model = new WorkFlowModel().Editor(id.Value, documentTypeID.Value);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }

        public ActionResult Diagram(int? id, int? documentTypeID)
        {
            if (id == null || documentTypeID == null)
                return HttpNotFound();
            var model = new WorkFlowModel().Editor(id.Value, documentTypeID.Value);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }
        [HttpPost]
        public ActionResult DiagramGenerator(WorkflowViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            DiagramViewModel viewModel = new DiagramViewModel();
            try
            {
                viewModel = new DiagramGeneratorModel().Get(model);
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return Json(new { response, viewModel }, JsonRequestBehavior.AllowGet);
        }
    }
}
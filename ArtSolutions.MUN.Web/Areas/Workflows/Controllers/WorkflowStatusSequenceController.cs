using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Controllers
{
    public class WorkflowStatusSequenceController : BaseController
    {
        // GET: Workflows/WorkflowStatusSequence
        [HttpGet]
        public ActionResult Get(int? workflowID, int? id)
        {
            if (workflowID == null || id == null)
            {
                return HttpNotFound();
            }
            var model = new WorkflowStatusSequenceModel().Get(workflowID.Value, id.Value);
            return PartialView("_AddStatusSequence", model);
        }
        [HttpPost]
        public ActionResult Add(WorkflowStatusSequenceViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkflowStatusSequenceModel().Insert(model) > 0;
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
        public ActionResult Delete(WorkflowStatusSequenceViewModel model)
        {
            ResponceMessage response = new ResponceMessage();
            try
            {
                response.Status = new WorkflowStatusSequenceModel().Delete(model) > 0;
                response.Message = response.Status ? Resources.Global.DeletedMessage : string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, WorkflowStatusSequenceViewModel model)
        {
            var result = new WorkflowStatusSequenceModel().GetByPaging(model.WorkflowID, model.WorkflowVersionID);
            return Json(new { recordsFiltered = result.Count, recordsTotal = result.Count, data = result });
        }
    }
}
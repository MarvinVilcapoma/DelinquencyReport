using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Controllers
{
    public class WorkflowFormController : BaseController
    {
        // GET: Workflows/WorkflowForm
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, CaseWorkflowForm model)
        {
            model = new CaseWorkflowForm().Get(jqdtParams, model);
            return Json(new { recordsFiltered = model.TotalRecords, recordsTotal = model.TotalRecords, data = model.WorkflowFormList });
        }
    }
}
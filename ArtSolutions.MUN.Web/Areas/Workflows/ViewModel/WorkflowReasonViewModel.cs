using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowReasonViewModel: CommonViewModel
    {
        public int ID { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public List<WorkflowReasonViewModel> ReasonList { get; set; } = new List<WorkflowReasonViewModel>();
    }
}
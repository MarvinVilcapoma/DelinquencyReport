using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowSecurityGroupViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public int WorkFlowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public List<WorkflowSecurityGroupViewModel> SecurityGroupList { get; set; } = new List<WorkflowSecurityGroupViewModel>();
    }
}
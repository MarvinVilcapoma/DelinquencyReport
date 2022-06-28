using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityGroupViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public int WorkFlowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<WorkflowSecurityGroupViewModel> SecurityGroupList { get; set; } = new List<WorkflowSecurityGroupViewModel>();
    }
}

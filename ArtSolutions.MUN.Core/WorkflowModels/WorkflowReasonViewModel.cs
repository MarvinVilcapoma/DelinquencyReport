using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowReasonViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public List<WorkflowReasonViewModel> ReasonList { get; set; } = new List<WorkflowReasonViewModel>();
    }
}

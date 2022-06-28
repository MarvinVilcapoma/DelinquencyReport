using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowStatusViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int WorkflowID { get; set; }
        public string WorkflowVersionID { get; set; }
        public bool InitialStatus { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool Post { get; set; }
        public bool Void { get; set; }
        public bool IsPartial { get; set; }
        public bool IsFull { get; set; }
        public string Activity { get; set; }
        public string Reasons { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Groups { get; set; }
        public string Sequence { get; set; }
        public List<WorkflowStatusViewModel> StatusList { get; set; } = new List<WorkflowStatusViewModel>();
    }
}

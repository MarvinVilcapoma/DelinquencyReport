using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowStatusSequenceViewModel
    {
        public int WorkflowStatusID { get; set; }
        public int WorkflowStatusTargetID { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public int? FormID { get; set; }
        public int CompanyID { get; set; }
        public string Language { get; set; }
        public string TargetStatus { get; set; }
        public string TargetStatusActivity { get; set; }
        public string Form { get; set; }
        public string Sequence { get; set; }
        public string Groups { get; set; }
        public List<WorkflowStatusSequenceViewModel> WorkflowStatusSequenceList { get; set; } = new List<WorkflowStatusSequenceViewModel>();
    }
}

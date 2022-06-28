

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityStatusInGroupViewModel : CommonViewModel
    {
        public int StatusID { get; set; }
        public int GroupID { get; set; }
        public string Status { get; set; }
        public string Activity { get; set; }
        public string Group { get; set; }
    }
}

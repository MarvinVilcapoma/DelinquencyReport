using System;
namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class WorkflowHistoryLog
    {
        public int CompanyID { get; set; }
        public int CaseID { get; set; }
        public int StatuIdSource { get; set; }
        public int StatuIdTarget { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; } = 1;
        public int ReasonID { get; set; }
        public string Comment { get; set; }
        public Guid AssignToID { get; set; } = Guid.Empty;
        public string StatuIdTargetName { get; set; }
        public string ReasonName { get; set; }
    }
}
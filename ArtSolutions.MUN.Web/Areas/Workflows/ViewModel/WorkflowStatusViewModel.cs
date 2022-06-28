using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowStatusViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int WorkflowID { get; set; }
        public string WorkflowVersionID { get; set; }
        public int WorkflowVersion { get; set; }
        public bool InitialStatus { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool Post { get; set; }
        public bool Void { get; set; }
        public bool IsPartial { get; set; }
        public bool IsFull { get; set; }
        public string Activity { get; set; }
        public int? FormID { get; set; }
        public string Groups { get; set; }
        public string Reasons { get; set; }
        public string Sequence { get; set; }
        public List<WorkflowReasonViewModel> WorkflowReasonList { get; set; } = new List<WorkflowReasonViewModel>();
        public List<CaseWorkflowStatusReason> WorkflowStatusReasonList { get; set; } = new List<CaseWorkflowStatusReason>();
        public List<WorkflowStatusViewModel> StatusList { get; set; } = new List<WorkflowStatusViewModel>();
        public List<WorkflowSecurityGroupViewModel> GroupList { get; set; } = new List<WorkflowSecurityGroupViewModel>();
        public List<WorkflowSecurityStatusInGroupViewModel> StatusInGroupList { get; set; } = new List<WorkflowSecurityStatusInGroupViewModel>();
    }
    public class WorkflowHistoryLog
    {
        public int CompanyID { get; set; }        
        public int StatuIdSource { get; set; }
        public int StatuIdTarget { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; } = 1;
        public int ReasonID { get; set; }
        public string Comment { get; set; }
        public Guid AssignToID { get; set; } = Guid.Empty;
        public string StatuIdTargetName { get; set; }
    }
}
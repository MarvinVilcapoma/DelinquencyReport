using ArtSolutions.MUN.Web.Areas.Cases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowStatusSequenceViewModel
    {
        public int WorkflowStatusID { get; set; }
        public int WorkflowStatusTargetID { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public List<WorkflowStatusViewModel> TargetStatusList { get; set; } = new List<WorkflowStatusViewModel>();
        public int? FormID { get; set; }
        public string TargetStatus { get; set; }
        public string TargetStatusActivity { get; set; }
        public string Form { get; set; }
        public string FromStatus { get; set; }
        public string Sequence { get; set; }
        public int GroupID { get; set; }
        public string Groups { get; set; }
        public List<WorkflowSecurityGroupViewModel> SecurityGroupList { get; set; } = new List<WorkflowSecurityGroupViewModel>();
        public List<WorkflowStatusSequenceViewModel> WorkflowStatusSequenceList { get; set; } = new List<WorkflowStatusSequenceViewModel>();
        public List<CaseWorkflowForm> WorkflowFormList { get; set; } = new List<CaseWorkflowForm>();
    }
}
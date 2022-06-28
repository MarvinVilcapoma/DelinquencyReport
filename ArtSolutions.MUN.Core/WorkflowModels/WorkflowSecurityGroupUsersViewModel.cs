using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityGroupUsersViewModel : CommonViewModel
    {
        public string DisplayName { get; set; }
        public Guid UserID { get; set; }
        public string Groups { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public int ID { get; set; }
        public List<WorkflowSecurityGroupUsersViewModel> SecurityUserList { get; set; } = new List<WorkflowSecurityGroupUsersViewModel>();
    }
}

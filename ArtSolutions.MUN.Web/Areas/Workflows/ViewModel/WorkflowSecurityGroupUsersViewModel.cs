using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowSecurityGroupUsersViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public int WorkFlowID { get; set; }
        public int WorkflowVersionID { get; set; }
        public int DocumentTypeID { get; set; }
        public string DisplayName { get; set; }
        public Guid UserID { get; set; }
        public string Groups { get; set; }
        public List<WorkflowSecurityGroupUsersViewModel> SecurityUserList { get; set; } = new List<WorkflowSecurityGroupUsersViewModel>();
        public List<UserDataModel> Users { get; set; } = new List<UserDataModel>();
        public int GroupID { get; set; }
    }
}
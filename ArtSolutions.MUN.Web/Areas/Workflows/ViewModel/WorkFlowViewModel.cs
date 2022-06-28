using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class WorkflowViewModel: CommonViewModel
    {
        #region "Properties"
        public int ID { get; set; }
        public int Version { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentType { get; set; }
        public List<WorkflowViewModel> WorkflowList { get; set; } = new List<WorkflowViewModel>();
        public List<DocumentTypeViewModel> DocumentTypeList { get; set; } = new List<DocumentTypeViewModel>();
        public WorkflowStatusViewModel WorkflowStatusViewModel { get; set; } = new WorkflowStatusViewModel();
        public List<WorkflowStatusViewModel> WorkflowStatusList { get; set; } = new List<WorkflowStatusViewModel>();
        public List<WorkflowReasonViewModel> WorkflowReasonList { get; set; } = new List<WorkflowReasonViewModel>();
        #endregion
    }
}
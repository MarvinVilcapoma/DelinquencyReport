using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DocumentTypeID { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public List<MUNDOCDocumentWorkflowGetByPaging_Result> WorkflowList { get; set; } = new List<MUNDOCDocumentWorkflowGetByPaging_Result>();
        public List<WorkflowStatusViewModel> WorkflowStatusList { get; set; } = new List<WorkflowStatusViewModel>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowFormViewModel : CommonViewModel
    {
        public int ID { get; set; }
        public string URLEdit { get; set; }
        public string URLNew { get; set; }
        public string URLPrint { get; set; }
        public string URLView { get; set; }
        public bool UsePopup { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MUNDOCDocumentWorkflowFormGetByPaging_Result> WorkflowFormList { get; set; } = new List<MUNDOCDocumentWorkflowFormGetByPaging_Result>();
    }
}

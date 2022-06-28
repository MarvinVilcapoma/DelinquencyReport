using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityStatusInGroupModel
    {
        public List<MUNDOCDocumentWorkflowSecurityStatusInGroupGet_Result> Get(WorkflowSecurityStatusInGroupViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowSecurityStatusInGroupGet(model.StatusID, model.GroupID,
                    model.Language).ToList();
            }
        }
    }
}

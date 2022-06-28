using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowFormModel
    {
        public WorkflowFormViewModel Get(WorkflowFormViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.WorkflowFormList = context.MUNDOCDocumentWorkflowFormGetByPaging(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.SortColumn,
                    model.Sortby.ToString(), model.FilterText).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
            }
            return model;
        }
    }
}

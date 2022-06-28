using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowReasonModel
    {
        public List<MUNDOCDocumentWorkflowReasonGet_Result> Get(int companyID, string language, int id)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowReasonGet(companyID, language, id).ToList();
            }
        }
        public WorkflowReasonViewModel Get(WorkflowReasonViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.ReasonList = context.MUNDOCDocumentWorkflowReasonGetByPaging(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(),
                    model.WorkflowID, model.FilterText).Select(a => new WorkflowReasonViewModel
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Description = a.Description
                    }).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
                return model;
            }
        }
        public int Update(WorkflowReasonViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowReasonUpdate(model.CompanyID, model.Name, model.Description, model.CreatedUserID,
                    model.CreatedDate, model.ID, model.Language);
            }
        }
        public WorkflowReasonViewModel GetByWorkflowID(WorkflowReasonViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.ReasonList = context.MUNDOCDocumentWorkflowReasonGetByWorkflowID(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(),
                    model.WorkflowID, model.WorkflowVersionID).Select(a => new WorkflowReasonViewModel
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Description = a.Description,
                        IsPublic = a.IsPublic
                    }).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
                return model;
            }
        }

        public List<MUNDOCDocumentWorkflowStatusReasonGet_Result> StatusReasonGet(int wokrlflowID, string version,
            string language, int statusID, int reasonID)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusReasonGet(wokrlflowID, version, language, statusID, reasonID).ToList();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityGroupModel
    {
        public List<MUNDOCDocumentWorkflowSecurityGroupGet_Result> Get(int companyID, string language, int id)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowSecurityGroupGet(companyID, language, id).ToList();
            }
        }
        public WorkflowSecurityGroupViewModel Get(WorkflowSecurityGroupViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.SecurityGroupList = context.MUNDOCDocumentWorkflowSecurityGroupGetByPaging(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(), model.FilterText).
                    Select(a => new WorkflowSecurityGroupViewModel
                    {
                        Name = a.Group,
                        ID = a.GroupId,
                        Description = a.Description,
                        IsPublic = a.IsPublic
                    }).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
                return model;
            }
        }
        public int Insert(WorkflowSecurityGroupViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowSecurityGroupInsert(model.CompanyID, model.Name,
                    model.Description, model.CreatedUserID, model.CreatedDate);
            }
        }
        public int Update(WorkflowSecurityGroupViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowSecurityGroupUpdate(model.ID, model.CompanyID, model.Name,
                    model.Description, model.CreatedUserID, model.CreatedDate, model.Language);
            }
        }
        public WorkflowSecurityGroupViewModel GetByWorkflowID(WorkflowSecurityGroupViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.SecurityGroupList = context.MUNDOCDocumentWorkflowSecurityGroupGetByWorkflowID(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(),
                    model.WorkFlowID, model.WorkflowVersionID).Select(a => new WorkflowSecurityGroupViewModel
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

    }
}

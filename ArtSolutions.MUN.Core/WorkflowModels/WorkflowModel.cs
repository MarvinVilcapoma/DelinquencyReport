using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowModel
    {
        public WorkflowViewModel Get(WorkflowViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.WorkflowList = context.MUNDOCDocumentWorkflowGetByPaging(model.CompanyID, model.Language, model.PageIndex,
                    model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(), model.FilterText, model.DocumentTypeID).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
            }
            return model;
        }
        public int ActiveStatusUpdate(WorkflowViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowActiveStatusUpdate(model.ID, model.CompanyID,
                    model.IsActive, model.CreatedUserID, model.CreatedDate);
            }
        }
        public int Insert(WorkflowViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                DataTable dataTable = new DataTable();
                if (model.WorkflowStatusList != null && model.WorkflowStatusList.Any())
                {
                    dataTable = model.WorkflowStatusList.Select(a => new
                    {
                        a.Name,
                        a.Activity,
                        Description = a.Description ?? "",
                        a.InitialStatus,
                        a.AllowEdit,
                        a.AllowDelete,
                        a.Post,
                        a.Void,
                        a.IsFull,
                        a.IsPartial,
                        a.Reasons,
                        Groups = a.Groups ?? ""
                    }).ToDataTable();
                }

                SqlParameter tableVar = new SqlParameter("@TableVar", SqlDbType.Structured)
                {
                    Value = dataTable,
                    TypeName = "[dbo].[MUNDOCDocumentWorkflowStatusUTD]"
                };
                SqlParameter[] parms = {
                    new SqlParameter("@CompanyID",model.CompanyID),
                    new SqlParameter("@DocumentTypeID",model.DocumentTypeID),
                    new SqlParameter("@Name",model.Name),
                    new SqlParameter("@Description",(model.Description ?? "")),
                    new SqlParameter("@CreatedUserID",model.CreatedUserID),
                    new SqlParameter("@CreatedDate",model.CreatedDate),
                    tableVar
                };
                return context.ExecuteSqlProcedure(parms, "MUNDOCDocumentWorkflowInsert");
            }
        }

        public List<MUNDOCDocumentWorkflowHistoryLogGetForSplit_Result> DocumentWorkflowHistoryLogGetForSplit(int companyID, int TransferID, string language)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowHistoryLogGetForSplit( TransferID, language).ToList();
            }
        }
    }
}

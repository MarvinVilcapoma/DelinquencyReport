using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowStatusModel
    {
        public WorkflowStatusViewModel Get(WorkflowStatusViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.StatusList = context.MUNDOCDocumentWorkflowStatusGetByPaging(model.CompanyID, model.Language, model.PageIndex,
                    model.PageSize, objectParameter, model.SortColumn, model.Sortby.ToString(),
                    model.WorkflowID).Select(a => new WorkflowStatusViewModel
                    {
                        Name = a.Name,
                        Activity = a.Activity,
                        Description = a.Description,
                        AllowDelete = a.AllowDelete,
                        AllowEdit = a.AllowEdit,
                        InitialStatus = a.InitialStatus,
                        Post = a.Post,
                        Void = a.Void,
                        ID = a.ID,
                        Sequence = a.Sequence,
                        Reasons = a.Reasons
                    }).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
            }
            return model;
        }
        public int Insert(WorkflowStatusViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                DataTable dataTable = new DataTable();
                List<WorkflowStatusViewModel> statusList = new List<WorkflowStatusViewModel> {
                    model
                };
                if (statusList != null && statusList.Any())
                {
                    dataTable = statusList.Select(a => new
                    {
                        a.Name,
                        a.Activity,
                        a.Description,
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
                    new SqlParameter("@WorkflowID",model.WorkflowID),
                    new SqlParameter("@WorkflowVersion",model.WorkflowVersionID),
                    new SqlParameter("@CreatedUserID",model.CreatedUserID),
                    new SqlParameter("@CreatedDate",model.CreatedDate),
                    tableVar
                };
                return context.ExecuteSqlProcedure(parms, "MUNDOCDocumentWorkflowStatusInsert");
            }
        }
        public int Update(WorkflowStatusViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusUpdate(model.ID, model.CompanyID, model.WorkflowID, Convert.ToInt32(model.WorkflowVersionID),
                    model.CreatedUserID, model.CreatedDate, model.Name, model.Activity, model.Description, model.InitialStatus,
                    model.AllowEdit, model.AllowDelete, model.Post, model.Void, model.Groups, model.Language, model.Reasons);
            }
        }
        public int SequenceUpdate(WorkflowStatusViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                DataTable dataTable = new DataTable();
                if (model.StatusList != null && model.StatusList.Any())
                {
                    dataTable = model.StatusList.Select(a => new
                    {
                        WorkflowStatusTargetID = a.ID,
                        FormID = a.SequenceID,
                        Groups = ""
                    }).ToDataTable();
                }
                SqlParameter tableVar = new SqlParameter("@TableVar", SqlDbType.Structured)
                {
                    Value = dataTable,
                    TypeName = "[dbo].[MUNDOCDocumentWorkflowStatusSequenceUTD]"
                };
                SqlParameter[] parms = {
                    new SqlParameter("@CompanyID",model.CompanyID),
                    new SqlParameter("@CreatedUserID",model.CreatedUserID),
                    new SqlParameter("@CreatedDate",model.CreatedDate),
                    new SqlParameter("@WorkflowID",model.WorkflowID),
                    new SqlParameter("@WorkflowVersion",model.WorkflowVersionID),
                    tableVar
                };
                return context.ExecuteSqlProcedure(parms, "MUNDOCDocumentWorkflowStatusSequenceUpdate");
            }
        }
        public List<MUNDOCDocumentWorkflowStatusGet_Result> WorkflowStatusGet(WorkflowStatusViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusGet(model.WorkflowID, model.CompanyID, model.WorkflowVersionID, model.Language, model.IsPublic, model.IsDeleted, model.ID).ToList();
            }
        }
       
    }
}

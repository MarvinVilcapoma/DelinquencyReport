using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowStatusSequenceModel
    {
        public int Insert(WorkflowStatusSequenceViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                DataTable dataTable = new DataTable();
                if (model.WorkflowStatusSequenceList != null && model.WorkflowStatusSequenceList.Any())
                {
                    dataTable = model.WorkflowStatusSequenceList.Select(a => new
                    {
                        a.WorkflowStatusTargetID,
                        a.FormID,
                        a.Groups
                    }).ToDataTable();
                }
                SqlParameter tableVar = new SqlParameter("@TableVar", SqlDbType.Structured)
                {
                    Value = dataTable,
                    TypeName = "[dbo].[MUNDOCDocumentWorkflowStatusSequenceUTD]"
                };
                SqlParameter[] parms = {
                    new SqlParameter("@CompanyID",model.CompanyID),
                    new SqlParameter("@WorkflowID",model.WorkflowID),
                    new SqlParameter("@WorkflowVersion",model.WorkflowVersionID),
                    new SqlParameter("@WorkflowStatusID",model.WorkflowStatusID),
                    tableVar
                };
                return context.ExecuteSqlProcedure(parms, "MUNDOCDocumentWorkflowStatusSequenceInsert");
            }
        }
        public List<WorkflowStatusSequenceViewModel> Get(WorkflowStatusSequenceViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusSequenceGet(model.CompanyID, model.Language, model.WorkflowID,
                    model.WorkflowVersionID, model.WorkflowStatusID).Select(a => new WorkflowStatusSequenceViewModel
                    {
                        WorkflowStatusID = a.WorkflowStatusID,
                        WorkflowStatusTargetID = a.WorkflowStatusTargetID,
                        WorkflowID = a.WorkflowID,
                        WorkflowVersionID = Convert.ToInt32(a.WorkflowVersionID),
                        FormID = a.FormID,
                        TargetStatus = a.TargetStatus,
                        TargetStatusActivity = a.TargetStatusActivity,
                        Form = a.Form,
                        Groups = a.Groups
                    }).ToList();
            }
        }
        public int Delete(WorkflowStatusSequenceViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusSequenceDelete(model.WorkflowStatusID, model.WorkflowID,
                    model.WorkflowVersionID, model.WorkflowStatusTargetID, model.CompanyID);
            }
        }
        public List<MUNDOCDocumentWorkflowStatusSequenceGetByPaging_Result> GetByPaging(WorkflowStatusSequenceViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusSequenceGetByPaging(model.CompanyID,
                    model.Language, model.WorkflowID, model.WorkflowVersionID).ToList();
            }
        }

    }
}

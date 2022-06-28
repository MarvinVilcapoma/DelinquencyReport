using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class WorkflowSecurityGroupUsersModel
    {
        public WorkflowSecurityGroupUsersViewModel GetByPaging(WorkflowSecurityGroupUsersViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                ObjectParameter objectParameter = new ObjectParameter("TotalRecord", typeof(int));
                model.SecurityUserList = context.MUNDOCDocumentWorkflowSecurityGroupUsersGetByPaging(model.CompanyID, model.Language,
                    model.PageIndex, model.PageSize, objectParameter, model.WorkflowID, model.WorkflowVersionID, model.SortColumn, model.Sortby.ToString()).
                    Select(a => new WorkflowSecurityGroupUsersViewModel
                    {
                        DisplayName = a.DisplayName,
                        UserID = a.UserID.Value,
                        Groups = a.Groups
                    }).ToList();
                model.TotalRecord = Convert.ToInt32(objectParameter.Value);
                return model;
            }
        }
        public int Insert(WorkflowSecurityGroupUsersViewModel model)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                DataTable table = new DataTable();
                table = model.SecurityUserList.Select(a => new
                {
                    a.ID,
                    a.UserID,
                    a.DisplayName
                }).ToDataTable();
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@CompanyID", model.CompanyID),
                    new SqlParameter
                    {
                        ParameterName ="@SecurityUsersUTD",
                        Value =table,
                        SqlDbType =SqlDbType.Structured,
                        TypeName ="MUNDOCDocumentWorkflowSecurityGroupUsersUTD"
                    },
                };
                return context.ExecuteSqlProcedure(sqlParameters, "MUNDOCDocumentWorkflowSecurityGroupUsersInsert");
            }
        }
        public List<MUNDocumentWorkflowSecurityGroupUsersGet_Result> Get(int statusID, int companyID, int groupID)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDocumentWorkflowSecurityGroupUsersGet(statusID, companyID, groupID).ToList();
            }
        }

        public List<MUNDOCDocumentWorkflowSecurityGroupUsersUpdateProfile_Result> DocumentWorkflowSecurityGroupUsersUpdateProfile(int companyID, Guid userID, string firstName, string lastName)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowSecurityGroupUsersUpdateProfile(companyID, userID, firstName, lastName).ToList();
            }
        }
    }
}

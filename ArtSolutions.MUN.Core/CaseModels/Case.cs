using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class Case
    {
        #region "Cases"
        public List<MUNDOCDocumentWorkflowGet_Result> DocumentWorkflowGet(int? documentTypeID, int? workFlowID, int? WorkFlowVersion, int companyID, string language, bool? isActive, bool? isDeleted)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowGet(companyID, language, isActive, isDeleted, workFlowID, documentTypeID).ToList();
            }
        }
        public List<MUNDOCDocumentWorkflowStatusGet_Result> WorkflowStatusGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusGet(model.WorkflowID, model.CompanyID, model.Version, model.Language, model.IsPublic, model.IsDeleted, model.StatusID).ToList();
            }
        }
        public List<MUNDOCDocumentWorkflowStatusReasonGet_Result> WorkflowReasonGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusReasonGet(model.WorkflowID, model.Version, model.Language, model.StatusID, model.ReasonID).ToList();
            }
        }
        public List<MUNITAXMasterCasesGet_Result> MasterCasesGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXMasterCasesGet(model.ReasonID, model.CompanyID, model.StatusID, model.WorkflowID, model.Language, model.CaseIDs).ToList();
            }
        }
        public CaseList GetByPaging(CaseModel model)
        {
            CaseList masterCases = new CaseList();
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecords", typeof(int));

                masterCases.CaseModels = context.MUNITAXMasterCasesGetWithPaging(model.CompanyID, model.FilterText, model.CaseID, model.KeyCode, model.BussinessName, model.PriorityID,
                        model.Weight, model.AsignedTo, model.StatusID, model.ReasonID, model.CurrentIndex, model.PageSize, TotalRecord, model.SortColumn,
                        model.Sort, model.Language, model.WorkflowID).ToList();

                masterCases.TotalRecord = Convert.ToInt32(TotalRecord.Value);
            }
            return masterCases;
        }
        public List<MUNITAXCasePriorityGet_Result> CasePriorityGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCasePriorityGet(model.Language).ToList();
            }
        }
        public List<MUNITAXTeamGet_Result> CaseTeamGet(Guid? id)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXTeamGet(id).ToList();
            }
        }
        public int Insert(CaseModel model)
        {
            int caseID = 0;
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                decimal result = context.MUNITAXCasesInsert(model.CompanyID, model.AccountID, model.WorkflowID, model.Name, model.Refrence, model.Note, model.PriorityID,
                    model.Weight, model.OwnerID, model.AsignedTo, model.CreatedUserID, model.CreatedDate, model.DocumentType, model.AccountServices).FirstOrDefault().Value;
                int.TryParse(result.ToString(), out caseID);
            }
            return caseID;
        }
        public List<MUNITAXCasesGet_Result> Get(int caseId, string language, int companyID)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCasesGet(caseId, language, companyID).ToList();
            }
        }
        public List<MUNDOCDocumentWorkflowStatusActivityGet_Result> StatusActivityGet(CaseModel model, byte type)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowStatusActivityGet(model.CompanyID, model.Language, model.WorkflowID, model.Version
                    , model.StatusID, type).ToList();
            }
        }
        public List<MUNDOCDocumentWorkflowFormGet_Result> WorkflowFormGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowFormGet(model.FormID, model.Language).ToList();
            }
        }
        public int Update(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCasesUpdate(model.CaseID, model.CompanyID, model.Name, model.Refrence, model.Note, model.PriorityID,
                    model.Weight, model.OwnerID, model.AsignedTo, model.ModifiedUserID, model.ModifiedDate);
            }
        }
        #endregion "Cases"

        #region "Case Comments"
        public int CasesCommentUpdate(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                DataTable workfloHistoryLogs = model.WorkflowHistoryLogs.ToDataTable();
                SqlParameter[] sqlparameters = {
                   new SqlParameter { ParameterName = "@MUNITAXMasterCasesUTD",Value = workfloHistoryLogs,SqlDbType = SqlDbType.Structured,TypeName = "MUNITAXMasterCasesUTD" },
                    new SqlParameter("@ModifiedBy", model.ModifiedUserID),
                    new SqlParameter("@ModifiedDate", model.ModifiedDate)
                };
                return context.ExecuteSqlProcedure(sqlparameters, "MUNITAXMasterCasesCommentUpdate");
            }
        }
        public int CasesStatusUpdate(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                DataTable workfloHistoryLogs = model.WorkflowHistoryLogs.ToDataTable();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@MUNITAXMasterCasesUTD",Value = workfloHistoryLogs,SqlDbType = SqlDbType.Structured,TypeName = "MUNITAXMasterCasesUTD"},
                    new SqlParameter("@ModifiedBy", model.ModifiedUserID),
                    new SqlParameter("@ModifiedDate", model.ModifiedDate)
                };
                return context.ExecuteSqlProcedure(sqlparameters, "MUNITAXMasterCasesStatusUpdate");
            }
        }
        public List<MUNDOCDocumentWorkflowHistoryLogGet_Result> DocumentWorkflowHistoryLogGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNDOCDocumentWorkflowHistoryLogGet(model.CaseIDs, model.Language).ToList();
            }
        }
        public MUNITAXCasesKeyCode_Result KeyCodeGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCasesKeyCode(model.CompanyID).FirstOrDefault();
            }
        }
        #endregion "Case Comments"

        #region "Case Images"
        public int CaseImagesInsert(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                DataTable caseModels = model.CaseImages.ToDataTable();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@MUNITAXCaseImagesUTD",Value = caseModels,SqlDbType = SqlDbType.Structured,TypeName = "MUNITAXCaseImagesUTD"}
                };
                return context.ExecuteSqlProcedure(sqlparameters, "MUNITAXCaseImagesInsert");
            }
        }
        public CaseImagesList CaseImagesGet(CaseModel model)
        {
            CaseImagesList caseImageList = new CaseImagesList();
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));
                caseImageList.CaseImages = entities.MUNITAXCaseImagesGetWithPaging(model.CaseID, model.CurrentIndex, model.PageSize, totalRecord,
                                model.SortColumn, model.Sort, model.CompanyID).ToList();
                caseImageList.TotalRecord = Convert.ToInt32(totalRecord.Value);
            }
            return caseImageList;
        }
        #endregion

        #region "Dashboards"
        public List<MUNITAXMasterCasesGetBalanceByStatus_Result> CasesGetBalanceByStatus(CaseModel model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNITAXMasterCasesGetBalanceByStatus(model.CompanyID, model.Language).ToList();
            }
        }
        public List<MUNITAXMasterCasesGetCountByStatus_Result> CasesGetCountByStatus(CaseModel model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNITAXMasterCasesGetCountByStatus(model.CompanyID, model.Language).ToList();
            }
        }
        #endregion "Dashboards"
    }
}

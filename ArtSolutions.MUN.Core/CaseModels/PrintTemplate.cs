using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class PrintTemplate
    {
        #region "Properties"
        public int CompanyID { get; set; }
        public string FilterText { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string Language { get; set; }
        public int SrNo { get; set; }
        public int ID { get; set; } = 0;
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string WorkflowName { get; set; }
        public int WorkFlowID { get; set; }
        public int FileID { get; set; }
        public int StatusID { get; set; }
        public Guid CreatedUserID { get; set; } = Guid.Empty;
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; } = Guid.Empty;
        public DateTime ModifiedDate { get; set; }
        public int DataSourceID { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public PrintTemplateList GetWithPaging(PrintTemplate model)
        {
            PrintTemplateList printTemplateList = new PrintTemplateList();
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

                printTemplateList.PrintTemplates = entities.MUNPRNPrintTemplateGetWithPaging(model.CompanyID, model.FilterText, model.CurrentIndex,
                        model.PageSize, totalRecord, model.SortColumn, model.SortDirection, model.Language).Select(a => new PrintTemplate
                        {
                            Code = a.Code,
                            TemplateName = a.TemplateName,
                            Description = a.TemplateDiscription,
                            WorkflowName = a.WorkflowName,
                            ID = a.ID
                        }).ToList();
                printTemplateList.TotalRecord = Convert.ToInt32(totalRecord.Value);
            }
            return printTemplateList;
        }
        public List<MUNPRNPrintTemplateRelatedStepsGet_Result> RelatedStepsGet(CaseModel model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNPRNPrintTemplateRelatedStepsGet(model.CompanyID, model.Language, model.StatusIDs).ToList();
            }
        }
        public int Insert(PrintTemplate model)
        {
            var result = 0;
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                var tempResult = entities.MUNPRNPrintTemplateInsert(model.CompanyID, model.WorkFlowID, model.Code, model.FileID, model.TemplateName, model.Description,
                    model.StatusID, model.CreatedUserID, model.CreatedDate, model.DataSourceID);

                result = tempResult.FirstOrDefault().Value;
            }
            return result;
        }
        public List<MUNPRNPrintTemplateGet_Result> Get(PrintTemplate model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNPRNPrintTemplateGet(model.CompanyID, model.Language, model.StatusID, model.ID, model.WorkFlowID).ToList();
            }
        }
        public int Update(PrintTemplate model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNPRNPrintTemplateUpdate(model.ID, model.Code, model.TemplateName, model.Description, model.WorkFlowID, model.StatusID,
                    model.DataSourceID, model.Language, model.ModifiedUserID, model.ModifiedDate, model.CompanyID).FirstOrDefault().Value;
            }
        }
        #endregion "Public Methods"
    }
    public class PrintTemplateList
    {
        public List<PrintTemplate> PrintTemplates { get; set; }
        public int TotalRecord { get; set; }
    }
}

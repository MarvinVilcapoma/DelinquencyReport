using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class PrintCenter
    {
        #region "Properties"
        public int SrNo { get; set; }
        public int ID { get; set; }
        public string KeyCode { get; set; }
        public int PrintTemplateID { get; set; }
        public int OutputFormat { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public Destinatary Destinatary { get; set; } = new Destinatary();
        public int DataSourceID { get; set; }
        public Guid CreatedUserID { get; set; } = Guid.Empty;
        public DateTime CreatedDate { get; set; }
        public string CaseIDs { get; set; }
        public int FileID { get; set; }
        public int CaseCount { get; set; }
        public decimal Balance { get; set; }
        public Guid ModifidUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string FilterText { get; set; } = null;
        public string Language { get; set; } = "en";
        public string TemplateName { get; set; }
        public int CompanyID { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public int Insert(PrintCenter model)
        {
            using (CaseDataModelContainer entity = new CaseDataModelContainer())
            {
                return Convert.ToInt32(entity.MUNPRNPrintCenterInsert(model.KeyCode, model.PrintTemplateID, model.OutputFormat, model.Comments, model.Date,
                        model.Destinatary.Name, model.Destinatary.Position, model.Destinatary.Department, model.DataSourceID, model.CreatedUserID,
                        model.CreatedDate, model.CaseIDs, model.CaseCount, model.Balance).FirstOrDefault().Value);
            }
        }
        public List<MUNITAXDataSourceGet_Result> DataSourceGet(int dataSourceID)
        {
            using (CaseDataModelContainer entity = new CaseDataModelContainer())
            {
                return entity.MUNITAXDataSourceGet(dataSourceID).ToList();
            }
        }

        public List<MUNITAXAccountViewGet_Result> AccountViewGet(int companyID)
        {
            using (CaseDataModelContainer entity = new CaseDataModelContainer())
            {
                return entity.MUNITAXAccountViewGet(companyID).ToList();
            }
        }

        public int PrintCenterFileIDUpdate(PrintCenter model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNPRNPrintCenterFileIDUpdate(model.ID, model.FileID, model.ModifidUserID, model.ModifiedDate);
            }
        }

        public PrintCenterList GetByPaging(PrintCenter model)
        {
            PrintCenterList printCenterList = new PrintCenterList();
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));
                printCenterList.PrintCenters = entities.MUNPRNPrintCenterGetWithPaging(model.FilterText, model.CurrentIndex, model.PageSize, totalRecord, model.SortColumn,
                    model.SortDirection, model.Language, model.CompanyID).ToList();
                printCenterList.TotalRecord = Convert.ToInt32(totalRecord.Value);
            }
            return printCenterList;
        }
        public int PrintCenterLogInsert(PrintCenterLog model)
        {
            using (CaseDataModelContainer entities = new CaseDataModelContainer())
            {
                return entities.MUNPRNPrintCenterLogInsert(model.UserID, model.GenerateDate, model.FileID, model.Action,
                    model.CreatedUserID, model.CreatedDate);
            }
        }
        #endregion "Public Methods"
    }
    public class Destinatary
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }
    public class PrintCenterList
    {
        public List<MUNPRNPrintCenterGetWithPaging_Result> PrintCenters { get; set; }
        public int TotalRecord { get; set; }
    }
    public class PrintCenterLog
    {
        public Guid UserID { get; set; }
        public DateTime GenerateDate { get; set; }
        public int FileID { get; set; }
        public string Action { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class PostingProcess
    {
        public PostingProcessListModel GetWithPaging(int companyID, int? iD, string filterText, bool? isVoid, string documentTypeID, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PostingProcessListModel model = new PostingProcessListModel();
                model.PostingProcessList = context.MUNACCPostingProcessGetWithPaging(companyID, iD, filterText, isVoid, documentTypeID, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public List<MUNACCPostingProcessGet_Result> Get(int companyID, int ID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                List<MUNACCPostingProcessGet_Result> model = new List<MUNACCPostingProcessGet_Result>();
                model = context.MUNACCPostingProcessGet(companyID, ID, language).ToList();
                return model;
            }
        }
        public List<MUNACCPostingProcessDetailsGetWithGroupSum_Result> GetWithGroupSum(int companyID, int ID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                var model = context.MUNACCPostingProcessDetailsGetWithGroupSum(companyID, ID, language).ToList();
                return model;
            }
        }
        public MUNACCPostingProcessGetByLastInserted_Result LastInsertedGet(int companyID, int PaymentType, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                MUNACCPostingProcessGetByLastInserted_Result model = new MUNACCPostingProcessGetByLastInserted_Result();
                model = context.MUNACCPostingProcessGetByLastInserted(companyID, language, PaymentType).FirstOrDefault();
                return model;
            }
        }
        public List<MUNACCJournalDetailForPostingProcess_Result> JournalDetailForPostingProcess(int companyId, DateTime startPeriodDate, DateTime endPeriodDate, string documentTypeID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNACCJournalDetailForPostingProcess(companyId, language, startPeriodDate, endPeriodDate, documentTypeID).ToList();
            }
        }
        public int Update(PostingProcessModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var Retval = new ObjectParameter("Retval", typeof(int));
                context.MUNACCPostingProcessUpdateStatus(model.ID, model.WorkflowStatusID, model.ModifiedUserID, model.ModifiedDate, Retval);
                return (int)Retval.Value;
            }
        }
        public int Insert(int companyID, string commaSeparatedJournalID, string documentTypeDetail, DateTime asOfDate, int workflowID, int workflowVersionID, int workflowStatusID, DateTime createdDate, Guid createdUserID, int? PaymentType)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                context.MUNACCPostingProcessInsertForJournal(companyID, commaSeparatedJournalID, documentTypeDetail, asOfDate, workflowID, workflowVersionID, workflowStatusID, createdDate, createdUserID, PaymentType, ID);
                return int.Parse(ID.Value.ToString());
            }
        }
        public bool UpdateFINJournalID(JournalSyns model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var Retval = new ObjectParameter("Retval", typeof(int));
                context.MUNAccJournalUpdateForFINJournalID(model.FINJournalListJSON, model.ModifiedUserID, model.ModifiedDate, Retval);
                return (bool)Retval.Value;
            }
        }
        public NewPostingProcessModel InsertJournalForPayment(int CompanyID, int PaymentReceiptType, DateTime AsOfDate, DateTime CreatedDate, Guid CreatedUserID, string Language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                NewPostingProcessModel model = new NewPostingProcessModel();
                context.Database.CommandTimeout = 3000;
                var Retval = new ObjectParameter("JournalIDs", typeof(string));
                model.NewPostedList = context.MUNACCJournalBulkInsertForPayment(CompanyID, PaymentReceiptType, AsOfDate, CreatedDate, CreatedUserID, Language, Retval).ToList();
                model.JournalIds = Convert.ToString(Retval.Value);
                return model;
            }
        }
    }
}

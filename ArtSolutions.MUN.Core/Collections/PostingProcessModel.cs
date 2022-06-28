using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class PostingProcessModel
    {
        public int ID { get; set; }
        public int SequenceNo { get; set; }
        public int CompanyID { get; set; }
        public DateTime AsOfDate { get; set; }
        public string DocumentType { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public int WorkflowStatusID { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
    public class PostingProcessListModel
    {
        public List<MUNACCPostingProcessGetWithPaging_Result> PostingProcessList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class PostingProcessJournal
    {
        public string JournalID { get; set; }
        public string DocuemntTypeDetail { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public DateTime AsOfDate { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVerionID { get; set; }
        public int WorkflowStatusID { get; set; }
        public int? PaymentType { get; set; }
    }

    public class NewPostingProcessModel
    {
        public int PaymentReceiptType { get; set; }
        public DateTime AsOfDate { get; set; }        
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public string JournalIds { get; set; }
        public List<MUNACCJournalBulkInsertForPayment_Result> NewPostedList { get; set; }
    }
}

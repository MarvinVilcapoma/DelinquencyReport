using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class CreditNoteModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int PaymentID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
        public string CommaSeperatedImageIDs { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool IsVoid { get; set; }
        public string AccountDisplayName { get; set; }
        public decimal AvailableAmount { get; set; }
        public string ReceiptNumber { get; set; }  //Payment Receipt Number
        public int ServiceTypeID { get; set; }
        public string PaymentPlanName { get; set; }
        public int WorkflowStatusID { get; set; }        
    }
    public class CreditNoteAppliedDetail
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public string PaymentPlanName { get; set; }
    }
    public class CreditNoteRefundDetail
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public string PaymentPlanName { get; set; }
    }
    public partial class CreditNoteImagesModel
    {
        public int ImageID { get; set; }        
        public string FileName { get; set; }
    }
    public class CreditNoteListModel
    {
        public List<MUNCOLCreditNoteGetWithPaging_Result> CreditNoteList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class CreditNotePrintModel
    {
        public CreditNoteModel CreditNote { get; set; }
        public List<CreditNoteAppliedDetail> CreditNoteAppliedDetailList { get; set; }
        public List<CreditNoteRefundDetail> CreditNoteRefundDetailList { get; set; }
        public List<CreditNoteImagesModel> CreditNoteImagesList { get; set; }
    }
}

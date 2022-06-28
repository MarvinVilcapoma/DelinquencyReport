using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.Collections
{
    public class DebitNoteModel
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
        public bool IsPaid { get; set; }
    }
    public partial class DebitNoteImagesModel
    {
        public int ImageID { get; set; }
        public string FileName { get; set; }
    }
    public class DebitNotePrintModel
    {
        public DebitNoteModel DebitNote { get; set; }
        public List<DebitNoteImagesModel> DebitNoteImagesList { get; set; } 
        public List<DebitNotePaidModel> DebitNotePaidList { get; set; }
    }
    public class DebitNotePaidModel
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool ISVoid { get; set; }
        public decimal Amount { get; set; }
    }
    public class DebitNoteListModel
    {
        #region public properties       
        public List<MUNCOLDebitNoteGetWithPaging_Result> DebitNoteList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion  
    }
}

using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class CreditNoteModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int PaymentID { get; set; }
        public string Number { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime Date { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal Amount { get; set; }
        public string CommaSeperatedImageIDs { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<bool> IsVoid { get; set; }
        public string Status { get; set; }
        public string AccountDisplayName { get; set; }
        public Nullable<decimal> AvailableAmount { get; set; }
        public string ReceiptNumber { get; set; }  //Payment Receipt Number
        public int ServiceTypeID { get; set; }
        public string PaymentPlanName { get; set; }
        public int WorkflowStatusID { get; set; }
        public bool IsCreditHistory { get; set; }
        public string FormattedAmount
        {
            get
            {
                return Amount.ToString("C");
            }
        }
        public string FormattedAvailableAmount
        {
            get
            {
                return AvailableAmount.HasValue ? AvailableAmount.Value.ToString("C") : "";
            }
        }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        #endregion
    }
    public class CreditNoteAppliedDetail
    {
        #region public properties       
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public string PaymentPlanName { get; set; }
        #endregion
    }
    public class CreditNoteRefundDetail
    {
        #region public properties       
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public string PaymentPlanName { get; set; }
        #endregion
    }
    public partial class CreditNoteImagesModel
    {
        #region public properties       
        public int ImageID { get; set; }
        public string FileName { get; set; }
        #endregion  
    }
    public class CreditNoteListModel
    {
        #region public properties       
        public List<CreditNoteModel> CreditNoteList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion  
    }
    public class CreditNotePrintModel
    {
        #region public properties       
        public CreditNoteModel CreditNote { get; set; }
        public List<CreditNoteAppliedDetail> CreditNoteAppliedDetailList { get; set; }
        public List<CreditNoteRefundDetail> CreditNoteRefundDetailList { get; set; }
        public List<CreditNoteImagesModel> CreditNoteImagesList { get; set; }
        public List<CaseWorkflowStatus> WorkflowStatusList { get; set; }
        public WorkflowStatusModel WorkflowStatus { get; set; }
        public int WorkflowStatusID { get; set; }
        public string Date { get; set; }
        #endregion  
    }
}
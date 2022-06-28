using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class DebitNoteModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int? PaymentID { get; set; }
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
        public bool SelectedItem { get; set; }
        public bool IsDebitNote { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDebitHistory { get; set; }
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

        public List<DebitNoteModel> GetDebitNotes(int AccountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = AccountID });
            return new RestSharpHandler().RestRequest<List<DebitNoteModel>>(APISelector.Municipality, true, "api/DebitNote/DebitNoteDetailGet", "GET", lstParameter);
        }

    }
    public partial class DebitNoteImagesModel
    {
        #region public properties       
        public int ImageID { get; set; }
        public string FileName { get; set; }
        #endregion  
    }
    public class DebitNotePrintModel
    {
        #region public properties       
        public DebitNoteModel DebitNote { get; set; }
        public List<DebitNoteImagesModel> DebitNoteImagesList { get; set; }
        public List<DebitNotePaidDetailModel> DebitNotePaidList { get; set; }
        public List<CaseWorkflowStatus> WorkflowStatusList { get; set; }
        public WorkflowStatusModel WorkflowStatus { get; set; }
        public int WorkflowStatusID { get; set; }
        public string Date { get; set; }
        #endregion  
    }
    public class DebitNotePaidDetailModel
    {
        #region public properties       
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool ISVoid { get; set; }
        public decimal Amount { get; set; }
        #endregion
    }
    public class DebitNoteListModel
    {
        #region public properties       
        public List<DebitNoteModel> DebitNoteList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion  
    }
}
using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PostingProcessModel
    {
        #region public properties 
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public System.DateTime AsOfDate { get; set; }
        public string DocumentType { get; set; }
        public bool IsVoid { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public int WorkflowStatusID { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool IsPost { get; set; }
        public string FormattedAmount
        {
            get
            {
                return Amount.HasValue ? Amount.Value.ToString("C") : "";
            }
        }

        public string FormattedDate
        {
            get
            {
                return AsOfDate.ToString("d");
            }
        }
        #endregion
    }

    public class PostingProcessListModel
    {
        #region public properties 
        public List<PostingProcessModel> PostingProcessList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }

    public class JournalListForPosting
    {
        #region public properties 
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public string NameFriendly { get; set; }
        public string ReferenceAccountName { get; set; }
        public string Memo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Amount { get; set; }
        public string CollectionTypeName { get; set; }
        public string Grant { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string DocumentTypeDetail { get; set; }
        public System.DateTime AsOfDate { get; set; }
        public int PostingProcessID { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public Nullable<int> WorkflowStatusID { get; set; }
        public int IsPost { get; set; }
        public int IsVoid { get; set; }
        public string Status { get; set; }
        public string ReceiptNumber { get; set; }
        public int CompanyID { get; set; }
        public int DocumentTypeID { get; set; }
        public int FiscalYearID { get; set; }
        public int FiscalYearPeriodID { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<int> BankID { get; set; }
        public string ApplicationSource { get; set; }
        public string ReferenceNumber { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int JounalLine { get; set; }
        public int GrantID { get; set; }
        public string ReferenceID { get; set; }
        public int ReferenceAccountID { get; set; }
        public string IntegrationError { get; set; }
        public Nullable<int> FINJournalID { get; set; }
        public Nullable<int> FINJournalEntrySequence { get; set; }
        public int DocumentID { get; set; }       
        public string FormattedAsOfDate
        {
            get
            {
                return this.AsOfDate.ToString("d");
            }
        }
        #endregion
    }

    public class PostingProcessJournal
    {
        #region public properties 
        public string JournalID { get; set; }
        public string DocuemntTypeDetail { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public DateTime AsOfDate { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVerionID { get; set; }
        public int WorkflowStatusID { get; set; }
        public int? PaymentType { get; set; }
        #endregion
    }

    public class NewPostingProcessModel
    {
        public int? ID { get; set; }
        public int workflowID { get; set; }
        public int workflowStatusID { get; set; }
        public int? PaymentReceiptType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? LastAsOfDate { get; set; }
        public DateTime AsOfDate { get; set; }        
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }        
        public string JournalIds { get; set; }
        public IEnumerable<SelectListItem> PaymentReceiptTypeList { get; set; }
        public List<WorkflowViewModel> WorkFlowList { get; set; }
        public List<CaseWorkflowStatus> WorkflowStatusList { get; set; }
        public List<NewPostingProcessDetails> NewPostedList { get; set; } = new List<NewPostingProcessDetails>();
        public List<JournalListForPosting> JournalForPostingList { get; set; } = new List<JournalListForPosting>();
    }

    public class NewPostingProcessDetails
    {
        public Nullable<int> ID { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> CollectionTypeID { get; set; }
        public string CollectionTypeName { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
    }
}
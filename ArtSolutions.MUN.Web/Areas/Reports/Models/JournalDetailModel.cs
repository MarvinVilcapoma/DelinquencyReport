using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class JournalDetailModel
    {
        #region public properties
        public List<JournalDetailListModel> JournalDetailListModel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DocumentTypeIDs { get; set; }
        public string FilterDocumentTypeIDs { get; set; }
        public List<SelectListItem> lstDocumentType { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<SelectListItemViewModel> AccountServiceTypeList { get; set; }
        public List<SelectListItemViewModel> InvoiceTypeList { get; set; }
        public List<SelectListItemViewModel> PaymentPlanTypeList { get; set; }
        public List<SelectListItemViewModel> BankAccountList { get; set; }
        public List<SelectListItemViewModel> GrantList { get; set; }
        public List<SelectListItemViewModel> CheckbookList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public string[] AccountServiceTypeIDs { get; set; }
        public string FilterAccountServiceTypeID { get; set; }
        public string[] InvoiceTypeIDs { get; set; }
        public string FilterInvoiceTypeID { get; set; }
        public string[] PaymentPlanTypeIDs { get; set; }
        public string FilterPaymentPlanTypeID { get; set; }
        public string[] BankAccountIDs { get; set; }
        public string FilterBankAccountID { get; set; }
        public string[] GrantIDs { get; set; }
        public string FilterGrantID { get; set; }
        public string[] CheckbookIDs { get; set; }
        public string FilterCheckbookID { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        #endregion

        #region Constructor
        public JournalDetailModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }

    public class JournalDetailListModel
    {
        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public string NameFriendly { get; set; }
        public string RegisterNumber { get; set; }
        public string ReferenceAccountName { get; set; }
        public string Memo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Amount { get; set; }
        public string CollectionTypeName { get; set; }
        public string Grant { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string ReceiptNumber { get; set; }
        public int IsPost { get; set; }
        public int IsVoid { get; set; }
        public string Status { get; set; }
        public string ServiceTypeName { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public Nullable<int> FINJournalID { get; set; }
        public string ChecbookName { get; set; }
        #endregion
    }
}
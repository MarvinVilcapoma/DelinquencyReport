using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class FINJournalModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int DocumentType { get; set; }
        public DateTime Date { get; set; }
        public int FiscalYearID { get; set; }
        public int PeriodID { get; set; }
        public string Memo { get; set; }
        public string ProjectID { get; set; }
        public int? RelatedDocID { get; set; }
        public bool IsPost { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public string ApplicationSource { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? BankAccountID { get; set; }
        public string Number { get; set; }
        public int AccountID { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> FinJournalID { get; set; }

        public List<FINJournalDetail> AccountList { get; set; }
    }

    public class FINJournalDetail
    {
        public int JounalLine { get; set; }
        public int AccountID { get; set; }
        public int? ClassID { get; set; }
        public int? GrantID { get; set; }

        public string ReferenceID { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceCustomerVendorEmployeID { get; set; }
        public string ReferenceCustomerVendorEmployeName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    public class JournalSyns
    {
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FINJournalListJSON { get; set; }
    }

}

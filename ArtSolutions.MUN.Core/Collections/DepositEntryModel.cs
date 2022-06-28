using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class DepositEntryModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SequenceID { get; set; }
        public int? DepositTypeID { get; set; }
        public int? DepositTypeTableID { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal NetDeposit { get; set; }
        public int ClosingCount { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsVoid { get; set; }
        public byte[] RowVersion { get; set; }
        public string CommaSeperatedClosingIds { get; set; }
        public string DepositTypeName { get; set; }
        public string VoidReason { get; set; }
        public int BankAccountID { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public int? FinanceAccountID { get; set; }
        public string FinanceAccountCode { get; set; }
        public string FinanceAccountName { get; set; }
    }
    public class DepositEntryListModel
    {
        public List<MUNCOLDepositGetWithPaging_Result> DepositEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class DepositEntryPrintModel
    {
        public DepositEntryModel DepositEntry { get; set; }
        public List<ClosingEntryModel> ClosedEntryList { get; set; }
    }
    public class BankAccountModel
    {
        public int ID { get; set; }
        public int BankAccountID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? BankID { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public int? AccountID { get; set; }
        public string FinanceAccount { get; set; }
    }
}

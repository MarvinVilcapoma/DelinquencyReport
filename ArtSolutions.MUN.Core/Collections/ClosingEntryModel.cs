using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class ClosingEntryModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public Guid CashierID { get; set; }
        public string CashierName { get; set; }
        public int ClosingTypeID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? PaymentOptionID { get; set; }
        public int? FinanceCheckbookID { get; set; }
        public string FinanceCheckbookCode { get; set; }
        public string FinanceCheckbookName { get; set; }
        public int? CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public decimal NetClosing { get; set; }
        public int PaymentReceiptCount { get; set; }
        public bool IsDeposited { get; set; }
        public bool IsVoid { get; set; }
        public string CommaSeperatedPaymentIds { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }       
        public decimal TotalCash { get; set; }
        public decimal TotalChequedelBancoNacional { get; set; }
        public decimal TotalCreditCard { get; set; }
        public decimal TotalBankTransfer { get; set; }
        public decimal TotalChequedelBancodeCostaRica { get; set; }
        public decimal TotalAdjustment { get; set; }
        public decimal TotalBankTransferByBancoNacionaldeCostaRica { get; set; }
        public decimal TotalBank { get; set; }

        #region Extra Columns
        public string ClosingTypeName { get; set; }
        public int DepositID { get; set; }
        public int ClosingID { get; set; }
        public string VoidReason { get; set; }
        public string PaymentType { get; set; }
        public string ServiceType { get; set; }
        public string CollectionType { get; set; }
        public string PaymentOption { get; set; }
        #endregion
    }
    public class ClosingEntryListModel
    {
        public List<MUNCOLClosingGetWithPaging_Result> ClosingEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ClosingEntryPrintModel
    {
        public ClosingEntryModel ClosingEntry { get; set; }
        public List<PaymentModel> PaymentReceipts { get; set; }
    }
    public class ReceiptEntryModel
    {
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string AccountDisplayName { get; set; }
        public Nullable<DateTime> VoidDate { get; set; }
        public string VoidReason { get; set; }
    }
}

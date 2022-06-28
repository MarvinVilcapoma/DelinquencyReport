//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtSolutions.MUN.Core
{
    using System;
    
    public partial class MUNCOLClosingPrint_Result
    {
        public int ID { get; set; }
        public System.Guid CashierID { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public int ClosingTypeID { get; set; }
        public System.DateTime Date { get; set; }
        public int PaymentReceiptCount { get; set; }
        public decimal NetClosing { get; set; }
        public byte[] RowVersion { get; set; }
        public Nullable<bool> IsVoid { get; set; }
        public bool IsDeposited { get; set; }
        public byte[] RowVersion1 { get; set; }
        public string VoidReason { get; set; }
        public Nullable<decimal> TotalCash { get; set; }
        public Nullable<decimal> TotalChequedelBancoNacional { get; set; }
        public Nullable<decimal> TotalCreditCard { get; set; }
        public Nullable<decimal> TotalBankTransfer { get; set; }
        public Nullable<decimal> TotalChequedelBancodeCostaRica { get; set; }
        public Nullable<int> PaymentOptionID { get; set; }
        public Nullable<decimal> TotalAdjustment { get; set; }
        public string PaymentOptionName { get; set; }
        public Nullable<decimal> TotalBank { get; set; }
        public Nullable<decimal> TotalBankTransferByBancoNacionaldeCostaRica { get; set; }
    }
}
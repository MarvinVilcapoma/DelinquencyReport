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
    
    public partial class MUNSERREPReceiptDetail_Result
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public string RegisterNumber { get; set; }
        public string DisplayName { get; set; }
        public string ServiceTypeName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClosingNumber { get; set; }
        public string DepositNumber { get; set; }
        public string BankAccountCode { get; set; }
        public string BankAccountName { get; set; }
        public string GrantCode { get; set; }
        public string GrantName { get; set; }
        public string ChecbookName { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> TotalAllocated { get; set; }
        public decimal ReceiptTotalAmount { get; set; }
        public System.Guid Collector { get; set; }
    }
}

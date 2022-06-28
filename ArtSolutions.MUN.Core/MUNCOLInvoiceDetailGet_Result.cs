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
    
    public partial class MUNCOLInvoiceDetailGet_Result
    {
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public Nullable<int> AccountServiceCollectionDetailID { get; set; }
        public Nullable<int> FINGrantID { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public string ItemType { get; set; }
        public string FINGrantName { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> Total { get; set; }
        public byte[] RowVersion { get; set; }
        public Nullable<int> ReceivableAccountID { get; set; }
        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public Nullable<int> RevenueAccountID { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public Nullable<int> CheckbookID { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public Nullable<int> CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public Nullable<int> CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        public string FINGrantCode { get; set; }
    }
}

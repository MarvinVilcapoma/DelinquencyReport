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
    
    public partial class MUNACCJournalDetailForPostingProcess_Result
    {
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
        public int IsPost { get; set; }
        public int IsVoid { get; set; }
        public string ReceiptNumber { get; set; }
    }
}

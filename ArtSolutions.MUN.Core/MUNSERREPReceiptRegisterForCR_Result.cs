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
    
    public partial class MUNSERREPReceiptRegisterForCR_Result
    {
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public string TaxNumber { get; set; }
        public string AccountDisplayName { get; set; }
        public string PaymentType { get; set; }
        public string Bank { get; set; }
        public Nullable<System.Guid> CreatedUserID { get; set; }
        public decimal Interest { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal TotalAmountAuxiliaryReceipt { get; set; }
        public decimal TotalAmountOfficialReceipt { get; set; }
        public string PaymentPeriod { get; set; }
        public string CustomField { get; set; }
        public string StatusWithBatchNumber { get; set; }
        public int PaymentReceiptType { get; set; }
        public int PrintTemplateID { get; set; }
        public decimal Discount { get; set; }
    }
}

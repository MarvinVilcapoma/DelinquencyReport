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
    
    public partial class MUNCOLInvoiceGetWithPaging_Result
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public bool IsPost { get; set; }
        public bool IsVoid { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public string AccountDisplayName { get; set; }
        public decimal Payments { get; set; }
        public bool AccountIsActive { get; set; }
        public string ServiceName { get; set; }
    }
}
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
    
    public partial class MUNCOLPaymentGetWithPaging_Result
    {
        public int ID { get; set; }
        public string AccountDisplayName { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public bool IsPost { get; set; }
        public decimal NetPayment { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string ItemName { get; set; }
        public string PaymentPlanName { get; set; }
    }
}

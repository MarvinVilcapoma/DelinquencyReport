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
    
    public partial class MUNAccountCreditHistoryGet_Result
    {
        public Nullable<int> ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public string Reason { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<int> PaymentID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string PaymentNumber { get; set; }
        public string Number { get; set; }
    }
}

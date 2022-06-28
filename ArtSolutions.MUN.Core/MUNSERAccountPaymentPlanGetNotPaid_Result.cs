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
    
    public partial class MUNSERAccountPaymentPlanGetNotPaid_Result
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int ServicePaymentPlanID { get; set; }
        public string Number { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal InstalmentPercentage { get; set; }
        public decimal InterestPercentage { get; set; }
        public int Months { get; set; }
        public bool IsEditable { get; set; }
        public decimal InstalmentAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDebt { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public string InactiveReason { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string PaymentPlanName { get; set; }
    }
}
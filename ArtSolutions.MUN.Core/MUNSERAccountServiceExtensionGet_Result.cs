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
    
    public partial class MUNSERAccountServiceExtensionGet_Result
    {
        public int CompanyID { get; set; }
        public int AccountServiceID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Months { get; set; }
        public decimal GrossVolume { get; set; }
        public decimal ExemptionAmount { get; set; }
        public decimal Total { get; set; }
        public decimal CreditAmount { get; set; }
        public Nullable<int> ImageID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string FileName { get; set; }
    }
}

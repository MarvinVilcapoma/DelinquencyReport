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
    
    public partial class MUNSERFillingLicenseInsert_Result
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        public decimal GrossVolume { get; set; }
        public decimal ExemptionAmount { get; set; }
        public decimal Total { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int AccountServiceID { get; set; }
        public Nullable<decimal> PercentageValue { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public int IsEditPermission { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
    }
}

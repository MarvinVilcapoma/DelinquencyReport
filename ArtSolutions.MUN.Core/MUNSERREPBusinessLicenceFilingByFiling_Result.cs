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
    
    public partial class MUNSERREPBusinessLicenceFilingByFiling_Result
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string SocialSecurity { get; set; }
        public int FiscalYearID { get; set; }
        public string FormattedFiscalYearID { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public string AccountIdByFilling { get; set; }
        public Nullable<decimal> SalesVolume { get; set; }
        public Nullable<System.Guid> FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
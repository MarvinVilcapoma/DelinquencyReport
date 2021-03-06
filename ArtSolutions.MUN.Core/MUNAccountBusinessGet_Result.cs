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
    
    public partial class MUNAccountBusinessGet_Result
    {
        public int ID { get; set; }
        public string RegisterNumber { get; set; }
        public int CompanyID { get; set; }
        public int AccountTypeID { get; set; }
        public int IsBusiness { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int SequenceID { get; set; }
        public string DisplayName { get; set; }
        public string PrintCheckAs { get; set; }
        public string BusinessDescription { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyISOCode { get; set; }
        public string Notes { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public string StateNumber { get; set; }
        public System.DateTime InitialDate { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string LegalName { get; set; }
        public string DBAName { get; set; }
        public Nullable<int> NAICSCodeID { get; set; }
        public Nullable<int> NAICSCodeTableID { get; set; }
        public string NAICSCode { get; set; }
        public Nullable<int> BusinessTypeID { get; set; }
        public Nullable<int> BusinessTypeTableID { get; set; }
        public string BusinessType { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public Nullable<int> ZoneTableID { get; set; }
        public string Zone { get; set; }
        public string ARPEDescription { get; set; }
        public string MaskedTaxNumber { get; set; }
        public string UnMaskedTaxNumber { get; set; }
        public bool IsSponsor { get; set; }
        public string ReferenceID { get; set; }
        public string BusinessAccountName { get; set; }
        public string BanacioIdentifica { get; set; }
        public Nullable<bool> IsInJudicialCollection { get; set; }
        public Nullable<bool> IsNeedTempRight { get; set; }
        public bool ExemptFromIVA { get; set; }
    }
}

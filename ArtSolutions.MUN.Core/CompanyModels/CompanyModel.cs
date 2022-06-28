using System;

namespace ArtSolutions.MUN.Core
{
    public class CompanyModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public string WorkPhone { get; set; }
        public string URL { get; set; }
        public string Email { get; set; }
        public Nullable<int> LogoID { get; set; }
        public int CountryID { get; set; }
        public string CompanySize { get; set; }
        public bool IsAgreedTerms { get; set; }
        public System.DateTime AgreeTermsOnDate { get; set; }
        public string TermsDetails { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public string Modules { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public CompanyParameterModel CompanyParameter { get; set; }
        public string Code { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> CountryStateID { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public Nullable<int> CompnayCountryID { get; set; }
        public Nullable<int> MUNAccountAddressesID { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string StateName { get; set; }
    }
}

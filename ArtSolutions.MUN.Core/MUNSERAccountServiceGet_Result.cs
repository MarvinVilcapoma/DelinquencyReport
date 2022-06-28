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
    
    public partial class MUNSERAccountServiceGet_Result
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string SSEIN { get; set; }
        public string LicenseType { get; set; }
        public int GroupId { get; set; }
        public int ServiceID { get; set; }
        public int ServiceTypeID { get; set; }
        public Nullable<decimal> AnnualIncome { get; set; }
        public string LicenseNumber { get; set; }
        public int Year { get; set; }
        public int IssueNumber { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public bool IsIssued { get; set; }
        public Nullable<System.Guid> IssuedBy { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public bool IsLock { get; set; }
        public bool IsVoid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string ServiceName { get; set; }
        public int AccountTypeID { get; set; }
        public Nullable<int> FINItemID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int FrequencyID { get; set; }
        public Nullable<System.DateTime> ExtensionEndDate { get; set; }
        public Nullable<int> ExtensionID { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string SerCustomField1 { get; set; }
        public string SerCustomField2 { get; set; }
        public string SerCustomField3 { get; set; }
        public int FilingTypeID { get; set; }
        public string Property { get; set; }
        public Nullable<int> AccountPropertyID { get; set; }
        public Nullable<System.DateTime> CustomDateField { get; set; }
        public string SerCustomDateField { get; set; }
        public string TotalValueLabel { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<bool> IsDeletedProperty { get; set; }
        public Nullable<bool> IsTransferService { get; set; }
        public string TransferNotes { get; set; }
        public Nullable<bool> IsDateFieldCustomField1 { get; set; }
        public Nullable<int> PeriodFrequencyID { get; set; }
        public Nullable<bool> MultipleServicesPerYearPerRight { get; set; }
        public string CustomField4 { get; set; }
        public string SerCustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public string SerCustomField5 { get; set; }
        public Nullable<bool> IsInJudicialCollection { get; set; }
        public string BusinessDescription { get; set; }
    }
}

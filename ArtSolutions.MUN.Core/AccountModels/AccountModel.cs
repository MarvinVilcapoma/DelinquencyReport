using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.AccountModels
{

    #region Account
    public class AccountList
    {
        public List<MUNAccountGetWithPaging_Result> AccountModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountListForSearch
    {
        public List<MUNAccountGetForSearch_Result> AccountList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountIndividualModel
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public string RegisterNumber { get; set; }
        public int CompanyID { get; set; }
        public int AccountTypeID { get; set; }
        public int IsBusiness { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string DisplayName { get; set; }
        public string PrintCheckAs { get; set; }
        public string BusinessDescription { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyISOCode { get; set; }
        public string Notes { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public string StateNumber { get; set; }
        public DateTime InitialDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Nullable<int> SalutationID { get; set; }
        public Nullable<int> SalutationTableID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public Nullable<int> SuffixID { get; set; }
        public Nullable<int> SuffixTableID { get; set; }
        public List<AccountEmail> EmailList { get; set; }
        public List<AccountPhone> PhoneList { get; set; }
        public List<AccountAddress> AddressList { get; set; }
        public List<AccountContact> ContactList { get; set; }
        public List<AccountFile> FileList { get; set; }
        public Nullable<Guid> InactiveByUserID { get; set; }
        public Nullable<DateTime> InactiveDate { get; set; }
        public int PaymentTermID { get; set; }
        public bool IsExempt { get; set; }
        public Nullable<int> TaxID { get; set; }
        public byte[] RowVersion { get; set; }
        public int SequenceID { get; set; }
        public string MaskedTaxNumber { get; set; }
        public string UnMaskedTaxNumber { get; set; }
        public bool IsSponsor { get; set; }
        public bool ExemptFromIVA { get; set; }
        public string ReferenceID { get; set; }
        public string BanacioIdentifica { get; set; }
        public int? IDTypeID { get; set; }
        public int? IDTypeIDTableID { get; set; }
        public string IDType { get; set; }
        public string Address { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public bool IsNeedTempRight { get; set; }
    }
    public class AccountBusinessModel
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public string RegisterNumber { get; set; }
        public int CompanyID { get; set; }
        public int AccountTypeID { get; set; }
        public int IsBusiness { get; set; }
        public Nullable<int> ParentID { get; set; }
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
        public DateTime InitialDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string LegalName { get; set; }
        public string DBAName { get; set; }
        public Nullable<int> NAICSCodeID { get; set; }
        public Nullable<int> NAICSCodeIDTable { get; set; }
        public Nullable<int> BusinessTypeID { get; set; }
        public Nullable<int> BusinessTypeTableID { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public Nullable<int> ZoneTableID { get; set; }
        public string ARPEDescription { get; set; }
        public List<AccountEmail> EmailList { get; set; }
        public List<AccountPhone> PhoneList { get; set; }
        public List<AccountAddress> AddressList { get; set; }
        public List<AccountContact> ContactList { get; set; }
        public List<AccountFile> FileList { get; set; }
        public Nullable<Guid> InactiveByUserID { get; set; }
        public Nullable<DateTime> InactiveDate { get; set; }
        public int PaymentTermID { get; set; }
        public bool IsExempt { get; set; }
        public Nullable<int> TaxID { get; set; }
        public int SequenceID { get; set; }
        public int NAICSCodeTableID { get; set; }
        public string NAICSCode { get; set; }
        public string BusinessType { get; set; }
        public string Zone { get; set; }
        public string MaskedTaxNumber { get; set; }
        public string UnMaskedTaxNumber { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsSponsor { get; set; }
        public bool ExemptFromIVA { get; set; }
        public string ReferenceID { get; set; }
        public string BanacioIdentifica { get; set; }
        public string BusinessAccountName { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public bool IsNeedTempRight { get; set; }
    }
    #endregion

    #region Account Support Tables
    public enum TableType
    {
        Email = 1,
        Phone = 2,
        Address = 3,
        Contact = 4
    }
    public class AccountSupportTable
    {
        public int AccountID { get; set; }
        public int MunID { get; set; }
        public int FinID { get; set; }
        public int Type { get; set; }
        public AccountSupportTable(int accountID, int munID, int finID, int supportType)
        {
            AccountID = accountID;
            MunID = munID;
            FinID = finID;
            Type = supportType;
        }
    }
    public class AccountEmail
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public string Email { get; set; }
        public int EmailTypeID { get; set; }
        public int EmailTypeTableID { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
    public class AccountPhone
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneTypeID { get; set; }
        public int PhoneTypeTableID { get; set; }
        public bool IsWorkPrimary { get; set; }
        public bool IsMobilePrimary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class AccountAddress
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int? AddressTypeID { get; set; }
        public int? AddressTypeTableID { get; set; }
        public bool IsPrimary { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> CountryStateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> TownID { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string MapLocation { get; set; }
        public bool IsActive { get; set; }
    }
    public class AccountAddressForPrint : AccountAddress
    {
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string TwoLetterIsoCode { get; set; }
    }
    public class AccountContact
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> PositionTableID { get; set; }
        public int? SalutationID { get; set; }
        public int? SalutationTableID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string OtherPhoneNumber { get; set; }
        public string PrimaryEmail { get; set; }
        public string OtherEmail { get; set; }
        public bool IsActive { get; set; }
    }
    public class AccountFile
    {
        public int ID { get; set; }
        public int FileID { get; set; }
        public Nullable<int> FileTypeID { get; set; }
        public Nullable<int> FileTypeTableID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    #endregion

    #region Import Account
    public class AccountImportViewModel
    {
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string DisplayName { get; set; }
        public string BusinessDescription { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string Fax { get; set; }
        public string Currency { get; set; }
        public string IDType { get; set; }
        public string TaxNumber { get; set; }
        public string StateNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public DateTime? InitialDate { get; set; }
        public string NAICSCode { get; set; }
        public string BusinessType { get; set; }
        public string Zone { get; set; }
        public string ARPEDescription { get; set; }
        public string PostalAddressLine1 { get; set; }
        public string PostalAddressLine2 { get; set; }
        public string PostalCity { get; set; }
        public string PostalState { get; set; }
        public string PostalCountry { get; set; }
        public string PostalTown { get; set; }
        public string PostalZipCode { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeAddressLine2 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeState { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficeTown { get; set; }
        public string OfficeZipCode { get; set; }
        public string ContactName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPosition { get; set; }
        public string ReferenceID { get; set; }
    }
    public class AccountImportModel
    {
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AccountType { get; set; }
        public List<AccountImportViewModel> ImportList { get; set; }
    }
    public class ValidImportAccountModel
    {
        public string GroupCode { get; set; }
        public string Reason { get; set; }
    }
    #endregion

    #region Account As People
    public class AccountAsPeopleModel
    {
        public List<MUNAccountGetAsPeople_Result> PeopleList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    #endregion

    #region AccountForJudicialCollection
    public class AccountForJudicialCollectionModel
    {
        public int CompanyID { get; set; }
        public int ID { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
    #endregion

    #region AccountExport
    public class AccountListExport
    {
        public List<MUNAccountExport_Result> ExportAccountList { get; set; }
    }
    #endregion
}

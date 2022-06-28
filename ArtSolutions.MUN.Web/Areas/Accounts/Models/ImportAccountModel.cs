using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class ImportAccountModel
    {
        #region public properties
        public static string[] GroupCodeColumn = new string[] { "GroupCode", "GROUPCODE" };
        public static string[] AccountTypeColumn = new string[] { "AccountType", "Type" };
        public static string[] SalutationColumn = new string[] { "Salutation" };
        public static string[] FirstNameColumn = new string[] { "FirstName  " };
        public static string[] LastNameColumn = new string[] { "LastName" };
        public static string[] NameColumn = new string[] { "Name" };
        public static string[] DisplayNameColumn = new string[] { "DisplayName" };
        public static string[] BusinessDescriptionColumn = new string[] { "BusinessDescription" };
        public static string[] EmailColumn = new string[] { "Email" };
        public static string[] WebSiteColumn = new string[] { "WebSite" };
        public static string[] WorkPhoneColumn = new string[] { "WorkPhone" };
        public static string[] MobilePhoneColumn = new string[] { "MobilePhone" };
        public static string[] CurrencyColumn = new string[] { "Currency" };
        public static string[] IDTypeColumn = new string[] { "IDType", "ID Type" };
        public static string[] TaxNumberColumn = new string[] { "TaxNumber", "Tax Number" };
        public static string[] StateNumberColumn = new string[] { "StateNumber", "State Number" };
        public static string[] TreasuryNumberColumn = new string[] { "TreasuryNumber", "Treasury Number" };
        public static string[] InitialDateColumn = new string[] { "InitialDate", "Initial Date" };
        public static string[] NAICSCodeColumn = new string[] { "NAICSCode", "NAICS Code" };
        public static string[] BusinessTypeColumn = new string[] { "BusinessType", "Business Type" };
        public static string[] ZoneColumn = new string[] { "Zone" };
        public static string[] ARPEDescriptionColumn = new string[] { "ARPEDescription", "ARPE Description" };
        public static string[] PostalAddressLine1Column = new string[] { "PostalAddressLine1" };
        public static string[] PostalAddressLine2Column = new string[] { "PostalAddressLine2" };
        public static string[] PostalCityColumn = new string[] { "PostalCity" };
        public static string[] PostalTownColumn = new string[] { "PostalTown" };
        public static string[] PostalStateColumn = new string[] { "PostalState" };
        public static string[] PostalCountryColumn = new string[] { "PostalCountry" };
        public static string[] PostalZipCodeColumn = new string[] { "PostalZipCode" };
        public static string[] OfficeAddressLine1Column = new string[] { "OfficeAddressLine1" };
        public static string[] OfficeAddressLine2Column = new string[] { "OfficeAddressLine2" };
        public static string[] OfficeCityColumn = new string[] { "OfficeCity" };
        public static string[] OfficeTownColumn = new string[] { "OfficeTown" };
        public static string[] OfficeStateColumn = new string[] { "OfficeState" };
        public static string[] OfficeCountryColumn = new string[] { "OfficeCountry" };
        public static string[] OfficeZipCodeColumn = new string[] { "OfficeZipCode" };
        public static string[] ContactNameColumn = new string[] { "ContactName" };
        public static string[] ContactLastNameColumn = new string[] { "ContactLastName  " };
        public static string[] ContactEmailColumn = new string[] { "ContactEmail" };
        public static string[] ContactPhoneColumn = new string[] { "ContactPhone" };
        public static string[] ContactPositionColumn = new string[] { "ContactPosition" };
        public static string[] ReferenceIDColumn = new string[] { "ReferenceID" };

        public const string S_ImportAccountData = "dtAccountSession";
        public const string S_ImportAccount = "dtAccountTypeSession";
        #endregion

        #region public methods
        public List<ValidAccountStatement> ImportAccountValid(ValidAccountModel ImportViewAccount)
        {
            var url = "api/Account/ValidateImportAccount";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccount);
            List<ValidAccountStatement> model1 = new List<ValidAccountStatement>();
            model1 = objRestSharpHandler.RestRequest<List<ValidAccountStatement>>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return model1;
        }

        public int ImportAccountInsert(ValidAccountModel ImportViewAccount)
        {
            var url = "api/Account/InsertImportAccount";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccount);

            var result = objRestSharpHandler.RestRequest<int>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return result;
        }
        #endregion
    }
    public class ImportViewAccountModel
    {
        #region public properties
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string BusinessDescription { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
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
        public string PostalTown { get; set; }
        public string PostalState { get; set; }
        public string PostalCountry { get; set; }
        public string PostalZipCode { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeAddressLine2 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeTown { get; set; }
        public string OfficeState { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficeZipCode { get; set; }
        public string ContactName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPosition { get; set; }
        public string ReferenceID { get; set; }

        #endregion
    }
    public class ImportAccountFieldModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string FileName { get; set; }
        public int type { get; set; }
        #endregion
    }
    public class ValidAccountStatement
    {
        #region public properties
        public string GroupCode { get; set; }
        public string Reason { get; set; }
        #endregion
    }
    public class AccountMappingViewModel
    {
        #region public properties
        public string ImportID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string GroupCode { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string AccountType { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string DisplayName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string BusinessDescription { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Email { get; set; }
        public string Website { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Currency { get; set; }
        public string IDType { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TaxNumber { get; set; }
        public string StateNumber { get; set; }
        public string TreasuryNumber { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string InitialDate { get; set; }
        public string NAICSCode { get; set; }
        public string BusinessType { get; set; }
        public string Zone { get; set; }
        public string ARPEDescription { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string PostalAddressLine1 { get; set; }
        public string PostalAddressLine2 { get; set; }
        public string PostalCity { get; set; }
        public string PostalTown { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string PostalState { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string PostalCountry { get; set; }
        public string PostalZipCode { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeAddressLine2 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeTown { get; set; }
        public string OfficeState { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficeZipCode { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ContactName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ContactPosition { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FileName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Salutation { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string LastName { get; set; }
        public int type { get; set; }
        public string DateFormat { get; set; }
        public string ReferenceID { get; set; }
        public IEnumerable<SelectListItem> HeaderColumnList { get; set; }
        public IEnumerable<SelectListItem> DateFormatList { get; set; }
        #endregion

        #region Constructor
        public AccountMappingViewModel()
        {
            HeaderColumnList = new List<SelectListItem>();
            string __dateFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateFormatList = new SelectList(new List<SelectListItem> {
                                    new SelectListItem{ Text="MM/dd/yyyy", Value = "MM/dd/yyyy" },
                                    new SelectListItem{ Text="MM-dd-yyyy", Value = "MM-dd-yyyy" },
                                    new SelectListItem{ Text="MM.dd.yyyy", Value = "MM.dd.yyyy" },

                                    new SelectListItem{ Text="dd/MM/yyyy", Value = "dd/MM/yyyy" },
                                    new SelectListItem{ Text="dd-MM-yyyy", Value = "dd-MM-yyyy" },
                                    new SelectListItem{ Text="dd.MM.yyyy", Value = "dd.MM.yyyy" },

                                    new SelectListItem{ Text="yyyy/MM/dd", Value = "yyyy/MM/dd" },
                                    new SelectListItem{ Text="yyyy-MM-dd", Value = "yyyy-MM-dd" },
                                    new SelectListItem{ Text="yyyy.MM.dd", Value = "yyyy.MM.dd" },

                                    new SelectListItem{ Text="yyyy/dd/MM", Value = "yyyy/dd/MM" },
                                    new SelectListItem{ Text="yyyy-dd-MM", Value = "yyyy-dd-MM" },
                                    new SelectListItem{ Text="yyyy.dd.MM", Value = "yyyy.dd.MM" },

                                    new SelectListItem{ Text="MM/dd/yy", Value = "MM/dd/yy" },
                                    new SelectListItem{ Text="MM-dd-yy", Value = "MM-dd-yy" },
                                    new SelectListItem{ Text="MM.dd.yy", Value = "MM.dd.yy" },

                                    new SelectListItem{ Text="dd/MM/yy", Value = "dd/MM/yy" },
                                    new SelectListItem{ Text="dd-MM-yy", Value = "dd-MM-yy" },
                                    new SelectListItem{ Text="dd.MM.yy", Value = "dd.MM.yy" },

                                    new SelectListItem{ Text="yy/MM/dd", Value = "yy/MM/dd" },
                                    new SelectListItem{ Text="yy-MM-dd", Value = "yy-MM-dd" },
                                    new SelectListItem{ Text="yy.MM.dd", Value = "yy.MM.dd" },

                                    new SelectListItem{ Text="yy/dd/MM", Value = "yy/dd/MM" },
                                    new SelectListItem{ Text="yy-dd-MM", Value = "yy-dd-MM" },
                                    new SelectListItem{ Text="yy.dd.MM", Value = "yy.dd.MM" },
            }, "Value", "Text", __dateFormat);
        }

        public AccountMappingViewModel(string fileName, int type, DataTable importDataTable)
        {
            this.FileName = fileName;
            this.type = type;
            this.HeaderColumnList = new SelectList(importDataTable.Columns.Cast<DataColumn>()
                      .Select(x => new { Name = x.ColumnName }).ToList(), "Name", "Name");

            if (this.HeaderColumnList != null && this.HeaderColumnList.Any())
            {
                this.GroupCode = this.HeaderColumnList.Any(a => ImportAccountModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                            this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                            : "";
                this.AccountType = this.HeaderColumnList.Any(a => ImportAccountModel.AccountTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                            this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.AccountTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                            : "";
                this.Name = this.HeaderColumnList.Any(a => ImportAccountModel.NameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.NameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.DisplayName = this.HeaderColumnList.Any(a => ImportAccountModel.DisplayNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.DisplayNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.BusinessDescription = this.HeaderColumnList.Any(a => ImportAccountModel.BusinessDescriptionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.BusinessDescriptionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.Email = this.HeaderColumnList.Any(a => ImportAccountModel.EmailColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.EmailColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.Website = this.HeaderColumnList.Any(a => ImportAccountModel.WebSiteColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.WebSiteColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.WorkPhone = this.HeaderColumnList.Any(a => ImportAccountModel.WorkPhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.WorkPhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.MobilePhone = this.HeaderColumnList.Any(a => ImportAccountModel.MobilePhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.MobilePhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.Currency = this.HeaderColumnList.Any(a => ImportAccountModel.CurrencyColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.CurrencyColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.IDType = this.HeaderColumnList.Any(a => ImportAccountModel.IDTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.IDTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.TaxNumber = this.HeaderColumnList.Any(a => ImportAccountModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.StateNumber = this.HeaderColumnList.Any(a => ImportAccountModel.StateNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.StateNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.TreasuryNumber = this.HeaderColumnList.Any(a => ImportAccountModel.TreasuryNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.TreasuryNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.InitialDate = this.HeaderColumnList.Any(a => ImportAccountModel.InitialDateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.InitialDateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.NAICSCode = this.HeaderColumnList.Any(a => ImportAccountModel.NAICSCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.NAICSCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.BusinessType = this.HeaderColumnList.Any(a => ImportAccountModel.BusinessTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.BusinessTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.Zone = this.HeaderColumnList.Any(a => ImportAccountModel.ZoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ZoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.ARPEDescription = this.HeaderColumnList.Any(a => ImportAccountModel.ARPEDescriptionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ARPEDescriptionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.PostalAddressLine1 = this.HeaderColumnList.Any(a => ImportAccountModel.PostalAddressLine1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalAddressLine1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.PostalAddressLine2 = this.HeaderColumnList.Any(a => ImportAccountModel.PostalAddressLine2Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalAddressLine2Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.PostalCity = this.HeaderColumnList.Any(a => ImportAccountModel.PostalCityColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalCityColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.PostalTown = this.HeaderColumnList.Any(a => ImportAccountModel.PostalTownColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalTownColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.PostalState = this.HeaderColumnList.Any(a => ImportAccountModel.PostalStateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalStateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.PostalCountry = this.HeaderColumnList.Any(a => ImportAccountModel.PostalCountryColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalCountryColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.PostalZipCode = this.HeaderColumnList.Any(a => ImportAccountModel.PostalZipCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.PostalZipCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeAddressLine1 = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeAddressLine1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeAddressLine1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeAddressLine2 = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeAddressLine2Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeAddressLine2Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeCity = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeCityColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeCityColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeTown = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeTownColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeTownColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeState = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeStateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeStateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeCountry = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeCountryColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeCountryColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.OfficeZipCode = this.HeaderColumnList.Any(a => ImportAccountModel.OfficeZipCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.OfficeZipCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.ContactName = this.HeaderColumnList.Any(a => ImportAccountModel.ContactNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ContactNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.ContactLastName = this.HeaderColumnList.Any(a => ImportAccountModel.ContactLastNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ContactLastNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.ContactEmail = this.HeaderColumnList.Any(a => ImportAccountModel.ContactEmailColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ContactEmailColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.ContactPhone = this.HeaderColumnList.Any(a => ImportAccountModel.ContactPhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ContactPhoneColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.ContactPosition = this.HeaderColumnList.Any(a => ImportAccountModel.ContactPositionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ContactPositionColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.FirstName = this.HeaderColumnList.Any(a => ImportAccountModel.FirstNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                     this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.FirstNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                     : "";
                this.LastName = this.HeaderColumnList.Any(a => ImportAccountModel.LastNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.LastNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.Salutation = this.HeaderColumnList.Any(a => ImportAccountModel.SalutationColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.SalutationColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.ReferenceID = this.HeaderColumnList.Any(a => ImportAccountModel.ReferenceIDColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountModel.ReferenceIDColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
            }
        }
        #endregion
    }
    public class InvalidAccountModel
    {
        #region public properties
        public bool Valid { get; set; }
        public string FileName { get; set; }
        public int AccType { get; set; }
        public List<ValidAccountStatement> ValidAccountStatement { get; set; }
        #endregion
    }
    public class ValidAccountModel
    {
        #region public properties
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AccountType { get; set; }
        public List<ImportViewAccountModel> ImportList { get; set; }
        #endregion

        #region Constructor
        public ValidAccountModel()
        { }
        public ValidAccountModel(DataTable importDataTable, AccountMappingViewModel accountMappingModel)
        {
            this.CompanyID = UserSession.Current.CompanyID;
            this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.CreatedUser = this.ModifiedUser = UserSession.Current.Id;
            this.IsValidate = true;
            this.AccountType = accountMappingModel.type;
            this.ImportList = new ImportModel().GetAccountListFromImportDatatTable(importDataTable, accountMappingModel);
        }
        #endregion
    }
    public class FinishViewAccountModel
    {
        #region public properties
        public List<string> HeaderColumnList { get; set; } = new List<string>();
        public List<ImportViewAccountModel> ImportaccountViewModel { get; set; }
        public int AccountType { get; set; }
        #endregion

        #region Constructor
        public FinishViewAccountModel()
        { }
        public FinishViewAccountModel(DataTable importDataTable, AccountMappingViewModel accountMappingModel)
        {
            this.HeaderColumnList = importDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            this.ImportaccountViewModel = new ImportModel().GetAccountListFromImportDatatTable(importDataTable, accountMappingModel);
            this.AccountType = accountMappingModel.type;
        }
        #endregion
    }
}
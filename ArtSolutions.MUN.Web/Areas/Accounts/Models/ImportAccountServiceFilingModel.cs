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
    public class ImportAccountServiceFilingModel
    {
        #region public properties
        public static string[] GroupCodeColumn = new string[] { "GroupCode", "GROUPCODE" };
        public static string[] YearColumn = new string[] { "Year" };
        public static string[] MonthColumn = new string[] { "Month" };
        public static string[] ServiceCodeColumn = new string[] { "ServiceCode" };
        public static string[] ServiceNameColumn = new string[] { "ServiceName" };
        public static string[] TaxNumberColumn = new string[] { "TaxNumber" };
        public static string[] CustomField1Column = new string[] { "CustomField1" };
        public static string[] LastReadingColumn = new string[] { "LastReading" };
        public static string[] CurrentReadingColumn = new string[] { "CurrentReading" };
        public static string[] DifferenceColumn = new string[] { "Difference" };

        public const string S_ImportAccountData = "dtAccountSession";
        public const string S_ImportAccount = "dtAccountTypeSession";
        #endregion

        #region public methods
        public List<ValidAccountServiceFilingStatement> ImportAccountServiceFilingValid(ValidAccountServiceFilingModel ImportViewAccount)
        {
            ImportViewAccount.CreatedBy = UserSession.Current.Username;

            var url = "api/AccountService/ValidateImportAccountServiceFiling";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccount);
            List<ValidAccountServiceFilingStatement> model1 = new List<ValidAccountServiceFilingStatement>();
            model1 = objRestSharpHandler.RestRequest<List<ValidAccountServiceFilingStatement>>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return model1;
        }

        public int ImportAccountServiceFilingInsert(ValidAccountServiceFilingModel ImportViewAccount)
        {
            ImportViewAccount.CreatedBy = UserSession.Current.Username;

            var url = "api/AccountService/InsertImportAccountServiceFiling";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccount);

            var result = objRestSharpHandler.RestRequest<int>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return result;
        }
        #endregion
    }
    public class ImportViewAccountServiceFilingModel
    {
        #region public properties
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string CustomField1 { get; set; }
        public string LastReading { get; set; }
        public string CurrentReading { get; set; }
        public string Difference { get; set; }
        public string TaxNumber { get; set; }
        #endregion
    }
    public class ImportAccountServiceFilingFieldModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string FileName { get; set; }
        #endregion

        #region public methods
        public int ImportFile(ImportAccountServiceFilingFieldModel model)
        {
            return 0;
        }
        #endregion
    }
    public class ValidAccountServiceFilingStatement
    {
        #region public properties
        public string GroupCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CustomField1 { get; set; }
        public string TaxNumber { get; set; }
        public string Reason { get; set; }
        #endregion
    }
    public class AccountServiceFilingMappingViewModel
    {
        #region public properties
        public string ImportID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string GroupCode { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Year { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Month { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ServiceCode { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string ServiceName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TaxNumber { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField1 { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string LastReading { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CurrentReading { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Difference { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FileName { get; set; }
        public IEnumerable<SelectListItem> HeaderColumnList { get; set; }
        public IEnumerable<SelectListItem> DateFormatList { get; set; }
        #endregion

        #region Constructor
        public AccountServiceFilingMappingViewModel()
        {
            HeaderColumnList = new List<SelectListItem>();
        }

        public AccountServiceFilingMappingViewModel(string fileName, DataTable importDataTable)
        {
            this.FileName = fileName;
            this.HeaderColumnList = new SelectList(importDataTable.Columns.Cast<DataColumn>()
                      .Select(x => new { Name = x.ColumnName }).ToList(), "Name", "Name");

            if (this.HeaderColumnList != null && this.HeaderColumnList.Any())
            {
                this.GroupCode = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                           this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                           : "";
                this.Year = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.YearColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                            this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.YearColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                            : "";
                this.Month = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.MonthColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                            this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.MonthColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                            : "";
                this.ServiceCode = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.ServiceCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.ServiceCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.ServiceName = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.ServiceNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.ServiceNameColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.TaxNumber = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                        this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                        : "";
                this.CustomField1 = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.CustomField1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.CustomField1Column.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.LastReading = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.LastReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.LastReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.CurrentReading = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.CurrentReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.CurrentReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
                this.Difference = this.HeaderColumnList.Any(a => ImportAccountServiceFilingModel.DifferenceColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                       this.HeaderColumnList.FirstOrDefault(a => ImportAccountServiceFilingModel.DifferenceColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                       : "";
            }
        }

        #endregion
    }
    public class InvalidAccountServiceFilingModel
    {
        #region public properties
        public bool Valid { get; set; }
        public string FileName { get; set; }
        public List<ValidAccountServiceFilingStatement> ValidAccountStatement { get; set; }
        #endregion
    }
    public class ValidAccountServiceFilingModel
    {
        #region public properties
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<ImportViewAccountServiceFilingModel> ImportList { get; set; }
        #endregion

        #region Constructor
        public ValidAccountServiceFilingModel()
        { }
        public ValidAccountServiceFilingModel(DataTable importDataTable, AccountServiceFilingMappingViewModel MappingModel)
        {
            this.CompanyID = UserSession.Current.CompanyID;
            this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.CreatedUser = this.ModifiedUser = UserSession.Current.Id;
            this.IsValidate = true;
            this.ImportList = new ImportModel().GetAccountServiceFilingListFromImportDatatTable(importDataTable, MappingModel);
        }
        #endregion
    }
    public class FinishViewAccountServiceFilingModel
    {
        #region public properties
        public List<string> HeaderColumnList { get; set; } = new List<string>();
        public List<ImportViewAccountServiceFilingModel> ImportaccountViewModel { get; set; }
        #endregion

        #region Constructor
        public FinishViewAccountServiceFilingModel()
        { }
        public FinishViewAccountServiceFilingModel(DataTable importDataTable, AccountServiceFilingMappingViewModel MappingModel)
        {
            this.HeaderColumnList = importDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            this.ImportaccountViewModel = new ImportModel().GetAccountServiceFilingListFromImportDatatTable(importDataTable, MappingModel);
        }
        #endregion
    }
}
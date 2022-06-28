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
    public class ImportMeasuredWaterFilingModel
    {
        #region public properties        
        public static string[] RowNoColumn = new string[] { "RowNo" };
        public static string[] YearColumn = new string[] { "Year" };
        public static string[] MonthColumn = new string[] { "Month" };
        public static string[] UBICACIONColumn = new string[] { "UBICACION" };
        public static string[] CODIGOMColumn = new string[] { "CODIGOM" };
        public static string[] CATEGORIAColumn = new string[] { "CATEGORIA" };
        public static string[] LECTURAACTColumn = new string[] { "LECTURAACT" };
        public static string[] FECHAColumn = new string[] { "FECHA" };
        public static string[] TaxNumberColumn = new string[] { "TaxNumber" };
        public static string[] LastReadingColumn = new string[] { "LastReading" };
        public static string[] DifferenceColumn = new string[] { "Difference" };
        public static string[] NoteColumn = new string[] { "Note" };
        public static string[] IsValidColumn = new string[] { "IsValid" };
        public static string[] IsEditableColumn = new string[] { "IsEditable" };


        public const string S_ImportMeasuredWaterFilingData = "dtMeasuredWaterFilingSession";
        public const string S_ValidateMeasuredWaterFilingData = "dtValidateMeasuredWaterFilingSession";
        #endregion

        #region public methods   
        public ValidMeasuredWaterFilingModel GetValidateMeasuredWaterFilingForFixFile(int? periodYear, int? periodMonth, bool isGenerateFile, bool isInconsistencies, bool isHighConsumption, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "periodYear", Value = periodYear });
            lstParameter.Add(new NameValuePair { Name = "periodMonth", Value = periodMonth });
            lstParameter.Add(new NameValuePair { Name = "isGenerateFile", Value = isGenerateFile });
            lstParameter.Add(new NameValuePair { Name = "isInconsistencies", Value = isInconsistencies });
            lstParameter.Add(new NameValuePair { Name = "isHighConsumption", Value = isHighConsumption });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<ValidMeasuredWaterFilingModel>(APISelector.Municipality, true, "api/AccountService/GetValidateMeasuredWaterFilingForFixFile", "GET", lstParameter);
        }
        public DataTable InsertValidateMeasuredWaterFilingForFixFile(ValidMeasuredWaterFilingModel ImportViewMeasuredWaterFiling)
        {
            var url = "api/AccountService/InsertValidateMeasuredWaterFilingForFixFile";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewMeasuredWaterFiling);
            return objRestSharpHandler.RestRequest<DataTable>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
        }
        public DataTable UpdateValidateMeasuredWaterFilingForFixFile(ValidMeasuredWaterFilingModel ImportViewMeasuredWaterFiling)
        {
            var url = "api/AccountService/UpdateValidateMeasuredWaterFilingForFixFile";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewMeasuredWaterFiling);
            return objRestSharpHandler.RestRequest<DataTable>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
        }
        public ValidateMeasuredWaterAccountServiceFilingModel GetAccountServiceForValidateMeasuredWaterFiling(ValidMeasuredWaterFilingModel ImportViewAccount)
        {
            ImportViewAccount.CreatedBy = UserSession.Current.Username;

            var url = "api/AccountService/GetAccountServiceForValidateMeasuredWaterFiling";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccount);
            return objRestSharpHandler.RestRequest<ValidateMeasuredWaterAccountServiceFilingModel>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
        }
        public DataTable ImportMeasuredWaterFilingValid(ValidMeasuredWaterFilingModel ImportViewMeasuredWaterFiling)
        {
            var url = "api/AccountService/ValidateImportMeasuredWaterFiling";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewMeasuredWaterFiling);
            return objRestSharpHandler.RestRequest<DataTable>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
        }
        public int ImportMeasuredWaterFilingInsert(ValidMeasuredWaterFilingModel ImportViewMeasuredWaterFiling)
        {
            ImportViewMeasuredWaterFiling.CreatedUser = UserSession.Current.Id;
            ImportViewMeasuredWaterFiling.CreatedDate = Common.CurrentDateTime;
            ImportViewMeasuredWaterFiling.ModifiedUser = UserSession.Current.Id;
            ImportViewMeasuredWaterFiling.ModifiedDate = Common.CurrentDateTime;
            ImportViewMeasuredWaterFiling.CreatedBy = UserSession.Current.Username;

            var url = "api/AccountService/InsertImportMeasuredWaterFiling";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewMeasuredWaterFiling);
            var result = objRestSharpHandler.RestRequest<int>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return result;
        }
        #endregion
    }
    public class ImportViewMeasuredWaterFilingModel
    {
        #region public properties
        public int ImportID { get; set; }
        public int RowNo { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CATEGORIA { get; set; }
        public string TaxNumber { get; set; }
        public string UBICACION { get; set; }
        public string CODIGOM { get; set; }
        public string LECTURAACT { get; set; }
        public string LastReading { get; set; }
        public string Difference { get; set; }
        public string FECHA { get; set; }
        public string Note { get; set; }
        public string IsValid { get; set; }
        public string IsEditable { get; set; }
        #endregion
    }
    public class ImportMeasuredWaterFilingFieldModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string FileName { get; set; }
        public int type { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public DateTime PeriodDate { get; set; }
        public string UploadedFileNames { get; set; }
        public bool IsFileFixed { get; set; }
        public bool IsRevalidate { get; set; }
        public List<ImportViewMeasuredWaterFilingModel> ImportList { get; set; }
        public List<ValidImportMeasuredWaterFilingFixFileModel> FileDataList { get; set; }
        public List<MeasuredWaterFilingFixFileModel> MeasuredWaterFilingList { get; set; }
        public MeasuredWaterFilingFixFileModel MeasuredWaterFilingModel { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsLoadMore { get; set; }
        public string MeasuredWaterFilingHtmlString { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        #endregion
    }
    //public class ValidMeasuredWaterFilingStatement
    //{
    //    #region public properties      
    //    public string Year { get; set; }
    //    public string Month { get; set; }
    //    public string CATEGORIA { get; set; }
    //    public string TaxNumber { get; set; }
    //    public string UBICACION { get; set; }
    //    public string CODIGOM { get; set; }
    //    public decimal? LECTURAACT { get; set; }
    //    public decimal? LastReading { get; set; }
    //    public decimal? Difference { get; set; }
    //    public string Note { get; set; }
    //    public bool IsValid { get; set; }
    //    public string FECHA { get; set; }
    //    #endregion
    //}
    public class MeasuredWaterFilingMappingViewModel
    {
        #region public properties
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string RowNo { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Year { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Month { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string UBICACION { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CODIGOM { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CATEGORIA { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string LECTURAACT { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FECHA { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TaxNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string LastReading { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Difference { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Note { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string IsValid { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string IsEditable { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FileName { get; set; }
        public IEnumerable<SelectListItem> HeaderColumnList { get; set; }
        public IEnumerable<SelectListItem> DateFormatList { get; set; }

        #endregion

        #region Constructor
        public MeasuredWaterFilingMappingViewModel()
        {
            HeaderColumnList = new List<SelectListItem>();
        }
        public MeasuredWaterFilingMappingViewModel(string fileName, int year, int month, DataTable importDataTable)
        {
            this.FileName = fileName;
            this.PeriodYear = year;
            this.PeriodMonth = month;

            this.HeaderColumnList = new SelectList(importDataTable.Columns.Cast<DataColumn>()
                      .Select(x => new { Name = x.ColumnName }).ToList(), "Name", "Name");

            if (this.HeaderColumnList != null && this.HeaderColumnList.Any())
            {
                this.RowNo = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.RowNoColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.RowNoColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.Year = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.YearColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                           this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.YearColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                           : "";

                this.Month = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.MonthColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                         this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.MonthColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                         : "";

                this.UBICACION = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.UBICACIONColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.UBICACIONColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.CODIGOM = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.CODIGOMColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                           this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.CODIGOMColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                           : "";

                this.CATEGORIA = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.CATEGORIAColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.CATEGORIAColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.LECTURAACT = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.LECTURAACTColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.LECTURAACTColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.FECHA = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.FECHAColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                 this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.FECHAColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                 : "";

                this.TaxNumber = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                              this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                              : "";
                this.LastReading = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.LastReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                              this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.LastReadingColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                              : "";
                this.Difference = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.DifferenceColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                              this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.DifferenceColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                              : "";
                this.Note = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.NoteColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                             this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.NoteColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                             : "";
                this.IsValid = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.IsValidColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                             this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.IsValidColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                             : "";
                this.IsEditable = this.HeaderColumnList.Any(a => ImportMeasuredWaterFilingModel.IsEditableColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                            this.HeaderColumnList.FirstOrDefault(a => ImportMeasuredWaterFilingModel.IsEditableColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                            : "";
            }
        }
        #endregion
    }
    public class InvalidMeasuredWaterFilingModel
    {
        #region public properties
        public bool IsReValidate { get; set; }
        public bool IsValidate { get; set; }
        public bool Valid { get; set; }
        public string FileName { get; set; }
        public DataTable ValidMeasuredWaterFilingStatement { get; set; }
        public DataTable InValidMeasuredWaterFilingStatement { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public string UploadedFileNames { get; set; }
        #endregion
    }
    public class ValidMeasuredWaterFilingModel
    {
        #region public properties
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<ImportViewMeasuredWaterFilingModel> ImportList { get; set; }
        public List<ValidImportMeasuredWaterFilingFixFileModel> MeasuredWaterFilingList { get; set; }
        public DataTable FileDataList { get; set; }
        public int? ErrorCode { get; set; }
        public bool Valid { get; set; }
        public bool IsReValidate { get; set; }
        public string UploadedFileNames { get; set; }
        public string FileName { get; set; }
        public bool? IsValid { get; set; }
        public bool ValidateCustomField { get; set; }
        public int PageSize { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEdit { get; set; }
        public int TotalRecord { get; set; }
        public int? RowNo { get; set; }
        public int? CurrentReading { get; set; }
        public DateTime? ProcessDate { get; set; }
        #endregion

        #region Constructor
        public ValidMeasuredWaterFilingModel() { }
        public ValidMeasuredWaterFilingModel(DataTable importDataTable, MeasuredWaterFilingMappingViewModel model, bool isValidate, bool isForFixFile, List<ImportViewMeasuredWaterFilingModel> importList)
        {
            this.CompanyID = UserSession.Current.CompanyID;
            this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.CreatedUser = this.ModifiedUser = UserSession.Current.Id;
            this.IsValidate = isValidate;
            this.PeriodYear = model.PeriodYear;
            this.PeriodMonth = model.PeriodMonth;

            if (importList == null)
                this.ImportList = new ImportModel().GetMeasuredWaterFilingListFromImportDatatTable(importDataTable, model, isForFixFile);
            else
                this.ImportList = importList;
        }
        #endregion
    }
    public class FinishViewMeasuredWaterFilingModel
    {
        #region public properties
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public List<string> HeaderColumnList { get; set; } = new List<string>();
        public List<ImportViewMeasuredWaterFilingModel> ImportMeasuredWaterFilingViewModel { get; set; }
        public DataTable ValidMeasuredWaterFilingStatement { get; set; }
        #endregion

        #region Constructor
        public FinishViewMeasuredWaterFilingModel()
        { }
        public FinishViewMeasuredWaterFilingModel(DataTable importDataTable, MeasuredWaterFilingMappingViewModel model)
        {
            this.PeriodYear = model.PeriodYear;
            this.PeriodMonth = model.PeriodMonth;
            this.HeaderColumnList = importDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            this.ImportMeasuredWaterFilingViewModel = new ImportModel().GetMeasuredWaterFilingListFromImportDatatTable(importDataTable, model, false);
        }
        #endregion
    }
    public class MeasuredWaterFilingReadImportFileModel
    {
        public DataTable MeasuredWaterFilingList { get; set; }
        public bool IsValidData { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
    public class ValidImportMeasuredWaterFilingFixFileModel
    {
        #region public properties
        public int RowNo { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CATEGORIA { get; set; }
        public string TaxNumber { get; set; }
        public string UBICACION { get; set; }
        public string CODIGOM { get; set; }
        public string LECTURAACT { get; set; }
        public string LastReading { get; set; }
        public string Difference { get; set; }
        public string FECHA { get; set; }
        public string Note { get; set; }
        public string IsValid { get; set; }
        public bool IsEditable { get; set; }
        #endregion
    }
    public class MeasuredWaterFilingFixFileModel
    {
        #region public properties      
        public string CATEGORIA { get; set; }
        public string TaxNumber { get; set; }
        public string UBICACION { get; set; }
        public string CODIGOM { get; set; }
        public string LECTURAACT { get; set; }
        public string LastReading { get; set; }
        public string Difference { get; set; }
        public string FECHA { get; set; }
        public string Note { get; set; }
        public string IsValid { get; set; }
        public int RowNo { get; set; }
        #endregion
    }
    public class ValidateMeasuredWaterAccountServiceFilingModel
    {
        #region public properties
        public DataTable AccountServiceList { get; set; }
        //public DataTable InvalidDetailList { get; set; }
        public DataTable ValidDetailList { get; set; }
        public DataTable AccountServiceNotExistList { get; set; }
        #endregion
    }
}
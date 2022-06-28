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
    public class ImportAccountPropertyModel
    {
        #region public properties
        public static string[] GroupCodeColumn = new string[] { "GroupCode", "GROUPCODE" };
        public static string[] RightNumberColumn = new string[] { "RightNumber", "Right Number" };
        public static string[] DuplicateColumn = new string[] { "Duplicate" };
        public static string[] HorizontalColumn = new string[] { "Horizontal" };
        public static string[] RightCodeColumn = new string[] { "RightCode", "Right Code" };
        public static string[] TaxNumberColumn = new string[] { "TaxNumber" };
        public static string[] TerrainAreaColumn = new string[] { "TerrainArea", "Terrain Area (m²)" };
        public static string[] PercentageColumn = new string[] { "Percentage" };
        public static string[] MaterialTypeColumn = new string[] { "MaterialType", "Type" };
        public static string[] ConstructionTypeColumn = new string[] { "ConstructionType", "Construction Type" };
        public static string[] CurrentAgeColumn = new string[] { "CurrentAge", "Current Age" };
        public static string[] BuildingAreaColumn = new string[] { "BuildingArea", "Building Area (m²)" };

        public const string S_ImportAccountPropertyData = "dtAccountPropertySession";
        #endregion

        #region public methods
        public List<ValidAccountPropertyStatement> ImportAccountPropertyValid(ValidAccountPropertyModel ImportViewAccountProperty)
        {
            ImportViewAccountProperty.ImportViewAccountPropertyList = ImportViewAccountProperty.ImportViewAccountPropertyDetailWithErrorModel.ImportList;

            var url = "api/AccountProperty/ValidateImportAccountProperty";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewAccountProperty);
            List<ValidAccountPropertyStatement> model1 = new List<ValidAccountPropertyStatement>();
            model1 = objRestSharpHandler.RestRequest<List<ValidAccountPropertyStatement>>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return model1;
        }
        public int ImportAccountPropertyInsert(ValidAccountPropertyModel model)
        {
            if (model.ImportViewAccountPropertyDetailWithErrorModel != null && model.ImportViewAccountPropertyDetailWithErrorModel.ImportList != null)
                model.ImportViewAccountPropertyList = model.ImportViewAccountPropertyDetailWithErrorModel.ImportList;
            var url = "api/AccountProperty/InsertImportAccountProperty";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            var result = objRestSharpHandler.RestRequest<int>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return result;
        }
        #endregion
    }
    public class ImportViewAccountPropertyModel
    {
        #region public properties
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string RightNumber { get; set; }
        public string Duplicate { get; set; }
        public string Horizontal { get; set; }
        public string RightCode { get; set; }
        public string TaxNumber { get; set; }
        public decimal? TerrainArea { get; set; }
        public decimal? Percentage { get; set; }
        public string MaterialType { get; set; }
        public string ConstructionType { get; set; }
        public int? CurrentAge { get; set; }
        public decimal? BuildingArea { get; set; }

        #endregion
    }
    public class ImportAccountPropertyFieldModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string FileName { get; set; }
        public int type { get; set; }
        #endregion
    }
    public class ValidAccountPropertyStatement
    {
        #region public properties
        public string GroupCode { get; set; }
        public string Reason { get; set; }
        #endregion
    }
    public class AccountPropertyMappingViewModel
    {
        #region public properties
        public string ImportID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string GroupCode { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string RightNumber { get; set; }
        public string Duplicate { get; set; }
        public string Horizontal { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string RightCode { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TaxNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TerrainArea { get; set; }
        public string Percentage { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string MaterialType { get; set; }
        public string ConstructionType { get; set; }
        public string CurrentAge { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string BuildingArea { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FileName { get; set; }
        public IEnumerable<SelectListItem> HeaderColumnList { get; set; }
        #endregion

        #region Constructor
        public AccountPropertyMappingViewModel()
        {
            HeaderColumnList = new List<SelectListItem>();
        }
        public AccountPropertyMappingViewModel(string fileName, int type, DataTable importDataTable)
        {
            this.FileName = fileName;
            this.HeaderColumnList = new SelectList(importDataTable.Columns.Cast<DataColumn>()
                      .Select(x => new { Name = x.ColumnName }).ToList(), "Name", "Name");

            if (this.HeaderColumnList != null && this.HeaderColumnList.Any())
            {
                this.GroupCode = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                               this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.GroupCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                               : "";
                this.RightNumber = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.RightNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                 this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.RightNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                 : "";
                this.Duplicate = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.DuplicateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                               this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.DuplicateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                               : "";
                this.Horizontal = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.HorizontalColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.HorizontalColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                : "";
                this.RightCode = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.RightCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                               this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.RightCodeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                               : "";
                this.TaxNumber = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                           this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                           : "";
                this.TerrainArea = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.TerrainAreaColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                 this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.TerrainAreaColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                 : "";
                this.Percentage = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.PercentageColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.PercentageColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                : "";
                this.MaterialType = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.MaterialTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                         this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.MaterialTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                         : "";
                this.ConstructionType = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.ConstructionTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                      this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.ConstructionTypeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                      : "";
                this.CurrentAge = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.CurrentAgeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.CurrentAgeColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                : "";
                this.BuildingArea = this.HeaderColumnList.Any(a => ImportAccountPropertyModel.BuildingAreaColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                  this.HeaderColumnList.FirstOrDefault(a => ImportAccountPropertyModel.BuildingAreaColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                  : "";
            }
        }
        #endregion
    }
    public class InvalidAccountPropertyModel
    {
        #region public properties
        public bool Valid { get; set; }
        public string FileName { get; set; }
        public List<ValidAccountPropertyStatement> ValidAccountPropertyStatement { get; set; }
        #endregion
    }
    public class ValidAccountPropertyModel
    {
        #region public properties
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ImportViewAccountPropertyModel> ImportViewAccountPropertyList { get; set; }
        public ImportViewAccountPropertyDetailWithErrorModel ImportViewAccountPropertyDetailWithErrorModel { get; set; }
        #endregion

        #region Constructor
        public ValidAccountPropertyModel()
        { }
        public ValidAccountPropertyModel(DataTable importDataTable, AccountPropertyMappingViewModel accountPropertyMappingModel)
        {
            this.CompanyID = UserSession.Current.CompanyID;
            this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.CreatedUser = this.ModifiedUser = UserSession.Current.Id;
            this.IsValidate = true;
            this.ImportViewAccountPropertyDetailWithErrorModel = new ImportModel().GetAccountPropertyListFromImportDatatTable(importDataTable, accountPropertyMappingModel);
        }
        #endregion
    }
    public class FinishViewAccountPropertyModel
    {
        #region public properties
        public List<string> HeaderColumnList { get; set; } = new List<string>();
        public List<ImportViewAccountPropertyModel> ImportAccountPropertyViewModel { get; set; }
        #endregion

        #region Constructor
        public FinishViewAccountPropertyModel()
        { }
        public FinishViewAccountPropertyModel(DataTable importDataTable, AccountPropertyMappingViewModel accountPropertyMappingModel)
        {
            this.HeaderColumnList = importDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            ImportViewAccountPropertyDetailWithErrorModel model = new ImportModel().GetAccountPropertyListFromImportDatatTable(importDataTable, accountPropertyMappingModel);

            if (model != null)
                this.ImportAccountPropertyViewModel = model.ImportList;

        }
        #endregion
    }

    public class ImportViewAccountPropertyDetailWithErrorModel
    {
        public List<ImportViewAccountPropertyModel> ImportList { get; set; }

        public List<ValidAccountPropertyStatement> ErrorList { get; set; }

    }
}
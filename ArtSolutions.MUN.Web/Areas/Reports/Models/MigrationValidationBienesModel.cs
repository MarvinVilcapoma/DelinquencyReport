using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MigrationValidationBienesModel : DataExportModel
    {
        #region public properties      
        public List<MigrationValidationBienesList> MigrationValidationBienesList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public int? TypeID { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
        public string[] PropertyIDs { get; set; }
        public string FilterFincaIDs { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public MigrationValidationBienesModel()
        {
            this.TypeList = new List<SelectListItem>();
            this.AccountList = new List<SelectListItemViewModel>();
            this.PropertyList = new List<SelectListItem>();
        }
        #endregion
    }
    public class MigrationValidationBienesList
    {
        #region public properties   
        public int RowNo { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<int> AccountTypeID { get; set; }
        public Nullable<int> AccountPropertyID { get; set; }
        public string Cedula { get; set; }
        public string Finca { get; set; }
        public string Year { get; set; }
        public double TOTIMPONI_SIM { get; set; }
        public Nullable<decimal> TOTIMPONI_RULE { get; set; }
        public double IMPUESTO_SIM { get; set; }
        public Nullable<decimal> IMPUESTO_RULE { get; set; }
        public double IMPUEXENTO_SIM { get; set; }
        public Nullable<decimal> IMPUEXENTO_RULE { get; set; }
        public double PEREXONERA_SIM { get; set; }
        public Nullable<int> PEREXONERA_RULE { get; set; }
        public double CANCELADO_SIM { get; set; }
        public Nullable<decimal> CANCELADO_RULE { get; set; }
        public double PERIODOS_SIM { get; set; }
        public Nullable<int> PERIODOS_RULE { get; set; }
        #endregion
    }
}
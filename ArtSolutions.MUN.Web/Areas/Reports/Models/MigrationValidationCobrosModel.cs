using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MigrationValidationCobrosModel : DataExportModel
    {
        #region public properties      
        public List<MigrationValidationCobrosList> MigrationValidationCobrosList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public int? TypeID { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<SelectListItem> ServiceList { get; set; }
        public string[] ServiceIDs { get; set; }
        public string FilterServiceCodes { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public MigrationValidationCobrosModel()
        {
            this.TypeList = new List<SelectListItem>();
            this.AccountList = new List<SelectListItemViewModel>();
            this.ServiceList = new List<SelectListItem>();
        }
        #endregion
    }
    public class MigrationValidationCobrosList
    {
        #region public properties   
        public Nullable<int> AccountID { get; set; }
        public Nullable<int> AccountTypeID { get; set; }
        public string Cedula { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Segrega { get; set; }
        //public Nullable<int> CurrentYearRule { get; set; }
        //public Nullable<double> CurrentYearSIM { get; set; }
        //public Nullable<int> PastYearRule { get; set; }
        //public Nullable<double> PastYearSIM { get; set; }
        //public Nullable<decimal> CurrentYearRuleAmount { get; set; }
        //public Nullable<double> CurrentYearSIMAmount { get; set; }
        //public Nullable<decimal> PastYearRuleAmount { get; set; }
        //public Nullable<double> PastYearSIMAmount { get; set; }

        public Nullable<int> Rule { get; set; }
        public Nullable<int> SIM { get; set; }
        public Nullable<decimal> RuleAmount { get; set; }
        public Nullable<double> SIMAmount { get; set; }
        public string CustomField1 { get; set; }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountsConsumptionAndCollectionModel : DataExportModel
    {
        #region public properties    
        public List<AccountsConsumptionAndCollectionColumnModel> ColumnList { get; set; }
        public DataTable DomesticWaterServiceList { get; set; }
        public DataTable OrdinaryWaterServiceList { get; set; }
        public DataTable ReproductiveWaterServiceList { get; set; }
        public DataTable PreferedWaterServiceList { get; set; }
        public DataTable GovernmentWaterServiceList { get; set; }
        public DataTable SummaryList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public DateTime Period { get; set; }
        public string FormattedPeriod { get; set; }
        #endregion
    }
    public class AccountsConsumptionAndCollectionColumnModel
    {
        public string ColumnName { get; set; }
    }
}
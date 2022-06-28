using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionDailyIncomeStateModel
    {
        #region public properties
        public List<DailyIncomeStateModel> DailyIncomeStateList { get; set; }        
        public Int32 TotalRecord { get; set; }
        public DateTime StartPeriodDate { get; set; }        
        public int[] ServiceIDs { get; set; }
        public int ServiceID { get; set; }
        public string FilterServiceID { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
    public class DailyIncomeStateModel
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Charges { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Payment { get; set; }
    }
}
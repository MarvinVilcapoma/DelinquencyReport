using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class RoutaMissingServicesModel
    {
        #region public properties     
        public List<RoutaMissingServicesList> RoutaMissingServicesList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion
    }
    public class RoutaMissingServicesList
    {
        #region public properties
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public string Meter { get; set; }
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public int FiscalYearID { get; set; }
        public string ServiceName { get; set; }
        #endregion
    }
}
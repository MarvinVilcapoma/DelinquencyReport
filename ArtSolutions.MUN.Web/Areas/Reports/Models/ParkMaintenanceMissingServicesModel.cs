using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ParkMaintenanceMissingServicesModel
    {
        #region public properties     
        public List<ParkMaintenanceMissingServicesList> ParkMaintenanceMissingServicesList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int? Year { get; set; }
        public int TotalRecord { get; set; }
        #endregion
    }
    public class ParkMaintenanceMissingServicesList
    {
        #region public properties
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public decimal? PropertyTaxPrincipalAmount { get; set; }
        public string RightNumber { get; set; }
        public int? FiscalYearID { get; set; }
        public string Code { get; set; }
        public string ServiceName { get; set; }
        #endregion

        #region custom properties 
        public string FormattedPropertyTaxPrincipalAmount
        {
            get
            {
                return this.PropertyTaxPrincipalAmount == null ? null : this.PropertyTaxPrincipalAmount.Value.ToString("C");
            }
        }
        #endregion
    }
}
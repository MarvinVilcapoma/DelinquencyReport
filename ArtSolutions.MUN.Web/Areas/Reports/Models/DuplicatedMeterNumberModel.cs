using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DuplicatedMeterNumberModel
    {
        #region public properties     
        public List<DuplicatedMeterNumberList> DuplicatedMeterNumberList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion
    }
    public class DuplicatedMeterNumberList
    {
        #region public properties
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public string Meter { get; set; }
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public string ServiceName { get; set; }
        public bool IsLock { get; set; }
        #endregion

        #region custom properties 
        public string Status
        {
            get
            {
                return this.IsLock ? Resources.Report.Blocked : Resources.Report.NotBlocked;
            }
        }
        #endregion
    }
}
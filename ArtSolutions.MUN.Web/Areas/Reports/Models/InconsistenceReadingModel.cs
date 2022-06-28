using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class InconsistenceReadingModel
    {
        #region public properties
        public DateTime Period { get; set; }
        public List<InconsistenceReadingList> InconsistenceReadingList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public string FormattedPeriod { get; set; }
        public int TotalRecord { get; set; }
        public int TotalConsumption { get; set; }
        public decimal TotalAmount { get; set; }
        #endregion
    }

    public class InconsistenceReadingList
    {
        #region public properties  
        public string MeterNumber { get; set; }
        public string RigthNumber { get; set; }
        public string AccountID { get; set; }
        public string DisplayName { get; set; }
        public decimal LastReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal WaterConsumption { get; set; }
        public int FiscalYearID { get; set; }
        public Nullable<long> RowNo { get; set; }
        #endregion

        #region custom properties
        public string FormattedMeter
        {
            get
            {
                return MeterNumber == null ? "-" : (MeterNumber.Length > 25 ? MeterNumber.Substring(0, 25) + "..." : MeterNumber);
            }
        }
        public string FormattedRightNumber
        {
            get
            {
                return RigthNumber == null ? "-" : (RigthNumber.Length > 25 ? RigthNumber.Substring(0, 25) + "..." : RigthNumber);
            }
        }
        public string FormattedDisplayName
        {
            get
            {
                return DisplayName.Length > 40 ? DisplayName.Substring(0, 40) + "..." : DisplayName;
            }
        }
        public string FormattedConsumption
        {
            get
            {
                return WaterConsumption.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedLastReading
        {
            get
            {
                return LastReading.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedCurrentReading
        {
            get
            {
                return CurrentReading.ToString(Common.DecimalPoints);
            }
        }
        #endregion
    }
}
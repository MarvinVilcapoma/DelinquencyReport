using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ConsumptionByRangeModel
    {
        #region public properties     
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal? From { get; set; }
        public decimal? To { get; set; }
        public string TaxNumber { get; set; }
        public string Meter { get; set; }
        public string FilterTaxNumber { get; set; }
        public string FilterMeater { get; set; }
        public List<ConsumptionByRangeList> ConsumptionRangeList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string FormattedFromDate { get; set; }
        public string FormattedToDate { get; set; }  
        public string ddlFrom { get; set; }
        public string ConditionTypeID { get; set; }
        public string ddlTo { get; set; }
        #endregion
    }
    public class ConsumptionByRangeList
    {
        #region public properties  
        public Nullable<long> RowNo { get; set; }
        public string Meter { get; set; }
        public string TaxNumber { get; set; }
        public string AccountName { get; set; }
        public decimal LastReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal Consumption { get; set; }
        public string FincaID { get; set; }
        #endregion

        #region custom properties
        public string FormattedMeter
        {
            get
            {
                return Meter == null ? "-" : (Meter.Length > 25 ? Meter.Substring(0, 25) + "..." : Meter);
            }
        }
        public string FormattedAccountName
        {
            get
            {
                return AccountName.Length > 40 ? AccountName.Substring(0, 40) + "..." : AccountName;
            }
        }      
        public string FormattedConsumption
        {
            get
            {
                return Consumption.ToString(Common.DecimalPoints);
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
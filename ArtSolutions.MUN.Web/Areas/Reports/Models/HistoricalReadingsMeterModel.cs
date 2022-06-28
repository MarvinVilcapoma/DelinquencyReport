using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class HistoricalReadingsMeterModel
    {
        #region public properties       
        public string Meter { get; set; }
        public int? AccountId { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<HistoricalReadingsMeterList> HistoricalReadingsMeterList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public HistoricalReadingsMeterModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class HistoricalReadingsMeterList
    {
        #region public properties  
        public string Meter { get; set; }
        public string AccountName { get; set; }
        public decimal LastReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal Consumption { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string ServiceType { get; set; }
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
        public string FormattedMonth
        {
            get
            {
                //return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(this.Month));
                return String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM}", new DateTime(DateTime.Now.Year, this.Month, 1));
            }
        }
        public string FormattedAmount
        {
            get
            {
                return Amount.ToString("C");
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
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class HistoricalPaymentReportModel
    {
        #region public properties  
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<HistoricalPaymentList> HistoricalPaymentList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string FormattedFromDate { get; set; }
        public string FormattedToDate { get; set; }

        #endregion

        #region Constructor
        public HistoricalPaymentReportModel()
        {
            this.HistoricalPaymentList = new List<HistoricalPaymentList>();
        }
        #endregion
    }
    public class HistoricalPaymentList
    {
        #region public properties  
        public string TaxpayerName { get; set; }
        public string TaxpayerID { get; set; }
        public string ServiceCoe { get; set; }
        public string ServiceName { get; set; }
        public string RecieptNumber { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> IVA { get; set; }
        public Nullable<decimal> Fine { get; set; }
        public Nullable<decimal> TotalPayment { get; set; }
        public Nullable<System.DateTime> DateOfPayment { get; set; }
        public string TypeOfPayment { get; set; }
        public string BillerName { get; set; }
        #endregion

        #region custom properties
        #endregion
    }
}
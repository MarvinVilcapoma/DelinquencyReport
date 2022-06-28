using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AmnestyMovementReportModel
    {    
        #region public properties  
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<AmnestyMovementReportList> AmnestyMovementReportList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        public string FormattedFromDate { get; set; }
        public string FormattedToDate { get; set; }

        #endregion

        #region Constructor
        public AmnestyMovementReportModel()
        {
            this.AmnestyMovementReportList = new List<AmnestyMovementReportList>();
        }
        #endregion
    }
    public class AmnestyMovementReportList
    {
        #region public properties  
        public System.DateTime DateOfMovement { get; set; }
        public string TaxpayerName { get; set; }
        public decimal AmnestyAmount { get; set; }
        public decimal TotalPayment { get; set; }
        public string Number { get; set; }
        #endregion

        #region custom properties
        #endregion
    }
}
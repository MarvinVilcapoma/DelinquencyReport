using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ClosingSummaryInvoiceModel
    {
        #region public properties             
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
        public DateTime? StartPeriodDate { get; set; }
        public DateTime? EndPeriodDate { get; set; }
        public decimal? NetClosingFrom { get; set; }
        public decimal? NetClosingTo { get; set; }
        public string[] OtherServiceIDs { get; set; }
        public string FilterOtherServiceID { get; set; }
        public string[] GrantIDs { get; set; }
        public string FilterGrantID { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<GrantModel> GrantList { get; set; }
        public List<ServiceModel> OtherServiceList { get; set; }
        #endregion
    }
}
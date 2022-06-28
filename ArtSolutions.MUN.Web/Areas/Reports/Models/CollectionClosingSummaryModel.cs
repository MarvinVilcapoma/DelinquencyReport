using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionClosingSummaryModel
    {
        #region public properties
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterCashierID { get; set; }
        public decimal NetClosingFrom { get; set; }
        public decimal NetClosingTo { get; set; }
        public IEnumerable<SelectListItem> CashierList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
}
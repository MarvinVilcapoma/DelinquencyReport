using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ClosingSummaryPaymentTypeModel
    {
        #region public properties             
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public List<PaymentModel> PaymentList { get; set; }
        public Int32 TotalClosingRecord { get; set; }
        public DateTime? StartPeriodDate { get; set; }
        public DateTime? EndPeriodDate { get; set; }
        public string[] PaymentTypeIDs { get; set; }
        public string FilterPaymentTypeID { get; set; }
        public decimal? NetClosingFrom { get; set; }
        public decimal? NetClosingTo { get; set; }
        public List<SelectListItemViewModel> PaymentTypeList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
}
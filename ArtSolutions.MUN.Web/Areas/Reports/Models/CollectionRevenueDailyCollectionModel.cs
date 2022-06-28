using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionRevenueDailyCollectionModel
    {
        #region public properties
        public List<RevenueDailyCollectionModel> RevenueDailyCollectionList { get; set; }        
        public Int32 TotalRecord { get; set; }
        public DateTime StartPeriodDate { get; set; }        
        public int[] ServiceIDs { get; set; }
        public int ServiceID { get; set; }
        public string FilterServiceID { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
    public class RevenueDailyCollectionModel
    {
        public string ServiceName { get; set; }
        public Nullable<decimal> ForgiveInterest { get; set; }
        public Nullable<decimal> PaidInterest { get; set; }
        public Nullable<decimal> CurrentPayment { get; set; }
        public Nullable<decimal> PreviousPayment { get; set; }
        public Nullable<decimal> AdvancePayment { get; set; }
        public Nullable<decimal> Total { get; set; }
    }
}
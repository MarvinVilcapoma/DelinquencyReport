using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptOtherConceptModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<SelectListItem> PaymentPlanList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public Guid[] CashierIDs { get; set; }
        public string FilterCashierID { get; set; }
        public Guid CashierID { get; set; }
        public string[] PaymentPlanIDs { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptOtherConceptModelList> OtherConceptReceiptDetailList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptOtherConceptModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptOtherConceptModelList
    {
        #region public properties
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string CollectionConceptName { get; set; }
        public string Number { get; set; }
        public string Discription { get; set; }
        public decimal Amount { get; set; }

        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }     
        public string FormattedAmount
        {
            get
            {
                return Amount.ToString("C");
            }
        }
        #endregion
    }

    public class ReceiptOtherConceptModelDataList
    {
        #region public properties
        public string CollectionConcept { get; set; }
        public List<ReceiptOtherConceptModelList> collectionList { get; set; }
        #endregion
    }
}
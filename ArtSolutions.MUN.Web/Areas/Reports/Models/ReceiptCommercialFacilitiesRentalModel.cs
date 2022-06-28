using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptCommercialFacilitiesRentalModel : DataExportModel
    {
        #region public properties  
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterCashierID { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptCommercialFacilitiesRentalList> ReceiptCommercialFacilitiesRentalList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptCommercialFacilitiesRentalModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptCommercialFacilitiesRentalList
    {
        #region public properties
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Number { get; set; }
        public string AccountDisplayName { get; set; }
        public string RegisterNumber { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal TotalPayment { get; set; }
        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("d");
            }
        }
        public string FormattedTotalPayment
        {
            get
            {
                return this.TotalPayment > 0 ? this.TotalPayment.ToString("C") : "";
            }
        }
        #endregion
    }
}
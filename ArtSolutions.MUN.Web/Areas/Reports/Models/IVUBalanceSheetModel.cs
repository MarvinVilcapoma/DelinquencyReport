using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUBalanceSheetModel : DataExportModel
    {
        #region public properties
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<IVUBalanceSheetList> IVUBalanceSheetList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public IVUBalanceSheetModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class IVUBalanceSheetList
    {
        #region public properties                   
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalTaxableSales { get; set; }
        #endregion

        #region custom properties
        public string FormattedPeriod
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.StartPeriodID) + "-" + this.FiscalYearID;
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.ToString("C");
            }
        }
        public string FormattedCharges
        {
            get
            {
                return Charges.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.ToString("C");
            }
        }
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        public string FormattedTotalTaxableSales
        {
            get
            {
                return TotalTaxableSales.ToString("C");
            }
        }
        #endregion
    }
}
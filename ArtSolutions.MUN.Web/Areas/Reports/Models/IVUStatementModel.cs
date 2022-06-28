using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUStatementModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public Nullable<decimal> BalanceFrom { get; set; }
        public Nullable<decimal> BalanceTo { get; set; }
        public List<IVUStatementList> IVUStatementList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }
        public string StatementType { get; set; }
        public DateTime FutureDate { get; set; }
        #endregion

        #region Constructor
        public IVUStatementModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class IVUStatementList
    {
        #region public properties
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal IVUTaxBalance { get; set; }
        public decimal Payments { get; set; }
        public Nullable<decimal> TotalBalance { get; set; }
        #endregion

        #region custom properties
        public string FormattedIVUPeriod
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.StartPeriodID) + "-" + this.FiscalYearID;
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
        public string FormattedIVUTaxBalance
        {
            get
            {
                return IVUTaxBalance.ToString("C");
            }
        }
        public string FormattedPayments
        {
            get
            {
                return Payments.ToString("C");
            }
        }
        public string FormattedTotalBalance
        {
            get
            {
                return TotalBalance.Value.ToString("C") ?? 0.ToString("C");
            }
        }
        public string FormattedCertificateTotalBalance
        {
            get
            {
                return (TotalBalance.Value + Payments).ToString("C") ?? 0.ToString("C");
            }
        }
        #endregion
    }
}
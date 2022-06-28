using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountStatementModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public int? AccountPropertyID { get; set; }
        public int? Year { get; set; }
        public int? Period { get; set; }
        public DateTime? Date { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
        public List<AccountStatementList> AccountStatementList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountServiceCollectionDetailIDs { get; set; }
        public string AccountPaymentPlanNames { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal DebitBalance { get; set; }

        #endregion

        #region Constructor
        public AccountStatementModel()
        {
            this.AccountList = new List<AccountSearchModel>();
            this.PropertyList = new List<SelectListItem>();
        }
        #endregion
    }

    public class AccountStatementList
    {
        #region public properties
        public int ServiceID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int AccountServiceCollectionDetailID { get; set; }
        public string ConsecutiveNumber { get; set; }
        public string MeterNumber { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public string RightNumber { get; set; }
        public decimal Principal { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Discount { get; set; }
        public decimal PendingAmount { get; set; }
        public Nullable<bool> HasActivePaymentPlan { get; set; }
        public int AccountID { get; set; }
        public string PeriodName { get; set; }
        public Nullable<int> IsMonthly { get; set; }
        public Nullable<decimal> PreviousMeasure { get; set; }
        public Nullable<decimal> ActualMeasure { get; set; }
        public Nullable<decimal> WaterConsumption { get; set; }
        public string AccountPaymentPlanNames { get; set; }
        #endregion

        #region custom properties       
        public string FormattedPeriod
        {
            get
            {
                return this.StartDate.ToString("d") + " - " + this.DueDate.ToString("d");
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
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
        public string FormattedDiscount
        {
            get
            {
                return Discount.ToString("C");
            }
        }
        public string FormattedPendingAmount
        {
            get
            {
                return PendingAmount.ToString("C");
            }
        }
        public string FormattedHasActivePaymentPlan
        {
            get
            {
                return HasActivePaymentPlan == null ? "-" : Global.Yes;
            }
        }
        public string FormattedPreviousMeasure
        {
            get
            {
                return PreviousMeasure == null ? "-" : PreviousMeasure.Value.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedActualMeasure
        {
            get
            {
                return ActualMeasure == null ? "-" : ActualMeasure.Value.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedWaterConsumption
        {
            get
            {
                return WaterConsumption == null ? "-" : WaterConsumption.Value.ToString(Common.DecimalPoints);
            }
        }
        #endregion
    }
}
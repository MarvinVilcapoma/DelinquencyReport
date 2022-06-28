using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountExclusionFormModel : DataExportModel
    {
        #region public properties     
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string Observations { get; set; }
        public List<PropertyTaxList> PropertyTaxList { get; set; }
        public List<WaterMeasureList> WaterMeasureList { get; set; }
        public List<OtherServicesList> OtherServicesList { get; set; }
        public List<HistoricalList> HistoricalList { get; set; }
        public AccountModel AccountModel { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }     
        #endregion

        #region Constructor
        public AccountExclusionFormModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class PropertyTaxList
    {
        #region public properties
        public int Year { get; set; }
        public string RightNumber { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal Principal { get; set; }
        public string Service { get; set; }
        public decimal Penalties { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmountByService { get; set; }
        #endregion

        #region custom properties 
        public string FormattedTotalValue
        {
            get
            {
                return TotalValue == null ? "-" : TotalValue.Value.ToString("C");
            }
        }
        public string FormattedService
        {
            get
            {
                return Service.Length > 40 ? Service.Substring(0, 40) + "..." : Service;
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.ToString("C");
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedPaidAmount
        {
            get
            {
                return PaidAmount.ToString("C");
            }
        }
        public string FormattedBalanceAmountByService
        {
            get
            {
                return BalanceAmountByService.ToString("C");
            }
        }
        #endregion
    }
    public class WaterMeasureList
    {
        #region public properties        
        public string Service { get; set; }
        public string CustomField1 { get; set; }
        public decimal PreviousMeasure { get; set; }
        public decimal ActualMeasure { get; set; }
        public string CustomField2 { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Balance { get; set; }
        #endregion

        #region custom properties         
        public string FormattedService
        {
            get
            {
                return Service.Length > 40 ? Service.Substring(0, 40) + "..." : Service;
            }
        }
        public string FormattedMonth
        {
            get
            {
                return String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM}", new DateTime(DateTime.Now.Year, this.Month, 1));
            }
        }
        public string FormattedPreviousMeasure
        {
            get
            {
                return PreviousMeasure.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedActualMeasure
        {
            get
            {
                return ActualMeasure.ToString(Common.DecimalPoints);
            }
        }
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        #endregion
    }
    public class OtherServicesList
    {
        #region public properties        
        public string Service { get; set; }
        public string RightNumber { get; set; }
        public decimal Principal { get; set; }
        public int PendingPeriod { get; set; }
        public string Segrega { get; set; }
        public int FiscalYearID { get; set; }
        public decimal Balance { get; set; }
        #endregion

        #region custom properties 
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedService
        {
            get
            {
                return Service.Length > 100 ? Service.Substring(0, 100) + "..." : Service;
            }
        }
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        #endregion
    }
    public class HistoricalList
    {
        public int FiscalYearID { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public string FormattedAmountSubjectToTax
        {
            get
            {
                return AmountSubjectToTax.ToString("C");
            }
        }
        public string FormattedPrincipal
        {
            get
            {
                return Principal.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.ToString("C");
            }
        }
        public string FormattedPaidAmount
        {
            get
            {
                return PaidAmount.ToString("C");
            }
        }
        public string FormattedRemainingAmount
        {
            get
            {
                return RemainingAmount.ToString("C");
            }
        }
    }
}
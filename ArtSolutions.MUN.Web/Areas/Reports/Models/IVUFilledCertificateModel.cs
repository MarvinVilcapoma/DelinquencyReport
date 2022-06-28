using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUFilledCertificateModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<IVUFilledCertificateList> IVUFilledCertificateList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public DateTime FutureDate {get;set;}
        #endregion

        #region Constructor
        public IVUFilledCertificateModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }

    public class IVUFilledCertificateList
    {
        #region public properties
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }
        public bool IsFilled { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
        public decimal PaidAmount { get; set; }
        #endregion

        #region custom properties
        public string FormattedIVUPeriod
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
                return this.Principal.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return this.Penalties.ToString("C");
            }
        }
        public string FormattedCharges
        {
            get
            {
                return this.Charges.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return this.Interest.ToString("C");
            }
        }
        public string FormattedBalance
        {
            get
            {
                return this.Balance.ToString("C");
            }
        }
        public string FormattedDescription
        {
            get
            {
                return this.IsFilled == true ? Resources.Report.NoDebt : Resources.Report.HasDebt;
            }
        }
        public string FormattedPaidAmount
        {
            get
            {
                return this.PaidAmount.ToString("C");
            }
        }
        
        #endregion
    }
}
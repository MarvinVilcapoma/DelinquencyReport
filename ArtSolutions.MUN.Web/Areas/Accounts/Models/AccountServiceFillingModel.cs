using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceFillingModel
    {
        #region public properties       
        public int AccountServiceCollectionDetailId { get; set; }
        public int AccountServiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal SalesVolume { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Discount { get; set; }
        public decimal Debt { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Rate { get; set; }
        public int CollectionRuleID { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsPayed { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string ServiceNameWithYear { get; set; }
        public int FiscalYearID { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }
        public string ServicePeriod { get; set; }
        public System.DateTime FillingDueDate { get; set; }
        public System.DateTime PaymentDueDate { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public string Frequency { get; set; }
        public Nullable<System.DateTime> DiscountDate { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public int ServiceID { get; set; }
        public string PopupServiceTitle
        {
            get
            {
                if (this.StartPeriodID > 0 && this.EndPeriodID > 0)
                    return ServiceName + " - " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.StartPeriodID) + " " + this.FiscalYearID + " - " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.EndPeriodID) + " " + this.FiscalYearID;
                else
                    return "";
            }
        }
        public bool? IsLock { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<decimal> ExceptionValue { get; set; }
        public decimal? PercentageValue { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public int FilingTypeID { get; set; }
        public Nullable<int> FilingFormID { get; set; }
        public int SequenceID { get; set; }
        public Nullable<int> ServiceStartPeriod { get; set; }

        public decimal AdjustmentAmount { get; set; }
        public int Period { get; set; }
        public Nullable<int> PaymentPaidPeriod { get; set; }
        public int IsEditPermission { get; set; }
        public bool? IsInJudicialCollection { get; set; }

        #region custom properties
        public string RowVersion64
        {
            get
            {
                if (this.RowVersion != null)
                    return Convert.ToBase64String(this.RowVersion);

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.RowVersion = null;
                else
                    this.RowVersion = Convert.FromBase64String(value);
            }
        }
        public string GetFormattedCollectionRate
        {
            get
            {
                if (this.CollectionRuleID == (int)EnCollectionRule.Fee || this.CollectionRuleID == (int)EnCollectionRule.RangeFee) // Flat Fee OR Range Flat Fee
                {
                    return this.Rate.ToString("C");
                }
                else // % OR Range% OR % Of %
                {
                    return this.Rate.ToString("0.#######") + "%";
                }
            }
        }
        public string GetRateFormat
        {
            get
            {
                if (this.CollectionRuleID == (int)EnCollectionRule.Fee || this.CollectionRuleID == (int)EnCollectionRule.RangeFee) // Flat Fee OR Range Flat Fee
                {
                    return "";
                }
                else // % OR Range% OR % Of %
                {
                    return "%";
                }
            }
        }
        #endregion
        #region reference properties
        public AccountServiceCollectionFillingLicenseModel CollectionFillingModel { get; set; }
        public AccountServiceCollectionFillingTaxModel CollectionFillingTaxModel { get; set; }
        public AccountServiceCollectionFillingUnitModel CollectionFillingUnitModel { get; set; }
        public AccountServiceCollectionFillingMeasuredWaterModel CollectionFillingMeasuredWaterModel { get; set; }
        public AccountServiceCollectionFillingPropertyTaxModel CollectionFillingPropertyTaxModel { get; set; }
        #endregion
        #endregion

        #region Constructor       
        public AccountServiceFillingModel()
        {
            CollectionFillingModel = new AccountServiceCollectionFillingLicenseModel();
            CollectionFillingTaxModel = new AccountServiceCollectionFillingTaxModel();
            CollectionFillingUnitModel = new AccountServiceCollectionFillingUnitModel();
            CollectionFillingMeasuredWaterModel = new AccountServiceCollectionFillingMeasuredWaterModel();
            CollectionFillingPropertyTaxModel = new AccountServiceCollectionFillingPropertyTaxModel();
        }
        #endregion

        #region public methods  
        public List<AccountServiceFillingModel> Get(int accountServiceId, int? accountServiceCollectionDetailId, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailID", Value = accountServiceCollectionDetailId });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<AccountServiceFillingModel>>(APISelector.Municipality, true, "api/AccountService/FillingGet", "GET", lstParameter);
        }

        public AccountServiceFillingModel GetAutoFiling(int accountServiceCollectionID)
        {
            AccountServiceFillingModel model = new AccountServiceFillingModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            return new RestSharpHandler().RestRequest<List<AccountServiceFillingModel>>(APISelector.Municipality, true, "api/AccountService/CollectionAutoFillingGet", "GET", lstParameter).FirstOrDefault();
        }

        public string ConfigureDiscountInfoMessage(List<AccountServiceFillingModel> model)
        {
            AccountServiceFillingModel filingModel = model.Where(x => x.FillingDate.HasValue && x.DiscountDate.HasValue && x.DiscountDate.Value >= DateTime.Today && x.Total > 0).FirstOrDefault();
            if (filingModel != null && filingModel.DiscountPercentage.HasValue && filingModel.DiscountPercentage.Value > 0)
            {
                return "<small>"
                    + Resources.AccountService.DiscountMessageStart
                    + " <span class='text-success font-bold'>" + (filingModel.DiscountPercentage.Value / 100).ToString("P") + "</span> "
                    + Resources.AccountService.DiscountMessageEnd
                    + " <span class='text-success font-bold'>" + filingModel.DiscountDate.Value.ToString("MMMM d, yyyy") + " </span></small>";
            }
            return string.Empty;
        }

        public bool IsValidForExtension(AccountServiceModel model)
        {
            if (model != null)
            {
                AccountServiceFillingModel filingModel = model.Filling.FirstOrDefault() ?? new AccountServiceFillingModel();
                if ((model.ExtensionID == 0 || model.ExtensionID == null) && filingModel.SalesVolume == 0
                    && filingModel.FillingDate == null  // Yet Not Filed
                    && filingModel.FillingDueDate.Date >= Common.CurrentDateTime.Date  // should apply to future filing
                    && model.ServiceTypeID == 1) // Allowed only for business license
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public void DeleteFillingAutoFiling(int ID, string Notes)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = ID });
            lstParameter.Add(new NameValuePair { Name = "notes", Value = Notes });
            lstParameter.Add(new NameValuePair { Name = "createdUserID", Value = UserSession.Current.Id });
            lstParameter.Add(new NameValuePair { Name = "createdDate", Value = Common.CurrentDateTime.ToString(CultureInfo.InvariantCulture) });
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteFillingAutoFiling", "GET", lstParameter);
        }
        #endregion
    }

    #region Enum    
    public enum EnCollectionRule
    {
        Fee = 1,
        Percentage = 2,
        PercentegeofPercentage = 3,
        RangeFee = 4,
        RangePercentage = 5,
        RangeFeeandPercentage = 6
    }
    #endregion
}
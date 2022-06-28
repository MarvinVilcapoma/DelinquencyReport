using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionDetailModel
    {
        #region properties
        public Nullable<int> ID { get; set; }
        public string ServiceFrequencyName { get; set; }
        public Nullable<int> AccountServiceID { get; set; }
        public string AccountServiceName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameWithYear { get; set; }
        public decimal AmountSubjectToTax { get; set; }
        public decimal SalesVolume { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal AmnestyAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal Debt { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Total { get; set; }
        public bool IsPayed { get; set; }
        public Nullable<System.DateTime> DiscountDate { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> CalculatedDiscount { get; set; }
        public decimal Payment { get; set; }
        public Nullable<int> FiscalYearID { get; set; }
        public int PreviousFiscalYearID
        {
            get
            {
                return FiscalYearID??0 - 1;
            }
        }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
        public decimal Principal1 { get; set; }
        public decimal Principal2 { get; set; }
        public string FiscalYearWithPeriod { get; set; }
        public string ItemName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string FormattedDueDate
        {
            get
            {
                return String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM, yyyy}", DueDate);
            }
        }
        public string FrequnecyMonthly
        {
            get
            {
                return String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", DueDate);
            }
        }
        public bool SelectedItem { get; set; }
        public Nullable<decimal> TotalPaidAmountSummary { get; set; }
        public string AccountServiceCollectionDetailIDs { get; set; }
        public int CollectionFillingPropertyTaxID { get; set; }
        public string ServiceType { get; set; }
        public int ServiceTypeID { get; set; }
        public int ServiceID { get; set; }
        public string RightNumber { get; set; }
        public int FrequencyID { get; set; }
        public string ServiceNameWithSegrega { get; set; }

        public decimal? PreviousMeasure { get; set; }
        public decimal? ActualMeasure { get; set; }
        public decimal? WaterConsumption { get; set; }
        public string ServiceCode { get; set; }
        public string MeterNumber { get; set; }
        public int SequenceID { get; set; }
        public decimal PrincipalMinusDiscount { get; set; }
        public int IsEditPermission { get; set; }
        public DateTime? FillingDate { get; set; }

        #endregion

        #region public methods
        public List<AccountServiceCollectionDetailModel> Get(int accountServiceId, int? accountServiceCollectionDetailId, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailID", Value = accountServiceCollectionDetailId });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDetailModel>>(APISelector.Municipality, true, "api/AccountService/CollectionDetailGet", "GET", lstParameter);
        }

        public List<AccountServiceCollectionDetailModel> GetNotPaid(int accountId, DateTime? paymentDate, bool IsIvaApply = false,bool applybyAmnesty=false)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "PaymentDate", Value = paymentDate.HasValue ? paymentDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            lstParameter.Add(new NameValuePair { Name = "IsIvaApply", Value = IsIvaApply });
            lstParameter.Add(new NameValuePair { Name = "applybyAmnesty", Value = applybyAmnesty });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDetailModel>>(APISelector.Municipality, true, "api/AccountService/CollectionDetailGetNotPaid", "GET", lstParameter);
        }

        public List<AccountServiceCollectionDetailModel> GetPropertyTaxNotPaid(int accountPropertyID, int accountPropertyRightID, DateTime? paymentDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountPropertyID", Value = accountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "AccountPropertyRightID", Value = accountPropertyRightID });
            lstParameter.Add(new NameValuePair { Name = "PaymentDate", Value = paymentDate.HasValue ? paymentDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDetailModel>>(APISelector.Municipality, true, "api/AccountService/CollectionPropertyTaxDetailGetNotPaid", "GET", lstParameter);
        }

        public List<AccountServiceCollectionDetailModel> GetNotPaidSummary(int accountID, string collectiondetailIDs,bool forEdit, bool applybyAmnesty)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "collectiondetailIDs", Value = collectiondetailIDs });
            lstParameter.Add(new NameValuePair { Name = "forEdit", Value = forEdit });
            lstParameter.Add(new NameValuePair { Name = "applybyAmnesty", Value = applybyAmnesty });
            //lstParameter.Add(new NameValuePair { Name = "serviceTypeID", Value = serviceTypeID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDetailModel>>(APISelector.Municipality, true, "api/AccountService/CollectionDetailSummaryGetNotPaid", "GET", lstParameter);
        }
        #endregion
    }
    public class AccountServiceCollectionPrintModel
    {
        #region public properties            
        public string ServiceName { get; set; }
        public decimal TotalPrincipal { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal Total { get; set; }
        #endregion
    }
}
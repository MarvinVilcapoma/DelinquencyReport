using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUCollectionDetailModel
    {
        #region public properties
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public List<IVUCollectionDetailList> IVUCollectionDetailList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }

    public class IVUCollectionDetailList
    {
        #region public properties
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }
        public decimal TotalUse { get; set; }
        public decimal TotalTaxable { get; set; }
        public decimal TotalSubjectToIVU { get; set; }
        public decimal MunicipalTax { get; set; }
        public decimal PayableTax { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalPaid { get; set; }
        public Guid FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public string User { get; set; }
        public Nullable<DateTime> FillingDate { get; set; }
        public string PaymentDate { get; set; }
        public string ReceiptNumber { get; set; }
        #endregion

        #region custom properties
        public string FormattedPeriod
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.StartPeriodID) + "-" + this.FiscalYearID;
            }
        }
        public string FormattedFillingDate
        {
            get
            {
                return FillingDate.HasValue ? FillingDate.Value.ToString("d") : "";
            }
        }
        public string FormattedTotalUse
        {
            get
            {
                return TotalUse.ToString("C");
            }
        }
        public string FormattedTotalTaxable
        {
            get
            {
                return TotalTaxable.ToString("C");
            }
        }
        public string FormattedTotalSubjectToIVU
        {
            get
            {
                return TotalSubjectToIVU.ToString("C");
            }
        }
        public string FormattedMunicipalTax
        {
            get
            {
                return MunicipalTax.ToString("C");
            }
        }
        public string FormattedPayableTax
        {
            get
            {
                return PayableTax.ToString("C");
            }
        }
        public string FormattedTotalPaid
        {
            get
            {
                return TotalPaid.ToString("C");
            }
        }
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        public string FormattedUser
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.FillingBy.ToString()).FirstOrDefault();
                this.User = string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                return this.IsFromPortal == false ? this.User : (this.User + " ( " + Report.MerchantsPortal + " ) ");
            }
        }
        public string FormattedPaymentDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.PaymentDate))
                    return "-";
                string str = null;
                foreach (string item in this.PaymentDate.Split(','))
                {
                    str += Convert.ToDateTime(item).ToString("d") + " <br /> ";
                }
                return str.Remove(str.Length - 1);
            }
        }
        public string FormattedReceiptNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ReceiptNumber))
                    return "-";
                string str = null;
                foreach (string item in this.ReceiptNumber.Split(','))
                {
                    str += item + " <br /> ";
                }
                return str.Remove(str.Length - 1);
            }
        }
        #endregion
    }
}
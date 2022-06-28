using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CashReceiptControlModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalRecord { get; set; }
        public List<CashReceiptControlList> CashReceiptControlList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
    public class CashReceiptControlList
    {
        #region public properties
        public System.DateTime Date { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalReceipt { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Penalties { get; set; }
        public decimal? Interest { get; set; }
        public decimal? IVA { get; set; }
        public System.Guid ReceiptCreator { get; set; }
        public Nullable<System.DateTime> CashRegisterDate { get; set; }
        public Nullable<System.Guid> CashRegisterCreator { get; set; }
        public Nullable<System.DateTime> DepositEntryDate { get; set; }
        public string Bank { get; set; }
        public Nullable<System.Guid> DepositCreator { get; set; }
        public bool IsVoid { get; set; }
        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("d");
            }
        }
        public string VoidedReceipts
        {
            get
            {
                return this.IsVoid == true ? Resources.Global.Voided : "";
            }
        }
        public string FormattedTotalReceipt
        {
            get
            {
                return this.TotalReceipt.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return this.Penalties.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return this.Interest == null ? "-" : this.Interest.Value.ToString("C");
            }
        }
        public string FormattedIVA
        {
            get
            {
                return this.IVA == null ? "-" : this.IVA.Value.ToString("C");
            }
        }
        public string FormattedReceiptCreator
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.ReceiptCreator.ToString()).FirstOrDefault();
                return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
            }
        }
        public string FormattedCashRegisterDate
        {
            get
            {
                return this.CashRegisterDate == null ? "" : this.CashRegisterDate.Value.ToString("d");
            }
        }
        public string FormattedCashRegisterCreator
        {
            get
            {
                if (this.CashRegisterCreator == null)
                    return null;
                else
                {
                    UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.CashRegisterCreator.ToString()).FirstOrDefault();
                    return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                }
            }
        }
        public string FormattedDepositEntryDate
        {
            get
            {
                return this.DepositEntryDate == null ? "" : this.DepositEntryDate.Value.ToString("d");
            }
        }
        public string FormattedDepositCreator
        {
            get
            {
                if (this.DepositCreator == null)
                    return null;
                else
                {
                    UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.DepositCreator.ToString()).FirstOrDefault();
                    return string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                }
            }
        }
        #endregion
    }
}
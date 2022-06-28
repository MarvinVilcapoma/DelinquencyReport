using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptRegisterModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PaymentReceiptTypeID { get; set; }
        public int? PrintTemplateID { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterCashierID { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptRegisterList> ReceiptRegisterList { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptRegisterModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
            this.TypeList = new List<SelectListItem>();
        }
        #endregion
    }
    public class ReceiptRegisterList
    {
        #region public properties
        public int ID { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public string TaxNumber { get; set; }
        public string AccountDisplayName { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public string Cashier { get; set; }
        public bool IsOfficialReceipt { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public Nullable<decimal> Charges { get; set; }
        public decimal TotalAmountAuxiliaryReceipt { get; set; }
        public decimal TotalAmountOfficialReceipt { get; set; }
        public string PaymentType { get; set; }
        public string StatusWithBatchNumber { get; set; }
        public string Bank { get; set; }
        public string PaymentPeriod { get; set; }
        public string CustomField { get; set; }
        public Nullable<int> PaymentReceiptType { get; set; }
        public Nullable<int> PrintTemplateID { get; set; }
        #endregion

        #region custom properties  
        public string FormattedPaymentType
        {
            get
            {
                //return this.PrintTemplateID == 10 ? COLPayment.ByOtherService : this.PaymentType;
                return this.PaymentType;
            }
        }
        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("d");
            }
        }
        public string FormattedTotalAmountAuxiliaryReceipt
        {
            get
            {

                return this.TotalAmountAuxiliaryReceipt > 0 ? this.TotalAmountAuxiliaryReceipt.ToString("C") : "";
            }
        }
        public string FormattedTotalAmountOfficialReceipt
        {
            get
            {
                return this.TotalAmountOfficialReceipt > 0 ? this.TotalAmountOfficialReceipt.ToString("C") : "";
            }
        }
        public string FormattedInterest
        {
            get
            {
                return this.Interest > 0 ? this.Interest.Value.ToString("C") : "";
            }
        }
        public string FormattedDiscount
        {
            get
            {
                return this.Discount > 0 ? this.Discount.Value.ToString("C") : "";
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return this.Penalties > 0 ? this.Penalties.Value.ToString("C") : "";
            }
        }
        public string FormattedCharges
        {
            get
            {
                return this.Charges > 0 ? this.Charges.Value.ToString("C") : "";
            }
        }
        public string FormattedCashier
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.CreatedUserID.ToString()).FirstOrDefault();
                this.Cashier = string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName;
                return this.Cashier;
            }
        }
        public string FormattedStatusBatchNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(this.StatusWithBatchNumber))
                {
                    string[] result = this.StatusWithBatchNumber.Split(',');
                    return (result[0] == "D" ? Global.IPost : Global.Issued) + "<br/>" + result[1];
                }
                else
                    return null;
            }
        }
        #endregion
    }
}
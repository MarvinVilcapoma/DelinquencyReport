using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptPaymentPlanModel : DataExportModel
    {
        #region public properties     
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BalanceFrom { get; set; }
        public decimal? BalanceTo { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<SelectListItem> PaymentPlanList { get; set; }
        public string[] AccountIDs { get; set; }
        public Guid[] CashierIDs { get; set; }
        public Guid CashierID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterCashierID { get; set; }
        public string FilterPaymentPlanID { get; set; }
        public string[] PaymentPlanIDs { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public List<ReceiptPaymentPlanModelList> PaymentPlanReceiptDetailList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public ReceiptPaymentPlanModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptPaymentPlanModelList
    {
        #region public properties
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public string Number { get; set; }
        public string AccountPaymentPlanNumber { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Payment { get; set; }

        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
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
        public string FormattedPaymentamount
        {
            get
            {
                return Payment.ToString("C");
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class PaymentsMadeByTheTaxpayerModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<PaymentsMadeByTheTaxpayerList> PaymentsMadeByTheTaxpayerList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public PaymentsMadeByTheTaxpayerModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class PaymentsMadeByTheTaxpayerList
    {
        #region public properties
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Number { get; set; }
        public string DisplayName { get; set; }
        public string PaymentReceiptType { get; set; }
        public decimal? RegularPayment { get; set; }
        public int AdvancedPayment { get; set; }
        public decimal? Penalties { get; set; }
        public decimal? Interest { get; set; }
        public decimal? Others { get; set; }
        public decimal Total { get; set; }
        public int IsVoid { get; set; }
        #endregion

        #region custom properties        
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class PaymentPlanByTaxpayerModel
    {
        #region public properties    
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public int? Year { get; set; }
        public DateTime? Date { get; set; }
        public int? QuotasToCalculate { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SelectListItem> AccountPaymentPlanList { get; set; }
        public string AccountPaymentPlanIDs { get; set; }
        public AccountModel AccountModel { get; set; }
        public List<AccountPaymentPlanServiceDetailList> AccountPaymentPlanServiceDetailList { get; set; }
        public List<PaymentPlanByTaxpayerList> PaymentPlanByTaxpayerList { get; set; }
        public int MinMOROSAS { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public string RowNos { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public int MinAPager { get; set; }
        #endregion

        #region Constructor
        public PaymentPlanByTaxpayerModel()
        {
            this.AccountList = new List<AccountSearchModel>();
            this.AccountPaymentPlanList = new List<SelectListItem>();
        }
        #endregion
    }
    public class AccountPaymentPlanServiceDetailList
    {
        public int RowNo { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string Periods { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public bool IsMigration { get; set; }
        public bool SelectedItem { get; set; }
        public string PaymentPlanNameWithNumber { get; set; }
    }
    public class PaymentPlanByTaxpayerList
    {
        public int ID { get; set; }
        public string PaymentPlanNameWithNumber { get; set; }
        public decimal MONTOTOTAL { get; set; }
        public decimal? CUOTAMENSUAL { get; set; }
        public int APagar { get; set; }
        public int CANCELADAS { get; set; }
        public int QuotasToCalculate { get; set; }
        public int MOROSAS { get; set; }
        public DateTime? FECHAINICIAL { get; set; }
        public DateTime? FECHAFINAL { get; set; }
        public DateTime? ULTIMOPAGO { get; set; }
        public decimal SubTotal { get; set; }
        public decimal InterestTotal { get; set; }
        public decimal Total { get; set; }
    }
}
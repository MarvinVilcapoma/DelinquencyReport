using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptCollectedByBankModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public List<SelectListItem> ContractList { get; set; }
        public string[] AccountIDs { get; set; }
        public int[] BankIDs { get; set; }
        public int BankID { get; set; }
        public int[] ContractIDs { get; set; }
        public int ContractID { get; set; }
        public string FilterAccountID { get; set; }
        public string FilterBankID { get; set; }
        public string FilterContractID { get; set; }
        public List<ReceiptCollectedByBankList> ReceiptCollectedByBankList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        public string BankeNames { get; set; }
        public string ContractNames { get; set; }
        #endregion

        #region Constructor
        public ReceiptCollectedByBankModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class ReceiptCollectedByBankList
    {
        #region public properties
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Number { get; set; }
        public string DisplayName { get; set; }
        public string PaymentReceiptType { get; set; }
        public decimal? RegularPayment { get; set; }
        public int AdvancedPayment { get; set; }
        public decimal? IVA { get; set; }
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
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ControlOfPaymentsMadeInTheBankModel
    {
        #region public properties
        public List<ControlOfPaymentsMadeInTheBankList> ControlOfPaymentsMadeInTheBankList { get; set; }
        public Int32 TotalRecord { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public int[] ServiceIDs { get; set; }
        public int ServiceID { get; set; }
        public int[] BankIDs { get; set; }
        public int BankID { get; set; }
        public int[] ContractIDs { get; set; }
        public int ContractID { get; set; }
        public string FilterServiceID { get; set; }
        public string FilterBankID { get; set; }
        public string FilterContractID { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public List<SelectListItem> ContractList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public string BankeNames { get; set; }
        public string ContractNames { get; set; }
        public string ServicesNames { get; set; }
        #endregion
    }
    public class ControlOfPaymentsMadeInTheBankList
    {
        public string RecieptNumber { get; set; }
        public string TaxNumber { get; set; }
        public string AccountName { get; set; }
        public string ServiceName { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> DelayOnTaxInterest { get; set; }
        public Nullable<decimal> DelayInServiceInterest { get; set; }
    }
}
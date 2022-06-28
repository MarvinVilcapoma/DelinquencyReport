using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionDailyIncomeProduceBankModel
    {
        #region public properties
        public List<DailyIncomeProduceBankModel> DailyIncomeProduceBankList { get; set; }        
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
    public class DailyIncomeProduceBankModel
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Charges { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Payment { get; set; }
    }
}
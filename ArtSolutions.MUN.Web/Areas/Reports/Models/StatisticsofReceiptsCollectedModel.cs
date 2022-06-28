using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class StatisticsofReceiptsCollectedModel
    {
        #region public properties  
        public DateTime? Date { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public int? BankID { get; set; }
        public List<SelectListItem> ContractList { get; set; }
        public string Contract { get; set; }
        public List<StatisticsofReceiptsCollectedList> StatisticsofReceiptsCollectedList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public StatisticsofReceiptsCollectedModel()
        {
            BankList = new List<SelectListItem>();
            ContractList = new List<SelectListItem>();
            StatisticsofReceiptsCollectedList = new List<StatisticsofReceiptsCollectedList>();
        }
        #endregion
    }
    public class StatisticsofReceiptsCollectedList
    {
        #region public properties
        public DateTime Date { get; set; }
        public int PaymentOptionID { get; set; }
        public string BankName { get; set; }
        public string Contract { get; set; }
        public string ContractName { get; set; }
        public int TotalLinesInFileReceived { get; set; }
        public int TotalLinesWithPayments { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalAmount { get; set; }

        #endregion

        #region custom properties 
        #endregion
    }
}
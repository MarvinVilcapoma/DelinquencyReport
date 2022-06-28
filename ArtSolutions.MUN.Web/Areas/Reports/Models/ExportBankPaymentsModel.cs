using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ExportBankPaymentsModel
    {
        #region public properties       
        public DateTime DueDate { get; set; }
        public List<SelectListItem> BanacioList { get; set; }
        public string CommaSeperatedBanacioIDs { get; set; }
        public DataTable ExportBankPaymentsList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        public Dictionary<string, int> ColumnSchema { get; set; }
        #endregion

        #region Constructor
        public ExportBankPaymentsModel()
        {
            BanacioList = new List<SelectListItem>();
            ExportBankPaymentsList = new DataTable();
        }
        #endregion
    }
}
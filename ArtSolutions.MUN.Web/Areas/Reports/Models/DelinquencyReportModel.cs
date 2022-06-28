using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DelinquencyReportModel
    {
        #region public properties         
        public string ServiceCode { get; set; }
        public List<SelectListItem> listServiceType { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public string ServiceName { get; set; }
        public DateTime PreviousYear { get; set; }
        public DateTime CurrentYear { get; set; }
        public double GeneralAmount { get; set; }
        public double PercentagePreviousYear { get; set; }
        public double PercentageCurrentYear { get; set; }
        public double PercentageGeneral { get; set; }
        public double TotalAmount { get; set; }

        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MonthlyIncomeStatementByServicesModel
    {
        #region public properties
        public List<MonthlyIncomeStatementByServicesList> MonthlyIncomeStatementByServicesList { get; set; }
        public Int32 TotalRecord { get; set; }
        public DateTime ClosingDate { get; set; }        
        public int[] ServiceIDs { get; set; }
        public int ServiceID { get; set; }
        public string FilterServiceID { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
    public class MonthlyIncomeStatementByServicesList
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string ServiceName { get; set; }
        public decimal CurrentCharges { get; set; }
        public decimal CurrentInterest { get; set; }
        public decimal CurrentPayment { get; set; }
        public decimal PreviousCharges { get; set; }
        public decimal PreviousInterest { get; set; }
        public decimal PreviousPayment { get; set; }
    }

    public class QuarterlyIncomeStatementByServicesModel : MonthlyIncomeStatementByServicesModel
    {
        #region public properties
        public List<QuarterlyIncomeStatementByServicesList> QuarterlyIncomeStatementByServicesList { get; set; }        
        public int Quarter { get; set; }
        public int Year { get; set; }
        public List<SelectListItem> YearList { get; set; }
        #endregion
    }
    public class YearlyIncomeStatementByServicesModel : QuarterlyIncomeStatementByServicesModel
    {
        #region public properties
        public List<YearlyIncomeStatementByServicesList> YearlyIncomeStatementByServicesList { get; set; }
        #endregion
    }
    public class QuarterlyIncomeStatementByServicesList : MonthlyIncomeStatementByServicesList
    {
    }
    public class YearlyIncomeStatementByServicesList : MonthlyIncomeStatementByServicesList
    {
    }
}
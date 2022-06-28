using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUFormsNotFiledModel
    {
        #region public properties                
        public DateTime? Since { get; set; }
        public DateTime? Till { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public List<IVUFormsNotFiledList> IVUFormsNotFiledList { get; set; }
        public int TotalRecord { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public IVUFormsNotFiledModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class IVUFormsNotFiledList
    {
        #region public properties   
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string RegisterNumber { get; set; }
        public int StartPeriodID { get; set; }
        public int FiscalYearID { get; set; }      
        public string MonthsWithoutFiling { get; set; }
        #endregion        

        #region custom properties
        public string FormattedStartPeriod
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.StartPeriodID) + "-" + this.FiscalYearID;
            }
        }

        public string FormattedMonthsWithoutFiling
        {
            get
            {
                string str = null;
                foreach (string item in this.MonthsWithoutFiling.Split(','))
                {
                    string[] resultStr = item.Split('-');
                    str += System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(resultStr[1])) + "" + resultStr[0] + " <br /> ";
                }
                return str.Remove(str.Length - 1);
            }
        }
        #endregion        
    }
}
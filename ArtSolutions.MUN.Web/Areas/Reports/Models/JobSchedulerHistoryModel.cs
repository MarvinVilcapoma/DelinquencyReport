using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class JobSchedulerHistoryModel
    {
        #region public properties         
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<JobSchedulerHistoryList> JobSchedulerHistoryList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion


    }
    public class JobSchedulerHistoryList
    {
        #region public properties  
        public string JobName { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.TimeSpan> Start_Time { get; set; }
        public Nullable<System.DateTime> Last_Running_Date { get; set; }
        public Nullable<System.TimeSpan> Last_Running_Time { get; set; }
        public string Duration { get; set; }
        public string Frequency { get; set; }
        public string RunStatus { get; set; }
        #endregion
    }
}
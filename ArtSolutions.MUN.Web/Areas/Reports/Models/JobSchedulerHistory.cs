using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class JobSchedulerHistory
    {
        #region public methods  
        public JobSchedulerHistoryModel Get()
        {
            JobSchedulerHistoryModel model = new JobSchedulerHistoryModel();
            model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            return model;
        }
        public JobSchedulerHistoryModel Get(DateTime? startDate, DateTime? endDate)
        {
            JobSchedulerHistoryModel model = new JobSchedulerHistoryModel();           
            model = GetByPaging(startDate, endDate);
            model.JobSchedulerHistoryList = model.JobSchedulerHistoryList ?? new List<Models.JobSchedulerHistoryList>();
            model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            return model;
        }
        public JobSchedulerHistoryModel GetExportLayout(DateTime? startDate, DateTime? endDate)
        {
            return Get(startDate, endDate);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.JobSchedulerHistory, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        private JobSchedulerHistoryModel GetByPaging(DateTime? startDate, DateTime? endDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<JobSchedulerHistoryModel>(APISelector.Municipality, true, "api/Report/JobSchedulerHistoryGet", "GET", lstParameter);
        }
        #endregion
    }
}
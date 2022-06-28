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
    public class ExceptionReport
    {
        #region public methods  
        public ExceptionReportModel Get()
        {
            ExceptionReportModel model = new ExceptionReportModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ExceptionReportModel Get(DateTime? Date)
        {
            ExceptionReportModel model = new ExceptionReportModel();
            model = GetByPaging(Date);
            model.ExceptionList = model.ExceptionList ?? new List<ExceptionList>();
            model.ReportCompanyModel = GetReportCompany(Date, null);
            return model;
        }
        public ExceptionReportModel GetExportLayout(DateTime? Date)
        {
            return Get(Date);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ExceptionTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate, null, null, null, null, false);
        }
        private ExceptionReportModel GetByPaging(DateTime? Date)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
               new NameValuePair { Name = "Date", Value = (Date == null?null: Date.Value.ToString("d", CultureInfo.InvariantCulture)) },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ExceptionReportModel>(APISelector.Municipality, true, "api/Report/ExceptionGet", "GET", lstParameter);
        }
        #endregion
    }
}
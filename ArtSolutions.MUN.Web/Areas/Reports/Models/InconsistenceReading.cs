using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class InconsistenceReading
    {
        #region public methods  
        public InconsistenceReadingModel Get()
        {
            InconsistenceReadingModel model = new InconsistenceReadingModel();
            model.InconsistenceReadingList = model.InconsistenceReadingList ?? new List<Models.InconsistenceReadingList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public InconsistenceReadingModel Get(DateTime? period, int pageIndex, int pageSize)
        {
            InconsistenceReadingModel model = new InconsistenceReadingModel();
            model = GetByPaging(period, pageIndex, pageSize);
            model.InconsistenceReadingList = model.InconsistenceReadingList ?? new List<Models.InconsistenceReadingList>();
            model.ReportCompanyModel = GetReportCompany(null, null, null, null, period);
            return model;
        }
        public InconsistenceReadingModel GetExportLayout(DateTime? period, int pageIndex, int pageSize)
        {
            return Get(period, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, Nullable<decimal> FromRange = null, Nullable<decimal> ToRange = null, Nullable<DateTime> period = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.InconsistenceReadingReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate, FromRange, ToRange, true, period);
        }
        private InconsistenceReadingModel GetByPaging(DateTime? period, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "month", Value = period.Value.Month },
                new NameValuePair { Name = "year", Value = period.Value.Year },
                new NameValuePair { Name = "pageIndex", Value = pageIndex },
                new NameValuePair { Name = "pageSize", Value = pageSize  }
            };
            return new RestSharpHandler().RestRequest<InconsistenceReadingModel>(APISelector.Municipality, true, "api/Report/InconsistenceReadingGet", "GET", lstParameter);
        }
        #endregion
    }
}
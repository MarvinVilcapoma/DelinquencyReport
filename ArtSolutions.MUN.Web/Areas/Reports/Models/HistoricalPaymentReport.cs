using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class HistoricalPaymentReport
    {
        #region public methods  
        public HistoricalPaymentReportModel Get()
        {
            HistoricalPaymentReportModel model = new HistoricalPaymentReportModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public HistoricalPaymentReportModel Get(DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            HistoricalPaymentReportModel model = new HistoricalPaymentReportModel();
            model = GetByPaging(fromDate, toDate, pageIndex, pageSize);
            model.HistoricalPaymentList = model.HistoricalPaymentList ?? new List<Models.HistoricalPaymentList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate);
            return model;
        }
        public HistoricalPaymentReportModel GetExportLayout(DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            return Get(fromDate, toDate, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.HistoricalPaymentReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 14, FromDate, ToDate,null,null,true);
        }
        private HistoricalPaymentReportModel GetByPaging(DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            //DateTime _fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, 1);
            //DateTime _toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1);

            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "fromDate", Value = fromDate.Value.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "toDate", Value = toDate.Value.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "pageIndex", Value = pageIndex },
                new NameValuePair { Name = "pageSize", Value = pageSize  }
            };
            return new RestSharpHandler().RestRequest<HistoricalPaymentReportModel>(APISelector.Municipality, true, "api/Report/HistoricalPaymentReportGet", "GET", lstParameter);
        }
        #endregion
    }
}
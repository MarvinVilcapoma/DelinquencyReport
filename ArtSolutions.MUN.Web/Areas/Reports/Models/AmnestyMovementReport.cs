using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AmnestyMovementReport
    {
        #region public methods  
        public AmnestyMovementReportModel Get()
        {
            AmnestyMovementReportModel model = new AmnestyMovementReportModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AmnestyMovementReportModel Get(DateTime? fromDate, DateTime? toDate)
        {
            AmnestyMovementReportModel model = new AmnestyMovementReportModel();
            model = GetByPaging(fromDate, toDate);
            model.AmnestyMovementReportList = model.AmnestyMovementReportList ?? new List<Models.AmnestyMovementReportList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate);
            return model;
        }
        public AmnestyMovementReportModel GetExportLayout(DateTime? fromDate, DateTime? toDate)
        {
            return Get(fromDate, toDate);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AmnestyMovementReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }
        private AmnestyMovementReportModel GetByPaging(DateTime? fromDate, DateTime? toDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "fromDate", Value = fromDate.Value.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "toDate", Value = toDate.Value.ToString(CultureInfo.InvariantCulture) }
            };
            return new RestSharpHandler().RestRequest<AmnestyMovementReportModel>(APISelector.Municipality, true, "api/Report/AmnestyMovementReportGet", "GET", lstParameter);
        }
        #endregion
    }
}
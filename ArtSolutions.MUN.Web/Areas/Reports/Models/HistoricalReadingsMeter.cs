using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class HistoricalReadingsMeter
    {
        #region public methods  
        public HistoricalReadingsMeterModel Get(int? accountID)
        {
            HistoricalReadingsMeterModel model = new HistoricalReadingsMeterModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;
            if (accountID > 0)
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
            return model;
        }
        public HistoricalReadingsMeterModel Get(string meter, int? accountID)
        {
            HistoricalReadingsMeterModel model = new HistoricalReadingsMeterModel();
            if (!string.IsNullOrEmpty(meter) || accountID > 0)
                model = GetByPaging(meter, accountID);
            model.HistoricalReadingsMeterList = model.HistoricalReadingsMeterList ?? new List<Models.HistoricalReadingsMeterList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public HistoricalReadingsMeterModel GetExportLayout(string meter, int? accountID)
        {
            return Get(meter, accountID);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReportOfHistoricalReadingsByMeter, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }
        private HistoricalReadingsMeterModel GetByPaging(string meter, int? accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "meter", Value = meter },
                new NameValuePair { Name = "accountID", Value = accountID },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<HistoricalReadingsMeterModel>(APISelector.Municipality, true, "api/Report/HistoricalReadingsMeterGet", "GET", lstParameter);
        }
        #endregion
    }
}
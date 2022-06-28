using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ConsumptionByRange
    {
        #region public methods  
        public ConsumptionByRangeModel Get()
        {
            ConsumptionByRangeModel model = new ConsumptionByRangeModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ConsumptionByRangeModel Get(DateTime? fromDate, DateTime? toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize)
        {
            ConsumptionByRangeModel model = new ConsumptionByRangeModel();
            model = GetByPaging(fromDate, toDate, from, to, taxNumber, meter, conditionFrom, conditionTo, conditionType, pageIndex, pageSize);
            model.ConsumptionRangeList = model.ConsumptionRangeList ?? new List<Models.ConsumptionByRangeList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate, from, to);
            return model;
        }
        public ConsumptionByRangeModel GetExportLayout(DateTime? fromDate, DateTime? toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize)
        {
            return Get(fromDate, toDate, from, to, taxNumber, meter, conditionFrom, conditionTo, conditionType, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, Nullable<decimal> FromRange = null, Nullable<decimal> ToRange = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ConsumptionByRangeReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate, FromRange, ToRange, true);
        }
        private ConsumptionByRangeModel GetByPaging(DateTime? fromDate, DateTime? toDate, decimal? from, decimal? to, string taxNumber, string meter, string conditionFrom, string conditionTo, string conditionType, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "fromDate", Value = fromDate.Value.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "toDate", Value = toDate.Value.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "from", Value = from },
                new NameValuePair { Name = "to", Value = to },
                new NameValuePair { Name = "taxNumber", Value = taxNumber },
                new NameValuePair { Name = "meter", Value = meter },
                new NameValuePair { Name = "conditionFrom", Value = conditionFrom },
                new NameValuePair { Name = "conditionTo", Value = conditionTo },
                new NameValuePair { Name = "conditionType", Value = conditionType },
                new NameValuePair { Name = "pageIndex", Value = pageIndex },
                new NameValuePair { Name = "pageSize", Value = pageSize  }
            };
            return new RestSharpHandler().RestRequest<ConsumptionByRangeModel>(APISelector.Municipality, true, "api/Report/ConsumptionByRangeGet", "GET", lstParameter);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountsConsumptionAndCollection
    {
        #region public methods  
        public AccountsConsumptionAndCollectionModel Get()
        {
            AccountsConsumptionAndCollectionModel model = new AccountsConsumptionAndCollectionModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AccountsConsumptionAndCollectionModel Get(DateTime Period)
        {
            AccountsConsumptionAndCollectionModel model = new AccountsConsumptionAndCollectionModel();
            model = GetByPaging(Period);
            model.ReportCompanyModel = GetReportCompany(null, null, null, null, Period, model.ColumnList.Count + 2);
            return model;
        }
        public AccountsConsumptionAndCollectionModel GetExportLayout(DateTime Period)
        {
            return Get(Period);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, Nullable<decimal> FromRange = null, Nullable<decimal> ToRange = null, Nullable<DateTime> period = null, int? totalColumn = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AccountsConsumptionAndCollectionTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, totalColumn, FromDate, ToDate, FromRange, ToRange, true, period);
        }
        private AccountsConsumptionAndCollectionModel GetByPaging(DateTime Period)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "month", Value = Period.Month },
                new NameValuePair { Name = "year", Value = Period.Year }
            };
            return new RestSharpHandler().RestRequest<AccountsConsumptionAndCollectionModel>(APISelector.Municipality, true, "api/Report/AccountsConsumptionAndCollectionGet", "GET", lstParameter);
        }
        #endregion
    }
}
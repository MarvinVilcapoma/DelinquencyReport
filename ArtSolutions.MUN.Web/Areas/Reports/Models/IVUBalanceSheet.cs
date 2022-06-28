using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUBalanceSheet
    {
        #region public methods  
        public IVUBalanceSheetModel Get()
        {
            IVUBalanceSheetModel model = new IVUBalanceSheetModel();            
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public IVUBalanceSheetModel Get(DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? balanceFrom, decimal? balanceTo, JQDTParams jqdtParams)
        {
            IVUBalanceSheetModel model = new IVUBalanceSheetModel();
            if (jqdtParams != null)
                model = GetByPaging(startPeriod, endPeriod, accountIDs, balanceFrom, balanceTo, jqdtParams);
            model.IVUBalanceSheetList = model.IVUBalanceSheetList ?? new List<Models.IVUBalanceSheetList>();
            model.StartPeriod = startPeriod;
            model.EndPeriod = endPeriod;
            model.AccountIDs = accountIDs == null ? null : accountIDs.Split(',');
            model.BalanceFrom = balanceFrom;
            model.BalanceTo = balanceTo;
            model.ReportCompanyModel = GetReportCompany(startPeriod, endPeriod);
            return model;
        }

        public IVUBalanceSheetModel GetExportLayout(DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            return this.Get(startPeriod, endPeriod, accountIDs, balanceFrom, balanceTo, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods  
        private IVUBalanceSheetModel GetByPaging(DateTime startPeriod, DateTime endPeriod, string accountIDs, decimal? balanceFrom, decimal? balanceTo, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startPeriod", Value = startPeriod.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endPeriod", Value = endPeriod.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "rangeFrom", Value = balanceFrom },
                new NameValuePair { Name = "rangeTo", Value = balanceTo },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue}
            };
            return new RestSharpHandler().RestRequest<IVUBalanceSheetModel>(APISelector.Municipality, true, "api/Report/IVUBalanceSheetGet", "GET", lstParameter);
        }

        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.IVUBalanceSheetTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
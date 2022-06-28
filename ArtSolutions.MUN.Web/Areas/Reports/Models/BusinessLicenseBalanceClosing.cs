using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseBalanceClosing
    {
        #region public methods  
        public BusinessLicenseBalanceClosingModel Get()
        {
            BusinessLicenseBalanceClosingModel model = new BusinessLicenseBalanceClosingModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public BusinessLicenseBalanceClosingModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "commaSeperatedAccountIds", Value = commaSeperatedAccountIds },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
            };
            return new RestSharpHandler().RestRequest<BusinessLicenseBalanceClosingModel>(APISelector.Municipality, true, "api/Report/BusinessLicenseBalanceByClosingOperation", "GET", lstParameter);
        }

        public BusinessLicenseBalanceClosingModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
        {
            BusinessLicenseBalanceClosingModel model = new BusinessLicenseBalanceClosingModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, jqdtParams);
            model.BusinessLicenseBalanceByClosingList = model.BusinessLicenseBalanceByClosingList ?? new List<BusinessLicenseBalanceByClosingList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public BusinessLicenseBalanceClosingModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds)
        {
            return this.Get(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ClosingBalanceBusinessLicenseTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 11, FromDate, ToDate);
        }
        #endregion
    }
}
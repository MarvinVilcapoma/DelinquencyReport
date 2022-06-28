using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseCreditBalance
    {
        #region public methods  
        public BusinessLicenseCreditBalanceModel Get()
        {
            BusinessLicenseCreditBalanceModel model = new BusinessLicenseCreditBalanceModel();
            List<SalesCashierModel> cashierList = new SalesCashier().Get().OrderBy(x => x.UserName).ToList();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public BusinessLicenseCreditBalanceModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
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
            return new RestSharpHandler().RestRequest<BusinessLicenseCreditBalanceModel>(APISelector.Municipality, true, "api/Report/BusinessLicenseCreditBalance", "GET", lstParameter);
        }

        public BusinessLicenseCreditBalanceModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
        {
            BusinessLicenseCreditBalanceModel model = new BusinessLicenseCreditBalanceModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, jqdtParams);
            model.BusinessLicenseCreditBalanceList = model.BusinessLicenseCreditBalanceList ?? new List<BusinessLicenseCreditBalanceList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public BusinessLicenseCreditBalanceModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods 
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.CreditBalanceBusinessLicenseTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 12, FromDate, ToDate);
        }
        #endregion
    }
}
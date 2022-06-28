using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseFilingByFilingType
    {
        #region public methods  
        public BusinessLicenseFilingByFilingTypeModel Get()
        {
            BusinessLicenseFilingByFilingTypeModel model = new BusinessLicenseFilingByFilingTypeModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public BusinessLicenseFilingByFilingTypeModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
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
            return new RestSharpHandler().RestRequest<BusinessLicenseFilingByFilingTypeModel>(APISelector.Municipality, true, "api/Report/BusinessLicenseFilingbyFilingType", "GET", lstParameter);
        }

        public BusinessLicenseFilingByFilingTypeModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, JQDTParams jqdtParams)
        {
            BusinessLicenseFilingByFilingTypeModel model = new BusinessLicenseFilingByFilingTypeModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, jqdtParams);
            model.BusinessLicenseFilingByFilingTypeList = model.BusinessLicenseFilingByFilingTypeList ?? new List<BusinessLicenseFilingByFilingTypeList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public BusinessLicenseFilingByFilingTypeModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds)
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
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.BusinessLicenseFilingByFilingTypeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 10, FromDate, ToDate);
        }
        #endregion
    }
}
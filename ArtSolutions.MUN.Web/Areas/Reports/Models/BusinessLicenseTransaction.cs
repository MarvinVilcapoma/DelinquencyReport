using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseTransaction
    {
        #region public methods  
        public BusinessLicenseTransactionModel Get()
        {
            BusinessLicenseTransactionModel model = new BusinessLicenseTransactionModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public BusinessLicenseTransactionModel GetByPaging(DateTime? startDate, DateTime? endDate, int? searchStatus, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "commaSeperatedAccountIds", Value = string.Empty },
                new NameValuePair { Name = "searchStatus", Value = searchStatus },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
            };
            return new RestSharpHandler().RestRequest<BusinessLicenseTransactionModel>(APISelector.Municipality, true, "api/Report/BusinessLicenseTransaction", "GET", lstParameter);
        }

        public BusinessLicenseTransactionModel Get(DateTime? startDate, DateTime? endDate,int? searchStatus, JQDTParams jqdtParams)
        {
            BusinessLicenseTransactionModel model = new BusinessLicenseTransactionModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, searchStatus, jqdtParams);
            model.BusinessLicenseTransactionList = model.BusinessLicenseTransactionList ?? new List<BusinessLicenseTransactionList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public BusinessLicenseTransactionModel GetExportLayout(DateTime? startDate, DateTime? endDate,int? searchStatus)
        {
            return this.Get(startDate, endDate, searchStatus, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.TransactionBusinessLicenseTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, FromDate, ToDate);
        }
        #endregion
    }
}
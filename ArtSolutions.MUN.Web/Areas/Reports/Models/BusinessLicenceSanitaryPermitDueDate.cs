using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenceSanitaryPermitDueDate
    {
        #region public methods  
        public BusinessLicenceSanitaryPermitDueDateModel Get()
        {
            BusinessLicenceSanitaryPermitDueDateModel model = new BusinessLicenceSanitaryPermitDueDateModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public BusinessLicenceSanitaryPermitDueDateModel GetByPaging(DateTime? fromDate, DateTime? toDate, string commaSeperatedAccountIDs, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "fromDate", Value = fromDate == null?null: fromDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "toDate", Value =toDate==null?null:toDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "commaSeperatedAccountIDs", Value = commaSeperatedAccountIDs },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
            };
            return new RestSharpHandler().RestRequest<BusinessLicenceSanitaryPermitDueDateModel>(APISelector.Municipality, true, "api/Report/BusinessLicenceSanitaryPermitDueDate", "GET", lstParameter);
        }
        public BusinessLicenceSanitaryPermitDueDateModel Get(DateTime? fromDate, DateTime? toDate, string commaSeperatedAccountIDs, JQDTParams jqdtParams)
        {
            BusinessLicenceSanitaryPermitDueDateModel model = new BusinessLicenceSanitaryPermitDueDateModel();
            if (jqdtParams != null)
                model = GetByPaging(fromDate, toDate, commaSeperatedAccountIDs, jqdtParams);
            model.BusinessLicenceSanitaryPermitDueDateList = model.BusinessLicenceSanitaryPermitDueDateList ?? new List<BusinessLicenceSanitaryPermitDueDateList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate);
            return model;
        }
        public BusinessLicenceSanitaryPermitDueDateModel GetExportLayout(DateTime? fromDate, DateTime? toDate, string commaSeperatedAccountIDs)
        {
            return this.Get(fromDate, toDate, commaSeperatedAccountIDs, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.SanitaryPermitDueDateBusinessLicenseTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 5, fromDate, toDate,null,null,null,null,false);
        }
        #endregion
    }
}
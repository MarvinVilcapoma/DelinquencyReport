using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DuplicatedMeterNumber
    {
        #region public methods  
        public DuplicatedMeterNumberModel Get()
        {
            DuplicatedMeterNumberModel model = new DuplicatedMeterNumberModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public DuplicatedMeterNumberModel GetDuplicatedMeterNumber()
        {
            DuplicatedMeterNumberModel model = new DuplicatedMeterNumberModel();
            model = GetByPaging();
            model.DuplicatedMeterNumberList = model.DuplicatedMeterNumberList ?? new List<DuplicatedMeterNumberList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public DuplicatedMeterNumberModel GetExportLayout()
        {
            return GetDuplicatedMeterNumber();
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DuplicatedMeterNumberTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, null, null, null,null, null, null, false);
        }
        private DuplicatedMeterNumberModel GetByPaging()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<DuplicatedMeterNumberModel>(APISelector.Municipality, true, "api/Report/DuplicatedMeterNumberGet", "GET", lstParameter);
        }
        #endregion
    }
}
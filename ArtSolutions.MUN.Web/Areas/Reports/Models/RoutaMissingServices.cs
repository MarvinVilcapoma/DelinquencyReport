using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class RoutaMissingServices
    {
        #region public methods  
        public RoutaMissingServicesModel Get()
        {
            RoutaMissingServicesModel model = new RoutaMissingServicesModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public RoutaMissingServicesModel GetRoutaMissingServices()
        {
            RoutaMissingServicesModel model = new RoutaMissingServicesModel();
            model = GetByPaging();
            model.RoutaMissingServicesList = model.RoutaMissingServicesList ?? new List<RoutaMissingServicesList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public RoutaMissingServicesModel GetExportLayout()
        {
            return GetRoutaMissingServices();
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.RoutaMissingServices, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, null, null, null, null, null, null, false);
        }
        private RoutaMissingServicesModel GetByPaging()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<RoutaMissingServicesModel>(APISelector.Municipality, true, "api/Report/RoutaMissingServicesGet", "GET", lstParameter);
        }
        #endregion
    }
}
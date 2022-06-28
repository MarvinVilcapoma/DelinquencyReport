using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ParkMaintenanceMissingServices
    {
        #region public methods  
        public ParkMaintenanceMissingServicesModel Get()
        {
            ParkMaintenanceMissingServicesModel model = new ParkMaintenanceMissingServicesModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ParkMaintenanceMissingServicesModel GetParkMaintenanceMissingServices(int? year)
        {
            ParkMaintenanceMissingServicesModel model = new ParkMaintenanceMissingServicesModel();
            model = GetByPaging(year);
            model.ParkMaintenanceMissingServicesList = model.ParkMaintenanceMissingServicesList ?? new List<ParkMaintenanceMissingServicesList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ParkMaintenanceMissingServicesModel GetExportLayout(int? year)
        {
            return GetParkMaintenanceMissingServices(year);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ParkMaintenanceMissingServicesTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7,null,null,null,null,null,null,false);
        }
        private ParkMaintenanceMissingServicesModel GetByPaging(int? year)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "year", Value = year },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ParkMaintenanceMissingServicesModel>(APISelector.Municipality, true, "api/Report/ParkMaintenanceMissingServicesGet", "GET", lstParameter);
        }
        #endregion
    }
}
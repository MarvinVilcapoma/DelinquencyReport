using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionDailyIncomeState
    {
        #region public methods
        public CollectionDailyIncomeStateModel Get()
        {
            CollectionDailyIncomeStateModel model = new CollectionDailyIncomeStateModel();

            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CollectionDailyIncomeStateModel Get(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            CollectionDailyIncomeStateModel model = new CollectionDailyIncomeStateModel();
            model = GetByPaging(startPeriod, serviceIds, pageIndex, pageSize);
            model.DailyIncomeStateList = model.DailyIncomeStateList ?? new List<DailyIncomeStateModel>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CollectionDailyIncomeStateModel GetList(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            return Get(startPeriod, serviceIds, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(DateTime? startPeriod = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DailyIncomeStateTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, null, null, null, null, null, null, false);
        }
        private CollectionDailyIncomeStateModel GetByPaging(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriod", Value = startPeriod.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "serviceIds", Value = serviceIds });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<CollectionDailyIncomeStateModel>(APISelector.Municipality, true, "api/Report/CollectionDailyIncomeStateGet", "GET", lstParameter);
        }
        #endregion
    }
}
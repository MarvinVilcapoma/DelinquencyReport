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
    public class CollectionRevenueDailyCollection
    {
        #region public methods
        public CollectionRevenueDailyCollectionModel Get()
        {
            CollectionRevenueDailyCollectionModel model = new CollectionRevenueDailyCollectionModel();

            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CollectionRevenueDailyCollectionModel Get(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            CollectionRevenueDailyCollectionModel model = new CollectionRevenueDailyCollectionModel();
            model = GetByPaging(startPeriod, serviceIds, pageIndex, pageSize);
            model.RevenueDailyCollectionList = model.RevenueDailyCollectionList ?? new List<RevenueDailyCollectionModel>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CollectionRevenueDailyCollectionModel GetList(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            return Get(startPeriod, serviceIds, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(DateTime? startPeriod = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.RevenueDailyCollectionTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, null, null, null, null, null, null, false);
        }
        private CollectionRevenueDailyCollectionModel GetByPaging(DateTime startPeriod, string serviceIds, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriod", Value = startPeriod.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "serviceIds", Value = serviceIds });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<CollectionRevenueDailyCollectionModel>(APISelector.Municipality, true, "api/Report/RevenueByDailyCollectionGet", "GET", lstParameter);
        }
        #endregion
    }
}
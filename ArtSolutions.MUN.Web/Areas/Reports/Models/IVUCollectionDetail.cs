using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUCollectionDetail
    {
        #region public methods
        public IVUCollectionDetailModel Get()
        {
            IVUCollectionDetailModel model = new IVUCollectionDetailModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public IVUCollectionDetailModel Get(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate)
        {
            IVUCollectionDetailModel model = new IVUCollectionDetailModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, StartPeriodDate, EndPeriodDate);
            model.IVUCollectionDetailList = model.IVUCollectionDetailList ?? new List<Models.IVUCollectionDetailList>();
            model.StartPeriodDate = StartPeriodDate;
            model.EndPeriodDate = EndPeriodDate;
            model.ReportCompanyModel = GetReportCompany(StartPeriodDate, EndPeriodDate);            
            return model;
        }

        public IVUCollectionDetailModel GetByPaging(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            IVUCollectionDetailModel model = new RestSharpHandler().RestRequest<IVUCollectionDetailModel>(APISelector.Municipality, true, "api/Report/IVUCollectionDetailGet", "GET", lstParameter);
            return model;
        }

        public IVUCollectionDetailModel GetExportLayout(DateTime StartPeriodDate, DateTime EndPeriodDate)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, StartPeriodDate, EndPeriodDate);

        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.IVUCollectionDetailTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 13, FromDate, ToDate);
        }
        #endregion
    }
}
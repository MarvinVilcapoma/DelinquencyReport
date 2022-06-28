using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class RightsByOwnersID
    {
        #region public methods  
        public RightsByOwnersIDModel Get()
        {
            RightsByOwnersIDModel model = new RightsByOwnersIDModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public RightsByOwnersIDModel Get(JQDTParams jqdtParams, int? ownerID)
        {
            RightsByOwnersIDModel model = new RightsByOwnersIDModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, ownerID);
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public RightsByOwnersIDModel GetByPaging(JQDTParams jqdtParams, int? ownerID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "ownerID", Value = ownerID },
                new NameValuePair { Name = "pageIndex", Value = 0  },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue}
            };
            return new RestSharpHandler().RestRequest<RightsByOwnersIDModel>(APISelector.Municipality, true, "api/Report/RightsByOwnersID", "GET", lstParameter);
        }
        public RightsByOwnersIDModel GetExportLayout(int? ownerID)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, ownerID);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ListOfRightsByOwnersIDReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 13, FromDate, ToDate);
        }
        #endregion
    }
}
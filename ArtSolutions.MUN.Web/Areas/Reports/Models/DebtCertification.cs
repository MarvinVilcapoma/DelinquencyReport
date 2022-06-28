using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DebtCertification
    {
        #region public methods  
        public DebtCertificationModel Get()
        {
            DebtCertificationModel model = new DebtCertificationModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public DebtCertificationModel Get(JQDTParams jqdtParams, int? accountId)
        {
            DebtCertificationModel model = new DebtCertificationModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId);
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public DebtCertificationModel GetByPaging(JQDTParams jqdtParams, int? accountId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "accountID", Value = accountId },
                new NameValuePair { Name = "pageIndex", Value = 0  },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue}
            };
            return new RestSharpHandler().RestRequest<DebtCertificationModel>(APISelector.Municipality, true, "api/Report/DebtCertification", "GET", lstParameter);
        }
        public DebtCertificationModel GetExportLayout(int? accountId)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DebtCertificationTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
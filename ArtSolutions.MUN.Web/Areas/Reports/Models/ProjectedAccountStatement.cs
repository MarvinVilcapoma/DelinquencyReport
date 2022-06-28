using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ProjectedAccountStatement
    {
        #region public methods      
        public ProjectedAccountStatementModel Get(int? accountID)
        {
            ProjectedAccountStatementModel model = new ProjectedAccountStatementModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;

            if (accountID > 0)
            {
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
            }
            return model;
        }
        public ProjectedAccountStatementModel Get(JQDTParams jqdtParams, int? accountId, DateTime? tillDate)
        {
            ProjectedAccountStatementModel model = new ProjectedAccountStatementModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId,tillDate);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();
            model.Date = tillDate;
            return model;
        }
        public ProjectedAccountStatementModel GetByPaging(JQDTParams jqdtParams, int? accountId, DateTime? tillDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "tillDate", Value = (accountId > 0 && tillDate != null) ? tillDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<ProjectedAccountStatementModel>(APISelector.Municipality, true, "api/Report/ProjectedAccountStatementGet", "GET", lstParameter);
        }
        public ProjectedAccountStatementModel GetExportLayout(int? accountId, DateTime? tillDate)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId,tillDate);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ProjectedAccountStatement, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUFormsNotFiled
    {
        #region public methods
        public IVUFormsNotFiledModel Get()
        {
            IVUFormsNotFiledModel model = new IVUFormsNotFiledModel();
            List<AccountModel> accountList = new AccountModel().Get(null, 0, null, null, null, null, true).OrderBy(x => x.DisplayName).ToList();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public IVUFormsNotFiledModel Get(string accountIDs, DateTime? since, DateTime? till, JQDTParams jqdtParams)
        {
            IVUFormsNotFiledModel model = new IVUFormsNotFiledModel();
            if (jqdtParams != null)
                model = GetByPaging(accountIDs, since, till, jqdtParams);
            model.IVUFormsNotFiledList = model.IVUFormsNotFiledList ?? new List<Models.IVUFormsNotFiledList>();
            model.Since = since ?? DateTime.Today;
            model.Till = till ?? DateTime.Today;
            model.AccountIDs = accountIDs == null ? null : accountIDs.Split(',');
            model.ReportCompanyModel = GetReportCompany(since, till);
            return model;
        }

        public IVUFormsNotFiledModel GetExportLayout(string accountIDs, DateTime? since, DateTime? till)
        {
            return this.Get(accountIDs, since, till, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods
        private IVUFormsNotFiledModel GetByPaging(string accountIDs, DateTime? since, DateTime? till, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "sinceMonth", Value = since.HasValue ? since.Value.Month : 0 },
                new NameValuePair { Name = "sinceYear", Value = since.HasValue ? since.Value.Year : 0 },
                new NameValuePair { Name = "tillMonth", Value = till.HasValue ? till.Value.Month : 0 },
                new NameValuePair { Name = "tilleYear", Value = till.HasValue ? till.Value.Year : 0 },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
            };
            return new RestSharpHandler().RestRequest<IVUFormsNotFiledModel>(APISelector.Municipality, true, "api/Report/IVUFormsNotFiledGet", "GET", lstParameter);
        }

        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.IVUFormsNotFiledTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 5, FromDate, ToDate);
        }
        #endregion
    }
}
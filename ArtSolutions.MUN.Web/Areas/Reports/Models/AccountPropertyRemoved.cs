using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountPropertyRemoved
    {
        #region public methods  
        public AccountPropertyRemovedModel Get()
        {
            AccountPropertyRemovedModel model = new AccountPropertyRemovedModel();
            model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            model.AccountList = new AccountModel().GetForSearch(null, null, null, null).ToList();
            return model;
        }
        public AccountPropertyRemovedModel Get(DateTime? startDate, DateTime? endDate)
        {
            AccountPropertyRemovedModel model = new AccountPropertyRemovedModel();           
            model = GetByPaging(UserSession.Current.Id, startDate, endDate);
            model.AccountPropertyRemovedList = model.AccountPropertyRemovedList ?? new List<Models.AccountPropertyRemovedList>();
            model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            return model;
        }
        public AccountPropertyRemovedModel GetExportLayout(DateTime? startDate, DateTime? endDate)
        {
            return Get(startDate, endDate);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReportOfAccountPropertyRemoved, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, FromDate, ToDate);
        }
        private AccountPropertyRemovedModel GetByPaging(Guid RemovedByaccountID, DateTime? startDate, DateTime? endDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "removedByaccountID", Value = RemovedByaccountID },
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<AccountPropertyRemovedModel>(APISelector.Municipality, true, "api/Report/AccountPropertyRemovedGet", "GET", lstParameter);
        }
        #endregion
    }
}
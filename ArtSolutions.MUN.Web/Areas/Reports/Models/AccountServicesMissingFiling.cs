using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountServicesMissingFiling
    {
        #region public methods  
        public AccountServicesMissingFilingModel Get()
        {
            AccountServicesMissingFilingModel model = new AccountServicesMissingFilingModel();
            model.ServiceList = HMTLHelperExtensions.CreateSelectList(new Services.Models.Service().Get(null, null, null, false, null), "ID", "Name");
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AccountServicesMissingFilingModel Get(int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            AccountServicesMissingFilingModel model = new AccountServicesMissingFilingModel();
            model = GetByPaging(accountId, commaSeperatedServiceIDs, pageIndex, pageSize);
            model.AccountServicesMissingFilingList = model.AccountServicesMissingFilingList ?? new List<Models.AccountServicesMissingFilingList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AccountServicesMissingFilingModel GetList(int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            return Get(accountId, commaSeperatedServiceIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AccountServicesMissingFiling, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance,8, null, null, null, null, null, null, false);
        }
        private AccountServicesMissingFilingModel GetByPaging(int? accountId, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceIDs", Value = commaSeperatedServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<AccountServicesMissingFilingModel>(APISelector.Municipality, true, "api/Report/AccountServicesMissingFilingGet", "GET", lstParameter);
        }
        #endregion
    }
}
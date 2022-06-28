using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountExclusionForm
    {
        #region public methods  
        public AccountExclusionFormModel Get()
        {
            AccountExclusionFormModel model = new AccountExclusionFormModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AccountExclusionFormModel Get(int? accountId, string observations)
        {
            AccountExclusionFormModel model = new AccountExclusionFormModel();
            model = GetByPaging(accountId, observations);
            model.ReportCompanyModel = GetReportCompany();
            model.Observations = observations;
            return model;
        }
        public AccountExclusionFormModel GetExportLayout(int? accountId, string observations)
        {
            return Get(accountId, observations);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AccountExclusionFormReportTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, null, null, null, null, null, null, false);
        }
        private AccountExclusionFormModel GetByPaging(int? accountId, string observations)
        {
            AccountExclusionFormModel model = new AccountExclusionFormModel();
            model.AccountId = accountId;
            model.Observations = observations;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<AccountExclusionFormModel>(APISelector.Municipality, true, "api/Report/AccountExclusionForm", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
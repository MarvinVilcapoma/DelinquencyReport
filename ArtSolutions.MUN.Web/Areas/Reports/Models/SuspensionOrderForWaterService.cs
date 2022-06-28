using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class SuspensionOrderForWaterService
    {
        #region public methods      
        public SuspensionOrderForWaterServiceModel Get(int? accountID)
        {
            SuspensionOrderForWaterServiceModel model = new SuspensionOrderForWaterServiceModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;

            if (accountID > 0)
            {
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
            }
            return model;
        }
        public SuspensionOrderForWaterServiceModel Get(JQDTParams jqdtParams, int? accountId)
        {
            SuspensionOrderForWaterServiceModel model = new SuspensionOrderForWaterServiceModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId);
            //model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, null).FirstOrDefault();
            //model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public SuspensionOrderForWaterServiceModel GetByPaging(JQDTParams jqdtParams, int? accountId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<SuspensionOrderForWaterServiceModel>(APISelector.Municipality, true, "api/Report/SuspensionOrderForWaterServiceGet", "GET", lstParameter);
        }
        public SuspensionOrderForWaterServiceModel GetExportLayout(int? accountId)
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
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.AccountService.SuspensionOrder, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
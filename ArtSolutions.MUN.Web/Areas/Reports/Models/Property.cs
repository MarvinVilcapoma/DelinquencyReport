using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class Property
    {
        #region public methods      
        public PropertyModel Get(int? accountID)
        {
            PropertyModel model = new PropertyModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;
            if (accountID > 0)
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
            return model;
        }
        public PropertyModel Get(JQDTParams jqdtParams, int? accountId, string accountPropertyIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            PropertyModel model = new PropertyModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, accountPropertyIDs, balanceFrom, balanceTo);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, null).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public PropertyModel GetByPaging(JQDTParams jqdtParams, int? accountId, string accountPropertyIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyIDs });
            lstParameter.Add(new NameValuePair { Name = "rangeFrom", Value = balanceFrom });
            lstParameter.Add(new NameValuePair { Name = "rangeTo", Value = balanceTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            PropertyModel model = new RestSharpHandler().RestRequest<PropertyModel>(APISelector.Municipality, true, "api/Report/PropertyMovementGet", "GET", lstParameter);
            return model;
        }
        public PropertyModel GetExportLayout(int? accountId, string accountPropertyIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, accountPropertyIDs, balanceFrom, balanceTo);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.PropertyTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }
        #endregion
    }
}
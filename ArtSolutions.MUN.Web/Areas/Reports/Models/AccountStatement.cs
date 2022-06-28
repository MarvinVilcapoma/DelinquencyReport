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
    public class AccountStatement
    {
        #region public methods      
        public AccountStatementModel Get(int? accountID)
        {
            AccountStatementModel model = new AccountStatementModel();
            CreditHistoryModel creditHistory = new CreditHistoryModel();
            DebitHistoryModel debitHistory = new DebitHistoryModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;

            if (accountID > 0)
            {
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
                List<Accounts.Models.PropertyModel> list = new List<Accounts.Models.PropertyModel>();
                list.Add(new Accounts.Models.PropertyModel() { Property = Resources.Global.All, AccountPropertyID = -1 });
                list.Add(new Accounts.Models.PropertyModel() { Property = Resources.Report.Unassigned, AccountPropertyID = -2 });
                list.AddRange(new AccountProperty().GetAccountPropertyRightByOwner(accountID.Value, 0, 0, null));
                model.PropertyList = HMTLHelperExtensions.CreateSelectList(list, "AccountPropertyID", "Property");
                model.CreditBalance = creditHistory.Get(accountID.Value).CreditBalance;
                model.DebitBalance = debitHistory.Get(accountID.Value).DebitBalance;
            }
            return model;
        }
        public AccountStatementModel Get(JQDTParams jqdtParams, int? accountId, int? accountPropertyID, int? tillYear, int? tillPeriod,DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport)
        {
            AccountStatementModel model = new AccountStatementModel();
            CreditHistoryModel creditHistory = new CreditHistoryModel();
            DebitHistoryModel debitHistory = new DebitHistoryModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, accountPropertyID, tillYear, tillPeriod,tillDate, accountServiceCollectionDetailIDs, isExport);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, null).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();

            model.Year = tillYear;
            model.Date = tillDate;
            model.CreditBalance = creditHistory.Get(accountId.Value).CreditBalance;
            model.DebitBalance = debitHistory.Get(accountId.Value).DebitBalance;
            return model;
        }
        public AccountStatementModel GetByPaging(JQDTParams jqdtParams, int? accountId, int? accountPropertyID, int? tillYear,int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "tillYear", Value = accountId > 0 ? tillYear : null });
            lstParameter.Add(new NameValuePair { Name = "tillPeriod", Value = accountId > 0 ? tillPeriod : null });
            lstParameter.Add(new NameValuePair { Name = "tillDate", Value = (accountId > 0 && tillDate != null) ? tillDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailIDs", Value = accountId > 0 ? accountServiceCollectionDetailIDs : null });
            lstParameter.Add(new NameValuePair { Name = "isExport", Value = isExport });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            AccountStatementModel model = new RestSharpHandler().RestRequest<AccountStatementModel>(APISelector.Municipality, true, "api/Report/AccountStatementGet", "GET", lstParameter);
            return model;
        }
        public AccountStatementModel GetExportLayout(int? accountId, int? accountPropertyID, int? tillYear,int? tillPeriod, DateTime? tillDate, string accountServiceCollectionDetailIDs, bool isExport)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, accountPropertyID, tillYear, tillPeriod, tillDate, accountServiceCollectionDetailIDs, isExport);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AccountStatementTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 13, FromDate, ToDate);
        }
        #endregion
    }
}
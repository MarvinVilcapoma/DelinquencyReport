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
    public class IVUStatement
    {
        #region public methods      
        public IVUStatementModel Get()
        {
            IVUStatementModel model = new IVUStatementModel();
            model.AccountList.Insert(0, new SelectListItemViewModel { ID = "", Name = Resources.Global.DropDownSelectMessage });
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public IVUStatementModel Get(JQDTParams jqdtParams, int accountId, decimal? balanceFrom, decimal? balanceTo, string statementType, DateTime? futureDate = null)
        {
            IVUStatementModel model = new IVUStatementModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, balanceFrom, balanceTo, futureDate);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.BalanceFrom = balanceFrom;
            model.BalanceTo = balanceTo;
            model.StatementType = statementType;
            model.AccountPaymentPlanID = GetActiveAccountPaymentPlan(model.AccountModel.ID);
            model.ReportCompanyModel = GetReportCompany();
            model.FutureDate = futureDate.HasValue ? futureDate.Value : DateTime.Now;
            return model;
        }

        public IVUStatementModel GetAccountDetail(int accountId, decimal? balanceFrom, decimal? balanceTo)
        {
            IVUStatementModel model = new IVUStatementModel();
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.BalanceFrom = balanceFrom;
            model.BalanceTo = balanceTo;
            model.ReportCompanyModel = new ReportCompanyModel().Get();
            model.AccountPaymentPlanID = GetActiveAccountPaymentPlan(model.AccountModel.ID);
            return model;
        }

        public IVUStatementModel GetByPaging(JQDTParams jqdtParams, int accountId, decimal? balanceFrom, decimal? balanceTo, DateTime? futureDate = null)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "BalanceFrom", Value = balanceFrom });
            lstParameter.Add(new NameValuePair { Name = "BalanceTo", Value = balanceTo });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });

            if (futureDate.HasValue)
            {
                lstParameter.Add(new NameValuePair { Name = "FutureDate", Value = futureDate.Value.ToString("d", CultureInfo.InvariantCulture) });
                IVUStatementModel model = new RestSharpHandler().RestRequest<IVUStatementModel>(APISelector.Municipality, true, "api/Report/IVUStatementGetForHP", "GET", lstParameter);
                return model;
            }
            else
            {
                IVUStatementModel model = new RestSharpHandler().RestRequest<IVUStatementModel>(APISelector.Municipality, true, "api/Report/IVUStatementGet", "GET", lstParameter);
                return model;
            }
        }

        public IVUStatementModel GetExportLayout(int accountId, decimal? balanceFrom, decimal? balanceTo, string statementType, DateTime? futureDate = null)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, balanceFrom, balanceTo, statementType, futureDate);
        }
        #endregion

        #region private methods   
        private string GetActiveAccountPaymentPlan(int accountID)
        {
            List<AccountPaymentPlanModel> paymentPlanList = new Accounts.Models.AccountPaymentPlan().Get(null, accountID, true);
            if (paymentPlanList.Count > 0)
            {
                //List<AccountPaymentPlanModel> ivuPaymentPlanList = paymentPlanList.Where(x => x.ServiceTypeID == 3).ToList();
                List<AccountPaymentPlanModel> ivuPaymentPlanList = paymentPlanList;
                if (ivuPaymentPlanList.Count > 0)
                    return ivuPaymentPlanList.Last().Number;
            }
            return "";
        }

        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.IVUStatementTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
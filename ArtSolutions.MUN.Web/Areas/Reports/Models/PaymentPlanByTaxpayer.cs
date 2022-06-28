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
    public class PaymentPlanByTaxpayer
    {
        #region public methods      
        public PaymentPlanByTaxpayerModel Get(int? accountID)
        {
            PaymentPlanByTaxpayerModel model = new PaymentPlanByTaxpayerModel();
            model.ReportCompanyModel = GetReportCompany();
            model.AccountId = accountID;

            if (accountID > 0)
            {
                model.AccountList = new AccountModel().GetForSearch(null, accountID, null, null).ToList();
            }

            return model;
        }
        public PaymentPlanByTaxpayerModel Get(JQDTParams jqdtParams, int? accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            PaymentPlanByTaxpayerModel model = new PaymentPlanByTaxpayerModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail);
            model.AccountId = accountID;
            model.ReportCompanyModel = GetReportCompany();

            model.Year = tillYear;
            model.Date = tillDate;
            model.QuotasToCalculate = quotasToCalculate;
            return model;
        }
        public PaymentPlanByTaxpayerModel GetByPaging(JQDTParams jqdtParams, int? accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "accountPaymentPlanIDs", Value = accountID > 0 ? accountPaymentPlanIDs : null });
            lstParameter.Add(new NameValuePair { Name = "tillYear", Value = accountID > 0 ? tillYear : null });
            lstParameter.Add(new NameValuePair { Name = "tillDate", Value = (accountID > 0 && tillDate != null) ? tillDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            lstParameter.Add(new NameValuePair { Name = "quotasToCalculate", Value = accountID > 0 ? quotasToCalculate : null });
            lstParameter.Add(new NameValuePair { Name = "rowNos", Value = accountID > 0 ? rowNos : null });
            lstParameter.Add(new NameValuePair { Name = "isTermDetail", Value = isTermDetail });
            return new RestSharpHandler().RestRequest<PaymentPlanByTaxpayerModel>(APISelector.Municipality, true, "api/Report/PaymentPlanByTaxpayerGet", "GET", lstParameter);
        }
        public PaymentPlanByTaxpayerModel GetExportLayout(int? accountID, string accountPaymentPlanIDs, int? tillYear, DateTime? tillDate, int? quotasToCalculate, string rowNos, bool? isTermDetail)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountID, accountPaymentPlanIDs, tillYear, tillDate, quotasToCalculate, rowNos, isTermDetail);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.PaymentPlanByTaxpayer, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }
        #endregion
    }
}
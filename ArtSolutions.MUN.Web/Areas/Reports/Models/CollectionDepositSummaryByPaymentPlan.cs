using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionDepositSummaryByPaymentPlan
    {
        #region public methods
        public CollectionDepositSummaryByPaymentPlanModel Get()
        {
            CollectionDepositSummaryByPaymentPlanModel model = new CollectionDepositSummaryByPaymentPlanModel();
            model.ReportCompanyModel = GetReportCompany();

            List<PaymentPlanModel> paymentPlanModel = new PaymentPlan().Get(null,null,true);
            model.PaymentPlanList = paymentPlanModel.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.PaymentPlanList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            model.BankAccountList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            return model;
        }

        public CollectionDepositSummaryByPaymentPlanModel Get(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate,decimal? netDepositFrom, decimal? netDepositTo,string commaSeperatedPaymentPlanIds, string commaSeperatedBankAccountIds)
        {
            CollectionDepositSummaryByPaymentPlanModel model = new CollectionDepositSummaryByPaymentPlanModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate,netDepositFrom, netDepositTo, commaSeperatedPaymentPlanIds, commaSeperatedBankAccountIds);
            model.DepositEntryList = model.DepositEntryList ?? new List<DepositEntryModel>();
            model.DepositEntryList.ForEach(x => x.ClosedEntryList = model.ClosingEntryList.Where(c => c.DepositID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = startPeriodDate;
            model.EndPeriodDate = endPeriodDate;
            model.DepositEntryList.ForEach(d => d.ClosedEntryList.ForEach(k =>
            {
                k.FormatingDate = k.FormattedDate;
            }));
            model.ReportCompanyModel = GetReportCompany(startPeriodDate, endPeriodDate);
            return model;
        }

        public CollectionDepositSummaryByPaymentPlanModel GetByPaging(JQDTParams jqdtParams, DateTime? startPeriodDate, DateTime? endPeriodDate,decimal? netDepositFrom, decimal? netDepositTo,string commaSeperatedPaymentPlanIds, string commaSeperatedBankAccountIds)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedPaymentPlanIds", Value = commaSeperatedPaymentPlanIds });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedBankAccountIds", Value = commaSeperatedBankAccountIds });
            lstParameter.Add(new NameValuePair { Name = "netDepositFrom", Value = netDepositFrom });
            lstParameter.Add(new NameValuePair { Name = "netDepositTo", Value = netDepositTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            CollectionDepositSummaryByPaymentPlanModel model = new RestSharpHandler().RestRequest<CollectionDepositSummaryByPaymentPlanModel>(APISelector.Municipality, true, "api/Report/CollectionDepositSummaryByPaymentPlanGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DepositSummaryReportByPaymentPlan, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, fromDate, toDate);
        }
        #endregion
    }
}
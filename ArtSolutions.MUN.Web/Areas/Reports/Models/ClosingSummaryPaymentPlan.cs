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
    public class ClosingSummaryPaymentPlan
    {
        #region public methods
        public ClosingSummaryPaymentPlanModel Get()
        {
            ClosingSummaryPaymentPlanModel model = new ClosingSummaryPaymentPlanModel();
            model.ReportCompanyModel = GetReportCompany();

            List<PaymentPlanModel> paymentPlanModel = new PaymentPlan().Get(null, null, true);
            model.PaymentPlanList = paymentPlanModel.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.PaymentPlanList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            return model;
        }

        public ClosingSummaryPaymentPlanModel Get(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            ClosingSummaryPaymentPlanModel model = new ClosingSummaryPaymentPlanModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate, commaSeperatedPaymentPlanIDs, netClosingFrom, netClosingTo);
            model.ClosingEntryList = model.ClosingEntryList ?? new List<ClosingEntryModel>();
            // Set Payment Receipt as Child of Each Closing Receipt
            model.ClosingEntryList.ForEach(x => x.PostedPaymentReceiptList = model.PaymentList.Where(c => c.ClosingID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = startPeriodDate;
            model.EndPeriodDate = endPeriodDate;
            model.ReportCompanyModel = GetReportCompany(startPeriodDate, endPeriodDate);
            return model;
        }

        public ClosingSummaryPaymentPlanModel GetByPaging(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, string commaSeperatedPaymentPlanIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedPaymentPlanIDs", Value = commaSeperatedPaymentPlanIDs });
            lstParameter.Add(new NameValuePair { Name = "netClosingFrom", Value = netClosingFrom });
            lstParameter.Add(new NameValuePair { Name = "netClosingTo", Value = netClosingTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<ClosingSummaryPaymentPlanModel>(APISelector.Municipality, true, "api/Report/ClosingSummaryByPaymentPlan", "GET", lstParameter);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Global.CashRegisterSummaryByPaymentPlan, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
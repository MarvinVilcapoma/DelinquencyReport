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
    public class ClosingSummaryInvoice
    {
        #region public methods
        public ClosingSummaryInvoiceModel Get()
        {
            ClosingSummaryInvoiceModel model = new ClosingSummaryInvoiceModel();
            model.ReportCompanyModel = GetReportCompany();
            model.GrantList = new Service().GetGrantList();
            model.GrantList.Insert(0, new GrantModel { GrantID = 0, GrantName = Resources.Global.DropDownSelectAllMessage });
            model.OtherServiceList = new Service().GetForNoFiling();
            model.OtherServiceList.Insert(0, new ServiceModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            return model;
        }

        public ClosingSummaryInvoiceModel Get(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string otherServiceIDs, string grantIDs)
        {
            ClosingSummaryInvoiceModel model = new ClosingSummaryInvoiceModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate, netClosingFrom, netClosingTo,otherServiceIDs,grantIDs);
            model.ClosingEntryList = model.ClosingEntryList ?? new List<ClosingEntryModel>();
            // Set Payment Receipt as Child of Each Closing Receipt
            model.ClosingEntryList.ForEach(x => x.PostedPaymentReceiptList = model.PaymentList.Where(c => c.ClosingID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = startPeriodDate;
            model.EndPeriodDate = endPeriodDate;
            model.ReportCompanyModel = GetReportCompany(startPeriodDate, endPeriodDate);
            return model;
        }


        public ClosingSummaryInvoiceModel GetByPaging(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string otherServiceIDs, string grantIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "netClosingFrom", Value = netClosingFrom });
            lstParameter.Add(new NameValuePair { Name = "netClosingTo", Value = netClosingTo });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedOtherServiceIDs", Value = otherServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedGrantIDs", Value = grantIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<ClosingSummaryInvoiceModel>(APISelector.Municipality, true, "api/Report/ClosingSummaryByInvoice", "GET", lstParameter);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Global.CashRegisterSummaryByInvoice, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
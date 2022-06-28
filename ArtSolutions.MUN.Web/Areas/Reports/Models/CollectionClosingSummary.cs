using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionClosingSummary
    {
        #region public methods
        public CollectionClosingSummaryModel Get()
        {
            CollectionClosingSummaryModel model = new CollectionClosingSummaryModel();

            model.CashierList = new SelectList(new SalesCashier().Get(), "UserID", "UserName").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();

            return model;
        }

        public CollectionClosingSummaryModel Get(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, Guid[] CashierIDs, decimal? NetClosingFrom , decimal? NetClosingTo)
        {
            CollectionClosingSummaryModel model = new CollectionClosingSummaryModel();
            string cashierIds = null;
            if (CashierIDs != null)
            {
                cashierIds = string.Join(",", CashierIDs);
                if (Array.Exists(CashierIDs, element => element.Equals(new Guid("00000000-0000-0000-0000-000000000000"))))
                    cashierIds = null;
            }
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, StartPeriodDate, EndPeriodDate, cashierIds,NetClosingFrom,NetClosingTo);
            model.ClosingEntryList = model.ClosingEntryList ?? new List<ClosingEntryModel>();
            // Set Payment Receipt as Child of Each Closing Receipt
            model.ClosingEntryList.ForEach(x => x.PostedPaymentReceiptList = model.PaymentList.Where(c => c.ClosingID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = StartPeriodDate;
            model.EndPeriodDate = EndPeriodDate;
            model.ReportCompanyModel = GetReportCompany(StartPeriodDate, EndPeriodDate);
            return model;
        }

        public CollectionClosingSummaryModel GetByPaging(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, string commaseperatedCashierIds, decimal? NetClosingFrom , decimal? NetClosingTo)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "CommaSeperatedCashierIds", Value = commaseperatedCashierIds });            
            lstParameter.Add(new NameValuePair { Name = "NetClosingFrom", Value = NetClosingFrom });
            lstParameter.Add(new NameValuePair { Name = "NetClosingTo", Value = NetClosingTo });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            CollectionClosingSummaryModel model = new RestSharpHandler().RestRequest<CollectionClosingSummaryModel>(APISelector.Municipality, true, "api/Report/CollectionClosingSummaryGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ClosingSummaryTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
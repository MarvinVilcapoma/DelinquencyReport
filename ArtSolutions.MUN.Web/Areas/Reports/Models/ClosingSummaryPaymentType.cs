using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ClosingSummaryPaymentType
    {
        #region public methods
        public ClosingSummaryPaymentTypeModel Get()
        {
            ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
            
            var paymentOptionList = new Accounts.Models.AccountModel().GetSupportValues(17).OrderBy(x => x.Name).ToList();
            paymentOptionList.Add(new SupportValueModel { Description = "", Name = Resources.Global.CreditApplied, ID = -1 });
            model.PaymentTypeList = paymentOptionList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.PaymentTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ClosingSummaryPaymentTypeModel Get(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string paymentTypeIDs)
        {
            ClosingSummaryPaymentTypeModel model = new ClosingSummaryPaymentTypeModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate, netClosingFrom, netClosingTo, paymentTypeIDs);
            model.ClosingEntryList = model.ClosingEntryList ?? new List<ClosingEntryModel>();
            // Set Payment Receipt as Child of Each Closing Receipt
            model.ClosingEntryList.ForEach(x => x.PostedPaymentReceiptList = model.PaymentList.Where(c => c.ClosingID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = startPeriodDate;
            model.EndPeriodDate = endPeriodDate;
            model.ReportCompanyModel = GetReportCompany(startPeriodDate, endPeriodDate);
            return model;
        }

        public ClosingSummaryPaymentTypeModel GetByPaging(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, decimal? netClosingFrom, decimal? netClosingTo, string commaSeperatedPaymentTypeIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedPaymentTypeIDs", Value = commaSeperatedPaymentTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "netClosingFrom", Value = netClosingFrom });
            lstParameter.Add(new NameValuePair { Name = "netClosingTo", Value = netClosingTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<ClosingSummaryPaymentTypeModel>(APISelector.Municipality, true, "api/Report/ClosingSummaryByPaymentType", "GET", lstParameter);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Global.ClosingSummaryByPaymentType, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
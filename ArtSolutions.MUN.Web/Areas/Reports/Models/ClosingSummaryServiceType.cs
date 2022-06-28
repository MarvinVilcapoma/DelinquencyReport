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
    public class ClosingSummaryServiceType
    {
        #region public methods
        public ClosingSummaryServiceTypeModel Get()
        {
            ClosingSummaryServiceTypeModel model = new ClosingSummaryServiceTypeModel();
            List<ServiceTypeModel> serviceTypeModel = new ServiceTypeModel().GetServiceType(0);
            model.ServiceTypeList = serviceTypeModel.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.ServiceTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            model.ReportCompanyModel = GetReportCompany();
            return model;
        }

        public ClosingSummaryServiceTypeModel Get(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, string serviceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            ClosingSummaryServiceTypeModel model = new ClosingSummaryServiceTypeModel();           
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate, serviceTypeIDs, netClosingFrom, netClosingTo);
            model.ClosingEntryList = model.ClosingEntryList ?? new List<ClosingEntryModel>();
            // Set Payment Receipt as Child of Each Closing Receipt
            model.ClosingEntryList.ForEach(x => x.PostedPaymentReceiptList = model.PaymentList.Where(c => c.ClosingID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = startPeriodDate;
            model.EndPeriodDate = endPeriodDate;
            model.ReportCompanyModel = GetReportCompany(startPeriodDate, endPeriodDate);
            return model;
        }

        public ClosingSummaryServiceTypeModel GetByPaging(JQDTParams jqdtParams, DateTime startPeriodDate, DateTime endPeriodDate, string commaseperatedServiceTypeIDs, decimal? netClosingFrom, decimal? netClosingTo)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceTypeIDs", Value = commaseperatedServiceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "netClosingFrom", Value = netClosingFrom });
            lstParameter.Add(new NameValuePair { Name = "netClosingTo", Value = netClosingTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            ClosingSummaryServiceTypeModel model = new RestSharpHandler().RestRequest<ClosingSummaryServiceTypeModel>(APISelector.Municipality, true, "api/Report/ClosingSummaryByServiceType", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Global.ClosingSummaryServiceTypeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }
        #endregion
    }
}
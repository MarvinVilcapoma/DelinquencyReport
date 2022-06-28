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
    public class CollectionDepositSummaryByServiceType
    {        
        #region public methods
        public CollectionDepositSummaryByServiceTypeModel Get()
        {
            CollectionDepositSummaryByServiceTypeModel model = new CollectionDepositSummaryByServiceTypeModel();
            model.ServicetypeList = HMTLHelperExtensions.CreateSelectList(new Accounts.Models.ServiceTypeModel().GetServiceType(0), "ID", "Name");
            model.ReportCompanyModel = GetReportCompany();
            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            return model;
        }

        public CollectionDepositSummaryByServiceTypeModel Get(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, string serviceTypeIDs, decimal? NetDepositFrom, decimal? NetDepositTo, string BankAccountIDs)
        {
            CollectionDepositSummaryByServiceTypeModel model = new CollectionDepositSummaryByServiceTypeModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, StartPeriodDate, EndPeriodDate, serviceTypeIDs, NetDepositFrom, NetDepositTo, BankAccountIDs);
            model.DepositEntryList = model.DepositEntryList ?? new List<DepositEntryModel>();
            model.DepositEntryList.ForEach(x => x.ClosedEntryList = model.ClosingEntryList.Where(c => c.DepositID == x.ID).OrderBy(e => e.ID).ToList());
            model.StartPeriodDate = StartPeriodDate;
            model.EndPeriodDate = EndPeriodDate;
            model.DepositEntryList.ForEach(d => d.ClosedEntryList.ForEach(k =>
            {
                k.FormatingDate = k.FormattedDate;
            }));
            model.ReportCompanyModel = GetReportCompany(StartPeriodDate, EndPeriodDate);
            return model;
        }

        public CollectionDepositSummaryByServiceTypeModel GetByPaging(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, string serviceTypeIDs, decimal? NetDepositFrom, decimal? NetDepositTo, string BankAccountIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceTypeIDs", Value = serviceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedBankAccountIDs", Value = BankAccountIDs });
            lstParameter.Add(new NameValuePair { Name = "NetDepositFrom", Value = NetDepositFrom });
            lstParameter.Add(new NameValuePair { Name = "NetDepositTo", Value = NetDepositTo });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            CollectionDepositSummaryByServiceTypeModel model = new RestSharpHandler().RestRequest<CollectionDepositSummaryByServiceTypeModel>(APISelector.Municipality, true, "api/Report/CollectionDepositSummaryByServiceTypeGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DepositSummaryServiceTypeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
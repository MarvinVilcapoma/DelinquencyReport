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
    public class CollectionDepositSummary
    {
        #region public methods
        public CollectionDepositSummaryModel Get()
        {
            CollectionDepositSummaryModel model = new CollectionDepositSummaryModel();
            model.ReportCompanyModel = GetReportCompany();

            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = new List<SelectListItemViewModel>();
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            model.BankAccountList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            return model;
        }

        public CollectionDepositSummaryModel Get(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            CollectionDepositSummaryModel model = new CollectionDepositSummaryModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, StartPeriodDate, EndPeriodDate, NetDepositFrom, NetDepositTo, bankAccountIDs);
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

        public CollectionDepositSummaryModel GetByPaging(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, decimal? NetDepositFrom, decimal? NetDepositTo, string bankAccountIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "NetDepositFrom", Value = NetDepositFrom });
            lstParameter.Add(new NameValuePair { Name = "NetDepositTo", Value = NetDepositTo });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedBankAccountIDs", Value = bankAccountIDs });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            CollectionDepositSummaryModel model = new RestSharpHandler().RestRequest<CollectionDepositSummaryModel>(APISelector.Municipality, true, "api/Report/CollectionDepositSummaryGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DepositSummaryTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
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
    public class CollectionDepositSummaryByPaymentType
    {
        #region public methods
        public CollectionDepositSummaryByPaymentTypeModel Get()
        {
            CollectionDepositSummaryByPaymentTypeModel model = new CollectionDepositSummaryByPaymentTypeModel();
            var paymentOptionList = new Accounts.Models.AccountModel().GetSupportValues(17).OrderBy(x => x.Name).ToList();
            paymentOptionList.Add(new SupportValueModel { Description = "", Name = Resources.Global.CreditApplied, ID = -1 });
            model.PaymentypeList = new SelectList(paymentOptionList, "ID", "Name");

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

        public CollectionDepositSummaryByPaymentTypeModel Get(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, string paymentTypeIDs, string bankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo)
        {
            CollectionDepositSummaryByPaymentTypeModel model = new CollectionDepositSummaryByPaymentTypeModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, StartPeriodDate, EndPeriodDate, paymentTypeIDs, bankAccountIDs, NetDepositFrom, NetDepositTo);
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

        public CollectionDepositSummaryByPaymentTypeModel GetByPaging(JQDTParams jqdtParams, DateTime StartPeriodDate, DateTime EndPeriodDate, string paymentTypeIDs, string bankAccountIDs, decimal? NetDepositFrom, decimal? NetDepositTo)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedPaymentTypeIDs", Value = paymentTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedBankAccountIDs", Value = bankAccountIDs });
            lstParameter.Add(new NameValuePair { Name = "NetDepositFrom", Value = NetDepositFrom });
            lstParameter.Add(new NameValuePair { Name = "NetDepositTo", Value = NetDepositTo });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            CollectionDepositSummaryByPaymentTypeModel model = new RestSharpHandler().RestRequest<CollectionDepositSummaryByPaymentTypeModel>(APISelector.Municipality, true, "api/Report/CollectionDepositSummaryByPaymentTypeGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DepositSummaryPaymentTypeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
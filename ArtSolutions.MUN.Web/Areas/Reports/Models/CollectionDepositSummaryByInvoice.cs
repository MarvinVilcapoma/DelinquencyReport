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
    public class CollectionDepositSummaryByInvoice
    {
        #region public methods
        public CollectionDepositSummaryByInvoiceModel Get()
        {
            CollectionDepositSummaryByInvoiceModel model = new CollectionDepositSummaryByInvoiceModel();            
            model.ReportCompanyModel = GetReportCompany();

            List<ServiceTypeModel> serviceTypeModel = new ServiceTypeModel().GetServiceType(0);
            model.ServicetypeList = serviceTypeModel.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.ServicetypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
           
            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            model.BankAccountList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            return model;
        }

        public CollectionDepositSummaryByInvoiceModel Get(JQDTParams jqdtParams, DateTime? startPeriodDate, DateTime? endPeriodDate, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string commaSeperatedBankAccountIds)
        {
            CollectionDepositSummaryByInvoiceModel model = new CollectionDepositSummaryByInvoiceModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startPeriodDate, endPeriodDate, serviceTypeIDs, netDepositFrom, netDepositTo, commaSeperatedBankAccountIds);
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

        public CollectionDepositSummaryByInvoiceModel GetByPaging(JQDTParams jqdtParams, DateTime? startPeriodDate, DateTime? endPeriodDate, string serviceTypeIDs, decimal? netDepositFrom, decimal? netDepositTo, string commaSeperatedBankAccountIds)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriodDate", Value = startPeriodDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "endPeriodDate", Value = endPeriodDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedBankAccountIds", Value = commaSeperatedBankAccountIds });
            lstParameter.Add(new NameValuePair { Name = "netDepositFrom", Value = netDepositFrom });
            lstParameter.Add(new NameValuePair { Name = "netDepositTo", Value = netDepositTo });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            CollectionDepositSummaryByInvoiceModel model = new RestSharpHandler().RestRequest<CollectionDepositSummaryByInvoiceModel>(APISelector.Municipality, true, "api/Report/CollectionDepositSummaryByInvoiceGet", "GET", lstParameter);
            return model;
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DepositSummaryReportByInvoice, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, fromDate, toDate);
        }
        #endregion
    }
}
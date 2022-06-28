using ArtSolutions.MUN.Web.Areas.Accounts.Models;
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
    public class CollectionDailyIncomeProduceBank
    {
        #region public methods
        public CollectionDailyIncomeProduceBankModel Get()
        {
            CollectionDailyIncomeProduceBankModel model = new CollectionDailyIncomeProduceBankModel();

            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.BankList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank").ToList(), "ID", "Name");
            model.ContractList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
            model.ContractList.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CollectionDailyIncomeProduceBankModel Get(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            CollectionDailyIncomeProduceBankModel model = new CollectionDailyIncomeProduceBankModel();
            model = GetByPaging(startPeriod, contractIds, bankIds, serviceIds);
            model.DailyIncomeProduceBankList = model.DailyIncomeProduceBankList ?? new List<DailyIncomeProduceBankModel>();
            model.ReportCompanyModel = GetReportCompany();
            if (!string.IsNullOrEmpty(serviceIds))
            {
                model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
                List<string> SelectedService = serviceIds.Split(',').ToList();
                model.ServicesNames = string.Join(", ", model.ServiceList.Where(d => SelectedService.FirstOrDefault(k => k == d.Value.ToString()) != null).Select(d => d.Text).ToArray());
            }
            if (!string.IsNullOrEmpty(contractIds))
            {
                model.ContractList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
                List<string> SelectedContract = contractIds.Split(',').ToList();
                model.ContractNames = string.Join(", ", model.ContractList.Where(d => SelectedContract.FirstOrDefault(k => k == d.Value.ToString()) != null).Select(d => d.Value + " = " + d.Text).ToArray());
            }
            if (!string.IsNullOrEmpty(bankIds))
            {
                model.BankList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank").ToList(), "ID", "Name");
                List<string> SelectedBank = bankIds.Split(',').ToList();
                model.BankeNames = string.Join(", ", model.BankList.Where(d => SelectedBank.FirstOrDefault(k => k == d.Value.ToString()) != null).Select(d => d.Text).ToArray());
            }
            return model;
        }
        public CollectionDailyIncomeProduceBankModel GetList(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            return Get(startPeriod, contractIds, bankIds, serviceIds);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(DateTime? startPeriod = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.DailyIncomeProduceBankTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, null, null, null, null, null, null, false);
        }
        private CollectionDailyIncomeProduceBankModel GetByPaging(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriod", Value = startPeriod.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "serviceIds", Value = serviceIds });
            lstParameter.Add(new NameValuePair { Name = "contractIds", Value = contractIds });
            lstParameter.Add(new NameValuePair { Name = "bankIds", Value = bankIds });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = int.MaxValue });
            return new RestSharpHandler().RestRequest<CollectionDailyIncomeProduceBankModel>(APISelector.Municipality, true, "api/Report/CollectionDailyIncomeProduceBankGet", "GET", lstParameter);
        }
        #endregion
    }
}
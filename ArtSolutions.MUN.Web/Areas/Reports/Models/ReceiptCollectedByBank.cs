using ArtSolutions.MUN.Web.Areas.Accounts.Models;
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
    public class ReceiptCollectedByBank
    {
        #region public methods      
        public ReceiptCollectedByBankModel Get()
        {
            ReceiptCollectedByBankModel model = new ReceiptCollectedByBankModel();
            model.BankList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank").ToList(), "ID", "Name");
            model.ContractList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
            model.ContractList.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ReceiptCollectedByBankModel Get(JQDTParams jqdtParams, DateTime? startDate, string contractIds, string bankIds, string accountIDs)
        {
            ReceiptCollectedByBankModel model = new ReceiptCollectedByBankModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startDate, contractIds, bankIds, accountIDs);
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
            model.ReportCompanyModel = GetReportCompany(startDate);
            return model;
        }
        public ReceiptCollectedByBankModel GetByPaging(JQDTParams jqdtParams, DateTime? startDate, string contractIds, string bankIds, string accountIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
                {
                    new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "accountIDs", Value = accountIDs },
                    new NameValuePair { Name = "contractIds", Value = contractIds },
                    new NameValuePair { Name = "bankIds", Value = bankIds },
                    new NameValuePair { Name = "pageIndex", Value = 0 },
                    new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
                };
            return new RestSharpHandler().RestRequest<ReceiptCollectedByBankModel>(APISelector.Municipality, true, "api/Report/ReceiptCollectedByBankGet", "GET", lstParameter);
        }
        public ReceiptCollectedByBankModel GetExportLayout(DateTime? startDate, string contractIds, string bankIds, string accountIDs)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startDate, contractIds, bankIds, accountIDs);
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> fromDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReceiptCollectedByBankTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 11, null, null, null, null, null, null, false);
        }
        #endregion
    }
}
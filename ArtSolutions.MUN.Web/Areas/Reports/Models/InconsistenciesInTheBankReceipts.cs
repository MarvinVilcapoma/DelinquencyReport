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
    public class InconsistenciesInTheBankReceipts
    {
        #region public methods      
        public InconsistenciesInTheBankReceiptsModel Get()
        {
            InconsistenciesInTheBankReceiptsModel model = new InconsistenciesInTheBankReceiptsModel();
            model.ReportCompanyModel = GetReportCompany();
            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.BankList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank").ToList(), "ID", "Name");
            model.ContractList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
            model.ContractList.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });
            return model;
        }
        public InconsistenciesInTheBankReceiptsModel Get(JQDTParams jqdtParams, DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            InconsistenciesInTheBankReceiptsModel model = new InconsistenciesInTheBankReceiptsModel();
            if (jqdtParams != null)
                model = GetByPaging(startPeriod, contractIds, bankIds, serviceIds);
            model.ReportCompanyModel = GetReportCompany();

            string contactNames = null;
            if (!string.IsNullOrEmpty(contractIds))
            {
                foreach (string items in contractIds.Split(','))
                {
                    contactNames += new AccountModel().GetSupportValues(46).Where(x => x.Description == items).Select(x => x.Name).FirstOrDefault() + ",";
                }
                model.ContractNames = contactNames.TrimEnd(',');
            }

            string bankNames = null;
            if (!string.IsNullOrEmpty(bankIds))
            {
                foreach (string items in bankIds.Split(','))
                {
                    bankNames += new AccountModel().GetSupportValues(17).Where(x => x.ID == Convert.ToInt32(items)).Select(x => x.Name).FirstOrDefault() + ",";
                }
                model.BankeNames = bankNames.TrimEnd(',');
            }

            return model;
        }
        public InconsistenciesInTheBankReceiptsModel GetByPaging(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "startPeriod", Value = startPeriod.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "serviceIds", Value = serviceIds });
            lstParameter.Add(new NameValuePair { Name = "contractIds", Value = contractIds });
            lstParameter.Add(new NameValuePair { Name = "bankIds", Value = bankIds });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<InconsistenciesInTheBankReceiptsModel>(APISelector.Municipality, true, "api/Report/InconsistenciesInTheBankReceiptsGet", "GET", lstParameter);
        }
        public InconsistenciesInTheBankReceiptsModel GetExportLayout(DateTime startPeriod, string contractIds, string bankIds, string serviceIds)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startPeriod, contractIds, bankIds, serviceIds);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ListofInconsistenciesintheApplicationofBankReceiptsReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, null, null, null, null, null, null, false);
        }
        #endregion
    }
}
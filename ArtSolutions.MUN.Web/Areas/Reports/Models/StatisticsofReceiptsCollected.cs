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
    public class StatisticsofReceiptsCollected
    {
        #region public methods      
        public StatisticsofReceiptsCollectedModel Get()
        {
            StatisticsofReceiptsCollectedModel model = new StatisticsofReceiptsCollectedModel();
            model.BankList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(17).Where(x => x.Description == "Bank").ToList(), "ID", "Name");
            model.ContractList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public StatisticsofReceiptsCollectedModel Get(JQDTParams jqdtParams, DateTime? date, int? bankId, string contract)
        {
            StatisticsofReceiptsCollectedModel model = new StatisticsofReceiptsCollectedModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, date, bankId, contract);
            model.StatisticsofReceiptsCollectedList = model.StatisticsofReceiptsCollectedList ?? new List<StatisticsofReceiptsCollectedList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public StatisticsofReceiptsCollectedModel GetByPaging(JQDTParams jqdtParams, DateTime? date, int? bankId, string contract)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
                {
                    new NameValuePair { Name = "date", Value = date == null?null: date.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "bankId", Value =bankId},
                    new NameValuePair { Name = "contract", Value =contract},
                    new NameValuePair { Name = "pageIndex", Value = 0 },
                    new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
                };
            return new RestSharpHandler().RestRequest<StatisticsofReceiptsCollectedModel>(APISelector.Municipality, true, "api/Report/StatisticsofReceiptsCollectedGet", "GET", lstParameter);
        }
        public StatisticsofReceiptsCollectedModel GetExportLayout(DateTime? date, int? bankId, string contract)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, date, bankId, contract);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> date = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.StatisticsOfReceiptsCollected, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 10, null, null, null, null, null, null, false);
        }
        #endregion

    }
}
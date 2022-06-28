using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class SummaryAccountStatement
    {
        #region public methods      
        public SummaryAccountStatementModel Get()
        {
            SummaryAccountStatementModel model = new SummaryAccountStatementModel();
            model.ReportCompanyModel = GetReportCompany();
            model.YearList = new SelectList(Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).Reverse()).ToList();
            model.YearList.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });
            return model;
        }
        public SummaryAccountStatementModel Get(JQDTParams jqdtParams, int? accountId, int? accountPropertyID, string commaSeperatedYearIDs)
        {
            SummaryAccountStatementModel model = new SummaryAccountStatementModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, accountPropertyID, commaSeperatedYearIDs);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, null).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public SummaryAccountStatementModel GetByPaging(JQDTParams jqdtParams, int? accountId, int? accountPropertyID, string commaSeperatedYearIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedYearIDs", Value = commaSeperatedYearIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = Int32.MaxValue });
            return new RestSharpHandler().RestRequest<SummaryAccountStatementModel>(APISelector.Municipality, true, "api/Report/SummaryAccountStatementGet", "GET", lstParameter);
        }
        public SummaryAccountStatementModel GetExportLayout(int? accountId, int? accountPropertyID, string commaSeperatedYearIDs)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, accountPropertyID, commaSeperatedYearIDs);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.SummaryAccountStatementTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
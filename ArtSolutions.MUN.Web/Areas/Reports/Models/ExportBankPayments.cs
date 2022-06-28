using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ExportBankPayments
    {
        #region public methods
        public ExportBankPaymentsModel Get()
        {
            ExportBankPaymentsModel model = new ExportBankPaymentsModel();
            model.ReportCompanyModel = GetReportCompany();
            model.BanacioList = new AccountModel().GetSupportValues(46).OrderBy(x => x.Description).Select(d => new SelectListItem { Text = d.Name, Value = d.Description.ToString() }).ToList();
            model.BanacioList.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });
            return model;
        }
        public ExportBankPaymentsModel Get(DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize)
        {
            ExportBankPaymentsModel model = new ExportBankPaymentsModel();
            model = GetByPaging(dueDate, commaSeperatedContractIDs, pageIndex, pageSize);
            model.ReportCompanyModel = GetReportCompany(dueDate);
            return model;
        }
        public ExportBankPaymentsModel GetExportLayout(DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize)
        {
            return Get(dueDate, commaSeperatedContractIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, Nullable<decimal> FromRange = null, Nullable<decimal> ToRange = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ExportBankPayments, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 137, FromDate, ToDate, FromRange, ToRange, true);
        }

        private ExportBankPaymentsModel GetByPaging(DateTime dueDate, string commaSeperatedContractIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
                {
                    new NameValuePair { Name = "dueDate", Value = dueDate.ToString("d", CultureInfo.InvariantCulture)  },
                    new NameValuePair { Name = "commaSeperatedContractIDs", Value = string.IsNullOrEmpty(commaSeperatedContractIDs)?null:commaSeperatedContractIDs},
                    new NameValuePair { Name = "pageIndex", Value = pageIndex },
                    new NameValuePair { Name = "pageSize", Value = pageSize  }
                };
            return new RestSharpHandler().RestRequest<ExportBankPaymentsModel>(APISelector.Municipality, true, "api/Report/ExportBankPaymentsGet", "GET", lstParameter);
        }
        #endregion
    }
}
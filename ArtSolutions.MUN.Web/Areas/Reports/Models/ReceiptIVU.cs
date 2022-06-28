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
    public class ReceiptIVU
    {
        #region public methods  
        public ReceiptIVUModel Get()
        {
            ReceiptIVUModel model = new ReceiptIVUModel();
            List<SalesCashierModel> cashierList = new SalesCashier().Get().OrderBy(x => x.UserName).ToList();
            model.CashierList = new List<SelectListItemViewModel>();
            model.CashierList = cashierList.Select(i => new SelectListItemViewModel
            {
                ID = i.UserID,
                Name = i.UserName
            }).ToList();
            model.CashierList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            model.ReportCompanyModel = GetReportCompany();

            return model;
        }

        public ReceiptIVUModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, JQDTParams jqdtParams)
        {
            ReceiptIVUModel model = new ReceiptIVUModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, jqdtParams);
            model.IVUReceiptDetailList = model.IVUReceiptDetailList ?? new List<Models.ReceiptIVUModelList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public ReceiptIVUModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, commaSeperatedAccountIds, commaSeperatedCashierIds, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReceiptIVUTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 14, FromDate, ToDate);
        }

        private ReceiptIVUModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string commaSeperatedAccountIds, string commaSeperatedCashierIds, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "commaSeperatedAccountIds", Value = commaSeperatedAccountIds },
                new NameValuePair { Name = "commaSeperatedCashierIds", Value = commaSeperatedCashierIds },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ReceiptIVUModel>(APISelector.Municipality, true, "api/Report/ReceiptByIVUGet", "GET", lstParameter);
        }
        #endregion
    }
}
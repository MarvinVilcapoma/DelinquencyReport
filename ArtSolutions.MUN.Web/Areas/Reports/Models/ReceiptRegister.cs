using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptRegister
    {
        #region public methods  
        public ReceiptRegisterModel Get()
        {
            ReceiptRegisterModel model = new ReceiptRegisterModel();

            List<SalesCashierModel> cashierList = new SalesCashier().Get().OrderBy(x => x.UserName).ToList();
            model.CashierList = new List<SelectListItemViewModel>();
            model.CashierList = cashierList.Select(i => new SelectListItemViewModel
            {
                ID = i.UserID,
                Name = i.UserName
            }).ToList();
            model.CashierList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            model.ReportCompanyModel = GetReportCompany();

            // CR Type Filter
            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Value = "i", Text = COLPayment.ByInvoice });
            typeList.Add(new SelectListItem() { Value = "o", Text = COLPayment.ByOtherService });
            typeList.Add(new SelectListItem() { Value = "2", Text = COLPayment.ByService });
            typeList.Add(new SelectListItem() { Value = "3", Text = COLPayment.ByPaymentPlan });
            typeList.Add(new SelectListItem() { Value = "4", Text = COLPayment.ByDebitNote });
            model.TypeList = typeList;
            //

            return model;
        }

        public ReceiptRegisterModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            ReceiptRegisterModel model = new ReceiptRegisterModel();
            model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
            model.ReceiptRegisterList = model.ReceiptRegisterList ?? new List<Models.ReceiptRegisterList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }
        public ReceiptRegisterModel GetForCR(DateTime? startDate, DateTime? endDate, int? paymentReceiptTypeID, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            ReceiptRegisterModel model = new ReceiptRegisterModel();
            model = GetByPagingForCR(startDate, endDate, paymentReceiptTypeID, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs);
            model.ReceiptRegisterList = model.ReceiptRegisterList ?? new List<Models.ReceiptRegisterList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate, 13);
            return model;
        }

        public ReceiptRegisterModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
        }
        public ReceiptRegisterModel GetExportLayoutForCR(DateTime? startDate, DateTime? endDate, int? paymentReceiptTypeID, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            return GetForCR(startDate, endDate, paymentReceiptTypeID, printTemplateID, balanceFrom, balanceTo, accountIDs, cashierIDs);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null, int NumberOfReportColumns = 14)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReceiptRegister, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, NumberOfReportColumns, FromDate, ToDate);
        }

        private ReceiptRegisterModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "cashierIDs", Value = cashierIDs },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ReceiptRegisterModel>(APISelector.Municipality, true, "api/Report/ReceiptRegisterGet", "GET", lstParameter);
        }
        private ReceiptRegisterModel GetByPagingForCR(DateTime? startDate, DateTime? endDate, int? paymentReceiptTypeID, int? printTemplateID, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "type", Value = paymentReceiptTypeID },
                new NameValuePair { Name = "printTemplateID", Value = printTemplateID },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "cashierIDs", Value = cashierIDs },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ReceiptRegisterModel>(APISelector.Municipality, true, "api/Report/ReceiptRegisterGetForCR", "GET", lstParameter);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptDetail
    {
        #region public methods  
        public ReceiptDetailModel Get()
        {
            ReceiptDetailModel model = new ReceiptDetailModel();
            
            List<SalesCashierModel> cashierList = new SalesCashier().Get().OrderBy(x => x.UserName).ToList();
            model.CashierList = new List<SelectListItemViewModel>();
            model.CashierList = cashierList.Select(i => new SelectListItemViewModel
            {
                ID = i.UserID,
                Name = i.UserName
            }).ToList();
            model.CashierList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            model.ReportCompanyModel = GetReportCompany();

            List<ServiceTypeModel> accountServiceTypeList = new ServiceTypeModel().GetByNoFilingType(false);
            model.AccountServiceTypeList = accountServiceTypeList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.AccountServiceTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<ServiceTypeModel> invoiceTypeList = new ServiceTypeModel().GetByNoFilingType(true);
            model.InvoiceTypeList = invoiceTypeList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.InvoiceTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<PaymentPlanModel> paymentPlanTypeList = new PaymentPlan().Get(null, null, true);
            model.PaymentPlanTypeList = paymentPlanTypeList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.PaymentPlanTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = new List<SelectListItemViewModel>();
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.AccountNumber + " - " + i.Name
            }).ToList();
            model.BankAccountList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<FinClassGrantModel> GrantList = new FinClassGrantModel().Get(null, true, null, true).OrderBy(x => x.Name).ToList();
            model.GrantList = new List<SelectListItemViewModel>();
            model.GrantList = GrantList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.GrantList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            List<FinClassGrantModel> checkBookList = new FinClassGrantModel().GetSalesCashier(null, null, true, null, null, null).OrderBy(x => x.Name).ToList();
            model.CheckbookList = new List<SelectListItemViewModel>();
            model.CheckbookList = checkBookList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            model.CheckbookList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });

            return model;
        }

        public ReceiptDetailModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs)
        {
            ReceiptDetailModel model = new ReceiptDetailModel();
            model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs);
            model.ReceiptDetailList = model.ReceiptDetailList ?? new List<Models.ReceiptDetailList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public ReceiptDetailModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReceiptDetail, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 17, FromDate, ToDate);
        }

        private ReceiptDetailModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "cashierIDs", Value = cashierIDs },
                new NameValuePair { Name = "accountServiceTypeIDs", Value = accountServiceTypeIDs },
                new NameValuePair { Name = "invoiceTypeIDs", Value = invoiceTypeIDs },
                new NameValuePair { Name = "paymentPlanTypeIDs", Value = paymentPlanTypeIDs },
                new NameValuePair { Name = "bankAccountIDs", Value = bankAccountIDs },
                new NameValuePair { Name = "grantIDs", Value = grantIDs },
                new NameValuePair { Name = "checkbookIDs", Value = checkbookIDs },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
            };
            return new RestSharpHandler().RestRequest<ReceiptDetailModel>(APISelector.Municipality, true, "api/Report/ReceiptDetailGet", "GET", lstParameter);
        }
        #endregion
    }
}
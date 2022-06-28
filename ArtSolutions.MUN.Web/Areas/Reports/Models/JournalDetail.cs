using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class JournalDetail
    {
        #region public methods
        public JournalDetailModel Get()
        {
            JournalDetailModel model = new JournalDetailModel();
            model.JournalDetailListModel = new List<Models.JournalDetailListModel>();
            model.ReportCompanyModel = GetReportCompany();
            model.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.lstDocumentType.Insert(0, new SelectListItem { Text = Resources.Global.DropDownSelectAllMessage, Value = "0" });

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

        public JournalDetailModel Get(DateTime? StartPeriodDate, DateTime? EndPeriodDate, string DocumentTypeID, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo, int? ReferrenceID)
        {
            JournalDetailModel model = new JournalDetailModel();
            model.JournalDetailListModel = GetByPaging(StartPeriodDate, EndPeriodDate, DocumentTypeID, accountIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo, ReferrenceID);
            model.ReportCompanyModel = GetReportCompany(StartPeriodDate, EndPeriodDate);
            model.JournalDetailListModel.ForEach(d => { d.Status = d.IsVoid > 0 ? Resources.Global.Voided : d.IsPost > 0 ? Resources.Global.Posted : Resources.Global.Draft; });
            return model;
        }

        public List<JournalDetailListModel> GetByPaging(DateTime? StartPeriodDate, DateTime? EndPeriodDate, string DocumentTypeID, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo, int? ReferrenceID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate?.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate?.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "DocumentTypeID", Value = DocumentTypeID });
            lstParameter.Add(new NameValuePair { Name = "accountIDs", Value = accountIDs });
            lstParameter.Add(new NameValuePair { Name = "accountServiceTypeIDs", Value = accountServiceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "invoiceTypeIDs", Value = invoiceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "paymentPlanTypeIDs", Value = paymentPlanTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "bankAccountIDs", Value = bankAccountIDs });
            lstParameter.Add(new NameValuePair { Name = "grantIDs", Value = grantIDs });
            lstParameter.Add(new NameValuePair { Name = "checkbookIDs", Value = checkbookIDs });
            lstParameter.Add(new NameValuePair { Name = "balanceFrom", Value = balanceFrom });
            lstParameter.Add(new NameValuePair { Name = "balanceTo", Value = balanceTo });
            lstParameter.Add(new NameValuePair { Name = "ReferrenceID", Value = ReferrenceID });
            List<JournalDetailListModel> model = new RestSharpHandler().RestRequest<List<JournalDetailListModel>>(APISelector.Municipality, true, "api/Report/JournalDetailGet", "GET", lstParameter);
            return model;
        }

        public JournalDetailModel GetExportLayout(DateTime? startDate, DateTime? endDate, string DocumentTypeID, string accountIDs, string accountServiceTypeIDs, string invoiceTypeIDs, string paymentPlanTypeIDs, string bankAccountIDs, string grantIDs, string checkbookIDs, decimal? balanceFrom, decimal? balanceTo)
        {
            return this.Get(startDate, endDate, DocumentTypeID, accountIDs, accountServiceTypeIDs, invoiceTypeIDs, paymentPlanTypeIDs, bankAccountIDs, grantIDs, checkbookIDs, balanceFrom, balanceTo, null);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.JournalDetailTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 17, FromDate, ToDate);
        }
        #endregion
    }
}
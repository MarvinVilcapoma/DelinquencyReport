using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptBySecurityBankAccount
    {
        #region public methods  
        public ReceiptBySecurityBankAccountModel Get()
        {
            ReceiptBySecurityBankAccountModel model = new ReceiptBySecurityBankAccountModel();
            
            List<FINBankAccountModel> bankAccountList = new FINBankAccount().Get(null, null, true);
            model.BankAccountList = new List<SelectListItemViewModel>();
            model.BankAccountList = bankAccountList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Code + " - " + i.Name
            }).ToList();
            model.BankAccountList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.DropDownSelectAllMessage });
            model.ReportCompanyModel = GetReportCompany();

            return model;
        }

        public ReceiptBySecurityBankAccountModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            ReceiptBySecurityBankAccountModel model = new ReceiptBySecurityBankAccountModel();
            model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs);
            model.SecurityBankAccountList = model.SecurityBankAccountList ?? new List<Models.ReceiptBySecurityBankAccountModelList>();
            
            var objDistinctList = model.SecurityBankAccountList.Select(d => new { BankName = d.BankName, BankAccount = d.BankAccount }).Distinct().ToList();
            model.SecurityBankAccountList = objDistinctList.Select(d => new ReceiptBySecurityBankAccountModelList
            {
                BankName = d.BankName,
                BankAccount = d.BankAccount,
                BankAccountList = model.SecurityBankAccountList.Where(k => k.BankAccount == d.BankAccount).ToList()
            }).ToList();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public ReceiptBySecurityBankAccountModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, bankAccountIDs);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.SecurityDetailBankAccountTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 17, FromDate, ToDate);
        }

        private ReceiptBySecurityBankAccountModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string bankAccountIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "bankAccountIDs", Value = bankAccountIDs },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ReceiptBySecurityBankAccountModel>(APISelector.Municipality, true, "api/Report/ReceiptBySecurityBankAccountGet", "GET", lstParameter);
        }
        #endregion
    }
}
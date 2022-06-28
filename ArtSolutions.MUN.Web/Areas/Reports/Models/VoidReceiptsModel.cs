using ArtSolutions.MUN.Web.Areas.Accounts.Models;
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
    public class VoidReceiptsModel
    {
        #region public properties
        public List<ReceiptEntryModel> ReceiptEntryList { get; set; }
        public Int32 TotalRecord { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BalanceFrom { get; set; }
        public decimal BalanceTo { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public Guid[] CashierIDs { get; set; }
        public string FilterCashierID { get; set; }
        public Guid CashierID { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public List<SelectListItemViewModel> CashierList { get; set; }
        public AccountModel AccountModel { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion

        #region Constructor
        public VoidReceiptsModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion

        #region public methods       
        public VoidReceiptsModel Get()
        {
            VoidReceiptsModel model = new VoidReceiptsModel();

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

        public VoidReceiptsModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, JQDTParams jqdtParams)
        {
            VoidReceiptsModel model = new VoidReceiptsModel();
            if (jqdtParams != null)
                model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, jqdtParams);
            model.ReceiptEntryList = model.ReceiptEntryList ?? new List<Models.ReceiptEntryModel>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        private VoidReceiptsModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs, JQDTParams jqdtParams)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "StartDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "EndDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "BalanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "BalanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "cashierIDs", Value = cashierIDs },
                new NameValuePair { Name = "pageIndex", Value = 0  },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<VoidReceiptsModel>(APISelector.Municipality, true, "api/Report/ReceiptVoidGet", "GET", lstParameter);
        }

        public VoidReceiptsModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            return new VoidReceiptsModel().Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            });
        }

        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.VoidReceipt, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, FromDate, ToDate);
        }
        #endregion
    }

    public class ReceiptEntryModel
    {
        #region public properties
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string AccountDisplayName { get; set; }
        public Nullable<DateTime> VoidDate { get; set; }
        public string VoidReason { get; set; }
        public decimal TotalPayment { get; set; }
        #endregion

        #region custom properties
        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("d");
            }
        }
        public string FormattedVoidDate
        {
            get
            {
                return this.VoidDate.HasValue ? this.VoidDate.Value.ToString("d") : "";
            }
        }
        public string FormattedTotalPayment
        {
            get
            {

                return this.TotalPayment > 0 ? this.TotalPayment.ToString("C") : "";
            }
        }
        #endregion
    }
}
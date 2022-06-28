using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ReceiptCommercialFacilitiesRental
    {
        #region public methods  
        public ReceiptCommercialFacilitiesRentalModel Get()
        {
            ReceiptCommercialFacilitiesRentalModel model = new ReceiptCommercialFacilitiesRentalModel();

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
                
        public ReceiptCommercialFacilitiesRentalModel Get(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            ReceiptCommercialFacilitiesRentalModel model = new ReceiptCommercialFacilitiesRentalModel();
            model = GetByPaging(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
            model.ReceiptCommercialFacilitiesRentalList = model.ReceiptCommercialFacilitiesRentalList ?? new List<Models.ReceiptCommercialFacilitiesRentalList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }

        public ReceiptCommercialFacilitiesRentalModel GetExportLayout(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            return Get(startDate, endDate, balanceFrom, balanceTo, accountIDs, cashierIDs);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ReceiptCommercialFacilitiesRental, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }

        private ReceiptCommercialFacilitiesRentalModel GetByPaging(DateTime? startDate, DateTime? endDate, decimal? balanceFrom, decimal? balanceTo, string accountIDs, string cashierIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "balanceFrom", Value = balanceFrom },
                new NameValuePair { Name = "balanceTo", Value = balanceTo },
                new NameValuePair { Name = "accountIDs", Value = accountIDs },
                new NameValuePair { Name = "cashierIDs", Value = cashierIDs },
                new NameValuePair { Name = "pageIndex", Value = 0  },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<ReceiptCommercialFacilitiesRentalModel>(APISelector.Municipality, true, "api/Report/ReceiptCommercialFacilitiesRentalGet", "GET", lstParameter);
        }
        #endregion
    }
}
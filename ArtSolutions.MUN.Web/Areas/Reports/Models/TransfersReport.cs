using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class TransfersReport
    {
        #region public methods  
        public TransfersReportModel Get()
        {
            TransfersReportModel model = new TransfersReportModel();
            model.ReportCompanyModel = GetReportCompany();

            List<SupportValueModel> transferTypeList = new AccountModel().GetSupportValues(42);
            model.TransferTypeList = transferTypeList.Select(i => new SelectListItemViewModel
            {
                ID = i.ID,
                Name = i.Name
            }).ToList();
            model.TransferTypeList.Insert(0, new SelectListItemViewModel { ID = 0, Name = Resources.Global.All });

            return model;
        }
        public TransfersReportModel Get(DateTime? fromDate, DateTime? toDate, int? transferTypeID)
        {
            TransfersReportModel model = new TransfersReportModel();
            model = GetByPaging(fromDate, toDate, transferTypeID);
            model.TransfersReportList = model.TransfersReportList ?? new List<TransfersReportList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate);
            return model;
        }
        public TransfersReportModel GetExportLayout(DateTime? fromDate, DateTime? toDate, int? transferTypeID)
        {
            return Get(fromDate, toDate, transferTypeID);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.TransfersReportTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 12, FromDate, ToDate, null, null, null, null, false);
        }
        private TransfersReportModel GetByPaging(DateTime? fromDate, DateTime? toDate, int? transferTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
               new NameValuePair { Name = "fromDate", Value = (fromDate == null?null: fromDate.Value.ToString("d", CultureInfo.InvariantCulture)) },
                new NameValuePair { Name = "toDate", Value = (toDate==null?null:toDate.Value.ToString("d", CultureInfo.InvariantCulture))},
                new NameValuePair { Name = "transferTypeID", Value = (transferTypeID==0?null:transferTypeID) },
                new NameValuePair { Name = "pageIndex", Value = 0 },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue  }
            };
            return new RestSharpHandler().RestRequest<TransfersReportModel>(APISelector.Municipality, true, "api/Report/TransfersReport", "GET", lstParameter);
        }
        #endregion
    }
}
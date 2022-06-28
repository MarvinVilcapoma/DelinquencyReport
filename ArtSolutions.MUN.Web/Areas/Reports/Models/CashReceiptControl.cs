using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CashReceiptControl
    {
        #region public methods      
        public CashReceiptControlModel Get()
        {
            CashReceiptControlModel model = new CashReceiptControlModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public CashReceiptControlModel Get(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            CashReceiptControlModel model = new CashReceiptControlModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startDate, endDate);
            model.CashReceiptControlList = model.CashReceiptControlList ?? new List<CashReceiptControlList>();
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }
        public CashReceiptControlModel GetByPaging(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
                {
                    new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "pageIndex", Value = 0 },
                    new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
                };
            return new RestSharpHandler().RestRequest<CashReceiptControlModel>(APISelector.Municipality, true, "api/Report/CashReceiptControlGet", "GET", lstParameter);
        }
        public CashReceiptControlModel GetExportLayout(DateTime? startDate, DateTime? endDate)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startDate, endDate);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.CashReceiptControl, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 14, FromDate, ToDate, null, null, null, null, false);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class PaymentsMadeByTheTaxpayer
    {
        #region public methods      
        public PaymentsMadeByTheTaxpayerModel Get()
        {
            PaymentsMadeByTheTaxpayerModel model = new PaymentsMadeByTheTaxpayerModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public PaymentsMadeByTheTaxpayerModel Get(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, string accountIDs)
        {
            PaymentsMadeByTheTaxpayerModel model = new PaymentsMadeByTheTaxpayerModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, startDate, endDate, accountIDs);
            model.ReportCompanyModel = GetReportCompany(startDate, endDate);
            return model;
        }
        public PaymentsMadeByTheTaxpayerModel GetByPaging(JQDTParams jqdtParams, DateTime? startDate, DateTime? endDate, string accountIDs)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
                {
                    new NameValuePair { Name = "startDate", Value = startDate == null?null: startDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "endDate", Value =endDate==null?null:endDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                    new NameValuePair { Name = "accountIDs", Value = accountIDs },
                    new NameValuePair { Name = "pageIndex", Value = 0 },
                    new NameValuePair { Name = "pageSize", Value = Int32.MaxValue }
                };
            return new RestSharpHandler().RestRequest<PaymentsMadeByTheTaxpayerModel>(APISelector.Municipality, true, "api/Report/PaymentsMadeByTheTaxpayerGet", "GET", lstParameter);
        }
        public PaymentsMadeByTheTaxpayerModel GetExportLayout(DateTime? startDate, DateTime? endDate, string accountIDs)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, startDate, endDate, accountIDs);
        }
        #endregion

        #region private methods  
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ListOfPaymentsMadeByTheTaxpayer, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 11, fromDate, toDate, null, null, null, null, false);
        }
        #endregion
    }
}
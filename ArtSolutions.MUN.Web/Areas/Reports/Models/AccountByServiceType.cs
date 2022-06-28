using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountByServiceType
    {
        
        #region public methods  
        public AccountByServiceTypeModel Get()
        {
            AccountByServiceTypeModel model = new AccountByServiceTypeModel();
            model.ReportCompanyModel = GetReportCompany();
            model.lstServiceType = new ServiceTypeModelReport().GetServiceType(0).OrderBy(x => x.Name).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return model;
        }

        public AccountByServiceTypeModel GetExportLayout(int serviceTypeID, bool isNotAssignServices)
        {
            AccountByServiceTypeModel model = new AccountByServiceTypeModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceTypeID", Value = serviceTypeID });
            lstParameter.Add(new NameValuePair { Name = "isNotAssignServices", Value = isNotAssignServices });
            model = new RestSharpHandler().RestRequest<AccountByServiceTypeModel>(APISelector.Municipality, true, "api/Report/AccountByServiceType", "GET", lstParameter);
            model.ReportCompanyModel = GetReportCompany();
            model.ServiceTypeLabel = new ServiceTypeModelReport().GetServiceType(0).Where(a=>a.ID==serviceTypeID).First().Name;
            return model;
        }

        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AccountByServiceTypeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, null, null, null, null, null, null, false);

        }
        #endregion
    }
}
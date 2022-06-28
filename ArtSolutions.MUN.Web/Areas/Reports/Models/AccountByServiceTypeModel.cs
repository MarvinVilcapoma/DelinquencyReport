using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AccountByServiceTypeModel : DataExportModel
    {
        #region public properties     
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int ServiceTypeID { get; set; }
        public List<SelectListItem> lstServiceType { get; set; }
        public List<AccountByServiceTypeList> AccountByServiceTypeList { get; set; }
        public string ServiceTypeLabel { get; set; }
        public bool isNotAssignServices { get; set; }
        #endregion
        #region constructor
        public AccountByServiceTypeModel()
        {
            this.AccountByServiceTypeList = new List<AccountByServiceTypeList>();
        }
        #endregion
    }
    public class ServiceTypeModelReport
    {
        #region public properties       
        public int ID { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        #endregion

        public List<ServiceTypeModelReport> GetServiceType(int groupId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "GroupID", Value = groupId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = DBNull.Value });
            return new RestSharpHandler().RestRequest<List<ServiceTypeModelReport>>(APISelector.Municipality, true, "api/AccountService/ServiceTypeGet", "GET", lstParameter);
        }
    }

    public class AccountByServiceTypeList
    {
        public string ServiceClass { get; set; }
        public string IDNumber { get; set; }
        public string DisplayName { get; set; }
        public string RightNumber { get; set; }
        public int Year { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
    }
}
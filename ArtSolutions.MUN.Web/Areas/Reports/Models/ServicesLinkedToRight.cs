using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ServicesLinkedToRight
    {
        #region public methods  
        public ServicesLinkedToRightModel Get()
        {
            ServicesLinkedToRightModel model = new ServicesLinkedToRightModel();

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Value = "1", Text = Resources.Report.ServiceWithoutRights });
            typeList.Add(new SelectListItem() { Value = "2", Text = Resources.Report.ServiceWithRights });
            typeList.Add(new SelectListItem() { Value = "3", Text = Resources.Report.CedulaWithoutRight });
            model.TypeList = typeList;

            model.ServiceList = HMTLHelperExtensions.CreateSelectList(new Services.Models.Service().Get(null, null, null, false, null)
            .Where(x => x.ServiceTypeID == 1 || x.ServiceTypeID == 4 || x.ServiceTypeID == 11 || x.ServiceTypeID == 16 || x.ServiceTypeID == 17 ||
            x.ServiceTypeID == 18 || x.ServiceTypeID == 19 || x.ServiceTypeID == 24 || x.ServiceTypeID == 25 || x.ServiceTypeID == 26 || x.ServiceTypeID==27)
            .ToList(), "ID", "Name");

            model.ServiceTypeList = HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetServiceType(0)
            .Where(x => x.ID == 1 || x.ID == 4 || x.ID == 11 || x.ID == 16 || x.ID == 17 || x.ID == 18 || x.ID == 19 ||
            x.ID == 24 || x.ID == 25 || x.ID == 26 || x.ID==27)
            .ToList(), "ID", "Name");

            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ServicesLinkedToRightModel Get(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            ServicesLinkedToRightModel model = new ServicesLinkedToRightModel();
            model = GetByPaging(typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, pageIndex, pageSize);
            model.ServicesLinkedToRightList = model.ServicesLinkedToRightList ?? new List<Models.ServicesLinkedToRightList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public ServicesLinkedToRightModel GetList(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            return Get(typeID, commaSeperatedServiceTypeIDs, commaSeperatedServiceIDs, commaSeperatedAccountIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.ServicesLinkedToRightTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 5, null, null, null, null, null, null, false);
        }
        private ServicesLinkedToRightModel GetByPaging(int? typeID, string commaSeperatedServiceTypeIDs, string commaSeperatedServiceIDs, string commaSeperatedAccountIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "typeID", Value = typeID });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceTypeIDs", Value = commaSeperatedServiceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceIDs", Value = commaSeperatedServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedAccountIDs", Value = commaSeperatedAccountIDs });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<ServicesLinkedToRightModel>(APISelector.Municipality, true, "api/Report/ServicesLinkedToRightGet", "GET", lstParameter);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MigrationValidationCobros
    {
        #region public methods  
        public MigrationValidationCobrosModel Get()
        {
            MigrationValidationCobrosModel model = new MigrationValidationCobrosModel();

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Value = "1", Text = Resources.Report.Different });
            typeList.Add(new SelectListItem() { Value = "2", Text = Resources.Report.Identical });
            typeList.Add(new SelectListItem() { Value = "3", Text = Resources.Report.MigrationDataInSIMNotInRULE });
            model.TypeList = typeList;

            model.ServiceList = HMTLHelperExtensions.CreateSelectList(new Services.Models.Service().GetForCobros(), "Code", "Name");

            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public MigrationValidationCobrosModel Get(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            MigrationValidationCobrosModel model = new MigrationValidationCobrosModel();
            model = GetByPaging(typeID, serviceCodes, accountIDs, pageIndex, pageSize);
            model.MigrationValidationCobrosList = model.MigrationValidationCobrosList ?? new List<Models.MigrationValidationCobrosList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public MigrationValidationCobrosModel GetList(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            return Get(typeID, serviceCodes, accountIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.MigrationValidationCobrosTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, null, null, null, null, null, null, false);
        }
        private MigrationValidationCobrosModel GetByPaging(int? typeID, string serviceCodes, string accountIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "typeID", Value = typeID });
            lstParameter.Add(new NameValuePair { Name = "serviceCodes", Value = serviceCodes });
            lstParameter.Add(new NameValuePair { Name = "accountIDs", Value = accountIDs });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<MigrationValidationCobrosModel>(APISelector.Municipality, true, "api/Report/MigrationValidationCobrosGet", "GET", lstParameter);
        }
        #endregion
    }
}
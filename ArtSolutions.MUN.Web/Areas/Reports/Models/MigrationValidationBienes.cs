using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MigrationValidationBienes
    {
        #region public methods  
        public MigrationValidationBienesModel Get()
        {
            MigrationValidationBienesModel model = new MigrationValidationBienesModel();

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Value = "1", Text = Resources.Report.Different });
            typeList.Add(new SelectListItem() { Value = "2", Text = Resources.Report.Identical });
            typeList.Add(new SelectListItem() { Value = "3", Text = Resources.Report.MigrationDataInSIMNotInRULE });
            model.TypeList = typeList;

            model.PropertyList = new List<SelectListItem>();

            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public MigrationValidationBienesModel Get(int? typeID, string FincaIDs, string accountIDs, int pageIndex, int pageSize)
        {
            MigrationValidationBienesModel model = new MigrationValidationBienesModel();
            model = GetByPaging(typeID, FincaIDs, accountIDs, pageIndex, pageSize);
            model.MigrationValidationBienesList = model.MigrationValidationBienesList ?? new List<Models.MigrationValidationBienesList>();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public MigrationValidationBienesModel GetList(int? typeID, string FincaIDs, string accountIDs, int pageIndex, int pageSize)
        {
            return Get(typeID, FincaIDs, accountIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany()
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.MigrationValidationBienesTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 5, null, null, null, null, null, null, false);
        }
        private MigrationValidationBienesModel GetByPaging(int? typeID, string FincaIDs, string accountIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "typeID", Value = typeID });
            lstParameter.Add(new NameValuePair { Name = "fincaIDs", Value = FincaIDs });
            lstParameter.Add(new NameValuePair { Name = "accountIDs", Value = accountIDs });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<MigrationValidationBienesModel>(APISelector.Municipality, true, "api/Report/MigrationValidationBienesGet", "GET", lstParameter);
        }
        #endregion
    }
}
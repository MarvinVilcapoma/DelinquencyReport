using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class NoDebtCertification
    {
        #region public methods  
        
        public NoDebtCertificationModel Get()
        {
            NoDebtCertificationModel model = new NoDebtCertificationModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public NoDebtCertificationModel Get(JQDTParams jqdtParams, int accountId, string note,string observation)
        {
            NoDebtCertificationModel model = new NoDebtCertificationModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, note);
            model.ReportCompanyModel = GetReportCompany();
            model.Note = note;
            model.Observations = observation;
            return model;
        }
        public NoDebtCertificationModel GetByPaging(JQDTParams jqdtParams, int accountId, string note)
        {
            NoDebtCertificationModel model = new NoDebtCertificationModel();
            model.AccountId = accountId;           
            model.PageIndex = 0;
            model.PageSize = Int32.MaxValue;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<NoDebtCertificationModel>(APISelector.Municipality, true, "api/Report/NoDebtCertification", "POST", null, lstObjParameter);
        }
        public NoDebtCertificationModel GetExportLayout(int accountId, string note,string observation="")
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, note, observation);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.NoDebtCertification, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 3, FromDate, ToDate);
        }
        #endregion
    }
}
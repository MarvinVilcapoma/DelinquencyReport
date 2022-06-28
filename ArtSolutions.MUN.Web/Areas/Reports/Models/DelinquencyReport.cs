using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class DelinquencyReport
    {
        #region public methods  

        public DelinquencyReportModel Get()
        {
            DelinquencyReportModel model = new DelinquencyReportModel();
            //model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            return model;
        }
        

        #endregion

    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class Journal
    {
        #region public methods
        public JournalModel Get()
        {
            JournalModel model = new JournalModel();
            model.JournalListModel = new List<Models.JournalListModel>();
            model.ReportCompanyModel = GetReportCompany();
            model.lstDocumentType = new DocumentTypeModel().Get(0, true).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return model;
        }

        public List<JournalListModel> Get(DateTime StartPeriodDate, DateTime EndPeriodDate, string DocumentTypeID)
        {
            List<JournalListModel> model = new List<JournalListModel>();
            model = GetByPaging(StartPeriodDate, EndPeriodDate, DocumentTypeID);
            return model;
        }

        public List<JournalListModel> GetByPaging(DateTime StartPeriodDate, DateTime EndPeriodDate, string DocumentTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = StartPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = EndPeriodDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "DocumentTypeID", Value = DocumentTypeID });
            List<JournalListModel> model = new RestSharpHandler().RestRequest<List<JournalListModel>>(APISelector.Municipality, true, "api/Report/JournalGet", "GET", lstParameter);
            model.ForEach(d => { d.Status = d.IsVoid > 0 ? Resources.Global.Voided : d.IsPost > 0 ? Resources.Global.Posted : Resources.Global.Draft; });
            return model;
        }

        public List<JournalListModel> GetExportLayout(DateTime? startDate, DateTime? endDate, string DocumentTypeID)
        {
            return this.Get(startDate.Value, endDate.Value, DocumentTypeID);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.JournalTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 9, FromDate, ToDate);
        }
        #endregion
    }
}
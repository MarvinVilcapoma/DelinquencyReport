using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class JournalModel
    {
        #region public properties
        public List<JournalListModel> JournalListModel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DocumentTypeIDs { get; set; }
        public string FilterDocumentTypeIDs { get; set; }
        public List<SelectListItem> lstDocumentType { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
    public class JournalListModel
    {
        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public string AccountName { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public int IsPost { get; set; }
        public int IsVoid { get; set; }
        public string Status { get; set; }
        #endregion
    }
}
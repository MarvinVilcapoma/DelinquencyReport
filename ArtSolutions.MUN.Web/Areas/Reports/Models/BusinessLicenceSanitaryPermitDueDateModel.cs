using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenceSanitaryPermitDueDateModel : DataExportModel
    {
        #region public properties     
        public int? AccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<BusinessLicenceSanitaryPermitDueDateList> BusinessLicenceSanitaryPermitDueDateList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenceSanitaryPermitDueDateModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class BusinessLicenceSanitaryPermitDueDateList
    {
        #region public properties
        public string ServiceName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DueDate { get; set; }
        public int Days { get; set; }
        public string PatentNumber { get; set; }
        public string PermisoSanitario { get; set; }
        #endregion

        #region custom properties 
        public string FormattedDueDate
        {
            get
            {
                return DueDate.ToString("d");
            }
        }
        #endregion
    }
}
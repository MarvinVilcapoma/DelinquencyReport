using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ServicesLinkedToRightModel : DataExportModel
    {
        #region public properties      
        public List<ServicesLinkedToRightList> ServicesLinkedToRightList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public int? TypeID { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public string FilterAccountID { get; set; }
        public List<SelectListItem> ServiceTypeList { get; set; }
        public string[] ServiceTypeIDs { get; set; }
        public string FilterServiceTypeID { get; set; }
        public List<SelectListItem> ServiceList { get; set; }
        public string[] ServiceIDs { get; set; }
        public string FilterServiceID { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public ServicesLinkedToRightModel()
        {
            this.TypeList = new List<SelectListItem>();
            this.AccountList = new List<SelectListItemViewModel>();
            this.ServiceTypeList = new List<SelectListItem>();
            this.ServiceList = new List<SelectListItem>();
        }
        #endregion
    }
    public class ServicesLinkedToRightList
    {
        #region public properties   
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public string ServiceName { get; set; }
        public string RightNumber { get; set; }
        public Nullable<int> AccountID { get; set; }
        public int AccountTypeID { get; set; }
        #endregion
    }
}
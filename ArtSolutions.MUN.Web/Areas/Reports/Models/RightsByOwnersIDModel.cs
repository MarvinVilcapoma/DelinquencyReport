using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class RightsByOwnersIDModel : DataExportModel
    {
        #region public properties     
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int OwnerID { get; set; }
        public List<SelectListItemViewModel> OwnerList { get; set; }
        public string[] OwnerIDs { get; set; }
        public AccountModel AccountModel { get; set; }
        public List<RightsByOwnersIDList> RightsByOwnersIDList { get; set; }
        public RightsByOwnersIDDetailModel RightsByOwnersIDDetailModel { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public Int32 TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        #endregion

        #region Constructor
        public RightsByOwnersIDModel()
        {
            this.OwnerList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class RightsByOwnersIDList
    {
        #region public properties
        public string PropertyCode { get; set; }
        public string PropertyNumber { get; set; }
        public string RightNumber { get; set; }
        public decimal? TotalArea { get; set; }
        public decimal? CurrentValue { get; set; }
        public string CurrentCode { get; set; }
        public DateTime? CurrentFecha { get; set; }
        public string CurrentNumber { get; set; }
        public decimal? PreviousValue { get; set; }
        public string PreviousCode { get; set; }
        public DateTime? PreviousFecha { get; set; }
        public string PreviousNumber { get; set; }
        public string ExonerationCode { get; set; }
        public string ExonerationType { get; set; }
        public decimal? ExonerationPercent { get; set; }
        #endregion

        #region custom properties  
        #endregion
    }
    public class RightsByOwnersIDDetailModel
    {
        #region public properties
        public int TotalFincas { get; set; }
        public int TotalOwner { get; set; }
        #endregion
    }
}
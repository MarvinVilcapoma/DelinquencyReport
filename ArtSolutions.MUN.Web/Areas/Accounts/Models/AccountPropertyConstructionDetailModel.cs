using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPropertyConstructionDetailModel
    {
        #region public properties       
        public int ID { get; set; }
        public int PropertyAccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int MaterialTypeID { get; set; }
        public int? ConstructionTypeID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> ConstructionYear { get; set; }
        public Nullable<int> TotalYears { get; set; }
        public int? StatusID { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "ValidFloorsValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? Floors { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "ValidChambersValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? Chambers { get; set; }

        public int? InternalWallsID { get; set; }
        public int? ExternalWallsID { get; set; }
        public int? StructureID { get; set; }
        public int? RoofID { get; set; }
        public int? CeilingID { get; set; }
        public int? FloorID { get; set; }
        public decimal? Bathroom { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal? BuildingArea { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> UnitValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> TotalValue { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public List<SelectListItem> MaterialTypeList { get; set; }
        public List<SelectListItem> ConstructionTypeList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> FloorsList { get; set; }
        public List<SelectListItem> StructureList { get; set; }
        public List<SelectListItem> RoofList { get; set; }
        public List<SelectListItem> CeilingList { get; set; }
        public List<SelectListItem> InternalWallsList { get; set; }
        public List<SelectListItem> ExternalWallsList { get; set; }
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public string MaterialType { get; set; }
        public string ConstructionType { get; set; }
        public string Status { get; set; }
        public string Structure { get; set; }
        public string Roof { get; set; }
        public string Ceiling { get; set; }
        public string Floor { get; set; }
        public string InternalWalls { get; set; }
        public string ExternalWalls { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDistributed { get; set; }
        public string UsefulLife { get; set; }
        #endregion
    }
}
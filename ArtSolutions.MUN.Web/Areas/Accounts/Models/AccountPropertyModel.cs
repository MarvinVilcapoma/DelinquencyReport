using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPropertyModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string PropertyNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string RigthNumber { get; set; }
        public Nullable<int> CondoID { get; set; }
        public Nullable<int> CondoTypeID { get; set; }
        public string CadastralPlanNumber { get; set; }
        public string Notes { get; set; }
        public int? MainAccessID { get; set; }
        public Nullable<decimal> PropertyFront { get; set; }
        public Nullable<decimal> PropertyLength { get; set; }
        public int? SlopeID { get; set; }
        public Nullable<decimal> Level { get; set; }
        public int? LevelTypeID { get; set; }
        public int? BlockLocationID { get; set; }
        public int? RegularID { get; set; }
        public int? HydrologyID { get; set; }
        public int? UseOfLandID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int OwnerID { get; set; }

        [Range(1, 100, ErrorMessageResourceName = "ValidPercentageValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> Percentage { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> Area { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "TerrainValueRangeValidation", ErrorMessageResourceType = typeof(Resources.AccountProperty))]
        public Nullable<decimal> TerrainValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> TotalTerrainValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> ContructionValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> ComplementaryValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<decimal> TotalValue { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string CadastralLocationPath { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public List<AccountPropertyConstructionDetailModel> AccountPropertyConstructionDetail { get; set; }
        public string AccountPropertyConstructionDetailJson { get; set; }
        public List<SelectListItem> CondoList { get; set; }
        public List<SelectListItem> CondoTypeList { get; set; }
        public List<SelectListItem> SlopeList { get; set; }
        public List<SelectListItem> MainAccessList { get; set; }
        public List<SelectListItem> LevelTypeList { get; set; }
        public List<SelectListItem> BlockLocationList { get; set; }
        public List<SelectListItem> RegularList { get; set; }
        public List<SelectListItem> HydrologyList { get; set; }
        public List<SelectListItem> UseOfLandList { get; set; }
        public List<SelectListItem> HomogeneousZoneList { get; set; }
        public List<SelectListItem> MovementList { get; set; }
        public List<SelectListItem> SplitTypeList { get; set; }
        public string RegisterNumber { get; set; }
        public string Condo { get; set; }
        public string CondoType { get; set; }
        public string MainAccess { get; set; }
        public string Slope { get; set; }
        public string LevelType { get; set; }
        public string BlockLocation { get; set; }
        public string Regular { get; set; }
        public string Hydrology { get; set; }
        public string UseOfLand { get; set; }
        public string HomogeneousZone { get; set; }
        public string Movement { get; set; }
        public string Owner { get; set; }
        public decimal ConstructionArea { get; set; }
        public decimal ComplementaryArea { get; set; }
        public List<SelectListItem> PropertyServicesList1 { get; set; }
        public List<SelectListItem> PropertyServicesList2 { get; set; }
        public List<AccountPropertyModel> AccountPropertyRightsList { get; set; }
        public byte[] AccountRowVersion { get; set; }
        public Nullable<int> FillingPropertyTaxID { get; set; }
        public DateTime FillingDate { get; set; }
        public DateTime DueDate { get; set; }
        public int AccountID { get; set; }
        public string FincaID { get; set; }
        public string ServicesList1 { get; set; }
        public string ServicesList2 { get; set; }
        public string OldFincaID { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "ValidRegularFactorValidationMsg", ErrorMessageResourceType = typeof(Resources.AccountProperty))]
        public Nullable<decimal> RegularFactor { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "ValidSlopeValidationMsg", ErrorMessageResourceType = typeof(Resources.AccountProperty))]
        public Nullable<decimal> SlopePercentage { get; set; }
        public List<SelectListItem> AccountList { get; set; }
        public List<SelectListItem> AccountPropertyList { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int OldAccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int AccountPropertyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int SplitTypeID { get; set; }
        public string ServiceIDs { set; get; }
        public string RigthCodes { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime SplitDate { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime MergetDate { get; set; }
        public string PropertyIDs { get; set; }
        public string AccountServiceIDs { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<decimal> HdnArea { get; set; }
        public int workflowID { get; set; }
        public int WorkflowVerionID { get; set; }
        public int workflowStatusID { get; set; }
        public Guid AssignToID { get; set; } = Guid.Empty;
        public int ReasonID { get; set; }
        public string Comments { get; set; }
        public string Property { get; set; }

        public string AssignToUserEmail { get; set; }
        public string AssignToName { get; set; }
        public decimal CurrentValueFromPropertyTax { get; set; }
        public decimal NewCurrentValue { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int MovementTypeID { get; set; }

        #region Proeprty Address        
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipPostalCode { get; set; }
        public int? CountryID { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public string CountryName { get; set; }
        public int? CountryStateID { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public string StateName { get; set; }
        public int? CityID { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public string CityName { get; set; }
        public int? TownID { get; set; }
        public List<SelectListItem> TownList { get; set; }
        public string TownName { get; set; }
        public AddressModel Address { get; set; }
        public Nullable<int> HomogeneousZoneID { get; set; }
        public Nullable<int> HomogeneousZoneTableID { get; set; }
        public Nullable<int> MovementID { get; set; }
        public Nullable<int> MovementTableID { get; set; }
        public string Number { get; set; }
        public Nullable<System.DateTime> DateOfMovement { get; set; }
        public Nullable<int> MaterialTypeID { get; set; }
        public Nullable<int> ConstructionID { get; set; }
        public string North { get; set; }
        public string South { get; set; }
        public string East { get; set; }
        public string West { get; set; }
        #endregion

        #endregion

        #region custom properties 
        public string FormattedContructionValue
        {
            get
            {
                return this.ContructionValue != null ? this.ContructionValue.Value.ToString("C") : null;
            }
        }
        public string FormattedTotalTerrainValue
        {
            get
            {
                return this.TotalTerrainValue != null ? this.TotalTerrainValue.Value.ToString("C") : null;
            }
        }
        public string FormattedTotalValue
        {
            get
            {
                return this.TotalValue != null ? this.TotalValue.Value.ToString("C") : null;
            }
        }
        public string FormattedArea
        {
            get
            {
                return this.Area != null ? this.Area.Value.ToString(Common.DecimalPoints) : null;
            }
        }

        public AccountModel AccountDetail { get; set; }
        #endregion
        public AccountPropertyModel()
        {
            AccountList = new List<SelectListItem>();
            AccountPropertyList = new List<SelectListItem>();
        }
    }
    public class AccountPropertyModelDetail
    {
        #region public properties       
        public List<AccountPropertyModel> AccountPropertyModelList { get; set; }
        public List<AccountPropertyConstructionDetailModel> AccountPropertyConstructionModelList { get; set; }
        #endregion
    }

    public class AccountPropertySearchModel
    {
        #region public properties       
        public int id { get; set; }
        public string text { get; set; }
        #endregion
    }

    public class MUNAccountPropertyRightHistoryModel
    {
        public string OldFincaID { get; set; }
        public string NewFincaID { get; set; }
        public string OldRightNumber { get; set; }
        public string NewRightNumber { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy
        {
            get
            {
                return new UserProfile().UserProfileGet(new UserProfileViewModel
                {
                    UserID = this.CreatedUserID
                }).FullName;
            }
        }
    }
}
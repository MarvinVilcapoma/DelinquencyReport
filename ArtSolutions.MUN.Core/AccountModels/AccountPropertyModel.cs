using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.AccountModels
{
    #region Account Property
    public class AccountPropertyModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string PropertyNumber { get; set; } // Required
        public string RigthNumber { get; set; }  // Required
        public Nullable<int> CondoID { get; set; }
        public Nullable<int> CondoTypeID { get; set; }
        public string CadastralPlanNumber { get; set; }
        //public decimal? TotalArea { get; set; }
        public string Notes { get; set; }
        public int? MainAccessID { get; set; }
        public decimal? PropertyFront { get; set; }
        public decimal? PropertyLength { get; set; }
        public int? SlopeID { get; set; }
        public decimal? Level { get; set; }
        public int? LevelTypeID { get; set; }
        public int? BlockLocationID { get; set; }
        public int? RegularID { get; set; }
        public int? HydrologyID { get; set; }
        public int? UseOfLandID { get; set; }
        public int OwnerID { get; set; } // Required
        public decimal? Percentage { get; set; }
        public decimal Area { get; set; } // Required
        public decimal? TerrainValue { get; set; }
        public decimal? TotalTerrainValue { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string CadastralLocationPath { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public List<AccountPropertyConstructionDetail> AccountPropertyConstructionDetail { get; set; }
        public string AccountPropertyConstructionDetailJson { get; set; }
        public string RegisterNumber { get; set; }
        public byte[] AccountRowVersion { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipPostalCode { get; set; }
        public int? CountryID { get; set; }
        public Nullable<int> CountryStateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> TownID { get; set; }
        public Nullable<int> HomogeneousZoneID { get; set; }
        public Nullable<int> MovementID { get; set; }
        public string Number { get; set; }
        public Nullable<System.DateTime> DateOfMovement { get; set; }
        public string ServicesList1 { get; set; }
        public string ServicesList2 { get; set; }
        public Nullable<decimal> RegularFactor { get; set; }
        public Nullable<decimal> SlopePercentage { get; set; }
        public string North { get; set; }
        public string South { get; set; }
        public string East { get; set; }
        public string West { get; set; }
        public string FincaID { get; set; }
        public int OldAccountID { get; set; }
        public int AccountPropertyID { get; set; }
        public DateTime SplitDate { get; set; }
        public DateTime MergetDate { get; set; }
        public string PropertyIDs { get; set; }
        public string AccountServiceIDs { set; get; }

        public int workflowID { get; set; }
        public int workflowStatusID { get; set; }
        public string CreatedBy { get; set; }
        public Guid AssignToID { get; set; } = Guid.Empty;
        public int ReasonID { get; set; }
        public string Comments { get; set; }
        public string AssignToUserEmail { get; set; }
        public string AssignToName { get; set; }
        public string Property { get; set; }
        public int MovementTypeID { get; set; }
        public int SplitTypeID { get; set; }
        public string SplitTypeName { get; set; }
        #endregion
    }
    public class AccountPropertyList
    {
        public List<MUNAccountPropertyGetWithPaging_Result> AccountPropertyModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountPropertyListForSearch
    {
        public List<MUNAccountPropertyGetForSearch_Result> AccountPropertyList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class AccountPropertyRightListForSearch
    {
        public List<MUNAccountPropertyRightGetForSearch_Result> AccountPropertyList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class AccountPropertyModelDetail
    {
        public List<MUNAccountPropertyGet_Result> AccountPropertyModelList { get; set; }
        public List<MUNAccountPropertyConstructionDetailGet_Result> AccountPropertyConstructionModelList { get; set; }
    }
    public class AccountPropertyConstructionDetail
    {
        #region public properties       
        public int ID { get; set; }
        public int PropertyAccountID { get; set; }
        public int MaterialTypeID { get; set; } //Required
        public int MaterialTypeTableID { get; set; }
        public int? ConstructionTypeID { get; set; }
        public int? ConstructionTypeTableID { get; set; }
        public int? ConstructionYear { get; set; }
        public Nullable<int> TotalYears { get; set; }
        public int? StatusID { get; set; }
        public int? StatusTableID { get; set; }
        public int? Floors { get; set; }
        public int? Chambers { get; set; }
        public int? InternalWallsID { get; set; }
        public int? ExternalWallsID { get; set; }
        public int? StructureID { get; set; }
        public int? StructureTableID { get; set; }
        public int? RoofID { get; set; }
        public int? RoofTableID { get; set; }
        public int? CeilingID { get; set; }
        public int? CeilingTableID { get; set; }
        public int? FloorID { get; set; }
        public int? FloorTableID { get; set; }
        public Nullable<decimal> Bathroom { get; set; }
        public decimal BuildingArea { get; set; } // Required
        public Nullable<decimal> UnitValue { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public Nullable<bool> IsDistributed { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
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
        public string UsefulLife { get; set; }
        #endregion
    }
    #endregion

    #region Import Account Property
    public class AccountPropertyImportViewModel
    {
        public int ImportID { get; set; }
        public string GroupCode { get; set; }
        public string RightNumber { get; set; }
        public string Duplicate { get; set; }
        public string Horizontal { get; set; }
        public string RightCode { get; set; }
        public string TaxNumber { get; set; }
        public decimal? TerrainArea { get; set; }
        public decimal? Percentage { get; set; }
        public string MaterialType { get; set; }
        public string ConstructionType { get; set; }
        public int? CurrentAge { get; set; }
        public decimal? BuildingArea { get; set; }
    }
    public class AccountPropertyImportModel
    {
        public int CompanyID { get; set; }
        public bool IsValidate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AccountType { get; set; }      
        public List<AccountPropertyImportViewModel> ImportViewAccountPropertyList { get; set; }
    }
    public class ValidImportAccountPropertyModel
    {
        public string GroupCode { get; set; }
        public string Reason { get; set; }
    }
    #endregion
}

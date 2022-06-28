using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    //public class PropertyRightModel
    //{
    //    #region public properties       
    //    public string RigthNumber { get; set; }
    //    public int OwnerID { get; set; }
    //    public decimal Percentage { get; set; }
    //    public decimal RightArea { get; set; }
    //    public decimal TerrainValue { get; set; }
    //    public decimal TotalTerrainValue { get; set; }
    //    public byte[] RowVersion { get; set; }
    //    public int Status { get; set; }
    //    public List<PropertyRightConstructionDetaiModel> AccountPropertyConstructionDetailJson { get; set; }
    //    #endregion
    //}

    public class AccountPropertyRightModel
    {
        #region public properties       
        public string AccountPropertyRigthID { get; set; }
        public string RigthNumber { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        #endregion
    }

    //public class PropertyRightConstructionDetaiModel
    //{
    //    #region public properties       
    //    public int ID { get; set; }
    //    public int ConstructionTypeID { get; set; }
    //    public int MaterialTypeID { get; set; }
    //    public int ConstructionYear { get; set; }
    //    public int TotalYears { get; set; }
    //    public int StatusID { get; set; }
    //    public int Floors { get; set; }
    //    public int Chambers { get; set; }
    //    public int InternalWallsID { get; set; }
    //    public int ExternalWallsID { get; set; }
    //    public int StructureID { get; set; }
    //    public int RoofID { get; set; }
    //    public int CeilingID { get; set; }
    //    public int FloorID { get; set; }
    //    public decimal? Bathroom { get; set; }
    //    public decimal BuildingArea { get; set; }
    //    public decimal UnitValue { get; set; }
    //    public decimal TotalValue { get; set; }
    //    public bool IsDistributed { get; set; }
    //    public bool IsActive { get; set; }
    //    public bool IsDeleted { get; set; }

    //    public string MaterialType { get; set; }
    //    public string ConstructionType { get; set; }
    //    public string Status { get; set; }
    //    public string Structure { get; set; }
    //    public string Roof { get; set; }
    //    public string Ceiling { get; set; }
    //    public string Floor { get; set; }
    //    public string InternalWalls { get; set; }
    //    public string ExternalWalls { get; set; }
    //    #endregion
    //}

    public class PropertyModel
    {
        public int AccountPropertyID { get; set; }
        public string Property { get; set; }

    }
}
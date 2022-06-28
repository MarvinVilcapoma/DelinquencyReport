using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.Collections
{
    public class PropertyTransferModel
    {
        public int TransferID { get; set; }
        public int OldAccountID { get; set; }
        public Nullable<int> AccountPropertyID { get; set; }
        public int TransferTypeID { get; set; }
        public int AccountServiceID { get; set; }
        public string AccountServiceIDs { get; set; }
        public int NewAccountID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int CompanyID { get; set; }
        public string language { get; set; }
        public string OldAccountName { get; set; }
        public string NewAccountName { get; set; }
        public string OldAccountTaxNumber { get; set; }
        public string NewAccountTaxNumber { get; set; }
        public DateTime TransferDate { get; set; }
        public string Property { get; set; }
        public string OldProperty { get; set; }
        public string TransferType { get; set; }
        public int workflowID { get; set; }
        public int WorkflowVerionID { get; set; }
        public int workflowStatusID { get; set; }
        public string DocumentTypeDetail { get; set; }
        public string Observation { get; set; }
        public List<AccountServiceTransferModel> AccountServiceList { get; set; }
        public List<PropertyTransferAccountDetailModel> TransferAccountDetailList { get; set; }
        public List<PropertyTransferConstructionDetail> TransferPropertyConstructionDetailList { get; set; }
        public PaymentAccountModel AccountFrom { get; set; }
        public PaymentAccountModel AccountTo { get; set; }
        public PaymentCompanyModel Company { get; set; }
        public int SplitTypeID { get; set; }
        public string SplitTypeName { get; set; }
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public Nullable<int> CondoID { get; set; }
        public Nullable<int> CondoTypeID { get; set; }
    }

    public class PropertyTransferListModel
    {
        public List<MUNSERTransferServiceGetWithPagging_Result> PropertyTransferList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

    public class PropertyTransferAccountDetailModel
    {
        public int ID { get; set; }        
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }        
        public int OwnerID { get; set; }
        public string Owner { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string CadastralPlanNumber { get; set; }
        public decimal Area { get; set; }
        public Nullable<decimal> TerrainValue { get; set; }
        public Nullable<decimal> TotalTerrainValue { get; set; }
        public Nullable<decimal> TotalValue { get; set; }        
        public string Number { get; set; }           
        public bool IsFromAccount { get; set; }
    }

    public class PropertyTransferConstructionDetail
    {
        public int ID { get; set; }
        public int PropertyAccountID { get; set; }
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public int MaterialTypeID { get; set; }
        public int MaterialTypeTableID { get; set; }
        public string MaterialType { get; set; }
        public Nullable<int> ConstructionTypeID { get; set; }
        public Nullable<int> ConstructionTypeTableID { get; set; }
        public string ConstructionType { get; set; }
        public Nullable<int> ConstructionYear { get; set; }
        public Nullable<int> TotalYears { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> StatusTableID { get; set; }
        public string Status { get; set; }
        public Nullable<int> Floors { get; set; }
        public Nullable<int> Chambers { get; set; }
        public Nullable<int> InternalWallsID { get; set; }
        public string InternalWalls { get; set; }
        public Nullable<int> ExternalWallsID { get; set; }
        public string ExternalWalls { get; set; }
        public Nullable<int> StructureID { get; set; }
        public Nullable<int> StructureTableID { get; set; }
        public string Structure { get; set; }
        public Nullable<int> RoofID { get; set; }
        public Nullable<int> RoofTableID { get; set; }
        public string Roof { get; set; }
        public Nullable<int> CeilingID { get; set; }
        public Nullable<int> CeilingTableID { get; set; }
        public string Ceiling { get; set; }
        public Nullable<int> FloorID { get; set; }
        public Nullable<int> FloorTableID { get; set; }
        public string Floor { get; set; }
        public Nullable<decimal> Bathroom { get; set; }
        public decimal BuildingArea { get; set; }
        public Nullable<decimal> UnitValue { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public bool IsDistributed { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string UsefulLife { get; set; }
    }
}
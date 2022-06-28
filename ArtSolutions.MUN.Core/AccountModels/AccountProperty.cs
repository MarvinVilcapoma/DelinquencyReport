using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountProperty
    {
        #region Account Property
        public AccountPropertyList Get(int companyID, string language, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder, int? AccountID = null)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;

                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountPropertyList accountPropertyList = new AccountPropertyList();

                accountPropertyList.AccountPropertyModelList = context.MUNAccountPropertyGetWithPaging(companyID, language, filterText, pageIndex, pageSize, sortColumn, sortOrder, AccountID, TotalRecord).ToList();
                accountPropertyList.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return accountPropertyList;
            }
        }
        public AccountPropertyModelDetail Get(int companyID, int? id, bool? isActive,int transferID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                AccountPropertyModelDetail model = new AccountPropertyModelDetail();
                model.AccountPropertyModelList = context.MUNAccountPropertyGet(companyID, language, id, isActive, transferID).ToList();
                model.AccountPropertyConstructionModelList = context.MUNAccountPropertyConstructionDetailGet(companyID, language, null, id, isActive).ToList();
                return model;
            }
        }
        public AccountPropertyListForSearch GetForSearch(int companyID, string searchText, int pageIndex, int pageSize)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountPropertyListForSearch accountPropertyList = new AccountPropertyListForSearch();
                accountPropertyList.AccountPropertyList = context.MUNAccountPropertyGetForSearch(companyID, searchText, pageIndex, pageSize, TotalRecord).ToList();
                accountPropertyList.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return accountPropertyList;
            }
        }
        public AccountPropertyRightListForSearch GetRightForSearch(int companyID, string searchText, int pageIndex, int pageSize)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountPropertyRightListForSearch accountPropertyList = new AccountPropertyRightListForSearch();
                accountPropertyList.AccountPropertyList = context.MUNAccountPropertyRightGetForSearch(companyID, searchText, pageIndex, pageSize, TotalRecord).ToList();
                accountPropertyList.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return accountPropertyList;
            }
        }
        public List<MUNAccountPropertyRightGetByOwner_Result> GetAccountPropertyRightByOwner(int companyId, int ownerId, int serviceID, int fiscalYearID, int? id, bool? isTransferByRight)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyRightGetByOwner(companyId, ownerId, serviceID, fiscalYearID, id, isTransferByRight).ToList();
            }
        }
        public List<MUNAccountPropertyRightGet_Result> GetAccountPropertyRight(int companyId)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyRightGet(companyId).ToList();
            }
        }
        public int ActiveInactive(int companyID, int ID, bool IsActive, Guid ModifiedUserID, DateTime ModifiedDate)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyUpdateStatus(companyID, ID, IsActive, ModifiedUserID, ModifiedDate).FirstOrDefault() ?? 0;

            }
        }

        public bool HasDebt(int ID, int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter IsPendingAmount = new ObjectParameter("IsPendingAmount", typeof(bool));
                context.MUNAccountPropertyHasDebt(ID, accountID, IsPendingAmount);
                return (bool)IsPendingAmount.Value;

            }
        }

        public int Delete(int ID, Guid ModifiedUserID, DateTime ModifiedDate)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyDelete(ID, ModifiedUserID, ModifiedDate).FirstOrDefault() ?? 0;

            }
        }
        public int Insert(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@AccountPropertyID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegisterNumber",Value=string.IsNullOrEmpty(model.RegisterNumber)?(object)DBNull.Value:model.RegisterNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@PropertyNumber",Value= model.PropertyNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CondoID", Value= model.CondoID.HasValue?model.CondoID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CondoTypeID", Value= model.CondoTypeID.HasValue?model.CondoTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CadastralPlanNumber",Value=string.IsNullOrEmpty(model.CadastralPlanNumber)?(object)DBNull.Value:model.CadastralPlanNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TotalValue", Value= model.TotalValue.HasValue?model.TotalValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Notes", Value= string.IsNullOrEmpty(model.Notes)?(object)DBNull.Value:model.Notes ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@RigthNumber", Value= model.RigthNumber ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@OwnerID", Value= model.OwnerID ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Percentage", Value= model.Percentage.HasValue?model.Percentage.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@RightArea", Value= model.Area ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@TerrainValue", Value= model.TerrainValue.HasValue?model.TerrainValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@TotalTerrainValue", Value= model.TotalTerrainValue.HasValue?model.TotalTerrainValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@MainAccessID", Value= model.MainAccessID.HasValue?model.MainAccessID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyFront", Value= model.PropertyFront.HasValue?model.PropertyFront.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@PropertyLength", Value= model.PropertyLength.HasValue?model.PropertyLength.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@SlopeID", Value= model.SlopeID.HasValue?model.SlopeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SlopePercentage", Value= model.SlopePercentage.HasValue?model.SlopePercentage.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Level", Value= model.Level.HasValue?model.Level.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@LevelTypeID", Value= model.LevelTypeID.HasValue?model.LevelTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BlockLocationID", Value= model.BlockLocationID.HasValue?model.BlockLocationID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegularID", Value= model.RegularID.HasValue?model.RegularID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegularFactor", Value= model.RegularFactor.HasValue?model.RegularFactor.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@HydrologyID", Value= model.HydrologyID.HasValue?model.HydrologyID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@UseOfLandID", Value= model.UseOfLandID.HasValue?model.UseOfLandID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@HomogeneousZoneID", Value= model.HomogeneousZoneID.HasValue?model.HomogeneousZoneID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MovementID", Value= model.MovementID.HasValue?model.MovementID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Latitude", Value= model.Latitude.HasValue?model.Latitude:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Longitude", Value= model.Longitude.HasValue?model.Longitude:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@CadastralLocationPath", Value= string.IsNullOrEmpty(model.CadastralLocationPath)?(object)DBNull.Value:model.CadastralLocationPath,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AddressLine1",Value=string.IsNullOrEmpty(model.AddressLine1)?(object)DBNull.Value:model.AddressLine1,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AddressLine2",Value=string.IsNullOrEmpty(model.AddressLine2)?(object)DBNull.Value:model.AddressLine2,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@ZipPostalCode",Value=string.IsNullOrEmpty(model.ZipPostalCode)?(object)DBNull.Value:model.ZipPostalCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CountryID", Value= model.CountryID.HasValue?model.CountryID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CountryStateID", Value= model.CountryStateID.HasValue?model.CountryStateID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CityID", Value= model.CityID.HasValue?model.CityID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@TownID", Value= model.TownID.HasValue?model.TownID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Number", Value= string.IsNullOrEmpty(model.Number)?(object)DBNull.Value:model.Number,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@DateOfMovement", Value= model.DateOfMovement.HasValue?model.DateOfMovement.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ServicesList1", Value= string.IsNullOrEmpty(model.ServicesList1)?(object)DBNull.Value:model.ServicesList1 ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@ServicesList2", Value= string.IsNullOrEmpty(model.ServicesList2)?(object)DBNull.Value:model.ServicesList2 ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@North", Value= string.IsNullOrEmpty(model.North)?(object)DBNull.Value:model.North ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@South", Value= string.IsNullOrEmpty(model.South)?(object)DBNull.Value:model.South ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@East", Value= string.IsNullOrEmpty(model.East)?(object)DBNull.Value:model.East ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@West", Value= string.IsNullOrEmpty(model.West)?(object)DBNull.Value:model.West ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AccountPropertyConstructionDetailJson",Value=string.IsNullOrEmpty(model.AccountPropertyConstructionDetailJson)?(object)DBNull.Value:model.AccountPropertyConstructionDetailJson},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                outputparam
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertyInsert");
                return (int)outputparam.Value;
            }
        }

        public int Split(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@TransferID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@FromOwnerID", Value=model.OldAccountID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyID", Value=model.AccountPropertyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SplitDate", Value=model.SplitDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@AccountPropertyConstructionDetailJson",Value=string.IsNullOrEmpty(model.AccountPropertyConstructionDetailJson)?(object)DBNull.Value:model.AccountPropertyConstructionDetailJson},
                                                new SqlParameter { ParameterName = "@WorkFlowID", Value=model.workflowID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@WorkflowStatusID", Value=model.workflowStatusID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@CreatedBy",Value= model.CreatedBy,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@ID", Value=model.ID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ReasonID", Value=model.ReasonID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Comment", Value=model.Comments,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AssignedTo", Value=model.AssignToID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@MovementID", Value=model.MovementTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SplitTypeID", Value=model.SplitTypeID,SqlDbType = SqlDbType.Int},
                                                outputparam
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertySplit");
                return (int)outputparam.Value;
            }
        }

        public void SplitDelete(int TransferID, int CompanyID )
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNAccountPropertySplitVoid(CompanyID, TransferID);
            }

        }
        public int Merge(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@TransferID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyID", Value=model.PropertyIDs,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@ToOwnerID", Value=model.OwnerID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@AccountServiceIDs", Value=model.AccountServiceIDs,SqlDbType = SqlDbType.NVarChar},



                                                new SqlParameter { ParameterName = "@CondoID", Value= model.CondoID.HasValue?model.CondoID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CondoTypeID", Value= model.CondoTypeID.HasValue?model.CondoTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CadastralPlanNumber",Value=string.IsNullOrEmpty(model.CadastralPlanNumber)?(object)DBNull.Value:model.CadastralPlanNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TotalValue", Value= model.TotalValue.HasValue?model.TotalValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Notes", Value= string.IsNullOrEmpty(model.Notes)?(object)DBNull.Value:model.Notes ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Percentage", Value= model.Percentage.HasValue?model.Percentage.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@RightArea", Value= model.Area ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@MergeDate", Value=model.MergetDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@PropertyNumber", Value=model.PropertyNumber,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@RigthNumber", Value=model.RigthNumber,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@AccountPropertyConstructionDetailJson",Value=string.IsNullOrEmpty(model.AccountPropertyConstructionDetailJson)?(object)DBNull.Value:model.AccountPropertyConstructionDetailJson},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@CreatedBy",Value= model.CreatedBy,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@MovementID", Value=model.MovementTypeID,SqlDbType = SqlDbType.Int},
                                                outputparam
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertyMerge");
                return (int)outputparam.Value;
            }
        }
        public int Update(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@ID", Value=model.ID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyNumber",Value= model.PropertyNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CondoID", Value= model.CondoID.HasValue?model.CondoID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CondoTypeID", Value= model.CondoTypeID.HasValue?model.CondoTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CadastralPlanNumber",Value=string.IsNullOrEmpty(model.CadastralPlanNumber)?(object)DBNull.Value:model.CadastralPlanNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Notes", Value= string.IsNullOrEmpty(model.Notes)?(object)DBNull.Value:model.Notes ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@RigthNumber", Value= model.RigthNumber ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@OwnerID", Value= model.OwnerID ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Percentage", Value= model.Percentage.HasValue?model.Percentage.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@RightArea", Value= model.Area ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@TerrainValue", Value= model.TerrainValue.HasValue?model.TerrainValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@TotalTerrainValue", Value= model.TotalTerrainValue.HasValue?model.TotalTerrainValue.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@MainAccessID", Value= model.MainAccessID.HasValue?model.MainAccessID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyFront", Value= model.PropertyFront.HasValue?model.PropertyFront.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@PropertyLength", Value= model.PropertyLength.HasValue?model.PropertyLength.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@SlopeID", Value= model.SlopeID.HasValue?model.SlopeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SlopePercentage", Value= model.SlopePercentage.HasValue?model.SlopePercentage.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Level", Value= model.Level.HasValue?model.Level.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@LevelTypeID", Value= model.LevelTypeID.HasValue?model.LevelTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BlockLocationID", Value= model.BlockLocationID.HasValue?model.BlockLocationID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegularID", Value= model.RegularID.HasValue?model.RegularID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegularFactor", Value= model.RegularFactor.HasValue?model.RegularFactor.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@HydrologyID", Value= model.HydrologyID.HasValue?model.HydrologyID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@UseOfLandID", Value= model.UseOfLandID.HasValue?model.UseOfLandID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@HomogeneousZoneID", Value= model.HomogeneousZoneID.HasValue?model.HomogeneousZoneID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MovementID", Value= model.MovementID.HasValue?model.MovementID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Latitude", Value= model.Latitude.HasValue?model.Latitude:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@Longitude", Value= model.Longitude.HasValue?model.Longitude:(object)DBNull.Value ,SqlDbType = SqlDbType.Decimal},
                                                new SqlParameter { ParameterName = "@CadastralLocationPath", Value= string.IsNullOrEmpty(model.CadastralLocationPath)?(object)DBNull.Value:model.CadastralLocationPath,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AddressLine1",Value=string.IsNullOrEmpty(model.AddressLine1)?(object)DBNull.Value:model.AddressLine1,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AddressLine2",Value=string.IsNullOrEmpty(model.AddressLine2)?(object)DBNull.Value:model.AddressLine2,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@ZipPostalCode",Value=string.IsNullOrEmpty(model.ZipPostalCode)?(object)DBNull.Value:model.ZipPostalCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CountryID", Value= model.CountryID.HasValue?model.CountryID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CountryStateID", Value= model.CountryStateID.HasValue?model.CountryStateID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CityID", Value= model.CityID.HasValue?model.CityID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@TownID", Value= model.TownID.HasValue?model.TownID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Number", Value= string.IsNullOrEmpty(model.Number)?(object)DBNull.Value:model.Number,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@DateOfMovement", Value= model.DateOfMovement.HasValue?model.DateOfMovement.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ServicesList1", Value= string.IsNullOrEmpty(model.ServicesList1)?(object)DBNull.Value:model.ServicesList1 ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@ServicesList2", Value= string.IsNullOrEmpty(model.ServicesList2)?(object)DBNull.Value:model.ServicesList2 ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@North", Value= string.IsNullOrEmpty(model.North)?(object)DBNull.Value:model.North ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@South", Value= string.IsNullOrEmpty(model.South)?(object)DBNull.Value:model.South ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@East", Value= string.IsNullOrEmpty(model.East)?(object)DBNull.Value:model.East ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@West", Value= string.IsNullOrEmpty(model.West)?(object)DBNull.Value:model.West ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@AccountPropertyConstructionDetailJson",Value=model.AccountPropertyConstructionDetailJson},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@AccountRowVersion",Value=model.AccountRowVersion}
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertyUpdate");
                return 1;
            }
        }
        public int? MergeCheckNotAssociatedServices(int companyID, string language, string commaSeparatedPropertyID, int? AccountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyMergeCheckNotAssociatedServices(companyID, language, commaSeparatedPropertyID, AccountID).FirstOrDefault().Value;
            }
        }
        public int AccountPropertyRightUpdate(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@ID", Value=model.ID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PropertyNumber",Value= model.PropertyNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CondoID", Value= model.CondoID.HasValue?model.CondoID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CondoTypeID", Value= model.CondoTypeID.HasValue?model.CondoTypeID.Value:(object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RigthNumber", Value= model.RigthNumber ,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertyRightUpdate");
                return 1;
            }
        }


        public int InsertTemporary(AccountPropertyModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@AccountPropertyID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@OwnerID", Value= model.OwnerID ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                outputparam
                                            };
                context.ExecuteSqlProcedure(sqlparameters, "MUNAccountPropertyTemporyInsert");
                return (int)outputparam.Value;
            }
        }
        #endregion

        #region Account Property Filing
        public List<MUNAccountPropertyGetForFilling_Result> GetForFilling(int companyID, int id, int accountServiceCollectionDetailId)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyGetForFilling(companyID, id, accountServiceCollectionDetailId).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingPropertyTaxHistoryGet_Result> PropertyFilingHistoryGet(int companyID, string language, int PropertyAccountID, string PropertyNumber, string RightNumber)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingPropertyTaxHistoryGet(companyID, PropertyAccountID, PropertyNumber, RightNumber).ToList();
            }
        }
        public List<MUNAccountPropertyRightHistoryGet_Result> AccountPropertyRightHistoryGet(int companyID, string language, int AccountPropertyID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPropertyRightHistoryGet(companyID, AccountPropertyID).ToList();
            }
        }
        public List<MUNSERAccountPropertyRightGetNotPaid_Result> PropertyRightGetNotPaid(int? accountPropertyID, bool? checkActivePaymentPlan, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountPropertyRightGetNotPaid(accountPropertyID, checkActivePaymentPlan, companyID, language).ToList();
            }
        }
        #endregion

        #region Import Account Property
        public List<ValidImportAccountPropertyModel> ImportAccountPropertyValidation(AccountPropertyImportModel model, string language)
        {
            List<ValidImportAccountPropertyModel> validData = new List<ValidImportAccountPropertyModel>();
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable importTable = model.ImportViewAccountPropertyList.ToDataTable();
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };

                SqlParameter[] sqlparameters = {
                        new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                        new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar},
                        new SqlParameter { ParameterName = "@IsValidate",Value=model.IsValidate,SqlDbType = SqlDbType.Int},
                        new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier},
                        new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                        new SqlParameter { ParameterName = "@ModifiedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier },
                        new SqlParameter { ParameterName = "@ModifiedDate",Value= model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                        new SqlParameter {ParameterName = "@MUNIMPPropertyAccounts",Value=importTable,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountPropertyImport"}
                        ,outParam,
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPAccountPropertyImport");
                validData = ds.Tables[0].ToList<ValidImportAccountPropertyModel>();
                return validData;
            }
        }
        public int ImportAccountPropertyInsert(AccountPropertyImportModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable dtImport = model.ImportViewAccountPropertyList.ToDataTable();

                int output = 1;
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                SqlParameter[] sqlparameters = {
                        new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@IsValidate", Value=model.IsValidate,SqlDbType = SqlDbType.Bit}
                        ,new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@ModifiedUser",Value=model.ModifiedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@ModifiedDate", Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@MUNIMPPropertyAccounts",Value=dtImport,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountPropertyImport"}
                        ,outParam
                };
                context.ExecuteSqlProcedureWithOutputParam(sqlparameters, "MUNIMPAccountPropertyImport");
                output = (int)outParam.Value;
                return output;
            }
        }
        #endregion
    }
}
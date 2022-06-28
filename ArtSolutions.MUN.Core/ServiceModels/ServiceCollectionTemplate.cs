using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionTemplate
    {
        #region CollectionTemplate
        public List<MUNSERServiceCollectionTemplateGet_Result> Get(int? iD, int companyID, string language, bool? isActive, string fiterText)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {               
                return context.MUNSERServiceCollectionTemplateGet(iD, companyID, language, isActive, fiterText).ToList();
            }
        }
        public ServiceCollectionTemplateListModel GetWithPaging(int companyID, string filterText, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ServiceCollectionTemplateListModel model = new ServiceCollectionTemplateListModel();
                model.CollectionTemplateList = context.MUNSERServiceCollectionTemplateGetWithPaging(companyID, language, filterText, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public int Insert(ServiceCollectionTemplateModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                DataTable serviceCollectionTemplateDetailTable = model.ServiceCollectionTemplateDetailListUTD.ToDataTable();
                SqlParameter outputparam = new SqlParameter { ParameterName = "@ID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Name", Value=model.Name,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Description",Value= !string.IsNullOrEmpty(model.Description) ? model.Description : (object)DBNull.Value ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsDeleteD",Value= model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive", Value= model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate, SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@CreatedUserID", Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value= model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@Locale", Value=model.Locale,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@MUNSERSeviceCollectionTemplateDetailUTD",Value=serviceCollectionTemplateDetailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERSeviceCollectionTemplateDetailUTD"},
                                                 outputparam,
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSERServiceCollectionTemplateInsert");
                return (int)outputparam.Value;



            }
        }
        public int Update(ServiceCollectionTemplateModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                DataTable serviceCollectionTemplateDetailTable = model.ServiceCollectionTemplateDetailListUTD.ToDataTable();
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@ID", Value=model.ID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Name", Value=model.Name,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Description",Value=!string.IsNullOrEmpty(model.Description) ? model.Description : (object)DBNull.Value,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsDeleteD",Value= model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive", Value= model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate, SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@CreatedUserID", Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value= model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@Locale", Value=model.Locale,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Original_RowVersion", Value=model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                                 new SqlParameter { ParameterName = "@MUNSERSeviceCollectionTemplateDetailUTD",Value=serviceCollectionTemplateDetailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERSeviceCollectionTemplateDetailUTD"},
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSERServiceCollectionTemplateUpdate");
                return model.ID;
            }
        }
        public int UpdateStatus(int id, int companyID, bool isActive)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCollectionTemplateUpdateStatus(id, companyID, isActive);
            }
        }
        #endregion

        #region CollectionTemplateDetail
        public List<MUNSERServiceCollectionTemplateDetailGet_Result> DetailGet (int? iD, int companyID, int collectionTemplateID, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCollectionTemplateDetailGet(iD, companyID, collectionTemplateID, language).ToList();
            }
        }
        #endregion
    }
}

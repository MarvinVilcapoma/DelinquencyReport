using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.SchedulerEmailModels
{
    public class SchedulerEmail
    {
        public List<MUNSchedulerEmailGet_Result> Get(int companyId, string language,int? id)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSchedulerEmailGet(companyId, id).ToList();
            }
        }

        public SchedulerEmailList GetByPaging(int? schedulerEmailID, int companyID, string language, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                SchedulerEmailList schedulerEmailList = new SchedulerEmailList();

                schedulerEmailList.SchedulerEmailModelList = context.MUNSchedulerEmailGetWithPaging(schedulerEmailID,companyID, language, filterText, pageIndex, pageSize, totalRecord, sortColumn, sortOrder).ToList();
                schedulerEmailList.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return schedulerEmailList;
            }
        }
        public int Insert(SchedulerEmailModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                SqlParameter outputparam = new SqlParameter { ParameterName = "@SchedulerEmailID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Email",Value=model.Email.Trim(),SqlDbType = SqlDbType.VarChar},                                                 
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsDeleted",Value=model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 outputparam,
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSchedulerEmailInsert");
                return (int)outputparam.Value; // returns SchedulerEmailID                               
            }
        }
        public int Update(SchedulerEmailModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@ID", Value = model.ID, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Email",Value=model.Email.Trim(),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@Original_RowVersion",Value= model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSchedulerEmailUpdate");
                return model.ID;
            }
        }

        public void Delete(SchedulerEmailModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.MUNSchedulerEmaileDelete(model.ID, model.ReasonForDeleted, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
            }
        }

    }
}

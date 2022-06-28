using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class PropertyTransfer
    {

        public PropertyTransferListModel GetWithPaging(int companyID, string filterText, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PropertyTransferListModel model = new PropertyTransferListModel();
                model.PropertyTransferList = context.MUNSERTransferServiceGetWithPagging(companyID, filterText, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public PropertyTransferModel GetPropertyTransfer(int ID, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                PropertyTransferModel model = new PropertyTransferModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= ID},
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERTransferServiceGet");
                model = ds.Tables[0].ToList<PropertyTransferModel>().FirstOrDefault() ?? new PropertyTransferModel();
                model.Company = ds.Tables[1].ToList<PaymentCompanyModel>().FirstOrDefault() ?? new PaymentCompanyModel();
                model.AccountFrom = ds.Tables[2].ToList<PaymentAccountModel>().FirstOrDefault() ?? new PaymentAccountModel();
                model.AccountTo = ds.Tables[3].ToList<PaymentAccountModel>().FirstOrDefault() ?? new PaymentAccountModel();
                model.AccountServiceList = ds.Tables[4].ToList<AccountServiceTransferModel>() ?? new List<AccountServiceTransferModel>();
                model.TransferAccountDetailList = ds.Tables[5].ToList<PropertyTransferAccountDetailModel>() ?? new List<PropertyTransferAccountDetailModel>();
                model.TransferPropertyConstructionDetailList = ds.Tables[6].ToList<PropertyTransferConstructionDetail>() ?? new List<PropertyTransferConstructionDetail>();
                return model;
            }
        }

        public int Insert(PropertyTransferModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@TransferID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID},
                    new SqlParameter { ParameterName = "@FromAccountID",Value=model.OldAccountID},
                    new SqlParameter { ParameterName = "@ToAccountID",Value = model.NewAccountID},
                    new SqlParameter { ParameterName = "@TransferTypeID",Value=model.TransferTypeID},
                    new SqlParameter { ParameterName = "@AccountPropertyID",Value=model.AccountPropertyID>0?model.AccountPropertyID:(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                    new SqlParameter { ParameterName = "@AccountServiceIDs",Value=model.AccountServiceIDs},
                    new SqlParameter { ParameterName = "@Observation",Value=string.IsNullOrEmpty(model.Observation)?(object)DBNull.Value:model.Observation,SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@WorkflowID",Value=model.workflowID},
                    new SqlParameter { ParameterName = "@WorkflowVersionID",Value=model.WorkflowVerionID},
                    new SqlParameter { ParameterName = "@WorkflowStatusID",Value=model.workflowStatusID},
                    new SqlParameter { ParameterName = "@TransferDate",Value=model.TransferDate},
                    new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID},
                    new SqlParameter { ParameterName = "@CreatedDate",Value=model.CreatedDate},
                    new SqlParameter { ParameterName = "@Language",Value=model.language},
                    new SqlParameter { ParameterName = "@PropertyNumber",Value=string.IsNullOrEmpty(model.PropertyNumber)?(object)DBNull.Value:model.PropertyNumber,SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@RigthNumber",Value=string.IsNullOrEmpty(model.RigthNumber)?(object)DBNull.Value:model.RigthNumber,SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@CondoID",Value=model.CondoID
                     ?? (object)DBNull.Value,SqlDbType = SqlDbType.Int},
                    new SqlParameter { ParameterName = "@CondoTypeID",Value=model.CondoTypeID
                     ?? (object)DBNull.Value,SqlDbType = SqlDbType.Int},
                    outputparam
                };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSERTransferServiceInsert");
                return (int)outputparam.Value;
            }
        }
        public int Update(PropertyTransferModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var Retval = new ObjectParameter("Retval", typeof(int));
                context.MUNSERTransferServiceUpdateStatus(model.TransferID, model.workflowStatusID, model.ModifiedUserID, model.ModifiedDate, Retval);
                return (int)Retval.Value;
            }
        }
    }
}

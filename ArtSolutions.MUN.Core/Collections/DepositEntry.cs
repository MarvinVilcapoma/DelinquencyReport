using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class DepositEntry
    {
        public DepositEntryListModel GetWithPaging(int companyID, int? accountID, int? iD, string filterText, bool? isVoid, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                DepositEntryListModel model = new DepositEntryListModel();
                model.DepositEntryList = context.MUNCOLDepositGetWithPaging(companyID, accountID, iD, filterText, isVoid, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public int Insert(DepositEntryModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter outputparam = new ObjectParameter("DepositId", typeof(int));
                context.MUNCOLDepositInsert(model.CompanyID, model.DepositTypeID, model.Date, model.Description,
                        model.NetDeposit, model.ClosingCount, model.BankAccountID, model.BankAccountCode,
                        model.BankAccountName, model.BankName, model.FinanceAccountID, model.FinanceAccountCode,
                        model.FinanceAccountName, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID,
                        model.ModifiedDate, model.CommaSeperatedClosingIds, outputparam);

                return (int)outputparam.Value;
            }
        }
        public DepositEntryPrintModel GetPrint(int iD, int companyID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                DepositEntryPrintModel model = new DepositEntryPrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLDepositPrint");
                model.DepositEntry = ds.Tables[0].ToList<DepositEntryModel>().FirstOrDefault() ?? new DepositEntryModel();
                model.ClosedEntryList = ds.Tables[1].ToList<ClosingEntryModel>() ?? new List<ClosingEntryModel>();
                return model;
            }
        }
        public bool Void(DepositEntryModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNCOLDepositVoid(model.ID, model.CompanyID, model.VoidReason, model.RowVersion, model.ModifiedUserID, model.ModifiedDate);
                return true;
            }
        }
    }
}

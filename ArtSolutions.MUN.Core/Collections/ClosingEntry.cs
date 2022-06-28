using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class ClosingEntry
    {
        public ClosingEntryListModel GetWithPaging(int companyID, int? accountID, int? iD, string filterText, bool? isDeposited, bool? isVoid, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ClosingEntryListModel model = new ClosingEntryListModel();
                model.ClosingEntryList = context.MUNCOLClosingGetWithPaging(companyID, accountID, iD, filterText, isDeposited, isVoid, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public List<MUNCOLClosingGet_Result> Get(int companyID, int? accountID, int? iD, string language, bool? OnlyPostedClosingEntries)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNCOLClosingGet(companyID, accountID, iD, language, OnlyPostedClosingEntries).ToList();
            }
        }
        public int Insert(ClosingEntryModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter outputparam = new ObjectParameter("ClosingId", typeof(int));
                context.MUNCOLClosingInsert(model.CompanyID, model.CashierID, model.CashierName, model.ClosingTypeID, model.Date,
                     model.Description, model.PaymentOptionID, model.NetClosing, model.PaymentReceiptCount,
                     model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate, model.CommaSeperatedPaymentIds,
                     outputparam);
                return (int)outputparam.Value;
            }
        }
        public ClosingEntryPrintModel GetPrint(int iD, int companyID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ClosingEntryPrintModel model = new ClosingEntryPrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLClosingPrint");
                model.ClosingEntry = ds.Tables[0].ToList<ClosingEntryModel>().FirstOrDefault() ?? new ClosingEntryModel();
                model.PaymentReceipts = ds.Tables[1].ToList<PaymentModel>() ?? new List<PaymentModel>();
                return model;
            }
        }
        public bool Void(ClosingEntryModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNCOLClosingVoid(model.ID, model.CompanyID, model.VoidReason, model.RowVersion, model.ModifiedUserID, model.ModifiedDate);
                return true;
            }
        }
    }
}

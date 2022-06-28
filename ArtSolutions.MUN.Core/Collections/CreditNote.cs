using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class CreditNote
    {
        public CreditNoteListModel GetWithPaging(int companyID, int? accountID, int? iD, string filterText, bool? isVoid, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                CreditNoteListModel model = new CreditNoteListModel();
                model.CreditNoteList = context.MUNCOLCreditNoteGetWithPaging(companyID, accountID, iD, filterText, isVoid, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public CreditNotePrintModel GetPrint(int iD, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                CreditNotePrintModel model = new CreditNotePrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLCreditNotePrint");
                model.CreditNote = ds.Tables[0].ToList<CreditNoteModel>().FirstOrDefault() ?? new CreditNoteModel();
                model.CreditNoteAppliedDetailList = ds.Tables[1].ToList<CreditNoteAppliedDetail>() ?? new List<CreditNoteAppliedDetail>();
                model.CreditNoteImagesList = ds.Tables[2].ToList<CreditNoteImagesModel>() ?? new List<CreditNoteImagesModel>();
                model.CreditNoteRefundDetailList = ds.Tables[3].ToList<CreditNoteRefundDetail>() ?? new List<CreditNoteRefundDetail>();

                return model;
            }
        }
        public int Insert(CreditNoteModel model,string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                context.MUNCOLCreditNoteByPaymentReceipt(model.CompanyID, model.AccountID, model.PaymentID, model.Date, model.Amount, model.Reason, model.CommaSeperatedImageIDs, model.CreatedUserID, model.CreatedDate, language, ID);
                return int.Parse(ID.Value.ToString());
            }
        }
        public int Update(CreditNoteModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;                
                var Retval = new ObjectParameter("Retval", typeof(int));
                context.MUNCOLCreditNoteUpdateStatus(model.ID, model.WorkflowStatusID, model.ModifiedUserID, model.ModifiedDate, Retval);
                return (int)Retval.Value;
            }
        }
    }
}

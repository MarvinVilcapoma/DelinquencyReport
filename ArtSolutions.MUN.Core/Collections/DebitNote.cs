using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.Collections
{
    public class DebitNote
    {
        public DebitNoteListModel GetWithPaging(int companyID, int? accountID, int? iD, string filterText, bool? isVoid, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                DebitNoteListModel model = new DebitNoteListModel();
                model.DebitNoteList = context.MUNCOLDebitNoteGetWithPaging(companyID, accountID, iD, filterText, isVoid, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public DebitNotePrintModel GetPrint(int iD, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                DebitNotePrintModel model = new DebitNotePrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLDebitNotePrint");
                model.DebitNote = ds.Tables[0].ToList<DebitNoteModel>().FirstOrDefault() ?? new DebitNoteModel();
                model.DebitNotePaidList = ds.Tables[1].ToList<DebitNotePaidModel>() ?? new List<DebitNotePaidModel>();
                model.DebitNoteImagesList = ds.Tables[2].ToList<DebitNoteImagesModel>() ?? new List<DebitNoteImagesModel>();
                
                return model;
            }
        }
        public int Insert(DebitNoteModel model, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                context.MUNCOLDebitNoteByPaymentReceipt(model.CompanyID, model.AccountID, model.PaymentID, model.Date, model.Amount, model.Reason, model.CommaSeperatedImageIDs, model.CreatedUserID, model.CreatedDate, language, ID);
                return int.Parse(ID.Value.ToString());
            }
        }
        public int Update(DebitNoteModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                var Retval = new ObjectParameter("Retval", typeof(int));
                context.MUNCOLDebitNoteUpdateStatus(model.ID, model.WorkflowStatusID, model.ModifiedUserID, model.ModifiedDate, Retval);
                return (int)Retval.Value;
            }
        }
        public List<MUNCOLDebitNoteGet_Result> DebitNotesGet(int companyID, int accountID)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNCOLDebitNoteGet(companyID,accountID).ToList();
            }
        }

        public int GetAccountDebitNoteExists(int CompanyID, int AccountID)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNCOLDebitNoteExistByAccount(CompanyID, AccountID);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class DebitHistory
    {
        public DebitHistoryList Get(int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DebitHistoryList debitHistoryList = new DebitHistoryList();
                debitHistoryList.DebitHistoryModelList = context.MUNAccountDebitHistoryGet(accountID).ToList();
                return debitHistoryList;
            }
        }
        public int Insert(DebitHistoryModel model)
        {
            int DebitHistoryID = 0;
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                DebitHistoryID = context.MUNAccountDebitHistoryInsert(model.Date, model.AccountID, model.DebitAmount, model.Reason, model.PaymentID,model.IsPaid, model.CreatedUserID, model.CreatedDate, ID);
                return DebitHistoryID;
            }
        }
        public DebitHistoryDetailModel GetDebitHistory(int ID,string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DebitHistoryDetailModel model = new DebitHistoryDetailModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= ID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountDebitHistoryDetailGet");
                model.DebitHistory = ds.Tables[0].ToList<DebitHistoryModel>().FirstOrDefault() ?? new DebitHistoryModel();
                model.DebitHistoryPaidList = ds.Tables[1].ToList<DebitHistoryPaidModel>() ?? new List<DebitHistoryPaidModel>();
                return model;
            }
        }
    }
}

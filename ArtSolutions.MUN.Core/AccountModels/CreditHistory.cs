using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class CreditHistory
    {
        public CreditHistoryList Get(int companyID,int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                CreditHistoryList creditHistoryList = new CreditHistoryList();
                creditHistoryList.CreditHistoryModelList = context.MUNAccountCreditHistoryGet(companyID, accountID).ToList();
                return creditHistoryList;
            }
        }
        public int Insert(CreditHistoryModel model)
        {
            int CreditHistoryID = 0;
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                CreditHistoryID = context.MUNAccountCreditHistoryInsert(model.CompanyID, model.Date, model.AccountID, model.CreditAmount, model.Reason,model.CreatedUserID, model.CreatedDate, ID);
                return CreditHistoryID;
            }
        }
    }
}

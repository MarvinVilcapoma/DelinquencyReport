using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountUserModels
{
    public class AccountUser
    {
        public List<MUNUserAccountGet_Result> Get(int companyID, int? accountID, int? accountTypeID, Guid userID)
        {
            using (AccountUserDataModelContainer context = new AccountUserDataModelContainer())
            {
                return context.MUNUserAccountGet(companyID, accountID, accountTypeID, userID).ToList();
            }
        }

        public int Insert(AccountUserModel model)
        {
            using (AccountUserDataModelContainer context = new AccountUserDataModelContainer())
            {
                context.MUNUserAccountInsert(model.CompanyID, model.AccountTypeID, model.AccountID, model.UserID, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate);
                return model.AccountID;
            }
        }
    }
}

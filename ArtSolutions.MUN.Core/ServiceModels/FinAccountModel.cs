using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class FinAccountModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameFriendly { get; set; }
        public string AccountType { get; set; }
        public string CodeFriendly { get; set; }
    }
    public class CheckbookAccountListModel
    {
        public string Code { get; set; }
        public int CheckbookId { get; set; }
        public string Name { get; set; }
        public int GrantID { get; set; }
        public int FinancePaymentAccountID { get; set; }
        public string FincanceAccountCode { get; set; }
        public string CodeFriendly { get; set; }
        public string FinanceAccountName { get; set; }
        public string NameFriendly { get; set; }
    }
    public class FinGrantAccountModel
    {
        public List<FinAccountModel> ReceivableAccountList { get; set; }
        public List<FinAccountModel> RevenueAccountList { get; set; }
        public List<CheckbookAccountListModel> CheckbookAccountList { get; set; }

    }
}

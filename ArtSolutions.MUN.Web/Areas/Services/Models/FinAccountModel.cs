namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FinAccountModel
    {
        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameFriendly { get; set; }
        public string AccountType { get; set; }
        public string CodeFriendly { get; set; }
        #endregion       
    }
    public class CheckbookAccountListModel
    {
        #region public properties
        public string Code { get; set; }
        public int CheckbookId { get; set; }
        public string Name { get; set; }
        public int GrantID { get; set; }
        public int FinancePaymentAccountID { get; set; }
        public string FincanceAccountCode { get; set; }
        public string CodeFriendly { get; set; }
        public string FinanceAccountName { get; set; }
        public string NameFriendly { get; set; }
        #endregion
    }

}

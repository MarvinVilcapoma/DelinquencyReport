namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class FINBankAccountModel
    {
        #region Public Property
        public int ID { get; set; }
        public int BankAccountID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? BankID { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public int? AccountID { get; set; }
        public string FinanceAccount { get; set; }
        #endregion
    }
}
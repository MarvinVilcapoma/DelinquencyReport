using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class FinSalesCashierDetailsModel
    {
        public List<FinSalesCashierDetailModel> SalesCashierDetais { get; set; }
        public FinSalesCashierModel SalesCashier { get; set; }
    }
    public class FinSalesCashierDetailModel
    {
        public int ID { get; set; }
        public int CashierID { get; set; }
        public int CompanyID { get; set; }
        public int PaymentOptionID { get; set; }
        public int? BankAccountID { get; set; }
        public int FinancePaymentAccountID { get; set; }
        public bool CreateDeposit { get; set; }
        public string FinancePaymentAccountName { get; set; }
    }
    public class FinSalesCashierModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SegmentID { get; set; }
        public int? AccountCodeID { get; set; }
        public List<FinSalesCashierDetailModel> SalesCashierDetailList { get; set; }
    }
}

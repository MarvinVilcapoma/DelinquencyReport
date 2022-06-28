using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class CreditHistoryList
    {
        public List<MUNAccountCreditHistoryGet_Result> CreditHistoryModelList { get; set; }
    }
    public class CreditHistoryModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal CreditAmount { get; set; }
        public string Reason { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AccountID { get; set; }
        public int? PaymentID { get; set; }
        public string PaymentNumber { get; set; }
        public string Number { get; set; }
        public int CompanyID { get; set; }
    }
}

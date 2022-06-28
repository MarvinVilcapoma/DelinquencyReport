using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class DebitHistoryList
    {
        public List<MUNAccountDebitHistoryGet_Result> DebitHistoryModelList { get; set; }
    }
    public class DebitHistoryModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal DebitAmount { get; set; }
        public string Reason { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int AccountID { get; set; }
        public int? PaymentID { get; set; }
        public string PaymentNumber { get; set; }
        public string Number { get; set; }
        public bool IsPaid { get; set; }
        public string AccountDisplayName { get; set; }
        public string ReceiptNumber { get; set; }
        public int ServiceTypeID { get; set; }
        public string PaymentPlanName { get; set; }
    }

    public class DebitHistoryDetailModel
    {
        public DebitHistoryModel DebitHistory { get; set; }
        public List<DebitHistoryPaidModel> DebitHistoryPaidList { get; set; }
    }

    public class DebitHistoryPaidModel
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool ISVoid { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceAdjustmentModel
    {
        public int AccountServiceId { get; set; }
        public string AccountServiceCollectionDetailId { get; set; }
        public int ServiceCollectionRuleId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        //public bool AdjustForAll { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ID { get; set; }
    }
}

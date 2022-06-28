using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceExemptionModel
    {
        public int ID { get; set; }
        public int AccountServiceId { get; set; }
        public string AccountServiceCollectionDetailId { get; set; }        
        public decimal Amount { get; set; }
        public string Reason { get; set; }       
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceExtensionModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
        public int Months { get; set; }        
        public decimal GrossVolume { get; set; }
        public decimal? ExemptionAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Total { get; set; }
        public int? ImageID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
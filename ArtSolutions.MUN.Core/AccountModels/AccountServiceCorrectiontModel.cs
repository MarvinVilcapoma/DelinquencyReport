using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceCorrectiontModel
    {
        public int AccountServiceId { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public decimal Amount { get; set; }
        public int PendingPeriod { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public int ExemptionPeriod { get; set; }
        public decimal ExemptionAmount { get; set; }
        public string Reason { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }        
        public string PropertyTaxJson { get; set; }
        public Nullable<int> MovementTypeID { get; set; }        
        public Nullable<decimal> Area { get; set; }        
        public Nullable<decimal> TerrainValue { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
    }
}

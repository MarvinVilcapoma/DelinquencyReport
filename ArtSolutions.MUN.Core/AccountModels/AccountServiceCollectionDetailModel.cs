using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountServiceCollectionDetailModel
    {
        public int FiscalYearID { get; set; }
        public string FiscalYearWithPeriod { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal Charges { get; set; }
        public decimal Interest { get; set; }
        public decimal Discount { get; set; }
        public decimal Balance { get; set; }
        public int AccountServiceID { get; set; }
        public decimal Principal1 { get; set; }
        public decimal Principal2 { get; set; }
        public string ItemName { get; set; }
        public string RightNumber { get; set; }
        public string ServiceName { get; set; }
        public DateTime DueDate { get; set; }
        public string ServiceNameWithYear { get; set; }
        public int FrequencyID { get; set; }
        public decimal? PreviousMeasure { get; set; }
        public decimal? ActualMeasure { get; set; }
        public decimal? WaterConsumption { get; set; }
        public int ServiceID { get; set; }
        public string ServiceCode { get; set; }

        public string MeterNumber { get; set; }
        public int SequenceID { get; set; }       
        public decimal PrincipalMinusDiscount { get; set; }

    }
}

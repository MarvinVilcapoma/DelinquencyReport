using System;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountPaymentPlanDetailModel
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal LateInterest { get; set; }
        public decimal Balance { get; set; }
        public decimal Payment { get; set; }
        public bool IsDownPayment { get; set; }
        public int Periods { get; set; }
        public string PaymentPlanNumber { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public decimal PreviousMeasure { get; set; }
        public decimal ActualMeasure { get; set; }
        public decimal WaterConsumption { get; set; }
        public string MeterNumber { get; set; }
        public Nullable<int> FiscalYearID { get; set; }
        public string FiscalYearWithPeriod { get; set; }
    }
}

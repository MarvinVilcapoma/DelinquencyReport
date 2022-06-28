using System;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCalculationDateModel
    {
        #region properties
        public int ID { get; set; }
        public int FiscalYearID { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }
        public string FillingDueDate { get; set; }
        public string PaymentDueDate { get; set; }
        public string PaymentGraceDate { get; set; }
        public string DiscountDate { get; set; }
        public int SequenceID { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        #endregion

        #region custom properties
        public string ValStartDate { get; set; }
        public string CurrentPeriodStartDate { get; set; }
        public string CurrentPeriodEndDate { get; set; }
        public bool IsEditable { get; set; }
        #endregion       
    }
    public enum EnDateTypes
    {
        FillingDueDates,
        PaymentDueDates,
        DiscountDates,
        PaymentGraceDates
    }
    public enum EnDueTypes
    {
        StartPeriodDate = 1,
        EndPeriodDate = 2,
        FixDays = 3,
        FixDate = 4
    }
}
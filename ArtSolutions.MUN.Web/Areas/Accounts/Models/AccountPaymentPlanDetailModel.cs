using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPaymentPlanDetailModel
    {
        #region public properties 
        public Nullable<int> ID { get; set; }
        public int SequenceID { get; set; }
        public System.DateTime DueDate { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal LateInterest { get; set; }
        public decimal AmnestyAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal Payment { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsDownPayment { get; set; }
        public int Periods { get; set; }
        public string PaymentPlanNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int AccountPaymentPlanID { get; set; }
        public int AccountPaymentPlanServiceDetailID { get; set; }
        public string ServiceName { get; set; }
        public string AccountServiceName { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> AccountServiceID { get; set; }
        public Nullable<int> AccountServiceCollectionDetailID { get; set; }
        public decimal? PreviousMeasure { get; set; }
        public decimal? ActualMeasure { get; set; }
        public decimal? WaterConsumption { get; set; }
        public string PaymentPlanName { get; set; }
        public Nullable<bool> IsInJudicialCollection { get; set; }
        #endregion

        #region custom properties     
        public string MonthlyDueDate { get; set; }
        public bool SelectedItem {get;set;}
        public Nullable<int> FiscalYearID { get; set; }
        public string MeterNumber { get; set; }
        public string FiscalYearWithPeriod { get; set; }        
        #endregion
    }
}
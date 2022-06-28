using ArtSolutions.MUN.Core.ReportModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountPaymentPlanModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        //public int ServiceTypeID { get; set; }
        public int ServicePaymentPlanID { get; set; }
        public string Number { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal InstalmentPercentage { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal LateInterestPercentage { get; set; }
        public int Months { get; set; }
        public bool IsEditable { get; set; }
        public decimal InstalmentAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDebt { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public string InactiveReason { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public string AccountPaymentPlanDetailJson { get; set; }
        public string AccountServiceCollectionDetailIDs { get; set; }
        public string ReasonForDeleted { get; set; }
        public bool ApplybyAmnesty { get; set; }
        public decimal AmnestyAmount { get; set; }
    }
    public class ListAccountPaymentPlanModel
    {
        public List<MUNSERAccountPaymentPlanGetWithPaging_Result> AccountPaymentPlanList { get; set; }
        public int TotalRecord { get; set; }
    }
    public class PaymentPlanExportList
    {
        public List<MUNSERREPPaymentPlanExport_Result> ExportPaymentPlanList { get; set; }
    }
    public class AccountPaymentPlanPrintModel
    {
        public ReportAccountModel AccountModel { get; set; }
        public AccountPaymentPlanModel AccountPaymentPlanModel { get; set; }
        public List<AccountPaymentPlanPrintDetailModel> AccountPaymentPlanList { get; set; }
    }
    public class AccountPaymentPlanPrintDetailModel
    {
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string Periods { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal LateInterest { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public DateTime StartDate { get; set; }
    }
   
}

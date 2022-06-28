using System;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class FinClassGrantModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGrant { get; set; }
        public Nullable<System.DateTime> InitialDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string SegmentID { get; set; }
        public Nullable<int> AccountCodeID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> GrantTypeID { get; set; }
        public Nullable<int> GrantTypeTableId14 { get; set; }
        public Nullable<int> GrantGroupID { get; set; }
        public Nullable<int> GrantFundBalanceID { get; set; }
        public Nullable<int> DueToOtherFundAccountID { get; set; }
        public Nullable<int> DueFromOtherFundAccountID { get; set; }
        public Nullable<int> FinanceTemplateID { get; set; }
        public Nullable<int> TotalGrant { get; set; }
        public int BudgetGrantClass { get; set; }
        public Nullable<int> Budgets { get; set; }
        public string ClassName { get; set; }
    }
}

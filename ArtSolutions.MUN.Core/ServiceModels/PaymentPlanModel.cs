using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class PaymentPlanModel
    {
        public int ID { get; set; }
        public string Locale { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        //public string ServiceTypeName { get; set; }
        //public int ServiceTypeID { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public System.DateTime EffectiveTo { get; set; }
        public decimal InstalmentPercentage { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal LateInterestPercentage { get; set; }
        public int Months { get; set; }
        public bool IsEditable { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
    public class PaymentPlanListModel
    {
        public List<MUNSERPaymentPlanGetWithPaging_Result> PaymentPlanList { get; set; }
        public int TotalRecord { get; set; }
    }
}

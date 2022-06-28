using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceList
    {
        public List<MUNSERServiceGetWithPaging_Result> ServiceModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }
    public class ServiceModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ServiceTypeID { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public int GroupID { get; set; }
        public string Code { get; set; }
        public int FilingTypeTableID { get; set; }
        public int FilingTypeID { get; set; }
        public bool UseFixedDueDate { get; set; }
        public Nullable<System.DateTime> FillingDueDate { get; set; }
        public Nullable<System.DateTime> PaymentDueDate { get; set; }
        public int FrequencyID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public System.DateTime EffectiveTo { get; set; }
        public Nullable<int> Sort { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<bool> UseFixedFillingDueDate { get; set; }
        public Nullable<int> FillingDueDays { get; set; }
        public Nullable<bool> UseFixedPaymentDueDate { get; set; }
        public Nullable<int> PaymentDueDays { get; set; }
        public Nullable<bool> UseFixedPaymentGracePeriod { get; set; }
        public Nullable<int> PaymentGracePeriod { get; set; }
        public Nullable<bool> UseFixedDiscountDate { get; set; }
        public Nullable<int> DiscountDueDays { get; set; }
        public List<ServiceCalculationDate> ServiceCalculationDateList { get; set; }
        public List<ServiceCollectionRuleModel> ServiceCollectionRuleList { get; set; }
        public List<ServiceCollectionRuleDetailModel> ServiceCollectionRuleDetailList { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTestMode { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsService { get; set; }
        public Nullable<int> MunicipalityID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> FinanceTemplate { get; set; }
        public bool ItemBuyed { get; set; }
        public bool ItemForSale { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public bool IsExempt { get; set; }
        public bool UseInventory { get; set; }
        public int FINItemID { get; set; }
        public int CollectionTemplateID { get; set; }
        public Nullable<int> PrintTemplateID { get; set; }
        public string ServiceExceptionListJSON { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomDateField { get; set; }
        public string TotalValueLabel { get; set; }        
        public bool AllowDuplicateCustomField1 { get; set; }
        public bool AllowDuplicateCustomField2 { get; set; }
        public bool AllowDuplicateCustomField3 { get; set; }
        public bool AllowDuplicateCustomDateField { get; set; }
        public Nullable<bool> AutoCreation { get; set; }
        public Nullable<bool> MultipleServicesPerYear { get; set; }
        public string AccountTypeIDs { get; set; }
        public Nullable<int> FilingFormID { get; set; }
        public Nullable<int> FilingDueType { get; set; }
        public Nullable<int> PaymentDueType { get; set; }
        public Nullable<int> DiscountDueType { get; set; }
        public Nullable<int> PaymentGraceType { get; set; }
        public string CustomField4 { get; set; }
        public bool AllowDuplicateCustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public bool AllowDuplicateCustomField5 { get; set; }
    }
    public class SalesPrintTemplateModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

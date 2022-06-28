using ArtSolutions.MUN.Web.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceModel
    {
        #region Properties
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int GroupID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int ServiceTypeID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int FrequencyID { get; set; }
        public int Period { get; set; }
        public string Service { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public int FilingTypeTableID { get; set; }
        public int FilingTypeID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public System.DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public System.DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public System.DateTime EffectiveFrom { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public System.DateTime EffectiveTo { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime InitialDate { get; set; }
        public System.DateTime FillingDate { get; set; }
        public System.DateTime PaymentDate { get; set; }
        [StringLength(500, ErrorMessageResourceName = "DescriptionLengthValidationMsg", ErrorMessageResourceType = typeof(Resources.Service))]
        public string Description { get; set; }
        public string Frequency { get; set; }
        public bool? UseFixedFillingDueDate { get; set; }
        public int? FillingDueDays { get; set; }
        public bool? UseFixedPaymentDueDate { get; set; }
        public int? PaymentDueDays { get; set; }
        public bool? UseFixedPaymentGracePeriod { get; set; }
        public int? PaymentGracePeriod { get; set; }
        public bool? UseFixedDiscountDate { get; set; }
        public int? DiscountDueDays { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTestMode { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> FinanceTemplate { get; set; }
        public int? CollectionTemplateID { get; set; }
        public string EffectiveFromByCulture { get; set; }
        public string EffectiveToByCulture { get; set; }
        public int? PrintTemplateID { get; set; }
        public string FilingType { get; set; }
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
        public bool? IsDateFieldCustomField1 { get; set; }
        public bool? MultipleServicesPerYearPerRight { get; set; }
        public string CustomField4 { get; set; }
        public bool AllowDuplicateCustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public bool AllowDuplicateCustomField5 { get; set; }
        #endregion

        #region custom properties
        public List<ServiceCollectionRuleModel> ServiceCollectionRuleList { get; set; }
        public List<ServiceCollectionRuleDetailModel> ServiceCollectionRuleDetailList { get; set; }
        public IEnumerable<SelectListItem> ServiceTypeGroupList { get; set; }
        public IEnumerable<SelectListItem> ServiceTypeList { get; set; }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
        public IEnumerable<SelectListItem> FilingTypeList { get; set; }
        public string ServiceCollectionRuleListJson { get; set; }
        public string ServiceCollectionRuleDetailListJson { get; set; }
        public List<ServiceCalculationDateModel> ServiceCalculationDateList { get; set; }
        public List<ServiceCalculationDateModel> ServiceCalculationDateList_Append { get; set; }
        public string ServiceCalculationDateListJson { get; set; }

        public Nullable<System.DateTime> PaymentGraceDate { get; set; }
        public Nullable<System.DateTime> DiscountDate { get; set; }
        public Nullable<System.DateTime> EffectiveTo_Original { get; set; }
        public IEnumerable<SelectListItem> CollectionTemplateList { get; set; }
        public string CollectionTemplateName { get; set; }
        public IEnumerable<SelectListItem> PrintTemplateList { get; set; }
        public string PrintTemplateName { get; set; }
        public string ServiceExceptionListJSON { get; set; }
        public IEnumerable<SelectListItem> AccountTypeList { get; set; }
        public IEnumerable<SelectListItem> FilingFormTypeList { get; set; }
        #endregion

        #region Constructor
        public ServiceModel()
        {
            ServiceCollectionRuleList = new List<ServiceCollectionRuleModel>();
            ServiceTypeList = new List<SelectListItem>();
            ServiceCollectionRuleDetailList = new List<ServiceCollectionRuleDetailModel>();
            ServiceCalculationDateList = new List<Models.ServiceCalculationDateModel>();
            ServiceCalculationDateList_Append = new List<ServiceCalculationDateModel>();
        }
        #endregion
    }
    public class ServiceList
    {
        #region Properties
        public IEnumerable<SelectListItem> ServiceSearch { get; set; }
        public List<ServiceModel> ServiceModelList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }
    public class SalesPrintTemplateModel
    {
        #region properties
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion
    }

    public class GrantModel
    {
        public int GrantID { get; set; }
        public string GrantName { get; set; }
    }

}
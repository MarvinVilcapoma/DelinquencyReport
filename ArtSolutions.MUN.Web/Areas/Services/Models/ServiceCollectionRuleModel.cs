using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCollectionRuleModel
    {
        #region Properties
        public int ID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Name { get; set; }
        public int ServiceID { get; set; }
        public int SequenceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int CollectionTypeID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int CollectionRuleID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int FrequencyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int CollectionFieldID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string EffectiveFrom { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string EffectiveTo { get; set; }
        public bool AlwaysApply { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool ApplyOnPaymentDueDate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }

        //18-Feb-2020
        public bool IsChanged { get; set; }
        public bool IsCopied { get; set; }
        //

        public bool IsDeleted { get; set; }
        public bool? IsNew { get; set; }
        public string Type { get; set; }
        public string OnField { get; set; }
        public string CalculationFrequency { get; set; }
        public string Rule { get; set; }
        public Nullable<System.Guid> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public bool? isAddedOrCopy { get; set; }

        public bool? IsEffectiveApply { get; set; }
        public int AllowEdit { get; set; }
        #endregion

        #region custom properties
        public string CollectionTypeName { get; set; }
        public string CollectionRuleName { get; set; }
        public string CollectionFieldName { get; set; }
        public string CollectionFrequencyName { get; set; }
        public IEnumerable<SelectListItem> CollectionTypeList { get; set; }
        public IEnumerable<SelectListItem> CollectionFieldList { get; set; }
        public IEnumerable<SelectListItem> CollectionRuleList { get; set; }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
        public List<ServiceCollectionRuleDetailModel> ServiceCollectionRuleDetailList { get; set; }
        #endregion

        #region Public Methods
        public bool IsServiceCollectionRuleExist(string collectionRuleName, int? id)
        {
            bool isExist = false;
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "collectionRuleName", Value = collectionRuleName });
                lstParameter.Add(new NameValuePair { Name = "id", Value = id });
                int data = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Service/IsServiceCollectionRuleExist", "GET", lstParameter);
                if (data > 0)
                    isExist = true;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }
        public void SetInitialDropDown()
        {
            this.CollectionTypeList = HMTLHelperExtensions.CreateSelectList(new CollectionTypeModel().Get(), "ID", "Name");
            this.CollectionFieldList = HMTLHelperExtensions.CreateSelectList(new CollectionFieldModel().Get(), "ID", "Name");
            this.CollectionRuleList = HMTLHelperExtensions.CreateSelectList(new CollectionRuleModel().Get(), "ID", "Name");
            this.FrequencyList = HMTLHelperExtensions.CreateSelectList(new FrequencyModel().Get(), "ID", "Name");          
            this.ServiceCollectionRuleDetailList = this.ServiceCollectionRuleDetailList == null ? new List<ServiceCollectionRuleDetailModel>() : this.ServiceCollectionRuleDetailList;
        }

        public List<ServiceCollectionRuleModel> Get(int serviceId, int? id,bool? isDeleted)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceId", Value = serviceId });
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });       
            lstParameter.Add(new NameValuePair { Name = "isDeleted", Value = isDeleted });
           return new RestSharpHandler().RestRequest<List<ServiceCollectionRuleModel>>(APISelector.Municipality, true, "api/Service/ServiceCollectionRuleGet", "GET", lstParameter);
        }
        #endregion
    }
}
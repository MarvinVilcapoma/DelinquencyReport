using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class Service
    {
        #region Public Methods
        public ServiceModel SetInitialDropDown(ServiceModel model)
        {
            model.ServiceTypeGroupList = HMTLHelperExtensions.CreateSelectList(new ServiceTypeGroupModel().GetServiceTypeGroup(), "ID", "Name");
            model.FrequencyList = HMTLHelperExtensions.CreateSelectList(new FrequencyModel().Get(true), "ID", "Name");
            model.CollectionTemplateList = HMTLHelperExtensions.CreateSelectList(new ServiceCollectionTemplateModel().Get(null, true, null), "ID", "Name");
            model.PrintTemplateList = HMTLHelperExtensions.CreateSelectList(new ServicePrintTemplateModel().Get(null, (int)EnumUtility.DocumentType.MunicipalityAccountService), "ID", "Name");
            if (model.GroupID > 0)
                model.ServiceTypeList = HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetServiceType(model.GroupID), "ID", "Name");
            model.AccountTypeList = HMTLHelperExtensions.CreateSelectList(new AccountTypeModel().GetAccountType(Common.CurrentApplication), "ID", "Name");
            model.FilingFormTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(35), "ID", "Name");
            return model;
        }

        public void SetInitialRadioButton(ServiceModel model)
        {
            model.FilingTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(18), "ID", "Name");
        }

        public bool IsServiceExist(string Name, int? Id)
        {
            bool isExist = false;
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "serviceName", Value = Name });
                lstParameter.Add(new NameValuePair { Name = "id", Value = Id });
                int data = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Service/IsServiceExist", "GET", lstParameter);
                if (data > 0)
                    isExist = true;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }

        public IEnumerable<SelectListItem> GetServiceTypeListByGroupId(int groupId)
        {
            return HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetServiceType(groupId), "ID", "Name");
        }

        public IEnumerable<SelectListItem> GetServiceListListByServiceTypeId(string serviceTypeIds)
        {
            List<ServiceModel> ServiceList = new Services.Models.Service().Get(null, null, null, false, null);
            if (!string.IsNullOrEmpty(serviceTypeIds))
            {
                List<int> ServiceTypeList = serviceTypeIds.Split(',').Select(d => int.Parse(d)).ToList();
                return HMTLHelperExtensions.CreateSelectList(ServiceList.Where(d => ServiceTypeList.FirstOrDefault(k => k == d.ServiceTypeID) > 0).ToList().ToList(), "ID", "Name");
            }
            return HMTLHelperExtensions.CreateSelectList(ServiceList, "ID", "Name");

        }

        public List<ServiceModel> Get(int? serviceTypeId, int? groupId, int? serviceId, bool isGetByEffectiveDate, string filingTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ServiceTypeID", Value = serviceTypeId });
            lstParameter.Add(new NameValuePair { Name = "GroupID", Value = groupId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = serviceId });
            lstParameter.Add(new NameValuePair { Name = "isGetByEffectiveDate", Value = isGetByEffectiveDate });
            lstParameter.Add(new NameValuePair { Name = "filingTypeID", Value = filingTypeID });
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/Get", "GET", lstParameter);
        }

        public List<ServiceModel> Get()
        {
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/Get", "GET", null);
        }

        public List<ServiceModel> GetForFACTU()
        {
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/GetForFACTU", "GET", null);
        }
        public List<ServiceModel> GetForCobros()
        {
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/GetForCobros", "GET", null);
        }
        public List<ServiceModel> GetForNoFiling()
        {
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/GetForNoFiling", "GET", null);
        }
        public List<ServiceModel> GetForFlexibleDatesAndFixDates(int? serviceTypeId, int? groupId, int? serviceId, bool isGetByEffectiveDate, int AccountTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ServiceTypeID", Value = serviceTypeId });
            lstParameter.Add(new NameValuePair { Name = "GroupID", Value = groupId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = serviceId });
            lstParameter.Add(new NameValuePair { Name = "isGetByEffectiveDate", Value = isGetByEffectiveDate });
            lstParameter.Add(new NameValuePair { Name = "AccountTypeID", Value = AccountTypeID });
            return new RestSharpHandler().RestRequest<List<ServiceModel>>(APISelector.Municipality, true, "api/Service/GetForFlexibleDatesAndFixDates", "GET", lstParameter);
        }

        public ServiceModel Get(int serviceId)
        {
            ServiceModel model = new ServiceModel();
            model = Get(null, null, serviceId, false, null).FirstOrDefault();
            if (model == null)
                throw new Exception(ArtSolutions.MUN.Web.Resources.Service.InvalidServiceNumber);
            ServiceCollectionTemplateModel collectionmodel = new ServiceCollectionTemplateModel().Get(model.CollectionTemplateID, true, null).FirstOrDefault();
            model.CollectionTemplateName = collectionmodel != null ? collectionmodel.Name : "";

            if (model.PrintTemplateID > 0)
            {
                ServicePrintTemplateModel printmodel = new ServicePrintTemplateModel().Get(model.PrintTemplateID, (int)EnumUtility.DocumentType.MunicipalityAccountService).FirstOrDefault();
                model.PrintTemplateName = printmodel != null ? printmodel.Name : "";
            }

            model.ServiceCollectionRuleList = new ServiceCollectionRuleModel().Get(serviceId, null,null);
            model.ServiceCollectionRuleDetailList = new ServiceCollectionRuleDetailModel().Get(model.ID, null, null,null);
            model.ServiceCalculationDateList = new ServiceCalculationDate().Get(model.ID, null);
            new ServiceCalculationDate().GenerateCalculationDateTable_ExistingDates(model);

            //Set Discount Percent 
            if (model.ServiceCalculationDateList.Count > 0 && model.UseFixedDiscountDate == false)
                model.DiscountPercentage = model.ServiceCalculationDateList[0].DiscountPercentage;
            // Set Exception
            List<ServiceExceptionModel> exceptionList = new ServiceExceptionModel().Get(null, model.ID);
            model.ServiceExceptionListJSON = JsonConvert.SerializeObject(exceptionList);
            model.AccountTypeList = HMTLHelperExtensions.CreateSelectList(new AccountTypeModel().GetAccountType(Common.CurrentApplication), "ID", "Name");
            model.FilingFormTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(35), "ID", "Name");
            return model;
        }

        public int Insert(ServiceModel model)
        {
            model.IsPublic = false;
            model.IsActive = true;
            model.IsDeleted = false;
            model.FilingTypeTableID = 18;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            CultureInfo ci = new CultureInfo(UserSession.Current.Culture);
            if (UserSession.Current.CountryID == 52)
            {
                ci.NumberFormat.CurrencyDecimalDigits = UserSession.Current.DecimalPoints;
                ci.NumberFormat.PercentDecimalDigits = UserSession.Current.DecimalPoints;
                ci.NumberFormat.CurrencyDecimalSeparator = ci.NumberFormat.NumberDecimalSeparator = ci.NumberFormat.PercentDecimalSeparator = ".";
                ci.NumberFormat.CurrencyGroupSeparator = ci.NumberFormat.NumberGroupSeparator = ci.NumberFormat.PercentGroupSeparator = ",";
            }

            model.ServiceCollectionRuleList = JsonConvert.DeserializeObject<List<ServiceCollectionRuleModel>>(model.ServiceCollectionRuleListJson, new JsonSerializerSettings
            {
                // culture separator is ","..
                Culture = ci //new System.Globalization.CultureInfo(UserSession.Current.Culture)
            });
            model.ServiceCollectionRuleList.ForEach(x =>
            {
                if (x.CollectionTypeID == 1) // Principal
                    x.AlwaysApply = true;
                else
                    x.AlwaysApply = false;
                if (x.ServiceCollectionRuleDetailList != null)
                    model.ServiceCollectionRuleDetailList.AddRange(x.ServiceCollectionRuleDetailList);
                //if (x.IsEffectiveApply == true) //code not required 
                //    x.EffectiveTo = DateTime.Now.AddDays(-1).ToString("d");
                UpdateCollectionRuleDateFormat(x);
            });

            var lstObj = new JavaScriptSerializer().Deserialize<List<ServiceCalculationDateModel>>(model.ServiceCalculationDateListJson);
            if (lstObj != null && lstObj.Count() > 0)
            {
                model.ServiceCalculationDateList = lstObj;
            }
            new ServiceCalculationDate().UpdateCalculationDateList(model);

            //Allow CustomField Duplicates
            model.AllowDuplicateCustomField1 = model.CustomField1 == null ? false : model.AllowDuplicateCustomField1;
            model.AllowDuplicateCustomField2 = model.CustomField2 == null ? false : model.AllowDuplicateCustomField2;
            model.AllowDuplicateCustomField3 = model.CustomField3 == null ? false : model.AllowDuplicateCustomField3;
            model.AllowDuplicateCustomField4 = model.CustomField4 == null ? false : model.AllowDuplicateCustomField4;
            model.AllowDuplicateCustomField5 = model.CustomField5 == null ? false : model.AllowDuplicateCustomField5;
            //

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Service/Insert", "POST", null, lstObjParameter);
        }

        public int Update(ServiceModel model)
        {
            model.IsPublic = false;
            model.IsActive = true;
            model.IsDeleted = false;
            model.FilingTypeTableID = 18;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            CultureInfo ci = new CultureInfo(UserSession.Current.Culture);
            if (UserSession.Current.CountryID == 52)
            {
                ci.NumberFormat.CurrencyDecimalDigits = UserSession.Current.DecimalPoints;
                ci.NumberFormat.PercentDecimalDigits = UserSession.Current.DecimalPoints;
                ci.NumberFormat.CurrencyDecimalSeparator = ci.NumberFormat.NumberDecimalSeparator = ci.NumberFormat.PercentDecimalSeparator = ".";
                ci.NumberFormat.CurrencyGroupSeparator = ci.NumberFormat.NumberGroupSeparator = ci.NumberFormat.PercentGroupSeparator = ",";
            }

            model.ServiceCollectionRuleList = JsonConvert.DeserializeObject<List<ServiceCollectionRuleModel>>(model.ServiceCollectionRuleListJson, new JsonSerializerSettings
            {
                // culture separator is ","..
                Culture = ci // new System.Globalization.CultureInfo(UserSession.Current.Culture)
            });
            model.ServiceCollectionRuleList.ForEach(x =>
            {
                //18-Feb-2020
                if (x.IsActive == false || (x.IsNew == true &&  x.IsCopied == false)) // Rule Deactivated Or New Rule Added
                {
                    x.IsCopied = false;
                    x.IsChanged = false;
                }
                //

                if (x.CollectionTypeID == 1) // Principal
                    x.AlwaysApply = true;
                else
                    x.AlwaysApply = false;
                if (x.ServiceCollectionRuleDetailList != null)
                    model.ServiceCollectionRuleDetailList.AddRange(x.ServiceCollectionRuleDetailList);
                //if (x.IsEffectiveApply == true) // code not required 
                //    x.EffectiveTo = DateTime.Now.AddDays(-1).ToString("d");
                UpdateCollectionRuleDateFormat(x);
            });

            var lstObj = new JavaScriptSerializer().Deserialize<List<ServiceCalculationDateModel>>(model.ServiceCalculationDateListJson);
            if (lstObj != null && lstObj.Count() > 0)
            {
                model.ServiceCalculationDateList = lstObj;
            }

            new ServiceCalculationDate().UpdateCalculationDateList(model);

            //Allow CustomField Duplicates
            model.AllowDuplicateCustomField1 = model.CustomField1 == null ? false : model.AllowDuplicateCustomField1;
            model.AllowDuplicateCustomField2 = model.CustomField2 == null ? false : model.AllowDuplicateCustomField2;
            model.AllowDuplicateCustomField3 = model.CustomField3 == null ? false : model.AllowDuplicateCustomField3;
            model.AllowDuplicateCustomField4 = model.CustomField4 == null ? false : model.AllowDuplicateCustomField4;
            model.AllowDuplicateCustomField5 = model.CustomField5 == null ? false : model.AllowDuplicateCustomField5;
            //

            //14-Feb-2020 - CO-1349 - For Wrong Data issue fix
            if (model.UseFixedFillingDueDate == null)
                model.FilingDueType = null;
            if (model.UseFixedPaymentDueDate == null)
                model.PaymentDueType = null;
            if (model.UseFixedDiscountDate == null)
                model.DiscountDueType = null;
            if (model.PaymentGracePeriod == null)
                model.PaymentGraceType = null;
            //

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Service/Update", "POST", null, lstObjParameter);
        }
        public ServiceModel Edit(int serviceId)
        {
            ServiceModel model = Get(serviceId);
            SetInitialDropDown(model);
            SetInitialRadioButton(model);
            model.ServiceCalculationDateListJson = new JavaScriptSerializer().Serialize(model.ServiceCalculationDateList);
            model.ServiceCollectionRuleListJson = new JavaScriptSerializer().Serialize(model.ServiceCollectionRuleList);
            model.ServiceCollectionRuleDetailListJson = new JavaScriptSerializer().Serialize(model.ServiceCollectionRuleDetailList);

            //Set Discount Percent 
            if (model.ServiceCalculationDateList.Count > 0 && model.UseFixedDiscountDate == false)
            {
                model.DiscountPercentage = model.ServiceCalculationDateList[0].DiscountPercentage;
            }
            return model;
        }

        public ServiceList Get(int? serviceID, string filtertext, int pageIndex, int pageSize, string sortColumn, string sortOrder, Nullable<int> GroupID, Nullable<int> ServiceTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceID", Value = serviceID });
            lstParameter.Add(new NameValuePair { Name = "filtertext", Value = filtertext });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = sortColumn });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            lstParameter.Add(new NameValuePair { Name = "groupID", Value = GroupID });
            lstParameter.Add(new NameValuePair { Name = "serviceTypeID", Value = ServiceTypeID });
            return new RestSharpHandler().RestRequest<ServiceList>(APISelector.Municipality, true, "api/Service/GetByPaging", "GET", lstParameter, null);
        }

        public List<GrantModel> GetGrantList()
        {
            return new RestSharpHandler().RestRequest<List<GrantModel>>(APISelector.Municipality, true, "api/Service/GetGrantList", "GET", null);
        }
        #endregion

        #region Private Methods
        private void UpdateCollectionRuleDateFormat(ServiceCollectionRuleModel model)
        {
            model.EffectiveFrom = DateTime.Parse(model.EffectiveFrom).ToString("d", CultureInfo.InvariantCulture);
            model.EffectiveTo = DateTime.Parse(model.EffectiveTo).ToString("d", CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
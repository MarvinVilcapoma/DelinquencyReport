using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceModel
    {
        #region public properties    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public int ServiceID { get; set; }
        public int GroupID { get; set; }
        public int ServiceTypeID { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string SSEIN { get; set; }
        public string LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public decimal AnnualIncome { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string ServiceName { get; set; }
        public List<ServiceTypeGroupModel> ServiceTypeGroupList { get; set; }
        public List<ServiceTypeModel> ServiceTypeList { get; set; }
        public List<ServiceModel> ServiceList { get; set; }
        public IEnumerable<SelectListItem> FiscalYearList { get; set; }
        public ServiceModel ServiceModel { get; set; }
        public List<AccountServiceFillingModel> Filling { get; set; }
        public AccountServiceCollectionDiscountModel Discount { get; set; }
        public AccountServiceDebtModel Debt { get; set; }
        public AccountServicePaymentHistoryModel PaymentHistory { get; set; }
        public List<AccountServiceNoteModel> NoteList { get; set; }
        public AccountServiceNoteModel Note { get; set; }
        public List<AccountServiceCollectionDebtModel> AdjustmentList { get; set; }
        public List<AccountServiceCollectionDebtModel> ExemptionList { get; set; }
        public List<AccountServicePaymentPlanModel> AccountPaymentPlanList { get; set; }
        public bool HasPrintTemplate { get; set; }
        public string AnnualIncomeWithCurrencyCode { get; set; }
        public Nullable<int> FINItemID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal AmountToPay { get; set; }
        public Nullable<int> IssueNumber { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public bool IsIssued { get; set; }
        public Nullable<System.Guid> IssuedBy { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public bool IsLock { get; set; }
        public string Notes { get; set; }
        public string RowVersion64
        {
            get
            {
                if (this.RowVersion != null)
                    return Convert.ToBase64String(this.RowVersion);

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.RowVersion = null;
                else
                    this.RowVersion = Convert.FromBase64String(value);
            }
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LockReasonTable12 { get; set; }
        public int? LockReasonTableValue { get; set; }
        public IEnumerable<SelectListItem> LockReasonList { get; set; }
        public int? LockActionTableValue { get; set; }
        public IEnumerable<SelectListItem> LockApplyList { get; set; }
        [StringLength(500, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string LockComment { get; set; }
        public int FrequencyID { get; set; }
        public string DiscountInfoMessage { get; set; }
        public bool IsAdjustable
        {
            get
            {
                if (Filling == null)
                    return false;
                else if (Filling.Where(x => x.Total > 0 && x.IsPayed == false).Count() > 0)// && Filling.FirstOrDefault(x => x.IsEditPermission == 1) != null)
                    return true;
                else
                    return false;
            }
        }
        public bool IsExtensionAllowed
        {
            get
            {
                if (Filling == null)
                    return false;
                else if (Filling.Where(x => x.SalesVolume > 0 && x.Total > 0 && x.FilingFormID == (int)EnFilingForm.PropertyTax).Count() > 0)// && Filling.FirstOrDefault(x => x.IsEditPermission == 1) != null)
                    return true;
                else
                    return false;
            }
        }
        public bool IsCorrectionAllowed
        {
            get
            {
                if (Filling == null || Filling.Where(x => x.PaymentPaidPeriod > 0).Count() == 0 && Filling.FirstOrDefault(x => x.FilingFormID == (int)EnFilingForm.PropertyTax) != null)// && Filling.FirstOrDefault(x => x.IsEditPermission == 1) != null)
                    return true;
                else
                    return false;
            }
        }

        public bool IsAllowedDelete
        {
            get
            {
                if (Filling == null || Filling.Where(x => x.PaymentPaidPeriod > 0).Count() == 0 && Filling.Where(x => x.IsEditPermission == 0).Count() <= 0)
                    return true;
                else
                    return false;
            }
        }
        public bool AllowExtension { get; set; }
        public bool AllowCollection { get; set; }
        public Nullable<System.DateTime> ExtensionEndDate { get; set; }
        public Nullable<int> ExtensionID { get; set; }
        public int? ServiceExceptionID { get; set; }
        public decimal? ServiceExceptionValue { get; set; }
        public List<ServiceExceptionModel> ServiceExceptionList { get; set; }
        public Nullable<int> AccountPaymentPlanID { get; set; }
        public System.DateTime? FilingDueDate { get; set; }
        public System.DateTime? PaymentDueDate { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public DateTime? CustomDateField { get; set; }
        public string SerCustomField1 { get; set; }
        public string SerCustomField2 { get; set; }
        public string SerCustomField3 { get; set; }
        public string SerCustomField4 { get; set; }
        public string SerCustomField5 { get; set; }
        public string SerCustomDateField { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField1NewValue { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField2NewValue { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField3NewValue { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField4NewValue { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string CustomField5NewValue { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime? CustomDateFieldNewValue { get; set; }
        public int FilingTypeID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int? AccountPropertyID { get; set; }
        public string Property { get; set; }
        public int? OwnerID { get; set; }
        public int? ServiceStartPeriod { get; set; }
        public int? LicenseAccountServiceID { get; set; }
        public decimal? PaidAmount { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
        public int? AccountServiceAccountPropertyID { get; set; }
        public string TotalValueLabel { get; set; }
        public string ReasonForDeleted { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public DateTime? CustomField1UpdateDate { get; set; }

        // Extra For Transfer Detail
        public string ServiceNameWithYear { get; set; }
        public int AccountServiceCollectionDetailId { get; set; }
        public decimal SalesVolume { get; set; }
        public decimal Principal { get; set; }
        public decimal Discounts { get; set; }
        public decimal Debts { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public int FiscalYearID { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public Nullable<int> FilingFormID { get; set; }
        public int SequenceID { get; set; }
        public Nullable<bool> IsTransferService { get; set; }
        public string TransferNotes { get; set; }
        public Nullable<bool> IsDeletedProperty { get; set; }
        public string GetFormattedCollectionRate
        {
            get
            {
                //if (this.CollectionRuleID == (int)EnCollectionRule.Fee || this.CollectionRuleID == (int)EnCollectionRule.RangeFee) // Flat Fee OR Range Flat Fee
                //{
                //    return this.Rate.ToString("C");
                //}
                //else // % OR Range% OR % Of %
                //{
                //    return this.Rate.ToString("0.#####") + "%";
                //}
                return (this.Rate.HasValue ? this.Rate.Value.ToString("0.#####") : 0.ToString("0.#####")) + "%";
            }
        }
        public bool? IsDateFieldCustomField1 { get; set; }
        public int? PeriodFrequencyID { get; set; }
        public bool? MultipleServicesPerYearPerRight { get; set; }
        public string CreatedBy { get; set; }
        public bool IsAccountServiceNote { get; set; }
        public decimal? PreviousMeasure { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public string BusinessDescription { get; set; }
        #endregion

        #region custom properties 
        public int FromAccountID { get; set; }
        public string FormattedAnnualIncome
        {
            get
            {
                return this.AnnualIncome.ToString("C");
                //return this.AnnualIncome != 0 ? this.AnnualIncome.ToString("C") : null;
            }
        }

        public string FormattedCreatedDate
        {
            get
            {
                return this.CreatedDate.ToString("d");
            }
        }

        public string FormattedModifiedDate
        {
            get
            {
                return this.ModifiedDate.ToString("d");
            }
        }
        public string FormattedCreatedUser
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.CreatedUserID.ToString()).FirstOrDefault();
                return model == null ? string.Empty : (string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName);
            }
        }
        public string FormattedFillingDueDate
        {
            get
            {
                return this.FilingDueDate.HasValue ? this.FilingDueDate.Value.ToString("d") : "";
            }
        }
        public string FormattedPaymentDueDate
        {
            get
            {
                return this.PaymentDueDate.HasValue ? this.PaymentDueDate.Value.ToString("d") : "";
            }
        }
        #endregion

        #region Constructor       
        public AccountServiceModel()
        {
            ServiceTypeList = new List<ServiceTypeModel>();
            ServiceList = new List<ServiceModel>();
            FiscalYearList = new List<SelectListItem>();
            Filling = new List<AccountServiceFillingModel>();
            Debt = new AccountServiceDebtModel();
            Discount = new AccountServiceCollectionDiscountModel();
            PaymentHistory = new AccountServicePaymentHistoryModel();
            NoteList = new List<AccountServiceNoteModel>();
            Note = new AccountServiceNoteModel();
            Note.AccountServiceID = ID;
            AdjustmentList = new List<AccountServiceCollectionDebtModel>();
            AccountPaymentPlanList = new List<AccountServicePaymentPlanModel>();
            LockReasonList = new List<SelectListItem>();
        }
        #endregion

        #region public methods  
        public List<AccountServiceModel> Get(int? accountserviceId, string filterText, int? accountId, int? fiscalYearID, bool? isLock)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceId", Value = accountserviceId });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "fiscalYearID", Value = fiscalYearID });
            lstParameter.Add(new NameValuePair { Name = "isLock", Value = isLock });
            return new RestSharpHandler().RestRequest<List<AccountServiceModel>>(APISelector.Municipality, true, "api/AccountService/Get", "GET", lstParameter);
        }

        public AccountServiceModel Get(int accountId, int accountTypeId, int serviceId, int accountserviceId, int? ownerID, int? accountPropertyID)
        {
            AccountServiceModel model = new AccountServiceModel();
            model.AccountTypeID = accountTypeId;

            if (accountId > 0 && serviceId >= 0)
            {
                model.AccountID = accountId;
                model.ServiceID = serviceId;
                model.AccountServiceAccountPropertyID = accountPropertyID;
                model.OwnerID = ownerID;
                model.ServiceTypeGroupList = new ServiceTypeGroupModel().GetServiceTypeGroup();
            }

            if (accountserviceId > 0)
            {
                List<AccountServiceModel> list = this.Get(accountserviceId, null, null, null, null);
                if (list.Count > 0)
                {
                    model = list.First();
                    AccountServiceFillingModel accountServiceFillingModel = new AccountServiceFillingModel();
                    model.Filling = accountServiceFillingModel.Get(model.ID, null, null);
                    model.Debt = new AccountServiceDebtModel(model.ID).Get();
                    model.Discount = new AccountServiceCollectionDiscountModel(model.ID).Get(); /*new AccountServiceCollectionDiscountModel().Get(model.ID, null);*/
                    model.PaymentHistory = new AccountServicePaymentHistoryModel(model.ID).Get();
                    model.NoteList = new AccountServiceNoteModel().Get(model.ID, null);
                    model.LockReasonList = HMTLHelperExtensions.CreateSelectList(GetLockReasons(), "ID", "Name");

                    model.LockApplyList = HMTLHelperExtensions.CreateSelectList(
                                                new AccountModel().GetSupportValues(47).Where(x => x.ID != 3).ToList(),
                                                "ID", "Name"); // Filter ID=3 - Block all type of services for this cedula [Reason = Only used for Judicial collection ,not required in UI,used for SP]

                    //model.HasPrintTemplate = new Service().Get(null, null, model.ServiceID, false, null).FirstOrDefault().PrintTemplateID == null ? false : true;
                    model.AdjustmentList = new AccountServiceAdjustmentModel().Get(model.ID) ?? new List<AccountServiceCollectionDebtModel>();
                    model.ExemptionList = new AccountServiceExemptionModel().GetExemptionList(model.ID) ?? new List<AccountServiceCollectionDebtModel>();
                    model.AccountPaymentPlanList = new AccountServicePaymentPlanModel().Get(null, null, accountserviceId) ?? new List<AccountServicePaymentPlanModel>();

                    //Discount infomation
                    model.DiscountInfoMessage = accountServiceFillingModel.ConfigureDiscountInfoMessage(model.Filling);
                    model.AllowExtension = accountServiceFillingModel.IsValidForExtension(model);
                    model.AllowCollection = new AccountServiceCorrectiontModel().ValidateUserPermissionForCorrection();
                }
            }
            return model;
        }

        public int Insert(AccountServiceModel model)
        {
            model.CreatedBy = UserSession.Current.Username;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/Insert", "POST", null, lstObjParameter);
        }

        public AccountServiceModel Update(bool isActive, int id, string Notes)
        {
            AccountServiceModel model = Get(id, null, null, null, null).FirstOrDefault();
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.Notes = Notes;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<AccountServiceModel>(APISelector.Municipality, true, "api/AccountService/Update", "POST", null, lstObjParameter);
        }

        public void Delete(int id, string Reason)
        {
            AccountServiceModel model = Get(id, null, null, null, null).FirstOrDefault();
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ReasonForDeleted = Reason;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/Delete", "POST", null, lstObjParameter);
        }
        public void DeleteNote(int id, string Reason, bool IsAccountServiceNote)
        {
            AccountServiceModel model = new AccountServiceModel();
            model.ID = id;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ReasonForDeleted = Reason;
            model.IsAccountServiceNote = IsAccountServiceNote;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteNote", "POST", null, lstObjParameter);
        }
        public int UpdateForCustomField(AccountServiceModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/UpdateForCustomField", "POST", null, lstObjParameter);
        }
        public int UpdateForRight(AccountServiceModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/UpdateForRight", "POST", null, lstObjParameter);
        }
        public int UpdateForLicenseNumber(AccountServiceModel model)
        {
            model.LicenseNumber = model.LicenseNumber == null ? "" : Regex.Replace(model.LicenseNumber, @"\s+", "");
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/UpdateForLicenseNumber", "POST", null, lstObjParameter);
        }
        public bool Issue(int id, byte[] original_rowversion)
        {
            AccountServiceModel obj = new AccountServiceModel();
            obj.ID = id;
            obj.IssueDate = Common.CurrentDateTime;
            obj.IssuedBy = UserSession.Current.Id;
            obj.ModifiedUserID = UserSession.Current.Id;
            obj.ModifiedDate = Common.CurrentDateTime;
            obj.RowVersion = original_rowversion;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(obj);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/Issue", "POST", null, lstObjParameter);
        }

        #region Lock
        private List<SupportValueModel> GetLockReasons()
        {
            return new AccountModel().GetSupportValues(12);
        }

        public bool Lock(AccountServiceModel model)
        {
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedBy = UserSession.Current.Username;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/Lock", "POST", null, lstObjParameter);
        }
        #endregion

        #endregion
    }
    public class AccountServiceList
    {
        #region public properties    
        public IEnumerable<SelectListItem> AccountServiceSearch { get; set; }
        public List<AccountServiceModel> AccountServiceModelList { get; set; }
        public List<SelectListItem> lstServiceType { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion

        #region public methods
        public AccountServiceList Get()
        {
            this.AccountServiceModelList = new List<AccountServiceModel>();
            this.lstServiceType = new ServiceTypeModel().GetServiceType(0).OrderBy(x => x.Name).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            this.lstServiceType.Insert(0, new SelectListItem { Text = Resources.Global.All, Value = "0" });
            return this;
        }
        public AccountServiceList Get(string filtertext, string ServiceTypeIDs, int? accountId, int? accountPropertyID, string serviceIDs, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            decimal outDecimal;
            if (Decimal.TryParse(filtertext, out outDecimal))
            {
                try
                {
                    filtertext = Convert.ToDecimal(filtertext).ToString(CultureInfo.InvariantCulture);
                }
                catch { }
            }

            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filtertext });
            lstParameter.Add(new NameValuePair { Name = "serviceTypeIDs", Value = ServiceTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "serviceIDs", Value = serviceIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = sortColumn == "FormattedAnnualIncome" ? "AnnualIncome" : sortColumn });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            return new RestSharpHandler().RestRequest<AccountServiceList>(APISelector.Municipality, true, "api/AccountService/GetByPaging", "GET", lstParameter, null);
        }


        public List<AccountServiceModel> GetAllAccountService(int accountId, int AccountPropertyID, int TransferTypeID, int TransferID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = AccountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "transferTypeID", Value = TransferTypeID });
            lstParameter.Add(new NameValuePair { Name = "transferID", Value = TransferID });
            return new RestSharpHandler().RestRequest<List<AccountServiceModel>>(APISelector.Municipality, true, "api/AccountService/GetAllAccountService", "GET", lstParameter, null);
        }

        public List<AccountServiceModel> GetAccountServiceForTimbre(int fiscalYearID, int accountID, int serviceID, int? accountPropertyID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "fiscalYearID", Value = fiscalYearID });
            lstParameter.Add(new NameValuePair { Name = "serviceID", Value = serviceID });
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID });
            return new RestSharpHandler().RestRequest<List<AccountServiceModel>>(APISelector.Municipality, true, "api/AccountService/GetAccountServiceForTimbre", "GET", lstParameter, null).ToList();
        }

        #endregion
    }
}
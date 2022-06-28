using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountModel
    {
        #region public properties       
        public int ID { get; set; }
        public string RegisterNumber { get; set; }
        public int AccountTypeID { get; set; }
        public int IsBusiness { get; set; }
        public Nullable<int> ParentID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string DisplayName { get; set; }
        public string PrintCheckAs { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string BusinessDescription { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> CurrencyID { get; set; }
        public string CurrencyISOCode { get; set; }
        public string Notes { get; set; }
        [Url(ErrorMessageResourceName = "ValWebsiteUrl", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Website { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        //[RegularExpression(@"^\d{3}-?\d{2}-?\d{4}$|^\d{2}-?\d{7}$|XXX-XX-?\d{4}$|^[\d-]{1,20}$", ErrorMessageResourceName = "ValSSEINNumber", ErrorMessageResourceType = typeof(Resources.Account))]
        public string TaxNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public string StateNumber { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public System.DateTime InitialDate { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int AccountID { get; set; }
        public int CompanyID { get; set; }
        public Nullable<Guid> InactiveByUserID { get; set; }
        public Nullable<DateTime> InactiveDate { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "ValidEmailAddressMsg", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string WorkPhone { get; set; }
        public string Mobile { get; set; }
        public List<EmailModel> EmailList { get; set; }
        public List<PhoneModel> PhoneList { get; set; }
        public List<AddressModel> AddressList { get; set; }
        public List<ContactsModel> ContactList { get; set; }
        public List<AttachmentModel> FileList { get; set; }
        public List<AccountBusinessModel> BusinessList { get; set; }
        public List<AccountServiceModel> LicenceList { get; set; }
        public List<AccountServiceModel> IVUList { get; set; }
        public List<AccountServiceModel> ServiceList { get; set; }
        public List<AccountPaymentPlanModel> AccountPaymentPlanList { get; set; }
        public AccountPropertyList PropertyList { get; set; }
        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public IEnumerable<SelectListItem> SubAccountList { get; set; }
        public IEnumerable<SelectListItem> CollectionTemplateList { get; set; }
        public IEnumerable<SelectListItem> NAICSCodeList { get; set; }
        public IEnumerable<SelectListItem> BusinessTypeList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> SalutionList { get; set; }
        public IEnumerable<SelectListItem> SuffixList { get; set; }
        //public IEnumerable<SelectListItem> IDTypeList { get; set; }
        public List<SupportValueModel> IDTypeList { get; set; }
        public AddressModel AddressModel { get; set; }
        public CurrencyModel CurrencyModel { get; set; }
        public GeneralBusinessModel GeneralIndBusinessModel { get; set; }
        public GeneralIndividualModel GeneralIndividualModel { get; set; }
        public RegistrationBusinessModel RegistrationBusinessModel { get; set; }
        public string EmailJson { get; set; }
        public string PhoneJson { get; set; }
        public string AddressJson { get; set; }
        public string ContactJson { get; set; }
        public string FileJson { get; set; }
        public string MobilePhone { get; set; }
        public int ServiceId { get; set; }
        [Display(Name = "IsSponsor", ResourceType = typeof(Resources.Account))]
        public bool IsSponsor { get; set; }

        [Display(Name = "ExemptFromIVA", ResourceType = typeof(Resources.Account))]
        public bool ExemptFromIVA { get; set; }
        public Nullable<int> AccountPaymentPlanID { get; set; }
        public List<InvoiceModel> OtherServiceList { get; set; }
        public int AccountType { get; set; }
        public string ReferenceID { get; set; }
        public string BanacioIdentifica { get; set; }
        public string AccountTypeName { get; set; }
        public string IDType { get; set; }
        public bool? IsInJudicialCollection { get; set; }
        public bool IsNeedTempRight { get; set; }
        public string CreatedBy { get; set; }
        public bool AllowAddInJudicialCollection { get; set; }
        public bool AllowRemoveFromJudicialCollection { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal DebitBalance { get; set; }
        #endregion

        #region Constructor
        public AccountModel()
        {
            AddressList = new List<AddressModel>();
            ContactList = new List<ContactsModel>();
            FileList = new List<AttachmentModel>();
            CollectionTemplateList = new List<SelectListItem>();
            SubAccountList = new List<SelectListItem>();
        }
        #endregion

        #region Public Methods         
        public List<AccountModel> Get(string id, int? accountType, string displayName, string taxNumber, string phoneNumber, string filterText, bool? isActive)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "accountType", Value = accountType });
            lstParameter.Add(new NameValuePair { Name = "displayName", Value = string.IsNullOrEmpty(displayName) ? null : displayName });
            lstParameter.Add(new NameValuePair { Name = "taxNumber", Value = string.IsNullOrEmpty(taxNumber) ? null : taxNumber });
            lstParameter.Add(new NameValuePair { Name = "phoneNumber", Value = string.IsNullOrEmpty(phoneNumber) ? null : phoneNumber });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
            return new RestSharpHandler().RestRequest<List<AccountModel>>(APISelector.Municipality, true, "api/Account/Get", "GET", lstParameter);
        }
        public List<AccountSearchModel> GetForSearch(string accountTypeIDs, int? accountID, string searchText, bool? isActive = true)
        {
            List<AccountSearchModel> accountlist = new List<AccountSearchModel>();
            AccountListForSearch list = new AccountListForSearch().Get(accountTypeIDs, accountID, searchText, isActive, 1, Int32.MaxValue);
            if (list != null && list.AccountList.Count > 0)
                accountlist = list.AccountList;
            return accountlist;
        }
        public AccountModel Get(int accountType, int? parentAccountId = null)
        {
            AccountModel model = new AccountModel();
            model.AccountTypeID = Convert.ToInt32(accountType);
            if (parentAccountId.HasValue) model.ParentID = parentAccountId.Value;
            model = model.SetInitialDropDown(model);
            return model;
        }

        public string GetAccountForExist(int accountType, string taxNumber, string treasuryNumber, string stateNumber, int? id)
        {
            string result = null;

            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "taxNumber", Value = taxNumber.Replace("-", "") });
            lstParameter.Add(new NameValuePair { Name = "treasuryNumber", Value = treasuryNumber });
            lstParameter.Add(new NameValuePair { Name = "stateNumber", Value = stateNumber });
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            string resultStr = new RestSharpHandler().RestRequest<object>(APISelector.Municipality, true, "api/Account/GetAccountForExist", "GET", lstParameter).ToString();

            if (!string.IsNullOrEmpty(resultStr))
            {
                if (resultStr.Contains("1"))
                {
                    result += (accountType == 4 ? Resources.Global.SocialSecurity : Resources.Account.EIN);
                }
                if (resultStr.Contains("2"))
                {
                    result = (string.IsNullOrEmpty(result) ? "" : result + " & ") + Resources.Account.StateDepartmentRegisterNumber;
                }
                if (resultStr.Contains("3"))
                {
                    result = (string.IsNullOrEmpty(result) ? "" : result + " & ") + Resources.Account.TreasuryDepartment;
                }
                return result += " " + Resources.Account.AlreadyExistMsg.ToLower();
            }
            else
                return result;
        }
        public List<SupportValueModel> GetSupportValues(int tableId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "TableID", Value = tableId });
            return new RestSharpHandler().RestRequest<List<SupportValueModel>>(APISelector.Municipality, true, "api/Account/SupportTableValueGet", "GET", lstParameter);
        }

        public AccountModel GetIndividual(int accountID)
        {
            AccountIndividualModel model = new AccountIndividualModel().Get(accountID);
            AccountModel Model = model;
            Model.GeneralIndividualModel = new GeneralIndividualModel(model);
            return Model;
        }

        public AccountModel GetIndividualForEdit(int accountID)
        {
            AccountIndividualModel model = new AccountIndividualModel().GetForEdit(accountID);
            AccountModel Model = model;
            Model.GeneralIndividualModel = new GeneralIndividualModel(model);
            return Model;
        }

        public AccountModel GetBusiness(int accountID)
        {
            AccountBusinessModel model = new AccountBusinessModel().Get(accountID);
            AccountModel Model = model;
            Model.GeneralIndBusinessModel = new GeneralBusinessModel(model);
            Model.RegistrationBusinessModel = new RegistrationBusinessModel(model);
            return Model;
        }

        public AccountModel GetBusinessForEdit(int accountID)
        {
            AccountBusinessModel model = new AccountBusinessModel().GetForEdit(accountID, null).FirstOrDefault();
            AccountModel Model = model;
            Model.GeneralIndBusinessModel = new GeneralBusinessModel(model);
            Model.RegistrationBusinessModel = new RegistrationBusinessModel(model);
            return Model;
        }

        public AccountModel View(int accountID, int accountType)
        {
            AccountModel model = new AccountModel();
            if (accountType == 4)
            {
                model = GetIndividual(accountID);
                model.BusinessList = new AccountBusinessModel().Get(null, model.ID, null);
            }
            AccountServiceModel accountServiceModel = new AccountServiceModel();
            if (accountType == 5)
            {
                model = GetBusiness(accountID);
            }
            model.AccountID = model.ID;
            model = model.SetInitialDropDown(model);
            model.EmailList = new EmailModel().Get(null, accountID);
            model.PhoneList = new PhoneModel().Get(null, accountID);
            model.AddressList = new AddressModel().Get(null, accountID);
            model.ContactList = new ContactsModel().Get(null, accountID);
            model.FileList = new AttachmentModel().Get(accountID);
            model.EmailJson = new JavaScriptSerializer().Serialize(model.EmailList);
            model.PhoneJson = new JavaScriptSerializer().Serialize(model.PhoneList);
            model.AddressJson = new JavaScriptSerializer().Serialize(model.AddressList);
            model.ContactJson = new JavaScriptSerializer().Serialize(model.ContactList);
            model.FileJson = new JavaScriptSerializer().Serialize(model.FileList);
            model.AllowAddInJudicialCollection = ValidateUserPermissionForAddInJudicialCollection();
            model.AllowRemoveFromJudicialCollection = ValidateUserPermissionForRemoveFromJudicialCollection();
            return model;
        }

        public AccountModel Edit(int accountID, int accountType)
        {
            AccountModel model = new AccountModel();
            if (accountType == 4)
            {
                model = GetIndividualForEdit(accountID);
                model.BusinessList = new AccountBusinessModel().GetForEdit(null, model.ID);
            }
            AccountServiceModel accountServiceModel = new AccountServiceModel();
            if (accountType == 5)
            {
                model = GetBusinessForEdit(accountID);
            }
            model.AccountID = model.ID;
            model.ServiceList = accountServiceModel.Get(0, null, accountID, null, null);
            model = model.SetInitialDropDown(model);

            model.EmailList = new EmailModel().Get(null, accountID);
            model.PhoneList = new PhoneModel().Get(null, accountID);
            model.AddressList = new AddressModel().Get(null, accountID);
            model.ContactList = new ContactsModel().Get(null, accountID);
            model.FileList = new AttachmentModel().Get(accountID);
            model.EmailJson = new JavaScriptSerializer().Serialize(model.EmailList);
            model.PhoneJson = new JavaScriptSerializer().Serialize(model.PhoneList);
            model.AddressJson = new JavaScriptSerializer().Serialize(model.AddressList);
            model.ContactJson = new JavaScriptSerializer().Serialize(model.ContactList);
            model.FileJson = new JavaScriptSerializer().Serialize(model.FileList);
            return model;
        }

        public int Insert(AccountModel model)
        {
            model.CurrencyISOCode = model.CurrencyISOCode.Split('-')[0];
            if (model.CurrencyID != 7)
                model.TaxNumber = model.TaxNumber.Replace("-", "");

            if (model.AccountTypeID == 4)
                return new AccountIndividualModel().InsertIndividual(model, model.AccountTypeID);
            else if (model.AccountTypeID == 5)
                return new AccountBusinessModel().InsertBusiness(model, model.AccountTypeID);
            return 0;
        }

        public int Update(AccountModel model)
        {
            model.CurrencyISOCode = model.CurrencyISOCode.Split('-')[0];
            if (model.CurrencyID != 7)
                model.TaxNumber = model.TaxNumber.Replace("-", "");

            if (model.AccountTypeID == 4)
                return new AccountIndividualModel().UpdateIndividual(model);
            if (model.AccountTypeID == 5)
                return new AccountBusinessModel().UpdateBusiness(model);
            return 0;
        }

        public AccountModel UpdateStatus(bool isActive, int id)
        {
            AccountModel model = Get(id.ToString(), null, null, null, null, null, null).FirstOrDefault();
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<AccountModel>(APISelector.Municipality, true, "api/Account/Update", "POST", null, lstObjParameter);
        }
        public bool? IsSponsorByAccount(int accountID, int? accountType)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "accountType", Value = accountType });
            return new RestSharpHandler().RestRequest<Nullable<bool>>(APISelector.Municipality, true, "api/Account/IsSponsorByAccount", "GET", lstParameter, null);
        }
        public bool ValidateUserPermissionForAddInJudicialCollection()
        {
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Account/ValidateUserPermissionForAddInJudicialCollection", "GET", null, null);
        }
        public bool ValidateUserPermissionForRemoveFromJudicialCollection()
        {
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Account/ValidateUserPermissionForRemoveFromJudicialCollection", "GET", null, null);
        }
        public bool UpdateForJudicialCollection(AccountModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedBy = UserSession.Current.Username;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Account/UpdateForJudicialCollection", "POST", null, lstObjParameter);
        }
        #endregion

        #region Private Methods       
        private AccountModel SetInitialDropDown(AccountModel model)
        {
            model.CurrencyList = HMTLHelperExtensions.CreateSelectList(GetCurrencies(), "ID", "Currency", model.CurrencyID, false, false, null, true);

            //model.SubAccountList = HMTLHelperExtensions.CreateSelectList(GetSubAccounts(), "ID", "DisplayName", model.ParentID, true, true, Global.DropDownSelectMessage).OrderBy(x => x.Text).ToList();
            if (model.ID > 0 && model.ParentID > 0)
                model.SubAccountList = HMTLHelperExtensions.CreateSelectList(GetSubAccounts(model.ParentID.Value), "id", "text", model.ParentID, true, true, Global.DropDownSelectMessage);

            //model.CollectionTemplateList = HMTLHelperExtensions.CreateSelectList(new ServiceCollectionTemplateModel().Get(null, true, null), "ID", "Name");

            if (model.AccountTypeID == 4)
            {
                model.SalutionList = HMTLHelperExtensions.CreateSelectList(GetSalutations(), "ID", "Name");
                model.SuffixList = HMTLHelperExtensions.CreateSelectList(GetSuffixes(), "ID", "Name");
                //model.IDTypeList = HMTLHelperExtensions.CreateSelectList(GetIDTypeList(), "ID", "Name");
                model.IDTypeList = GetIDTypeList().OrderBy(a=>a.Description).ToList();
                //model.FinanceTemplateList = HMTLHelperExtensions.CreateSelectList(new FinanceTemplateModel().Get(null, null).Where(x => x.AccountTypeID == 4).ToList(), "ID", "Name");
            }
            if (model.AccountTypeID == 5)
            {
                model.NAICSCodeList = HMTLHelperExtensions.CreateSelectList(GetNAICSCodes(), "ID", "Name").OrderBy(x => x.Text).ToList();
                model.BusinessTypeList = HMTLHelperExtensions.CreateSelectList(GetBusinessTypes(), "ID", "Name").OrderBy(x => x.Text).ToList();
                model.ZoneList = HMTLHelperExtensions.CreateSelectList(GetZones(), "ID", "Name");
                //model.FinanceTemplateList = HMTLHelperExtensions.CreateSelectList(new FinanceTemplateModel().Get(null, null).Where(x => x.AccountTypeID == 5).ToList(), "ID", "Name");
            }
            return model;
        }
        private List<SupportValueModel> GetSalutations()
        {
            return GetSupportValues(2);
        }
        private List<SupportValueModel> GetNAICSCodes()
        {
            List<SupportValueModel> listNAICS = GetSupportValues(8);
            listNAICS = (from c in listNAICS
                         select new SupportValueModel
                         {
                             ID = c.ID,
                             Name = string.Format("{0} - {1}", c.ID, c.Name)
                         }).ToList();
            return listNAICS;
        }
        private List<SupportValueModel> GetSuffixes()
        {
            return GetSupportValues(3);
        }
        private List<SupportValueModel> GetIDTypeList()
        {
            return GetSupportValues(41);
        }
        private List<SupportValueModel> GetBusinessTypes()
        {
            return GetSupportValues(9);
        }
        private List<SupportValueModel> GetZones()
        {
            return GetSupportValues(11);
        }
        private List<CurrencyModel> GetCurrencies()
        {
            return new RestSharpHandler().RestRequest<List<CurrencyModel>>(APISelector.Municipality, true, "api/Account/CompanyCurrencyGet", "GET", null);
        }
        private List<AccountSearchModel> GetSubAccounts(int parentID)
        {
            return GetForSearch("4", parentID, null);
        }
        #endregion
    }
    public class AccountList
    {
        #region public properties         
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> AccountTypeList { get; set; }
        public List<AccountModel> AccountModelList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AccountList()
        {
            AccountModelList = new List<AccountModel>();
            StatusList = new List<SelectListItem>();
        }
        #endregion

        #region Public Methods
        public AccountList Get()
        {
            AccountList model = new AccountList();

            model.StatusList.Insert(0, new SelectListItem { Text = Resources.Global.All, Value = "0" });
            model.StatusList.Insert(1, new SelectListItem { Text = Resources.Global.Active, Value = "1", Selected = true });
            model.StatusList.Insert(2, new SelectListItem { Text = Resources.Global.InActivebtn, Value = "2" });

            model.AccountTypeList = HMTLHelperExtensions.CreateSelectList(new AccountTypeModel().GetAccountType(Common.CurrentApplication).Where(x => x.ID == 4 || x.ID == 5).ToList(), "ID", "Name");
            model.AccountTypeList.Insert(0, new SelectListItem { Text = Resources.Global.All, Value = "0" });
            return model;
        }
        public AccountList Get(string accountTypeID, bool? status, string filtertext, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "status", Value = status });
            lstParameter.Add(new NameValuePair { Name = "accountTypeID", Value = accountTypeID });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filtertext });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = sortColumn });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            return new RestSharpHandler().RestRequest<AccountList>(APISelector.Municipality, true, "api/Account/GetByPaging", "GET", lstParameter, null);
        }
        #endregion
    }
    public class AccountListForSearch
    {
        #region public properties                    
        public List<AccountSearchModel> AccountList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AccountListForSearch()
        {
            AccountList = new List<AccountSearchModel>();
        }
        #endregion

        #region Public Methods     
        public AccountListForSearch Get(string accountTypeIDs, int? accountID, string searchText, bool? isActive, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountTypeIDs", Value = accountTypeIDs });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "searchText", Value = searchText });
            lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<AccountListForSearch>(APISelector.Municipality, true, "api/Account/GetForSearch", "GET", lstParameter);
        }
        #endregion
    }
    public class AccountSearchModel
    {
        #region Public Properties
        public int id { get; set; }
        public string text { get; set; }
        #endregion
    }
    public class AccountExportModel
    {
        public string AccountTypeName { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ServiceName { get; set; }
        public int ID { get; set; }
        public bool PaymentStatus { get; set; }
    }
    public class AccountListExport
    {
        public List<AccountExportModel> ExportAccountList { get; set; }
        #region Public Methods
        public AccountListExport GetAccountListExport()
        {
            AccountListExport model = new AccountListExport();
            model = new RestSharpHandler().RestRequest<AccountListExport>(APISelector.Municipality, true, "api/Account/GetExportAccountList", "GET", null, null);
            return model;
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountBusinessModel : AccountModel
    {
        #region public properties         
        public string LegalName { get; set; }
        public string DBAName { get; set; }
        public Nullable<int> NAICSCodeID { get; set; }
        public Nullable<int> NAICSCodeIDTable { get; set; }
        public Nullable<int> BusinessTypeID { get; set; }
        public Nullable<int> BusinessTypeTableID { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public Nullable<int> ZoneTableID { get; set; }
        public string ARPEDescription { get; set; }
        public string NAICSCode { get; set; }
        public string BusinessType { get; set; }
        public string Zone { get; set; }
        public string BusinessAccountName { get; set; }
        #endregion

        #region Constructor
        public AccountBusinessModel() { }
        public AccountBusinessModel(AccountModel model)
        {
            RegisterNumber = model.RegisterNumber;
            AccountTypeID = model.AccountTypeID;
            IsBusiness = model.IsBusiness;
            ParentID = model.ParentID;
            DisplayName = model.DisplayName;
            PrintCheckAs = model.PrintCheckAs;
            BusinessDescription = model.BusinessDescription;
            CurrencyID = model.CurrencyID;
            CurrencyISOCode = model.CurrencyISOCode;
            //FinanceTemplateID = model.FinanceTemplateID;
            Notes = model.Notes;
            BanacioIdentifica = model.BanacioIdentifica;
            Website = model.Website;
            TaxNumber = model.TaxNumber;
            TreasuryNumber = model.TreasuryNumber;
            StateNumber = model.StateNumber;
            InitialDate = model.InitialDate;
            IsActive = model.IsActive;
            CreatedUserID = UserSession.Current.Id;
            CreatedDate = Common.CurrentDateTime;
            ModifiedUserID = UserSession.Current.Id;
            ModifiedDate = Common.CurrentDateTime;
            LegalName = model.GeneralIndBusinessModel.LegalName;
            DBAName = model.GeneralIndBusinessModel.DBAName;
            if (model.RegistrationBusinessModel != null)
            {
                NAICSCodeID = model.RegistrationBusinessModel.NAICSCodeID;
                NAICSCodeIDTable = 8;
                BusinessTypeID = model.RegistrationBusinessModel.BusinessTypeID;
                BusinessTypeTableID = 9;
                ZoneID = model.RegistrationBusinessModel.ZoneID;
                ZoneTableID = 11;
                ARPEDescription = model.RegistrationBusinessModel.ARPEDescription;
            }
            IsSponsor = model.IsSponsor;
            ExemptFromIVA = model.ExemptFromIVA;
            EmailList = model.EmailJson == null ? new List<EmailModel>() : new JavaScriptSerializer().Deserialize<List<EmailModel>>(model.EmailJson);
            PhoneList = model.PhoneJson == null ? new List<PhoneModel>() : new JavaScriptSerializer().Deserialize<List<PhoneModel>>(model.PhoneJson);
            AddressList = model.AddressJson == null ? new List<AddressModel>() : new JavaScriptSerializer().Deserialize<List<AddressModel>>(model.AddressJson);
            ContactList = model.ContactJson == null ? new List<ContactsModel>() : new JavaScriptSerializer().Deserialize<List<ContactsModel>>(model.ContactJson);
            FileList = model.FileJson == null ? new List<AttachmentModel>() : new JavaScriptSerializer().Deserialize<List<AttachmentModel>>(model.FileJson);
        }
        #endregion

        #region public methods             
        public List<AccountBusinessModel> Get(int? accountID, int? parentId, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "parentId", Value = parentId });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<AccountBusinessModel>>(APISelector.Municipality, true, "api/Account/BusinessGet", "GET", lstParameter);
        }
        public List<AccountBusinessModel> GetForEdit(int? accountID, int? parentId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "parentId", Value = parentId });
            return new RestSharpHandler().RestRequest<List<AccountBusinessModel>>(APISelector.Municipality, true, "api/Account/BusinessGetForEdit", "GET", lstParameter);
        }
        public AccountBusinessModel Get(int accountID)
        {
            List<AccountBusinessModel> list = Get(accountID, null, null);
            if (list == null || list.Count == 0)
                throw new Exception(ArtSolutions.MUN.Web.Resources.Account.InvalidAccountNumber);
            return list.First();
        }
        public int InsertBusiness(AccountModel model, int accountTypeId)
        {
            model.AccountTypeID = accountTypeId;
            model.IsBusiness = 1;
            model.PrintCheckAs = "aaa";
            model.IsActive = true;

            AccountBusinessModel Model = new AccountBusinessModel(model);
            if (Model.BusinessTypeID == null)
                Model.BusinessTypeTableID = null;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(Model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/BusinessInsert", "POST", null, lstObjParameter);
        }
        public int UpdateBusiness(AccountModel model)
        {
            AccountBusinessModel Model = new AccountBusinessModel(model);
            Model.AccountID = model.AccountID;
            Model.InactiveByUserID = null;
            Model.InactiveDate = null;
            Model.IsBusiness = 1;
            Model.RowVersion = model.RowVersion;

            if (Model.BusinessTypeID == null)
                Model.BusinessTypeTableID = null;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(Model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/BusinessUpdate", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
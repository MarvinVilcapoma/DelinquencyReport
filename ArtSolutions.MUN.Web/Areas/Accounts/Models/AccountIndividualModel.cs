using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountIndividualModel : AccountModel
    {
        #region public properties        
        public Nullable<int> SalutationID { get; set; }
        public Nullable<int> SalutationTableID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public Nullable<int> SuffixID { get; set; }
        public Nullable<int> SuffixTableID { get; set; }
        public int? IDTypeID { get; set; }
        public int? IDTypeIDTableID { get; set; }
        public string IDType { get; set; }
        #endregion

        #region Constructor
        public AccountIndividualModel() { }
        public AccountIndividualModel(AccountModel model)
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
            SalutationID = model.GeneralIndividualModel.SalutationID;
            SalutationTableID = 2;
            FirstName = model.GeneralIndividualModel.FirstName;
            MiddleName = model.GeneralIndividualModel.MiddleName;
            LastName = model.GeneralIndividualModel.LastName;
            SecondLastName = model.GeneralIndividualModel.SecondLastName;
            SuffixID = Convert.ToInt32(model.GeneralIndividualModel.SuffixID);
            SuffixTableID = 3;
            IsSponsor = model.IsSponsor;
            ExemptFromIVA = model.ExemptFromIVA;
            IDTypeID = model.GeneralIndividualModel.IDTypeID;
            IDTypeIDTableID = 41;
            EmailList = model.EmailJson == null ? new List<EmailModel>() : new JavaScriptSerializer().Deserialize<List<EmailModel>>(model.EmailJson);
            PhoneList = model.PhoneJson == null ? new List<PhoneModel>() : new JavaScriptSerializer().Deserialize<List<PhoneModel>>(model.PhoneJson);
            AddressList = model.AddressJson == null ? new List<AddressModel>() : new JavaScriptSerializer().Deserialize<List<AddressModel>>(model.AddressJson);
            ContactList = model.ContactJson == null ? new List<ContactsModel>() : new JavaScriptSerializer().Deserialize<List<ContactsModel>>(model.ContactJson);
            FileList = model.FileJson == null ? new List<AttachmentModel>() : new JavaScriptSerializer().Deserialize<List<AttachmentModel>>(model.FileJson);
        }
        #endregion

        #region public methods     
        public AccountIndividualModel Get(int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountID });
            return new RestSharpHandler().RestRequest<AccountIndividualModel>(APISelector.Municipality, true, "api/Account/IndividualGet", "GET", lstParameter);
        }
        public AccountIndividualModel GetForEdit(int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountID });
            return new RestSharpHandler().RestRequest<AccountIndividualModel>(APISelector.Municipality, true, "api/Account/IndividualGetForEdit", "GET", lstParameter);
        }
        public int InsertIndividual(AccountModel model, int accountTypeId)
        {
            model.AccountTypeID = accountTypeId;
            model.IsBusiness = 0;
            model.PrintCheckAs = "aaa";
            model.IsActive = true;

            AccountIndividualModel Model = new AccountIndividualModel(model);
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(Model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/IndividualInsert", "POST", null, lstObjParameter);
        }
        public int UpdateIndividual(AccountModel model)
        {
            AccountIndividualModel Model = new AccountIndividualModel(model);
            Model.AccountID = model.AccountID;
            Model.InactiveByUserID = null;
            Model.InactiveDate = null;
            model.IsBusiness = 0;
            Model.RowVersion = model.RowVersion;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(Model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/IndividualUpdate", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
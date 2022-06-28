using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class ContactsModel
    {
        #region public properties  
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int AccountID { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> PositionTableID { get; set; }
        public string PositionType { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? SalutationID { get; set; }
        public int? SalutationTableID { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string LastName { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string PrimaryPhoneNumber { get; set; }
        public string OtherPhoneNumber { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "ValidEmailAddressMsg", ErrorMessageResourceType = typeof(Global))]
        public string PrimaryEmail { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "ValidEmailAddressMsg", ErrorMessageResourceType = typeof(Global))]
        public string OtherEmail { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem> SalutionList { get; set; }
        public IEnumerable<SelectListItem> PositionList { get; set; }
        #endregion

        #region public methods     
        public List<ContactsModel> Get(int? contactID, int? accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "contactID", Value = contactID });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<List<ContactsModel>>(APISelector.Municipality, true, "api/Account/ContactGet", "GET", lstParameter);
        }
        public ContactsModel Get(int? id)
        {
            ContactsModel model = new ContactsModel();

            if (id > 0)
                model = Get(id, null)[0];

            model.SalutionList = HMTLHelperExtensions.CreateSelectList(GetSalutations(), "ID", "Name");
            model.PositionList = HMTLHelperExtensions.CreateSelectList(GetPositions(), "ID", "Name");
            return model;
        }
        public int GetContactsForExist(int ID, int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/GetContactsForExist", "GET", lstParameter);
        }
        #endregion

        #region private methods           
        private List<SupportValueModel> GetSalutations()
        {
            return new AccountModel().GetSupportValues(2);
        }

        private List<SupportValueModel> GetPositions()
        {
            return new AccountModel().GetSupportValues(4);
        }
        #endregion
    }
}
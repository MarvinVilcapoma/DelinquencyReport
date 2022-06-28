using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class PhoneModel
    {
        #region public properties          
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int AccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int PhoneTypeID { get; set; }
        public int PhoneTypeTableID { get; set; }
        public string PhoneType { get; set; }
        public bool IsWorkPrimary { get; set; }
        public bool IsMobilePrimary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<SelectListItem> PhoneTypeList { get; set; }
        #endregion

        #region public methods     
        public List<PhoneModel> Get(int? id, int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "phoneTypeID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "phoneTypeTableID", Value = null });
            return new RestSharpHandler().RestRequest<List<PhoneModel>>(APISelector.Municipality, true, "api/Account/PhoneGet", "GET", lstParameter);
        }

        public PhoneModel Get(int AccountTypeID)
        {
            PhoneModel model = new PhoneModel();
            model.PhoneTypeList = HMTLHelperExtensions.CreateSelectList(GetPhoneTypes(AccountTypeID), "ID", "Name");
            return model;
        }
        #endregion

        #region private methods     
        private List<SupportValueModel> GetPhoneTypes(int AccountTypeID)
        {
            if (AccountTypeID == 4)
                return new AccountModel().GetSupportValues(5);
            else
                return new AccountModel().GetSupportValues(38);
        }
        #endregion
    }
}
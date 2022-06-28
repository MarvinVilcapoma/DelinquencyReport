using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class EmailModel
    {
        #region public properties  
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int AccountID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "ValidEmailAddressMsg", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int EmailTypeID { get; set; }
        public int EmailTypeTableID { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem> EmailTypeList { get; set; }
        #endregion

        #region public methods   
        public EmailModel Get()
        {
            EmailModel model = new EmailModel();
            model.EmailTypeList = HMTLHelperExtensions.CreateSelectList(GetEmailTypes(), "ID", "Name");
            return model;
        }

        public List<EmailModel> Get(int? id, int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "emailTypeID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "emailTypeTableID", Value = null });
            return new RestSharpHandler().RestRequest<List<EmailModel>>(APISelector.Municipality, true, "api/Account/EmailGet", "GET", lstParameter);
        }
        #endregion

        #region private methods    
        private List<SupportValueModel> GetEmailTypes()
        {
            return new AccountModel().GetSupportValues(7);
        }
        #endregion
    }
}
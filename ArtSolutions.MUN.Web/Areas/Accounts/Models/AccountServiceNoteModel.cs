using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceNoteModel
    {
        #region Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool? IsAccountServiceNote { get; set; }
        #endregion

        #region Methods
        public List<AccountServiceNoteModel> Get(int accountServiceId , int? id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });            
            return new RestSharpHandler().RestRequest<List<AccountServiceNoteModel>>(APISelector.Municipality, true, "api/AccountService/NoteGet", "GET", lstParameter);
        }

        public int Insert()
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.CreatedBy = UserSession.Current.Username;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/NoteInsert", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Helpers
{
    public class UserProfile
    {
        
        #region "Public Methods"
        public UserProfileViewModel UserProfileGet(UserProfileViewModel model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "userID", Value = model.UserID.ToString() },
            };
            return new RestSharpHandler().RestRequest<UserProfileViewModel>(APISelector.Security, true, "api/userprofile/GetUserDetailByUserID", "GET", lstParameter);
        }
        public List<UserProfileViewModel> GetUserByUserIDs(string userIDs, string filterText = "")
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "strUserIDs", Value = userIDs},
                new NameValuePair { Name = "filterText", Value = filterText }
            };
            return new RestSharpHandler().RestRequest<List<UserProfileViewModel>>(APISelector.Security, true, "api/userprofile/UserGetByUserIDs", "GET", lstParameter);
        }
        public void ChangeLanguage(string language)
        {
            UserProfileViewModel model = new UserProfileViewModel();
            model.UserID = UserSession.Current.Id;
            model.Language = language;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            new RestSharpHandler().RestRequest<int>(APISelector.Security, true, "api/userprofile/UpdateLanguage", "POST", null, lstObjParameter);
        }
        #endregion "Public Methods"
    }
    public class UserProfileViewModel
    {
        #region "Properties"
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        #endregion "Properties"
    }
}
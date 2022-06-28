using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class UserDataModel
    {
        #region public property
        public System.Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public string AboutMe { get; set; }
        public Nullable<int> ProfilePictureID { get; set; }
        public Nullable<int> BackGroundImageID { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public String Email { get; set; }
        #endregion

        #region Extra properties add by ankur
        public byte[] RowVersion { get; set; }
        public string FullName { get; set; }
        public bool PendingInvitation { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        #endregion

        #region Method Section

        /// <summary>
        /// GetUsers - used to get user details
        /// </summary>                  
        /// <returns>List<UserDetails> - List of all users</returns>  
        /// <param name="strUserIDs">comma separated string of UserIDs</param>
        public List<UserDataModel> GetUsers(string strUserIDs, string filterText = "")
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                    new NameValuePair { Name = "strUserIDs", Value = strUserIDs },
                    new NameValuePair { Name = "filterText", Value = filterText }

             };

            return new RestSharpHandler().RestRequest<List<UserDataModel>>(APISelector.Security, true, "api/userprofile/UserGetByUserIDs", "GET", lstParameter);
        }
        public List<UserDataModel> GetUsersByCompanyID()
        {
            RestSharpHandler _objRestSharpHandler;
            try
            {
                _objRestSharpHandler = new RestSharpHandler();

                List<NameValuePair> lstParameter = new List<NameValuePair>();

                lstParameter.Add(new NameValuePair { Name = "pendingInvitation", Value = true });

                string _strURL = "api/userprofile/GetUserList";

                return _objRestSharpHandler.RestRequest<List<UserDataModel>>(APISelector.Security, true, _strURL, "GET", lstParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserDataModel> GetByApplicationID(Guid applicationID, string filterText)
        {
            RestSharpHandler _objRestSharpHandler;
            try
            {
                _objRestSharpHandler = new RestSharpHandler();
                List<NameValuePair> lstParameter = new List<NameValuePair>();

                lstParameter.Add(new NameValuePair { Name = "applicationID", Value = applicationID });
                lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });

                string _strURL = "api/User/GetByApplicationID";

                return _objRestSharpHandler.RestRequest<List<UserDataModel>>(APISelector.Security, true, _strURL, "GET", lstParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
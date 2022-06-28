using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class UserModels
    {
        #region public property
        public System.Guid ID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool ResetPassword { get; set; }
        public bool UpdateProfile { get; set; }
        public string SecurityCode { get; set; }
        public bool PendingInvitation { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<System.Guid> LastApplicationAccessed { get; set; }
        public Nullable<int> LastCompanyVisited { get; set; }
        public byte[] RowVersion { get; set; }
        public string FullName { get; set; }
        public int RoleCount { get; set; }
        public int CompanyID { get; set; }
        public string ApplicationURL { get; set; }
        public Nullable<int> ProfilePictureID { get; set; }
        public bool? IsApplicationAdmin { get; set; }
        public bool? IsCompanyOwner { get; set; }
        public string Language { get; set; }
        #endregion  

        #region public methods
        public List<UserModels> GetUserInfo(string strUserIDs, string filterText = "")
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "strUserIDs", Value = strUserIDs },
                new NameValuePair { Name = "filterText", Value = filterText }
            };

            return new RestSharpHandler().RestRequest<List<UserModels>>(APISelector.Security, true, "api/userprofile/UserGetByUserIDs", "GET", lstParameter);
        }

        public UserModels GetAuthorByUserID(string UserID)
        {
            RestSharpHandler _objRestSharpHandler;
            try
            {
                _objRestSharpHandler = new RestSharpHandler();
                string url = "api/userprofile/GetUserDetailByUserID";
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "userID", Value = UserID });
                return _objRestSharpHandler.RestRequest<UserModels>(APISelector.Security, true, url, "GET", lstParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    public class UserDropdownModel
    {
        #region public methods
        public Guid ID { get; set; }
        public string Name { get; set; }
        #endregion
    }
}
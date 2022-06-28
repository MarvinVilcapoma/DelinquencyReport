using ArtSolutions.MUN.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ChatUser
    {
        #region public properties      
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public Nullable<System.Guid> ChatUserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsOnline { get; set; }
        public string ConnectionId { get; set; }
        public bool IsBusy { get; set; }
        public string Language { get; set; }
        public string IPAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfileImage { get; set; }
        #endregion

        #region public methods      
        public List<ChatUser> ActiveChatUserGet(int CompanyID, string Language, string Email, bool? IsOnline, Nullable<System.Guid> ChatUserID)
        {
            var url = "api/Chat/ChatUser/ActiveChatUserGet";
            RestSharpHandler _objRestSharpHandler = new RestSharpHandler();

            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CompanyID", Value = CompanyID });
            lstParameter.Add(new NameValuePair { Name = "Language", Value = Language });
            lstParameter.Add(new NameValuePair { Name = "Email", Value = Email });
            lstParameter.Add(new NameValuePair { Name = "IsOnline", Value = IsOnline });
            lstParameter.Add(new NameValuePair { Name = "ChatUserID", Value = ChatUserID });
            return _objRestSharpHandler.RestRequest<List<ChatUser>>(APISelector.CustomerService, false, url, "GET", lstParameter);
        }

        public ChatUser ChatUserInsert(string Email, string Name, string PhoneNumber, string ChatMessage, int CompanyID, string Language, string UserGUID)
        {
            ChatUser model = new ChatUser();

            var url = "api/Chat/ChatUser/ChatUserInsert";
            RestSharpHandler _objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            model.CompanyID = CompanyID;
            model.ChatUserID = new Guid(UserGUID);
            model.Email = Email;
            model.Name = Name;
            model.Note = ChatMessage;
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = new Guid(UserGUID);
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = new Guid(UserGUID);
            model.IsOnline = true;
            model.Language = Language;

            lstObjParameter.Add(model);
            return _objRestSharpHandler.RestRequest<ChatUser>(APISelector.CustomerService, false, url, "POST", null, lstObjParameter);
        }

        public ChatUser ChatUserStatusUpdate(int ID, bool IsOnline, int CompanyID, string Language)
        {
            var url = "api/Chat/ChatUser/ChatUserStatusUpdate";
            List<object> lstObjParameter = new List<object>();
            UserStatus model = new UserStatus();
            model.ID = ID;
            model.IsOnline = IsOnline;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CompanyID = CompanyID;
            model.Language = Language;
            lstObjParameter.Add(model);
            RestSharpHandler _objRestSharpHandler = new RestSharpHandler();
            return _objRestSharpHandler.RestRequest<ChatUser>(APISelector.CustomerService, false, url, "POST", null, lstObjParameter);
        }
        #endregion
    }

    public class UserStatus
    {
        #region public properties      
        public int ID { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CompanyID { get; set; }
        public string Language { get; set; }
        #endregion
    }

    public class IpInfo
    {
        #region public properties      
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }
        #endregion

        #region public methods      
        public static IpInfo GetUserLocationInfo()
        {
            try
            {
                string ipaddress = GetIpAddress();
                IpInfo ipInfo = new IpInfo();
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ipaddress);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                return ipInfo;
            }
            catch (Exception)
            {
                return new IpInfo();
            }
        }

        public static string GetIpAddress()
        {
            try
            {
                string url = "http://checkip.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string ipaddress = a3[0];
                return ipaddress;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
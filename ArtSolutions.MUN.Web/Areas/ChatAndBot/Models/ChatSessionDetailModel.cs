using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ChatSessionDetailModel
    {
        #region public properties      
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ChatSessionID { get; set; }
        public int ChatUserID { get; set; }
        public Guid LoggedInChatUserID { get; set; }
        public Guid AgentUserID { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
        public string IPAddress { get; set; }
        public string OSName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Browser { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        #endregion

        #region public methods
        public List<ChatSessionDetailModel> GetAllChatSessionsDetail(int ChatUserID, int CompanyID, string Language, Guid ChatAgentID)
        {
            var url = "api/Chat/ChatUser/ChatSessionDetailGet";
            RestSharpHandler _objRestSharpHandler = new RestSharpHandler();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ChatUserID", Value = ChatUserID });
            lstParameter.Add(new NameValuePair { Name = "CompanyID", Value = CompanyID });
            lstParameter.Add(new NameValuePair { Name = "Language", Value = Language });
            lstParameter.Add(new NameValuePair { Name = "ChatAgentID", Value = ChatAgentID });
            return _objRestSharpHandler.RestRequest<List<ChatSessionDetailModel>>(APISelector.CustomerService, false, url, "GET", lstParameter);
        }
        #endregion
    }

    public class ChatMessage
    {
        #region public properties      
        public int CountNumber { get; set; }
        public int? VisitorID { get; set; }
        public Guid? VisitorGUID { get; set; }
        public Guid? AgentGUID { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsVisitorToAgent { get; set; }
        public string AgentConnectionID { get; set; }
        public string VisitorConnectionID { get; set; }
        public string ClientTimeZone { get; set; }
        #endregion
    }
}
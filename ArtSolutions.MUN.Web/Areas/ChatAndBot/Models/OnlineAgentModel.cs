using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class OnlineAgentModel
    {
        #region Property Section
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public Guid AgentUserID { get; set; }
        public bool IsOnline { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Language { get; set; }
        #endregion

        #region Property Section
        public List<OnlineAgentModel> GetOnlineAgent(int CompanyID, string Language, bool? IsOnline)
        {
            var url = "api/Chat/ChatUser/OnlineAgentGet";
            RestSharpHandler _objRestSharpHandler = new RestSharpHandler();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "CompanyID", Value = CompanyID });
            lstParameter.Add(new NameValuePair { Name = "Language", Value = Language });
            lstParameter.Add(new NameValuePair { Name = "isOnline", Value = IsOnline });
            return _objRestSharpHandler.RestRequest<List<OnlineAgentModel>>(APISelector.CustomerService, false, url, "GET", lstParameter);
        }
        #endregion
    }
}
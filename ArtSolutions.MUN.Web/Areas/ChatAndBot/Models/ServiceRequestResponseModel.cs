using System;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ServiceRequestResponseModel
    {
        #region Property Section       
        public int ID { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ServiceRequestCategory { get; set; }
        public string RequesterName { get; set; }
        public string ResponseByName { get; set; }
        public Nullable<System.Guid> CreatedUserID { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ResponseCC { get; set; }
        public int TicketID { get; set; }
        public int ResponseTo { get; set; }
        public System.Guid ResponseFrom { get; set; }
        public Nullable<int> CannedAnswerID { get; set; }
        public Nullable<int> KBPostID { get; set; }
        public int Sequence { get; set; }
        public int ChannelID { get; set; }
        public string AgentEmailID { get; set; }
        public string RequesterEmailID { get; set; }
        public Guid RequesterUserID { get; set; }
        public string RequesterUserName { get; set; }
        public string RequesterUserEmail { get; set; }
        public Nullable<System.Guid> ResponseByUserID { get; set; }
        public string ResponseByEmail { get; set; }
        public string CC { get; set; }
        #endregion
    }
}
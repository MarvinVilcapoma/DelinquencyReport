using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Controllers
{
    public class ChatController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOnlineAgent()
        {
            bool isExist = RemoteFileExists(Common.URLCustomerServicePath);
            if (isExist)
            {
                OnlineAgentModel objOnlineAgentModel = new OnlineAgentModel();
                List<OnlineAgentModel> lstAgent = objOnlineAgentModel.GetOnlineAgent(UserSession.Current.CompanyID, UserSession.Current.Language, true);
                lstAgent = lstAgent.Where(x => x.AgentUserID != UserSession.Current.Id).ToList();
                string _strPartialView = string.Empty;
                if (lstAgent.Count == 0)
                {
                    ServiceRequestModel _objServiceRequest = new ServiceRequestModel();
                    _objServiceRequest.FillServiceRequestDataForChat(_objServiceRequest, true);
                    _strPartialView = ServiceRequestController.RenderPartialToString(this, "~/Views/Shared/_ChatBotTabs.cshtml", _objServiceRequest);
                }
                return Json(new { agentList = lstAgent, data = _strPartialView }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public string VisitorChat(string ImageSrc, string UserName, string Message, string MessageTime)
        {
            string strVisitorChat = "<div class='sidebar-message'>" +
                                "<a>" +
                                    "<div class='pull-left text-center'>" +
                                        "<img alt = 'image' class='img-circle message-avatar' src='" + ImageSrc + "'>" +
                                    "</div>" +
                                    "<div class='media-body'>" +
                                        "<strong>" + UserName + "</strong><br><p class='headerchatMessage'>" + Message + "</p><br>" +
                                        "<small class='text-muted'>" + MessageTime + "</small>" +
                                    "</div>" +
                                "</a>" +
                            "</div>";

            return strVisitorChat;
        }

        public UserModels GetAgentInfo(string userID)
        {
            UserModels objUserModels = new UserModels();
            List<UserModels> lstUser = objUserModels.GetUserInfo(userID);
            return lstUser.FirstOrDefault();
        }

        public static string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        public JsonResult AddVisitor()
        {
            ChatUser objChatUser = new ChatUser();
            objChatUser = objChatUser.ChatUserInsert(UserSession.Current.Email, UserSession.Current.Username, string.Empty, string.Empty, UserSession.Current.CompanyID, UserSession.Current.Language, Convert.ToString(UserSession.Current.Id));
            objChatUser.Language = UserSession.Current.Language;

            UserModels objUserModels = new UserModels();
            objUserModels = objUserModels.GetAuthorByUserID(Convert.ToString(UserSession.Current.Id));
            objChatUser.ProfileImage = Convert.ToString(objUserModels.ProfilePictureID);
            //string ipaddress = "27.121.103.12";
            //string ipaddress = "64.233.191.255";

            if (Request.Cookies["VisitorCookies"] != null)
            {
                HttpCookie nameCookie = Request.Cookies["VisitorCookies"];
                objChatUser.IPAddress = nameCookie.Values["VIPAddress"];
                objChatUser.Country = nameCookie.Values["VCountry"];
                objChatUser.City = nameCookie.Values["VCity"];
            }
            else
            {
                IpInfo objIpInfo = IpInfo.GetUserLocationInfo();
                HttpCookie nameCookie = new HttpCookie("VisitorCookies");
                //Set the Cookie value.
                nameCookie.Values["VIPAddress"] = objIpInfo.Ip;
                nameCookie.Values["VCountry"] = objIpInfo.Country;
                nameCookie.Values["VCity"] = objIpInfo.City;
                nameCookie.Values["VRegion"] = objIpInfo.Region;
                //Set the Expiry date.
                nameCookie.Expires = DateTime.Now.AddDays(1);
                //Add the Cookie to Browser.
                Response.SetCookie(nameCookie);
            }
            return Json(objChatUser, JsonRequestBehavior.AllowGet);
        }

        public void UpdateUserStatus(int ID, bool isOnline)
        {
            ChatUser objChatUser = new ChatUser();
            objChatUser.ChatUserStatusUpdate(ID, isOnline, UserSession.Current.CompanyID, UserSession.Current.Language);
        }

        public bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        public JsonResult GetChatHistory(int ChatUserID, bool IsVisitor)
        {
            List<ClientHistoryChat> lstClientHistoryChat = new List<ClientHistoryChat>();
            string strTitle = string.Empty;
            string strComboID = string.Empty;
            string strCollapseClass = string.Empty;

            string prevdate = "";
            string currentdate = "";

            List<ChatSessionDetailModel> lstChatSessionDetail = new ChatSessionDetailModel().GetAllChatSessionsDetail(ChatUserID, UserSession.Current.CompanyID, UserSession.Current.Language, new Guid());

            System.Web.Script.Serialization.JavaScriptSerializer objJSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            
            string chatdate = string.Empty;
            DateTime dtchatMessage = new DateTime();

            ChatUser objChatUserInfo = new ChatUser();
            List<ChatUser> lstActiveUserInfo = objChatUserInfo.ActiveChatUserGet(UserSession.Current.CompanyID, UserSession.Current.Language, null, true, null);
            objChatUserInfo = lstActiveUserInfo.Where(x => x.ID == ChatUserID).FirstOrDefault();

            string hideclass = "style='display: none;'";
            string collapseClass = "chatcollapse";

            foreach (ChatSessionDetailModel item in lstChatSessionDetail)
            {
                DateTime localeDate = TimeZone.CurrentTimeZone.ToLocalTime(item.CreatedDate);
                string strColId = localeDate.Date.ToString("ddMMyyyy") + item.ID;
                chatdate = string.Empty;

                if (dtchatMessage.Date != localeDate.Date)
                {
                    dtchatMessage = localeDate;
                    chatdate = dtchatMessage.ToString("d", CultureInfo.InvariantCulture);

                    if (dtchatMessage.Date == Common.CurrentDateTime.Date)
                    {
                        chatdate = ArtSolutions.MUN.Web.Resources.Chat.TodayTitle;
                        hideclass = string.Empty;
                        collapseClass = string.Empty;
                    }

                    if (IsVisitor)
                    {
                        strTitle = chatdate;
                        strComboID = strColId;
                        strCollapseClass = collapseClass;
                    }
                    else
                    {
                        strTitle = chatdate;
                        strComboID = strColId;
                        strCollapseClass = collapseClass;
                    }
                }

                List<ChatMessage> lstMessages = objJSSerializer.Deserialize<List<ChatMessage>>(item.Message);
                if (objChatUserInfo != null && lstMessages != null && lstMessages.Count > 0)
                {
                    foreach (ChatMessage objChatMessage in lstMessages)
                    {
                        ClientHistoryChat objClientHistoryChat = new ClientHistoryChat();
                        objClientHistoryChat.Title = strTitle;
                        objClientHistoryChat.ComboID = strComboID;
                        objClientHistoryChat.CollapseClass = strCollapseClass;
                        objClientHistoryChat.HideClass = hideclass;

                        if (!string.IsNullOrEmpty(objChatMessage.Message))
                        {
                            if (IsVisitor)
                            {
                                if (!objChatMessage.IsVisitorToAgent)
                                {
                                    objClientHistoryChat.IsVisitorToAgent = true;
                                    UserModels objUserModels = GetAgentInfo(objChatMessage.AgentGUID.ToString());
                                    if (objUserModels != null)
                                    {
                                        objClientHistoryChat.UserName = objUserModels.FullName.Trim();
                                        if (string.IsNullOrEmpty(objUserModels.UserName))
                                        {
                                            objClientHistoryChat.UserName = objUserModels.Email;
                                        }
                                        if (objUserModels.ProfilePictureID > 0)
                                        {
                                            objClientHistoryChat.ImageSrc = Common.SecurityFileURL + objUserModels.ProfilePictureID;
                                        }
                                        else
                                        {
                                            objClientHistoryChat.ImageSrc = Common.URLCustomerWebSitePath + Common.NoProfilePicture;
                                        }
                                    }
                                }
                                else
                                {
                                    objClientHistoryChat.IsVisitorToAgent = false;
                                    objClientHistoryChat.UserName = objChatUserInfo.Name.Trim();

                                    if (string.IsNullOrEmpty(objClientHistoryChat.UserName))
                                    {
                                        objClientHistoryChat.UserName = objChatUserInfo.Email;
                                    }
                                    if (UserSession.Current.ProfileImage > 0)
                                    {
                                        objClientHistoryChat.ImageSrc = Common.SecurityFileURL + ArtSolutions.MUN.Web.Helpers.UserSession.Current.ProfileImage;
                                    }
                                    else
                                    {
                                        objClientHistoryChat.ImageSrc = Common.URLCustomerWebSitePath + Common.NoProfilePicture;
                                    }
                                }

                                objClientHistoryChat.Message = objChatMessage.Message;
                                objClientHistoryChat.MessageTime = objChatMessage.DateTime;
                                objClientHistoryChat.strMessageTime = objChatMessage.DateTime.ToString("M/d/yyyy h:mm:ss tt");
                                objClientHistoryChat.MessageType = "chat-message active";
                            }
                        }
                        lstClientHistoryChat.Add(objClientHistoryChat);
                    }
                }
            }

            if (dtchatMessage.Date != Common.CurrentDateTime.Date)
            {
                ClientHistoryChat objClientHistoryChat = new ClientHistoryChat();
                objClientHistoryChat.Title = ArtSolutions.MUN.Web.Resources.Chat.TodayTitle;
                lstClientHistoryChat.Add(objClientHistoryChat);
            }

            var myResult = new
            {
                Conversation = lstClientHistoryChat,
                PreviousDate = prevdate,
                CurrentDate = currentdate
            };

            return Json(myResult, JsonRequestBehavior.AllowGet);
        }
    }

    public class ClientHistoryChat
    {
        public string ImageSrc { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public string strMessageTime { get; set; }
        public bool IsVisitorToAgent { get; set; }
        public string Title { get; set; }
        public string ComboID { get; set; }
        public string CollapseClass { get; set; }
        public string HideClass { get; set; }
        public string MessageType { get; set; }
    }
}
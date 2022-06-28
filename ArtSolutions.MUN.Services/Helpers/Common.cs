using ArtSolutions.MUN.Services.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace ArtSolutions.MUN.Services.Helpers
{
    public class Common
    {
        public static string URLSecurityAPIPath { get { return ConfigurationManager.AppSettings["SecurityAPIPath"]; } }

        public static string URLFinanceAPIPath { get { return ConfigurationManager.AppSettings["FinanceAPIPath"]; } }
        public static string EmailLogoUrl { get { return ConfigurationManager.AppSettings["EmailLogoUrl"]; } }


        public static void ValidateUserPermission(Guid token, int companyID, Guid featureID, Actions action)
        {
            string url = "api/system/ValidateToken";
            var objRestSharpHandler = new RestSharpHandler();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "token", Value = token.ToString() });
            lstParameter.Add(new NameValuePair { Name = "companyID", Value = companyID.ToString() });
            lstParameter.Add(new NameValuePair { Name = "featureID", Value = featureID.ToString() });
            lstParameter.Add(new NameValuePair { Name = "action", Value = action.ToString() });

            objRestSharpHandler.RestRequest<object>(APISelector.Security, url, "GET", lstParameter);
        }

        public static void InsertActivity(Guid token, int companyID, string featureID, string actionID, string objectID, string objectName)
        {
            string url = "api/system/InsertActivity";
            var objRestSharpHandler = new RestSharpHandler();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "token", Value = token });
            lstParameter.Add(new NameValuePair { Name = "companyID", Value = companyID });
            lstParameter.Add(new NameValuePair { Name = "featureID", Value = featureID });
            lstParameter.Add(new NameValuePair { Name = "actionID", Value = actionID });
            lstParameter.Add(new NameValuePair { Name = "objectID", Value = objectID });
            lstParameter.Add(new NameValuePair { Name = "objectName", Value = objectName });

            objRestSharpHandler.RestRequest<object>(APISelector.Security, url, "GET", lstParameter);
        }

        public static void SendEmail(Guid token, string language, int companyId, MailBoxModel model)
        {
            string url = "api/notification/SendEmail";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObject = new List<object>();
            lstObject.Add(model);

            objRestSharpHandler.RestRequest<bool>(APISelector.Security, url, "POST", null, lstObject, true,token,language,companyId);
        }

        public static void SendMailNotification(Guid token, string language, int companyId, string strToMail, int iEventID, Dictionary<string, string> replaceContent = null, Dictionary<string, string> subjectContent = null, List<string> lstToMail = null, bool isWithAuthentication = true, Guid? featureID = null, int? templateID = 0)
        {
            //************************ SendMail Code **************************//                            
            MailBoxModel mailBoxModel = new MailBoxModel();
            if (!string.IsNullOrEmpty(strToMail))
                mailBoxModel.To = strToMail;
            if (lstToMail != null && lstToMail.Count > 0)
                mailBoxModel.lstTo = lstToMail;
            mailBoxModel.EventId = iEventID;
            mailBoxModel.Id = templateID.Value;

            if (replaceContent != null)
            {
                List<NOTElement> elements = new List<NOTElement>();
                foreach (var item in replaceContent)
                {
                    elements.Add(new NOTElement() { Code = item.Key, Value = item.Value });
                }
                mailBoxModel.Elements = elements;
            }

            if (subjectContent != null)
            {
                List<NOTElement> subjectElements = new List<NOTElement>();
                foreach (var item in subjectContent)
                {
                    subjectElements.Add(new NOTElement() { Code = item.Key, Value = item.Value });
                }
                mailBoxModel.SubjectElements = subjectElements;
            }
            string _strURL = string.Empty;
            Common.SendEmail(token, language, companyId, mailBoxModel);
        }

        public static void AbortSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
    public class NameValuePair
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
    public class APIMessage
    {
        public string Message { get; set; }
    }
    public enum Actions
    {
        View,
        New,
        Edit,
        Delete,
        Post,
        Void,
        Process,
        Pay,
        Export,
        Print,
        Custom1,
        Custom2,
        Custom3,
        Custom4,
        Custom5,
        RePrint,
        Lock
    }
}
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace ArtSolutions.MUN.Web
{
    public class Common
    {
        #region Static Properties
        public static Guid CurrentApplication { get { return Guid.Parse("00000000-0009-0000-0000-000000000000"); } }
        public static Int32 PageSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]); } }
        public static DateTime CurrentDateTime { get { return System.DateTime.Now; } }
        public static string TempFolder { get { return HttpContext.Current.Server.MapPath("~/UploadCaseFiles"); } }
        public static string ExportFolderPath { get { return HttpContext.Current.Server.MapPath("~/Export"); } }
        public const string ExportHTMLFile = "Export.html";
        public static string[] ImportFileTypes = new string[] { ".xlsx",".csv" };
        public static string[] ImportTextFileTypes = new string[] { ".txt" };
        public static string DecimalPoints
        {
            get
            {
                int DecimalPoints = UserSession.Current.DecimalPoints;

                if (DecimalPoints == 1)
                    return "F1";
                else if (DecimalPoints == 2)
                    return "F2";
                else if (DecimalPoints == 3)
                    return "F3";
                else if (DecimalPoints == 4)
                    return "F4";
                else if (DecimalPoints == 5)
                    return "F5";
                else
                    return "F0";
            }
        }
        public static string FormatDecimalPoints
        {
            get
            {
                int DecimalPoints = UserSession.Current.DecimalPoints;

                if (DecimalPoints == 1)
                    return "0";
                else if (DecimalPoints == 2)
                    return "00";
                else if (DecimalPoints == 3)
                    return "000";
                else if (DecimalPoints == 4)
                    return "0000";
                else if (DecimalPoints == 5)
                    return "00000";
                else
                    return "";
            }
        }
        public static string NumberPoints
        {
            get
            {
                int DecimalPoints = UserSession.Current.DecimalPoints;

                if (DecimalPoints == 1)
                    return "N1";
                else if (DecimalPoints == 2)
                    return "N2";
                else if (DecimalPoints == 3)
                    return "N3";
                else if (DecimalPoints == 4)
                    return "N4";
                else if (DecimalPoints == 5)
                    return "N5";
                else
                    return "N0";
            }
        }
        #endregion

        #region URL Properties

        public static string URLServicePath { get { return ConfigurationManager.AppSettings["URLServicePath"]; } }
        public static string URLSecurityAPIPath { get { return ConfigurationManager.AppSettings["SecurityAPIPath"]; } }
        public static string URLSecurityServicePath { get { return ConfigurationManager.AppSettings["URLSecurityServicePath"]; } }
        public static string URLCustomerServicePath { get { return ConfigurationManager.AppSettings["URLCustomerServicePath"]; } }
        public static string URLFinancePath { get { return ConfigurationManager.AppSettings["URLFinancePath"]; } }
        public static string FileURL { get { return ConfigurationManager.AppSettings["URLServicePath"] + "api/file/GetFile?Id="; } }
        public static string SecurityFileURL { get { return ConfigurationManager.AppSettings["URLSecurityServicePath"] + "api/file/GetFile?Id="; } }
        public static string URLCustomerWebSitePath { get { return ConfigurationManager.AppSettings["URLCustomerWebSitePath"]; } }
        public static string BotId { get { return ConfigurationManager.AppSettings["BotId"]; } }
        public static string BotSecretKey { get { return ConfigurationManager.AppSettings["BotSecretKey"]; } }
        public static string ImportFilePath { get { return "~/ImportFile/"; } }
        public static string DownloadFilePath { get { return "~/DownloadFile/"; } }
        public static string URLSignalR { get { return ConfigurationManager.AppSettings["URLCustomerWebSitePath"] + "signalr"; } }
        #endregion

        #region Static Properties - File

        public static string MaxFilesToUpload { get { return ConfigurationManager.AppSettings["MaxFilesToUpload"]; } }
        public static string MaxFileLengthToUpload { get { return ConfigurationManager.AppSettings["MaxFileLengthToUpload"]; } }
        public static string AllowedFileTypesToUpload { get { return ConfigurationManager.AppSettings["AllowedFileTypesToUpload"]; } }
        public static string AllowedLogoFileTypesToUpload { get { return ConfigurationManager.AppSettings["AllowedLogoFileTypesToUpload"]; } }
        public static string NoProfilePicture { get { return ConfigurationManager.AppSettings["NoProfilePic"]; } }

        #endregion

        #region Const for Session and Tempdata

        public const string T_DocumentWorkflow = "DocumentWorkflow";
        public const string S_ExportData = "ExportData";

        #endregion Const for Session and Tempdata

        public static void AbortSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        #region "Enums"
        public enum NOTEvent
        {
            MuniTax_New_Legal_Case = 24,
            MuniTax_Update_Status_with_Comments = 25,
        }
        public enum Actions
        {
            Print = 1,
            Download = 2
        }
        public enum ContactType
        {
            Heir = 1,
            Spouse = 2,
            CRIM = 3,
            Alternate_Address = 4
        }
        public enum ActionMode
        {
            Add = 1,
            Edit,
            View,
            Delete
        }
        public enum DocumentTypeEnum
        {
            ServiceRequest = 45,
            CreditNotes = 50,
            PostingProcess = 51,
            PropertyTransfer = 57,
            SplitRight = 58
        }
        public enum SearchTypeEnum
        {
            Account = 1,
            AccountService = 2,
            AccountProperty = 3
        }

        #endregion "Enums"

        #region "Chat"
        /// <summary>
        /// SendMail - used to send mail
        /// </summary>
        /// <returns>int - integer value which we received as a mail response</returns>  
        /// <param name="mbModel">object of MailBoxModel model</param>
        /// <param name="strURL">string URL of mail notification</param>
        public static int SendMail(MailBoxModel mbModel, string strURL)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "To", Value = Convert.ToString(mbModel.To) });
            lstParameter.Add(new NameValuePair { Name = "lstTo", Value = Convert.ToString(mbModel.lstTo) });
            RestSharpHandler objRestSharpHandler = new RestSharpHandler();

            List<object> lstObject = new List<object>();
            lstObject.Add(mbModel);

            int response = objRestSharpHandler.RestRequest<int>(APISelector.Security, true, strURL, "POST", null, lstObject);

            return response;
        }

        public static void SetCookie(string strName, string strValue)
        {
            DestroyCookie(strName);
            HttpCookie cookie = new HttpCookie(strName, strValue);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void DestroyCookie(string strName)
        {
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies[strName];
            if (currentUserCookie != null)
            {
                HttpContext.Current.Response.Cookies.Remove(strName);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                HttpContext.Current.Response.SetCookie(currentUserCookie);
            }
        }
        #endregion

        public static string GetAbsoluteUrl(string page)
        {
            if (HttpContext.Current == null)
            {
                return page;
            }
            else if (VirtualPathUtility.IsAbsolute(page))
            {
                return
                  HttpContext.Current.Request.Url.Scheme + "://"
                  + HttpContext.Current.Request.Url.Authority
                  + HttpContext.Current.Request.ApplicationPath
                  + page;
            }
            else
            {
                return
                  HttpContext.Current.Request.Url.Scheme + "://"
                  + HttpContext.Current.Request.Url.Authority
                  + VirtualPathUtility.ToAbsolute(page);
            }
        }

        public static string GetRomanNumber(int Number)
        {
            switch (Number)
            {
                case 1: return "I " + Resources.Global.Trimester;
                case 2: return "II " + Resources.Global.Trimester; ;
                case 3: return "III " + Resources.Global.Trimester; ;
                case 4: return "IV " + Resources.Global.Trimester; ;
                case 5: return "V" + Resources.Global.Trimester; ;
                case 6: return "VI " + Resources.Global.Trimester; ;
                case 7: return "VII " + Resources.Global.Trimester; ;
                case 8: return "VIII " + Resources.Global.Trimester; ;
                case 9: return "IX " + Resources.Global.Trimester; ;
                case 10: return "X " + Resources.Global.Trimester; ;
                case 11: return "XX " + Resources.Global.Trimester; ;
                case 12: return "XXX " + Resources.Global.Trimester; ;
                default: return "";

            }
        }
        public static decimal GetConfigureDecimal(decimal? d, int decimalPlaces)
        {
            if (d.HasValue)
                return Convert.ToDecimal(d.Value.ToString("N" + decimalPlaces));
            else return 0;
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
    public class SelectListItemViewModel
    {
        public object ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Activity { get; set; }
        public int SequenceID { get; set; }
        public bool InitialStatus { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool Post { get; set; }
        public bool Void { get; set; }
        public bool IsPublic { get; set; }
        public int FormID { get; set; }
    }
    public class ResponceMessage
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class UploadFiles
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string Extention { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileNameWithExtention { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
    }

}
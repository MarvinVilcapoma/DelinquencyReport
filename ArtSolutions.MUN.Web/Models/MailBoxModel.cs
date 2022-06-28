using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Models
{
    public class MailBoxModel
    {
        public Guid Token { get; set; }
        public int CompanyId { get; set; }
        public int EventId { get; set; }
        public int Id { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        [AllowHtml]
        public string MailMessage { get; set; }

        public List<string> lstTo { get; set; }

        public List<string> lstCc { get; set; }

        public List<string> lstBcc { get; set; }


        public string lstAttachment { get; set; }
        public bool IsDraft { get; set; }

        public List<NOTElement> Elements { get; set; }

        public int SendMail(MailBoxModel model, string url)
        {
            RestSharpHandler objRestSharpHandler = new RestSharpHandler();

            List<object> lstObject = new List<object>();

            lstObject.Add(model);

            return objRestSharpHandler.RestRequest<int>(APISelector.Security, true, url, "POST", null, lstObject);
        }
    }
}
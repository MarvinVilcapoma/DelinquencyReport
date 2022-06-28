using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Services.Models
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
        public List<NOTElement> SubjectElements { get; set; }
    }
    public class NOTElement
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string ElementValue { get; set; }
        public string Value { get; set; }
    }
}
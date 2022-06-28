using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FinanceCompanyAccount
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string AccountCodeCode { get; set; }
        public string Code { get; set; }
        public string CodeFriendly { get; set; }
        public string Name { get; set; }
        public string NameFriendly { get; set; }
        public int AccountType { get; set; }
        public string ControlTypeCode { get; set; }
    }
}
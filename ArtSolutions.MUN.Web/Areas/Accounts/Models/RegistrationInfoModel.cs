using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class RegistrationInfoModel
    {
        public string TaxNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public string StateNumber { get; set; }
        public Nullable<System.DateTime> InitialDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class GeneralInfoModel
    {
        public int ID { get; set; }
        public string RegisterNumber { get; set; }
        public int CompanyID { get; set; }
        public int AccountTypeID { get; set; }
        public bool IsBusiness { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int Sequence { get; set; }
        public string DisplayName { get; set; }
        public string PrintCheckAs { get; set; }
        public string BusinessDescription { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyISOCode { get; set; }
        public Nullable<int> FinanceTemplateID { get; set; }
        public string Notes { get; set; }
        public string Website { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string TaxNumber { get; set; }
        public string TreasuryNumber { get; set; }
        public string StateNumber { get; set; }
        public Nullable<System.DateTime> InitialDate { get; set; }
    }
}
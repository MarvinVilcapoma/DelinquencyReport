using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Helpers
{
    public static class EnumUtility
    {
        public enum DocumentType
        {
            MunicipalityAccountService = 23,
            Invoice = 30,
            Payment = 31,
        }
        public enum ServiceCode
        {
            DOMESTICMEASUREDWATERSERVICES = 1,
            ORDINARYMEASUREDWATERSERVICES = 2,
            REPRODUCTIVEMEASUREDWATERSERVICE = 3,
            PREFEREDMEASUREDWATERSERVICE = 4,
            GOVERNMENTMEASUREDWATERSERVICE = 5
        }
        public enum FINAccountType
        {
            AR, // Account Receivable
            RE  // Revenue Account
        }
    }
}
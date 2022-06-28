using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Helpers
{
    [Serializable]
    public class UserSession
    {
        public UserSession()
        {
        }

        // Gets the current session.
        public static UserSession Current
        {
            get
            {
                UserSession session =
                  (UserSession)HttpContext.Current.Session["__UserSession__"];
                if (session == null)
                {
                    session = new UserSession();
                    HttpContext.Current.Session["__UserSession__"] = session;
                }
                return session;
            }
        }
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public int CompanyID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int ProfileImage { get; set; }
        public string Language { get; set; }
        public bool IsOwner { get; set; }
        public bool IsCompanyConfigured { get; set; }
        public string Culture { get; set; }
        public int DecimalPoints { get; set; }
        public int CountryID { get; set; }
        public string CurrencyName { get; set; }
    }
}
using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Services.Models
{
    public class SecuirtyAPIModel
    {
        public string PaymentNumberPrefixByUserRoleGet(Guid Token, int CompanyID, string Language)
        {
            List<Guid> userRoles = new RestSharpHandler().RestRequest<List<Guid>>(APISelector.Security, "api/Role/SECRoleGetRoleIDByUserID", "GET", null, null, true, Token, Language, CompanyID).ToList();
            if (userRoles.Contains(new Guid("00000000-0000-0000-0000-000000066666"))) // Official Collector 
                return "RO";
            else if (userRoles.Contains(new Guid("00000000-0000-0000-0000-000000666666"))) // Auxiliar Collector
                return "RA";
            else throw new Exception("50004");
        }

        public List<SalesCashierModel> SalesCashierGet(Guid Token, int CompanyID, string Language)
        {
            string roleIds = "00000000-0000-0000-0000-000000066666,00000000-0000-0000-0000-000000666666";
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "roleIDs", Value = roleIds });
            return new RestSharpHandler().RestRequest<List<SalesCashierModel>>(APISelector.Security, "api/User/GetByRoleIDs", "GET", lstParameter, null, true, Token, Language, CompanyID).ToList();
        }
    }
}
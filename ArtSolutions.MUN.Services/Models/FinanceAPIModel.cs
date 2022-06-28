using ArtSolutions.MUN.Core.Collections;
using ArtSolutions.MUN.Core.ServiceModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Services.Models
{
    public class FinanceAPIModel
    {
        public List<FinClassGrantModel> ClassGrantGet(Guid Token, int CompanyID, string Language, int? ID, string FilterText, bool? IsActive, bool? IsGrant)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = FilterText });
            lstParameter.Add(new NameValuePair { Name = "Isactive", Value = IsActive });
            lstParameter.Add(new NameValuePair { Name = "IsGrant", Value = IsGrant });
            return new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Finance, "api/ClassGrant/GetGrantForCollection", "GET", lstParameter, null, true, Token, Language, CompanyID).ToList();
            //return new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Finance, "api/ClassGrant/Get", "GET", lstParameter, null, true, Token, Language, CompanyID).ToList();
        }
        public FinGrantAccountModel AccountGet(Guid Token, int CompanyID, string Language, int GrantID, string AccountType)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountType", Value = AccountType });
            lstParameter.Add(new NameValuePair { Name = "GrantID", Value = GrantID });
            lstParameter.Add(new NameValuePair { Name = "moduleId", Value = 2 });
            return new RestSharpHandler().RestRequest<FinGrantAccountModel>(APISelector.Finance, "api/ChartOfAccount/GetCollectionAccounts", "GET", lstParameter, null, true, Token, Language, CompanyID);
        }
        public List<FinPaymentOptionModel> PaymentOptionGet(Guid Token, int CompanyID, string Language, int? CashierId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "cashierID", Value = CashierId });
            return new RestSharpHandler().RestRequest<List<FinPaymentOptionModel>>(APISelector.Finance, "api/Common/PaymentOptionsGet", "GET", lstParameter, null, true, Token, Language, CompanyID).ToList();
        }
    }
}
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountTypeModel
    {
        #region public properties       
        public int ID { get; set; }
        public string Name { get; set; }        
        #endregion

        #region public methods 
        public List<AccountTypeModel> GetAccountType(Guid ApplicationID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();          
            lstParameter.Add(new NameValuePair { Name = "ApplicationID", Value = ApplicationID });
            return new RestSharpHandler().RestRequest<List<AccountTypeModel>>(APISelector.Municipality, true, "api/AccountService/AccountTypeGet", "GET", lstParameter);
        }
        #endregion
    }
}
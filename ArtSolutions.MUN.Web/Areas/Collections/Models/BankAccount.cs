using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class FINBankAccount
    {
        #region public methods
        public List<FINBankAccountModel> Get(string filterText, int? Id, bool? isActive = null, int? cashierID = null)
        {
            if (Id == 0) Id = null;

            List<FINBankAccountModel> model = new List<FINBankAccountModel>();
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
                lstParameter.Add(new NameValuePair { Name = "id", Value = Id });
                lstParameter.Add(new NameValuePair { Name = "IsActive", Value = isActive });
                lstParameter.Add(new NameValuePair { Name = "cashierID", Value = cashierID });

                model = new RestSharpHandler().RestRequest<List<FINBankAccountModel>>(APISelector.Municipality, true, "api/Finance/GetAllBankAccount", "GET", lstParameter, null);
            }
            catch (Exception) { }
            return model;
        }

        public bool CheckJorunalTransactionIsMatched(int referenceID, int documentTypeID)
        {
            bool Retval = false;
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "referenceID", Value = referenceID });
                lstParameter.Add(new NameValuePair { Name = "documentTypeID", Value = documentTypeID });

                Retval = new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Finance/CheckTransactionIsMatched", "GET", lstParameter, null);
            }
            catch (Exception) { }
            return Retval;
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class FiscalYearModel
    {
        #region public properties       
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion

        #region public methods  
        public List<FiscalYearModel> GetFiscalYear()
        {
            return new RestSharpHandler().RestRequest<List<FiscalYearModel>>(APISelector.Municipality, true, "api/AccountService/FiscalYearGet", "GET", null);
        }

        public IEnumerable<SelectListItem> GetFiscalYearByService(int serviceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceId", Value = serviceId });
            return HMTLHelperExtensions.CreateSelectList(new RestSharpHandler().RestRequest<List<FiscalYearModel>>(APISelector.Municipality, true, "api/AccountService/FiscalYearGetByService", "GET", lstParameter).ToList(), "ID", "ID", null, true, true, Global.DropDownSelectMessage);
        }

        public IEnumerable<SelectListItem> GetFiscalYearByServiceNotInAccount(int serviceId, int accountId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "serviceID", Value = serviceId });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountId });
            return HMTLHelperExtensions.CreateSelectList(new RestSharpHandler().RestRequest<List<FiscalYearModel>>(APISelector.Municipality, true, "api/AccountService/FiscalYearGetByServiceNotInAccount", "GET", lstParameter).ToList(), "ID", "ID", null, true, true, Global.DropDownSelectMessage);
        }
        #endregion
    }
}
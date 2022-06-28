using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceExemptionModel
    {
        #region public properties
        public int AccountServiceId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string AccountServiceCollectionDetailId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal? Amount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }
        public List<AccountServiceCollectionDetailModel> CollectionDetail { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ID { get; set; }
        public string ServicePeriod { get; set; }
        public int Mode { get; set; }
        #endregion

        #region public method
        public void Insert()
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/ProcessExemption", "POST", null, lstObjParameter);
        }
        public void Delete(int ID)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.ID = ID;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteExemption", "POST", null, lstObjParameter);
        }
        public void DeleteAll(int ID)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.AccountServiceId = ID;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteAllExemption", "POST", null, lstObjParameter);
        }

        public AccountServiceExemptionModel Get(int accountServiceID, int serviceID)
        {
            this.CollectionDetail = new AccountServiceCollectionDetailModel().Get(accountServiceID, null, null).Where(x => x.AmountSubjectToTax > 0 && x.PaidAmount == 0 && x.IsEditPermission == 1).ToList();
            this.AccountServiceId = accountServiceID;
            return this;
        }
        public List<AccountServiceCollectionDebtModel> GetExemptionList(int accountserviceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountServiceID", Value = accountserviceId });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDebtModel>>(APISelector.Municipality, true, "api/AccountService/ExemptionGet", "GET", lstParameter);
        }
        public List<AccountServiceExemptionModel> GetExemptionHistory(int accountserviceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountServiceID", Value = accountserviceId });
            return new RestSharpHandler().RestRequest<List<AccountServiceExemptionModel>>(APISelector.Municipality, true, "api/AccountService/ExemptionHistoryGet", "GET", lstParameter);
        }
        #endregion
    }
}
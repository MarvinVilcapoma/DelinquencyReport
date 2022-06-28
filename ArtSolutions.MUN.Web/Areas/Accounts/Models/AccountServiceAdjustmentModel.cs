using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceAdjustmentModel
    {
        #region public properties
        public int AccountServiceId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string AccountServiceCollectionDetailId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int ServiceCollectionRuleId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal? Amount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }
        public bool AdjustForAll { get; set; }
        public List<AccountServiceCollectionDetailModel> CollectionDetail { get; set; }
        public List<ServiceCollectionRuleModel> ServiceCollectionRule { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ID { get; set; }
        #endregion

        #region public method
        public void Insert()
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            //if (this.AccountServiceCollectionDetailId > 0)
            //    this.AdjustForAll = false;
            //else
            //    this.AdjustForAll = true;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/ProcessAdjustment", "POST", null, lstObjParameter);
        }

        public List<AccountServiceCollectionDebtModel> Get(int accountserviceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountServiceID", Value = accountserviceId });            
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDebtModel>>(APISelector.Municipality, true, "api/AccountService/AdjustmentsGet", "GET", lstParameter);
        }

        public AccountServiceAdjustmentModel Get(int accountServiceID, int serviceID)
        {
            this.CollectionDetail = new AccountServiceCollectionDetailModel().Get(accountServiceID, null, null).Where(x => x.FillingDate.HasValue && x.PaidAmount == 0 && x.IsPayed == false && x.IsEditPermission == 1).ToList();
            this.ServiceCollectionRule = new ServiceCollectionRuleModel().Get(serviceID, null,false).Where(x => x.IsActive == true).ToList();
            this.AccountServiceId = accountServiceID;
            return this;
        }

        public void Delete(int ID)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.ID = ID;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteAdjustment", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
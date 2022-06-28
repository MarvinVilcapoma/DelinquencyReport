using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceExtensionModel
    {
        #region public properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(1, 12, ErrorMessageResourceName = "RequiredMonthRangeValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int Months { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]        
        public decimal GrossVolume { get; set; }
        public decimal? ExemptionAmount { get; set; }
        public decimal CreditAmount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal Total { get; set; }
        public int? ImageID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        #endregion

        #region Extra Properties
        public string FileName { get; set; }
        #endregion

        #region public methods
        public AccountServiceExtensionModel Get(int accountserviceID, DateTime filingDueDate)
        {
            this.AccountServiceID = accountserviceID;
            this.StartDate = filingDueDate.AddDays(1);
            this.EndDate = filingDueDate.AddDays(1).AddMonths(1);
            return this;
        }

        public AccountServiceExtensionModel Get(int ID, int AccountServiceID)
        {
            List<NameValuePair> lstObjParameter = new List<NameValuePair>();
            lstObjParameter.Add(new NameValuePair() { Name = "ID", Value = ID });
            lstObjParameter.Add(new NameValuePair() { Name = "AccountServiceID", Value = AccountServiceID });
            return new RestSharpHandler().RestRequest<AccountServiceExtensionModel>(APISelector.Municipality, true, "api/AccountService/ExtensionGet", "GET", lstObjParameter);
        }

        public int Insert(AccountServiceExtensionModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.IsActive = true;
            model.IsDeleted = false;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountService/ExtensionInsert", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
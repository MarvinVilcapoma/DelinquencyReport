using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCorrectiontModel
    {
        #region public properties
        public int AccountServiceId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<System.DateTime> FillingDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal Amount { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int PendingPeriod { get; set; }

        
        public decimal? AdjustmentAmount { get; set; }

        
        public int? ExemptionPeriod { get; set; }

        
        public decimal? ExemptionAmount { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }           
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public AccountServiceFillingModel collectionDetail { get; set; }
        public string PropertyTaxJson { get; set; }
        public Nullable<int> MovementTypeID { get; set; }
        public Nullable<decimal> Area { get; set; }
        public Nullable<decimal> TerrainValue { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        #endregion

        #region public method
        public void Insert()
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.MovementTypeID = this.collectionDetail.CollectionFillingPropertyTaxModel.MovementTypeID;
            this.Area = this.collectionDetail.CollectionFillingPropertyTaxModel.Area;
            this.TerrainValue = this.collectionDetail.CollectionFillingPropertyTaxModel.TerrainValue;
            this.TotalValue = this.collectionDetail.CollectionFillingPropertyTaxModel.TotalValue;
            this.PropertyTaxJson = new JavaScriptSerializer().Serialize(this.collectionDetail.CollectionFillingPropertyTaxModel.AccountPropertyList);
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/ProcessCorrection", "POST", null, lstObjParameter);
        }
        public bool ValidateUserPermissionForCorrection()
        {
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/ValidateUserPermissionForCorrection", "GET", null, null);
        }
        #endregion
    }
}
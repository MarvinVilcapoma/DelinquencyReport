using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class PaymentPlanModel
    {
        #region Public Properties
        public int ID { get; set; }
        public string Name { get; set; }
        //public string ServiceTypeName { get; set; }
        //public int ServiceTypeID { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public System.DateTime EffectiveTo { get; set; }
        public decimal InstalmentPercentage { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal LateInterestPercentage { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(1, Int32.MaxValue, ErrorMessageResourceName = "MustBeEqualOrGreaterThan3Msg", ErrorMessageResourceType = typeof(Resources.PaymentPlan))]
        public int Months { get; set; }
        public bool IsEditable { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion

        #region custom properties
        public List<SelectListItem> ServiceTypeList { get; set; }
        public string InstalmentPercentageValue { get; set; }
        public string InterestPercentageValue { get; set; }
        public string LateInterestPercentageValue { get; set; }
        #endregion

        #region Constructor
        public PaymentPlanModel()
        {
            ServiceTypeList = new List<SelectListItem>();
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Companies.Models
{
    public class CurrencyModel
    {
        #region public properties        
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ISOCode { get; set; }
        public string Currency { get; set; }
        public string Symbol { get; set; }
        public string CultureLocalization { get; set; }
        public string Format { get; set; }
        public int Decimals { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }           
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class RegistrationBusinessModel
    {
        #region public properties  
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> NAICSCodeID { get; set; }
        public Nullable<int> NAICSCodeIDTable { get; set; }
        public string NAICSCode { get; set; }
        public Nullable<int> BusinessTypeID { get; set; }
        public Nullable<int> BusinessTypeTableID { get; set; }
        public string BusinessType { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> ZoneID { get; set; }
        public Nullable<int> ZoneTableID { get; set; }
        public string Zone { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string ARPEDescription { get; set; }
        #endregion

        #region Constructor
        public RegistrationBusinessModel()
        {
        }
        public RegistrationBusinessModel(AccountBusinessModel model)
        {
            this.NAICSCodeID = model.NAICSCodeID ?? 0;
            this.NAICSCodeIDTable = model.NAICSCodeIDTable ?? 0;
            this.NAICSCode = model.NAICSCode ?? "";
            this.BusinessTypeID = model.BusinessTypeID ?? 0;
            this.BusinessTypeTableID = model.BusinessTypeTableID ?? 0;
            this.BusinessType = model.BusinessType ?? "";
            this.ZoneID = model.ZoneID ?? 0;
            this.ZoneTableID = model.ZoneTableID ?? 0;
            this.Zone = model.Zone ?? "";
            this.ARPEDescription = model.ARPEDescription ?? "";
        }
        #endregion
    }
}
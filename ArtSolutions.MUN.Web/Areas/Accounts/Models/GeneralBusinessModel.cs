using ArtSolutions.MUN.Web.Resources;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class GeneralBusinessModel
    {
        #region public properties  

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string LegalName { get; set; }
        public string DBAName { get; set; }
        #endregion

        #region Constructor
        public GeneralBusinessModel()
        {

        }

        public GeneralBusinessModel(AccountBusinessModel model)
        {
            this.LegalName = model.LegalName??"";
            this.DBAName = model.DBAName??"";
        }
        #endregion
    }
}
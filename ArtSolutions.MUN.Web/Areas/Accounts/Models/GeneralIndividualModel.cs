using ArtSolutions.MUN.Web.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class GeneralIndividualModel
    {
        #region public properties  
        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public Nullable<int> SalutationID { get; set; }
        public Nullable<int> SalutationTableID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public Nullable<int> SuffixID { get; set; }
        public Nullable<int> SuffixTableID { get; set; }
        public int? IDTypeID { get; set; }
        public int? IDTypeIDTableID { get; set; }
        public string IDType { get; set; }
        #endregion

        #region Constructor
        public GeneralIndividualModel()
        {

        }

        public GeneralIndividualModel(AccountIndividualModel model)
        {
            this.SalutationID = model.SalutationID;
            this.SalutationTableID = model.SalutationTableID;
            this.FirstName = model.FirstName;
            this.MiddleName = model.MiddleName;
            this.LastName = model.LastName;
            this.SecondLastName = model.SecondLastName;
            this.SuffixID = model.SuffixID;
            this.SuffixTableID = model.SuffixTableID;
            this.IDTypeID = model.IDTypeID;
            this.IDTypeIDTableID = model.IDTypeIDTableID;
            this.IDType = model.IDType;
        }
        #endregion
    }
}
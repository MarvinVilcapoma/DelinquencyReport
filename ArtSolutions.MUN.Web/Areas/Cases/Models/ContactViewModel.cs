using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class ContactViewModel
    {
        public int CaseID { get; set; }
        public int WorkflowVersionID { get; set; } = 1;
        public int WorkflowID { get; set; }
        public int StatusID { get; set; }
        public int ReasonID { get; set; }
        public int ContactTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryID { get; set; }
        public int CountryStateID { get; set; }
        public string ZipPostalCode { get; set; }
        public string Comment { get; set; }
        public string ReferenceID { get; set; }
        public string ReferenceDate { get; set; }
        public Guid CreatedUserID { get; set; } = Helpers.UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        public List<CountryModel> CountryList { get; set; } = new List<CountryModel>();
        public List<SelectListItemViewModel> CountryStateList { get; set; } = new List<SelectListItemViewModel>();
    }
    public class ContactList
    {
        public List<ContactViewModel> ContactViewModel { get; set; }
    }
}
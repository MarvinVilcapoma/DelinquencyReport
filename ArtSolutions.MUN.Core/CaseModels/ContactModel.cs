using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class ContactModel
    {
        public int Insert(ContactList model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                DataTable caseModels = model.ContactViewModel.ToDataTable();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@MUNFRMContactTable",Value = caseModels,SqlDbType = SqlDbType.Structured,TypeName = "MUNFRMContactUTD"}
                };
                return context.ExecuteSqlProcedure(sqlparameters, "MUNFRMContactInsert");
            }
        }
    }
    public class ContactViewModel
    {
        public int CompanyID { get; set; }
        public int CaseID { get; set; }
        public int WorkflowVersionID { get; set; }
        public int WorkflowID { get; set; }
        public int StatusID { get; set; }
        public int ReasonID { get; set; }
        public int ContactTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public int CountryID { get; set; }
        public int CountryStateID { get; set; }
        public string ReferenceID { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string Comment { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ContactList
    {
        public List<ContactViewModel> ContactViewModel { get; set; } = new List<ContactViewModel>();
    }

}

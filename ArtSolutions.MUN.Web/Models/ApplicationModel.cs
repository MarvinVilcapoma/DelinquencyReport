using System;

namespace ArtSolutions.MUN.Web.Models
{
    public class ApplicationModel
    {
        public System.Guid ID { get; set; }
        public string Abbreviation { get; set; }
        public bool IsFramework { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public Nullable<int> MenuImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationURL { get; set; }
    }
    public class CompanyDropdownModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public System.Guid UserID { get; set; }
        public bool IsCompanyOwner { get; set; }
        public string Language { get; set; }
    }
}
using System;

namespace ArtSolutions.MUN.Web.Areas.Companies.Models
{
    public class ModuleModel
    {
        #region public properties        
        public System.Guid ID { get; set; }
        public System.Guid ApplicationID { get; set; }
        public string Abbreviation { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        #endregion
    }
}
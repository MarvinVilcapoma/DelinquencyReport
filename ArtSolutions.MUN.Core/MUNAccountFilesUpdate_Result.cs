//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtSolutions.MUN.Core
{
    using System;
    
    public partial class MUNAccountFilesUpdate_Result
    {
        public int FileID { get; set; }
        public int AccountID { get; set; }
        public int CompanyID { get; set; }
        public int FileTypeID { get; set; }
        public int FileTypeTableID { get; set; }
        public byte[] Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int SequenceID { get; set; }
    }
}

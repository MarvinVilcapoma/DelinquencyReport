using System;

namespace ArtSolutions.MUN.Web.Models
{
    public class FileModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public byte[] Data { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public bool IsListedInFolders { get; set; }
        public Nullable<int> FolderID { get; set; }
        public bool IsPublicImage { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}
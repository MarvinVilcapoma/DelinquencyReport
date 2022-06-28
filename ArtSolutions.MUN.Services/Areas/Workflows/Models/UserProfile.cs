using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Models
{
    public class UserProfile
    {
        public System.Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public string AboutMe { get; set; }
        public Nullable<int> ProfilePictureID { get; set; }
        public Nullable<int> BackGroundImageID { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] Rowversion { get; set; }
    }
}
using System;

namespace ArtSolutions.MUN.Core.Collections
{
    public class FinPaymentOptionModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SequenceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ImageID { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}

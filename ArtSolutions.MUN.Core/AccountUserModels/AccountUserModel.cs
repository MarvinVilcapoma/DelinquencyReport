using System;

namespace ArtSolutions.MUN.Core.AccountUserModels
{
    public class AccountUserModel
    {
        public int CompanyID { get; set; }
        public int AccountTypeID { get; set; }
        public int AccountID { get; set; }
        public Guid UserID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}

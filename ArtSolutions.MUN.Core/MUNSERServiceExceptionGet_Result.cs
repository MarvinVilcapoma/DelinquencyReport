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
    
    public partial class MUNSERServiceExceptionGet_Result
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ServiceID { get; set; }
        public decimal ExceptionValue { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> Sort { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int AllowDelete { get; set; }
    }
}

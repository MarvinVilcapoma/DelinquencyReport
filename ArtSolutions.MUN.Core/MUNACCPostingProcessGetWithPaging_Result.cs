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
    
    public partial class MUNACCPostingProcessGetWithPaging_Result
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public System.DateTime AsOfDate { get; set; }
        public string DocumentType { get; set; }
        public bool IsVoid { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool IsPost { get; set; }
        public Nullable<int> WorkflowStatusID { get; set; }
        public string Status { get; set; }
    }
}

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
    
    public partial class MUNREPTransfersReport_Result
    {
        public string RightNumber { get; set; }
        public string PreviousOwner { get; set; }
        public string CurrentOwner { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public string Service { get; set; }
        public Nullable<decimal> BalanceAmountByService { get; set; }
        public Nullable<System.Guid> CreatedUserID { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public string Observation { get; set; }
        public string TransferType { get; set; }
        public string NewRight { get; set; }
        public Nullable<int> TransferTypeID { get; set; }
    }
}

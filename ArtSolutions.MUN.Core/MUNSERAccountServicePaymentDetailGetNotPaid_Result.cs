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
    
    public partial class MUNSERAccountServicePaymentDetailGetNotPaid_Result
    {
        public int ID { get; set; }
        public int AccountServiceCollectionDetailID { get; set; }
        public int SalesDocumentID { get; set; }
        public decimal AmountToPay { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
    }
}

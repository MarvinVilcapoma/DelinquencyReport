using System;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionTemplateDetailModel : ServiceCollectionTemplateDetailModelUTD
    {
        public int ID { get; set; }
        public string Year { get; set; }
        public int YearTableID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }

    }
    public class ServiceCollectionTemplateDetailModelUTD
    {
        public int GrantID { get; set; }
        public string GrantCode { get; set; }
        public string GrantName { get; set; }
        public int YearID { get; set; }
        public int CollectionTypeID { get; set; }
        public int ReceivableAccountID { get; set; }
        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public int RevenueAccountID { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public int CheckbookID { get; set; }
        public string CheckbookCode { get; set; }        
        public string CheckbookName { get; set; }
        public int CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public int CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        public double Percentage { get; set; }
        

    }
}

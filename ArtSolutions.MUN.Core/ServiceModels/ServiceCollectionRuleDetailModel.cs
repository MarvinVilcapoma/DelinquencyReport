namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionRuleDetailModel
    {
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int ServiceCollectionRuleID { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Value { get; set; }
        public decimal SecondValue { get; set; }
        public bool IsEditable { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
    }
}

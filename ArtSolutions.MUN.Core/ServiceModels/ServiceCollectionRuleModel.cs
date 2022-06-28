using System;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionRuleModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ServiceID { get; set; }
        public int SequenceID { get; set; }
        public int CollectionTypeID { get; set; }
        public int CollectionRuleID { get; set; }
        public int FrequencyID { get; set; }
        public int CollectionFieldID { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool AlwaysApply { get; set; }
        public bool ApplyOnPaymentDueDate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }

        ////18-Feb-2020 - Uncomment after changes done
        //public bool IsChanged { get; set; }
        //public bool IsCopied { get; set; }
        ////
        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionTemplateModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public byte[] RowVersion { get; set; }
        public List<ServiceCollectionTemplateDetailModelUTD> ServiceCollectionTemplateDetailListUTD { get; set; }
        public List<ServiceCollectionTemplateDetailModel> ServiceCollectionTemplateDetailList { get; set; }
    }
    public class ServiceCollectionTemplateListModel
    {
        public List<MUNSERServiceCollectionTemplateGetWithPaging_Result> CollectionTemplateList { get; set; }
        public int TotalRecord { get; set; }
    }
}

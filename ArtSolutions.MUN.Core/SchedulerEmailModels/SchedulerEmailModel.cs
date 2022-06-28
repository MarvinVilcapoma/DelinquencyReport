using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.SchedulerEmailModels
{
    public class SchedulerEmailModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string ReasonForDeleted { get; set; }
    }
    public class SchedulerEmailList
    {
        public List<MUNSchedulerEmailGetWithPaging_Result> SchedulerEmailModelList { get; set; }
        public Int32 TotalRecord { get; set; }
    }

}

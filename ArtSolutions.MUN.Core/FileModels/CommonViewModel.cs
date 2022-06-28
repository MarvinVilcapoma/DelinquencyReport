using System;

namespace ArtSolutions.MUN.Core
{
    public class CommonViewModel
    {
        public int PageIndex { get; set; }
        public long PageSize { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime RowVersion { get; set; }
        public string SortColumn { get; set; }
        public SortDirection Sortby { get; set; }
        public string FilterText { get; set; }
        public enum SortDirection
        {
            Asc,
            Desc
        }
        public int CompanyID { get; set; }
        public string Language { get; set; }
        public int TotalRecord { get; set; }
    }
}

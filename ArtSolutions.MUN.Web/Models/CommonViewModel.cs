using System;

namespace ArtSolutions.MUN.Web.Models
{
    public class CommonViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedUserID { get; set; } = Helpers.UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        public Guid ModifiedUserID { get; set; } = Helpers.UserSession.Current.Id;
        public DateTime ModifiedDate { get; set; } = Common.CurrentDateTime;
        public SortDirection Sortby { get; set; }
        public enum SortDirection
        {
            Asc,
            Desc
        }
        public int CompanyID { get; set; }
        public string Language { get; set; }
        public int TotalRecord { get; set; }
        public bool IsActive { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 15;
        public string SortColumn { get; set; }
        public string FilterText { get; set; }
        public string SortOrder { get; set; }
    }
}
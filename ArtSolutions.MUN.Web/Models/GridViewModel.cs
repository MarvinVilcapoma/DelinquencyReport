namespace ArtSolutions.MUN.Web.Models
{
    public class GridViewModel
    {
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; } = 15;
        public string SortColumnName { get; set; }
        public string FilterText { get; set; }
        public string Sort { get; set; }
    }
}
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Models
{
    public class JQDTParams
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public JQDTColumnSearch Search { get; set; }
        public List<JQDTColumnOrder> Order { get; set; }
        public List<JQDTColumn> Columns { get; set; }
    }
    public enum JQDTColumnOrderDirection
    {
        Asc,
        Desc
    }
    public class JQDTColumnOrder
    {
        public int Column { get; set; }
        public JQDTColumnOrderDirection Dir { get; set; }
    }
    public class JQDTColumnSearch
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }
    public class JQDTColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public JQDTColumnSearch search { get; set; }
    }
}
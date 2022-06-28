
using ArtSolutions.MUN.Web.Models;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    public class DocumentTypeViewModel: CommonViewModel
    {
        #region Properties
        public int ID { get; set; }
        public bool IsPostingInJournal { get; set; }
        public bool? IsChild { get; set; }
        public bool IsBankingRule { get; set; }
        #endregion
    }
}
using System;

namespace ArtSolutions.MUN.Services.Areas.Accounts.Models
{
    public class FinanceTemplateModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> AccountTypeID { get; set; }
    }
}
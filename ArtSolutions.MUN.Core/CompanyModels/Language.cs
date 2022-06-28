using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.CompanyModels
{
    public class Language
    {
        public List<MUNLanguageGet_Result> Get(int? companyID)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNLanguageGet(companyID).ToList();
            }
        }
    }
}

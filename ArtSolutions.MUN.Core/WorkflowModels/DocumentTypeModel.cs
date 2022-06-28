using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.WorkflowModels
{
    public class DocumentTypeModel
    {
        public List<MUNDOCDocumentTypeGet_Result> Get(int id,bool isMunicipalityOnly, string language)
        {
            using (WorkflowDataModelContainer context = new WorkflowDataModelContainer())
            {
                return context.MUNDOCDocumentTypeGet(id, isMunicipalityOnly, language).ToList();
            }
        }
    }
}

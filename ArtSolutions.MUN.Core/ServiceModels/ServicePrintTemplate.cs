using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServicePrintTemplate
    {
        public List<MUNSERServicePrintTemplateGet_Result> Get(int? companyID ,int? templateId,int? documentTypeID, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServicePrintTemplateGet(templateId, companyID, documentTypeID, language).ToList();
            }
        }
    }
}

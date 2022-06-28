using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceExceptionModel
    {
        #region Public Methods
        public List<MUNSERServiceExceptionGet_Result> Get(int? id, int? serviceId, int? companyID, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceExceptionGet(id, serviceId, companyID, language).ToList();
            }
        }
        #endregion
    }
}

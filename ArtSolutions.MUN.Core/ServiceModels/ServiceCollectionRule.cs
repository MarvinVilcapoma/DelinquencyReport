using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCollectionRule
    {
        #region CollectionRule
        public int? IsServiceCollectionRuleExist(string collectionRuleName, string languageId, int? id)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCollectionRuleExist(collectionRuleName, languageId, id).FirstOrDefault();
            }
        }
        public List<MUNSERServiceCollectionRuleGet_Result> Get(int serviceId, int companyId, string language, int? id,bool? isDeleted)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCollectionRuleGet(serviceId, companyId, language, id, isDeleted).ToList();
            }
        }
        public List<MUNSERCollectionRuleGet_Result> CollectionRuleGet(string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERCollectionRuleGet(language).ToList();
            }
        }
        public List<MUNSERCollectionTypeGet_Result> CollectionTypeGet(string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERCollectionTypeGet(language).ToList();
            }
        }
        public List<MUNSERCollectionFieldGet_Result> CollectionFieldGet(string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERCollectionFieldGet(language).ToList();
            }
        }
        public List<MUNSERFrequencyGet_Result> FrequencyGet(string language,Nullable<bool> IsServiceFrequency)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERFrequencyGet(language,IsServiceFrequency).ToList();
            }
        }
        #endregion

        #region CollectionRuleDetail
        public List<MUNSERServiceCollectionRuleDetailGet_Result> DetailGet(int serviceId, int companyId, string language, int? id,int? serviceCollectionRuleID,bool? isDeleted)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCollectionRuleDetailGet(serviceId, companyId, language, id,serviceCollectionRuleID,isDeleted).ToList();
            }
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class PrintTemplateModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public  int DocumentTypeID { get; set; }
        public int SequenceID { get; set; }
        public int ImageID { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string TemplateContent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class PrintTemplate
    {
        public List<MUNCOLPrintTemplateGet_Result> Get(int? Id, int? CompanyId, int? DocumentTypeId, string Language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCOLPrintTemplateGet(Id, CompanyId, DocumentTypeId, Language).ToList();
            }
        }
    }
}

using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PrintTemplate
    {
        #region Public Methods
        public List<PrintTemplateModel> Get(int? id,int? documentTypeId)
        {            
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "DocumentTypeID", Value = documentTypeId });
            return new RestSharpHandler().RestRequest<List<PrintTemplateModel>>(APISelector.Municipality, true, "api/Payment/PrintTemplateGet", "GET", lstParameter, null);
        }
        #endregion
    }
    public class PrintTemplateModel
    {
        #region Public Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int DocumentTypeID { get; set; }
        public int SequenceID { get; set; }
        public Nullable<int> ImageID { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateContent { get; set; }
        #endregion
    }
}

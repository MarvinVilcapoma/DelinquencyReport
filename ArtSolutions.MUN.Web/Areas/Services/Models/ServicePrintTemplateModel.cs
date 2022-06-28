using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServicePrintTemplateModel
    {
        #region properties
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
        #region Methods
        public List<ServicePrintTemplateModel> Get(int? Id, int? documentTypeId)
        {
            List<NameValuePair> parameters = new List<NameValuePair>();
            parameters.Add(new NameValuePair() { Name = "ID", Value = Id });
            parameters.Add(new NameValuePair() { Name = "DocumentTypeID", Value = documentTypeId });
            return new RestSharpHandler().RestRequest<List<ServicePrintTemplateModel>>(APISelector.Municipality, true, "api/Service/PrintTemplateGet", "GET", parameters) ?? new List<ServicePrintTemplateModel>();
        }
        #endregion

    }
}
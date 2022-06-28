using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class DocumentTypeModel
    {
        #region Method
        public List<DocumentTypeViewModel> Get(int id = 0,bool isMunicipalityOnly=false)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="id",Value=id },
                new NameValuePair{ Name="isMunicipalityOnly",Value=isMunicipalityOnly }
            };
            return new RestSharpHandler().RestRequest<List<DocumentTypeViewModel>>(APISelector.Municipality, true, "api/Workflow/DocumentTypeGet", "GET", parms);
        }
        #endregion
    }
}
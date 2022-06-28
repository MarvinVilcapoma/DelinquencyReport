using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Models
{
    public class LanguageModel
    {
        public string LanguageCode { get; set; }
        public string Language { get; set; }
    }
    public class LanguageList
    {
        public List<LanguageModel> Get()
        {
            string url = "api/Company/GetLanguagelist";
            return new RestSharpHandler().RestRequest<List<LanguageModel>>(APISelector.Municipality, true, url, "GET", null);
        }

    }
}

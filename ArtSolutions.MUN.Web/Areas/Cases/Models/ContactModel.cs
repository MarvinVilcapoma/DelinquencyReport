using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class ContactModel
    {
        public int Insert(ContactList model)
        {
            List<object> lstObjParameter = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/ContactInsert", "POST", null, lstObjParameter);
        }
    }
}
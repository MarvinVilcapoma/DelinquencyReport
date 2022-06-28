using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseTeam : SelectListItemViewModel
    {
        #region "Properties"
        public bool IsDefault { get; set; }
        public string Email { get; set; }
        #endregion

        #region "Public Methods"
        public List<CaseTeam> CaseTeamGet(Guid id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            return new RestSharpHandler().RestRequest<List<CaseTeam>>(APISelector.Municipality, true, "api/case/CaseTeamGet", "GET", lstParameter);
        }
        #endregion
    }
}
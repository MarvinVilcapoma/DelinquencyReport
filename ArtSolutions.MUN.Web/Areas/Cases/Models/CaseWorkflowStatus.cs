
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseWorkflowStatus : SelectListItemViewModel
    {
        #region "Properties"
        public int? FormID { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public List<CaseWorkflowStatus> StatusActivityGet(int workflowID, int statusID, byte type=0)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "workflowID", Value = workflowID });
            lstParameter.Add(new NameValuePair { Name = "statusID", Value = statusID });
            lstParameter.Add(new NameValuePair { Name = "type", Value = type });
            return new RestSharpHandler().RestRequest<List<CaseWorkflowStatus>>(APISelector.Municipality, true, "api/case/StatusActivityGet", "GET", lstParameter);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowSecurityStatusInGroupModel
    {
        public List<WorkflowSecurityStatusInGroupViewModel> Get(int statusID,int groupId=0)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="statusID", Value= statusID },
                new NameValuePair{ Name="groupID",Value= groupId }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowSecurityStatusInGroupViewModel>>(APISelector.Municipality, true, "api/WorkflowStatus/StatusInGroupGet", "GET", parms);
        }
    }
}
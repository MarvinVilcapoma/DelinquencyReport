using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;
using System.Linq;
using static ArtSolutions.MUN.Web.Common;
using static ArtSolutions.MUN.Web.Models.CommonViewModel;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowReasonModel
    {
        public List<WorkflowReasonViewModel> Get(int id = 0)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "id", Value = id }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowReasonViewModel>>(APISelector.Municipality, true, "api/WorkflowReason/Get", "GET", lstParameter);
        }
        public WorkflowReasonViewModel Get(WorkflowReasonViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="filterText",Value=model.FilterText}
            };
            return new RestSharpHandler().RestRequest<WorkflowReasonViewModel>(APISelector.Municipality, true, "api/WorkflowReason/GetByPaging", "GET", parms);
        }
        public WorkflowReasonViewModel Get(JQDTParams jqdtParams, WorkflowReasonViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        public WorkflowReasonViewModel Get(int id, int actionType, int workflowID)
        {
            var model = new WorkflowReasonViewModel();
            var actionMode = (ActionMode)actionType;
            switch (actionMode)
            {
                case ActionMode.Edit:
                case ActionMode.View:
                    model = Get(id).FirstOrDefault();
                    break;
            }
            return model;
        }
        public int Update(WorkflowReasonViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowReason/Post", "Post", null, parms);
        }
        public WorkflowReasonViewModel GetByWorkflowID(WorkflowReasonViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="workflowID",Value=model.WorkflowID },
                new NameValuePair{ Name="WorkflowVersionID",Value=model.WorkflowVersionID}
            };
            return new RestSharpHandler().RestRequest<WorkflowReasonViewModel>(APISelector.Municipality, true, "api/WorkflowReason/GetByWorkflowID", "GET", parms);
        }
        public WorkflowReasonViewModel GetByWorkflowID(JQDTParams jqdtParams, WorkflowReasonViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = GetByWorkflowID(model);
            return model;
        }
        public List<WorkflowReasonViewModel> StausReasonGet(int workflowID, string version, int statusID, int reasonID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "workflowID", Value = workflowID },
                new NameValuePair { Name = "version", Value = version },
                new NameValuePair { Name = "statusID", Value = statusID },
                new NameValuePair { Name = "reasonID", Value = reasonID }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowReasonViewModel>>(APISelector.Municipality, true, "api/WorkflowReason/StatusReasonGet", "GET", lstParameter);
        }
    }
}
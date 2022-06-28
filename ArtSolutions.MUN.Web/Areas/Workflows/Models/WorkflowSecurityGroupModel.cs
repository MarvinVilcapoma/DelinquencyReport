using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;
using System.Linq;
using static ArtSolutions.MUN.Web.Common;
using static ArtSolutions.MUN.Web.Models.CommonViewModel;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowSecurityGroupModel
    {
        public List<WorkflowSecurityGroupViewModel> Get(int id = 0)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="id",Value=id }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowSecurityGroupViewModel>>(APISelector.Municipality, true, "api/WorkflowGroup/Get", "GET", parms);
        }
        public WorkflowSecurityGroupViewModel Get(WorkflowSecurityGroupViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="filterText", Value=model.FilterText }
            };
            return new RestSharpHandler().RestRequest<WorkflowSecurityGroupViewModel>(APISelector.Municipality, true, "api/WorkflowGroup/GetByPaging", "GET", parms);
        }
        public WorkflowSecurityGroupViewModel Get(JQDTParams jqdtParams, WorkflowSecurityGroupViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        public WorkflowSecurityGroupViewModel Get(int id, int actionType)
        {
            var model = new WorkflowSecurityGroupViewModel();
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
        public int Insert(WorkflowSecurityGroupViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowGroup/Post", "POST", null, parms);
        }
        public int Update(WorkflowSecurityGroupViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowGroup/Put", "Put", null, parms);
        }
        public WorkflowSecurityGroupViewModel GetByWorkflowID(WorkflowSecurityGroupViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="workFlowID", Value=model.WorkFlowID },
                new NameValuePair{ Name="WorkflowVersionID", Value=model.WorkflowVersionID }
            };
            return new RestSharpHandler().RestRequest<WorkflowSecurityGroupViewModel>(APISelector.Municipality, true, "api/WorkflowGroup/GetByWorkflowID", "GET", parms);
        }
        public WorkflowSecurityGroupViewModel GetByWorkflowID(JQDTParams jqdtParams, WorkflowSecurityGroupViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = GetByWorkflowID(model);
            return model;
        }

    }
}
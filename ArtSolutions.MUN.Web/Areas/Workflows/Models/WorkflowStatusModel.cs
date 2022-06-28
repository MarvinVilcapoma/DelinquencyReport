using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;
using System.Linq;
using static ArtSolutions.MUN.Web.Common;
using static ArtSolutions.MUN.Web.Models.CommonViewModel;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowStatusModel
    {
        public WorkflowStatusViewModel Get(int id, int actionType, string name = "", int workflowID = 0)
        {
            var model = new WorkflowStatusViewModel();
            var actionMode = (ActionMode)actionType;
            switch (actionMode)
            {
                case ActionMode.Add:
                    model.Name = name;
                    model.Activity = name;
                    break;
                case ActionMode.Edit:
                case ActionMode.View:
                    model = Get(workflowID, id).FirstOrDefault();
                    model.WorkflowStatusReasonList = new Cases.Models.CaseModel().WorkflowStatusReasonGet(workflowID, id, -1);
                    model.StatusInGroupList = new WorkflowSecurityStatusInGroupModel().Get(id);
                    break;
            }
            model.WorkflowReasonList = new WorkflowReasonModel().Get();
            model.GroupList = new WorkflowSecurityGroupModel().Get();
            return model;
        }
        public List<WorkflowStatusViewModel> Get(int workflowID, int id = 0)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "workflowID", Value = workflowID },
                new NameValuePair { Name = "id", Value = id }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowStatusViewModel>>(APISelector.Municipality, true, "api/WorkflowStatus/Get", "GET", lstParameter);
        }
        public List<WorkflowStatusViewModel> GetForAction(int WorkFlowID, byte type = 1, int? workFlowStatusID = null)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "WorkFlowID", Value = WorkFlowID });
            lstParameter.Add(new NameValuePair { Name = "Type", Value = type });
            lstParameter.Add(new NameValuePair { Name = "workFlowStatusID", Value = workFlowStatusID });
            return new RestSharpHandler().RestRequest<List<WorkflowStatusViewModel>>(APISelector.Municipality, true, "api/WorkflowStatus/GetForAction", "GET", lstParameter);
        }
        public WorkflowStatusViewModel Get(WorkflowStatusViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="workflowID",Value=model.WorkflowID }
            };
            return new RestSharpHandler().RestRequest<WorkflowStatusViewModel>(APISelector.Municipality, true, "api/WorkflowStatus/GetByPaging", "GET", parms);
        }
        public WorkflowStatusViewModel Get(JQDTParams jqdtParams, WorkflowStatusViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        public int Insert(WorkflowStatusViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowStatus/Post", "Post", null, parms);
        }
        public int Update(WorkflowStatusViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowStatus/Put", "Post", null, parms);
        }
        public int SequenceUpdate(WorkflowStatusViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowStatus/SequencePost", "Put", null, parms);
        }
    }
}
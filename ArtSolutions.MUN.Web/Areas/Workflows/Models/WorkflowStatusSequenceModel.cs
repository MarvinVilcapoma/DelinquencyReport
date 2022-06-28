using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowStatusSequenceModel
    {
        public WorkflowStatusSequenceViewModel Get(int workflowID, int id)
        {
            var model = new WorkflowStatusSequenceViewModel
            {
                TargetStatusList = new WorkflowStatusModel().Get(workflowID),
                WorkflowFormList = new Cases.Models.CaseWorkflowForm().WorkflowFormGet(0)
            };
            model.TargetStatusList = model.TargetStatusList.Where(a => a.ID != id).ToList();
            model.TargetStatusList.Insert(0, new WorkflowStatusViewModel
            {
                Name = Resources.Global.DropDownSelectMessage
            });
            model.WorkflowFormList.Insert(0, new Cases.Models.CaseWorkflowForm
            {
                Name = Resources.Global.DropDownSelectMessage
            });
            model.WorkflowStatusID = id;
            model.SecurityGroupList = new WorkflowSecurityGroupModel().Get();
            model.WorkflowStatusSequenceList = Get(workflowID, 0, id);
            return model;
        }
        public int Insert(WorkflowStatusSequenceViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowStatusSequence/Post", "POST", null, parms);
        }
        public List<WorkflowStatusSequenceViewModel> Get(int workflowID = 0, int workflowVersionID = 0, int statusID = 0)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair { Name ="workflowID",Value=workflowID },
                new NameValuePair { Name ="workflowVersionID",Value=workflowVersionID },
                new NameValuePair { Name ="statusID",Value=statusID}
            };
            return new RestSharpHandler().RestRequest<List<WorkflowStatusSequenceViewModel>>(APISelector.Municipality, true, "api/WorkflowStatusSequence/Get", "GET", parms);
        }
        public int Delete(WorkflowStatusSequenceViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowStatusSequence/Delete", "Delete", null, parms);
        }
        public List<WorkflowStatusSequenceViewModel> GetByPaging(int workflowID = 0, int workflowVersionID = 0)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair { Name ="workflowID",Value=workflowID },
                new NameValuePair { Name ="workflowVersionID",Value=workflowVersionID }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowStatusSequenceViewModel>>(APISelector.Municipality, true, "api/WorkflowStatusSequence/GetByPaging", "GET", parms);
        }
    }
}
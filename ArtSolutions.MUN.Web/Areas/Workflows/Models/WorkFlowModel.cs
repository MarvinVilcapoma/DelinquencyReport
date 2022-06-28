using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkFlowModel
    {
        public WorkflowViewModel Get(WorkflowViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="filterText",Value=model.FilterText },
                new NameValuePair{ Name="documentTypeID",Value=model.DocumentTypeID }
            };
            return new RestSharpHandler().RestRequest<WorkflowViewModel>(APISelector.Municipality, true, "api/WorkFlow/GetByPaging", "GET", parms);
        }
        public WorkflowViewModel Get(JQDTParams jqdtParams, WorkflowViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (CommonViewModel.SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        public int ActiveStatusUpdate(WorkflowViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkFlow/ActiveStatusUpdate", "PUT", null, parms);
        }
        public int Insert(WorkflowViewModel model)
        {
            List<object> parms = new List<object> {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkFlow/Post", "Post", null, parms);
        }
        public WorkflowViewModel Editor(int id, int documentTypeID)
        {
            WorkflowViewModel model = Get(id, null).FirstOrDefault();
            model.DocumentTypeID = documentTypeID;
            return model;
        }

        public List<WorkflowViewModel> Get(int id, int? documentTypeID)
        {
            List<NameValuePair> _lstParameter = new List<NameValuePair> {
                        new NameValuePair { Name = "documentTypeID", Value = documentTypeID },
                        new NameValuePair { Name = "workFlowID", Value = id },
                        new NameValuePair { Name = "workFlowVersion", Value = 0 },
                        new NameValuePair { Name = "isActive", Value = 1 },
                        new NameValuePair { Name = "isDeleted", Value = 0 }
                    };
            return new RestSharpHandler().RestRequest<List<WorkflowViewModel>>(APISelector.Municipality, true, "api/case/DocumentWorkflowGet", "GET", _lstParameter);
        }
    }
}
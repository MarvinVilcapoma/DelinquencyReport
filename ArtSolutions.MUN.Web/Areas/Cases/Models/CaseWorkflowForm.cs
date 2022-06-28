using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseWorkflowForm : SelectListItemViewModel
    {
        #region "Properties"
        public string Description { get; set; }
        public bool UsePopup { get; set; }
        public string URLNew { get; set; }
        public string URLEdit { get; set; }
        public string URLView { get; set; }
        public string URLPrint { get; set; }
        public string FilterText { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string SortColumn { get; set; }
        public SortDirection Sortby { get; set; }
        public enum SortDirection
        {
            Asc,
            Desc
        }
        public List<CaseWorkflowForm> WorkflowFormList { get; set; } = new List<CaseWorkflowForm>();
        #endregion "Properties"

        #region "Public Methods"
        public List<CaseWorkflowForm> WorkflowFormGet(int formID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "formID", Value = formID }
            };
            return new RestSharpHandler().RestRequest<List<CaseWorkflowForm>>(APISelector.Municipality, true, "api/case/WorkflowFormGet", "GET", lstParameter);
        }
        public CaseWorkflowForm Get(CaseWorkflowForm model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                 new NameValuePair{ Name="currentIndex", Value= model.CurrentIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="filterText",Value=model.FilterText },
            };
            return new RestSharpHandler().RestRequest<CaseWorkflowForm>(APISelector.Municipality, true, "api/WorkflowForm/GetByPaging", "GET", parms);
        }
        public CaseWorkflowForm Get(JQDTParams jqdtParams, CaseWorkflowForm model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.CurrentIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Models
{
    public class JsTreeViewModel
    {
        public string text { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public bool children { get; set; }
        public JsTreeAttributeViewModel a_attr { get; set; }
        public string icon { get; set; }
        public List<JsTreeViewModel> GetJsonTreeView(List<SelectListItemViewModel> lstModel, string methodName, bool childern,
            int workflowID = 0, int _statusID = 0, int _reasonID = 0, string id = "")
        {
            string tempID = (id + "_" + _statusID.ToString() + "_" + _reasonID.ToString());
            List<JsTreeViewModel> result = (from m in lstModel
                                            select new JsTreeViewModel
                                            {
                                                text = m.Name,
                                                state = "closed",
                                                id = tempID + "_" + m.ID, //GetUniqueID(), //Guid.NewGuid().ToString(),
                                                children = childern,
                                                icon = "fa fa-indent",
                                                a_attr = new JsTreeAttributeViewModel
                                                {
                                                    WorkflowID = Convert.ToString(workflowID),
                                                    NextMethodName = methodName,
                                                    CurrentID = m.ID.ToString(),
                                                    StatusID = id.ToLower().Equals("status") ? m.ID.ToString() : Convert.ToString(_statusID),
                                                    ReasonID = id.ToLower().Equals("reason") ? m.ID.ToString() : Convert.ToString(_reasonID),
                                                    CaseID = id.ToLower().Equals("cases") ? m.ID.ToString() : 0.ToString()
                                                }
                                            }).ToList();
            return result;
        }
        string GetUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
public class JsTreeAttributeViewModel
{
    public string CurrentID { get; set; }
    public string NextMethodName { get; set; }
    public string WorkflowID { get; set; }
    public string StatusID { get; set; }
    public string ReasonID { get; set; }
    public string CaseID { get; set; }
}


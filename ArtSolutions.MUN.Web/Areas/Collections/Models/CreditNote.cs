using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class CreditNote
    {
        #region public methods  
        public int Insert(CreditNoteModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            if (!string.IsNullOrEmpty(model.CommaSeperatedImageIDs))
            {
                model.CommaSeperatedImageIDs = model.CommaSeperatedImageIDs.Trim(',');
                model.CommaSeperatedImageIDs = model.CommaSeperatedImageIDs.Replace(",,", ",");
            }

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/CreditNote/Insert", "POST", null, lstObjParameter);
        }
        public CreditNoteListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            CreditNoteListModel list = new CreditNoteListModel();
            //Filter for Void/Post Status
            bool? isvoid = null;
            if (filterText.Trim().ToLower() == Resources.Global.Void.ToLower())
            {
                isvoid = true;
                filterText = null;
            }
            else if (filterText.Trim().ToLower() == Resources.Global.Posted.ToLower())
            {
                isvoid = false;
                filterText = null;
            }
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "IsVoid", Value = isvoid });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });

            list = new RestSharpHandler().RestRequest<CreditNoteListModel>(APISelector.Municipality, true, "api/CreditNote/GetWithPaging", "GET", lstParameter, null);
            list.CreditNoteList.ForEach(x => { x.Status = x.IsVoid == true ? Resources.Global.Void : Resources.Global.Posted; });
            return list;
        }

        public CreditNotePrintModel GetPrint(int id)
        {
            CreditNotePrintModel model = new CreditNotePrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            model = new RestSharpHandler().RestRequest<CreditNotePrintModel>(APISelector.Municipality, true, "api/CreditNote/GetPrint", "GET", lstParameter, null);
            if(model.CreditNote!=null)
            {
                model.Date = model.CreditNote.Date.ToString();
                model.WorkflowStatusID = model.CreditNote.WorkflowStatusID;
            }
            model.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(1, model.WorkflowStatusID, 2);
            return model;
        }
    
        public int Update(int ID, int WorkFlowStatusID)
        {
            CreditNoteModel model = new CreditNoteModel();
            model.ID = ID;
            model.WorkflowStatusID = WorkFlowStatusID;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/CreditNote/Update", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
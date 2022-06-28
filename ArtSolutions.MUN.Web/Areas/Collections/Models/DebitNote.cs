using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class DebitNote
    {
        #region public methods  
        public int Insert(DebitNoteModel model)
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
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/DebitNote/Insert", "POST", null, lstObjParameter);
        }
        public DebitNoteListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            DebitNoteListModel list = new DebitNoteListModel();
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

            list = new RestSharpHandler().RestRequest<DebitNoteListModel>(APISelector.Municipality, true, "api/DebitNote/GetWithPaging", "GET", lstParameter, null);
            list.DebitNoteList.ForEach(x => { x.Status = x.IsVoid == true ? Resources.Global.Void : Resources.Global.Posted; });
            return list;
        }

        public DebitNotePrintModel GetPrint(int id)
        {
            DebitNotePrintModel model = new DebitNotePrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            model = new RestSharpHandler().RestRequest<DebitNotePrintModel>(APISelector.Municipality, true, "api/DebitNote/GetPrint", "GET", lstParameter, null);
            if (model.DebitNote != null)
            {
                model.Date = model.DebitNote.Date.ToString();
                model.WorkflowStatusID = model.DebitNote.WorkflowStatusID;
            }
            model.WorkflowStatusList = new CaseWorkflowStatus().StatusActivityGet(1, model.WorkflowStatusID, 2);
            return model;
        }

        public int Update(int ID, int WorkFlowStatusID)
        {
            DebitNoteModel model = new DebitNoteModel();
            model.ID = ID;
            model.WorkflowStatusID = WorkFlowStatusID;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/DebitNote/Update", "POST", null, lstObjParameter);
        }

        public int GetAccountDebitNoteExist(int AccountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = AccountID });
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/GetAccountDebitNoteExist", "GET", lstParameter, null);
        }
        #endregion
    }
}
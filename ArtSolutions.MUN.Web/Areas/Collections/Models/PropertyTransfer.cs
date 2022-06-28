using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PropertyTransfer
    {
        #region Get methods
        public List<AccountServiceModel> GetAllAccountService(int accountID, int AccountPropertyID, int transferTypeID, int transferID = 0)
        {
            List<AccountServiceModel> model = new List<AccountServiceModel>();
            if (transferTypeID > 0)
                model = new AccountServiceList().GetAllAccountService(accountID, AccountPropertyID, transferTypeID, transferID);
            return model;
        }
        public PropertyTransferListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            PropertyTransferListModel list = new PropertyTransferListModel();
            //Filter for Void/Post Status

            List<NameValuePair> lstParameter = new List<NameValuePair>();

            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<PropertyTransferListModel>(APISelector.Municipality, true, "api/PropertyTransfer/GetWithPaging", "GET", lstParameter, null);
            return list;
        }
        public int Insert(PropertyTransferModel model)
        {
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.WorkflowVerionID = 1;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PropertyTransfer/Insert", "POST", null, lstObjParameter);
        }
        public PropertyTransferModel GetPropertyTransfer(int ID)
        {
            PropertyTransferModel model = new PropertyTransferModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();

            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            model = new RestSharpHandler().RestRequest<PropertyTransferModel>(APISelector.Municipality, true, "api/PropertyTransfer/Get", "GET", lstParameter, null);
            return model;
        }
        public int Update(int ID, int WorkFlowStatusID, bool Ispost)
        {
            PropertyTransferModel model = new PropertyTransferModel();
            model.TransferID = ID;
            model.workflowStatusID = WorkFlowStatusID;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PropertyTransfer/Update", "POST", null, lstObjParameter);
        }

        public PropertyTransferModel InitNewComment(int statusId, int WorkflowId, int reasonID = -1)
        {
            PropertyTransferModel model = new PropertyTransferModel();
            WorkflowReasonViewModel Reasonmodel = new WorkflowReasonViewModel();
            model.SecurityGroupUsersList = new SelectList(new Workflows.Models.WorkflowSecurityGroupUsersModel().GetForAssignedUser(statusId), "UserID", "DisplayName");
            model.WorkflowReasonList = new SelectList(new WorkflowReasonModel().StausReasonGet(WorkflowId, "1", statusId, reasonID), "ID", "Name");
            return model;
        }

        #endregion
    }


}
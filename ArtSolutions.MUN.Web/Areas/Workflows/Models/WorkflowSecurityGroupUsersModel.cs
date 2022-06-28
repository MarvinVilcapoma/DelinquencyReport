using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ArtSolutions.MUN.Web.Models.CommonViewModel;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class WorkflowSecurityGroupUsersModel
    {
        public WorkflowSecurityGroupUsersViewModel UserGet(int groupID, int workflowID, int documentTypeID)
        {
            var group = new WorkflowSecurityGroupModel().Get(groupID).FirstOrDefault();
            var userlist = new UserDataModel().GetUsersByCompanyID().Where(x => x.PendingInvitation == false && !string.IsNullOrEmpty(x.FirstName)).ToList();
            userlist.ForEach(x => x.FullName = x.FirstName + " " + x.LastName);
            WorkflowSecurityGroupUsersViewModel model = new WorkflowSecurityGroupUsersViewModel
            {
                SecurityUserList = Get(0, groupID),
                Users = userlist
            };
            model.Name = group.Name;
            model.WorkFlowID = workflowID;
            model.ID = groupID;
            model.DocumentTypeID = documentTypeID;
            return model;
        }
        public List<WorkflowSecurityGroupUsersViewModel> Get(int statusID = 0, int groupID = 0)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="groupID",Value=groupID },
                new NameValuePair{ Name="statusID", Value=statusID }
            };
            return new RestSharpHandler().RestRequest<List<WorkflowSecurityGroupUsersViewModel>>(APISelector.Municipality, true, "api/WorkflowGroup/GroupUserGet", "GET", parms);
        }
        public WorkflowSecurityGroupUsersViewModel Get(WorkflowSecurityGroupUsersViewModel model)
        {
            List<NameValuePair> parms = new List<NameValuePair> {
                new NameValuePair{ Name="currentIndex", Value= model.PageIndex },
                new NameValuePair{ Name="pageSize",Value=model.PageSize },
                new NameValuePair{ Name="sortColumn",Value=model.SortColumn },
                new NameValuePair{ Name="sortDirection",Value=(int)model.Sortby },
                new NameValuePair{ Name="workflowID",Value=model.WorkFlowID },
                new NameValuePair{ Name="workflowVersionID",Value=model.WorkflowVersionID}
            };
            return new RestSharpHandler().RestRequest<WorkflowSecurityGroupUsersViewModel>(APISelector.Municipality, true, "api/WorkflowGroup/GroupUsersGetByPaging", "GET", parms);
        }
        public WorkflowSecurityGroupUsersViewModel Get(JQDTParams jqdtParams, WorkflowSecurityGroupUsersViewModel model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;
            model.PageIndex = (jqdtParams.Start + 1);
            model.PageSize = jqdtParams.Length;
            model.Sortby = (SortDirection)jqdtParams.Order[0].Dir;
            model = Get(model);
            return model;
        }
        public int Insert(WorkflowSecurityGroupUsersViewModel model)
        {
            List<object> parms = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/WorkflowGroup/GroupUserInsert", "POST", null, parms);
        }

        public List<WorkflowSecurityGroupUsersViewModel> GetForAssignedUser(int statusID)
        {
            var AssignUserList = Get(statusID);
            var userlist = new UserProfile().GetUserByUserIDs(string.Join(",", AssignUserList.Select(x => x.UserID)));
            if (userlist.Count != 0)
            {
                AssignUserList = AssignUserList.Join(userlist, x => x.UserID, y => y.UserID, (x, y) =>
                                new WorkflowSecurityGroupUsersViewModel
                                {
                                    DisplayName = string.IsNullOrWhiteSpace(x.DisplayName) ? y.Email : x.DisplayName,
                                    GroupID = x.GroupID,
                                    UserID = x.UserID
                                }).ToList();
            }
            return AssignUserList;
        }
    }
}
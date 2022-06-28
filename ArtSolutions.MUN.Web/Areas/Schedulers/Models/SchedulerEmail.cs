using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Schedulers.Models
{
    public class SchedulerEmail
    {
        #region Public Methods

        public bool IsEmailExist(string Email, int? Id)
        {
            bool isExist = false;
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "email", Value = Email });
                lstParameter.Add(new NameValuePair { Name = "id", Value = Id });
                int data = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Service/IsEmailExist", "GET", lstParameter);
                if (data > 0)
                    isExist = true;
            }
            catch (Exception)
            {
                isExist = true;
            }
            return isExist;
        }

        public List<SchedulerEmailModel> Get()
        {
            return new RestSharpHandler().RestRequest<List<SchedulerEmailModel>>(APISelector.Municipality, true, "api/SchedulerEmail/Get", "GET", null);
        }

        public SchedulerEmailModel Get(int schedulerEmailID)
        {
            SchedulerEmailModel model = new SchedulerEmailModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = schedulerEmailID });
            model =  new RestSharpHandler().RestRequest<List<SchedulerEmailModel>>(APISelector.Municipality, true, "api/SchedulerEmail/Get", "GET", lstParameter).FirstOrDefault();
            return model;
        }

        public int Insert(SchedulerEmailModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/SchedulerEmail/Insert", "POST", null, lstObjParameter);
        }

        public int Update(SchedulerEmailModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/SchedulerEmail/Update", "POST", null, lstObjParameter);
        }
        public SchedulerEmailModel Edit(int schedulerEmailID)
        {
            SchedulerEmailModel model = Get(schedulerEmailID);
            return model;
        }

        public SchedulerEmailList Get(int? schedulerEmailID, string filtertext, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "schedulerEmailID", Value = schedulerEmailID });
            lstParameter.Add(new NameValuePair { Name = "filtertext", Value = filtertext });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = sortColumn });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            return new RestSharpHandler().RestRequest<SchedulerEmailList>(APISelector.Municipality, true, "api/SchedulerEmail/GetByPaging", "GET", lstParameter, null);
        }

        public int UpdateStatus(bool isActive, int id)
        {
            SchedulerEmailModel model = Get(id);
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/SchedulerEmail/Update", "POST", null, lstObjParameter);
        }

        public void Delete(int id, string Reason)
        {
            SchedulerEmailModel model = Get(id);
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ReasonForDeleted = Reason;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/SchedulerEmail/Delete", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
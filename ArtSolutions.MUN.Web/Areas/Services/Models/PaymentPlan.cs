using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class PaymentPlan
    {
        #region Public Methods
        public PaymentPlanModel Get(int? id = null)
        {
            PaymentPlanModel model = new PaymentPlanModel();
            if (id > 0)
                model = Get(id,null, null).FirstOrDefault();
            if(model!=null)
                model.ServiceTypeList = HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetServiceType(0), "ID", "Name");
            return model;
        }

        public List<PaymentPlanModel> Get(int? ID, DateTime? startDate, bool? isActive)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "id", Value = ID },
                //new NameValuePair { Name = "serviceTypeID", Value = serviceTypeID },
                new NameValuePair { Name = "date", Value = startDate.HasValue ? startDate.Value.ToString("d", CultureInfo.InvariantCulture) : null },
                new NameValuePair { Name = "isActive", Value = isActive}
            };
            return new RestSharpHandler().RestRequest<List<PaymentPlanModel>>(APISelector.Municipality, true, "api/PaymentPlan/Get", "GET", lstParameter);
        }

        public int Insert(PaymentPlanModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PaymentPlan/Insert", "POST", null, lstObjParameter);
        }

        public int Update(PaymentPlanModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PaymentPlan/Update", "POST", null, lstObjParameter);
        }

        public int UpdateStatus(bool isActive, int id)
        {
            PaymentPlanModel model = Get(id, null, null).FirstOrDefault();
            model.ID = id;
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PaymentPlan/UpdateStatus", "POST", null, lstObjParameter);
        }
        #endregion
    }    
}
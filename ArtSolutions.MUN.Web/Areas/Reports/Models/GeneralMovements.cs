using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class GeneralMovements
    {
        #region public methods  
        public GeneralMovementsModel Get()
        {
            GeneralMovementsModel model = new GeneralMovementsModel();
            model.ReportCompanyModel = GetReportCompany();
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;

            List<UserDataModel> userlist = new UserDataModel().GetByApplicationID(new Guid("00000000-0009-0000-0000-000000000000"), null).Where(x => x.PendingInvitation == false && !string.IsNullOrEmpty(x.FirstName)).ToList();
            model.UserList = new List<SelectListItemViewModel>();
            model.UserList = userlist.Select(i => new SelectListItemViewModel
            {
                ID = i.UserID,
                Name = i.FirstName + " " + i.LastName
            }).ToList();
            return model;
        }
        public GeneralMovementsModel Get(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            GeneralMovementsModel model = new GeneralMovementsModel();
            model = GetByPaging(userID,accountID, fromDate, toDate, pageIndex, pageSize);
            model.GeneralMovementsList = model.GeneralMovementsList ?? new List<Models.GeneralMovementsList>();
            model.ReportCompanyModel = GetReportCompany(fromDate, toDate);
            model.ReportCompanyModel.SubTitle = UserSession.Current.Username;
            return model;
        }
        public GeneralMovementsModel GetList(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            return Get(userID,accountID, fromDate, toDate, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.GeneralMovementsReport, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 7, FromDate, ToDate);
        }

        private GeneralMovementsModel GetByPaging(Guid? userID, int? accountID, DateTime? fromDate, DateTime? toDate, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "userID", Value = userID },
                new NameValuePair { Name = "accountID", Value = accountID },
                new NameValuePair { Name = "fromDate", Value = fromDate == null?null: fromDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "toDate", Value =toDate==null?null:toDate.Value.ToString("d", CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "pageIndex", Value = pageIndex },
                new NameValuePair { Name = "pageSize", Value = pageSize  }
            };
            return new RestSharpHandler().RestRequest<GeneralMovementsModel>(APISelector.Municipality, true, "api/Report/GeneralMovementsGet", "GET", lstParameter);
        }
        #endregion
    }
}
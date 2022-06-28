using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AdministrativeCollectionNotice
    {
        #region public methods      
        public AdministrativeCollectionNoticeModel Get()
        {
            AdministrativeCollectionNoticeModel model = new AdministrativeCollectionNoticeModel();
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public AdministrativeCollectionNoticeModel Get(JQDTParams jqdtParams, int? accountId, bool IsFirstNotice, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            AdministrativeCollectionNoticeModel model = new AdministrativeCollectionNoticeModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, IsFirstNotice, notificationExpirationDate,notificationDate, representativesName);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, null).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountModel.ContactList = new ContactsModel().Get(null, accountId);
            model.AccountModel.PhoneList = new PhoneModel().Get(null, accountId.Value);
            model.AccountModel.EmailList = new EmailModel().Get(null, accountId.Value);
            model.AccountId = accountId;
            model.ReportCompanyModel = GetReportCompany();
            model.NotificationExpirationDate = notificationExpirationDate;
            model.NotificationDate = notificationDate;
            model.RepresentativesName = representativesName;
            return model;
        }
        public AdministrativeCollectionNoticeModel GetByPaging(JQDTParams jqdtParams, int? accountId, bool IsFirstNotice, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "isFirstNotice", Value = IsFirstNotice });
            lstParameter.Add(new NameValuePair { Name = "notificationExpirationDate", Value = notificationExpirationDate == null ? null : notificationExpirationDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "notificationDate", Value = notificationDate == null ? null : notificationDate.Value.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "representativesName", Value = representativesName });
            return new RestSharpHandler().RestRequest<AdministrativeCollectionNoticeModel>(APISelector.Municipality, true, "api/Report/AdministrativeCollectionNoticeGet", "GET", lstParameter);
        }
        public AdministrativeCollectionNoticeModel GetExportLayout(int? accountId, bool IsFirstNotice, DateTime? notificationExpirationDate, DateTime? notificationDate, string representativesName)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, IsFirstNotice, notificationExpirationDate,notificationDate, representativesName);
        }
        #endregion

        #region private methods          
        private ReportCompanyModel GetReportCompany(Nullable<DateTime> FromDate = null, Nullable<DateTime> ToDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.AdministrativeCollectionFirstNoticeTitle, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, FromDate, ToDate);
        }
        #endregion
    }
}
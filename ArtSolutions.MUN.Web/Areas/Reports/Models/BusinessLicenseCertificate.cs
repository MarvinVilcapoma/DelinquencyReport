using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseCertificate
    {
        #region public methods  
        public BusinessLicenseCertificateModel Get()
        {
            BusinessLicenseCertificateModel model = new BusinessLicenseCertificateModel();
            model.AccountList.Insert(0, new SelectListItemViewModel { ID = "", Name = Resources.Global.DropDownSelectMessage });
            List<SalesCashierModel> cashierList = new SalesCashier().Get().OrderBy(x => x.UserName).ToList();
            return model;
        }

        public BusinessLicenseCertificateModel GetByPaging(JQDTParams jqdtParams, int accountId, DateTime futureDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "AccountId", Value = accountId },
                new NameValuePair { Name = "pageIndex", Value = 0  },
                new NameValuePair { Name = "pageSize", Value = Int32.MaxValue},
                new NameValuePair { Name = "FutureDate", Value = futureDate.ToString("d", CultureInfo.InvariantCulture) }};
            return new RestSharpHandler().RestRequest<BusinessLicenseCertificateModel>(APISelector.Municipality, true, "api/Report/BusinessLicenseStatementGetForHP", "GET", lstParameter);
        }

        public BusinessLicenseCertificateModel GetAccountDetail(int accountId)
        {
            BusinessLicenseCertificateModel model = new BusinessLicenseCertificateModel();
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = new ReportCompanyModel().Get();
            model.AccountPaymentPlanID = GetActiveAccountPaymentPlan(model.AccountModel.ID);
            return model;
        }

        public BusinessLicenseCertificateModel Get(int accountId, JQDTParams jqdtParams, DateTime futureDate)
        {
            BusinessLicenseCertificateModel model = new BusinessLicenseCertificateModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, futureDate);
            model.BusinessLicenseCertificateModelList = model.BusinessLicenseCertificateModelList ?? new List<Models.BusinessLicenseCertificateModelList>();
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = new ReportCompanyModel().Get();
            model.AccountPaymentPlanID = GetActiveAccountPaymentPlan(model.AccountModel.ID);
            model.FutureDate = futureDate;

            return model;
        }

        public BusinessLicenseCertificateModel GetExportLayout(int accountId, DateTime futureDate)
        {
            return this.Get(accountId, new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, futureDate);
        }
        #endregion

        #region public methods  
        private string GetActiveAccountPaymentPlan(int accountID)
        {
            List<AccountPaymentPlanModel> paymentPlanList = new Accounts.Models.AccountPaymentPlan().Get(null, accountID, true);
            if (paymentPlanList.Count > 0)
            {
                //List<AccountPaymentPlanModel> ivuPaymentPlanList = paymentPlanList.Where(x => x.ServiceTypeID == 3).ToList();
                List<AccountPaymentPlanModel> ivuPaymentPlanList = paymentPlanList;
                if (ivuPaymentPlanList.Count > 0)
                    return ivuPaymentPlanList.Last().Number;
            }
            return "";
        }
        #endregion
    }
}
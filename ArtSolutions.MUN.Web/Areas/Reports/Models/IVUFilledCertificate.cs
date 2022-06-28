using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class IVUFilledCertificate
    {
        #region public methods
        public IVUFilledCertificateModel Get()
        {
            IVUFilledCertificateModel model = new IVUFilledCertificateModel();           
            model.AccountList.Insert(0, new SelectListItemViewModel { ID = "", Name = Resources.Global.DropDownSelectMessage });
            return model;
        }

        public IVUFilledCertificateModel Get(JQDTParams jqdtParams, int accountId, DateTime futureDate)
        {
            IVUFilledCertificateModel model = new IVUFilledCertificateModel();
            if (jqdtParams != null)
                model = GetByPaging(jqdtParams, accountId, futureDate);
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            model.ReportCompanyModel = new ReportCompanyModel().Get();
            model.FutureDate = futureDate;
            return model;
        }

        public IVUFilledCertificateModel GetByPaging(JQDTParams jqdtParams, int accountId, DateTime futureDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountId", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = 0 });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = Int32.MaxValue });
            lstParameter.Add(new NameValuePair { Name = "FutureDate", Value = futureDate.ToString("d", CultureInfo.InvariantCulture) });
            IVUFilledCertificateModel model = new RestSharpHandler().RestRequest<IVUFilledCertificateModel>(APISelector.Municipality, true, "api/Report/IVUFilledCertificateGetforHP", "GET", lstParameter);
            return model;
        }

        public IVUFilledCertificateModel GetExportLayout(int accountId, DateTime futureDate)
        {
            return this.Get(new JQDTParams
            {
                Start = 0,
                Length = Int32.MaxValue
            }, accountId, futureDate);
        }

        public IVUFilledCertificateModel GetAccountDetail(int accountId)
        {
            IVUFilledCertificateModel model = new IVUFilledCertificateModel();
            model.AccountModel = new AccountModel().Get(accountId.ToString(), 0, null, null, null, null, true).FirstOrDefault();
            model.AccountModel.AddressList = new AddressModel().Get(null, accountId);
            model.AccountId = accountId;
            return model;
        }
        #endregion 
    }
}
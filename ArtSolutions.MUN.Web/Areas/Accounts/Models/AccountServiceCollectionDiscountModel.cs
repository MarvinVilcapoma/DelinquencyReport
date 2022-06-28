using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionDiscountModel
    {
        #region public properties      
        public int AccountServiceID { get; set; }
        public int AccountServiceCollectionDetailId { get; set; }
        public string ServiceNameWithYear { get; set; }
        public decimal Rate { get; set; }
        public decimal Principal { get; set; }
        public decimal Discount { get; set; }
        public string ServiceName { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<decimal> TotalExemption { get; set; }
        public IEnumerable<SelectListItem> CollectionDetailNameList { get; set; }
        public List<AccountServiceCollectionDiscountModel> CollectionDiscount { get; set; }
        #endregion

        #region Constructor
        public AccountServiceCollectionDiscountModel()
        {
            CollectionDetailNameList = new List<SelectListItem>();
            CollectionDiscount = new List<AccountServiceCollectionDiscountModel>();
        }

        public AccountServiceCollectionDiscountModel(int accountServiceId)
        {
            this.AccountServiceID = accountServiceId;
            CollectionDiscount = new List<AccountServiceCollectionDiscountModel>();
        }
        #endregion

        #region public methods  
        public List<AccountServiceCollectionDiscountModel> Get(int accountServiceId, int? accountServiceCollectionDetailId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountServiceId });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailID", Value = accountServiceCollectionDetailId });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionDiscountModel>>(APISelector.Municipality, true, "api/AccountService/CollectionDiscountGet", "GET", lstParameter);
        }

        public AccountServiceCollectionDiscountModel Get()
        {
            List<AccountServiceCollectionDetailModel> collectionDetail = new AccountServiceCollectionDetailModel().Get(this.AccountServiceID, null, null);
            if (collectionDetail.Count > 4)
            {
                this.CollectionDetailNameList = HMTLHelperExtensions.CreateSelectList(collectionDetail, "ID", "FrequnecyMonthly");
            }
            else
                this.CollectionDetailNameList = HMTLHelperExtensions.CreateSelectList(collectionDetail, "ID", "ServiceFrequencyName");
            return this;
        }
        #endregion
    }
}
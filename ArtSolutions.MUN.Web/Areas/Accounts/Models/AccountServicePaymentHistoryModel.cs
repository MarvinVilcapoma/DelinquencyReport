using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServicePaymentHistoryModel
    {
        #region properties
        public List<AccountServiceCollectionPaymentHistoryModel> CollectionPaymentHistory { get; set; }
        public IEnumerable<SelectListItem> CollectionDetailNameList { get; set; }
        public int AccountServiceId { get; set; }
        #endregion

        #region Constructor       
        public AccountServicePaymentHistoryModel()
        {
            CollectionDetailNameList = new List<SelectListItem>();
            CollectionPaymentHistory = new List<AccountServiceCollectionPaymentHistoryModel>();
        }

        public AccountServicePaymentHistoryModel(int accountServiceId)
        {
            this.AccountServiceId = accountServiceId;
            CollectionPaymentHistory = new List<AccountServiceCollectionPaymentHistoryModel>();
        }
        #endregion

        #region public methods
        public AccountServicePaymentHistoryModel Get()
        {
            List<AccountServiceCollectionDetailModel> collectionDetail = new AccountServiceCollectionDetailModel().Get(this.AccountServiceId, null, null);
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
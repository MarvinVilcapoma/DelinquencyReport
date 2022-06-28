using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceDebtModel
    {
        #region properties
        public List<AccountServiceCollectionDebtModel> CollectionDebt { get; set; }
        public IEnumerable<SelectListItem> CollectionDetailNameList { get; set; }
        public int AccountServiceId { get; set; }
        #endregion

        #region Constructor
        public AccountServiceDebtModel()
        {
            CollectionDetailNameList = new List<SelectListItem>();
            CollectionDebt = new List<AccountServiceCollectionDebtModel>();
        }

        public AccountServiceDebtModel(int accountServiceId)
        {
            this.AccountServiceId = accountServiceId;
            CollectionDebt = new List<AccountServiceCollectionDebtModel>();
        }
        #endregion

        #region public methods
        public AccountServiceDebtModel Get()
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class BusinessLicenseAccountServiceModel
    {
        #region public properties
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Principal { get; set; }
        public decimal Penalties { get; set; }
        public decimal? Discount { get; set; }
        public decimal Debt { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Total { get; set; }
        public bool SelectedItem { get; set; }
        public string AccountServiceCollectionDetailJSON { get; set; }
        public int FiscalYearID { get; set; }
        #endregion

        #region public methods
        public List<BusinessLicenseAccountServiceModel> GetCollectionDetailSummary(List<AccountServiceCollectionDetailModel> detailList)
        {
            List<BusinessLicenseAccountServiceModel> resultList = new List<BusinessLicenseAccountServiceModel>();
            foreach (var model in detailList)
            {
                if (resultList.Where(x => x.ID == model.AccountServiceID).Count() == 0)
                {
                    BusinessLicenseAccountServiceModel obj = new BusinessLicenseAccountServiceModel();
                    obj.ID = model.AccountServiceID??0;
                    obj.ServiceName = model.ServiceName;
                    obj.DueDate = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).LastOrDefault().DueDate.Value;
                    obj.Principal = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.Principal);
                    obj.Penalties = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.Penalties);
                    obj.Discount = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.Discount);
                    obj.Debt = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.Debt);
                    obj.PaidAmount = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.PaidAmount);
                    obj.Total = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).Sum(x => x.Total);
                    obj.AccountServiceCollectionDetailJSON = JsonConvert.SerializeObject(detailList.Where(x => x.AccountServiceID == model.AccountServiceID).ToList());
                    obj.FiscalYearID = detailList.Where(x => x.AccountServiceID == model.AccountServiceID).FirstOrDefault().FiscalYearID??0;
                    resultList.Add(obj);
                }
            }
            return resultList;
        }

        public List<AccountServiceCollectionDetailModel> GetCollectionDetailList(List<BusinessLicenseAccountServiceModel> businessLicenseAccountServiceList)
        {
            List<AccountServiceCollectionDetailModel> resultList = new List<AccountServiceCollectionDetailModel>();
            foreach (var model in businessLicenseAccountServiceList)
            {
                resultList.AddRange(JsonConvert.DeserializeObject<List<AccountServiceCollectionDetailModel>>(model.AccountServiceCollectionDetailJSON));
            }
            return resultList;
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCollectionTemplateModel
    {
        #region Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Locale { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion
        #region Reference Properties
        public List<ServiceCollectionTemplateDetailModel> ServiceCollectionTemplateDetailList { get; set; }
        public List<ServiceCollectionTemplateDetailModel> ServiceCollectionTemplateDetailListUTD { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public List<FinClassGrantModel> GrantList { get; set; }
        public IEnumerable<SelectListItem> CollectionTypeList { get; set; }
        #endregion
        #region Custom Properties
        public bool EnableViewMode { get; set; }
        public Nullable<bool> IsPublic { get; set; }

        #endregion
        #region Methods
        public List<ServiceCollectionTemplateModel> Get(int? id, bool? isActive, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "IsActive", Value = isActive });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            return new RestSharpHandler().RestRequest<List<ServiceCollectionTemplateModel>>(APISelector.Municipality, true, "api/CollectionTemplate/Get", "GET", lstParameter) ?? new List<ServiceCollectionTemplateModel>();
        }
        public ServiceCollectionTemplateListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            return new RestSharpHandler().RestRequest<ServiceCollectionTemplateListModel>(APISelector.Municipality, true, "api/CollectionTemplate/GetWithPaging", "GET", lstParameter, null);
        }
        public void Get()
        {
            this.ServiceCollectionTemplateDetailList = new List<ServiceCollectionTemplateDetailModel>();
            BindDropDown(this, true, true, true);
            // Add Default LineItems
            foreach (var year in this.YearList)
            {
                foreach (var collectionType in this.CollectionTypeList)
                {
                    ServiceCollectionTemplateDetailModel item = new ServiceCollectionTemplateDetailModel();
                    item.YearID = int.Parse(year.Value);
                    item.CollectionTypeID = int.Parse(collectionType.Value);
                    this.ServiceCollectionTemplateDetailList.Add(item);
                }
            }
        }
        public int Insert(ServiceCollectionTemplateModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ServiceCollectionTemplateDetailListUTD = model.ServiceCollectionTemplateDetailList;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/CollectionTemplate/Insert", "POST", null, lstObjParameter);
        }
        public int Update(ServiceCollectionTemplateModel model)
        {
            model.IsActive = true;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ServiceCollectionTemplateDetailListUTD = model.ServiceCollectionTemplateDetailList;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/CollectionTemplate/Update", "POST", null, lstObjParameter);
        }
        public ServiceCollectionTemplateModel AddDetail(int yearId)
        {
            if (this.ServiceCollectionTemplateDetailList == null)
                this.ServiceCollectionTemplateDetailList = new List<ServiceCollectionTemplateDetailModel>();
            this.ServiceCollectionTemplateDetailList = this.ServiceCollectionTemplateDetailList.Where(x => x.IsRemoved == false).ToList();

            if (ValidateItems(this.ServiceCollectionTemplateDetailList, this.YearList, this.CollectionTypeList, false, false, false))
            {
                foreach (var item in this.ServiceCollectionTemplateDetailList)
                {
                    var finAccounts = new FinGrantAccountModel().Get(item.GrantID, EnumUtility.FINAccountType.AR);
                    item.ReceivableAccountList = finAccounts.ReceivableAccountList.OrderBy(x => x.NameFriendly).ToList();
                    item.RevenueAccountList = finAccounts.RevenueAccountList.OrderBy(x => x.NameFriendly).ToList();
                    item.CheckbookAccountList = finAccounts.CheckbookAccountList.OrderBy(x => x.NameFriendly).ToList();
                }
                this.ServiceCollectionTemplateDetailList.Insert(ServiceCollectionTemplateDetailList.FindLastIndex(x => x.YearID == yearId) + 1, new ServiceCollectionTemplateDetailModel() { YearID = yearId });
                BindDropDown(this, true, false, false);
            }
            return this;
        }
        public bool ValidateItems(List<ServiceCollectionTemplateDetailModel> templateDetailList, IEnumerable<SelectListItem> yearList, IEnumerable<SelectListItem> collectionTypeList, bool checkForPercentage, bool checkForMinEntry, bool checkforSameTypeForAllYear)
        {
            BindDropDown(this, false, true, true);
            //Check for duplicate same entry
            var duplicateEntries = templateDetailList
                            .GroupBy(x => new { x.YearID, x.CollectionTypeID, x.GrantID, x.ReceivableAccountID, x.RevenueAccountID, x.Percentage })
                            .Where(g => g.Count() > 1)
                            .ToDictionary(x => x.Key.YearID, y => y.Key.CollectionTypeID);
            string errMsg = "";
            foreach (KeyValuePair<int, int> entry in duplicateEntries)
            {
                if (entry.Key > 0 && entry.Value > 0)
                {
                    if (errMsg == "")
                        errMsg = Resources.ServiceCollectionTemplate.DuplicateEntryMessage + " <br />";
                    errMsg += Resources.Global.Year + " : " + YearList.SingleOrDefault(s => s.Value == entry.Key.ToString()).Text
                        + "," + Resources.Global.Type + " : " + CollectionTypeList.SingleOrDefault(s => s.Value == entry.Value.ToString()).Text + " <br />";
                }
            }
            if (errMsg != "")
                throw new Exception(errMsg);

            //Percentage should be 100 for collection of each year.
            if (checkForPercentage)
            {
                var percentageEntries_NotValid = templateDetailList
                   .GroupBy(x => new { x.YearID, x.CollectionTypeID })
                   .Select(x => new { x.Key.YearID, x.Key.CollectionTypeID, TotalPercentage = x.Sum(y => y.Percentage) })
                   .Where(x => x.TotalPercentage != 100).ToList();
                foreach (var item in percentageEntries_NotValid)
                {
                    if (item.YearID > 0 && item.CollectionTypeID > 0)
                    {
                        if (errMsg == "")
                            errMsg = Resources.ServiceCollectionTemplate.PercentageNotValidMessage + " <br />";
                        errMsg += Resources.Global.Year + " : " + YearList.SingleOrDefault(s => s.Value == item.YearID.ToString()).Text
                            + "," + Resources.Global.Type + " : " + CollectionTypeList.SingleOrDefault(s => s.Value == item.CollectionTypeID.ToString()).Text + " <br />";
                    }
                }
                if (errMsg != "")
                    throw new Exception(errMsg);
            }
            if (checkForMinEntry)
            {
                // Check for atleast one item for each year for principal collection type.
                var cnt = templateDetailList.Where(x => x.CollectionTypeID == 1).ToList().GroupBy(x => x.YearID).ToList().Count;
                if (cnt < 3)
                    throw new Exception(ArtSolutions.MUN.Web.Resources.ServiceCollectionTemplate.ValMinTemplateItem);
            }
            if (checkforSameTypeForAllYear)
            {
                var CntCharges = templateDetailList.Where(x => x.CollectionTypeID == 2).ToList().GroupBy(x => x.YearID).ToList().Count;
                var CntPenalty = templateDetailList.Where(x => x.CollectionTypeID == 3).ToList().GroupBy(x => x.YearID).ToList().Count;
                var CntInterest = templateDetailList.Where(x => x.CollectionTypeID == 4).ToList().GroupBy(x => x.YearID).ToList().Count;
                if ((CntCharges > 0 && CntCharges < 3) || (CntPenalty > 0 && CntPenalty < 3) || (CntInterest > 0 && CntInterest < 3))
                    throw new Exception(ArtSolutions.MUN.Web.Resources.ServiceCollectionTemplate.ValCollectionTypeSameForEachYear);
            }
            return true;
        }
        public void BindDropDown(ServiceCollectionTemplateModel model, bool getGrant, bool getYear, bool getType)
        {
            if (getGrant)
                model.GrantList = new FinClassGrantModel().Get(null, true, null, true).OrderBy(x => x.Name).ToList();
            if (getYear)
                model.YearList = new SelectList(new AccountModel().GetSupportValues(15), "ID", "Name");
            if (getType)
                model.CollectionTypeList = HMTLHelperExtensions.CreateSelectList(new CollectionTypeModel().Get(), "ID", "Name");
        }
        public ServiceCollectionTemplateModel Edit(int id)
        {
            ServiceCollectionTemplateModel model = new ServiceCollectionTemplateModel();
            model = Get(id, null, null).FirstOrDefault();
            if (model == null || model.ID == 0)
                throw new Exception(ArtSolutions.MUN.Web.Resources.ServiceCollectionTemplate.InvalidTemplateNumber);
            model.ServiceCollectionTemplateDetailList = new ServiceCollectionTemplateDetailModel().Get(null, model.ID);
            BindDropDown(model, true, true, true);
            // Get Receivable & Revenue Account List
            foreach (var item in model.ServiceCollectionTemplateDetailList)
            {
                var finAccounts = new FinGrantAccountModel().Get(item.GrantID, EnumUtility.FINAccountType.AR);
                item.ReceivableAccountList = finAccounts.ReceivableAccountList.OrderBy(x => x.NameFriendly).ToList();
                item.RevenueAccountList = finAccounts.RevenueAccountList.OrderBy(x => x.NameFriendly).ToList();
                item.CheckbookAccountList = finAccounts.CheckbookAccountList.OrderBy(x => x.NameFriendly).ToList();
            }
            return model;
        }
        #endregion
    }
    public class ServiceCollectionTemplateListModel
    {
        public List<ServiceCollectionTemplateModel> CollectionTemplateList { get; set; }
        public int TotalRecord { get; set; }
    }
}
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingUnitModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        //public decimal Length { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        //public decimal Width { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal Value { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total { get; set; }
        public decimal? ReturnAmount { get; set; }
        public decimal? PurchasesubjectToTax { get; set; }
        public System.Guid FillingBy { get; set; }
        public bool IsFromPortal { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public string CommaSeperatedImageIDs { get; set; }
        public Nullable<System.DateTime> FillingDate { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int AccountServiceID { get; set; }
        public int? IsEditPermission { get; set; }
        public List<AccountServiceCollectionFillingImagesModel> CollectionFillingImagesList { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public string Notes { get; set; }
        #endregion

        #region Constructor       
        public AccountServiceCollectionFillingUnitModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingUnitModel Get(int accountServiceCollectionID, int fillingType)
        {
            AccountServiceCollectionFillingUnitModel model = new AccountServiceCollectionFillingUnitModel();
            if (fillingType == 1) //Filling Unit
            {
                model = GetCollectionFillingUnit(accountServiceCollectionID, null).FirstOrDefault();
                var objImageList=new AccountServiceCollectionFillingImagesModel().GetCollectionFillingUnitImages(model.ID);
                model.CollectionFillingImagesList = objImageList != null ? objImageList : new List<AccountServiceCollectionFillingImagesModel>();
            }
            return model;
        }

        private List<AccountServiceCollectionFillingUnitModel> GetCollectionFillingUnit(int accountServiceCollectionID, int? fillingUnitID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingUnitID", Value = fillingUnitID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingUnitModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingUnitGet", "GET", lstParameter);
        }

        public AccountServiceCollectionFillingUnitModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingUnitModel fillingModel = model.CollectionFillingUnitModel;
            fillingModel.AccountServiceCollectionID = model.AccountServiceCollectionDetailId;
            fillingModel.FillingDate = model.FillingDate;
            fillingModel.CreatedUserID = UserSession.Current.Id;
            fillingModel.CreatedDate = Common.CurrentDateTime;
            fillingModel.ModifiedUserID = UserSession.Current.Id;
            fillingModel.ModifiedDate = Common.CurrentDateTime;
            fillingModel.RowVersion = model.RowVersion;
            fillingModel.IsDeleted = false;
            fillingModel.IsActive = true;
            fillingModel.IsFromPortal = false;
            fillingModel.FillingBy = UserSession.Current.Id;
            fillingModel.ApplyToAllYear = true;
            if (!string.IsNullOrEmpty(fillingModel.CommaSeperatedImageIDs))
            {
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Trim(',');
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Replace(",,", ",");
            }

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(fillingModel);
            if (fillingModel.ID > 0)
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingUnitModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingUnitUpdate", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingUnitModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingUnitInsert", "POST", null, lstObjParameter);
        }

        public decimal GetFillingUnitRule(int AccountServiceCollectionID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = AccountServiceCollectionID });
            return new RestSharpHandler().RestRequest<decimal>(APISelector.Municipality, true, "api/AccountService/CollectionFillingUnitRuleGet", "GET", lstParameter, null);
        }

        public void Delete(int ID, string Notes)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.AccountServiceCollectionID = ID;
            this.Notes = Notes;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteFillingUnit", "POST", null, lstObjParameter);
        }


        #endregion
    }
}
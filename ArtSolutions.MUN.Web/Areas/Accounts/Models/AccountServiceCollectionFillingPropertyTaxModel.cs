using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingPropertyTaxModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }
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
        public List<AccountPropertyModel> AccountPropertyList { get; set; }
        public Nullable<System.DateTime> ReFillingDate { get; set; }
        public string PropertyTaxJson { get; set; }
        public decimal? ExemptionAmount { get; set; }
        public Nullable<int> MovementTypeID { get; set; }
        public List<SelectListItem> MovementTypeList { get; set; }
        public Nullable<decimal> Area { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "TerrainValueRangeValidation", ErrorMessageResourceType = typeof(Resources.AccountProperty))]
        public Nullable<decimal> TerrainValue { get; set; }
        public int PropertyAccountID { get; set; }                
        public string PropertyNumber { get; set; }
        public string RigthNumber { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        #endregion

        #region Constructor       
        public AccountServiceCollectionFillingPropertyTaxModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
            AccountPropertyList = new List<AccountPropertyModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingPropertyTaxModel Get(int accountServiceCollectionID)
        {
            AccountServiceCollectionFillingPropertyTaxModel model = new AccountServiceCollectionFillingPropertyTaxModel();
            model = GetCollectionFillingPropertyTax(accountServiceCollectionID, null).FirstOrDefault();
            var objImageList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingPropertyTaxImages(model.ID);
            model.CollectionFillingImagesList = objImageList != null ? objImageList : new List<AccountServiceCollectionFillingImagesModel>();
            model.MovementTypeList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetSupportValues(39), "ID", "Name");
            return model;
        }

        private List<AccountServiceCollectionFillingPropertyTaxModel> GetCollectionFillingPropertyTax(int accountServiceCollectionID, int? fillingPropertyTaxID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingPropertyTaxID", Value = fillingPropertyTaxID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingPropertyTaxModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingPropertyTaxGet", "GET", lstParameter);
        }

        public AccountServiceCollectionFillingPropertyTaxModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingPropertyTaxModel fillingModel = model.CollectionFillingPropertyTaxModel;
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
            fillingModel.PropertyTaxJson = new JavaScriptSerializer().Serialize(model.CollectionFillingPropertyTaxModel.AccountPropertyList);

            if (!string.IsNullOrEmpty(fillingModel.CommaSeperatedImageIDs))
            {
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Trim(',');
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Replace(",,", ",");
            }

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(fillingModel);
            if (fillingModel.ID > 0)
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingPropertyTaxModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingPropertyTaxUpdate", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingPropertyTaxModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingPropertyTaxInsert", "POST", null, lstObjParameter);
        }
        public void Delete(int ID, string Notes)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.AccountServiceCollectionID = ID;
            this.Notes = Notes;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteFillingPropertyTax", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
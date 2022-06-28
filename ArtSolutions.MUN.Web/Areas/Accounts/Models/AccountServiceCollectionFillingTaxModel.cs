using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingTaxModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }
        public decimal FormIVUTreasury { get; set; }
        public decimal PurchasesImportsResale { get; set; }
        public decimal ImportsUse { get; set; }
        public decimal UseofInventory { get; set; }
        public decimal TotalUseSubjectIVU { get; set; }
        public decimal TaxableFurnitureSale { get; set; }
        public decimal TaxableServicesSale { get; set; }
        public decimal TaxableAdmissions { get; set; }
        public decimal TaxableItemsReturns { get; set; }
        public decimal TotalTaxableSales { get; set; }
        public decimal ExemptFurnitureSale { get; set; }
        public decimal ExemptServicesSale { get; set; }
        public decimal ExemptAdmissions { get; set; }
        public decimal ExemptReturns { get; set; }
        public decimal TotalExemptSales { get; set; }
        public decimal CreditSaleProperty { get; set; }
        public decimal CreditUncollectibleAccounts { get; set; }
        public decimal TaxCreditPaid { get; set; }
        public decimal Total { get; set; }
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
        #endregion

        #region Constructor       
        public AccountServiceCollectionFillingTaxModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingTaxModel Get(int accountServiceCollectionID, int fillingType)
        {
            AccountServiceCollectionFillingTaxModel model = new AccountServiceCollectionFillingTaxModel();
            model = GetCollectionFillingTax(accountServiceCollectionID, null).FirstOrDefault();
            model.CollectionFillingImagesList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingTaxImages(model.ID);
            return model;
        }

        public List<AccountServiceCollectionFillingTaxModel> GetCollectionFillingTax(int accountServiceCollectionID, int? fillingTaxID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingTaxID", Value = fillingTaxID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingTaxModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxGet", "GET", lstParameter);
        }

        public AccountServiceCollectionFillingTaxModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingTaxModel fillingModel = model.CollectionFillingTaxModel;
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

            if (!string.IsNullOrEmpty(fillingModel.CommaSeperatedImageIDs))
            {
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Trim(',');
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Replace(",,", ",");
            }

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(fillingModel);
            if (fillingModel.ID > 0)
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingTaxModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxUpdate", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingTaxModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxInsert", "POST", null, lstObjParameter);
        }
        #endregion
    }

    public class AccountServiceCollectionFillingImagesModel
    {
        #region properties
        public int ImageID { get; set; }
        public int FillingID { get; set; }
        public string FileName { get; set; }
        #endregion

        #region public methods
        public List<AccountServiceCollectionFillingImagesModel> GetCollectionFillingLicenseImages(int fillingID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fillingID", Value = fillingID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingImagesModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseImagesGet", "GET", lstParameter);
        }

        public List<AccountServiceCollectionFillingImagesModel> GetCollectionFillingTaxImages(int fillingID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fillingID", Value = fillingID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingImagesModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxImagesGet", "GET", lstParameter);
        }

        public List<AccountServiceCollectionFillingImagesModel> GetCollectionFillingUnitImages(int fillingID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fillingID", Value = fillingID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingImagesModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingUnitImagesGet", "GET", lstParameter);
        }

        public List<AccountServiceCollectionFillingImagesModel> GetCollectionFillingMeasuredWaterImages(int fillingID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fillingID", Value = fillingID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingImagesModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingMeasuredWaterImagesGet", "GET", lstParameter);
        }

        public List<AccountServiceCollectionFillingImagesModel> GetCollectionFillingPropertyTaxImages(int fillingID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fillingID", Value = fillingID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingImagesModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingPropertyTaxImagesGet", "GET", lstParameter);
        }
        #endregion
    }
}
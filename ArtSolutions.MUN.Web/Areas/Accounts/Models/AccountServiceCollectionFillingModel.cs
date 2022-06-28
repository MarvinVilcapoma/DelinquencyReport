using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessageResourceName = "RequiredMinValueMsg", ErrorMessageResourceType = typeof(Global))]
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal GrossVolume { get; set; }
        public decimal? ExemptionAmount { get; set; }
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
        public List<AccountServiceCollectionFillingImagesModel> CollectionFillingImagesList { get; set; }
        #endregion

        #region Constructor       
        public AccountServiceCollectionFillingModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingModel Get(int accountServiceCollectionID,int fillingType)
        {
            AccountServiceCollectionFillingModel model = new AccountServiceCollectionFillingModel();
            if (fillingType == 1) //Filling License
            {
                model = GetCollectionFillingLicense(accountServiceCollectionID,null).FirstOrDefault();
                model.CollectionFillingImagesList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingLicenseImages(model.ID);
            }
            else //Filling Tax
            {
                model = GetCollectionFillingTax(accountServiceCollectionID,null).FirstOrDefault();
                model.CollectionFillingImagesList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingTaxImages(model.ID);
            }
            return model;
        }
        public List<AccountServiceCollectionFillingModel> GetCollectionFillingLicense(int accountServiceCollectionID,int? fillingLicenseID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingLicenseID", Value = fillingLicenseID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseGet", "GET", lstParameter);
        }
        public List<AccountServiceCollectionFillingModel> GetCollectionFillingTax(int accountServiceCollectionID,int? fillingTaxID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingTaxID", Value = fillingTaxID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxGet", "GET", lstParameter);
        }
        public AccountServiceCollectionFillingModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingModel fillingModel = model.CollectionFillingModel;
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

            if (model.GroupID == (int)EnServiceGroup.License)
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseInsert", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingTaxInsert", "POST", null, lstObjParameter);
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
        #endregion
    }
}
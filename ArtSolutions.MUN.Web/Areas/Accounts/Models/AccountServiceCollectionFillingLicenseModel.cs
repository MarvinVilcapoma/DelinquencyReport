using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingLicenseModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal GrossVolume { get; set; }
        public decimal? ExemptionAmount { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(1, 100, ErrorMessageResourceName = "ValidPercentageValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal? PercentageValue { get; set; }

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
        public AccountServiceCollectionFillingLicenseModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingLicenseModel Get(int accountServiceCollectionID, int filingFormID, int fillingType)
        {
            AccountServiceCollectionFillingLicenseModel model = new AccountServiceCollectionFillingLicenseModel();
            if (fillingType == 1) 
            {
                model = GetCollectionFillingLicense(accountServiceCollectionID, null).FirstOrDefault();

                if (filingFormID == 6)
                    model.Total = (model.GrossVolume * model.PercentageValue.Value / 100);

                model.CollectionFillingImagesList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingLicenseImages(model.ID);
            }

            return model;
        }

        private List<AccountServiceCollectionFillingLicenseModel> GetCollectionFillingLicense(int accountServiceCollectionID, int? fillingLicenseID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingLicenseID", Value = fillingLicenseID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingLicenseModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseGet", "GET", lstParameter);
        }

        public AccountServiceCollectionFillingLicenseModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingLicenseModel fillingModel = model.CollectionFillingModel;
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
            if (fillingModel.ID <= 0)
                fillingModel.ApplyToAllYear = true;

            if (!string.IsNullOrEmpty(fillingModel.CommaSeperatedImageIDs))
            {
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Trim(',');
                fillingModel.CommaSeperatedImageIDs = fillingModel.CommaSeperatedImageIDs.Replace(",,", ",");
            }

            if (model.FilingFormID == 6)
                fillingModel.Total = fillingModel.GrossVolume;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(fillingModel);
            if (fillingModel.ID > 0)
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingLicenseModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseUpdate", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingLicenseModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingLicenseInsert", "POST", null, lstObjParameter);
        }

        public void Delete(int ID, string Notes)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.AccountServiceCollectionID = ID;
            this.Notes = Notes;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteFillingLicense", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
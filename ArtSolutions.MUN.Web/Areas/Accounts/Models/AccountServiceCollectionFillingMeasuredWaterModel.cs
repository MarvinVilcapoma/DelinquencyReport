using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServiceCollectionFillingMeasuredWaterModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int AccountServiceCollectionID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public bool? ApplyToAllYear { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal PreviousMeasure { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal ActualMeasure { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal WaterConsumption { get; set; }
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
        public AccountServiceCollectionFillingMeasuredWaterModel()
        {
            CollectionFillingImagesList = new List<AccountServiceCollectionFillingImagesModel>();
        }
        #endregion

        #region public methods  
        public AccountServiceCollectionFillingMeasuredWaterModel Get(int accountServiceCollectionID)
        {
            AccountServiceCollectionFillingMeasuredWaterModel model = new AccountServiceCollectionFillingMeasuredWaterModel();
            model = GetCollectionFillingMeasuredWater(accountServiceCollectionID, null).FirstOrDefault();
            if (model != null)
            {
                var objImageList = new AccountServiceCollectionFillingImagesModel().GetCollectionFillingMeasuredWaterImages(model.ID);
                model.CollectionFillingImagesList = objImageList != null ? objImageList : new List<AccountServiceCollectionFillingImagesModel>();
            }
            return model;
        }
        public decimal PreviousMeasureGet(int accountServiceCollectionID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });

            return new RestSharpHandler().RestRequest<decimal>(APISelector.Municipality, true, "api/AccountService/CollectionFillingPreviousMeasuredWaterGet", "GET", lstParameter);
        }

        private List<AccountServiceCollectionFillingMeasuredWaterModel> GetCollectionFillingMeasuredWater(int accountServiceCollectionID, int? fillingMeasuredWaterID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionID", Value = accountServiceCollectionID });
            lstParameter.Add(new NameValuePair { Name = "fillingMeasuredWaterID", Value = fillingMeasuredWaterID });
            return new RestSharpHandler().RestRequest<List<AccountServiceCollectionFillingMeasuredWaterModel>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingMeasuredWaterGet", "GET", lstParameter);
        }

        public AccountServiceCollectionFillingMeasuredWaterModel Filling(AccountServiceFillingModel model)
        {
            AccountServiceCollectionFillingMeasuredWaterModel fillingModel = model.CollectionFillingMeasuredWaterModel;
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
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingMeasuredWaterModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingMeasuredWaterUpdate", "POST", null, lstObjParameter);
            else
                return new RestSharpHandler().RestRequest<AccountServiceCollectionFillingMeasuredWaterModel>(APISelector.Municipality, true, "api/AccountService/CollectionFillingMeasuredWaterInsert", "POST", null, lstObjParameter);
        }

        public void Delete(int ID, string Notes)
        {
            this.CreatedUserID = UserSession.Current.Id;
            this.CreatedDate = Common.CurrentDateTime;
            this.AccountServiceCollectionID = ID;
            this.Notes = Notes;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(this);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/DeleteFillingMeasuredWater", "POST", null, lstObjParameter);
        }

        public MemoryStream CollectionFillingMeasuredWaterExport(int? AccountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = AccountID });
            List<string> data = new RestSharpHandler().RestRequest<List<string>>(APISelector.Municipality, true, "api/AccountService/CollectionFillingMeasuredWaterExport", "GET", lstParameter);

            string ExportData = "";
            if (data != null && data.Count > 0)
            {
                data.ForEach(d =>
                {
                    ExportData = ExportData + d + "\r\n";
                });
            }
            var byteArray = Encoding.Default.GetBytes(ExportData);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }
        #endregion
    }
}
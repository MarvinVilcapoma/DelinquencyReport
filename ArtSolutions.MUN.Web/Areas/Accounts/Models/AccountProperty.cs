using ArtSolutions.MUN.Web.Areas.Cases.Models;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountProperty
    {
        #region Public Methods              
        public AccountPropertyModel Get(int? ownerID)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = FillDropdownDetail(model, null);
            if (ownerID > 0)
                model.OwnerID = ownerID.Value;
            return model;
        }

        public int Insert(AccountPropertyModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Insert", "POST", null, lstObjParameter);
        }

        public int Delete(int id)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model.ID = id;
            model.IsActive = false;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Delete", "POST", null, lstObjParameter);
        }

        public int AccountPropertyRightUpdate(int id, string PropertyNumber, string RigthNumber, Nullable<int> CondoID, Nullable<int> CondoTypeID)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model.ID = id;
            model.PropertyNumber = PropertyNumber;
            model.RigthNumber = RigthNumber;
            model.CondoID = CondoID;
            model.CondoTypeID = CondoTypeID;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/AccountPropertyRightUpdate", "POST", null, lstObjParameter);
        }

        public int Split(AccountPropertyModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedBy = UserSession.Current.Username;
            var ObjUserProfile = new UserProfile().GetUserByUserIDs(model.AssignToID.ToString()).FirstOrDefault();
            model.AssignToUserEmail = ObjUserProfile.Email;
            model.AssignToName = ObjUserProfile.FullName;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Split", "POST", null, lstObjParameter);
        }
        public int EditSplit(PropertyTransferModel model)
        {
            AccountPropertyModel data = new AccountPropertyModel();
            data.ID = model.TransferID;
            data.OldAccountID = model.OldAccountID;
            data.AccountPropertyID = model.AccountPropertyID.Value;
            data.SplitDate = model.TransferDate;
            data.AccountPropertyConstructionDetailJson = model.OldAccountProperty.AccountPropertyConstructionDetailJson;
            data.workflowID = model.workflowID;
            data.workflowStatusID = model.workflowStatusID;
            data.workflowID = model.workflowID;
            data.CreatedUserID = UserSession.Current.Id;
            data.CreatedDate = Common.CurrentDateTime;
            data.ModifiedUserID = UserSession.Current.Id;
            data.ModifiedDate = Common.CurrentDateTime;
            data.CreatedBy = UserSession.Current.Username;
            data.AssignToID = model.AssignToID;
            data.ReasonID = model.ReasonID;
            data.Comments = model.Comments;
            data.Property = model.Property;
            var ObjUserProfile = new UserProfile().GetUserByUserIDs(model.AssignToID.ToString()).FirstOrDefault();
            data.AssignToUserEmail = ObjUserProfile.Email;
            data.AssignToName = ObjUserProfile.FullName;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(data);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Split", "POST", null, lstObjParameter);
        }
        public bool DeleteSplit(int id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "TransferID", Value = id });
            //lstParameter.Add(new NameValuePair { Name = "Reason", Value = reason });
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountProperty/DeleteSplit", "GET", lstParameter, null);
        }

        public int Merge(AccountPropertyModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedBy = UserSession.Current.Username;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Merge", "POST", null, lstObjParameter);
        }
        public int? GetMergeCheckNotAssociatedServices(string commaSeparatedPropertyID, int? AccountID = null)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "commaSeparatedPropertyID", Value = commaSeparatedPropertyID });
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = AccountID });
            return new RestSharpHandler().RestRequest<int?>(APISelector.Municipality, true, "api/AccountProperty/MergeCheckNotAssociatedServices", "GET", lstParameter, null);
        }

        public int Update(AccountPropertyModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/Update", "POST", null, lstObjParameter);
        }

        public int UpdateStatus(bool isActive, int id)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model.ID = id;
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/ActiveInactive", "POST", null, lstObjParameter);
        }

        public bool HasDebt(int id, int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountProperty/AccountPropertyHasDebt", "GET", lstParameter, null);
        }

        public AccountPropertyModel View(int? id, bool? isActive = null)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = Get(id, isActive);
            model = GetAccountDetails(model, id);
            model = FillDropdownDetail(model, 3);
            return model;
        }

        public AccountPropertyModel Edit(int? id, bool? isActive = null)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = Get(id, isActive);
            model = FillDropdownDetail(model, null);
            return model;
        }

        public AccountPropertyModel GetAccountPropertyDetail(int id, int TransferID = 0)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = Get(id, null, TransferID);
            return model;
        }

        public AccountPropertyModel GetAccountPropertyOnly(int? id, bool? isActive = null)
        {
            AccountPropertyModelDetail AccountPropertyDetail = new AccountPropertyModelDetail();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
            AccountPropertyDetail = new RestSharpHandler().RestRequest<AccountPropertyModelDetail>(APISelector.Municipality, true, "api/AccountProperty/Get", "GET", lstParameter, null);
            return AccountPropertyDetail.AccountPropertyModelList.FirstOrDefault();
        }

        public AccountPropertyModel Get(int? id, bool? isActive = null, int TransferID = 0)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model = new AccountProperty().Get(null);
            AccountPropertyModelDetail AccountPropertyDetail = new AccountPropertyModelDetail();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
            lstParameter.Add(new NameValuePair { Name = "transferID", Value = TransferID });
            AccountPropertyDetail = new RestSharpHandler().RestRequest<AccountPropertyModelDetail>(APISelector.Municipality, true, "api/AccountProperty/Get", "GET", lstParameter, null);

            if (AccountPropertyDetail.AccountPropertyModelList.Count > 0)
            {
                model = AccountPropertyDetail.AccountPropertyModelList.FirstOrDefault();
                model.AccountPropertyConstructionDetail = AccountPropertyDetail.AccountPropertyConstructionModelList;
                model.AccountPropertyConstructionDetailJson = new JavaScriptSerializer().Serialize(BindJsonModal(model));
            }
            model.Address = new AddressModel();
            model.Address.AddressLine1 = model.AddressLine1;
            model.Address.AddressLine2 = model.AddressLine2;
            model.Address.ZipPostalCode = model.ZipPostalCode;
            model.Address.Country = model.CountryName;
            model.Address.State = model.StateName;
            model.Address.City = model.CityName;
            model.Address.Town = model.TownName;
            return model;
        }
        private AccountPropertyModel GetAccountDetails(AccountPropertyModel Model, int? ID)
        {
            Model.AccountDetail = new AccountModel();
            Model.AccountDetail.AccountType = 6;
            Model.AccountDetail.ID = ID.Value;
            Model.AccountDetail.AccountID = Model.AccountID;
            Model.AccountDetail.IsActive = Model.IsActive;
            return Model;
        }
        private AccountPropertyModel FillDropdownDetail(AccountPropertyModel model, int? actionType)
        {
            if (actionType != 3) // actionType= 3 (view)
            {
                model.CondoList = GetSupportValues(19).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.CondoTypeList = GetSupportValues(20).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.MainAccessList = GetSupportValues(21).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.SlopeList = GetSupportValues(22).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).OrderBy(x => x.Value).ToList();
                model.LevelTypeList = GetSupportValues(23).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.BlockLocationList = GetSupportValues(24).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.RegularList = GetSupportValues(25).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.HydrologyList = GetSupportValues(26).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.UseOfLandList = GetSupportValues(27).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.HomogeneousZoneList = GetSupportValues(40).OrderBy(x => x.ID).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.MovementList = GetSupportValues(39).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.SplitTypeList = GetSupportValues(48).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
                model.CountryList = HMTLHelperExtensions.CreateSelectList(new AddressModel().GetCountries(), "ID", "Name");
                model.StateList = new List<SelectListItem>();
                model.CityList = new List<SelectListItem>();
                model.TownList = new List<SelectListItem>();
                if (model.CountryID > 0 && model.CountryStateID > 0)
                    model.StateList = new AddressModel().GetStates(model.CountryID.Value).ToList();
                if (model.CountryID > 0 && model.CountryStateID > 0 && model.CityID > 0)
                    model.CityList = new AddressModel().GetCities(model.CountryID.Value, model.CountryStateID.Value).ToList();
                if (model.CountryID > 0 && model.CountryStateID > 0 && model.CityID > 0 && model.TownID > 0)
                    model.TownList = new AddressModel().GetTowns(model.CountryID.Value, model.CountryStateID.Value, model.CityID.Value).ToList();
            }
            model.PropertyServicesList1 = GetSupportValues(43).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.PropertyServicesList1.Insert(0, new SelectListItem { Text = Resources.Global.All, Value = "0" });
            model.PropertyServicesList2 = GetSupportValues(44).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.PropertyServicesList2.Insert(0, new SelectListItem { Text = Resources.Global.All, Value = "0" });

            //Edit- Fill Owner List
            if (model.OwnerID > 0)
            {
                model.AccountList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetForSearch(null, model.OwnerID, null), "id", "text");
            }
            //

            return model;
        }
        public List<AccountPropertyModel> GetForFilling(int id, int AccountServiceCollectionDetailId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "accountServiceCollectionDetailId", Value = AccountServiceCollectionDetailId });
            return new RestSharpHandler().RestRequest<List<AccountPropertyModel>>(APISelector.Municipality, true, "api/AccountProperty/GetForFilling", "GET", lstParameter, null);
        }

        public List<AccountPropertyModel> GetFillingHistory(int PropertyAccountID, string PropertyNumber, string RightNumber, string TimeZone)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "PropertyAccountID", Value = PropertyAccountID });
            lstParameter.Add(new NameValuePair { Name = "PropertyNumber", Value = PropertyNumber });
            lstParameter.Add(new NameValuePair { Name = "RightNumber", Value = RightNumber });
            List<AccountPropertyModel> model = new RestSharpHandler().RestRequest<List<AccountPropertyModel>>(APISelector.Municipality, true, "api/AccountProperty/PropertyFilingHistoryGet", "GET", lstParameter, null);
            model.ForEach(x =>
            {
                x.ModifiedDate = GetLocalDateFromUTC(TimeZone, x.ModifiedDate);
            });

            return model;
        }
        public List<MUNAccountPropertyRightHistoryModel> GetAccountPropertyRightHistory(int AccountPropertyID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountPropertyID", Value = AccountPropertyID });            
            List<MUNAccountPropertyRightHistoryModel> model = new RestSharpHandler().RestRequest<List<MUNAccountPropertyRightHistoryModel>>(APISelector.Municipality, true, "api/AccountProperty/AccountPropertyRightHistoryGet", "GET", lstParameter, null);
            return model;
        }

        public List<AccountPropertyConstructionDetailModel> BindJsonModal(AccountPropertyModel model)
        {
            List<AccountPropertyConstructionDetailModel> objData = new List<AccountPropertyConstructionDetailModel>();
            objData = model.AccountPropertyConstructionDetail.Select(d => new AccountPropertyConstructionDetailModel
            {
                ID = d.ID,
                ConstructionTypeID = d.ConstructionTypeID,
                ConstructionType = d.ConstructionType,
                MaterialTypeID = d.MaterialTypeID,
                MaterialType = d.MaterialType,
                ConstructionYear = d.ConstructionYear ?? 0,
                TotalYears = d.TotalYears ?? 0,
                StatusID = d.StatusID,
                Status = d.Status,
                Floors = d.Floors,
                Chambers = d.Chambers,
                InternalWallsID = d.InternalWallsID,
                InternalWalls = d.InternalWalls,
                ExternalWallsID = d.ExternalWallsID,
                ExternalWalls = d.ExternalWalls,
                StructureID = d.StructureID,
                Structure = d.Structure,
                RoofID = d.RoofID,
                Roof = d.Roof,
                CeilingID = d.CeilingID,
                Ceiling = d.Ceiling,
                FloorID = d.FloorID,
                Floor = d.Floor,
                Bathroom = d.Bathroom ?? 0,
                UsefulLife = d.UsefulLife,
                BuildingArea = d.BuildingArea.Value,
                UnitValue = d.UnitValue ?? 0,
                TotalValue = d.TotalValue ?? 0,
                IsDistributed = d.IsDistributed,
                IsActive = d.IsActive,
                IsDeleted = d.IsDeleted
            }).ToList();
            return objData;
        }

        public List<SupportValueModel> GetSupportValues(int tableId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "TableID", Value = tableId });
            return new RestSharpHandler().RestRequest<List<SupportValueModel>>(APISelector.Municipality, true, "api/Account/SupportTableValueGet", "GET", lstParameter);
        }

        public List<AccountPropertyRightModel> GetNotPaid(int accountPropertyID, bool? checkActivePaymentPlan)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID });
            lstParameter.Add(new NameValuePair { Name = "checkActivePaymentPlan", Value = checkActivePaymentPlan });
            return new RestSharpHandler().RestRequest<List<AccountPropertyRightModel>>(APISelector.Municipality, true, "api/AccountProperty/AccountPropertyRightGetNotPaid", "GET", lstParameter);
        }
        public List<PropertyModel> GetAccountPropertyRightByOwner(int ownerID, int serviceID, int fiscalYearID, int? id, bool? IsTransferByRight = false)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ownerID", Value = ownerID });
            lstParameter.Add(new NameValuePair { Name = "serviceID", Value = serviceID });
            lstParameter.Add(new NameValuePair { Name = "fiscalYearID", Value = fiscalYearID });
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "isTransferByRight", Value = IsTransferByRight });
            return new RestSharpHandler().RestRequest<List<PropertyModel>>(APISelector.Municipality, true, "api/AccountProperty/GetAccountPropertyRightByOwner", "GET", lstParameter);
        }
        public List<PropertyModel> GetAccountPropertyRight()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            return new RestSharpHandler().RestRequest<List<PropertyModel>>(APISelector.Municipality, true, "api/AccountProperty/GetAccountPropertyRight", "GET", lstParameter);
        }

        public List<PropertyTransferModel> DocumentWorkflowHistoryLogGetForSplit(int TransferID)
        {
            PropertyTransferModel model = new PropertyTransferModel();
            List<PropertyTransferModel> propertyTransferModel = new List<PropertyTransferModel>();
            if (TransferID > 0)
            {

                List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "TransferID", Value = TransferID }
            };
                propertyTransferModel = new RestSharpHandler().RestRequest<List<PropertyTransferModel>>(APISelector.Municipality, true, "api/Workflow/DocumentWorkflowHistoryLogGetForSplit", "GET", lstParameter);

                var userIDs = string.Join(",", propertyTransferModel.Select(x => x.AssignToID));
                var userlist = new UserProfile().GetUserByUserIDs(userIDs);
                if (userlist.Count != 0)
                    propertyTransferModel.ForEach(x => x.AssignToName = userlist.Find(a => a.UserID == x.AssignToID).FullName);
                propertyTransferModel.ForEach(a => a.CreatedDateInString = a.CreatedDate.ToString("g"));
            }
            return propertyTransferModel;
        }

        public int InsertTemporary(int AccountID)
        {
            AccountPropertyModel model = new AccountPropertyModel();
            model.OwnerID = AccountID;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountProperty/InsertTemporary", "POST", null, lstObjParameter);
        }
        #endregion

        #region TIMEZONE methods
        private DateTime GetLocalDateFromUTC(string timezoneID, DateTime date)
        {
            DateTime _dtLocalDateTime;
            TimeZoneInfo _objTimeZone;
            try
            {
                _objTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneID);
            }
            catch (Exception)
            {
                _objTimeZone = CreateTimeZone(timezoneID);
            }
            return _dtLocalDateTime = TimeZoneInfo.ConvertTimeFromUtc(date, _objTimeZone);
        }
        private TimeZoneInfo CreateTimeZone(string timeZoneID)
        {
            const string _fileName = @"TimeZoneInfo.txt";
            string _targetFolder = System.Web.HttpContext.Current.Server.MapPath("~/ImportFile");
            string _filePath = Path.Combine(_targetFolder, _fileName);
            TimeZoneInfo _objTimeZone = null;
            bool _found = false;

            if (File.Exists(_filePath))
            {
                using (StreamReader _objReader = new StreamReader(_filePath))
                {
                    string _timeZoneInfo;
                    while (_objReader.Peek() >= 0)
                    {
                        _timeZoneInfo = _objReader.ReadLine();
                        if (_timeZoneInfo.Contains(timeZoneID))
                        {
                            _objTimeZone = TimeZoneInfo.FromSerializedString(_timeZoneInfo);
                            _found = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                Stream _objFileStream = new FileStream(_filePath, FileMode.Create);
                _objFileStream.Close();
            }

            if (!_found)
            {
                // Define other custom time zone arguments
                string _displayName = TimeZoneInfo.Local.DisplayName;
                string _standardName = TimeZoneInfo.Local.StandardName;
                string _daylightName = TimeZoneInfo.Local.DaylightName;
                TimeSpan offset = new TimeSpan(TimeZoneInfo.Local.BaseUtcOffset.Hours, TimeZoneInfo.Local.BaseUtcOffset.Minutes, TimeZoneInfo.Local.BaseUtcOffset.Seconds);
                _objTimeZone = TimeZoneInfo.CreateCustomTimeZone(timeZoneID, offset, _displayName, _standardName, _daylightName, TimeZoneInfo.Local.GetAdjustmentRules());

                // Write time zone to the file
                using (StreamWriter _objWriter = new StreamWriter(_filePath, true))
                {
                    _objWriter.WriteLine(_objTimeZone.ToSerializedString());
                }
            }
            return _objTimeZone;
        }
        #endregion
    }
    public class AccountPropertyList
    {
        #region public properties       
        public IEnumerable<SelectListItem> Accountsearch { get; set; }
        public List<AccountPropertyModel> AccountPropertyModelList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion

        #region Public Methods
        public AccountPropertyList()
        {
            AccountPropertyModelList = new List<AccountPropertyModel>();
        }
        public AccountPropertyList Get(string filtertext, int pageIndex, int pageSize, string sortColumn, string sortOrder, int? AccountID = null)
        {
            decimal outDecimal;
            if (Decimal.TryParse(filtertext, out outDecimal))
            {
                try
                {
                    filtertext = Convert.ToDecimal(filtertext).ToString(CultureInfo.InvariantCulture);
                }
                catch { }
            }

            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filtertext });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = sortColumn });
            lstParameter.Add(new NameValuePair { Name = "sortOrder", Value = sortOrder });
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = AccountID });
            return new RestSharpHandler().RestRequest<AccountPropertyList>(APISelector.Municipality, true, "api/AccountProperty/GetByPaging", "GET", lstParameter, null);
        }
        #endregion
    }

    public class AccountPropertyListForSearch
    {
        #region public properties                    
        public List<AccountPropertySearchModel> AccountPropertyList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AccountPropertyListForSearch()
        {
            AccountPropertyList = new List<AccountPropertySearchModel>();
        }
        #endregion

        #region Public Methods     
        public AccountPropertyListForSearch Get(string searchText, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "searchText", Value = searchText });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<AccountPropertyListForSearch>(APISelector.Municipality, true, "api/AccountProperty/GetForSearch", "GET", lstParameter);
        }
        public AccountPropertyListForSearch GetRight(string searchText, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "searchText", Value = searchText });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<AccountPropertyListForSearch>(APISelector.Municipality, true, "api/AccountProperty/GetRightForSearch", "GET", lstParameter);
        }
        #endregion
    }
}
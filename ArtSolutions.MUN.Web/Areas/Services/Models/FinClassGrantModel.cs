using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FinClassGrantModel
    {
        #region public properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CombineName { get; set; }
        public string Description { get; set; }
        public bool IsGrant { get; set; }
        public Nullable<System.DateTime> InitialDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string SegmentID { get; set; }
        public Nullable<int> AccountCodeID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> GrantTypeID { get; set; }
        public Nullable<int> GrantTypeTableId14 { get; set; }
        public Nullable<int> GrantGroupID { get; set; }
        public Nullable<int> GrantFundBalanceID { get; set; }
        public Nullable<int> DueToOtherFundAccountID { get; set; }
        public Nullable<int> DueFromOtherFundAccountID { get; set; }
        public Nullable<int> FinanceTemplateID { get; set; }
        public Nullable<int> TotalGrant { get; set; }
        public int BudgetGrantClass { get; set; }
        public Nullable<int> Budgets { get; set; }
        public string ClassName { get; set; }

        //Extra Column For Receivable, Revenu & Cash Account
        public int ReceivableAccountID { get; set; }
        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public int RevenueAccountID { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public int CheckbookID { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public int CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public int CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        #endregion

        #region Public Methods
        public List<FinClassGrantModel> Get(int? id, bool? isActive, string filterText, bool? isGrant)
        {
            List<FinClassGrantModel> list = new List<FinClassGrantModel>();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "IsActive", Value = isActive });
            lstParameter.Add(new NameValuePair { Name = "IsGrant", Value = isGrant });
            list = new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Municipality, true, "api/CollectionTemplate/ClassGrantGet", "GET", lstParameter, null);
            return list;
        }
        public List<FinClassGrantModel> GrantGetBySponsor(DateTime date, int AccountId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "date", Value = date.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "AccountId", Value = AccountId });
            return new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Municipality, true, "api/Finance/GrantGetBySponsor", "GET", lstParameter, null);
        }
        public List<FinClassGrantModel> GetSalesCashier(int? id, string filterText, bool? isActive, int? moduleID, int? bankAccountID, int? grantID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "isActive", Value = isActive });
            lstParameter.Add(new NameValuePair { Name = "moduleID", Value = moduleID });
            lstParameter.Add(new NameValuePair { Name = "bankAccountID", Value = bankAccountID });
            lstParameter.Add(new NameValuePair { Name = "grantID", Value = grantID });
            return new RestSharpHandler().RestRequest<List<FinClassGrantModel>>(APISelector.Municipality, true, "api/Finance/GetSalesCashier", "GET", lstParameter, null);
        }
        #endregion
    }
}
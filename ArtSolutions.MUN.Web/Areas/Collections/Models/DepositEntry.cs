using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class DepositEntry
    {
        #region public methods
        public DepositEntryModel Get()
        {
            DepositEntryModel model = new DepositEntryModel();
            model.DepositTypeList = new SelectList(new AccountModel().GetSupportValues(14), "ID", "Name").OrderBy(x => x.Text);
            model.ClosedEntryList = new ClosingEntry().Get(null, null, true);
            model.BankAccountList = new FINBankAccount().Get(null, null, true);
            model.Date = Common.CurrentDateTime;
            return model;
        }
        public DepositEntryListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            DepositEntryListModel list = new DepositEntryListModel();
            //Filter for Void/Post Status
            bool? isvoid = null;
            if (filterText == Resources.Global.Void.ToLower())
            {
                isvoid = true;
                filterText = null;
            }
            else if (filterText == Resources.Global.Posted.ToLower())
            {
                isvoid = false;
                filterText = null;
            }
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "IsVoid", Value = isvoid });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<DepositEntryListModel>(APISelector.Municipality, true, "api/DepositEntry/GetWithPaging", "GET", lstParameter, null);
            return list;
        }
        public int Insert(DepositEntryModel model)
        {
            model.Date = model.CreatedDate = model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedUserID = model.ModifiedUserID = UserSession.Current.Id;
            model.CommaSeperatedClosingIds = string.Join(",", model.ClosedEntryList.Select(x => x.ID).ToList());

            if (!string.IsNullOrEmpty(model.FinanceAccount))
            {
                string[] financeAccount = model.FinanceAccount.Split('-');
                model.FinanceAccountID = model.AccountID;
                model.FinanceAccountCode = financeAccount[0];
                model.FinanceAccountName = financeAccount[1];
            }
            
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/DepositEntry/Insert", "POST", null, lstObjParameter);
        }
        public DepositEntryPrintModel GetPrint(int id)
        {
            DepositEntryPrintModel list = new DepositEntryPrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            list = new RestSharpHandler().RestRequest<DepositEntryPrintModel>(APISelector.Municipality, true, "api/DepositEntry/GetPrint", "GET", lstParameter, null);

            if (list.DepositEntry != null)
            {
                list.DepositEntry.ClosedEntryList = list.ClosedEntryList;
                list.DepositEntry.DepositTypeList = new SelectList(new AccountModel().GetSupportValues(14), "ID", "Name");
                list.DepositEntry.BankAccountList = new FINBankAccount().Get(null, null, true);
            }
            return list;
        }
        public bool Void(DepositEntryModel model)
        {
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/DepositEntry/Void", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
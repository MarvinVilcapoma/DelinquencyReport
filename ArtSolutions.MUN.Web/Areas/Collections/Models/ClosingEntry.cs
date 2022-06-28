using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class ClosingEntry
    {
        #region public methods
        public ClosingEntryModel Get()
        {
            ClosingEntryModel model = new ClosingEntryModel();
            model.CashierList = new SelectList(new SalesCashier().Get(), "UserID", "UserName").OrderBy(x => x.Text);
            model.ClosingTypeList = new SelectList(new AccountModel().GetSupportValues(13), "ID", "Name").OrderBy(x => x.Text);
           
            ////List<FinClassGrantModel> checkBookList = new FinClassGrantModel().GetSalesCashier(null, null, true, null, null, null).OrderBy(x => x.Name).ToList();
            ////model.FinanceCheckbookList = new List<SelectListItemViewModel>();
            ////model.FinanceCheckbookList = checkBookList.Select(i => new SelectListItemViewModel
            ////{
            ////    ID = i.ID,
            ////    Name = i.Code + " - " + i.Name
            ////}).ToList();

            var paymentOptionList = new AccountModel().GetSupportValues(17).OrderBy(x=>x.Name).ToList();
            paymentOptionList.Add(new SupportValueModel { Description = "", Name = Resources.Global.CreditApplied, ID = -1 });
            model.PaymentOptionList = new SelectList(paymentOptionList, "ID", "Name");

            model.Date = Common.CurrentDateTime;
            return model;
        }

        public ClosingEntryListModel GetWithPaging(JQDTParams jqdtParams, string filterText)
        {
            ClosingEntryListModel list = new ClosingEntryListModel();
            // Filter for deposit
            bool? isdeposited = null;
            if (filterText == Resources.Global.Yes.ToLower())
            {
                isdeposited = true;
                filterText = null;
            }
            else if (filterText == Resources.Global.No.ToLower())
            {
                isdeposited = false;
                filterText = null;
            }
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
            lstParameter.Add(new NameValuePair { Name = "IsDeposited", Value = isdeposited });
            lstParameter.Add(new NameValuePair { Name = "IsVoid", Value = isvoid });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<ClosingEntryListModel>(APISelector.Municipality, true, "api/ClosingEntry/GetWithPaging", "GET", lstParameter, null);
            list.ClosingEntryList.ForEach(x =>
            {
                x.DepositedText = x.IsDeposited == true ? Resources.Global.Yes : Resources.Global.No;
            });
            return list;
        }
        public List<ClosingEntryModel> Get(int? id, int? accountId, bool? onlyPostedClosingEntries)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "OnlyPostedClosingEntries", Value = onlyPostedClosingEntries });
            return new RestSharpHandler().RestRequest<List<ClosingEntryModel>>(APISelector.Municipality, true, "api/ClosingEntry/Get", "GET", lstParameter, null);
        }
        public int Insert(ClosingEntryModel model)
        {
            ////
            //FinClassGrantModel checkBook = new FinClassGrantModel().GetSalesCashier(model.FinanceCheckbookID, null, true, null, null, null).FirstOrDefault();
            //model.FinanceCheckbookCode = checkBook.Code;
            //model.FinanceCheckbookName = checkBook.Name;

            ////Get Cash Account Detail From CheckBook
            //FinSalesCashierDetailsModel checkBookDetail = new FinSalesCashierDetailsModel().GetSalesCashierDetails(model.FinanceCheckbookID);
            //if (checkBookDetail != null && checkBookDetail.SalesCashierDetais != null && checkBookDetail.SalesCashierDetais.Any())
            //{
            //    var cashierDetais = checkBookDetail.SalesCashierDetais.Where(x => x.PaymentOptionID == 1).FirstOrDefault();
            //    model.CashAccountID = cashierDetais.FinancePaymentAccountID;
            //    if (!string.IsNullOrEmpty(cashierDetais.FinancePaymentAccountName))
            //    {
            //        string[] financePaymentAccount = cashierDetais.FinancePaymentAccountName.Split('-');
            //        model.CashAccountCode = financePaymentAccount[0];
            //        model.CashAccountName = financePaymentAccount[1];
            //    }
            //}
            ////

            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.CommaSeperatedPaymentIds = string.Join(",", model.PostedPaymentReceiptList.Select(x => x.ID).ToList());

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/ClosingEntry/Insert", "POST", null, lstObjParameter);
        }

        public ClosingEntryPrintModel GetPrint(int id)
        {
            ClosingEntryPrintModel list = new ClosingEntryPrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            list = new RestSharpHandler().RestRequest<ClosingEntryPrintModel>(APISelector.Municipality, true, "api/ClosingEntry/GetPrint", "GET", lstParameter, null);

            if (list.ClosingEntry != null)
            {
                list.ClosingEntry.PostedPaymentReceiptList = list.PaymentReceipts;
                list.ClosingEntry.CashierList = new SelectList(new SalesCashier().Get(), "UserID", "UserName");
                list.ClosingEntry.ClosingTypeList = new SelectList(new AccountModel().GetSupportValues(13), "ID", "Name");

                var paymentOptionList = new AccountModel().GetSupportValues(17).OrderBy(x => x.Name).ToList();
                paymentOptionList.Add(new SupportValueModel { Description = "", Name = Resources.Global.CreditApplied, ID = -1 });
                list.ClosingEntry.PaymentOptionList = new SelectList(paymentOptionList, "ID", "Name");

                //list.ClosingEntry.PaymentOptionList = new SelectList(new AccountModel().GetSupportValues(17), "ID", "Name");

                //List<FinClassGrantModel> checkBookList = new FinClassGrantModel().GetSalesCashier(null, null, true, null, null, null).OrderBy(x => x.Name).ToList();

                //if (checkBookList != null && checkBookList.Any())
                //{
                //    if (list.ClosingEntry.FinanceCheckbookID.HasValue)
                //    {
                //        var financeCheckbook = checkBookList.Where(x => x.ID == list.ClosingEntry.FinanceCheckbookID.Value).FirstOrDefault();
                //        list.ClosingEntry.FinanceCheckbookName = financeCheckbook.Code + " - " + financeCheckbook.Name;
                //    }
                //}

                //list.ClosingEntry.FinanceCheckbookList = new List<SelectListItemViewModel>();
                //list.ClosingEntry.FinanceCheckbookList = checkBookList.Select(i => new SelectListItemViewModel
                //{
                //    ID = i.ID,
                //    Name = i.Code + " - " + i.Name
                //}).ToList();

                if (list.ClosingEntry.PaymentOptionList != null && list.ClosingEntry.PaymentOptionList.Any() && list.ClosingEntry.PaymentOptionID > 0)
                    list.ClosingEntry.PaymentOptionName = list.ClosingEntry.PaymentOptionList.Where(x => x.Value == Convert.ToString(list.ClosingEntry.PaymentOptionID)).FirstOrDefault().Text;
            }
            return list;
        }

        public bool Void(ClosingEntryModel model)
        {
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/ClosingEntry/Void", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
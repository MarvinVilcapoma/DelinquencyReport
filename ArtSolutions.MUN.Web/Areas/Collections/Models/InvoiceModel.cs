using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class InvoiceModel
    {
        #region Public Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int DocumentTypeID { get; set; }
        public int SequenceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public System.DateTime Date { get; set; }
        public int FiscalYearID { get; set; }
        public string FiscalYearPeriodID { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public int? AccountServiceID { get; set; }
        public Nullable<int> AccountContactID { get; set; }
        public Nullable<int> AccountAddressID { get; set; }
        public Nullable<int> PaymentOptionID { get; set; }
        public int PrintTemplateID { get; set; }
        public string Notes { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsPost { get; set; }
        public bool IsVoid { get; set; }
        public bool IsDeleted { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public decimal Total { get; set; }
        public decimal Payments { get; set; }
        public decimal Balance { get; set; }
        public decimal SubTotalCompanyCurrency { get; set; }
        public decimal DiscountTotalCompanyCurrency { get; set; }
        public decimal TotalCompanyCurrency { get; set; }
        public decimal PaymentsCompanyCurrency { get; set; }
        public decimal BalanceCompanyCurrency { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsManual { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        [StringLength(500, ErrorMessageResourceName = "ValMaxLength", ErrorMessageResourceType = typeof(Resources.Global))]
        public string VoidReason { get; set; }
        public DateTime VoidDate { get; set; }
        public bool IsSponsor { get; set; }
        public bool AccountIsActive { get; set; }
        #endregion

        #region Constructor
        public InvoiceModel()
        {
            AccountList = new List<AccountSearchModel>();
        }
        #endregion

        #region Reference 
        public List<AccountSearchModel> AccountList { get; set; }
        public List<InvoiceDetailModel> InvoiceDetail { get; set; }
        #endregion

        #region Extra Properties
        public bool SelectedInvoice { get; set; }
        public string ItemType { get; set; }
        public string ServiceName { get; set; }
        public string FINGrantName { get; set; }
        public string CollectionItemName
        {
            get
            {
                if (!string.IsNullOrEmpty(ServiceName))
                    return ServiceName;
                else if (!string.IsNullOrEmpty(FINGrantName))
                    return FINGrantName;
                else
                    return "-";
            }
        }
        public string FormattedTotal { get { return Total.ToString("c"); } }
        public string FormattedDate { get { return Date.ToString("d"); } }
        public string AccountDisplayName { get; set; }
        public string Status { get; set; }
        public string RowVersion64
        {
            get
            {
                if (this.RowVersion != null)
                    return Convert.ToBase64String(this.RowVersion);

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.RowVersion = null;
                else
                    this.RowVersion = Convert.FromBase64String(value);
            }
        }
        public string InvoiceDetailJson { get; set; }
        #endregion

        #region Public Methods
        public List<InvoiceModel> Get(int? id, int? accountId, int? accountserviceID, bool? onlyPendingInvoice, bool? onlyPostedInvoice)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "AccountServiceID", Value = AccountServiceID });
            lstParameter.Add(new NameValuePair { Name = "OnlyPendingInvoice", Value = onlyPendingInvoice });
            lstParameter.Add(new NameValuePair { Name = "OnlyPostedInvoice", Value = onlyPostedInvoice });
            List<InvoiceModel> data = new RestSharpHandler().RestRequest<List<InvoiceModel>>(APISelector.Municipality, true, "api/Invoice/Get", "GET", lstParameter);
            return data;
        }

        public InvoiceListModel GetWithPaging(JQDTParams jqdtParams, string filterText, int? accountID)
        {
            decimal outDecimal;
            DateTime outDate;
            InvoiceListModel list = new InvoiceListModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();

            if (Decimal.TryParse(filterText, out outDecimal))
            {
                try
                {
                    filterText = Convert.ToDecimal(filterText).ToString(CultureInfo.InvariantCulture);
                }
                catch { }
            }
            if (DateTime.TryParse(filterText, out outDate))
            {
                try
                {
                    filterText = Convert.ToDateTime(filterText).ToString("d", CultureInfo.InvariantCulture);
                }
                catch { }
            }
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = accountID });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<InvoiceListModel>(APISelector.Municipality, true, "api/Invoice/GetWithPaging", "GET", lstParameter, null);
            return list;
        }
        public void NewInvoice(int? accountID)
        {
            this.Date = Common.CurrentDateTime.Date;
            this.InvoiceDetail = new List<InvoiceDetailModel>();
            this.IsSponsor = false;

            if (accountID > 0)
            {
                this.AccountID = accountID.Value;
                this.AccountList = new AccountModel().GetForSearch(null, accountID, null);
            }
        }
        public InvoiceModel EditInvoice(int ID)
        {
            InvoiceModel model = new InvoiceModel();
            model = new InvoiceModel().Get(ID, null, null, null, null).FirstOrDefault();
            if (model == null || model.ID == 0)
                throw new Exception(Resources.COLInvoice.InvalidNumber);
            else if (model.Payments > 0)
                throw new Exception(Resources.COLInvoice.PaymentReceiptExistErrMsg);
            else if (model.IsVoid)
                throw new Exception(Resources.COLInvoice.VoidedInvoiceUpdateErrorMsg);
            else if (model.AccountServiceID.HasValue)
                throw new Exception(Resources.COLInvoice.ServiceInvoiceUpdateErrMsg);

            model.AccountList = new AccountModel().GetForSearch(null, model.AccountID, null).ToList();

            model.InvoiceDetail = new InvoiceDetailModel().Get(null, model.ID);

            List<FinClassGrantModel> data = new FinClassGrantModel().GrantGetBySponsor(model.Date, model.AccountID);
            data.ForEach(d =>
            {
                var FinGrantModel = new FinGrantAccountModel().Get(d.ID, EnumUtility.FINAccountType.AR);
                if (FinGrantModel.ReceivableAccountList != null && FinGrantModel.ReceivableAccountList.Count > 0)
                {
                    d.ReceivableAccountID = FinGrantModel.ReceivableAccountList.FirstOrDefault().ID;
                    d.ReceivableName = FinGrantModel.ReceivableAccountList.FirstOrDefault().Name;
                    d.ReceivableCode = FinGrantModel.ReceivableAccountList.FirstOrDefault().CodeFriendly;
                }
                if (FinGrantModel.RevenueAccountList != null && FinGrantModel.RevenueAccountList.Count > 0)
                {
                    d.RevenueAccountID = FinGrantModel.RevenueAccountList.FirstOrDefault().ID;
                    d.RevenueName = FinGrantModel.RevenueAccountList.FirstOrDefault().Name;
                    d.RevenueCode = FinGrantModel.RevenueAccountList.FirstOrDefault().CodeFriendly;
                    d.CreditAccountID = FinGrantModel.RevenueAccountList.FirstOrDefault().ID;
                    d.CreditAccountName = FinGrantModel.RevenueAccountList.FirstOrDefault().Name;
                    d.CreditAccountCode = FinGrantModel.RevenueAccountList.FirstOrDefault().CodeFriendly;
                }
                if (FinGrantModel.CheckbookAccountList != null && FinGrantModel.CheckbookAccountList.Count > 0)
                {
                    d.CashAccountID = FinGrantModel.CheckbookAccountList.FirstOrDefault().FinancePaymentAccountID;
                    d.CashAccountName = FinGrantModel.CheckbookAccountList.FirstOrDefault().FinanceAccountName;
                    d.CashAccountCode = FinGrantModel.CheckbookAccountList.FirstOrDefault().CodeFriendly;
                    d.CheckbookID = FinGrantModel.CheckbookAccountList.FirstOrDefault().CheckbookId;
                    d.CheckbookName = FinGrantModel.CheckbookAccountList.FirstOrDefault().Name;
                    d.CheckbookCode = FinGrantModel.CheckbookAccountList.FirstOrDefault().Code;
                }
            });
            model.InvoiceDetail.ForEach(d =>
            {
                d.InvoiceTypeList = new SelectList(new AccountModel().GetSupportValues(16), "ID", "Name", d.ItemTypeID);
                if (d.FINGrantID > 0)
                {
                    d.FinClassGrantList = data;
                    var FinGrantModel = new FinGrantAccountModel().Get(d.FINGrantID.Value, EnumUtility.FINAccountType.AR);
                    d.FinCheckBookList = FinGrantModel.CheckbookAccountList;
                }
                if (d.ServiceID > 0)
                    d.ServiceList = new Service().GetForNoFiling();
            });
            model.IsSponsor = new AccountModel().IsSponsorByAccount(model.AccountID, 0) == true;
            return model;
        }

        public InvoiceModel Save(InvoiceModel model)
        {
            ValidateInvoice(model);
            var jsonSerialiser = new JavaScriptSerializer();
            model.InvoiceDetailJson = jsonSerialiser.Serialize(model.InvoiceDetail);
            model.CreatedDate = model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedUserID = model.ModifiedUserID = UserSession.Current.Id;
            if (model.ID == 0)
            {
                List<object> lstObjParameter = new List<object>();
                lstObjParameter.Add(model);
                return new RestSharpHandler().RestRequest<InvoiceModel>(APISelector.Municipality, true, "api/Invoice/ManualInsert", "POST", null, lstObjParameter);
            }
            else
            {
                List<object> lstObjParameter = new List<object>();
                lstObjParameter.Add(model);
                return new RestSharpHandler().RestRequest<InvoiceModel>(APISelector.Municipality, true, "api/Invoice/Update", "POST", null, lstObjParameter);
            }
        }

        public List<FinClassGrantModel> FinGrantDetailList(DateTime date, int AccountId)
        {
            List<FinClassGrantModel> data = new List<FinClassGrantModel>();
            data = new FinClassGrantModel().GrantGetBySponsor(date, AccountId);
            data.ForEach(d =>
            {
                var model = new FinGrantAccountModel().Get(d.ID, EnumUtility.FINAccountType.AR);
                if (model.ReceivableAccountList != null && model.ReceivableAccountList.Count > 0)
                {
                    d.ReceivableAccountID = model.ReceivableAccountList.FirstOrDefault().ID;
                    d.ReceivableName = model.ReceivableAccountList.FirstOrDefault().Name;
                    d.ReceivableCode = model.ReceivableAccountList.FirstOrDefault().CodeFriendly;
                }
                if (model.RevenueAccountList != null && model.RevenueAccountList.Count > 0)
                {
                    d.RevenueAccountID = model.RevenueAccountList.FirstOrDefault().ID;
                    d.RevenueName = model.RevenueAccountList.FirstOrDefault().Name;
                    d.RevenueCode = model.RevenueAccountList.FirstOrDefault().CodeFriendly;
                    d.CreditAccountID = model.RevenueAccountList.Count() > 1 ? model.RevenueAccountList.Skip(1).FirstOrDefault().ID : model.RevenueAccountList.FirstOrDefault().ID;
                    d.CreditAccountName = model.RevenueAccountList.Count() > 1 ? model.RevenueAccountList.Skip(1).FirstOrDefault().Name : model.RevenueAccountList.FirstOrDefault().Name;
                    d.CreditAccountCode = model.RevenueAccountList.Count() > 1 ? model.RevenueAccountList.Skip(1).FirstOrDefault().CodeFriendly : model.RevenueAccountList.FirstOrDefault().CodeFriendly;
                }
            });
            return data;
        }

        public void ValidateInvoice(InvoiceModel model)
        {
            if (model.Total <= 0)
                throw new Exception(Resources.COLInvoice.TotalValidationMsg);
            if (model.InvoiceDetail.Where(d => d.ServiceID != null && d.ServiceID.Value > 0).Count() > 0 && model.InvoiceDetail.Where(d => d.FINGrantID != null && d.FINGrantID.Value > 0).Count() > 0)
                throw new Exception(Resources.COLInvoice.ServiceTypeMsg);
            if (model.InvoiceDetail.Where(d => d.FINGrantID != null && d.FINGrantID.Value > 0).GroupBy(d => new { d.ItemTypeID, d.FINGrantID }).Any(d => d.Count() > 1))
                throw new Exception(Resources.COLInvoice.GrantServiceCompareMsg);
            if (model.InvoiceDetail.Where(d => d.ServiceID != null && d.ServiceID.Value > 0).GroupBy(d => new { d.ServiceID }).Any(d => d.Count() > 1))
                throw new Exception(Resources.COLInvoice.ServiceCompareMsg);
            //if (model.InvoiceDetail.Where(d => d.ServiceID != null && d.ServiceID.Value > 0).Count() > 1)
            //    throw new Exception(Resources.COLInvoice.OtherServiceCompareMsg);
        }

        public bool Void(InvoiceModel model)
        {
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Invoice/Void", "POST", null, lstObjParameter);
        }

        public InvoicePrintModel GetPrint(int id)
        {
            InvoicePrintModel model = new InvoicePrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            model = new RestSharpHandler().RestRequest<InvoicePrintModel>(APISelector.Municipality, true, "api/Invoice/GetPrint", "GET", lstParameter, null);

            if (model.PrintTemplate != null)
            {
                model.PrintTemplate.TemplateContent = PreparePrintTemplate(model);
            }
            return model;
        }
        public string PreparePrintTemplate(InvoicePrintModel model)
        {
            //Replace Print Template Content
            string templateBody = model.PrintTemplate.TemplateContent;
            //Status
            if (model.Invoice.IsVoid)
                templateBody = templateBody.Replace("##Status##", Resources.Global.Void);
            else
                templateBody = templateBody.Replace("##Status##", Resources.Global.Posted);
            if (model.Company.LogoID > 0)
                templateBody = templateBody.Replace("##ImgURL##", ArtSolutions.MUN.Web.Common.FileURL + model.Company.LogoID);
            else
                templateBody = templateBody.Replace("##ImgURL##", Common.GetAbsoluteUrl("/Content/Images/no_image_available.png"));

            //Company Info
            templateBody = templateBody.Replace("##CompanyName##", model.Company.Name);
            string compAddress = model.Company.Address1;
            if (!string.IsNullOrEmpty(model.Company.Address2))
                compAddress = compAddress + ", " + model.Company.Address2;
            compAddress = compAddress + ",<br/>";
            if (!string.IsNullOrEmpty(model.Company.StateName))
                compAddress = compAddress + model.Company.StateName + ", ";
            if (!string.IsNullOrEmpty(model.Company.City))
                compAddress = compAddress + model.Company.City + ", ";
            if (!string.IsNullOrEmpty(model.Company.CountyCode))
                compAddress = compAddress + " " + model.Company.CountyCode;
            if (!string.IsNullOrEmpty(model.Company.ZipPostalCode))
                compAddress = compAddress + " " + model.Company.ZipPostalCode;
            compAddress = compAddress + "<br />";
            compAddress = compAddress + "P: " + model.Company.WorkPhone;
            templateBody = templateBody.Replace("##CompanyAddress##", compAddress);

            //Customer Info
            string accntAddress = model.Account.DisplayName;
            accntAddress = accntAddress + "<br /> " + model.Account.Address1;
            if (!string.IsNullOrEmpty(model.Account.Address2))
                accntAddress = accntAddress + ", " + model.Account.Address2;
            accntAddress = accntAddress + ",<br/>";
            if (!string.IsNullOrEmpty(model.Account.StateName))
                accntAddress = accntAddress + model.Account.StateName + ", ";
            if (!string.IsNullOrEmpty(model.Account.City))
                accntAddress = accntAddress + model.Account.City + ", ";
            if (!string.IsNullOrEmpty(model.Account.CountyCode))
                accntAddress = accntAddress + " " + model.Account.CountyCode;
            if (!string.IsNullOrEmpty(model.Account.ZipPostalCode))
                accntAddress = accntAddress + " " + model.Account.ZipPostalCode;
            accntAddress = accntAddress + "<br />";
            accntAddress = accntAddress + "P: " + model.Account.PhoneNumber;
            templateBody = templateBody.Replace("##AccountAddress##", accntAddress);
            templateBody = templateBody.Replace("##AccountId##", model.Account.ID.ToString());
            templateBody = templateBody.Replace("##AccountTaxNumber##", model.Account.TaxNumber);

            // Invoice Info
            templateBody = templateBody.Replace("##InvoiceNumber##", model.Invoice.Number);
            templateBody = templateBody.Replace("##InvoiceDate##", model.Invoice.Date.ToString("d"));
            templateBody = templateBody.Replace("##ServiceName##", model.Invoice.ServiceName);
            templateBody = templateBody.Replace("##InvoiceTotal##", model.Invoice.Total.ToString("c"));
            templateBody = templateBody.Replace("##Reference##", !string.IsNullOrEmpty(model.Invoice.Reference) ? model.Invoice.Reference : "-");
            // Static Values
            templateBody = templateBody.Replace("##MunicipalIdentification##", "123456");
            templateBody = templateBody.Replace("##IdentificationNumber##", "ABCD123456/BO/156");

            //Payment Received Detail Info
            StringBuilder strPaymentDetail = new StringBuilder();
            if (model.ServiceTypeID > 0)
            {
                templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.AccountServiceCollectionDetailList.Sum(x => x.Balance).ToString("c"));

                foreach (var item in model.AccountServiceCollectionDetailList)
                {
                    strPaymentDetail.Append("<tr>");
                    if (model.ServiceTypeID == 1)
                    {
                        strPaymentDetail.Append("<td class='center table-border-td'>" + @item.PreviousFiscalYearID + "-" + item.FiscalYearID + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal1.ToString("c") + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal2.ToString("c") + "</td>");
                    }
                    else
                    {
                        strPaymentDetail.Append("<td class='center table-border-td'>" + @item.FiscalYearWithPeriod + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal.ToString("c") + "</td>");
                    }
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Penalties.ToString("c") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Charges.ToString("c") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Interest.ToString("c") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Discount.Value.ToString("c") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Balance.ToString("c") + "</td>");
                    strPaymentDetail.Append("</tr>");
                }
                templateBody = templateBody.Replace("##CollectionDetailItem##", strPaymentDetail.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.InvoiceDetailList.Sum(x => x.Total).Value.ToString("c"));
                foreach (var item in model.InvoiceDetailList)
                {
                    strPaymentDetail.Append("<tr>");
                    strPaymentDetail.Append("<td class='left table-border-td'>" + @item.ItemType + "</td>");
                    strPaymentDetail.Append("<td class='left table-border-td'>" + @item.ItemName + "</td>");
                    strPaymentDetail.Append("<td class='left table-border-td'>" + @item.Description + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Total.Value.ToString("c") + "</td>");
                    strPaymentDetail.Append("</tr>");
                }
                templateBody = templateBody.Replace("##CollectionDetailItem##", strPaymentDetail.ToString());
            }
            // Invoice Notes
            if (!string.IsNullOrEmpty(model.Invoice.Notes))
            {
                StringBuilder strNotes = new StringBuilder();
                strNotes = strNotes.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.Global.Note + "</td></tr><tr><td>");
                strNotes = strNotes.Append(model.Invoice.Notes);
                strNotes = strNotes.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##InvoiceNotes##", strNotes.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##InvoiceNotes##", "</br>");
            }
            // Invoice Attachment
            if (!string.IsNullOrEmpty(model.Invoice.TermsAndConditions))
            {
                StringBuilder strNotes = new StringBuilder();
                strNotes = strNotes.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.Global.TermsAndConditions + "</td></tr><tr><td>");
                strNotes = strNotes.Append(model.Invoice.TermsAndConditions);
                strNotes = strNotes.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##InvoiceTerms##", strNotes.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##InvoiceTerms##", " ");
            }
            // Invoice Void Reason
            if (!string.IsNullOrEmpty(model.Invoice.VoidReason))
            {
                StringBuilder strReason = new StringBuilder();
                strReason = strReason.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.Global.VoidReason + "</td></tr><tr><td>");
                strReason = strReason.Append(model.Invoice.VoidReason);
                strReason = strReason.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##InvoiceVoidReason##", strReason.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##InvoiceVoidReason##", " ");
            }

            //Collector Info
            UserProfileViewModel userProfileModel = new UserProfile().GetUserByUserIDs(model.Invoice.CreatedUserID.ToString()).FirstOrDefault();
            templateBody = templateBody.Replace("##CollectorName##", string.IsNullOrEmpty(userProfileModel.FullName) ? userProfileModel.Email : userProfileModel.FullName);

            //Country wise Label in Footer
            templateBody = templateBody.Replace("##CountryFooterName##", UserSession.Current.CountryID == 52 ? Resources.Global.FooterNameForCostaRica : Resources.Global.FooterNameForPuertoRico);

            return templateBody;
        }
        #endregion
    }

    public class InvoiceListModel
    {
        #region Public Properties
        public List<InvoiceModel> InvoiceList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }

    public class InvoicePrintModel
    {
        #region Public Properties
        public InvoiceCompanyModel Company { get; set; }
        public InvoiceAccountModel Account { get; set; }
        public InvoiceModel Invoice { get; set; }
        public List<InvoiceDetailModel> InvoiceDetailList { get; set; }
        public List<AccountServiceCollectionDetailModel> AccountServiceCollectionDetailList { get; set; }
        public PrintTemplateModel PrintTemplate { get; set; }
        public int ServiceTypeID { get; set; }
        #endregion
    }

    public class InvoiceCompanyModel
    {
        #region Public Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string WorkPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string StateName { get; set; }
        public int LogoID { get; set; }
        #endregion
    }
    public class InvoiceAccountModel
    {
        #region Public Properties
        public int ID { get; set; }
        public string TaxNumber { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string StateName { get; set; }
        public string CountyCode { get; set; }
        #endregion
    }
}
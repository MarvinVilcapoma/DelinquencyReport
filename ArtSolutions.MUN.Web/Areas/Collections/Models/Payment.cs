using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class Payment
    {
        #region Get methods
        private PaymentModel Get()
        {
            PaymentModel model = new PaymentModel();
            model.PaymentOptionList = new List<PaymentOptionModel>();
            model.PaymentOptionList.Add(new PaymentOptionModel());
            model.PaymentOptionList[0].PaymentOptions = new SelectList(new AccountModel().GetSupportValues(17), "ID", "Name").OrderBy(x => x.Text);
            model.Date = Common.CurrentDateTime;
            model.AllowManualReceipt = ValidateUserPermissionForManualReceipt();
            return model;
        }
        public PaymentModel GetByService(int? accountID, int? serviceTypeID)
        {
            PaymentModel model = Get();
            model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);

            if (accountID > 0 && serviceTypeID > 0)
            {
                model.AccountID = accountID.Value;
                model.ServiceTypeID = serviceTypeID.Value;
                model.AccountList = new SelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text", model.AccountID).OrderBy(x => x.Text);
                model.ServiceTypeList = new ServiceTypeModel().GetNotPaid(model.AccountID, true);
                model.AccountServiceCollectionDetailList = new Payment().GetAccountServiceCollectionDetailOnly(model.AccountID, model.Date);
                //model.AccountServiceCollectionDetailListJson = JsonConvert.SerializeObject(model.AccountServiceCollectionDetailList);
            }
            return model;
        }
        public PaymentModel GetByOtherService(int? accountID, int? serviceTypeID)
        {
            PaymentModel model = Get();
            model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);
            model.InvoiceDetail = new List<InvoiceDetailModel>();
            if (accountID > 0 && serviceTypeID > 0)
            {
                model.AccountID = accountID.Value;
                model.ServiceTypeID = serviceTypeID.Value;
                model.AccountList = new SelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text", model.AccountID).OrderBy(x => x.Text);

            }
            return model;
        }
        public PaymentModel GetByDebitNote(int? accountID)
        {
            PaymentModel model = Get();
            model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);
            if (accountID > 0)
            {
                model.AccountID = accountID.Value;
                model.AccountList = new SelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text", model.AccountID).OrderBy(x => x.Text);

            }
            return model;
        }
        public PaymentModel GetByPropertyTax()
        {
            PaymentModel model = Get();
            model.AccountPropertyRightList = new List<AccountPropertyRightModel>();
            model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);
            return model;
        }
        public PaymentModel GetByPaymentPlan(int? accountID, int? accountPaymentPlanID)
        {
            PaymentModel model = Get();
            model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);

            if (accountID > 0 && accountPaymentPlanID > 0)
            {
                model.AccountID = accountID.Value;
                model.AccountPaymentPlanID = accountPaymentPlanID.Value;
                model.AccountList = new SelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text", model.AccountID).OrderBy(x => x.Text);
                model.AccountPaymentPlanList = new Accounts.Models.AccountPaymentPlan().GetNotPaid(null, accountID, true, model.Date);
                model.AccountPaymentPlanList.ForEach(x =>
                {
                    x.Number = x.Number + " | " + x.PaymentPlanName;
                });
                model.AccountPaymentPlanDetailList = new AccountPaymentPlanDetail().Get(null, accountPaymentPlanID.HasValue ? accountPaymentPlanID.Value.ToString() : null, true, false, null);
            }
            return model;
        }
        public PaymentModel GetByInvoice(int? invoiceID)
        {
            PaymentModel model = Get();
            if (invoiceID > 0)
            {
                model.PostedInvoiceList = new InvoiceModel().Get(invoiceID, null, null, null, null);
                if (model.PostedInvoiceList.Count == 0)
                {
                    throw new Exception(Resources.COLInvoice.InvalidNumber);
                }

                if (model.PostedInvoiceList.FirstOrDefault().Balance == 0)
                {
                    throw new Exception(Resources.COLPayment.ValPaymentProcessedMsg);
                }

                model.PostedInvoiceList[0].SelectedInvoice = true;
                model.AccountID = model.PostedInvoiceList.FirstOrDefault().AccountID;
                InvoiceModel obj = model.PostedInvoiceList.FirstOrDefault();
                model.ApplyCreditAmount = new Payment().GetAvailableCreditBalance(model.AccountID);
                model.AccountList = new SelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text", model.AccountID).OrderBy(x => x.Text);
            }
            return model;
        }
        public List<PaymentModel> GetForClosingEntry(int? id, int? accountId, Guid? cashierId, DateTime? closingDate, bool? onlyPostedReceipts, int? paymentOptionID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = accountId });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "CashierID", Value = cashierId });
            lstParameter.Add(new NameValuePair { Name = "ClosingDate", Value = closingDate != null ? closingDate.Value.ToString("d", CultureInfo.InvariantCulture) : null });
            lstParameter.Add(new NameValuePair { Name = "OnlyPostedReceipts", Value = onlyPostedReceipts });
            lstParameter.Add(new NameValuePair { Name = "PaymentOptionID", Value = paymentOptionID });
            return new RestSharpHandler().RestRequest<List<PaymentModel>>(APISelector.Municipality, true, "api/Payment/GetForClosingEntry", "GET", lstParameter, null);
        }
        public PaymentListModel GetWithPaging(JQDTParams jqdtParams, int? year, string filterText)
        {
            PaymentListModel list = new PaymentListModel();
            //Filter for Void/Post Status
            bool? isvoid = null;
            if (filterText.Trim().ToLower() == Resources.Global.Void.ToLower())
            {
                isvoid = true;
                filterText = null;
            }
            else if (filterText.Trim().ToLower() == Resources.Global.Posted.ToLower())
            {
                isvoid = false;
                filterText = null;
            }
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "ID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "year", Value = year });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "IsVoid", Value = isvoid });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<PaymentListModel>(APISelector.Municipality, true, "api/Payment/GetWithPaging", "GET", lstParameter, null);
            list.PaymentList.ForEach(x => { x.Status = x.IsVoid == true ? Resources.Global.Void : x.IsPost ? Resources.Global.Posted : Resources.Global.Draft; });
            return list;
        }
        public decimal GetAvailableCreditBalance(int AccountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "AccountID", Value = AccountID });
            return new RestSharpHandler().RestRequest<decimal>(APISelector.Municipality, true, "api/Payment/GetAvailableCreditBalance", "GET", lstParameter, null);
        }
        #endregion

        #region Insert methods
        public int InsertByInvoice(PaymentModel model)
        {
            model.Date = model.Date.Add(new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second));
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.IsClosed = false;
            model.PaymentOptionJSON = JsonConvert.SerializeObject(model.PaymentOptionList.Where(x => x.Amount > 0));
            model.InvoiceID = model.PostedInvoiceList.FirstOrDefault().ID;
            model.NetPayment = (model.PaymentOptionList?.Sum(x => x.Amount.Value) ?? 0) + model.ApplyCreditAmount;
            model.Date = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/InsertByInvoice", "POST", null, lstObjParameter);
        }
        public int InsertByOtherService(PaymentModel model)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            model.InvoiceDetailJson = jsonSerialiser.Serialize(model.InvoiceDetail);

            model.Date = model.Date.Add(new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second));
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.IsClosed = false;
            model.PaymentOptionJSON = JsonConvert.SerializeObject(model.PaymentOptionList.Where(x => x.Amount > 0));
            model.NetPayment = (model.PaymentOptionList?.Sum(x => x.Amount.Value) ?? 0) + model.ApplyCreditAmount;
            model.Date = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/InsertByOtherService", "POST", null, lstObjParameter);
        }
        public int InsertByService(PaymentModel model)
        {
            model.Date = model.Date.Add(new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second));
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.IsClosed = false;
            model.PaymentOptionJSON = JsonConvert.SerializeObject(model.PaymentOptionList.Where(x => x.Amount>0));
            model.PaymentDetailList = new List<PaymentDetailModel>();
            model.Date = Common.CurrentDateTime;

            decimal totalDiscount = 0, totalPayment = 0, totalAmnestyAmount = 0;
            model.AccountServiceCollectionDetailList.ForEach(d =>
            {
                if (d.ServiceTypeID == 20 || d.ServiceTypeID == 22)
                {
                    d.Discount = 0;
                }
            });
            foreach (var detail in model.AccountServiceCollectionDetailList.Where(x => x.Payment > 0))
            {
                totalDiscount = totalDiscount + detail.Discount.Value;
                //totalPayment = totalPayment + detail.Payment;  //Fixed For decimal issue
                totalPayment = totalPayment + detail.Total ?? 0;
                totalAmnestyAmount = totalAmnestyAmount + detail.AmnestyAmount;

                model.PaymentDetailList.Add(
                    new PaymentDetailModel()
                    {
                        AccountServiceCollectionDetailID = detail.ID,
                        AccountServiceID = detail.AccountServiceID,
                        CollectionDetailDebt = detail.Debt,
                        Discount = detail.Discount.Value,
                        AmnestyAmount = detail.AmnestyAmount,
                        //SubTotal = detail.Payment + detail.Discount.Value,
                        //Total = detail.Payment
                        SubTotal = detail.Total ?? 0 + detail.Discount.Value,
                        Total = detail.Total ?? 0
                    }
                    );
            }
            model.NetPayment = (model.PaymentOptionList?.Sum(x => x.Amount.Value) ?? 0) + model.ApplyCreditAmount; ;

            if (totalPayment != model.NetPayment && totalPayment > model.NetPayment)
            {
                decimal Difference = totalPayment - model.NetPayment;
                model.PaymentOptionList.FirstOrDefault().Amount = model.PaymentOptionList.FirstOrDefault().Amount + Difference;
                model.NetPayment = totalPayment;
            }
            ////0.006 condition added for [CO-959]. also asked Gerardo the issue
            //if ((model.NetPayment != totalPayment) &&(model.NetPayment - totalPayment > (decimal)0.006))
            //    throw new Exception(Resources.COLPayment.ValPaymentOptionTotalAmount);
            model.TotalDiscount = totalDiscount;
            model.TotalPayment = totalPayment + model.TotalDiscount;

            model.AmnestyAmount = totalAmnestyAmount;

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/InsertByService", "POST", null, lstObjParameter);
        }
        public int InsertByPaymentPlan(PaymentModel model)
        {
            List<int> AccountPaymentPlanDetailIDs = new List<int>();

            if (!string.IsNullOrWhiteSpace(model.AccountPaymentPlanDetailIDs))
            {
                AccountPaymentPlanDetailIDs = model.AccountPaymentPlanDetailIDs.Split(',').Select(int.Parse).ToList();
            }

            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.IsClosed = false;
            model.PaymentOptionJSON = JsonConvert.SerializeObject(model.PaymentOptionList.Where(x => x.Amount > 0));
            model.TotalPayment = model.NetPayment = (model.PaymentOptionList?.Sum(x => x.Amount.Value) ?? 0) + model.ApplyCreditAmount;
            model.PaymentDetailList = new List<PaymentDetailModel>();
            model.Date = Common.CurrentDateTime;
            decimal currentItemAmount = 0;
            decimal amounttopay = model.NetPayment + model.ApplyCreditAmount;
            foreach (var detail in model.AccountPaymentPlanDetailList.Where(d => AccountPaymentPlanDetailIDs != null && (AccountPaymentPlanDetailIDs.FirstOrDefault(k => k == d.ID) > 0)))
            {
                if (detail.Balance <= amounttopay)
                {
                    currentItemAmount = detail.Balance;
                    amounttopay = amounttopay - currentItemAmount;
                }
                else
                {
                    currentItemAmount = amounttopay;
                    amounttopay = 0;
                }
                model.PaymentDetailList.Add(
                    new PaymentDetailModel()
                    {
                        AccountServiceCollectionDetailID = null,
                        AccountServiceID = null,
                        CollectionDetailDebt = 0,
                        AccountPaymentPlanID = detail.AccountPaymentPlanID,
                        AccountPaymentPlanDetailID = detail.ID,
                        AccountPaymentPlanServiceDetailID = detail.AccountPaymentPlanServiceDetailID,
                        Discount = 0,
                        SubTotal = currentItemAmount,
                        Total = currentItemAmount
                    }
                    );
                if (amounttopay == 0)
                {
                    break;
                }
            }

            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/InsertByPaymentPlan", "POST", null, lstObjParameter);
        }
        public int InsertByDebitNote(PaymentModel model)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            model.DebitNotesJson = jsonSerialiser.Serialize(model.AccountDebitNoteList.Where(x => x.SelectedItem == true));

            model.Date = model.Date.Add(new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second));
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.IsClosed = false;
            model.PaymentOptionJSON = JsonConvert.SerializeObject(model.PaymentOptionList.Where(x=>x.Amount>0));
            model.NetPayment = (model.PaymentOptionList?.Sum(x => x.Amount.Value) ?? 0) + model.ApplyCreditAmount;
            model.Date = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Payment/InsertByDebitNote", "POST", null, lstObjParameter);
        }
        #endregion

        #region Print methods
        public PaymentPrintModel GetPrint(int id, int? serviceTypeId)
        {
            PaymentPrintModel list = new PaymentPrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "ServiceTypeID", Value = serviceTypeId });
            list = new RestSharpHandler().RestRequest<PaymentPrintModel>(APISelector.Municipality, true, "api/Payment/GetPrint", "GET", lstParameter, null);
            return list;
        }

        public PaymentPrintModel GetPaymentPlanPrint(int id)
        {
            PaymentPrintModel list = new PaymentPrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            list = new RestSharpHandler().RestRequest<PaymentPrintModel>(APISelector.Municipality, true, "api/Payment/GetPaymentPlanPrint", "GET", lstParameter, null);
            return list;
        }

        public string PreparePrintTemplate(PaymentPrintModel model, string Type, int? serviceTypeId, bool includeAttachments)
        {
            //Replace Print Template Content
            string templateBody = model.PrintTemplate.TemplateContent;

            //Logo
            if (model.Company.LogoID > 0)
            {
                templateBody = templateBody.Replace("##ImgURL##", ArtSolutions.MUN.Web.Common.FileURL + model.Company.LogoID);
            }
            else
            {
                templateBody = templateBody.Replace("##ImgURL##", Common.GetAbsoluteUrl("/Content/Images/no_image_available.png"));
            }

            //Company Info
            templateBody = templateBody.Replace("##CompanyName##", model.Company.Name);
            string compAddress = model.Company.Address1;
            if (!string.IsNullOrEmpty(model.Company.Address2))
            {
                compAddress = compAddress + ", " + model.Company.Address2;
            }

            compAddress = compAddress + ",<br/>";
            if (!string.IsNullOrEmpty(model.Company.StateName))
            {
                compAddress = compAddress + model.Company.StateName + ", ";
            }

            if (!string.IsNullOrEmpty(model.Company.City))
            {
                compAddress = compAddress + model.Company.City + ", ";
            }

            if (!string.IsNullOrEmpty(model.Company.CountyCode))
            {
                compAddress = compAddress + " " + model.Company.CountyCode;
            }

            if (!string.IsNullOrEmpty(model.Company.ZipPostalCode))
            {
                compAddress = compAddress + " " + model.Company.ZipPostalCode;
            }

            compAddress = compAddress + "<br />";
            compAddress = compAddress + "P: " + model.Company.WorkPhone;
            templateBody = templateBody.Replace("##CompanyAddress##", compAddress);

            //Customer Info
            string accntAddress = model.Account.DisplayName;
            accntAddress = accntAddress + "<br /> " + model.Account.Address1;
            if (!string.IsNullOrEmpty(model.Account.Address2))
            {
                accntAddress = accntAddress + ", " + model.Account.Address2;
            }

            accntAddress = accntAddress + ",<br/>";
            if (!string.IsNullOrEmpty(model.Account.StateName))
            {
                accntAddress = accntAddress + model.Account.StateName + ", ";
            }

            if (!string.IsNullOrEmpty(model.Account.City))
            {
                accntAddress = accntAddress + model.Account.City + ", ";
            }

            if (!string.IsNullOrEmpty(model.Account.CountyCode))
            {
                accntAddress = accntAddress + " " + model.Account.CountyCode;
            }

            if (!string.IsNullOrEmpty(model.Account.ZipPostalCode))
            {
                accntAddress = accntAddress + " " + model.Account.ZipPostalCode;
            }

            accntAddress = accntAddress + "<br />";
            accntAddress = accntAddress + "P: " + model.Account.PhoneNumber;
            templateBody = templateBody.Replace("##AccountAddress##", accntAddress);
            templateBody = templateBody.Replace("##AccountId##", model.Account.ID.ToString());
            templateBody = templateBody.Replace("##AccountTaxNumber##", model.Account.TaxNumber);
            templateBody = templateBody.Replace("##MunicipalIdentification##", model.Account.RegisterNumber);

            // Payment Info
            templateBody = templateBody.Replace("##PaymentNumber##", model.Payment.Number);
            templateBody = templateBody.Replace("##PaymentDate##", model.Payment.Date.ToString("d"));
            templateBody = templateBody.Replace("##PaymentNetPayment##", model.Payment.NetPayment.ToString("c"));

            //Payment Option Model
            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 1).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##CashAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 1).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##CashAmount##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 2).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##CheckAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 2).FirstOrDefault().Amount.Value.ToString("c"));
                templateBody = templateBody.Replace("##IdentificationNumber##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 2).FirstOrDefault().Notes);
            }
            else
            {
                templateBody = templateBody.Replace("##CheckAmount##", 0.ToString("c"));
                templateBody = templateBody.Replace("##IdentificationNumber##", "-");
            }
            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 3).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##CreditCardAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 3).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##CreditCardAmount##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 4).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##BankTransferByBancodeCostaRicaAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 4).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##BankTransferByBancodeCostaRicaAmount##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 5).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##CostaRicaChequeAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 5).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##CostaRicaChequeAmount##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 6).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##AdjustmentAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 6).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##AdjustmentAmount##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 7).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##Bank##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 7).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##Bank##", 0.ToString("c"));
            }

            if (model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 8).FirstOrDefault() != null)
            {
                templateBody = templateBody.Replace("##BankTransferByBancoNacionaldeCostaRicaAmount##", model.PaymentOptionList.Where(x => x.PaymentOptionTableValue == 8).FirstOrDefault().Amount.Value.ToString("c"));
            }
            else
            {
                templateBody = templateBody.Replace("##BankTransferByBancoNacionaldeCostaRicaAmount##", 0.ToString("c"));
            }

            //Payment Received Detail Info
            StringBuilder strPaymentDetail = new StringBuilder();
            if (Type == "PaymentPlan")
            {
                templateBody = templateBody.Replace("##PaymentPlanName##", model.Payment.PaymentPlanName);
                //templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.PaymentPlanDetailList.Sum(x => x.Balance).ToString("c"));
                templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.PaymentPlanDetailList.Sum(x => x.Payment).ToString("c"));


                foreach (var item in model.PaymentPlanDetailList)
                {
                    if (item.IsDownPayment == true)
                    {
                        strPaymentDetail.Append("<tr>");
                        //1-Oct-2019
                        // strPaymentDetail.Append("<td class='left table-border-td'>" + @item.DueDate.ToString("d") + " - " + @item.ServiceName + "</td>");
                        strPaymentDetail.Append
                        ("<td class='left table-border-td'>" +
                            (@item.DueDate.ToString("d") + " - " + @item.ServiceName)
                            //+ (item.ServiceID >= 9 && item.ServiceID <= 13 ? "<br/>" + Resources.AccountService.PreviousMeasure + ": " + item.PreviousMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.ActualMeasure + ": " + item.ActualMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.WaterConsumption + ": " + item.WaterConsumption.Value.ToString(Common.DecimalPoints) : "")
                            + "</td>"
                        );
                        //
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Balance.ToString("c") + "</td>");
                        strPaymentDetail.Append("</tr>");
                    }
                    else
                    {
                        strPaymentDetail.Append("<tr>");

                        //1-Oct-2019
                        //strPaymentDetail.Append("<td class='left table-border-td'>" + @item.SequenceID + " - " + @item.ServiceName + "</td>");

                        strPaymentDetail.Append
                        ("<td class='left table-border-td'>" +
                            (@item.SequenceID + " - " + @item.ServiceName)
                            //+ (item.ServiceID >= 9 && item.ServiceID <= 13 ? "<br/>" + Resources.AccountService.PreviousMeasure + ": " + item.PreviousMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.ActualMeasure + ": " + item.ActualMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.WaterConsumption + ": " + item.WaterConsumption.Value.ToString(Common.DecimalPoints) : "")
                            + "</td>"
                        );

                        strPaymentDetail.Append("<td class='left table-border-td'>" + @item.DueDate.ToString("d") + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal.ToString("c") + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Interest.ToString("c") + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.LateInterest.ToString("c") + "</td>");
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Payment.ToString("c") + "</td>");
                        strPaymentDetail.Append("</tr>");
                    }
                }
                templateBody = templateBody.Replace("##CollectionDetailItem##", strPaymentDetail.ToString());
            }
            else
            {
                //if (serviceTypeId > 0)
                if (model.PrintTemplate.ID != 6 && model.PrintTemplate.ID != 10 && model.PrintTemplate.ID != 11)
                {
                    templateBody = templateBody.Replace("##PaymentServiceTypeName##", model.Payment.ServiceTypeName);
                    templateBody = templateBody.Replace("##CollectionDetailTotalAmnesty##", model.Payment.AmnestyAmount.ToString("c"));
                    if (model.Payment.Date.Date < new DateTime(2022, 3, 28).Date)
                    {
                        templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.Payment.NetPayment.ToString("c"));
                    }
                    else
                    {
                        templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.AccountServiceCollectionDetail.Sum(x => x.Balance).ToString("c"));
                    }

                    // Set Header Title
                    if (serviceTypeId == 3)
                    {
                        templateBody = templateBody.Replace("##HeaderIVUTitle##", "IVU");
                    }
                    else
                    {
                        templateBody = templateBody.Replace("##HeaderIVUTitle##", "");
                    }

                    foreach (var item in model.AccountServiceCollectionDetail)
                    {
                        strPaymentDetail.Append("<tr>");
                        //if (serviceTypeId == 1)
                        //{
                        //    strPaymentDetail.Append("<td class='center table-border-td'>" + @item.ItemName + " - " + @item.PreviousFiscalYearID + "-" + item.FiscalYearID + "</td>");
                        //    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal1.ToString("c") + "</td>");
                        //    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal2.ToString("c") + "</td>");
                        //}
                        //else
                        //{
                        //strPaymentDetail.Append("<td class='center table-border-td'>" + @item.FiscalYearWithPeriod + "</td>");

                        if (item.FrequencyID < 6)
                        {
                            strPaymentDetail.Append("<td class='center table-border-td'>" + (@item.RightNumber + @item.ServiceName + " - " + String.Format(new System.Globalization.CultureInfo(UserSession.Current.Language), "{0:MMMM yyyy}", item.DueDate)) + (((item.ServiceID >= 9 && item.ServiceID <= 13) || (item.ServiceID >= 19287 && item.ServiceID <= 19291)) ? "<br/>" + Resources.AccountService.PreviousMeasure + ": " + item.PreviousMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.ActualMeasure + ": " + item.ActualMeasure.Value.ToString(Common.DecimalPoints) + " - " + Resources.AccountService.WaterConsumption + ": " + item.WaterConsumption.Value.ToString(Common.DecimalPoints) : "") + "</td>");
                        }
                        else
                        {
                            strPaymentDetail.Append("<td class='center table-border-td'>" + @item.RightNumber + @item.ServiceNameWithYear + "</td>");
                        }

                        //if (serviceTypeId != 20 && serviceTypeId != 22)
                        if (model.PrintTemplate.ID != 9)
                        {
                            strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Principal.ToString("c") + "</td>");
                        }
                        //}
                        //if (serviceTypeId != 20 && serviceTypeId != 22)
                        if (model.PrintTemplate.ID != 9)
                        {
                            strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Penalties.ToString("c") + "</td>");
                            strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Charges.ToString("c") + "</td>");
                            strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Interest.ToString("c") + "</td>");
                            strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Discount.Value.ToString("c") + "</td>");
                        }
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Balance.ToString("c") + "</td>");
                        strPaymentDetail.Append("</tr>");
                    }
                    templateBody = templateBody.Replace("##CollectionDetailItem##", strPaymentDetail.ToString());
                }
                else
                {

                    templateBody = templateBody.Replace("##CollectionDetailTotalBalance##", model.InvoiceDetail.Sum(x => x.Total).Value.ToString("c"));
                    templateBody = templateBody.Replace("##PaymentServiceTypeName##", model.InvoiceDetail.FirstOrDefault().ItemType);

                    foreach (var item in model.InvoiceDetail)
                    {
                        strPaymentDetail.Append("<tr>");
                        if (model.PrintTemplate.ID == 6)
                        {
                            strPaymentDetail.Append("<td class='center table-border-td'>" + @item.Number + "</td>");
                        }

                        strPaymentDetail.Append("<td class='left table-border-td'>" + @item.Date.ToString("d") + "</td>");
                        strPaymentDetail.Append("<td class='left table-border-td'>" + @item.ItemType + "</td>");
                        strPaymentDetail.Append("<td class='left table-border-td'>" + @item.ItemName + "</td>");
                        if(model.PrintTemplate.ID==11)
                        {
                            strPaymentDetail.Append("<td class='left table-border-td'>" + @item.Description + "</td>");
                        }
                        strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Total.Value.ToString("c") + "</td>");
                        strPaymentDetail.Append("</tr>");
                    }

                    templateBody = templateBody.Replace("##CollectionDetailItem##", strPaymentDetail.ToString());
                }
            }

            // Payment Notes
            if (!string.IsNullOrEmpty(model.Payment.Notes))
            {
                StringBuilder strNotes = new StringBuilder();
                strNotes = strNotes.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.COLPayment.InternalNotes + "</td></tr><tr><td>");
                strNotes = strNotes.Append(model.Payment.Notes);
                strNotes = strNotes.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##PaymentNotes##", strNotes.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##PaymentNotes##", "</br>");
            }

            // Payment Attachment
            if (model.Payment.AttachmentID > 0 && includeAttachments)
            {
                StringBuilder strAttachment = new StringBuilder();
                strAttachment = strAttachment.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.Global.Attachment + "</td></tr><tr><td>");
                strAttachment = strAttachment.Append("<a href='" + Common.GetAbsoluteUrl("~/File/DownloadFile?id=" + model.Payment.AttachmentID) + "' >" + model.Payment.FileName + "</a>");
                strAttachment = strAttachment.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##PaymentAttachments##", strAttachment.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##PaymentAttachments##", " ");
            }

            // Payment Void Reason
            if (!string.IsNullOrEmpty(model.Payment.VoidReason))
            {
                StringBuilder strReason = new StringBuilder();
                strReason = strReason.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold'>" + Resources.Global.VoidReason + "</td></tr><tr><td>");
                strReason = strReason.Append(model.Payment.VoidReason);
                strReason = strReason.Append("</td></tr></table><br /><br />");
                templateBody = templateBody.Replace("##PaymentVoidReason##", strReason.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##PaymentVoidReason##", " ");
            }
            // Credit note
            if (model.CreditNote.Count() > 0)
            {
                StringBuilder strCreditNote = new StringBuilder();
                strCreditNote = strCreditNote.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold col-lg-12' colspan='4'>" + Resources.Global.CreditNote + "</td></tr>" +
                    "<tr><td class='left bold col-lg-2'>" + Resources.Global.Date + "</td><td class='left bold col-lg-3'>" + Resources.Global.Number + "</td><td class='left bold col-lg-5'>" + Resources.Global.Reason + "</td><td class='right bold col-lg-2'>" + Resources.Global.Amount + "</td></tr>");
                model.CreditNote.ForEach(d =>
                {
                    strCreditNote = strCreditNote.Append("<tr><td class='left col-lg-2'>" + d.Date.ToString("d") + "</td><td class='left col-lg-3'>" + d.Number + "</td><td class='left col-lg-5'>" + d.Reason + "</td><td class='right col-lg-2'>" + d.Amount.ToString("C") + "</td></tr>");
                });
                strCreditNote = strCreditNote.Append("<tbody></table><br /><br />");
                templateBody = templateBody.Replace("##CreditNote##", strCreditNote.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##CreditNote##", " ");
            }
            //Debit Note
            if (model.DebitNote.Count() > 0)
            {
                StringBuilder strDebitNote = new StringBuilder();
                strDebitNote = strDebitNote.Append("<table cellspacing='0' class='table-border'><tbody><tr><td class='bg-gray bold col-lg-12' colspan='4'>" + Resources.Global.DebitNote + "</td></tr>" +
                    "<tr><td class='left bold col-lg-2'>" + Resources.Global.Date + "</td><td class='left bold col-lg-3'>" + Resources.Global.Number + "</td><td class='left bold col-lg-5'>" + Resources.Global.Reason + "</td><td class='right bold col-lg-2'>" + Resources.Global.Amount + "</td></tr>");
                model.DebitNote.ForEach(d =>
                {
                    strDebitNote = strDebitNote.Append("<tr><td class='left col-lg-2'>" + d.Date.ToString("d") + "</td><td class='left col-lg-3'>" + d.Number + "</td><td class='left col-lg-5'>" + d.Reason + "</td><td class='right col-lg-2'>" + d.Amount.ToString("C") + "</td></tr>");
                });
                strDebitNote = strDebitNote.Append("<tbody></table><br /><br />");
                templateBody = templateBody.Replace("##DebitNote##", strDebitNote.ToString());
            }
            else
            {
                templateBody = templateBody.Replace("##DebitNote##", " ");
            }
            templateBody = templateBody.Replace("##CreditApplied##", model.Payment.ApplyCreditAmount.ToString("C"));

            //Collector Info
            UserProfileViewModel userProfileModel = new UserProfile().GetUserByUserIDs(model.Payment.CreatedUserID.ToString()).FirstOrDefault();
            templateBody = templateBody.Replace("##CollectorName##", string.IsNullOrEmpty(userProfileModel.FullName) ? userProfileModel.Email : userProfileModel.FullName);

            //Country wise Label in Footer
            templateBody = templateBody.Replace("##CountryFooterName##", UserSession.Current.CountryID == 52 ? Global.FooterNameForCostaRica : Global.FooterNameForPuertoRico);

            return templateBody;
        }
        #endregion

        #region Void methods
        public bool Void(PaymentModel model)
        {
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Payment/Void", "POST", null, lstObjParameter);
        }

        public bool PaymentPlanVoid(PaymentModel model)
        {
            model.RowVersion = Convert.FromBase64String(model.RowVersion64);
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Payment/PaymentPlanVoid", "POST", null, lstObjParameter);
        }
        #endregion

        #region Validation methods
        public bool ValidateUserPermissionForManualReceipt()
        {
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/Payment/ValidateUserPermissionForManualReceipt", "GET", null, null);
        }
        public void ValidateInvoicePayment(PaymentModel model)
        {
            if (model.PostedInvoiceList != null)
            {
                model.PostedInvoiceList = model.PostedInvoiceList.Where(x => x.SelectedInvoice == true).ToList();
            }

            if ((model.PaymentOptionList == null || model.PaymentOptionList.Count == 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOnePaymentOption);
            }
            else if ((model.PaymentOptionList == null || model.PaymentOptionList.Sum(x => x.Amount) <= 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValTotalAmountRequired);
            }
            else if (model.PostedInvoiceList == null || model.PostedInvoiceList.Count() <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOneInvoice);
            }
            else if (model.ApplyCreditAmount > model.PostedInvoiceList.Sum(x => x.Balance))
            {
                throw new Exception(Resources.COLPayment.ValCreditCompareItemTotalAmount);
            }
            else if (model.PostedInvoiceList.Sum(x => x.Balance) != ((model.PaymentOptionList?.Sum(x => x.Amount) ?? 0) + model.ApplyCreditAmount))
            {
                throw new Exception(Resources.COLPayment.ValPaymentOptionTotalAmount);
            }
        }
        public void ValidateOtherServicePayment(PaymentModel model)
        {
            if (model.InvoiceDetail.Where(d => d.ServiceID != null && d.ServiceID.Value > 0).Count() > 0 && model.InvoiceDetail.Where(d => d.FINGrantID != null && d.FINGrantID.Value > 0).Count() > 0)
            {
                throw new Exception(Resources.COLInvoice.ServiceTypeMsg);
            }

            if (model.InvoiceDetail.Where(d => d.FINGrantID != null && d.FINGrantID.Value > 0).GroupBy(d => new { d.ItemTypeID, d.FINGrantID }).Any(d => d.Count() > 1))
            {
                throw new Exception(Resources.COLInvoice.GrantServiceCompareMsg);
            }

            if (model.InvoiceDetail.Where(d => d.ServiceID != null && d.ServiceID.Value > 0).GroupBy(d => new { d.ServiceID }).Any(d => d.Count() > 1))
            {
                throw new Exception(Resources.COLInvoice.ServiceCompareMsg);
            }

            if ((model.PaymentOptionList == null || model.PaymentOptionList.Count == 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOnePaymentOption);
            }
            else if ((model.PaymentOptionList == null || model.PaymentOptionList.Sum(x => x.Amount) <= 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValTotalAmountRequired);
            }
            else if (model.ApplyCreditAmount > model.InvoiceDetail.Sum(x => x.Total))
            {
                throw new Exception(Resources.COLPayment.ValCreditCompareItemTotalAmount);
            }
            else if (model.InvoiceDetail.Sum(x => x.Total) != ((model.PaymentOptionList?.Sum(x => x.Amount) ?? 0) + model.ApplyCreditAmount))
            {
                throw new Exception(COLPayment.ValPaymentOptionTotalAmount);
            }
        }
        public void ValidateDebitNotePayment(PaymentModel model)
        {
            if ((model.PaymentOptionList == null || model.PaymentOptionList.Count == 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOnePaymentOption);
            }
            else if ((model.PaymentOptionList == null || model.PaymentOptionList.Sum(x => x.Amount) <= 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValTotalAmountRequired);
            }
            else if (model.ApplyCreditAmount > model.AccountDebitNoteList.Where(x => x.SelectedItem == true).Sum(x => x.Amount))
            {
                throw new Exception(Resources.COLPayment.ValCreditCompareItemTotalAmount);
            }
            else if (model.AccountDebitNoteList.Where(x => x.SelectedItem == true).Sum(x => x.Amount) != ((model.PaymentOptionList?.Sum(x => x.Amount) ?? 0) + model.ApplyCreditAmount))
            {
                throw new Exception(COLPayment.ValPaymentOptionTotalAmount);
            }
        }
        public void ValidateServicePayment(PaymentModel model)
        {
            if ((model.PaymentOptionList == null || model.PaymentOptionList.Count == 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOnePaymentOption);
            }
            else if ((model.PaymentOptionList == null || model.PaymentOptionList.Sum(x => x.Amount) <= 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValTotalAmountRequired);
            }
            else if (model.AccountServiceCollectionDetailList == null || model.AccountServiceCollectionDetailList.Count() <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOneInvoice);
            }
            else if (model.AccountServiceCollectionDetailList.Where(x => x.SelectedItem == true).Count() > 0)
            {
                decimal? TotalSelectAmount = model.AccountServiceCollectionDetailList.Where(x => x.SelectedItem == true).Sum(x => x.Total);
                if (TotalSelectAmount.HasValue)
                {
                    TotalSelectAmount = Convert.ToDecimal(TotalSelectAmount.Value.ToString(Common.DecimalPoints));
                }

                if (model.ApplyCreditAmount > TotalSelectAmount)
                {
                    throw new Exception(Resources.COLPayment.ValCreditCompareItemTotalAmount);
                }
                else if ((((model.PaymentOptionList?.Sum(x => x.Amount) ?? 0) + model.ApplyCreditAmount) > TotalSelectAmount))
                {
                    throw new Exception(Resources.COLPayment.ValPaymentOptionCompareItemTotalAmount);
                }
                else if (((model.PaymentOptionList?.Sum(x => x.Amount) ?? 0) + model.ApplyCreditAmount) < TotalSelectAmount)
                {
                    throw new Exception(Resources.COLPayment.ValPartialPayment);
                }
                else
                {
                    model.AccountServiceCollectionDetailList = model.AccountServiceCollectionDetailList.Where(x => x.SelectedItem == true).ToList();
                }
            }
            else
            {
                throw new Exception(Resources.COLPayment.SelectService);
            }
        }
        public void ValidatePaymentPlanPayment(PaymentModel model)
        {
            List<int> AccountPaymentPlanDetailIDs = new List<int>();

            if (!string.IsNullOrWhiteSpace(model.AccountPaymentPlanDetailIDs))
            {
                AccountPaymentPlanDetailIDs = model.AccountPaymentPlanDetailIDs.Split(',').Select(int.Parse).ToList();
            }

            if ((model.PaymentOptionList == null || model.PaymentOptionList.Count == 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOnePaymentOption);
            }
            else if ((model.PaymentOptionList == null || model.PaymentOptionList.Sum(x => x.Amount) <= 0) && model.ApplyCreditAmount <= 0)
            {
                throw new Exception(Resources.COLPayment.ValTotalAmountRequired);
            }
            else if (model.AccountPaymentPlanDetailIDs == null || model.AccountPaymentPlanDetailIDs.Count() == 0)
            {
                throw new Exception(Resources.COLPayment.ValMinOneItem);
            }
            else if (model.ApplyCreditAmount > (model.PaymentOptionList?.Sum(x => x.Amount).Value ?? 0) + model.ApplyCreditAmount)
            {
                throw new Exception(Resources.COLPayment.ValCreditCompareItemTotalAmount);
            }
            else
            {
                decimal totalamount = (model.PaymentOptionList?.Sum(x => x.Amount).Value ?? 0) + model.ApplyCreditAmount;
                decimal basePayment = 0;
                if (model.AccountPaymentPlanDetailList.Exists(x => x.IsDownPayment == true))
                {
                    basePayment = model.AccountPaymentPlanDetailList.Where(x => x.IsDownPayment == true && AccountPaymentPlanDetailIDs != null && (AccountPaymentPlanDetailIDs.FirstOrDefault(k => k == x.ID) > 0)).Sum(d => d.Balance);
                    if (basePayment != totalamount)
                    {
                        throw new Exception(Resources.COLPayment.ValFullDownPaymentMessage);
                    }
                }
                else
                {
                    basePayment = model.AccountPaymentPlanDetailList.Where(x => x.IsDownPayment == false && AccountPaymentPlanDetailIDs != null && (AccountPaymentPlanDetailIDs.FirstOrDefault(k => k == x.ID) > 0)).Sum(k => k.Balance);
                    while (totalamount > Convert.ToDecimal(0.1) && basePayment > 0)
                    {
                        totalamount = totalamount - basePayment;
                    }
                    if ((totalamount * Convert.ToDecimal(-1)) > Convert.ToDecimal(0.05))
                    {
                        throw new Exception(Resources.COLPayment.ValFullPaymentMessage);
                    }
                }
            }
        }
        public bool ValidatePaymentForVoid(int? FINJournalID, int DocumentTypeID)
        {
            return ((FINJournalID > 0 && new FINBankAccount().CheckJorunalTransactionIsMatched(FINJournalID.Value, DocumentTypeID)) ? true : false);
        }
        #endregion

        #region Receipt by Service additional methods
        //public List<AccountServiceCollectionDetailModel> GetAccountServiceCollectionDetail(int accountId, DateTime? paymentDate, decimal TotalAmount, string selectedItemIds, string CollectionDetailList)
        //{
        //    List<AccountServiceCollectionDetailModel> model = new List<AccountServiceCollectionDetailModel>();
        //    List<int> CollectionID = JsonConvert.DeserializeObject<List<int>>(selectedItemIds);

        //    if (accountId > 0)
        //    {
        //        //model = new AccountServiceCollectionDetailModel().GetNotPaid(accountId, paymentDate);
        //        model = JsonConvert.DeserializeObject<List<AccountServiceCollectionDetailModel>>(CollectionDetailList);

        //        bool IsDiscountProcessForProperty = false;
        //        //if (servicetypeId == 20 || servicetypeId == 22)
        //        //    IsDiscountProcessForProperty = true;
        //        if (model != null && model.FirstOrDefault(d => d.ServiceTypeID == 20 || d.ServiceTypeID == 22) != null)
        //            IsDiscountProcessForProperty = true;
        //        processDiscount(model, TotalAmount, selectedItemIds, IsDiscountProcessForProperty);
        //        //model.ForEach(x => { if (selectedItemIds.Contains(x.ID.ToString())) { x.SelectedItem = true; } });
        //        model.ForEach(x =>
        //        {
        //            if (CollectionID.Where(d => d == x.ID).Count() > 0)
        //            {
        //                x.SelectedItem = true;
        //            }
        //        });
        //    }
        //    return model;
        //}
        public List<AccountServiceCollectionDetailModel> GetAccountServiceCollectionDetail(int accountId, DateTime? paymentDate, string selectedItemIds, bool IsIvaApply = false, bool applybyAmnesty = false)
        {
            decimal TotalAmount = 0;
            List<AccountServiceCollectionDetailModel> model = new List<AccountServiceCollectionDetailModel>();
            List<int> CollectionID = new List<int>();

            if (!string.IsNullOrEmpty(selectedItemIds))
            {
                CollectionID = selectedItemIds.Split(',').Select(int.Parse).ToList();
            }

            if (accountId > 0)
            {
                model = new AccountServiceCollectionDetailModel().GetNotPaid(accountId, paymentDate, IsIvaApply, applybyAmnesty);

                model.ForEach(x =>
                {
                    if (CollectionID.Where(d => d == x.ID).Count() > 0)
                    {
                        x.SelectedItem = true;
                    }
                });

                TotalAmount = model.Where(x => x.SelectedItem == true).Sum(x => x.Total.Value);

                bool IsDiscountProcessForProperty = false;
                if (model != null && model.FirstOrDefault(d => d.ServiceTypeID == 20 || d.ServiceTypeID == 22) != null)
                {
                    IsDiscountProcessForProperty = true;
                }

                processDiscount(model, TotalAmount, selectedItemIds, IsDiscountProcessForProperty);
            }
            return model;
        }

        public List<AccountServiceCollectionDetailModel> GetAccountServiceCollectionDetailOnly(int accountId, DateTime? paymentDate, bool IsIvaApply = false, bool applybyAmnesty = false)
        {
            List<AccountServiceCollectionDetailModel> model = new List<AccountServiceCollectionDetailModel>();

            if (accountId > 0)
            {
                model = new AccountServiceCollectionDetailModel().GetNotPaid(accountId, paymentDate, IsIvaApply, applybyAmnesty);
                bool IsDiscountProcessForProperty = false;
                //if (servicetypeId == 20 || servicetypeId == 22)
                //    IsDiscountProcessForProperty = true;
                if (model != null && model.FirstOrDefault(d => d.ServiceTypeID == 20 || d.ServiceTypeID == 22) != null)
                {
                    IsDiscountProcessForProperty = true;
                }

                processDiscount(model, 0, "", IsDiscountProcessForProperty);
            }
            return model;
        }

        public List<DebitNoteModel> GetDebitNotes(int AccountID, string selectedItemIds)
        {
            decimal TotalAmount = 0;
            List<DebitNoteModel> model = new List<DebitNoteModel>();
            List<String> CollectionID = new List<String>();
            List<Boolean> SelectDebitNotes = new List<Boolean>();

            if (!string.IsNullOrEmpty(selectedItemIds))
            {
                CollectionID = selectedItemIds.Split(',').ToList();
            }

            if (AccountID > 0)
            {
                model = new DebitNoteModel().GetDebitNotes(AccountID);

                model.ForEach(x =>
                {
                    if (CollectionID.Where(d => d.Split('-').First() == x.ID.ToString() && d.Split('-').Last()==x.IsDebitNote.ToString()).Count() > 0 )
                    {
                        x.SelectedItem = true;
                    }
                });

                TotalAmount = model.Where(x => x.SelectedItem == true).Sum(x => x.Amount);
            }
            return model;
        }
        public List<DebitNoteModel> GetDebitNotesOnly(int accountId)
        {
            List<DebitNoteModel> model = new List<DebitNoteModel>();

            if (accountId > 0)
            {
                model = new DebitNoteModel().GetDebitNotes(accountId);
            }
            return model;
        }

        public List<AccountServiceCollectionDetailModel> GetAccountServiceCollectionPropertyTaxDetail(int accountPropertyID, int accountPropertyRightID, DateTime? paymentDate, decimal TotalAmount, string selectedItemIds)
        {
            List<int> selectedItemList = JsonConvert.DeserializeObject<List<int>>(selectedItemIds);
            List<AccountServiceCollectionDetailModel> model = new List<AccountServiceCollectionDetailModel>();
            if (accountPropertyID > 0 & accountPropertyRightID > 0)
            {
                model = new AccountServiceCollectionDetailModel().GetPropertyTaxNotPaid(accountPropertyID, accountPropertyRightID, paymentDate);
                processDiscount(model, TotalAmount, selectedItemIds, true);
                //model.ForEach(x => { if (selectedItemIds.Split(',').ToList().Contains(x.ID.ToString())) { x.SelectedItem = true; } });
                model.ForEach(x => { if (selectedItemList.Count > 0 && (selectedItemList.Where(d => d == x.ID).Count() > 0)) { x.SelectedItem = true; } });
            }
            return model;
        }
        public List<BusinessLicenseAccountServiceModel> GetAccountServiceCollectionDetailForBusinessLicense(int accountId, int servicetypeId, DateTime? paymentDate, decimal TotalAmount, string selectedItemIds)
        {
            List<BusinessLicenseAccountServiceModel> servicemodelList = new List<BusinessLicenseAccountServiceModel>();
            if (accountId > 0 & servicetypeId > 0)
            {
                List<AccountServiceCollectionDetailModel> model = new AccountServiceCollectionDetailModel().GetNotPaid(accountId, paymentDate);
                processDiscountForBusinessLicense(model, TotalAmount, selectedItemIds);
                servicemodelList = new BusinessLicenseAccountServiceModel().GetCollectionDetailSummary(model);
                servicemodelList.ForEach(x => { if (selectedItemIds.Contains(x.ID.ToString())) { x.SelectedItem = true; } });
            }
            return servicemodelList;
        }
        private void processDiscountForBusinessLicense(List<AccountServiceCollectionDetailModel> model, decimal? TotalAmount, string selectedItemIds)

        {
            List<int> selectedItemList = JsonConvert.DeserializeObject<List<int>>(selectedItemIds);
            if (selectedItemList != null && selectedItemList.Count > 0)
            {
                model = model.Where(x => selectedItemIds.Contains(x.AccountServiceID.ToString())).ToList();
            }

            if (TotalAmount == null)
            {
                TotalAmount = 0;
            }

            #region Removing Discount Values if partial payment is done for any collection detail            
            foreach (var obj in model)
            {
                if ((
                    model.Where(x => x.AccountServiceID == obj.AccountServiceID).Any(x => x.CalculatedDiscount == 0) // If any detail has no discount
                    ||
                    obj.TotalPaidAmountSummary > 0  // if partial payment is already done
                    ||
                    model.Where(x => x.AccountServiceID == obj.AccountServiceID).Sum(x => (x.Total - x.CalculatedDiscount)) > TotalAmount // entered amount > total of (all collection detail - applicable discount)
                    ) && TotalAmount > 0)
                {
                    obj.CalculatedDiscount = 0;
                }
                obj.SelectedItem = selectedItemIds.Contains(obj.AccountServiceID.ToString()) ? true : false;
            }
            #endregion

            if (TotalAmount == 0)
            {
                model.ForEach(x => { x.Discount = x.TotalPaidAmountSummary > 0 ? 0 : x.CalculatedDiscount; x.Total = x.Total - x.Discount; });
            }
            else if (TotalAmount > 0)
            {
                foreach (var x in model)
                {
                    decimal? invoiceAmount = x.Total - x.CalculatedDiscount;
                    if (TotalAmount - invoiceAmount >= 0)
                    {
                        x.Discount = x.CalculatedDiscount; x.Total = invoiceAmount;
                        x.Payment = x.Total.Value;
                        TotalAmount = TotalAmount - invoiceAmount;
                    }
                    else
                    {
                        x.Payment = TotalAmount.Value;
                        break;
                    }
                };
            }
        }
        private void processDiscount(List<AccountServiceCollectionDetailModel> model, decimal? TotalAmount, string selectedItemIds, bool IsDiscountProcessForProperty = false)
        {
            List<int> selectedItemList = new List<int>();

            if (!string.IsNullOrEmpty(selectedItemIds))
            {
                selectedItemList = selectedItemIds.Split(',').Select(int.Parse).ToList();
            }
            //List<int> selectedItemList = JsonConvert.DeserializeObject<List<int>>(selectedItemIds);
            if (selectedItemList != null && selectedItemList.Count > 0)
            {
                model = model.Where(x => selectedItemList.Where(d => d == x.ID).Count() > 0).ToList();
            }
            //model = model.Where(x => selectedItemIds.Contains(x.ID.ToString())).ToList();
            if (TotalAmount == null)
            {
                TotalAmount = 0;
            }

            if (TotalAmount == 0)
            {
                model.ForEach(x =>
                {
                    //if (!IsDiscountProcessForProperty) ////CO-904
                    if (!(x.ServiceTypeID == 20 || x.ServiceTypeID == 22))
                    {
                        x.Discount = 0;// x.CalculatedDiscount;
                    }

                    x.Total = x.Total; //- x.CalculatedDiscount;
                    x.Payment = x.Total.Value;

                });
            }
            else if (TotalAmount > 0)
            {
                foreach (var x in model)
                {
                    decimal? invoiceAmount = x.Total; //- x.CalculatedDiscount;
                    if (TotalAmount - invoiceAmount >= 0)
                    {
                        //if (IsDiscountProcessForProperty)
                        //    x.Discount = x.Discount + x.CalculatedDiscount;
                        //if (!IsDiscountProcessForProperty)  //CO-904
                        if (!(x.ServiceTypeID == 20 || x.ServiceTypeID == 22))
                        {
                            x.Discount = 0; //x.CalculatedDiscount;
                        }

                        x.Total = invoiceAmount;
                        x.Payment = x.Total.Value;
                        TotalAmount = TotalAmount - invoiceAmount;
                    }
                    else
                    {
                        //if (IsDiscountProcessForProperty) ////CO-904
                        if (x.ServiceTypeID == 20 || x.ServiceTypeID == 22)
                        {
                            x.Discount = 0;
                        }

                        x.Payment = TotalAmount.Value;
                        break;
                    }
                };
            }
        }
        #endregion
    }
}
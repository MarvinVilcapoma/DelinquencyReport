using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPaymentPlan
    {
        #region Public Methods 
        public AccountPaymentPlanModel Get(int? id = null)
        {
            AccountPaymentPlanModel model = new AccountPaymentPlanModel();
            if (id > 0)
            {
                model = Get(id, null, null).FirstOrDefault();
                if (model != null)
                {
                    model.AccountList = HMTLHelperExtensions.CreateSelectList(new AccountModel().GetForSearch(null, model.AccountID, null), "id", "text");
                    model.ServiceTypeList = HMTLHelperExtensions.CreateSelectList(new ServiceTypeModel().GetNotPaid(model.AccountID, false), "ID", "Name", null, true, true, Global.DropDownSelectMessage);

                    if (id > 0)
                        model.ServicePaymentPlanList = HMTLHelperExtensions.CreateSelectList(new Services.Models.PaymentPlan().Get(model.ServicePaymentPlanID, null, true), "ID", "Name", model.ServicePaymentPlanID);
                    else
                        model.ServicePaymentPlanList = HMTLHelperExtensions.CreateSelectList(new Services.Models.PaymentPlan().Get(null, model.StartDate, true), "ID", "Name", model.ServicePaymentPlanID);

                    model.AccountServiceCollectionDetailList = new AccountServicePaymentPlanModel().GetSummary(id.Value);

                    if (model.ID > 0 && model.AccountServiceCollectionDetailList != null & model.AccountServiceCollectionDetailList.Any())
                    {
                        model.AccountServiceCollectionDetailList.ForEach(x =>
                        {
                            x.SelectedItem = true;
                        });
                    }

                    model.AccountPaymentPlanDetailList = new AccountPaymentPlanDetail().Get(null, id.Value.ToString(), true, null,null);

                    if (model.AccountPaymentPlanDetailList != null && model.AccountPaymentPlanDetailList.Any())
                        model.MonthlyInterest = model.AccountPaymentPlanDetailList.ToList().Where(x => x.SequenceID == 1).Sum(x => x.Interest);

                    if (model.AccountServiceCollectionDetailList != null && model.AccountServiceCollectionDetailList.Any())
                    {
                        model.AccountPaymentPlanServiceIDs = String.Join(",", model.AccountServiceCollectionDetailList.Select(x => x.AccountServiceID));
                        model.AccountPaymentPlanServiceCollectionDetailIDs = String.Join(",", model.AccountServiceCollectionDetailList.Select(x => x.ID));
                    }
                }
                else
                {
                    model = new AccountPaymentPlanModel();
                }
            }
            return model;
        }

        public List<AccountPaymentPlanModel> Get(int? ID, int? accountID, bool? isActive)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "id", Value = ID },
                new NameValuePair { Name = "accountID", Value = accountID },
                new NameValuePair { Name = "isActive", Value = isActive}
            };
            return new RestSharpHandler().RestRequest<List<AccountPaymentPlanModel>>(APISelector.Municipality, true, "api/AccountPaymentPlan/Get", "GET", lstParameter);
        }

        public List<AccountPaymentPlanModel> GetNotPaid(int? ID, int? accountID, bool? isActive, DateTime? receiptDate)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "id", Value = ID },
                new NameValuePair { Name = "accountID", Value = accountID },
                new NameValuePair { Name = "date", Value = receiptDate.HasValue? receiptDate.Value.ToString(CultureInfo.InvariantCulture):Common.CurrentDateTime.ToString(CultureInfo.InvariantCulture) },
                new NameValuePair { Name = "isActive", Value = isActive}
            };
            return new RestSharpHandler().RestRequest<List<AccountPaymentPlanModel>>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetNotPaid", "GET", lstParameter);
        }
        public bool IsAccountHavePaymentPlan(int ID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "accountID", Value = ID }
            };
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountPaymentPlan/IsAccountHavePaymentPlan", "GET", lstParameter);
        }

        public AccountPaymentPlanModel GetPaymentPlanSummary(int paymentPlanId, decimal totalDebt, DateTime startDate, decimal InstalmentPercentage, decimal InterestPercentage, decimal LateInterestPercentage, int Months, int accountId, string accountServiceIds, string ServiceCollectionDetailIDs, bool forEdit, bool applybyAmnesty)
        {
            PaymentPlanModel paymentPlanModel = new Services.Models.PaymentPlan().Get(paymentPlanId, null, true).FirstOrDefault();
            List<AccountServiceCollectionDetailModel> serviceCollectionDetailModel = new List<AccountServiceCollectionDetailModel>();
            serviceCollectionDetailModel = new AccountServiceCollectionDetailModel().GetNotPaidSummary(accountId, ServiceCollectionDetailIDs, forEdit, applybyAmnesty).ToList();
            List<int> AccountServiceIds = new List<int>();
            List<int> ServiceCollectionDetailIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(accountServiceIds))
                AccountServiceIds = accountServiceIds.Split(',').Select(int.Parse).ToList();
            if (!string.IsNullOrWhiteSpace(ServiceCollectionDetailIDs))
                ServiceCollectionDetailIds = ServiceCollectionDetailIDs.Split(',').Select(int.Parse).ToList();
            paymentPlanModel.InstalmentPercentage = InstalmentPercentage;
            paymentPlanModel.InterestPercentage = InterestPercentage;
            paymentPlanModel.Months = Months;
            var instalmentAmount = (paymentPlanModel.InstalmentPercentage * totalDebt) / 100;
            var principal = ((totalDebt - instalmentAmount) / paymentPlanModel.Months);
            //var interest = (paymentPlanModel.InterestPercentage * principal) / 100;
            var interest = (paymentPlanModel.InterestPercentage * totalDebt) / 100;
            var monthlyAmount = principal + interest;
            //var monthlyAmount = principal;
            //var totalIntrest = interest * paymentPlanModel.Months;

            AccountPaymentPlanModel model = new AccountPaymentPlanModel();
            model.StartDate = startDate;
            model.EndDate = startDate.AddMonths(instalmentAmount > 0 ? paymentPlanModel.Months : paymentPlanModel.Months - 1);
            model.InstalmentPercentage = paymentPlanModel.InstalmentPercentage;
            model.InterestPercentage = paymentPlanModel.InterestPercentage;
            model.TotalDebt = totalDebt;
            model.InstalmentAmount = instalmentAmount;
            model.Months = paymentPlanModel.Months;
            model.MonthlyAmount = monthlyAmount;
            List<AccountPaymentPlanDetailModel> accountPaymentPlanDetailList = new List<AccountPaymentPlanDetailModel>();
            if (instalmentAmount > 0)
            {
                //foreach (var item in serviceCollectionDetailModel.Where(d => AccountServiceIds != null && (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0) && ServiceCollectionDetailIds != null && (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).GroupBy(d => new { d.AccountServiceName, d.ServiceID, d.AccountServiceID, d.ID }).Select(d => new { AccountServiceName = d.Key.AccountServiceName, ServiceID = d.Key.ServiceID, AccountServiceID = d.Key.AccountServiceID, AccountServiceCollectionDetailID = d.Key.ID, TotalDebt = d.Sum(k => k.Total) }).Distinct().ToList())
                foreach (var item in serviceCollectionDetailModel.Where(
                    d => AccountServiceIds != null &&
                    (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0) &&
                    ServiceCollectionDetailIds != null &&
                    (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).GroupBy(d => new { d.AccountServiceName, d.ServiceID, d.AccountServiceID, d.ID, d.ActualMeasure, d.PreviousMeasure, d.WaterConsumption }).Select(d => new { AccountServiceName = d.Key.AccountServiceName, ServiceID = d.Key.ServiceID, AccountServiceID = d.Key.AccountServiceID, AccountServiceCollectionDetailID = d.Key.ID, ActualMeasure = d.Key.ActualMeasure, PreviousMeasure = d.Key.PreviousMeasure, WaterConsumption = d.Key.WaterConsumption, TotalDebt = d.Sum(k => k.Total), ServiceName = d.Min(k => k.ServiceName) }).Distinct().ToList())
                //foreach (var item in serviceCollectionDetailModel.Where(
                //    d => AccountServiceIds != null &&
                //    (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0) &&
                //    ServiceCollectionDetailIds != null &&
                //    (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).GroupBy(d => new { d.ServiceName, d.ServiceID }).Select(d => new { AccountServiceName = d.Key.ServiceName, ServiceID = d.Key.ServiceID, AccountServiceID = d.Min(k => k.AccountServiceID), AccountServiceCollectionDetailID = d.Min(k => k.ID), ActualMeasure = d.Max(k => k.ActualMeasure), PreviousMeasure = d.Min(k => k.PreviousMeasure), WaterConsumption = (d.Max(k => k.ActualMeasure) - d.Min(k => k.PreviousMeasure)), TotalDebt = d.Sum(k => k.Total) }).Distinct().ToList())
                {
                    var ServiceinstalmentAmount = (paymentPlanModel.InstalmentPercentage * item.TotalDebt) / 100;
                    var Serviceprincipal = ((item.TotalDebt - ServiceinstalmentAmount) / paymentPlanModel.Months);
                    //var Serviceinterest = (paymentPlanModel.InterestPercentage * Serviceprincipal) / 100;
                    var Serviceinterest = (paymentPlanModel.InterestPercentage * item.TotalDebt) / 100;
                    if (startDate.Date >= DateTime.Now.Date)
                    {
                        Serviceinterest = 0;
                    }
                    var ServicemonthlyAmount = Serviceprincipal + Serviceinterest;
                    //var ServicetotalIntrest = Serviceinterest * paymentPlanModel.Months;
                    accountPaymentPlanDetailList.Add(new AccountPaymentPlanDetailModel()
                    {
                        SequenceID = 0,
                        DueDate = startDate,
                        MonthlyDueDate = DateTime.Parse(startDate.ToString()).ToString("d", CultureInfo.InvariantCulture),
                        Principal = decimal.Parse(ServiceinstalmentAmount.Value.ToString(Common.DecimalPoints)),
                        Balance = decimal.Parse(ServiceinstalmentAmount.Value.ToString(Common.DecimalPoints)),
                        //Principal = ServiceinstalmentAmount??0,
                        //Balance = ServiceinstalmentAmount??0,
                        IsDownPayment = true,
                        IsActive = true,
                        ServiceName = item.ServiceName,
                        AccountServiceName = item.AccountServiceName,
                        ActualMeasure = item.ActualMeasure,
                        PreviousMeasure = item.PreviousMeasure,
                        WaterConsumption = item.WaterConsumption,
                        ServiceID = item.ServiceID,
                        AccountServiceID = item.AccountServiceID ?? 0,
                        AccountServiceCollectionDetailID = item.AccountServiceCollectionDetailID ?? 0,
                        LateInterest = 0
                        //AccountServiceIDs = string.Join(",", serviceCollectionDetailModel.Where(d =>d.ServiceID == item.ServiceID && AccountServiceIds != null && (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0)).Select(d=>d.AccountServiceID).Distinct().ToList()),
                        //AccountServiceCollectionDetailIDs = string.Join(",", serviceCollectionDetailModel.Where(d => d.ServiceID == item.ServiceID && ServiceCollectionDetailIds != null && (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).Select(d => d.ID).Distinct().ToList())
                    });
                }

                startDate = startDate.AddMonths(1);
            }

            int sequenceID = 1;
            for (int i = 1; i <= paymentPlanModel.Months; i++)
            {
                if (i > 1)
                {
                    startDate = startDate.AddMonths(1);
                    sequenceID = sequenceID + 1;
                }

                foreach (var item in serviceCollectionDetailModel.Where(
                    d => AccountServiceIds != null &&
                    (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0) &&
                    ServiceCollectionDetailIds != null &&
                    (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).GroupBy(d => new
                    {
                        d.AccountServiceName,
                        d.ServiceID,
                        d.AccountServiceID,
                        d.ID,
                        d.ActualMeasure,
                        d.PreviousMeasure,
                        d.WaterConsumption
                    }).Select(d => new { AccountServiceName = d.Key.AccountServiceName, ServiceID = d.Key.ServiceID, AccountServiceID = d.Key.AccountServiceID, AccountServiceCollectionDetailID = d.Key.ID, ActualMeasure = d.Key.ActualMeasure, PreviousMeasure = d.Key.PreviousMeasure, WaterConsumption = d.Key.WaterConsumption, TotalDebt = d.Sum(k => k.Total), ServiceName = d.Min(k => k.ServiceName) }).Distinct().ToList())
                {
                    //    foreach (var item in serviceCollectionDetailModel.Where(
                    //    d => AccountServiceIds != null &&
                    //    (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0) &&
                    //    ServiceCollectionDetailIds != null &&
                    //    (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).GroupBy(d => new
                    //    {
                    //        d.ServiceName,
                    //        d.ServiceID
                    //    }).Select(d => new { AccountServiceName = d.Key.ServiceName, ServiceID = d.Key.ServiceID, AccountServiceID = d.Min(k => k.AccountServiceID), AccountServiceCollectionDetailID = d.Min(k => k.ID), ActualMeasure = d.Max(k => k.ActualMeasure), PreviousMeasure = d.Min(k => k.PreviousMeasure), WaterConsumption = (d.Max(k => k.ActualMeasure) - d.Min(k => k.PreviousMeasure)), TotalDebt = d.Sum(k => k.Total) }).Distinct().ToList())
                    //{
                    var ServiceinstalmentAmount = (paymentPlanModel.InstalmentPercentage * item.TotalDebt) / 100;
                    var Serviceprincipal = ((item.TotalDebt - ServiceinstalmentAmount) / paymentPlanModel.Months);
                    //var Serviceinterest = (paymentPlanModel.InterestPercentage * Serviceprincipal) / 100;
                    var Serviceinterest = (paymentPlanModel.InterestPercentage * item.TotalDebt) / 100;
                    //if (startDate.Date >= DateTime.Today.Date)// 20-Apr-2020
                    //{
                    //    Serviceinterest = 0;
                    //}
                    //else // 20-Apr-2020
                    //{
                    //    //Serviceinterest = Serviceinterest * (Convert.ToDecimal(DateTime.Today.Subtract(startDate).Days / (365.25 / 12)) > 0 ? Convert.ToInt32(DateTime.Today.Subtract(startDate).Days / (365.25 / 12)) : 1);
                    //    Serviceinterest = Serviceinterest * ((DateTime.Today.Subtract(startDate).Days / 30) > 0 ? (DateTime.Today.Subtract(startDate).Days / 30) + 1 : 1);
                    //}
                    model.TotalInterest += Serviceinterest ?? 0;
                    var ServicemonthlyAmount = Serviceprincipal + Serviceinterest;
                    //var ServicetotalIntrest = Serviceinterest * paymentPlanModel.Months;
                    decimal ServiceLateInterest = 0;
                    if (startDate.Date < DateTime.Now.Date && UserSession.Current.CountryID == 52)
                    {
                        PaymentPlanModel _paymentPlanModel = new Services.Models.PaymentPlan().Get(null, startDate.Date, true).FirstOrDefault();

                        if (_paymentPlanModel != null)
                        {
                            ServiceLateInterest += decimal.Parse((((Convert.ToDecimal(ServicemonthlyAmount) * Convert.ToDecimal(_paymentPlanModel.LateInterestPercentage)) / 100)
                                                   * ((DateTime.Now.Date.Month - startDate.Month) + ((DateTime.Now.Date.Year - startDate.Year) * 12))).ToString(Common.DecimalPoints));

                        }

                        //if (startDate.Date < new DateTime(2012, 04, 01).Date)
                        //{
                        //    ServiceLateInterest = decimal.Parse(
                        //        (((Convert.ToDouble((ServicemonthlyAmount) * 2)) / 100)
                        //        * ((new DateTime(2012, 03, 31).Month - startDate.Month) + ((new DateTime(2012, 03, 31).Year - startDate.Year) * 12))
                        //        + (((Convert.ToDouble((ServicemonthlyAmount)) * 1.5) / 100)
                        //        * ((DateTime.Now.Date.Month - new DateTime(2012, 03, 31).Month) + ((DateTime.Now.Date.Year - new DateTime(2012, 03, 31).Year) * 12)))
                        //        ).ToString(Common.DecimalPoints));

                        //    //ServiceLateInterest = decimal.Parse(
                        //    //        (((Convert.ToDouble((ServicemonthlyAmount) * 2)) / 100)
                        //    //        * ((new DateTime(2012, 03, 31).Month - startDate.Month) + ((new DateTime(2012, 03, 31).Year - startDate.Year) * 12))
                        //    //        + (((Convert.ToDouble((ServicemonthlyAmount)) * 1.5) / 100)
                        //    //        * ((DateTime.Now.Date.Month - new DateTime(2012, 03, 31).Month) + ((DateTime.Now.Date.Year - new DateTime(2012, 03, 31).Year) * 12)))
                        //    //        ).ToString());

                        //}
                        //else
                        //{
                        //    ServiceLateInterest = decimal.Parse((((Convert.ToDecimal(ServicemonthlyAmount) * Convert.ToDecimal(1.5)) / 100)
                        //                           * ((DateTime.Now.Date.Month - startDate.Month) + ((DateTime.Now.Date.Year - startDate.Year) * 12))).ToString(Common.DecimalPoints));

                        //    //ServiceLateInterest = decimal.Parse((((Convert.ToDecimal(ServicemonthlyAmount) * Convert.ToDecimal(1.5)) / 100)
                        //    //                           * ((DateTime.Now.Date.Month - startDate.Month) + ((DateTime.Now.Date.Year - startDate.Year) * 12))).ToString());

                        //}
                    }
                    accountPaymentPlanDetailList.Add(new AccountPaymentPlanDetailModel()
                    {
                        SequenceID = sequenceID,
                        DueDate = startDate,
                        MonthlyDueDate = DateTime.Parse(startDate.ToString()).ToString("d", CultureInfo.InvariantCulture),
                        Principal = decimal.Parse(Serviceprincipal.Value.ToString(Common.DecimalPoints)),
                        Interest = decimal.Parse(Serviceinterest.Value.ToString(Common.DecimalPoints)),
                        LateInterest = ServiceLateInterest,
                        //Balance = decimal.Parse((ServicemonthlyAmount.Value + ServiceLateInterest).ToString(Common.DecimalPoints)),
                        Balance = decimal.Parse((Serviceprincipal.Value + Serviceinterest.Value + ServiceLateInterest).ToString(Common.DecimalPoints)),


                        //Principal = Serviceprincipal??0,
                        //Interest = Serviceinterest??0,
                        //LateInterest = ServiceLateInterest,
                        //Balance = (Serviceprincipal ?? 0) + (Serviceinterest ?? 0) + ServiceLateInterest,
                        IsActive = true,
                        ServiceName = item.ServiceName,
                        AccountServiceName = item.AccountServiceName,
                        ServiceID = item.ServiceID,
                        ActualMeasure = item.ActualMeasure,
                        PreviousMeasure = item.PreviousMeasure,
                        WaterConsumption = item.WaterConsumption,
                        AccountServiceID = item.AccountServiceID ?? 0,
                        AccountServiceCollectionDetailID = item.AccountServiceCollectionDetailID ?? 0
                        //AccountServiceIDs = string.Join(",", serviceCollectionDetailModel.Where(d => d.ServiceID == item.ServiceID && AccountServiceIds != null && (AccountServiceIds.FirstOrDefault(k => k == d.AccountServiceID) > 0)).Select(d => d.AccountServiceID).Distinct().ToList()),
                        //AccountServiceCollectionDetailIDs = string.Join(",", serviceCollectionDetailModel.Where(d => d.ServiceID == item.ServiceID && ServiceCollectionDetailIds != null && (ServiceCollectionDetailIds.FirstOrDefault(k => k == d.ID) > 0)).Select(d => d.ID).Distinct().ToList())
                    });
                }
            }
            model.TotalPayment = totalDebt + model.TotalInterest;
            model.AccountPaymentPlanDetailList = accountPaymentPlanDetailList;
            model.MonthlyInterest = model.AccountPaymentPlanDetailList.Where(d => d.IsDownPayment == false && d.SequenceID == 1).Sum(d => d.Interest);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            model.AccountPaymentPlanDetailJson = serializer.Serialize(accountPaymentPlanDetailList);

            return model;
        }

        public PaymentPlanModel GetPaymentPlan(int paymentPlanId)
        {
            PaymentPlanModel model = new Services.Models.PaymentPlan().Get(paymentPlanId, null, true).FirstOrDefault();
            return model;
        }

        public int Insert(AccountPaymentPlanModel model)
        {
            model.IsEditable = false;
            model.IsActive = true;
            model.InactiveDate = null;
            model.InactiveReason = null;
            model.IsDeleted = false;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.CreatedBy = UserSession.Current.Username;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountPaymentPlan/Insert", "POST", null, lstObjParameter);
        }
        public int Update(AccountPaymentPlanModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountPaymentPlan/Update", "POST", null, lstObjParameter);
        }
        public int UpdateStatus(bool isActive, int id)
        {
            AccountPaymentPlanModel model = Get(id, null, null).FirstOrDefault();
            model.ID = id;
            model.IsActive = isActive;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/AccountPaymentPlan/UpdateStatus", "POST", null, lstObjParameter);
        }

        public void Delete(int id, string Reason)
        {
            AccountPaymentPlanModel model = Get(id);
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.ReasonForDeleted = Reason;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountPaymentPlan/Delete", "POST", null, lstObjParameter);
        }
        #endregion
    }
    public class AccountPaymentPlanPrint
    {
        #region Public Methods 
        public AccountPaymentPlanPrintModel Print(int accountID, int AccountPaymentPlanID)
        {
            AccountPaymentPlanPrintModel model = new AccountPaymentPlanPrintModel();

            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "accountID", Value = accountID },
                new NameValuePair { Name = "AccountPaymentPlanID", Value = AccountPaymentPlanID  },
            };

            model = new RestSharpHandler().RestRequest<AccountPaymentPlanPrintModel>(APISelector.Municipality, true, "api/AccountPaymentPlan/Print", "GET", lstParameter);
            model.CompanyModel = new RestSharpHandler().RestRequest<CompanyModel>(APISelector.Municipality, true, "api/Company/Get", "GET", null);
            return model;
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPaymentPlanModel
    {
        #region public properties 
        public int ID { get; set; }
        public int AccountID { get; set; }
        //public int ServiceTypeID { get; set; }
        public int ServicePaymentPlanID { get; set; }
        public string Number { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal InstalmentPercentage { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal LateInterestPercentage { get; set; }
        public int Months { get; set; }
        public bool IsEditable { get; set; }
        public decimal InstalmentAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalDebt { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public string InactiveReason { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public string ReasonForDeleted { get; set; }
        public int IsDeletePermission { get; set; }
        public bool ApplybyAmnesty { get; set; }
        public decimal AmnestyAmount { get; set; }
        public int PaidMonths { get; set; }
        public int NotPaidMonths { get; set; }
        #endregion

        #region custom properties               
        public List<SelectListItem> AccountList { get; set; }
        public List<SelectListItem> ServiceTypeList { get; set; }
        public List<SelectListItem> ServicePaymentPlanList { get; set; }
        public List<AccountServiceCollectionDetailModel> AccountServiceCollectionDetailList { get; set; }
        public List<AccountPaymentPlanDetailModel> AccountPaymentPlanDetailList { get; set; }
        public string AccountPaymentPlanDetailJson { get; set; }
        public string AccountServiceCollectionDetailIDs { get; set; }
        public string AccountPaymentPlanServiceIDs { get; set; }
        public string AccountPaymentPlanServiceCollectionDetailIDs { get; set; }
        public string DisplayName { get; set; }
        public string PaymentPlanName { get; set; }
        public string StartDateByCulture { get; set; }
        public string EndDateByCulture { get; set; }
        public string MonthlyAmountByCulture { get; set; }
        public string TotalPaymentByCulture { get; set; }
        public string PaymentPlanNameWithNumber { get; set; }
        public bool IsAllowedDelete
        {
            get
            {
                if (IsDeletePermission == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Constructor
        public AccountPaymentPlanModel()
        {
            this.AccountList = new List<SelectListItem>();
            this.ServiceTypeList = new List<SelectListItem>();
            this.ServicePaymentPlanList = new List<SelectListItem>();
            this.AccountServiceCollectionDetailList = new List<AccountServiceCollectionDetailModel>();
            this.AccountPaymentPlanDetailList = new List<AccountPaymentPlanDetailModel>();
        }
        #endregion
    }
    public class AccountPaymentPlanListModel
    {
        #region Property Section 
        public List<AccountPaymentPlanModel> AccountPaymentPlanList { get; set; }
        public List<AccountPaymentPlanModel> ExportPaymentPlanList { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Method Section
        public AccountPaymentPlanListModel GetByPaging(string filterText, int? accountID, int? accountPropertyID, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "filterText", Value = filterText},
                new NameValuePair { Name = "accountID", Value = accountID},
                new NameValuePair { Name = "accountPropertyID", Value = accountPropertyID},
                new NameValuePair { Name = "pageIndex", Value = pageIndex},
                new NameValuePair { Name = "pageSize", Value = pageSize},
                new NameValuePair { Name = "sortColumn", Value = sortColumn},
                new NameValuePair { Name = "sortOrder", Value = sortOrder}
            };
            AccountPaymentPlanListModel list = new RestSharpHandler().RestRequest<AccountPaymentPlanListModel>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetByPaging", "GET", lstParameter);
            list.AccountPaymentPlanList.ForEach(a =>
            {
                a.StartDateByCulture = a.StartDate.ToString("d");
                a.EndDateByCulture = a.EndDate.ToString("d");
                a.MonthlyAmountByCulture = a.MonthlyAmount.ToString("C");
                a.TotalPaymentByCulture = a.TotalPayment.ToString("C");
            });
            return list;
        }

        public AccountPaymentPlanListModel GetExportPaymentPlan()
        {
            AccountPaymentPlanListModel list = new RestSharpHandler().RestRequest<AccountPaymentPlanListModel>(APISelector.Municipality, true, "api/AccountPaymentPlan/GetExportPaymentPlan", "GET", null);
            return list;
        }
        #endregion
    }
    public class AccountPaymentPlanPrintModel
    {
        public List<AccountPaymentPlanPrintDetailModel> AccountPaymentPlanList { get; set; }
        public AccountPaymentPlanModel AccountPaymentPlanModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public CompanyModel CompanyModel { get; set; }
    }
    public class AccountPaymentPlanPrintDetailModel
    {
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string Periods { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal LateInterest { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public DateTime StartDate { get; set; }
    }
}
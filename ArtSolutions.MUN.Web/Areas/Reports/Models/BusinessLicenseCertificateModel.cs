﻿using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class BusinessLicenseCertificateModel : DataExportModel
    {
        #region public properties     
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public List<SelectListItemViewModel> AccountList { get; set; }
        public string[] AccountIDs { get; set; }
        public List<BusinessLicenseCertificateModelList> BusinessLicenseCertificateModelList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        public string AccountPaymentPlanID { get; set; }    
        public DateTime FutureDate { get; set; }
        #endregion

        #region Constructor
        public BusinessLicenseCertificateModel()
        {
            this.AccountList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class BusinessLicenseCertificateModelList
    {
        #region public properties
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string AccountDisplayName { get; set; }
        public string AccountRegisterNumber { get; set; }
        public string Number { get; set; }
        public decimal Principal1 { get; set; }
        public decimal Principal2 { get; set; }
        public Nullable<decimal> Penalties { get; set; }
        public Nullable<decimal> Charges { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string FormattedFiscalYearID { get; set; }
        public decimal Balance { get; set; }
        #endregion

        #region custom properties       
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }       
        public string FormattedPrincipal1
        {
            get
            {
                return Principal1.ToString("C");
            }
        }
        public string FormattedPrincipal2
        {
            get
            {
                return Principal2.ToString("C");
            }
        }
        public string FormattedPenalties
        {
            get
            {
                return Penalties.Value.ToString("C");
            }
        }
        public string FormattedCharges
        {
            get
            {
                return Charges.Value.ToString("C");
            }
        }
        public string FormattedInterest
        {
            get
            {
                return Interest.Value.ToString("C");
            }
        }
        public string FormattedDiscount
        {
            get
            {
                return Discount.Value.ToString("C");
            }
        }
        public string FormattedBalanceamount
        {
            get
            {
                return Balance.ToString("C");
            }
        }
        #endregion
    }
}
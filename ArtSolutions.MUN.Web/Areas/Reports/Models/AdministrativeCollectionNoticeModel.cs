using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class AdministrativeCollectionNoticeModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime? NotificationExpirationDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public String RepresentativesName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime? NotificationDate { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SummaryAccountStatementList> SummaryAccountStatementList { get; set; }
        public List<AccountPropertyList> AccountPropertyList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public AdministrativeCollectionNoticeModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class AccountPropertyList
    {
        public string FincaID { get; set; }
        public decimal TotalValue { get; set; }
    }
}
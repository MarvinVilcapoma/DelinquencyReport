using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class SuspensionOrderForWaterServiceModel
    {
        #region public properties    
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? AccountId { get; set; }
        public List<AccountSearchModel> AccountList { get; set; }
        public List<SuspensionOrderForWaterServiceList> SuspensionOrderForWaterServiceList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public AccountModel AccountModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public SuspensionOrderForWaterServiceModel()
        {
            this.AccountList = new List<AccountSearchModel>();
        }
        #endregion
    }
    public class SuspensionOrderForWaterServiceList
    {
        public string ServiceName { get; set; }
        public decimal? TotalPrincipal { get; set; }
        public decimal? TotalInterest { get; set; }
        public decimal? Total { get; set; }
        public string Meter { get; set; }
        public string FincaID { get; set; }
        public int ServiceTypeID { get; set; }
    }
}
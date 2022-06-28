﻿using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class CollectionDepositSummaryModel
    {
        #region public properties
        public List<DepositEntryModel> DepositEntryList { get; set; }
        public List<ClosingEntryModel> ClosingEntryList { get; set; }
        public Int32 TotalDepositRecord { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public decimal NetDepositFrom { get; set; }
        public decimal NetDepositTo { get; set; }
        public List<SelectListItemViewModel> BankAccountList { get; set; }
        public string FilterBankAccountID { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class TransfersReportModel
    {
        #region public properties     
        public int? TransferTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<SelectListItemViewModel> TransferTypeList { get; set; }
        public List<TransfersReportList> TransfersReportList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public TransfersReportModel()
        {
            this.TransferTypeList = new List<SelectListItemViewModel>();
        }
        #endregion
    }
    public class TransfersReportList
    {
        #region public properties
        public string RightNumber { get; set; }
        public int TransferTypeID { get; set; }
        public string TransferType { get; set; }
        public string PreviousOwner { get; set; }
        public string NewRight { get; set; }
        public string CurrentOwner { get; set; }
        public int? Year { get; set; }
        public decimal? TotalValue { get; set; }
        public string Service { get; set; }
        public decimal? BalanceAmountByService { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime TransferDate { get; set; }
        public string Observation { get; set; }
        #endregion

        #region custom properties 
        public string FormattedCreatedUser
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.CreatedUserID.ToString()).FirstOrDefault();
                return model == null ? "-" : (string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName);
            }
        }
        public string FormattedTransferDate
        {
            get
            {
                return TransferDate.ToString("d");
            }
        }
        public string FormattedObservation
        {
            get
            {
                return Observation.Length > 40 ? Observation.Substring(0, 40) + "..." : Observation;
            }
        }
        public string FormattedPreviousOwner
        {
            get
            {
                return PreviousOwner.Length > 40 ? PreviousOwner.Substring(0, 40) + "..." : PreviousOwner;
            }
        }
        public string FormattedCurrentOwner
        {
            get
            {
                return CurrentOwner.Length > 40 ? CurrentOwner.Substring(0, 40) + "..." : CurrentOwner;
            }
        }
        public string FormattedService
        {
            get
            {
                return Service.Length > 40 ? Service.Substring(0, 40) + "..." : Service;
            }
        }
        #endregion
    }
}
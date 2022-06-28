using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class CreditHistoryModel
    {
        #region public properties
        public int ID { get; set; }
        public int AccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime Date { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal CreditAmount { get; set; }
        public int? PaymentID { get; set; }
        public string PaymentNumber { get; set; }
        public string Number { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<CreditHistoryModel> CreditHistoryModelList { get; set; }
        public string AccountDisplayName { get; set; }
        public string ReceiptNumber { get; set; }
        public int ServiceTypeID { get; set; }
        public string PaymentPlanName { get; set; }
        public decimal AvailableAmount { get; set; }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("d");
            }
        }
        public string FormattedAmount
        {
            get
            {
                if (PaymentID == null)
                {
                    return CreditAmount.ToString("C");
                }
                else
                {
                    return (CreditAmount * -1).ToString("C");
                }
            }
        }
        public decimal CreditBalance { get; set; }
        #endregion

        #region Constructroctor
        public CreditHistoryModel()
        {
            CreditHistoryModelList = new List<CreditHistoryModel>();
        }
        #endregion

        #region public method
        public CreditHistoryModel Get(int accountID)
        {
            CreditHistoryModel model = new CreditHistoryModel();
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
                model = new RestSharpHandler().RestRequest<CreditHistoryModel>(APISelector.Municipality, true, "api/Account/CreditHistoryGet", "GET", lstParameter, null);
                model.CreditBalance = (model.CreditHistoryModelList.Where(x => x.PaymentID == null).Sum(a => a.CreditAmount)) - 
                                      (model.CreditHistoryModelList.Where(a => a.PaymentID != null).Sum(a => a.CreditAmount));
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int Insert(CreditHistoryModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/CreditHistoryInsert", "POST", null, lstObjParameter);
        }
        public CreditHistoryDetailModel GetCreditHistoryDetail(int id)
        {
            CreditHistoryDetailModel model = new CreditHistoryDetailModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            model = new RestSharpHandler().RestRequest<CreditHistoryDetailModel>(APISelector.Municipality, true, "api/Account/CreditHistoryDetailGet", "GET", lstParameter, null);
            if (model.CreditHistory != null)
            {
                model.Date = model.CreditHistory.Date.ToString();
            }
            return model;
        }
        #endregion
    }
    public class CreditHistoryDetail
    {
        public DateTime Date { get; set; }
        public bool IsVoid { get; set; }
        public decimal Amount { get; set; }
        public string Number { get; set; }
        public int ID { get; set; }
        public string PaymentPlanName { get; set; }
        public int ServiceTypeID { get; set; }
    }

    public class CreditHistoryDetailModel
    {
        public CreditHistoryModel CreditHistory { get; set; }
        public List<CreditHistoryDetail> CreditHistoryDetailList { get; set; }
        public string Date { get; set; }
    }
}
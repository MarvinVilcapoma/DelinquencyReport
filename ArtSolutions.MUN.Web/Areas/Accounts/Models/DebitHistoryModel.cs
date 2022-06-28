using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class DebitHistoryModel
    {
        #region public properties
        public int ID { get; set; }
        public int AccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public DateTime Date { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Reason { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal DebitAmount { get; set; }
        public int? PaymentID { get; set; }
        public string PaymentNumber { get; set; }
        public string Number { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<DebitHistoryModel> DebitHistoryModelList { get; set; }
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
                    return DebitAmount.ToString("C");
                }
                else
                {
                    return (DebitAmount * -1).ToString("C");
                }
            }
        }
        public decimal DebitBalance { get; set; }
        public string AccountDisplayName { get; set; }
        public string ReceiptNumber { get; set; }
        public int ServiceTypeID { get; set; }
        public string PaymentPlanName { get; set; }
        public bool IsPaid { get; set; }
        #endregion

        #region Constructroctor
        public DebitHistoryModel()
        {
            DebitHistoryModelList = new List<DebitHistoryModel>();
        }
        #endregion

        #region public method
        public DebitHistoryModel Get(int accountID)
        {
            DebitHistoryModel model = new DebitHistoryModel();
            try
            {
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
                model = new RestSharpHandler().RestRequest<DebitHistoryModel>(APISelector.Municipality, true, "api/Account/DebitHistoryGet", "GET", lstParameter, null);
                decimal totalAmount = model.DebitHistoryModelList.Sum(a => a.DebitAmount);
                decimal debitAmount = model.DebitHistoryModelList.Where(a => a.PaymentID != null && a.IsPaid == true).Sum(a => a.DebitAmount);
                model.DebitBalance = totalAmount - debitAmount;
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int Insert(DebitHistoryModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.IsPaid = false;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Account/DebitHistoryInsert", "POST", null, lstObjParameter);
        }
        #endregion
    }

    public class DebitHistoryDetailModel
    {
        public DebitHistoryModel DebitHistory { get; set; }
        public List<DebitHistoryPaidDetailModel> DebitHistoryPaidList { get; set; }
        public string Date { get; set; }
        #region public method
        public DebitHistoryDetailModel DebitHistoryDetailGet(int ID)
        {
            DebitHistoryDetailModel model = new DebitHistoryDetailModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            model = new RestSharpHandler().RestRequest<DebitHistoryDetailModel>(APISelector.Municipality, true, "api/Account/DebitHistoryDetailGet", "GET", lstParameter, null);
            if (model.DebitHistory != null)
            {
                model.Date = model.DebitHistory.Date.ToString();
            }
            return model;
        }
        #endregion
    }
    public class DebitHistoryPaidDetailModel
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool ISVoid { get; set; }
        public decimal Amount { get; set; }
    }
}
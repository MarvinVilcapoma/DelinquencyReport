using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class FinSalesCashierDetailsModel
    {
        #region public properties
        public List<FinSalesCashierDetailModel> SalesCashierDetais { get; set; }
        public FinSalesCashierModel SalesCashier { get; set; }
        #endregion

        #region Public Methods
        public FinSalesCashierDetailsModel GetSalesCashierDetails(int? id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            return new RestSharpHandler().RestRequest<FinSalesCashierDetailsModel>(APISelector.Municipality, true, "api/Finance/GetSalesCashierDetails", "GET", lstParameter, null);
        }
        #endregion
    }
    public class FinSalesCashierDetailModel
    {
        public int ID { get; set; }
        public int CashierID { get; set; }
        public int CompanyID { get; set; }
        public int PaymentOptionID { get; set; }
        public int? BankAccountID { get; set; }
        public int FinancePaymentAccountID { get; set; }
        public bool CreateDeposit { get; set; }
        public string FinancePaymentAccountName { get; set; }
    }

    public class FinSalesCashierModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SegmentID { get; set; }
        public int? AccountCodeID { get; set; }
        public List<FinSalesCashierDetailModel> SalesCashierDetailList { get; set; }
    }
}
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class FINPaymentOptionsModel
    {
        #region Properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int SequenceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ImageID { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion
    }

    public class PaymentOptions
    {
        #region Methods
        public List<FINPaymentOptionsModel> Get(int? cashierID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "cashierID", Value = cashierID });
            return new RestSharpHandler().RestRequest<List<FINPaymentOptionsModel>>(APISelector.Municipality, true, "api/Payment/PaymentOptionsGet", "GET", lstParameter, null);
        }
        #endregion
    }
}
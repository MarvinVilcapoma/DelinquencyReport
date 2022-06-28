using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class InvoiceDetailModel
    {
        #region public properties       
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int InvoiceID { get; set; }
        public int ItemTypeTableID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int ItemTypeID { get; set; }
        public int? AccountServiceCollectionDetailID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int? ServiceID { get; set; }
        public int? FINGrantID { get; set; }
        public string Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal? Total { get; set; }
        public decimal Payment { get; set; }
        public decimal Balance { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion

        #region Reference
        public IEnumerable<SelectListItem> InvoiceTypeList { get; set; }
        public List<FinClassGrantModel> FinClassGrantList { get; set; }
        public List<CheckbookAccountListModel> FinCheckBookList { get; set; }
        public List<ServiceModel> ServiceList { get; set; }
        #endregion

        #region Extra Properties
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public string ServiceName { get; set; }
        public string FINGrantName { get; set; }
        public string FINGrantCode { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        //Exctra Column For Receivable, Revenu & Cash Account
        public int? ReceivableAccountID { get; set; }
        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public int? RevenueAccountID { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public int? CheckbookID { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public int? CashAccountID { get; set; }
        public string CashAccountCode { get; set; }
        public string CashAccountName { get; set; }
        public int? CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        #endregion

        #region Methods
        public List<InvoiceDetailModel> Get(int? id, int? invoiceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "InvoiceID", Value = invoiceId });
            return new RestSharpHandler().RestRequest<List<InvoiceDetailModel>>(APISelector.Municipality, true, "api/Invoice/DetailGet", "GET", lstParameter);
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ArtSolutions.MUN.Web.Helpers.EnumUtility;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCollectionTemplateDetailModel
    {
        #region Properties
        public int ID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public string Year { get; set; }
        public int YearID { get; set; }
        public int YearTableID { get; set; }
        public int ServiceCollectionTemplateID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int GrantID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int CollectionTypeID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int ReceivableAccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public int RevenueAccountID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        public decimal Percentage { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUserID { get; set; }

        public string GrantCode { get; set; }
        public string GrantName { get; set; }

        public string ReceivableCode { get; set; }
        public string ReceivableName { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }

        public string CheckbookCode { get; set; }
        public int? CheckbookID { get; set; }
        public string CheckbookName { get; set; }       
        public int? CashAccountID { get; set; }
        public string CashAccountCode { get; set; }        
        public string CashAccountName { get; set; }

        public int? CreditAccountID { get; set; }
        public string CreditAccountCode { get; set; }
        public string CreditAccountName { get; set; }

        #region Reference Properties        
        public List<FinAccountModel> ReceivableAccountList { get; set; }
        public List<FinAccountModel> RevenueAccountList { get; set; }
        public List<CheckbookAccountListModel> CheckbookAccountList { get; set; }
        #endregion
        #region Extra Properties
        public bool IsRemoved { get; set; }
        #endregion
        public ServiceCollectionTemplateDetailModel()
        {
            ReceivableAccountList = new List<FinAccountModel>();
            RevenueAccountList = new List<FinAccountModel>();
            CheckbookAccountList = new List<CheckbookAccountListModel>();
        }
        #endregion

        #region Public Methods
        public List<ServiceCollectionTemplateDetailModel> Get(int? id, int collectionTemplateId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "CollectionTemplateID", Value = collectionTemplateId });
            return new RestSharpHandler().RestRequest<List<ServiceCollectionTemplateDetailModel>>(APISelector.Municipality, true, "api/CollectionTemplate/DetailGet", "GET", lstParameter);
        }
        #endregion
    }
}
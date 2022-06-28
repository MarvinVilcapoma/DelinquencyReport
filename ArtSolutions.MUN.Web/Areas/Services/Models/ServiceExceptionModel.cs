using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceExceptionModel
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ServiceID { get; set; }        
        public decimal ExceptionValue { get; set; }
        public string FormattedExceptionValue { get { return ExceptionValue.ToString(Common.DecimalPoints); } }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Sort { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SequenceID { get; set; }
        public int AllowDelete { get; set; }
        #region Public Methods
        public List<ServiceExceptionModel> Get(int? id , int? serviceID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = id });
            lstParameter.Add(new NameValuePair { Name = "ServiceID", Value = serviceID });
            return  new RestSharpHandler().RestRequest<List<ServiceExceptionModel>>(APISelector.Municipality, true, "api/Service/ExceptionGet", "GET", lstParameter);            
        }
        #endregion
    }
    public class ServiceExceptionViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]        
        public decimal ExceptionValue { get; set; }        
        public string Description { get; set; }       
        public List<ServiceExceptionModel> ServiceExceptionList { get; set; }
    }
}
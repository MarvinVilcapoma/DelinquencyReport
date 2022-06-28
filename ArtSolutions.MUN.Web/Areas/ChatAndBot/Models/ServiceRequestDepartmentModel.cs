using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ServiceRequestDepartmentModel
    {
        #region Property Section
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsPublic { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string FilterText { get; set; }
        #endregion

        #region Method Section
        /// <summary>
        /// GetDepartmentByCompanyID - used to fetch service request type department based on companyID
        /// </summary>       
        /// <returns>List of ServiceRequestDepartmentModel</returns>        
        public List<ServiceRequestDepartmentModel> GetDepartmentByCompanyID(bool? isActive = null, int workflowID = 0)
        {
            try
            {
                List<NameValuePair> _lstParameter = new List<NameValuePair>() {
                    new NameValuePair { Name = "id", Value = this.ID },
                    new NameValuePair { Name = "filtertext", Value = this.FilterText },
                    new NameValuePair { Name = "workflowID", Value = workflowID },
                    new NameValuePair { Name = "isActive", Value = isActive }
                };

                string _strURL = "api/Department/Get";
                RestSharpHandler objRestSharpHandler = new RestSharpHandler();
                List<ServiceRequestDepartmentModel> lstServiceRequest = objRestSharpHandler.RestRequest<List<ServiceRequestDepartmentModel>>(APISelector.CustomerService, true, _strURL, "GET", _lstParameter);
                return lstServiceRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
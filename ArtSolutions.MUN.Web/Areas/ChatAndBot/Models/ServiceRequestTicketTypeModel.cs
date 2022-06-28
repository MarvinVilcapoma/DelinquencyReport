using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ServiceRequestTicketTypeModel
    {
        #region Property Section
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public System.Guid? WorkflowID { get; set; }
        public Nullable<int> FormID { get; set; }
        public string Code { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public Nullable<int> ImageID { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        #endregion

        #region Method Section
        /// <summary>
        /// GetTicketTypeByDepartmentID - used to fetch service request type ticket type based on departmentID
        /// </summary>       
        /// <param name="iDepartmentID">integer DepartmentID</param>
        /// <returns>List of ServiceRequestTicketTypeModel</returns>        
        public List<ServiceRequestTicketTypeModel> GetTicketTypeByDepartmentID(int iDepartmentID = 0, int iTicketTypeID = 0, int iWorkflowID = 0, bool? isDeleted = null)
        {
            try
            {
                string url = "api/ServiceRequest/GetTicketTypeByDepartmentID";
                RestSharpHandler objRestSharpHandler = new RestSharpHandler();

                List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "iDepartmentID", Value = iDepartmentID },
                new NameValuePair { Name = "iTicketTypeID", Value = iTicketTypeID },
                new NameValuePair { Name = "workflowID", Value = iWorkflowID },
                new NameValuePair { Name = "isDeleted", Value = isDeleted }
                };

                List<ServiceRequestTicketTypeModel> lstServiceRequest = objRestSharpHandler.RestRequest<List<ServiceRequestTicketTypeModel>>(APISelector.CustomerService, true, url, "GET", lstParameter);
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
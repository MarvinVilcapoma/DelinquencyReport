using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ServiceRequestDepartmentUserModel
    {
        #region Property Section
        public Nullable<System.Guid> UserID { get; set; }
        public int DepartmentID { get; set; }
        public string UserName { get; set; }
        public int CompanyID { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string Email { get; set; }
        public int? ProfilePictureID { get; set; }
        #endregion

        #region Method Section
        /// <summary>
        /// GetDepartmentUserByDepartmentID - used to fetch service request type department user based on departmentID
        /// </summary>       
        /// <param name="iDepartmentID">integer DepartmentID</param>
        /// <returns>List of ServiceRequestDepartmentUserModel</returns>        
        public List<ServiceRequestDepartmentUserModel> GetDepartmentUserByCompanyID(int iDepartmentID)
        {
            try
            {
                string url = "api/ServiceRequest/GetDepartmentUserByDepartmentID";
                RestSharpHandler objRestSharpHandler = new RestSharpHandler();
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "iDepartmentID", Value = Convert.ToString(iDepartmentID) });
                List<ServiceRequestDepartmentUserModel> lstServiceRequest = objRestSharpHandler.RestRequest<List<ServiceRequestDepartmentUserModel>>(APISelector.CustomerService, true, url, "GET", lstParameter);
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
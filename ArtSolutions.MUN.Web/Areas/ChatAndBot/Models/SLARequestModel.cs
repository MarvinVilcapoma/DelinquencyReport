using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class SLARequestModel
    {
        #region Property Section
        public int ID { get; set; }
        public int TicketTypeID { get; set; }
        public string ServiceRequestName { get; set; }
        [Required(ErrorMessageResourceName = "FieldRequiredMessage", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public decimal FirstResponse { get; set; }
        [Required(ErrorMessageResourceName = "FieldRequiredMessage", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public decimal TotalTime { get; set; }
        public bool TrackTime { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public string FilterString { get; set; }
        public List<SelectListItem> lstRequestType { get; set; }
        public bool DisplayMode { get; set; }
        #endregion

        #region Method Section
        /// <summary>
        /// GetSLARequestByCompantID - used to get SLA Request data to display on list page
        /// </summary>                  
        /// <param name="slaModel">object of SLA Request model</param>
        /// <returns>List<ServiceRequestSLAModel> - List of SLA Request model</returns>         
        public List<SLARequestModel> GetSLARequestByCompanyID(SLARequestModel slaModel = null)
        {
            RestSharpHandler _objRestSharpHandler;
            try
            {
                _objRestSharpHandler = new RestSharpHandler();

                string _strURL = "api/RequestTypeSLA/Get";
                List<NameValuePair> lstParameter = new List<NameValuePair>();

                lstParameter.Add(new NameValuePair { Name = "iID", Value = slaModel.ID });
                lstParameter.Add(new NameValuePair { Name = "iTicketTypeID", Value = slaModel.TicketTypeID });
                lstParameter.Add(new NameValuePair { Name = "strFilterString", Value = slaModel.FilterString });
                List<SLARequestModel> lstSLAModel = _objRestSharpHandler.RestRequest<List<SLARequestModel>>(APISelector.CustomerService, true, _strURL, "GET", lstParameter);

                return lstSLAModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using static ArtSolutions.MUN.Web.Common;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class ServiceRequestModel
    {
        #region Property Section       
        public int ID { get; set; }
        public int Sequence { get; set; }
        public int CompanyID { get; set; }
        public int? RequesterID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public Guid? RequesterUserID { get; set; }
        public Nullable<int> DocumentRelatedID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public string Subject { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public int CategoryID { get; set; }
        [Required]
        public int PriorityID { get; set; }
        public Nullable<int> StatusID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public int DepartmentID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public int ChannelID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public Guid? AgentID { get; set; }
        public string ImageID { get; set; }
        public string FilterText { get; set; }
        //public System.Guid WorkflowID { get; set; }
        public int WorkflowID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public int TicketTypeID { get; set; }
        public Guid? SourceID { get; set; }
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DueDate { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public string DeptUserName { get; set; }
        public string RequesterName { get; set; }
        public string RequesterEmail { get; set; }
        public string RequesterPhone { get; set; }
        public string AgentEmail { get; set; }
        public bool IsFollowerActive { get; set; }
        public byte[] RowVersion { get; set; }
        public List<SelectListItem> lstPriorities { get; set; }
        public List<SelectListItem> lstCategories { get; set; }
        public List<SelectListItem> lstChannels { get; set; }
        public List<SelectListItem> lstDepartments { get; set; }
        public List<SelectListItem> lstTicketTypes { get; set; }
        public List<SelectListItem> lstAssignedTo { get; set; }
        public List<SelectListItem> lstStatus { get; set; }
        public List<SelectListItem> lstRequester { get; set; }
        public List<ServiceRequestResponseModel> lstServiceResponse { get; set; }
        public List<FieldPropertiesModel> lstFields { get; set; }
        public int? FormID { get; set; }
        public string JsonSchema { get; set; }
        public string JsonResult { get; set; }
        public SRFieldFormModel objFieldForm { get; set; }
        public Guid UserID { get; set; }
        public bool IsFromChatWindow { get; set; }

        public int DocumentTypeID { get; set; }
        public int? RelatedDocID { get; set; }
        public string WorkflowVersionID { get; set; }
        public int? WorkflowReasonID { get; set; }

        #endregion

        /// <summary>
        /// ServiceRequestDefaultAttributes - used to get default attribute of service request
        /// </summary>                               
        /// <returns>List<ServiceRequestModel> - object of ServiceRequestDefaultAttributes class</returns>    
        public ServiceRequestDefaultAttributes GetServiceRequestDefaultAttributes(int iDepartmentID = 0)
        {
            try
            {
                string _strURL = "api/ServiceRequest/GetServiceRequestDefaultAttributes";
                RestSharpHandler objRestSharpHandler = new RestSharpHandler();
                List<NameValuePair> lstParameter = new List<NameValuePair>();
                lstParameter.Add(new NameValuePair { Name = "iCompanyID", Value = UserSession.Current.CompanyID });
                lstParameter.Add(new NameValuePair { Name = "iDepartmentID", Value = iDepartmentID });
                ServiceRequestDefaultAttributes objServiceRequestDefaultAttributes = objRestSharpHandler.RestRequest<ServiceRequestDefaultAttributes>(APISelector.CustomerService, true, _strURL, "GET", lstParameter, null);
                return objServiceRequestDefaultAttributes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// InsertServiceRequestData - used to Insert service request data
        /// </summary>                  
        /// <returns>object - objServiceRequest of newly inserted ticket</returns>  
        /// <param name="srModel">object of service request model</param>
        public ServiceRequestModel InsertServiceRequestData(ServiceRequestModel srModel)
        {
            RestSharpHandler _objRestSharpHandler;
            try
            {
                _objRestSharpHandler = new RestSharpHandler();

                srModel.CompanyID = UserSession.Current.CompanyID;
                srModel.CreatedDate = srModel.ModifiedDate = Common.CurrentDateTime;
                srModel.CreatedUserID = srModel.ModifiedUserID = srModel.UserID = UserSession.Current.Id;
                //Fixed value which we need to change later               
                srModel.DocumentRelatedID = srModel.RequesterID = srModel.Sequence = 1;
                srModel.StatusID = srModel.StatusID;
                //srModel.WorkflowID = Guid.Parse("a259808e-9f66-4237-ba93-b9698fc8d7b4");
                srModel.WorkflowID = srModel.WorkflowID;
                //End Fixed value which we need to change later                
                srModel.ImageID = !string.IsNullOrEmpty(srModel.ImageID) ? srModel.ImageID : "";
                if (srModel.objFieldForm != null)
                {
                    srModel.FormID = srModel.objFieldForm.FormID;
                    srModel.JsonSchema = srModel.objFieldForm.JsonSchema;
                    srModel.JsonResult = srModel.objFieldForm.JsonResult;
                }
                if (srModel.DueDate == null)
                {
                    srModel.DueDate = Common.CurrentDateTime;
                }

                srModel.DocumentTypeID = (int)DocumentTypeEnum.ServiceRequest;
                srModel.RelatedDocID = null;
                srModel.WorkflowVersionID = "1";
                srModel.WorkflowReasonID = 0;

                string url = "api/ServiceRequest/Insert";
                List<object> lstObject = new List<object>();
                lstObject.Add(srModel);
                ServiceRequestModel objServiceRequest = _objRestSharpHandler.RestRequest<ServiceRequestModel>(APISelector.CustomerService, true, url, "POST", null, lstObject);

                return objServiceRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SendMailNotification - used to send mail notification when any service request status change
        /// </summary>                         
        /// <param name="strToMail">string to whom we want to send mail notification</param>
        /// <param name="iEventID">int eventID which is used to fetch template based on it</param>
        /// <param name="replaceContent">Dictionary<string,string> replaceContent which is used to replace mail content</param>
        public void SendMailNotification(string strToMail, int iEventID, Dictionary<string, string> replaceContent = null, List<string> lstToMail = null)
        {
            try
            {

                //************************ SendMail Code **************************//                            
                MailBoxModel mailBoxModel = new MailBoxModel();
                if (!string.IsNullOrEmpty(strToMail))
                    mailBoxModel.To = strToMail;
                if (lstToMail != null && lstToMail.Count > 0)
                    mailBoxModel.lstTo = lstToMail;
                mailBoxModel.EventId = iEventID;


                if (replaceContent != null)
                {
                    List<NOTElement> elements = new List<NOTElement>();
                    foreach (var item in replaceContent)
                    {
                        elements.Add(new NOTElement() { Code = item.Key, Value = item.Value });
                    }
                    mailBoxModel.Elements = elements;
                }
                //string securityCode = Url.Encode(Common.Encrypt(Model.Email.Trim()));
                //elements.Add(new NOTElement() { Code = "[InviteUrl]", ElementValue = Url.Action("PasswordReset", "Account", new { area = "" }, protocol: Request.Url.Scheme) + "?Email=" + securityCode });

                string _strURL = "api/notification/SendEmail";
                Common.SendMail(mailBoxModel, _strURL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FillServiceRequestDataForChat - used to fill service request model data for chat
        /// </summary>                  
        /// <returns>object of ServiceRequest Model</returns>  
        public ServiceRequestModel FillServiceRequestDataForChat(ServiceRequestModel objServiceRequestModel, bool isSetDefaultTextInListItem = false)
        {
            ServiceRequestDepartmentModel _objServiceRequestDepartment;
            ServiceRequestTicketTypeModel _objServiceRequestTicketType;

            try
            {
                _objServiceRequestDepartment = new ServiceRequestDepartmentModel();
                _objServiceRequestTicketType = new ServiceRequestTicketTypeModel();

                //Fill department select list items in lstDepartments
                List<ServiceRequestDepartmentModel> lstSRDepartment = _objServiceRequestDepartment.GetDepartmentByCompanyID();
                objServiceRequestModel.lstDepartments = HMTLHelperExtensions.CreateSelectList(lstSRDepartment, "ID", "Name", (objServiceRequestModel != null && objServiceRequestModel.DepartmentID > 0 ? lstSRDepartment.Where(x => x.ID == objServiceRequestModel.DepartmentID).Select(x => x.ID).FirstOrDefault() : 0), isSetDefaultTextInListItem, false, Resources.Global.DropDownSelectMessage);

                //Set departmentId which is used to fill next two dropdown during page load
                if (lstDepartments != null && lstDepartments.Count > 0 && !isSetDefaultTextInListItem)
                {
                    if (objServiceRequestModel != null && objServiceRequestModel.DepartmentID > 0)
                    {
                        objServiceRequestModel.DepartmentID = Convert.ToInt32(lstDepartments.Where(x => x.Selected == true).FirstOrDefault().Value);
                    }
                    else
                    {
                        objServiceRequestModel.DepartmentID = Convert.ToInt32(lstDepartments.Where(x => Convert.ToInt32(x.Value) > 0).FirstOrDefault().Value);
                    }
                }

                //Fill ticket type select list items in lstRequestType
                List<ServiceRequestTicketTypeModel> lstSRTicketType = _objServiceRequestTicketType.GetTicketTypeByDepartmentID(this.DepartmentID);
                objServiceRequestModel.lstTicketTypes = HMTLHelperExtensions.CreateSelectList(lstSRTicketType, "ID", "Name", null, isSetDefaultTextInListItem, false, Resources.Global.DropDownSelectMessage);

                return objServiceRequestModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime SetDueDateFromRequestType(int iTicketTypeID)
        {
            try
            {
                //Set due date based on request type              
                SLARequestModel _objSLARequest = new SLARequestModel();
                _objSLARequest.TicketTypeID = iTicketTypeID;

                _objSLARequest = _objSLARequest.GetSLARequestByCompanyID(_objSLARequest).FirstOrDefault();
                if (_objSLARequest != null)
                {
                    int totalDays = Convert.ToInt32(_objSLARequest.TotalTime / 24);
                    return DateTime.Now.AddDays(totalDays);
                }
                else
                    return DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class TicketTypeWorkflow
    {
        public int TicketTypeID { get; set; }
        public int WorkflowID { get; set; }
        public string Name { get; set; }

        public List<TicketTypeWorkflow> GetTicketTypeWorkflow(int iTicketTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                    new NameValuePair { Name = "iTicketTypeID", Value = iTicketTypeID }
            };
            return new RestSharpHandler().RestRequest<List<TicketTypeWorkflow>>(APISelector.CustomerService, true, "api/ServiceRequest/GetTicketTypeWorkflow", "GET", lstParameter);
        }
    }

    public class ServiceRequestDefaultAttributes
    {
        public int PriorityID { get; set; }
        public int CategoryID { get; set; }
        public int WebChannelID { get; set; }
        public int ChatChannelID { get; set; }
        public int BOTChannelID { get; set; }
        public System.Guid DepartmentUserID { get; set; }
        public int StatusID { get; set; }
    }
}
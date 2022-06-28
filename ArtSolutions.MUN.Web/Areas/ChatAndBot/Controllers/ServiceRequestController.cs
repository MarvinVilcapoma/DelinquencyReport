using ArtSolutions.MUN.Web.Areas.ChatAndBot.Models;
using ArtSolutions.MUN.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Controllers
{
    public class ServiceRequestController : BaseController
    {
        [HttpPost]
        public JsonResult InsertServiceRequest(ServiceRequestModel srModel)
        {
            ServiceRequestModel _objServiceRequest;
            try
            {
                if (srModel.objFieldForm != null && !string.IsNullOrEmpty(srModel.objFieldForm.JsonSchema))
                {
                    List<FieldPropertiesModel> lstSchema = JsonConvert.DeserializeObject<List<FieldPropertiesModel>>(srModel.objFieldForm.JsonSchema);
                    List<FieldPropertiesModel> lstResult = new List<FieldPropertiesModel>();

                    List<string> lstDefaultProperty = new List<string> { "Description", "StatusID", "ImageID", "RequesterUserID", "PriorityID", "Subject", "CategoryID",
                                                                         "ChannelID", "DueDate", "DepartmentID", "TicketTypeID", "AgentID", "objFieldForm.JsonSchema",
                                                                         "X-Requested-With", "objFieldForm.FormID" };

                    FieldPropertiesModel _objFieldProperty = null;
                    for (int i = 0; i < Request.Form.Count; i++)
                    {
                        if (lstDefaultProperty.Any(x => x.Contains(Request.Form.Keys[i])))
                            continue;
                        _objFieldProperty = new FieldPropertiesModel();
                        _objFieldProperty = lstSchema.Where(x => x.name.Trim().ToLower().Equals(Request.Form.Keys[i].Trim().ToLower())).FirstOrDefault();
                        switch (_objFieldProperty.type)
                        {
                            case "checkbox-group":
                                var chkValues = Request.Form.GetValues(Request.Form.Keys[i]);
                                var k = 0;
                                if (chkValues.Length > 1)
                                {
                                    for (int j = 0; j < chkValues.Length; j++)
                                    {
                                        if (j > 0 && Convert.ToBoolean(chkValues[j - 1]))
                                            continue;
                                        _objFieldProperty.values[k++].selected = Convert.ToBoolean(chkValues[j]);
                                    }
                                }
                                break;
                            case "radio-group":
                            case "select":
                                _objFieldProperty.values.ToList().ForEach(x => x.selected = false);
                                if (_objFieldProperty.multiple)
                                {
                                    if (!string.IsNullOrEmpty(Request.Form[i]))
                                    {
                                        foreach (var item in Request.Form[i].Split(','))
                                        {
                                            _objFieldProperty.values.Where(x => x.value.Equals(item)).FirstOrDefault().selected = true;
                                        }
                                    }
                                }
                                else
                                {
                                    _objFieldProperty.values.Where(x => x.value.Equals(Request.Form[i])).FirstOrDefault().selected = true;
                                }
                                break;
                            default:
                                _objFieldProperty.value = Request.Form[i];
                                break;
                        }

                        lstResult.Add(_objFieldProperty);
                        lstSchema.Remove(_objFieldProperty);
                    }
                    srModel.objFieldForm.JsonResult = JsonConvert.SerializeObject(lstResult);
                }

                _objServiceRequest = new ServiceRequestModel();
                ServiceRequestDefaultAttributes _objServiceRequestDefaultAttributes = new ServiceRequestDefaultAttributes();

                _objServiceRequestDefaultAttributes = _objServiceRequest.GetServiceRequestDefaultAttributes(srModel.DepartmentID);

                if (_objServiceRequestDefaultAttributes != null)
                {
                    srModel.PriorityID = (srModel != null && srModel.PriorityID > 0 ? srModel.PriorityID : _objServiceRequestDefaultAttributes.PriorityID);
                    srModel.CategoryID = (srModel != null && srModel.CategoryID > 0 ? srModel.CategoryID : _objServiceRequestDefaultAttributes.CategoryID);
                    srModel.StatusID = (srModel != null && srModel.StatusID > 0 ? srModel.StatusID : _objServiceRequestDefaultAttributes.StatusID);
                    srModel.AgentID = (srModel != null && srModel.AgentID != null ? srModel.AgentID : _objServiceRequestDefaultAttributes.DepartmentUserID);

                    if (srModel.IsFromChatWindow)
                        srModel.ChannelID = (srModel != null && srModel.ChannelID > 0 ? srModel.ChannelID : _objServiceRequestDefaultAttributes.ChatChannelID);
                    else
                        srModel.ChannelID = _objServiceRequestDefaultAttributes.WebChannelID;
                    
                }

                srModel.DueDate = srModel.SetDueDateFromRequestType(srModel.TicketTypeID);

                List<TicketTypeWorkflow> lstTicketTypeWorkflow = new TicketTypeWorkflow().GetTicketTypeWorkflow(srModel.TicketTypeID);
                srModel.WorkflowID = lstTicketTypeWorkflow[0].WorkflowID;

                _objServiceRequest = _objServiceRequest.InsertServiceRequestData(srModel);
                if (_objServiceRequest != null)
                {
                    Dictionary<string, string> replaceContent = null;
                    var _SuccessMsg = Resources.Global.SavedSuccessfullyMessage;
                    if (!srModel.IsFromChatWindow)
                        TempData["SuccessMsg"] = _SuccessMsg;

                    if (!string.IsNullOrEmpty(_objServiceRequest.AgentEmail))
                    {
                        replaceContent = new Dictionary<string, string>
                        {
                            { "[ServiceName]", _objServiceRequest.Subject },
                            { "[UserName]",_objServiceRequest.AgentEmail}
                        };
                        _objServiceRequest.SendMailNotification(_objServiceRequest.AgentEmail, 16, replaceContent); //Mail to Requester
                    }

                    if (_objServiceRequest.RequesterUserID != null)
                    {
                        UserDataModel _objUserDetails = new UserDataModel();

                        _objUserDetails = _objUserDetails.GetUsers(Convert.ToString(_objServiceRequest.RequesterUserID)).FirstOrDefault();

                        replaceContent = new Dictionary<string, string>
                        {
                            { "[Requester]", _objUserDetails.Email }
                        };
                        _objServiceRequest.SendMailNotification(_objUserDetails.Email, 17, replaceContent); //Mail to Requester                  
                    }
                    if (!srModel.IsFromChatWindow)
                        return Json(new { status = true, message = _SuccessMsg }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { status = true, message = _SuccessMsg, data = _objServiceRequest.ID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var _ErrorMsg = ex.Message;
                return Json(new { status = false, message = _ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTicketTypeAndDeptUserIDByDepartmentID(string departmentID)
        {
            ServiceRequestTicketTypeModel _objServiceRequestTicketType;
            try
            {
                _objServiceRequestTicketType = new ServiceRequestTicketTypeModel();
                if (string.IsNullOrEmpty(departmentID)) departmentID = "0";

                List<ServiceRequestTicketTypeModel> lstSRTicketType = _objServiceRequestTicketType.GetTicketTypeByDepartmentID(Convert.ToInt32(departmentID));
                ViewBag.DropDownData = HMTLHelperExtensions.CreateSelectList(lstSRTicketType, "ID", "Name", null, true, false, Resources.Global.DropDownSelectMessage);

                ServiceRequestDepartmentUserModel _objServiceRequestDepartmentUser = new ServiceRequestDepartmentUserModel(); ;

                _objServiceRequestDepartmentUser = _objServiceRequestDepartmentUser.GetDepartmentUserByCompanyID(Convert.ToInt32(departmentID)).Where(x => x.IsPrimaryContact == true).FirstOrDefault();
                string _strDeptUserID = string.Empty;
                if (_objServiceRequestDepartmentUser != null)
                {
                    _strDeptUserID = Convert.ToString(_objServiceRequestDepartmentUser.UserID);
                }
                string _strPartialView = RenderPartialToString(this, "~/Areas/ChatAndBot/Views/Shared/_DropDownListFill.cshtml", ViewBag.DropDownData);
                return Json(new { html = _strPartialView, data = _strDeptUserID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return PartialView("~/Areas/ServiceRequest/Views/Shared/_DropDownListFill.cshtml");
            }
        }

        public static string RenderPartialToString(Controller controller, string partialViewName, object model)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName);

            if (result.View != null)
            {
                controller.ViewData.Model = model;
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(controller.ControllerContext, result.View, controller.ViewData, controller.TempData, output);
                        result.View.Render(viewContext, output);
                    }
                }
                return sb.ToString();
            }
            return String.Empty;
        }
    }
}
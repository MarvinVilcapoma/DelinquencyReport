using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtSolutions.MUN.Web.Areas.Cases.Models;
using System.Threading.Tasks;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using System.IO;

namespace ArtSolutions.MUN.Web.Areas.Cases.Controllers
{
    public class CaseController : BaseController
    {
        #region "Cases"
        // GET: Cases/Case
        [HttpGet]
        public ActionResult List(int? id)
        {
            if (id == null)
                id = 1;
            CaseModel model = new CaseModel();
            try
            {
                model = new CaseModel(id.Value);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult GetWorkflowStatus(int workflowID)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var result = new CaseModel().WorkflowStatusGet(workflowID);
                bool isChild = true;
                if (result.Count == 0)
                {
                    result.Add(new SelectListItemViewModel
                    {
                        ID = -1,
                        Name = Resources.Global.NoDataMessage
                    });
                    isChild = false;
                }
                List<JsTreeViewModel> data = new JsTreeViewModel().GetJsonTreeView(result, "GetWorkflowStatusReason", isChild, workflowID, 0, 0, "status");

                jResult = Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpGet]
        public ActionResult GetWorkflowStatusReason(int statusid, int workflowID, bool isJsTree, int reasonID = -1)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var result = new CaseModel().WorkflowStatusReasonGet(workflowID, statusid, reasonID);

                if (isJsTree)
                {
                    var selectListViewModel = result.Select(a => new SelectListItemViewModel
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();

                    List<JsTreeViewModel> data = new JsTreeViewModel().GetJsonTreeView(selectListViewModel, "MasterCasesGet", true, workflowID, statusid, 0, "reason");

                    jResult = Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                    jResult = Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;

        }
        [HttpGet]
        public ActionResult MasterCasesGet(int reasonid, int statusid, int workflowID)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var result = new CaseModel().MasterCasesGet(reasonid, statusid, workflowID).Select(a => new SelectListItemViewModel
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();

                List<JsTreeViewModel> data = new JsTreeViewModel().GetJsonTreeView(result, "#", false, workflowID, statusid, reasonid, "cases");

                jResult = Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;

        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, CaseModel customSearch)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CaseList masterCases = new CaseModel().GetByPaging(customSearch, jqdtParams);

                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = masterCases.TotalRecord, recordsTotal = masterCases.TotalRecord, data = masterCases.CaseModels });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;


        }
        [HttpGet]
        public ActionResult Add(int? id)
        {
            if (id == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            CaseModel model = new CaseModel();
            try
            {
                model = new CaseModel().Add(id.Value);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AccountServiceGet(JQDTParams jqdtParams, CaseModel customSearch)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CaseModel caseModel = customSearch.AccountServiceGet(customSearch);

                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = caseModel.AccountServiceViewModels.Count, recordsTotal = caseModel.AccountServiceViewModels.Count, data = caseModel.AccountServiceViewModels });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpPost]
        public ActionResult Add(CaseModel model)
        {
            string msg = string.Empty;
            bool status = false;
            try
            {
                status = model.Insert(model);
                if (status)
                {
                    msg = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult View(int? id, int? caseid)
        {
            if (id == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            else if (caseid == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            CaseModel model = new CaseModel();
            try
            {
                model = model.View(caseid.Value);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult StatusActivityGet(int workflowID, int statusID)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var result = new CaseWorkflowStatus().StatusActivityGet(workflowID, statusID);

                jResult = Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpGet]
        public ActionResult WorkflowFormGet(int id)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var result = new CaseWorkflowForm().WorkflowFormGet(id);

                jResult = Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpGet]
        public ActionResult Edit(int? id, int? caseid)
        {
            if (id == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            else if (caseid == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            CaseModel model = new CaseModel();
            try
            {
                model = model.Edit(caseid.Value);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            // get the previous url and store it with view model
            ViewBag.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CaseModel model)
        {
            string msg = string.Empty;
            bool status = false;
            try
            {
                status = model.Update(model);
                if (status)
                {
                    msg = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion "Cases"

        #region "Case Comment Form"
        [HttpPost]
        public ActionResult CasesCommentUpdate(CaseModel model)
        {
            JsonResult jResult = new JsonResult();
            string msg = string.Empty;
            bool status = false;
            try
            {
                status = model.CasesCommentUpdate(model);
                if (status)
                    msg = Resources.Global.SavedSuccessfullyMessage;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            jResult = Json(new { Status = status, Message = msg }, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        [HttpPost]
        public ActionResult CasesStatusUpdate(CaseModel model)
        {
            JsonResult jResult = new JsonResult();
            string msg = string.Empty;
            bool result = false;
            try
            {
                result = model.CasesStatusUpdate(model) > 0;
                msg = Resources.Global.SavedSuccessfullyMessage;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            jResult = Json(new { Status = result, Message = msg }, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        [HttpGet]
        public ActionResult DocumentWorkflowHistoryLogGet(string caseIDs)
        {
            JsonResult jResult = new JsonResult();
            string msg = string.Empty;
            bool status = true;
            try
            {
                var data = new CaseModel().DocumentWorkflowHistoryLogGet(caseIDs);

                var result = data.GroupBy(a => new { a.CaseID, a.Name }).
                            Select(a => new
                            {
                                CaseID = a.Key.CaseID,
                                Name = a.Key.Name,
                                Logs = a.ToList()
                            }).ToList();

                jResult = Json(new { status = status, Message = msg, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                status = false;
                jResult = Json(new { status = status, Message = msg, data = "" }, JsonRequestBehavior.AllowGet);
            }

            return jResult;
        }
        public ActionResult UploadFileTemporary()
        {
            var model = new CaseModel().UploadFileTemporary(Request.Files[0]);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveTemporaryFile(string fileName)
        {
            Guid id = new Guid();
            ResponceMessage responseMessage = new ResponceMessage();
            if (!string.IsNullOrWhiteSpace(Convert.ToString(Session["UploadCommentFiles"])))
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var model = new CaseModel().RemovedFileTemporary(fileName);
                    if (!string.IsNullOrWhiteSpace(model.FileName))
                    {
                        responseMessage.Status = true;
                        id = model.ID;
                    }
                }
                else
                {
                    Session.Remove("UploadCommentFiles");
                    responseMessage.Status = true;
                }
            }
            else responseMessage.Status = true;
            return Json(new { responseMessage = responseMessage, id = id }, JsonRequestBehavior.AllowGet);
        }
        #endregion  "Case Comment Form"

        #region "Case History Tabe"
        [HttpPost]
        public ActionResult HistoryList(JQDTParams jqdtParams, CaseMeetingNotes customSearch)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CaseMeetingNotesList meetingNoteList = new CaseMeetingNotesList().GetByPaging(jqdtParams, customSearch);

                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = meetingNoteList.TotalRecords, recordsTotal = meetingNoteList.TotalRecords, data = meetingNoteList.CaseMeetingList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpPost]
        public ActionResult AddHistoryList(CaseMeetingNotes model)
        {
            string msg = string.Empty;
            bool status = false;
            JsonResult jResult = new JsonResult();
            try
            {
                status = model.Insert(model) > 0;
                msg = status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            jResult = Json(new { Status = status, Message = msg }, JsonRequestBehavior.AllowGet);

            return jResult;
        }
        [HttpGet]
        public ActionResult EditHistory(int id, int caseID)
        {
            CaseMeetingNotes CaseMeetingNotes = new CaseMeetingNotes().Get(new CaseMeetingNotes
            {
                ID = id,
                CaseID = caseID
            });
            return Json(CaseMeetingNotes, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditHistory(CaseMeetingNotes model)
        {
            ResponceMessage responseMessage = new ResponceMessage();
            try
            {
                responseMessage.Status = model.Update(model) > 0;
                responseMessage.Message = responseMessage.Status ? Resources.Global.SavedSuccessfullyMessage : string.Empty;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
            }
            return Json(responseMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region  "Case Print Form"
        [HttpGet]
        public ActionResult PrintLetter(string caseids, int? statuidtarget, int? statuidsource, int? workflowid)
        {
            if (string.IsNullOrWhiteSpace(caseids) || statuidtarget == null || statuidsource == null || workflowid == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            PrintLetterModel PrintLetterModel = new PrintLetterModel().PrintLetter(caseids, statuidtarget.Value, workflowid.Value);

            return View(PrintLetterModel);
        }

        [HttpPost]
        public ActionResult PrintLetter(PrintCenter model)
        {
            ResponceMessage responceMessage = new ResponceMessage();
            JsonResult jResult = new JsonResult();
            try
            {
                responceMessage.Status = model.PrintLetter(model).ID > 0;
                if (responceMessage.Status)
                {
                    responceMessage.Message = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            jResult = Json(responceMessage, JsonRequestBehavior.AllowGet);
            return jResult;
        }

        [HttpGet]
        public ActionResult PrintLetterList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrintLetterList(JQDTParams jqdtParams, PrintCenter model)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                PrintCenterList printCenterList = new PrintCenterList().Get(model, jqdtParams);

                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = printCenterList.TotalRecord, recordsTotal = printCenterList.TotalRecord, data = printCenterList.PrintCenters });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;

        }

        [HttpGet]
        public ActionResult PrintLetterDownload(int? id, int? actionid)
        {
            JsonResult jResult = new JsonResult();
            ResponceMessage responceMessage = new ResponceMessage();
            try
            {
                if (id == null || actionid == null)
                    return RedirectToAction("NotFoundError", "Home", new { area = "" });

                Common.Actions actions = (Common.Actions)actionid.Value;
                responceMessage.Status = new PrintCenterLog().PrintCenterLogInsert(new PrintCenterLog
                {
                    FileID = id.Value,
                    Action = actions.ToString()
                }) > 0;
                if (responceMessage.Status)
                {
                    return RedirectToAction("DownloadFile", "File", new { area = "", id = id });
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            return jResult;
        }
        [HttpPost]
        public ActionResult PreviewLatter(PrintCenter model)
        {
            ResponceMessage responceMessage = new ResponceMessage();

            var data = new PrintCenter().PreviewLetter(model);

            responceMessage.Status = data != null;

            if (responceMessage.Status)
            {
                TempData["DownloadFile"] = data;
                responceMessage.Message = model.OutputFormat.ToString();
            }

            return Json(responceMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DownloadPreviewLatter(int id)
        {
            if (TempData["DownloadFile"] != null)
            {
                byte[] data = TempData["DownloadFile"] as byte[];

                return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, ("Sample." + (id == 1 ? "docx" : "pdf")));
            }
            return new EmptyResult();
        }
        #endregion

        #region "Case Attachments Tab"
        [HttpPost]
        public ActionResult CaseImagesList(JQDTParams jqdtParams, CaseModel customSearch)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                CaseImageList caseImageList = new CaseImage().CaseImageGetByPaging(customSearch, jqdtParams);

                jResult = Json(new
                {
                    draw = jqdtParams.Draw,
                    recordsFiltered = caseImageList.TotalRecord,
                    recordsTotal = caseImageList.TotalRecord,
                    data = caseImageList.CaseImages
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return jResult;
        }

        [HttpPost]
        public ActionResult AddAttachment(int id, string fileName)
        {
            ResponceMessage responceMessage = new ResponceMessage();
            try
            {
                responceMessage.Status = new CaseModel().AddAttachment(id, fileName);
                if (responceMessage.Status)
                {
                    responceMessage.Message = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            return Json(responceMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Case Contact Information"
        public ActionResult GetCountries()
        {
            var countryList = new Companies.Models.CompanyModel().GetCountries();
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStates(int id)
        {
            var stateList = new Companies.Models.CountryModel().GetStates(id);
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ContactInsert(ContactList model)
        {
            ResponceMessage responceMessage = new ResponceMessage();
            try
            {
                responceMessage.Status = new ContactModel().Insert(model) > 0;
                if (responceMessage.Status)
                {
                    responceMessage.Message = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            return Json(responceMessage, JsonRequestBehavior.AllowGet); ;
        }
        #endregion "Case Heir Information"

        #region "Dashboard"
        public ActionResult Dashboard()
        {
            CaseModel caseModel = new CaseModel()
            {
                CaseModels = new DashboardModel().CasesGetBalanceByStatus(),
                CaseCounts = new DashboardModel().CasesGetCountByStatus()
            };
            return View(caseModel);
        }
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
        #endregion "Dashboard"
    }
}
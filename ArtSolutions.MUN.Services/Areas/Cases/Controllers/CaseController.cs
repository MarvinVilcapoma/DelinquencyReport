using ArtSolutions.MUN.Core.CaseModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;


namespace ArtSolutions.MUN.Services.Areas.Cases.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseController : ApiController
    {
        const string _featureID = "00000000-0009-0007-0001-000000000000";

        #region "Cases"
        [Route("DocumentWorkflowGet")]
        [HttpGet]
        public IHttpActionResult DocumentWorkflowGet(int? documentTypeID, int? workFlowID, int? workFlowVersion, bool? isActive = true, bool? isDeleted = false)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().DocumentWorkflowGet(documentTypeID, workFlowID, workFlowVersion, companyId, language, isActive, isDeleted);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.DocumentWorkflowGet", language, ex));
            }
        }

        [Route("WorkflowStatusGet")]
        [HttpGet]
        public IHttpActionResult WorkflowStatusGet(int workflowID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var workFlowStatus = new Case().WorkflowStatusGet(new CaseModel
                {
                    WorkflowID = workflowID,
                    CompanyID = companyID,
                    Language = language
                });

                return Ok(workFlowStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.WorkflowStatusGet", language, ex));
            }
        }

        [Route("WorkflowReasonGet")]
        [HttpGet]
        public IHttpActionResult WorkflowReasonGet(int workflowID, int statusID, int reasonID = -1)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var workFlowReason = new Case().WorkflowReasonGet(new CaseModel
                {
                    WorkflowID = workflowID,
                    Language = language,
                    StatusID = statusID,
                    ReasonID = reasonID
                });

                return Ok(workFlowReason);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.WorkflowReasonGet", language, ex));
            }
        }

        [Route("MasterCasesGet")]
        [HttpGet]
        public IHttpActionResult MasterCasesGet(int reasonID, int statusId, int workflowid, string caseIds)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var cases = new Case().MasterCasesGet(new CaseModel
                {
                    ReasonID = reasonID,
                    StatusID = statusId,
                    WorkflowID = workflowid,
                    CompanyID = companyID,
                    Language = language,
                    CaseIDs = caseIds
                });

                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.MasterCasesGet", language, ex));
            }
        }

        [Route("GetByPaging")]
        [HttpGet]
        public IHttpActionResult Get(string filterText, int caseID, string keyCode, string bussinessName, int priorityID, int weight, Guid assignedTo,
                int statusID, int reasonID, int currentIndex, int pageSize, string sortColumn, string sort, int workflowID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                CaseList masterCases = new Case().GetByPaging(new CaseModel
                {
                    CompanyID = companyID,
                    FilterText = filterText,
                    CaseID = caseID,
                    KeyCode = keyCode,
                    BussinessName = bussinessName,
                    PriorityID = priorityID,
                    Weight = weight,
                    AsignedTo = assignedTo,
                    StatusID = statusID == 0 ? -1 : statusID,
                    ReasonID = reasonID,
                    CurrentIndex = currentIndex,
                    PageSize = pageSize,
                    Sort = sort,
                    SortColumn = sortColumn,
                    Language = language,
                    WorkflowID = workflowID
                });

                return Ok(masterCases);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.GetByPaging", language, ex));
            }
        }

        [Route("CasePriorityGet")]
        [HttpGet]
        public IHttpActionResult CasePriorityGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().CasePriorityGet(new CaseModel
                {
                    Language = language
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CasePriorityGet", language, ex));
            }
        }

        [Route("CaseTeamGet")]
        [HttpGet]
        public IHttpActionResult CaseTeamGet(Guid? id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().CaseTeamGet(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CaseTeamGet", language, ex));
            }
        }

        [Route("CompanyGet")]
        [HttpGet]
        public IHttpActionResult CompanyGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Core.CompanyModels.Company().Get();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CompanyModels.Company.Get", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(CaseModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.New);

                var result = new Case().Insert(model);

                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.New.ToString(), result.ToString(), model.Name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.Insert", language, ex));
            }
        }
        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().Get(id, language, companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.Get", language, ex));
            }
        }

        [Route("StatusActivityGet")]
        [HttpGet]
        public IHttpActionResult StatusActivityGet(int workflowID, int statusID, byte type = 0)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().StatusActivityGet(new CaseModel
                {
                    CompanyID = companyId,
                    Language = language,
                    WorkflowID = workflowID,
                    StatusID = statusID
                }, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.StatusActivityGet", language, ex));
            }
        }

        [Route("WorkflowFormGet")]
        [HttpGet]
        public IHttpActionResult WorkflowFormGet(int formID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new Case().WorkflowFormGet(new CaseModel
                {
                    Language = language,
                    FormID = formID
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.WorkflowFormGet", language, ex));
            }
        }
        [HttpPost]
        [Route("CasesCommentUpdate")]
        public IHttpActionResult CasesCommentUpdate(CaseModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                if (model.WorkflowHistoryLogs != null && model.WorkflowHistoryLogs.Any())
                {
                    model.WorkflowHistoryLogs.ToList().ForEach(a => a.CompanyID = companyId);
                }

                var result = new Case().CasesCommentUpdate(new CaseModel
                {
                    ModifiedDate = model.ModifiedDate,
                    ModifiedUserID = model.ModifiedUserID,
                    WorkflowHistoryLogs = model.WorkflowHistoryLogs
                });

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), string.Join(",", model.WorkflowHistoryLogs.Select(a => a.CaseID)), "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CasesCommentUpdate", language, ex));
            }
        }
        [HttpPost]
        [Route("CasesStatusUpdate")]
        public IHttpActionResult CasesStatusUpdate(CaseModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                if (model.WorkflowHistoryLogs != null && model.WorkflowHistoryLogs.Any())
                {
                    model.WorkflowHistoryLogs.ToList().ForEach(a => a.CompanyID = companyId);
                }
                var result = new Case().CasesStatusUpdate(new CaseModel
                {
                    ModifiedDate = model.ModifiedDate,
                    ModifiedUserID = model.ModifiedUserID,
                    WorkflowHistoryLogs = model.WorkflowHistoryLogs
                });

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), string.Join(",", model.WorkflowHistoryLogs.Select(a => a.CaseID)), "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CasesStatusUpdate", language, ex));
            }
        }
        [HttpGet]
        [Route("DocumentWorkflowHistoryLogGet")]
        public IHttpActionResult DocumentWorkflowHistoryLogGet(string caseids)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var result = new Case().DocumentWorkflowHistoryLogGet(new CaseModel
                {
                    CaseIDs = caseids,
                    Language = language
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.DocumentWorkflowHistoryLogGet", language, ex));
            }
        }

        //Update legal case
        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(CaseModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyID, Guid.Parse(_featureID), Actions.Edit);

                var result = new Case().Update(model);

                Common.InsertActivity(token, model.CompanyID, _featureID.ToString(), Actions.Edit.ToString(), model.CaseID.ToString(), model.Name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.Update", language, ex));
            }
        }
        #endregion

        #region "Case Meetings"
        [HttpGet]
        [Route("CaseMeetingNotesGetByPaging")]
        public IHttpActionResult CaseMeetingNotesGet(int caseID, int currentIndex, int pageSize, string sortColumn, string sortDirection)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                CaseMeetingNotesList caseMeetingNotesList = new CaseMeetingNotes().GetWithPaging(new CaseMeetingNotes
                {
                    CaseID = caseID,
                    CurrentIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    Language = language,
                    CompanyId = companyId
                });

                return Ok(caseMeetingNotesList);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.CaseMeetingNotes.GetWithPaging", language, ex));
            }
        }
        [HttpGet]
        [Route("CaseMeetingNotesGet")]
        public IHttpActionResult CaseMeetingNotesGet(int id, int caseID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                CaseMeetingNotesList caseMeetingNotesList = new CaseMeetingNotes().Get(new CaseMeetingNotes
                {
                    CaseID = caseID,
                    Language = language,
                    CompanyId = companyId,
                    ID = id
                });

                return Ok(caseMeetingNotesList);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.CaseMeetingNotes.Get", language, ex));
            }
        }
        [HttpGet]
        [Route("MeetingTypeGet")]
        public IHttpActionResult MeetingTypeGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var model = new CaseMeetingNotes().MeetingTypeGet(new CaseMeetingNotes
                {
                    Language = language
                });

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.CaseMeetingNotes.MeetingTypeGet", language, ex));
            }
        }

        [HttpPost]
        [Route("MeetingNotesInsert")]
        public IHttpActionResult MeetingNotesInsert(CaseMeetingNotes model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var returnVal = new CaseMeetingNotes().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.CaseID.ToString(), model.MeetingType.ToString());

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.CaseMeetingNotes.Insert", language, ex));
            }
        }
        [HttpPost]

        [Route("MeetingNotesUpdate")]
        public IHttpActionResult MeetingNotesUpdate(CaseMeetingNotes model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                model.CompanyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, model.CompanyId, Guid.Parse(_featureID), Actions.Edit);

                var returnVal = new CaseMeetingNotes().Update(model);

                Common.InsertActivity(token, model.CompanyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), model.MeetingType.ToString());

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.CaseMeetingNotes.Update", language, ex));
            }
        }

        #endregion

        #region "Company Keycode"
        [HttpGet]
        [Route("keycodeget")]
        public IHttpActionResult KeyCodeGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new Case().KeyCodeGet(new CaseModel
                {
                    CompanyID = companyId
                });

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.KeyCodeGet", language, ex));
            }
        }
        #endregion

        #region "Views"
        [HttpGet]
        public IHttpActionResult AccountViewGet()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new PrintCenter().AccountViewGet(companyId);

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.AccountViewGet", language, ex));
            }
        }
        #endregion

        #region "Case Images"
        [HttpPost]
        [Route("CaseImagesInsert")]
        public IHttpActionResult CaseImagesInsert(CaseModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var returnVal = new Case().CaseImagesInsert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), "", "");

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CaseImagesInsert", language, ex));
            }
        }
        [HttpGet]
        [Route("caseimagesget")]
        public IHttpActionResult CaseImagesGet(int caseid, int currentIndex, int pageSize, string sortColumn, string sortDirection)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new Case().CaseImagesGet(new CaseModel
                {
                    CaseID = caseid,
                    CurrentIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sort = sortDirection,
                    CompanyID = companyId
                });
                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CaseImagesGet", language, ex));
            }
        }
        #endregion

        #region "Print Center"
        [HttpPost]
        public IHttpActionResult PrintCenterInsert(PrintCenter model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var returnVal = new PrintCenter().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.KeyCode.ToString(), model.Destinatary.Name.ToString());

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.Insert", language, ex));
            }
        }
        [HttpPost]
        [Route("PrintCenterFileIDUpdate")]
        public IHttpActionResult PrintCenterFileIDUpdate(PrintCenter model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.Edit);

                var returnVal = new PrintCenter().PrintCenterFileIDUpdate(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), model.FileID.ToString());

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.PrintCenterFileIDUpdate", language, ex));
            }
        }

        [HttpGet]
        [Route("PrintCenterGet")]
        public IHttpActionResult PrintCenterGet(string filterText, int currentIndex, int pageSize, string sortColumn, string sortDirection)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new PrintCenter().GetByPaging(new PrintCenter
                {
                    FilterText = filterText,
                    CurrentIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    Language = language,
                    CompanyID = companyId
                });

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.GetByPaging", language, ex));
            }
        }

        [HttpPost]
        [Route("PrintCenterLogInsert")]
        public IHttpActionResult PrintCenterLogInsert(PrintCenterLog model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                var returnVal = new PrintCenter().PrintCenterLogInsert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.New.ToString(), model.UserID.ToString(), model.Action);

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.PrintCenterLogInsert", language, ex));
            }
        }

        #endregion "Print Center"

        #region "Case Contact Information"
        [HttpPost]
        [Route("ContactInsert")]
        public IHttpActionResult ContactInsert(ContactList model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.New);

                model.ContactViewModel.ForEach(a =>
                {
                    a.CompanyID = companyId;
                });

                var returnVal = new ContactModel().Insert(model);

                Common.InsertActivity(token, companyId, _featureID.ToString(), Actions.Edit.ToString(), model.ContactViewModel.FirstOrDefault().CaseID.ToString(), model.ContactViewModel.FirstOrDefault().FirstName.ToString());

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.ContactModel.Insert", language, ex));
            }
        }
        #endregion

        #region "Dashboards"
        [HttpGet]
        [Route("CasesGetBalanceByStatus")]
        public IHttpActionResult CasesGetBalanceByStatus()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new Case().CasesGetBalanceByStatus(new CaseModel
                {
                    Language = language,
                    CompanyID = companyId
                });

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CasesGetBalanceByStatus", language, ex));
            }
        }
        [HttpGet]
        [Route("CasesGetCountByStatus")]
        public IHttpActionResult CasesGetCountByStatus()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new Case().CasesGetCountByStatus(new CaseModel
                {
                    Language = language,
                    CompanyID = companyId
                });

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.Case.CasesGetCountByStatus", language, ex));
            }
        }
        #endregion
    }
}

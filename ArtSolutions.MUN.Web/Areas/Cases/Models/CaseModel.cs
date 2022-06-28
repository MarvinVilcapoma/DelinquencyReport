using System;
using System.Collections.Generic;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Linq;
using System.IO;
using System.Web;
using ArtSolutions.MUN.Web.Areas.PrintTemplates.Models;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseModel : GridViewModel
    {
        #region "Properties"
        public int ID { get; set; }
        public long? SrNO { get; set; }
        public int CaseID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "RequiredValidationMsg")]
        public string Name { get; set; }
        public string Refrence { get; set; }
        public string Description { get; set; }
        public string KeyCode { get; set; }
        public int CompanyID { get; set; }
        public string BusinessName { get; set; }
        public int PriorityID { get; set; }
        public string PriorityName { get; set; }
        public string ServiceType { get; set; }
        public int Weight { get; set; }
        public Guid AssignToID { get; set; } = Guid.Empty;
        public string AssignedTo { get; set; }
        public decimal? Balance { get; set; }
        public string BalanceWithCurrency { get; set; }
        public int ReasonID { get; set; }
        public string MuicipalityName { get; set; }
        public int StatusID { get; set; }
        public int WorkflowID { get; set; }
        public int AccountID { get; set; }
        public string Note { get; set; }
        public List<Accounts.Models.AccountServiceModel> AccountServiceViewModels { get; set; }
        public List<Accounts.Models.AccountModel> AccountModels { get; set; }
        public List<SelectListItemViewModel> WorkflowReason { get; set; }
        public List<SelectListItemViewModel> DocumentWorkflow { get; set; }
        public List<SelectListItemViewModel> CompanyList { get; set; }
        public List<SelectListItemViewModel> CasePriorityList { get; set; }
        public List<SelectListItemViewModel> WeightList { get; set; }
        public List<CaseTeam> CaseTeamList { get; set; }
        public List<CaseTeam> OwnerTeamList { get; set; }
        public Guid OwnerID { get; set; } = Guid.Empty;
        public string Owner { get; set; }
        public Guid CreatedUserID { get; set; } = Guid.Empty;
        public DateTime CreatedDate { get; set; }
        public int DocumentType { get; set; } = 24;
        public string AccountServices { get; set; }
        public string AccountName { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SequenceID { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? SortID { get; set; }
        public int? ImageID { get; set; }
        public string WorkFlowName { get; set; }
        public List<WorkflowHistoryLog> WorkflowHistoryLogs { get; set; }
        public string Comments { get; set; }
        public string StatusName { get; set; }
        public string CreatedDateInString { get; set; }
        public Guid ModifiedUserID { get; set; } = Guid.Empty;
        public List<CaseModel> CaseModels { get; set; } = new List<CaseModel>();
        public List<CaseWorkflowStatus> StatusActions { get; set; }
        public CaseMeetingNotes CaseMeetingNotes { get; set; }
        public List<CaseImage> CaseImages { get; set; } = new List<CaseImage>();
        public List<CaseModel> CaseCounts { get; set; } = new List<CaseModel>();
        #endregion "Properties"

        #region "Constructor"
        public CaseModel()
        {
            WorkflowReason = new List<SelectListItemViewModel>();
            DocumentWorkflow = new List<SelectListItemViewModel>();
            CompanyList = new List<SelectListItemViewModel>();
            CasePriorityList = new List<SelectListItemViewModel>();
            WeightList = new List<SelectListItemViewModel>();
            CaseTeamList = new List<CaseTeam>();
            AccountServiceViewModels = new List<Accounts.Models.AccountServiceModel>();
            AccountModels = new List<Accounts.Models.AccountModel>();
            WorkflowHistoryLogs = new List<WorkflowHistoryLog>();
            CaseMeetingNotes = new CaseMeetingNotes();
        }
        public CaseModel(int _workflowID)
        {
            DocumentWorkflow = DocumentWorkFlowGet();
            WeightList = WeightListGet();
            WeightList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            CasePriorityList = CasePriorityGet();
            CasePriorityList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            CaseTeamList = new CaseTeam().CaseTeamGet(Guid.Empty);
            CompanyList = CompanyGet();
            CompanyID = UserSession.Current.CompanyID;
            WorkflowID = _workflowID;
        }
        #endregion "Constructor"

        #region "Public Methods"
        public CaseModel Add(int workflowID)
        {
            CaseModel model = new CaseModel()
            {
                DocumentWorkflow = DocumentWorkFlowGet(),
                CasePriorityList = CasePriorityGet(),
                OwnerTeamList = new CaseTeam().CaseTeamGet(Guid.Empty),
                CaseTeamList = new CaseTeam().CaseTeamGet(Guid.Empty),
                AccountModels = new Accounts.Models.AccountModel().Get(null, 0, null, null, null, null, true),
                WorkflowID = workflowID
            };
            model.DocumentWorkflow.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.CasePriorityList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.AccountModels.Insert(0, new Accounts.Models.AccountModel { AccountID = -1, DisplayName = Resources.Global.DropDownSelectMessage });
            return model;
        }
        public List<SelectListItemViewModel> DocumentWorkFlowGet(int id = 0)
        {
            List<NameValuePair> parms = new List<NameValuePair>
            {
                 new NameValuePair { Name = "documentTypeID", Value = id },
                        new NameValuePair { Name = "workFlowID", Value = 0 },
                        new NameValuePair { Name = "workFlowVersion", Value = 0 },
                        new NameValuePair { Name = "isActive", Value = 1 },
                        new NameValuePair { Name = "isDeleted", Value = 0 }
            };
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/case/DocumentWorkflowGet", "GET", parms);
        }
        public List<SelectListItemViewModel> WorkflowStatusGet(int workflowId, int id = 0)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "workflowID", Value = workflowId },
                new NameValuePair { Name = "id", Value = id }
            };
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/case/WorkflowStatusGet", "GET", lstParameter);
        }
        public List<CaseWorkflowStatusReason> WorkflowStatusReasonGet(int workflowId, int statusId, int reasonId = -1)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "workflowID", Value = workflowId },
                new NameValuePair { Name = "statusID", Value = statusId },
                new NameValuePair { Name = "reasonID", Value = reasonId }
            };
            return new RestSharpHandler().RestRequest<List<CaseWorkflowStatusReason>>(APISelector.Municipality, true, "api/case/WorkflowReasonGet", "GET", lstParameter);
        }
        public List<CaseModel> MasterCasesGet(int reasonID, int statusID, int workflowID, string caseIds = null)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "reasonID", Value = reasonID },
                new NameValuePair { Name = "statusID", Value = statusID },
                new NameValuePair { Name = "workflowid", Value = workflowID },
                new NameValuePair { Name = "caseIds", Value = caseIds }
            };
            return new RestSharpHandler().RestRequest<List<CaseModel>>(APISelector.Municipality, true, "api/case/MasterCasesGet", "GET", lstParameter);
        }
        public List<SelectListItemViewModel> WeightListGet()
        {
            List<SelectListItemViewModel> model = new List<SelectListItemViewModel>();
            for (int i = 1; i < 11; i++)
            {
                model.Add(new SelectListItemViewModel
                {
                    ID = i,
                    Name = i.ToString()
                });
            }
            return model;
        }
        public List<SelectListItemViewModel> CasePriorityGet()
        {
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/case/CasePriorityGet", "GET", null);
        }

        public List<SelectListItemViewModel> CompanyGet()
        {
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/case/CompanyGet", "GET", null);
        }
        public CaseList GetByPaging(CaseModel model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "filterText", Value = model.FilterText },
                new NameValuePair { Name = "caseID", Value = model.CaseID },
                new NameValuePair { Name = "keyCode", Value = model.KeyCode },
                new NameValuePair { Name = "bussinessName", Value = model.BusinessName },
                new NameValuePair { Name = "priorityID", Value = model.PriorityID },
                new NameValuePair { Name = "weight", Value = model.Weight },
                new NameValuePair { Name = "assignedTo", Value = model.AssignToID },
                new NameValuePair { Name = "statusID", Value = model.StatusID },
                new NameValuePair { Name = "reasonID", Value = model.ReasonID },
                new NameValuePair { Name = "currentIndex", Value = model.CurrentIndex },
                new NameValuePair { Name = "pageSize", Value = model.PageSize },
                new NameValuePair { Name = "sortColumn", Value = model.SortColumnName },
                new NameValuePair { Name = "sort", Value = model.Sort },
                new NameValuePair { Name = "workflowID", Value = model.WorkflowID }
            };
            return new RestSharpHandler().RestRequest<CaseList>(APISelector.Municipality, true, "api/case/GetByPaging", "GET", lstParameter);
        }
        public CaseList GetByPaging(CaseModel customSearch, JQDTParams jqdtParams)
        {
            CaseList masterCases = new CaseList();
            customSearch.SortColumnName = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;

            customSearch.CurrentIndex = (jqdtParams.Start + 1);

            customSearch.PageSize = jqdtParams.Length;

            customSearch.Sort = jqdtParams.Order[0].Dir.ToString();

            masterCases = GetByPaging(customSearch);

            if (masterCases.CaseModels.Any())
            {
                masterCases.CaseModels.ToList().ForEach(a => a.BalanceWithCurrency = a.Balance.Value.ToString("C"));
            }
            return masterCases;
        }
        public bool Insert(CaseModel model)
        {
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            int caseID = 0;
            List<object> lstObjParameter = new List<object>
            {
                model
            };
            caseID = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/Insert", "POST", null, lstObjParameter);
            if (caseID > 0)
            {
                Accounts.Models.AccountServiceModel accountServiceModel = new Accounts.Models.AccountServiceModel();
                model.AccountServiceViewModels = accountServiceModel.Get(0, null, model.AccountID, null, null);
                if (model.AccountServiceViewModels != null && model.AccountServiceViewModels.Any())
                {
                    if (!string.IsNullOrWhiteSpace(model.AccountServices))
                    {
                        model.AccountServiceViewModels = model.AccountServiceViewModels.Where(a => model.AccountServices.Split(",".ToCharArray()).Any(b => b == a.ID.ToString())).ToList();
                    }
                }
                MailBoxModel mailBoxModel = new MailBoxModel()
                {
                    EventId = (int)Common.NOTEvent.MuniTax_New_Legal_Case
                };
                List<NOTElement> elements = new List<NOTElement>();
                elements.AddRange(new NOTElement[] {
                        new NOTElement() { Code = "[CaseID]", Value = caseID.ToString() },
                        new NOTElement() { Code = "[CaseName]", Value = model.Name },
                        new NOTElement() { Code = "[CaseReference]", Value = model.Refrence },
                        new NOTElement() { Code = "[CasePriority]", Value = model.PriorityName },
                        new NOTElement() { Code = "[AccountName]", Value = model.AccountName },
                        new NOTElement() { Code = "[AccountServices]", Value = model.GetAccountSerivceForMail(model.AccountServiceViewModels) }
                    });

                var owner = new CaseTeam().CaseTeamGet(model.OwnerID);
                mailBoxModel.Elements = elements;
                string url = "api/notification/SendEmail";

                if (owner != null && owner.Any())
                {
                    mailBoxModel.To = owner.FirstOrDefault().Email;
                    mailBoxModel.SendMail(mailBoxModel, url);
                }

                var assignTo = new CaseTeam().CaseTeamGet(model.AssignToID);
                if (assignTo != null && assignTo.Any())
                {
                    mailBoxModel.To = assignTo.FirstOrDefault().Email;
                    mailBoxModel.SendMail(mailBoxModel, url);
                }
            }
            return caseID > 0;
        }
        public string GetAccountSerivceForMail(List<Accounts.Models.AccountServiceModel> accountServiceModel)
        {
            string accountServices = "";
            accountServices += "<table>";
            accountServices += "<tr>";
            accountServices += "<th style=\"width: 10%\">ID</th>";
            accountServices += "<th style=\"text-align:left;width:40%;\">Service</th>";
            accountServices += "<th style=\"text-align:left;width:10%;\">Year</th>";
            accountServices += "<th style=\"width:10%\">License Type</th>";
            accountServices += "<th style=\"width:40%;text-align:right;\">Balance</th>";
            accountServices += "</tr>";
            foreach (var item in accountServiceModel)
            {
                accountServices += "<tr>";
                accountServices += "<td>" + item.ID + "</td>";
                accountServices += "<td style=\"text-align:left;\">" + item.ServiceName + "</td>";
                accountServices += "<td style=\"text-align:left;\">" + item.Year + "</td>";
                accountServices += "<td>" + item.LicenseType + "</td>";
                accountServices += "<td style=\"width:40%;text-align:right;\">" + item.AnnualIncome.ToString("C") + "</td>";
                accountServices += "</tr>";
            }
            accountServices += "</table>";
            return accountServices;
        }
        public List<CaseModel> Get(int id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "id", Value = id }
            };
            return new RestSharpHandler().RestRequest<List<CaseModel>>(APISelector.Municipality, true, "api/case/Get", "GET", lstParameter);
        }

        public bool CasesCommentUpdate(CaseModel model)
        {
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<string> fileNames = new List<string>();
            if (model.WorkflowHistoryLogs != null && model.WorkflowHistoryLogs.Any())
            {
                if (model.CaseImages != null && model.CaseImages.Any())
                {
                    // List<string> fileNames = model.CaseImages.Select(a => new { a.FileName }).ToList();
                    foreach (var item in model.CaseImages)
                    {
                        fileNames.Add(item.FileName);
                    }

                    string comment = string.Format(Resources.Case.CaseExtraComment, string.Join(",", fileNames));

                    model.WorkflowHistoryLogs.ForEach(a =>
                    {
                        a.Comment = a.Comment + "  " + comment;
                    });
                }
            }

            List<object> lstObjParameter = new List<object>
            {
                model
            };
            int result = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/CasesCommentUpdate", "POST", null, lstObjParameter);
            if (result > 0)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Current.Session["UploadCommentFiles"])) && model.CaseImages != null && model.CaseImages.Any())
                {
                    var listCaseImages = new List<CaseImage>();
                    foreach (WorkflowHistoryLog workflowHistoryLog in model.WorkflowHistoryLogs)
                    {
                        List<UploadFiles> uploadFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UploadFiles>>(Convert.ToString(HttpContext.Current.Session["UploadCommentFiles"]));
                        if (uploadFiles != null && uploadFiles.Any())
                        {
                            if (fileNames.Any())
                            {
                                uploadFiles = uploadFiles.Where(a => model.CaseImages.Any(b => b.OldFileName.ToLower().Equals(a.FileNameWithExtention.ToLower()))).ToList();
                                foreach (var item in uploadFiles)
                                {
                                    FileModel fileModel = new FileModel
                                    {
                                        Data = item.Data,
                                        ContentType = item.ContentType,
                                        FileName = model.CaseImages.Where(a => a.OldFileName.Trim().ToLower().Equals(item.FileNameWithExtention.Trim().ToLower())).FirstOrDefault().FileName,
                                        FileExtension = item.Extention,
                                        CreatedUserID = UserSession.Current.Id,
                                        CreatedDate = Common.CurrentDateTime,
                                        ModifiedUserID = UserSession.Current.Id,
                                        ModifiedDate = Common.CurrentDateTime,
                                        IsListedInFolders = false,
                                        IsDeleted = false,
                                        IsActive = true,
                                        IsPublicImage = false,
                                        Length = item.ContentLength
                                    };
                                    int fileId = new PrintTemplate().FileInsert(fileModel).ID;
                                    listCaseImages.Add(new CaseImage
                                    {
                                        ImageID = fileId,
                                        CaseID = workflowHistoryLog.CaseID
                                    });
                                }
                            }
                        }
                    }
                    new CaseImage().CaseImagesInsert(new CaseModel
                    {
                        CaseImages = listCaseImages
                    });
                    HttpContext.Current.Session.Remove("UploadCommentFiles");
                }

                MailBoxModel mailBoxModel = new MailBoxModel()
                {
                    EventId = (int)Common.NOTEvent.MuniTax_Update_Status_with_Comments
                };
                List<NOTElement> elements = new List<NOTElement>();
                elements.AddRange(new NOTElement[] {
                        new NOTElement() { Code = "[CaseID]", Value = string.Join(",",model.WorkflowHistoryLogs.Select(a=>a.CaseID).ToList()) },
                        new NOTElement() { Code = "[CaseStatus]", Value = model.WorkflowHistoryLogs.FirstOrDefault().StatuIdTargetName },
                        new NOTElement() { Code = "[CaseReason]", Value = model.WorkflowHistoryLogs.FirstOrDefault().ReasonName},
                        new NOTElement() { Code = "[CaseComment]", Value = model.WorkflowHistoryLogs.FirstOrDefault().Comment }
                    });
                mailBoxModel.Elements = elements;
                var assignTo = new CaseTeam().CaseTeamGet(model.WorkflowHistoryLogs.FirstOrDefault().AssignToID);
                if (assignTo != null && assignTo.Any())
                {
                    mailBoxModel.To = assignTo.FirstOrDefault().Email;
                    mailBoxModel.SendMail(mailBoxModel, "api/notification/SendEmail");
                }
            }
            return result > 0;
        }
        public int CasesStatusUpdate(CaseModel model)
        {
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/CasesStatusUpdate", "POST", null, lstObjParameter);
        }
        public List<CaseModel> DocumentWorkflowHistoryLogGet(CaseModel model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "caseids", Value = string.Join(",", model.WorkflowHistoryLogs.Select(a => a.CaseID).ToList()) }
            };
            return new RestSharpHandler().RestRequest<List<CaseModel>>(APISelector.Municipality, true, "api/case/DocumentWorkflowHistoryLogGet", "GET", lstParameter);
        }
        public CaseModel View(int id)
        {
            CaseModel model = new CaseModel();
            List<CaseModel> caseModels = Get(id);
            if (caseModels != null && caseModels.Any())
            {
                model = caseModels.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(model.AccountServices))
                {
                    Accounts.Models.AccountServiceModel accountServiceModel = new Accounts.Models.AccountServiceModel();
                    model.AccountServiceViewModels = accountServiceModel.Get(0, null, model.AccountID, null, null);
                    if (model.AccountServiceViewModels != null && model.AccountServiceViewModels.Any())
                    {
                        model.AccountServiceViewModels = model.AccountServiceViewModels.Where(a => model.AccountServices.Split(",".ToCharArray()).Any(b => b == a.ID.ToString())).ToList();
                    }
                }
                model.CaseTeamList = new CaseTeam().CaseTeamGet(Guid.Empty);
                model.CaseModels = DocumentWorkflowHistoryLogGet(model.CaseID.ToString());
                model.StatusActions = new CaseWorkflowStatus().StatusActivityGet(model.WorkflowID, model.StatusID);
                model.CaseMeetingNotes.MeetingTypes = model.CaseMeetingNotes.MeetingTypeGet();
            }
            return model;
        }
        public List<CaseModel> DocumentWorkflowHistoryLogGet(string caseIDs)
        {
            CaseModel model = new CaseModel();
            List<CaseModel> caseModel = new List<CaseModel>();
            if (!string.IsNullOrWhiteSpace(caseIDs))
            {
                foreach (var item in caseIDs.Split(",".ToCharArray()))
                {
                    model.WorkflowHistoryLogs.Add(new WorkflowHistoryLog
                    {
                        CaseID = Convert.ToInt32(item)
                    });
                }
                caseModel = DocumentWorkflowHistoryLogGet(model);

                caseModel.ForEach(a => a.CreatedDateInString = a.CreatedDate.ToString("g"));
            }
            return caseModel;
        }
        public CaseModel AccountServiceGet(CaseModel customSearch)
        {
            CaseModel caseModel = new CaseModel();
            Accounts.Models.AccountServiceModel accountServiceModel = new Accounts.Models.AccountServiceModel();
            caseModel.AccountServiceViewModels = accountServiceModel.Get(0, null, customSearch.AccountID, null, null);
            if (caseModel.AccountServiceViewModels.Any())
            {
                caseModel.AccountServiceViewModels.ToList().ForEach(a => a.AnnualIncomeWithCurrencyCode = a.AnnualIncome.ToString("C"));
            }
            return caseModel;
        }
        public bool AddAttachment(int caseId, string fileName)
        {
            bool result = false;
            int fileID = new PrintTemplates.Models.PrintTemplate().SaveFile(fileName);
            if (fileID > 0)
            {
                var caseModel = new CaseModel();
                caseModel.CaseImages.Add(new CaseImage
                {
                    ImageID = fileID,
                    CaseID = caseId
                });
                result = new CaseImage().CaseImagesInsert(caseModel) > 0;
            }
            return result;
        }
        public UploadFiles UploadFileTemporary(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();

            file.InputStream.CopyTo(target);

            byte[] data = target.ToArray();

            UploadFiles model = new UploadFiles
            {
                Data = data,
                Extention = Path.GetExtension(file.FileName),
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                ContentLength = file.ContentLength,
                ContentType = file.ContentType,
                FileNameWithExtention = file.FileName
            };
            List<UploadFiles> uploadfiles = new List<UploadFiles>();
            if (HttpContext.Current.Session["UploadCommentFiles"] != null)
            {
                uploadfiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UploadFiles>>(Convert.ToString(HttpContext.Current.Session["UploadCommentFiles"]));
                uploadfiles.Add(model);
            }
            else
                uploadfiles.Add(model);

            HttpContext.Current.Session["UploadCommentFiles"] = Newtonsoft.Json.JsonConvert.SerializeObject(uploadfiles);

            return model;
        }
        public UploadFiles RemovedFileTemporary(string fileName)
        {
            UploadFiles model = new UploadFiles();
            if (!string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Current.Session["UploadCommentFiles"])))
            {
                List<UploadFiles> uploadFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UploadFiles>>(Convert.ToString(HttpContext.Current.Session["UploadCommentFiles"]));
                if (uploadFiles != null && uploadFiles.Any())
                {
                    model = uploadFiles.Where(a => a.FileNameWithExtention.ToLower().Equals(fileName.ToLower())).FirstOrDefault();
                    uploadFiles.Remove(model);
                    HttpContext.Current.Session["UploadCommentFiles"] = Newtonsoft.Json.JsonConvert.SerializeObject(uploadFiles);
                }
            }
            return model;
        }
        public CaseModel Edit(int id)
        {
            CaseModel model = new CaseModel();
            List<CaseModel> caseModels = Get(id);
            if (caseModels != null && caseModels.Any())
            {
                model = caseModels.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(model.AccountServices))
                {
                    Accounts.Models.AccountServiceModel accountServiceModel = new Accounts.Models.AccountServiceModel();
                    model.AccountServiceViewModels = accountServiceModel.Get(0, null, model.AccountID, null, null);
                    if (model.AccountServiceViewModels != null && model.AccountServiceViewModels.Any())
                    {
                        model.AccountServiceViewModels = model.AccountServiceViewModels.Where(a => model.AccountServices.Split(",".ToCharArray()).Any(b => b == a.ID.ToString())).ToList();
                        if (model.AccountServiceViewModels.Any())
                            model.AccountServiceViewModels.ToList().ForEach(a => a.AnnualIncomeWithCurrencyCode = a.AnnualIncome.ToString("C"));
                    }
                }
                model.CaseTeamList = new CaseTeam().CaseTeamGet(Guid.Empty);
                model.CasePriorityList = CasePriorityGet();
                model.OwnerTeamList = new CaseTeam().CaseTeamGet(Guid.Empty);
                model.CasePriorityList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            }
            return model;
        }
        public bool Update(CaseModel model)
        {
            model.ModifiedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            List<object> lstObjParameter = new List<object>
            {
                model
            };
            var result = new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/Update", "POST", null, lstObjParameter);
            if (result > 0)
            {
                model.CaseModels = Get(model.CaseID);
                if (model.CaseModels != null && model.CaseModels.Any())
                {
                    MailBoxModel mailBoxModel = new MailBoxModel()
                    {
                        EventId = (int)Common.NOTEvent.MuniTax_New_Legal_Case
                    };
                    List<NOTElement> elements = new List<NOTElement>();
                    elements.AddRange(new NOTElement[] {
                        new NOTElement() { Code = "[CaseID]", Value = model.CaseID.ToString() },
                        new NOTElement() { Code = "[CaseName]", Value = model.Name },
                        new NOTElement() { Code = "[CaseReference]", Value = model.Refrence },
                        new NOTElement() { Code = "[CasePriority]", Value = model.PriorityName },
                        new NOTElement() { Code = "[AccountName]", Value = model.AccountName },
                        new NOTElement() { Code = "[AccountServices]", Value = model.GetAccountSerivceForMail(model.AccountServiceViewModels) }
                    });
                    string url = "api/notification/SendEmail";
                    mailBoxModel.Elements = elements;
                    if (model.AssignToID != model.CaseModels.FirstOrDefault().AssignToID)
                    {
                        var assignTo = new CaseTeam().CaseTeamGet(model.AssignToID);
                        if (assignTo != null && assignTo.Any())
                        {
                            mailBoxModel.To = assignTo.FirstOrDefault().Email;
                            mailBoxModel.SendMail(mailBoxModel, url);
                        }
                    }
                    if (model.OwnerID != model.CaseModels.FirstOrDefault().OwnerID)
                    {
                        var owner = new CaseTeam().CaseTeamGet(model.OwnerID);
                        if (owner != null && owner.Any())
                        {
                            mailBoxModel.To = owner.FirstOrDefault().Email;
                            mailBoxModel.SendMail(mailBoxModel, url);
                        }
                    }
                }
            }
            return result > 0;
        }
        #endregion
    }
}
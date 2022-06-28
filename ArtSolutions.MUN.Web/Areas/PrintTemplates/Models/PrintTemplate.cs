using System;
using System.Collections.Generic;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.IO;
using System.Web;
using System.Linq;
using ArtSolutions.MUN.Web.Areas.Cases.Models;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.PrintTemplates.Models
{
    public class PrintTemplate
    {
        #region "Properties"
        public string FilterText { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int SrNo { get; set; }
        public int ID { get; set; } = 0;
        [Required(ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "RequiredValidationMsg")]
        public string TemplateName { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "RequiredValidationMsg")]
        public string Code { get; set; }
        public string WorkflowName { get; set; }
        public int WorkflowID { get; set; }
        public List<SelectListItemViewModel> DocumentWorkflow { get; set; }
        public int StatusID { get; set; } = 0;
        public int DataSourceID { get; set; }
        public List<SelectListItemViewModel> ReletedSteps { get; set; }
        public List<SelectListItemViewModel> DataSourceList { get; set; } = new List<SelectListItemViewModel>();
        public int[] StatusArray { get; set; } = new int[] { 2, 5, 6, 7 };
        public Guid CreatedUserID { get; set; } = UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        public string FileName { get; set; }
        public int FileID { get; set; }
        public FileModel FileModel { get; set; } = new FileModel();
        public Guid ModifiedUserID { get; set; } = UserSession.Current.Id;
        public DateTime ModifiedDate { get; set; } = Common.CurrentDateTime;
        public string Status { get; set; }
        public string DataSource { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public PrintTemplateList GetByPaging(JQDTParams jqdtParams, PrintTemplate model)
        {
            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;

            model.CurrentIndex = (jqdtParams.Start + 1);

            model.PageSize = jqdtParams.Length;

            model.SortDirection = jqdtParams.Order[0].Dir.ToString();

            return GetByPaging(model);
        }
        public PrintTemplate Add()
        {
            PrintTemplate model = new PrintTemplate()
            {
                DocumentWorkflow = new Cases.Models.CaseModel().DocumentWorkFlowGet()
            };
            model.DocumentWorkflow.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.ReletedSteps = RelatedSteps();
            model.ReletedSteps.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.DataSourceList = DataSourceGet(0);
            model.DataSourceList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            return model;
        }
        public PrintTemplate Save(PrintTemplate model)
        {
            model.FileID = SaveFile(model.FileName);
            model.ID = Insert(model);
            return model;
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

            HttpContext.Current.Session["UploadFiles"] = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            return model;
        }
        public List<PrintTemplate> Get(PrintTemplate model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "statusid", Value = model.StatusID },
                new NameValuePair { Name = "id", Value = model.ID },
                new NameValuePair { Name = "workflowId", Value = model.WorkflowID }
            };
            return new RestSharpHandler().RestRequest<List<PrintTemplate>>(APISelector.Municipality, true, "api/printtemplate/get", "GET", lstParameter);
        }
        public List<SelectListItemViewModel> DataSourceGet(int dataSourceId)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "dataSourceID", Value = dataSourceId }
            };
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/printtemplate/datasourceget", "GET", lstParameter);
        }
        public PrintTemplate Edit(int id)
        {
            PrintTemplate model = new PrintTemplate()
            {
                DocumentWorkflow = new Cases.Models.CaseModel().DocumentWorkFlowGet()
            };
            model.DocumentWorkflow.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.ReletedSteps = RelatedSteps();
            model.ReletedSteps.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });
            model.DataSourceList = DataSourceGet(0);
            model.DataSourceList.Insert(0, new SelectListItemViewModel { ID = -1, Name = Resources.Global.DropDownSelectMessage });

            List<PrintTemplate> printTemplate = Get(new PrintTemplate
            {
                StatusID = 0,
                ID = id,
                WorkflowID = 0
            });
            if (printTemplate != null && printTemplate.Any())
            {
                model.Code = printTemplate.FirstOrDefault().Code;
                model.DataSourceID = printTemplate.FirstOrDefault().DataSourceID;
                model.TemplateName = printTemplate.FirstOrDefault().TemplateName;
                model.Description = printTemplate.FirstOrDefault().Description;
                model.WorkflowID = printTemplate.FirstOrDefault().WorkflowID;
                model.StatusID = printTemplate.FirstOrDefault().StatusID;
                model.FileID = printTemplate.FirstOrDefault().FileID;
                if (model.FileID > 0)
                {
                    model.FileModel = new PrintCenter().GetFiles(model.FileID);
                }
            }
            return model;
        }
        public PrintTemplate Edit(PrintTemplate model)
        {
            int result = Update(model);
            if (result > 0)
            {
                model.FileModel = new PrintCenter().GetFiles(model.FileID);
                if (model.FileModel != null)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Current.Session["UploadFiles"])))
                    {

                        UploadFiles UploadFile = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadFiles>(Convert.ToString(HttpContext.Current.Session["UploadFiles"]));
                        FileModel fileModel = new FileModel
                        {
                            ID = model.FileID,
                            Data = UploadFile.Data,
                            ContentType = UploadFile.ContentType,
                            FileName = model.FileName,
                            FileExtension = UploadFile.Extention,
                            ModifiedUserID = UserSession.Current.Id,
                            ModifiedDate = Common.CurrentDateTime,
                            IsListedInFolders = model.FileModel.IsListedInFolders,
                            IsDeleted = model.FileModel.IsDeleted,
                            IsActive = model.FileModel.IsActive,
                            IsPublicImage = model.FileModel.IsPublicImage,
                            Length = UploadFile.ContentLength
                        };
                        FileUpdate(fileModel);
                        HttpContext.Current.Session.Remove("UploadFiles");
                    }
                    else
                    {
                        if (model.FileModel.FileName != model.FileName)
                        {
                            FileModel fileModel = new FileModel
                            {
                                ID = model.FileID,
                                Data = model.FileModel.Data,
                                ContentType = model.FileModel.ContentType,
                                FileName = model.FileName,
                                FileExtension = model.FileModel.FileExtension,
                                ModifiedUserID = UserSession.Current.Id,
                                ModifiedDate = Common.CurrentDateTime,
                                IsListedInFolders = model.FileModel.IsListedInFolders,
                                IsDeleted = model.FileModel.IsDeleted,
                                IsActive = model.FileModel.IsActive,
                                IsPublicImage = model.FileModel.IsPublicImage,
                                Length = model.FileModel.Length
                            };
                            FileUpdate(fileModel);
                        }
                    }
                }
            }
            return model;
        }
        public PrintTemplate View(int id)
        {
            PrintTemplate model = new PrintTemplate();
            List<PrintTemplate> printTemplate = Get(new PrintTemplate
            {
                StatusID = 0,
                ID = id,
                WorkflowID = 0
            });
            if (printTemplate != null && printTemplate.Any())
            {
                model.Code = printTemplate.FirstOrDefault().Code;
                model.DataSourceID = printTemplate.FirstOrDefault().DataSourceID;
                model.TemplateName = printTemplate.FirstOrDefault().TemplateName;
                model.Description = printTemplate.FirstOrDefault().Description;
                model.WorkflowID = printTemplate.FirstOrDefault().WorkflowID;
                model.StatusID = printTemplate.FirstOrDefault().StatusID;
                model.FileID = printTemplate.FirstOrDefault().FileID;
                model.WorkflowName = printTemplate.FirstOrDefault().WorkflowName;
                model.Status = printTemplate.FirstOrDefault().Status;
                model.DataSource = printTemplate.FirstOrDefault().DataSource;
                if (model.FileID > 0)
                {
                    model.FileModel = new PrintCenter().GetFiles(model.FileID);
                }
            }
            return model;
        }

        #endregion

        #region "Private Methods"
        private PrintTemplateList GetByPaging(PrintTemplate model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "filterText", Value = model.FilterText },
                new NameValuePair { Name = "currentIndex", Value = model.CurrentIndex },
                new NameValuePair { Name = "pageSize", Value = model.PageSize },
                new NameValuePair { Name = "sortColumn", Value = model.SortColumn },
                new NameValuePair { Name = "sortDirection", Value = model.SortDirection }
            };
            return new RestSharpHandler().RestRequest<PrintTemplateList>(APISelector.Municipality, true, "api/printtemplate/getbypaging", "GET", lstParameter);
        }
        private List<SelectListItemViewModel> RelatedSteps()
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "statusids", Value = string.Join(",", StatusArray) }
            };
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/printtemplate/relatedstepsget", "GET", lstParameter);
        }
        private int Insert(PrintTemplate model)
        {
            List<Object> parms = new List<Object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/printtemplate/insert", "POST", null, parms);
        }
        public int Update(PrintTemplate model)
        {
            List<Object> parms = new List<Object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/printtemplate/Update", "POST", null, parms);
        }
        public FileModel FileInsert(FileModel model)
        {
            List<Object> parms = new List<Object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<FileModel>(APISelector.Municipality, true, "api/file/insert", "POST", null, parms);
        }
        public int SaveFile(string fileName)
        {
            int fileId = 0;

            if (!string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Current.Session["UploadFiles"])))
            {
                UploadFiles model = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadFiles>(Convert.ToString(HttpContext.Current.Session["UploadFiles"]));
                FileModel fileModel = new FileModel
                {
                    Data = model.Data,
                    ContentType = model.ContentType,
                    FileName = fileName,
                    FileExtension = model.Extention,
                    CreatedUserID = UserSession.Current.Id,
                    CreatedDate = Common.CurrentDateTime,
                    ModifiedUserID = UserSession.Current.Id,
                    ModifiedDate = Common.CurrentDateTime,
                    IsListedInFolders = false,
                    IsDeleted = false,
                    IsActive = true,
                    IsPublicImage = false,
                    Length = model.ContentLength
                };
                fileId = FileInsert(fileModel).ID;
                HttpContext.Current.Session.Remove("UploadFiles");
            }
            return fileId;
        }
        public FileModel FileUpdate(FileModel model)
        {
            List<Object> parms = new List<Object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<FileModel>(APISelector.Municipality, true, "api/file/update", "POST", null, parms);
        }
        #endregion
    }
    public class PrintTemplateList
    {
        #region public properties        
        public List<PrintTemplate> PrintTemplates { get; set; }
        public int TotalRecord { get; set; }
        #endregion
    }
}
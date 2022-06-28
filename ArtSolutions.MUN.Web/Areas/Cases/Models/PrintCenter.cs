using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using System.Collections;
using System.Data;
using ArtSolutions.MUN.Web.Areas.PrintTemplates.Models;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class PrintCenter : GridViewModel
    {
        #region "Properties"
        public int SrNo { get; set; }
        public int ID { get; set; }
        public string KeyCode { get; set; }
        public int PrintTemplateID { get; set; }
        public int OutputFormat { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public string PrintDate { get; set; }
        public Destinatary Destinatary { get; set; } = new Destinatary();
        public int DataSourceID { get; set; }
        public Guid CreatedUserID { get; set; } = UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        public string CaseIDs { get; set; }
        public int FileID { get; set; }
        public int CaseCount { get; set; }
        public decimal Balance { get; set; }
        public string EstimateAmount { get; set; }
        public Guid ModifidUserID { get; set; } = UserSession.Current.Id;
        public DateTime ModifiedDate { get; set; } = Common.CurrentDateTime;
        public int StatuIdSource { get; set; }
        public int StatuIdTarget { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; } = 1;
        public string TemplateName { get; set; }
        public string Format { get; set; }
        public string UserName { get; set; }
        #endregion

        #region "Public Methods"
        public PrintCenter PrintLetter(PrintCenter model)
        {
            SelectListItemViewModel keyCode = KeyCodeGet();
            //I guess user always put hime code in company table.
            model.KeyCode = keyCode.Name + "-" + keyCode.ID;
            model.ID = Insert(model);
            if (model.ID > 0)
            {
                var result = GenerateLetter(model);
                if (result)
                {
                    if (model.CaseIDs.Split(",".ToCharArray()).Length > 0)
                    {
                        List<WorkflowHistoryLog> workflowHistorylog = new List<WorkflowHistoryLog>();
                        foreach (var item in model.CaseIDs.Split(",".ToCharArray()))
                        {
                            workflowHistorylog.Add(new WorkflowHistoryLog
                            {
                                StatuIdSource = model.StatuIdSource,
                                StatuIdTarget = model.StatuIdTarget,
                                WorkflowID = model.WorkflowID,
                                CaseID = Convert.ToInt32(item)
                            });
                        }
                        new CaseModel().CasesStatusUpdate(new CaseModel
                        {
                            WorkflowHistoryLogs = workflowHistorylog
                        });
                    }
                }
            }
            return model;
        }
        public FileModel GetFiles(int fileId)
        {
            List<NameValuePair> parms = new List<NameValuePair>
            {
                new NameValuePair
                {
                    Name = "id",
                    Value = fileId
                }
            };
            return new RestSharpHandler().RestRequest<FileModel>(APISelector.Municipality, true, "api/file/DownloadFile", "GET", parms);
        }
        #endregion "Public Methods"

        #region "Private Methods"
        private SelectListItemViewModel KeyCodeGet()
        {
            return new RestSharpHandler().RestRequest<SelectListItemViewModel>(APISelector.Municipality, true, "api/case/keycodeget", "GET", null);
        }
        private int Insert(PrintCenter model)
        {
            List<object> parms = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/PrintCenterInsert", "POST", null, parms);
        }
        private bool GenerateLetter(PrintCenter model)
        {
            bool result = false;
            var printTemplate = new PrintTemplate().Get(new PrintTemplate
            {
                ID = model.PrintTemplateID
            });
            if (printTemplate != null && printTemplate.Any())
            {
                var fileModel = GetFiles(printTemplate.FirstOrDefault().FileID);
                if (!Directory.Exists(Common.TempFolder))
                {
                    Directory.CreateDirectory(Common.TempFolder);
                }
                var wordFile = Path.Combine(Common.TempFolder, (fileModel.FileName + "" + fileModel.FileExtension));
                var pdfFile = Path.Combine(Path.GetDirectoryName(wordFile), Path.GetFileNameWithoutExtension(wordFile)) + ".pdf";
                File.WriteAllBytes(wordFile, fileModel.Data);
                //Creating a new document.
                WordDocument document = new WordDocument();
                // Load template
                document.Open(wordFile, FormatType.Doc);

                var dataSource = new PrintTemplate().DataSourceGet(printTemplate.FirstOrDefault().DataSourceID);
                if (dataSource != null && dataSource.Any())
                {

                }

                //document.MailMerge.Execute(accountViewModel);
                PrintLetterModel PrintLetterModel = new PrintLetterModel();
                PrintLetterModel.PrintLetterModels.Add(new PrintLetterModel(model.Date.ToString("g"), model.Comments, model.Destinatary.Name,
                        model.Destinatary.Position, model.Destinatary.Department));

                PrintLetterModel.OutputFormatType outpotFormats = (PrintLetterModel.OutputFormatType)model.OutputFormat;
                PrintLetterModel.CaseModel = new CaseModel().MasterCasesGet(-1, -1, 0, model.CaseIDs);
                MailMergeDataTable cases = new MailMergeDataTable("Case", PrintLetterModel.PrintLetterModels);
                MailMergeDataTable caseDetails = new MailMergeDataTable("CaseDetails", PrintLetterModel.CaseModel);


                document.MailMerge.ExecuteGroup(cases);
                document.MailMerge.ExecuteGroup(caseDetails);
                document.Save(wordFile);
                if (outpotFormats == PrintLetterModel.OutputFormatType.PDF)
                {
                    DocToPDFConverter converter = new DocToPDFConverter();
                    PdfDocument pdfDoc = converter.ConvertToPDF(document);
                    pdfDoc.Save(pdfFile);
                    pdfDoc.Close(true);
                }
                document.Close();
                document.Dispose();
                byte[] data = File.ReadAllBytes(outpotFormats == PrintLetterModel.OutputFormatType.Word ? wordFile : pdfFile);
                int fileId = 0;
                var tempFileModel = new FileModel
                {
                    Data = data,
                    ContentType = outpotFormats == PrintLetterModel.OutputFormatType.PDF ? MimeMapping.GetMimeMapping(pdfFile) : MimeMapping.GetMimeMapping(wordFile),
                    FileName = outpotFormats == PrintLetterModel.OutputFormatType.Word ? Path.GetFileName(wordFile) : Path.GetFileName(pdfFile),
                    FileExtension = outpotFormats == PrintLetterModel.OutputFormatType.Word ? Path.GetExtension(wordFile) : Path.GetExtension(pdfFile),
                    CreatedUserID = UserSession.Current.Id,
                    CreatedDate = Common.CurrentDateTime,
                    ModifiedUserID = UserSession.Current.Id,
                    ModifiedDate = Common.CurrentDateTime,
                    IsListedInFolders = false,
                    IsDeleted = false,
                    IsActive = true,
                    IsPublicImage = false,
                    Length = data.Length
                };
                if (model.CaseIDs.Split(",".ToCharArray()).Length > 0)
                {
                    CaseModel caseModel = new CaseModel();
                    foreach (var item in model.CaseIDs.Split(",".ToCharArray()))
                    {
                        fileId = new PrintTemplates.Models.PrintTemplate().FileInsert(tempFileModel).ID;
                        caseModel.CaseImages.Add(new CaseImage
                        {
                            ImageID = fileId,
                            CaseID = Convert.ToInt32(item)
                        });
                    }
                    new CaseImage().CaseImagesInsert(caseModel);
                }
                fileId = new PrintTemplates.Models.PrintTemplate().FileInsert(tempFileModel).ID;
                result = PrintCenterFileIDUpdate(new PrintCenter
                {
                    ID = model.ID,
                    FileID = fileId
                }) > 0;
                try
                {
                    if (Directory.Exists(Common.TempFolder))
                        Array.ForEach(Directory.GetFiles(Common.TempFolder), File.Delete);
                }
                catch (Exception)
                {
                    // throw;
                }
            }
            return result;
        }
        private List<AccountView> AccountViewGet()
        {
            return new RestSharpHandler().RestRequest<List<AccountView>>(APISelector.Municipality, true, "api/case/AccountViewGet", "GET", null);
        }
        private int PrintCenterFileIDUpdate(PrintCenter model)
        {
            List<object> parms = new List<object>();
            parms.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/PrintCenterFileIDUpdate", "POST", null, parms);
        }
        public byte[] PreviewLetter(PrintCenter model)
        {
            byte[] data = null;
            var printTemplate = new PrintTemplates.Models.PrintTemplate().Get(new PrintTemplates.Models.PrintTemplate
            {
                ID = model.PrintTemplateID
            });
            if (printTemplate != null && printTemplate.Any())
            {
                var fileModel = GetFiles(printTemplate.FirstOrDefault().FileID);

                if (!Directory.Exists(Common.TempFolder))
                {
                    Directory.CreateDirectory(Common.TempFolder);
                }

                var wordFile = Path.Combine(Common.TempFolder, (fileModel.FileName + "" + fileModel.FileExtension));

                var pdfFile = Path.Combine(Path.GetDirectoryName(wordFile), Path.GetFileNameWithoutExtension(wordFile)) + ".pdf";

                File.WriteAllBytes(wordFile, fileModel.Data);

                //Creating a new document.
                WordDocument document = new WordDocument();
                // Load template
                document.Open(wordFile, FormatType.Doc);

                //Comment for testing

                //var accountViewModel = AccountViewGet();

                //document.MailMerge.Execute(accountViewModel);


                PrintLetterModel PrintLetterModel = new PrintLetterModel();

                PrintLetterModel.PrintLetterModels.Add(new PrintLetterModel(model.Date.ToString("g"), model.Comments, model.Destinatary.Name,
                        model.Destinatary.Position, model.Destinatary.Department));

                PrintLetterModel.OutputFormatType outpotFormats = (PrintLetterModel.OutputFormatType)model.OutputFormat;

                PrintLetterModel.CaseModel = new CaseModel().MasterCasesGet(-1, -1, 0, model.CaseIDs);

                MailMergeDataTable cases = new MailMergeDataTable("Case", PrintLetterModel.PrintLetterModels);

                MailMergeDataTable caseDetails = new MailMergeDataTable("CaseDetails", PrintLetterModel.CaseModel);


                document.MailMerge.ExecuteGroup(cases);

                document.MailMerge.ExecuteGroup(caseDetails);

                document.Save(wordFile);

                if (outpotFormats == PrintLetterModel.OutputFormatType.PDF)
                {
                    DocToPDFConverter converter = new DocToPDFConverter();

                    PdfDocument pdfDoc = converter.ConvertToPDF(document);

                    pdfDoc.Save(pdfFile);

                    pdfDoc.Close(true);
                }
                document.Close();
                document.Dispose();
                data = File.ReadAllBytes(outpotFormats == PrintLetterModel.OutputFormatType.Word ? wordFile : pdfFile);

                try
                {
                    if (Directory.Exists(Common.TempFolder))
                        Array.ForEach(Directory.GetFiles(Common.TempFolder), File.Delete);
                }
                catch (Exception)
                {
                    // throw;
                }
            }
            return data;
        }
        #endregion "Private Methods"
    }

    public class PrintCenterList
    {
        #region "Properties"
        public List<PrintCenter> PrintCenters { get; set; } = new List<PrintCenter>();
        public int TotalRecord { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public PrintCenterList Get(PrintCenter customSearch, JQDTParams jqdtParams)
        {
            PrintCenterList printCenterList = new PrintCenterList();

            customSearch.SortColumnName = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;

            customSearch.CurrentIndex = (jqdtParams.Start + 1);

            customSearch.PageSize = jqdtParams.Length;

            customSearch.Sort = jqdtParams.Order[0].Dir.ToString();

            printCenterList = Get(customSearch);

            if (printCenterList.PrintCenters.Any())
            {
                printCenterList.PrintCenters.ForEach(a =>
                {
                    a.Format = ((PrintLetterModel.OutputFormatType)a.OutputFormat).ToString();
                    a.EstimateAmount = a.Balance.ToString("C");
                    a.PrintDate = a.Date.ToString("MM/dd/yyyy");
                    a.UserName = new UserProfile().UserProfileGet(new UserProfileViewModel
                    {
                        UserID = a.CreatedUserID
                    }).FullName;
                });
            }
            return printCenterList;
        }
        #endregion

        #region "Private Methods"
        private PrintCenterList Get(PrintCenter model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "filterText", Value = model.FilterText });
            lstParameter.Add(new NameValuePair { Name = "currentIndex", Value = model.CurrentIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = model.PageSize });
            lstParameter.Add(new NameValuePair { Name = "sortColumn", Value = model.SortColumnName });
            lstParameter.Add(new NameValuePair { Name = "sortDirection", Value = model.Sort });
            return new RestSharpHandler().RestRequest<PrintCenterList>(APISelector.Municipality, true, "api/case/PrintCenterGet", "GET", lstParameter);
        }
        #endregion

    }
    public class PrintCenterLog
    {
        #region "Properties"
        public Guid UserID { get; set; } = UserSession.Current.Id;
        public DateTime GenerateDate { get; set; } = Common.CurrentDateTime;
        public int FileID { get; set; }
        public string Action { get; set; } = Common.Actions.Download.ToString();
        public Guid CreatedUserID { get; set; } = UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        #endregion "Properties"

        #region "Public Methods"
        public int PrintCenterLogInsert(PrintCenterLog model)
        {
            List<object> parms = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/PrintCenterLogInsert", "POST", null, parms);
        }
        #endregion
    }

}
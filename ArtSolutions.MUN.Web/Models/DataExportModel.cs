using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Linq;

namespace ArtSolutions.MUN.Web.Models
{
    public class DataExportModel
    {
        public string FolderPath { get; set; } = Common.ExportFolderPath;
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public string AsOF { get; set; }
        public string ReportDate { get; set; }
        public ExportType Type { get; set; }
        public string FileName { get; set; }
        public string TableHead { get; set; }
        public string TableBody { get; set; }
        public string TableFooter { get; set; }
        public string TableColumns { get; set; }
        public string TableMappingColumns { get; set; }
        public enum ExportType
        {
            Excel,
            PDF
        }

        public ExportFileViewModel GenerateExportFile(DataExportModel model)
        {
            ExportFileViewModel exportFileViewModel = new ExportFileViewModel();

            //string[] modifiedHTML = new string[] { };
            string[] htmlString = File.ReadAllLines(Path.Combine(model.FolderPath, Common.ExportHTMLFile));
            if (htmlString != null && htmlString.Any())
            {
                for (int i = 0; i < htmlString.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(htmlString[i]))
                        continue;

                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Report Title-->"))
                    {
                        htmlString[i] = model.ReportTitle;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Report Type-->"))
                    {
                        htmlString[i] = model.ReportType;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Report As OF-->"))
                    {
                        htmlString[i] = model.AsOF;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Report Date-->"))
                    {
                        htmlString[i] = model.ReportDate;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Table Header-->"))
                    {
                        htmlString[i] = model.TableHead;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Table Body-->"))
                    {
                        htmlString[i] = model.TableBody;
                    }
                    if (htmlString[i].Trim().Equals("<!--Do not delete this comment because This is a replace tag for Table Footer-->"))
                    {
                        htmlString[i] = model.TableFooter;
                    }
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    switch (model.Type)
                    {
                        case ExportType.PDF:
                            string tempString = string.Join(" ", htmlString);
                            TextReader txtReader = new StringReader(tempString);

                            // 1: create object of a itextsharp document class  
                            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

                            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
                            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, memoryStream);

                            doc.Open();

                            XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);

                            doc.Close();
                            //var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(tempString, PdfSharp.PageSize.A4);
                            //pdf.Save(memoryStream);
                            exportFileViewModel.Data = memoryStream.ToArray();
                            exportFileViewModel.FileName = model.FileName + ".pdf";
                            break;
                        case ExportType.Excel:
                            StreamWriter streamWriter = new StreamWriter(memoryStream);
                            streamWriter.WriteLine(string.Join(" ", htmlString));
                            streamWriter.Flush();
                            exportFileViewModel.Data = memoryStream.ToArray();
                            exportFileViewModel.FileName = model.FileName + ".xls";
                            break;
                    }
                }
            }
            return exportFileViewModel;
        }
    }
    public class ExportFileViewModel
    {
        public byte[] Data { get; set; } = new byte[] { };
        public string FileName { get; set; } = string.Empty;
    }
}
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ExportModel
    {
        #region public methods
        public ExportFileModel GenerateExportFile(ExportType Type, string FileName, string HtmlString, bool isLandscape = false)
        {
            ExportFileModel exportFileViewModel = new ExportFileModel();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                switch (Type)
                {
                    case ExportType.PDF:
                        string tempString = HtmlString;
                        TextReader txtReader = new StringReader(tempString);

                        // 1: create object of a itextsharp document class  
                        Document doc = new Document(PageSize.A4, 60f, 60f, 60f, 60f);
                        if (isLandscape)
                        {
                            doc.SetPageSize(PageSize.A4.Rotate());
                        }

                        // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
                        PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, memoryStream);

                        doc.Open();

                        XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);

                        doc.Close();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".pdf";
                        break;
                    case ExportType.Excel:
                        StreamWriter streamWriter = new StreamWriter(memoryStream);
                        streamWriter.WriteLine(HtmlString);
                        streamWriter.Flush();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".xls";                       
                        break;
                }
            }
            return exportFileViewModel;
        }

        public ExportFileModel GenerateExportFileA3(ExportType Type, string FileName, string HtmlString, bool isLandscape = false)
        {
            ExportFileModel exportFileViewModel = new ExportFileModel();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                switch (Type)
                {
                    case ExportType.PDF:
                        string tempString = HtmlString;
                        TextReader txtReader = new StringReader(tempString);

                        // 1: create object of a itextsharp document class  
                        Document doc = new Document(PageSize.A3, 60f, 60f, 60f, 60f);
                        if (isLandscape)
                        {
                            doc.SetPageSize(PageSize.A3.Rotate());
                        }

                        // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
                        PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, memoryStream);

                        doc.Open();

                        XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);

                        doc.Close();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".pdf";
                        break;
                    case ExportType.Excel:
                        StreamWriter streamWriter = new StreamWriter(memoryStream);
                        streamWriter.WriteLine(HtmlString);
                        streamWriter.Flush();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".xls";
                        break;
                }
            }
            return exportFileViewModel;
        }

        public ExportFileModel GenerateExportFileLetter(ExportType Type, string FileName, string HtmlString, bool isLandscape = false)
        {
            ExportFileModel exportFileViewModel = new ExportFileModel();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                switch (Type)
                {
                    case ExportType.PDF:
                        string tempString = HtmlString;
                        TextReader txtReader = new StringReader(tempString);

                        // 1: create object of a itextsharp document class  
                        Document doc = new Document(PageSize.LETTER, 60f, 60f, 60f, 60f);
                        if (isLandscape)
                        {
                            doc.SetPageSize(PageSize.LETTER.Rotate());
                        }

                        // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
                        PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, memoryStream);

                        doc.Open();

                        XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);

                        doc.Close();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".pdf";
                        break;
                    case ExportType.Excel:
                        StreamWriter streamWriter = new StreamWriter(memoryStream);
                        streamWriter.WriteLine(HtmlString);
                        streamWriter.Flush();
                        exportFileViewModel.Data = memoryStream.ToArray();
                        exportFileViewModel.FileName = FileName + ".xls";
                        break;
                }
            }
            return exportFileViewModel;
        }
        #endregion
    }

    public class ExportFileModel
    {
        #region public properties
        public byte[] Data { get; set; } = new byte[] { };
        public string FileName { get; set; } = string.Empty;
        #endregion
    }
    public enum ExportType
    {
        Excel,
        PDF
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using System.Text;
using ArtSolutions.MUN.Web.Areas.Collections.Models;
using System.Text.RegularExpressions;

namespace ArtSolutions.MUN.Web.Models
{
    public class ImportModel
    {
        public DataTable GetDataTableFromImportFile(string path, string extension)
        {
            DataTable dt = null;
            try
            {
                switch (extension)
                {
                    case ".xlsx":
                    case ".xls":
                        dt = GetDataTableFromExcel(path);
                        break;
                    case ".csv":
                        dt = GetDataTableFromCSV(path);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception(ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage);
            }
            return dt;
        }
        private DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
        public DataTable GetDataTableFromExcelForImportMeasuredWaterFiling(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();

                if (ws.Cells.Count() > 0)
                {
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();

                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                }
                return tbl;
            }
        }
        private DataTable GetDataTableFromCSV(string strFilePath)
        {
            DataTable csvData = new DataTable();

            using (TextFieldParser csvReader = new TextFieldParser(strFilePath, System.Text.Encoding.Default))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;
                //read column names
                string[] colFields = csvReader.ReadFields();
                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }
                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }

            return csvData;
        }
        public BankPaymentReadImportFileModel GetDataTableFromTxtForBankPayments(string strFilePath)
        {
            BankPaymentReadImportFileModel model = new BankPaymentReadImportFileModel();
            string note = null;

            DataTable dtBankPayments = new DataTable();
            dtBankPayments.Columns.Add("ImportID", typeof(System.String));
            dtBankPayments.Columns.Add("Contract", typeof(System.String));
            dtBankPayments.Columns.Add("Tax Number", typeof(System.String));
            dtBankPayments.Columns.Add("PERIODOREC", typeof(System.String));
            dtBankPayments.Columns.Add("Payment Date", typeof(System.String));
            dtBankPayments.Columns.Add("Amount", typeof(System.String));
            dtBankPayments.Columns.Add("Receipt Number", typeof(System.String));

            var fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);

            int rowCount = 1;
            List<string> noteList = File.ReadLines(strFilePath).Reverse().Where(line => line != "").Take(2).ToList();

            int note1Length = 0;
            int note2Length = 0;

            if (noteList != null && noteList.Count == 2)
            {
                note1Length = noteList[0].Replace(" ", "").Length;
                note2Length = noteList[1].Replace(" ", "").Length;

                //noteList.ForEach(x =>
                //{
                //    x = x.ToString().Trim();
                //});
                noteList = noteList.Select(x => x.Replace(x, x.Trim())).ToList();
            }

            if ((note1Length == 62 || note1Length == 63 || note1Length == 78) && (note2Length == 62 || note2Length == 63 || note2Length == 78))
                model.IsValidFile = true;
            else
                model.IsValidFile = false;

            if (model.IsValidFile)
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        line = line.ToString().Trim();

                        string convenio = null;
                        string cedula = null;
                        string period = null;
                        string paymentDate = null;
                        string receiptNumber = null;
                        string amount = null;

                        if (noteList.Contains(line))
                            note = note + '\n' + line;
                        else
                        {
                            if (line != "")
                            {
                                int totalChar = line.ToCharArray().Count();
                                                               
                                List<string> importBankList = line.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                                string strImportBankDetail = null;

                                if (importBankList!=null && importBankList.Any())
                                {
                                    if (importBankList[0] != null && importBankList[0].Any())
                                    {
                                        strImportBankDetail = importBankList[0].Trim();
                                        convenio = totalChar < 8 ? null : strImportBankDetail.Substring(5, 3);
                                        cedula = totalChar < (40+ (strImportBankDetail.Length-40)) ? null : strImportBankDetail.Substring(40, strImportBankDetail.Length - 40);
                                    }
                                    if (importBankList[1] != null && importBankList[1].Any())
                                    {
                                        strImportBankDetail = importBankList[1].Trim();
                                        period = totalChar < 8 ? null : strImportBankDetail.Substring(0, 8);
                                        paymentDate = totalChar < 16 ? null : strImportBankDetail.Substring(8, 8);
                                        amount = totalChar < 46 ? null : strImportBankDetail.Substring(28, 18);
                                        receiptNumber = totalChar < 68 ? null : strImportBankDetail.Substring(48, 20);
                                    }
                                }

                                DataRow dr = dtBankPayments.NewRow();
                                dr["ImportID"] = rowCount;
                                dr["Contract"] = !string.IsNullOrEmpty(convenio) ? convenio.Trim() : null;
                                dr["Tax Number"] = !string.IsNullOrEmpty(cedula) ? cedula.Trim() : null;
                                dr["PERIODOREC"] = !string.IsNullOrEmpty(period) ? period.Trim() : null;
                                dr["Payment Date"] = !string.IsNullOrEmpty(paymentDate) ? paymentDate.Trim() : null;
                                dr["Amount"] = !string.IsNullOrEmpty(amount) ? amount.Trim() : null;
                                dr["Receipt Number"] = !string.IsNullOrEmpty(receiptNumber) ? receiptNumber.Trim(new Char[] { '0' }) : null;

                                dtBankPayments.Rows.Add(dr);
                            }
                            rowCount++;
                        }
                    }
                }

                if (dtBankPayments.Rows.Count == 0)
                {
                    DataRow dr = dtBankPayments.NewRow();
                    dr["ImportID"] = null;
                    dr["Contract"] = null;
                    dr["Tax Number"] = null;
                    dr["PERIODOREC"] = null;
                    dr["Payment Date"] = null;
                    dr["Amount"] = null;
                    dr["Receipt Number"] = null;
                    dtBankPayments.Rows.Add(dr);
                }

                model.BankPaymentList = dtBankPayments;
                model.Note = note;

                //----------------------- CO-1714 Changes -----------------------          
                model.TotalLinesInFileReceived = noteList.Count + dtBankPayments.Rows.Count;
                model.TotalLinesWithPayments = dtBankPayments.Rows.Count;

                model.TotalCommission =
                    Convert.ToDecimal(
                          !string.IsNullOrEmpty(noteList[1].Substring(noteList[1].Length - 24).Substring(0, 18)) ?
                          (noteList[1].Substring(noteList[1].Length - 24).Substring(0, 16).Trim(new Char[] { '0' }) + "." + noteList[1].Substring(noteList[1].Length - 24).Substring(16, 2)) :
                          null
                     );

                model.TotalAmount =
                  Convert.ToDecimal(
                        !string.IsNullOrEmpty(noteList[0].Substring(noteList[0].Length - 24).Substring(0, 18)) ?
                        (noteList[0].Substring(noteList[0].Length - 24).Substring(0, 16).Trim(new Char[] { '0' }) + "." + noteList[0].Substring(noteList[0].Length - 24).Substring(16, 2)) :
                        null
                   );
                //
            }

            return model;
        }
        public DataTable GetDataTableForReadFile(string[] arrFilePath, int year, int month)
        {
            DataTable dtMeasuredWater = new DataTable();
            dtMeasuredWater.Columns.Add("RowNo", typeof(System.Int32));
            dtMeasuredWater.Columns.Add("Year", typeof(System.String));
            dtMeasuredWater.Columns.Add("Month", typeof(System.String));
            dtMeasuredWater.Columns.Add("CATEGORIA", typeof(System.String));
            dtMeasuredWater.Columns.Add("TaxNumber", typeof(System.String));
            dtMeasuredWater.Columns.Add("UBICACION", typeof(System.String));
            dtMeasuredWater.Columns.Add("CODIGOM", typeof(System.String));
            dtMeasuredWater.Columns.Add("LECTURAACT", typeof(System.String));
            dtMeasuredWater.Columns.Add("LastReading", typeof(System.String));
            dtMeasuredWater.Columns.Add("Difference", typeof(System.String));
            dtMeasuredWater.Columns.Add("FECHA", typeof(System.String));
            dtMeasuredWater.Columns.Add("Note", typeof(System.String));
            dtMeasuredWater.Columns.Add("IsValid", typeof(System.Boolean));
            dtMeasuredWater.Columns.Add("IsEditable", typeof(System.Boolean));

            int rowNo = 1;

            foreach (var strFilePath in arrFilePath)
            {
                var fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                var stream = new StreamReader(fileStream, Encoding.UTF8);

                using (var streamReader = stream)
                {
                    string line;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string ubicacion = null;
                        string codigom = null;
                        string categoria = null;
                        string lecturaact = null;
                        string fecha = null;
                        string taxNumber = null;
                        string lastReading = null;
                        string difference = null;
                        //string note = null;

                        if (line != "" && line.Contains('|'))
                        {
                            var arrLine = line.Split('|');
                            if (arrLine.Count() == 14) // Text File = 14
                            {
                                ubicacion = arrLine[0] + arrLine[1] + arrLine[2] + arrLine[3];
                                codigom = arrLine[4];
                                categoria = arrLine[5];
                                lecturaact = arrLine[8];
                                fecha = arrLine[9];

                                DataRow dr = dtMeasuredWater.NewRow();
                                dr["RowNo"] = rowNo++;
                                dr["Year"] = year;
                                dr["Month"] = month;
                                dr["CATEGORIA"] = !string.IsNullOrEmpty(categoria) ? categoria.Trim() : null;
                                dr["TaxNumber"] = !string.IsNullOrEmpty(taxNumber) ? taxNumber.Trim() : null;
                                dr["UBICACION"] = !string.IsNullOrEmpty(ubicacion) ? ubicacion.Trim() : null;
                                dr["CODIGOM"] = !string.IsNullOrEmpty(codigom) ? codigom.Trim() : null;
                                dr["LECTURAACT"] = !string.IsNullOrEmpty(lecturaact) ? lecturaact.Trim() : null;
                                dr["LastReading"] = !string.IsNullOrEmpty(lastReading) ? lastReading.Trim() : null;
                                dr["Difference"] = !string.IsNullOrEmpty(difference) ? difference.Trim() : null;
                                dr["FECHA"] = !string.IsNullOrEmpty(fecha) ? fecha.Trim() : null;
                                dr["Note"] = null;
                                dr["IsValid"] = true;
                                dr["IsEditable"] = true;

                                dtMeasuredWater.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            return dtMeasuredWater;
        }

        public MeasuredWaterFilingReadImportFileModel GetDataTableForMeasuredWaterFilingForFixFile(string[] arrFilePath, int year, int month)
        {
            MeasuredWaterFilingReadImportFileModel model = new MeasuredWaterFilingReadImportFileModel();

            DataTable dtMeasuredWater = new DataTable();
            dtMeasuredWater.Columns.Add("RowNo", typeof(System.Int32));
            dtMeasuredWater.Columns.Add("Year", typeof(System.String));
            dtMeasuredWater.Columns.Add("Month", typeof(System.String));
            dtMeasuredWater.Columns.Add("CATEGORIA", typeof(System.String));
            dtMeasuredWater.Columns.Add("TaxNumber", typeof(System.String));
            dtMeasuredWater.Columns.Add("UBICACION", typeof(System.String));
            dtMeasuredWater.Columns.Add("CODIGOM", typeof(System.String));
            dtMeasuredWater.Columns.Add("LECTURAACT", typeof(System.String));
            dtMeasuredWater.Columns.Add("LastReading", typeof(System.String));
            dtMeasuredWater.Columns.Add("Difference", typeof(System.String));
            dtMeasuredWater.Columns.Add("FECHA", typeof(System.String));
            dtMeasuredWater.Columns.Add("IsValid", typeof(System.String));

            foreach (var strFilePath in arrFilePath)
            {
                var fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                var stream = new StreamReader(fileStream, Encoding.UTF8);

                using (var streamReader = stream)
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string ubicacion = null;
                        string codigom = null;
                        string categoria = null;
                        string lecturaact = null;
                        string fecha = null;
                        string taxNumber = null;
                        string lastReading = null;
                        string difference = null;
                        //string note = null;

                        if (line != "" && line.Contains('|'))
                        {
                            int rowNo = 1;
                            var arrLine = line.Split('|');
                            if (arrLine.Count() == 14) // Text File = 14
                            {
                                ubicacion = arrLine[0] + arrLine[1] + arrLine[2] + arrLine[3];
                                codigom = arrLine[4];
                                categoria = arrLine[5];
                                lecturaact = arrLine[8];
                                fecha = arrLine[9];

                                DataRow dr = dtMeasuredWater.NewRow();
                                dr["RowNo"] = rowNo++;
                                dr["Year"] = year;
                                dr["Month"] = month;
                                dr["CATEGORIA"] = !string.IsNullOrEmpty(categoria) ? categoria.Trim() : null;
                                dr["TaxNumber"] = !string.IsNullOrEmpty(taxNumber) ? taxNumber.Trim() : null;
                                dr["UBICACION"] = !string.IsNullOrEmpty(ubicacion) ? ubicacion.Trim() : null;
                                dr["CODIGOM"] = !string.IsNullOrEmpty(codigom) ? codigom.Trim() : null;
                                dr["LECTURAACT"] = !string.IsNullOrEmpty(lecturaact) ? lecturaact.Trim() : null;
                                dr["LastReading"] = !string.IsNullOrEmpty(lastReading) ? lastReading.Trim() : null;
                                dr["Difference"] = !string.IsNullOrEmpty(difference) ? difference.Trim() : null;
                                dr["FECHA"] = !string.IsNullOrEmpty(fecha) ? fecha.Trim() : null;
                                dr["IsValid"] = null;

                                dtMeasuredWater.Rows.Add(dr);
                            }
                        }
                    }

                    if (dtMeasuredWater.Rows.Count > 0)
                    {
                        model.MeasuredWaterFilingList = dtMeasuredWater;
                        model.IsValidData = true;
                    }
                    else
                        model.IsValidData = false;
                }
            }
            return model;
        }
        public MeasuredWaterFilingReadImportFileModel GetDataTableForMeasuredWaterFiling(string[] arrFilePath, bool isValidate)
        {
            MeasuredWaterFilingReadImportFileModel model = new MeasuredWaterFilingReadImportFileModel();

            DataTable dtMeasuredWater = new DataTable();
            dtMeasuredWater.Columns.Add("CATEGORIA", typeof(System.String));
            dtMeasuredWater.Columns.Add("TaxNumber", typeof(System.String));
            dtMeasuredWater.Columns.Add("UBICACION", typeof(System.String));
            dtMeasuredWater.Columns.Add("CODIGOM", typeof(System.String));
            dtMeasuredWater.Columns.Add("LECTURAACT", typeof(System.String));
            dtMeasuredWater.Columns.Add("LastReading", typeof(System.String));
            dtMeasuredWater.Columns.Add("Difference", typeof(System.String));
            dtMeasuredWater.Columns.Add("FECHA", typeof(System.String));
            dtMeasuredWater.Columns.Add("Note", typeof(System.String));

            foreach (var strFilePath in arrFilePath)
            {
                var fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                var stream = new StreamReader(fileStream, Encoding.UTF8);

                using (var streamReader = stream)
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string ubicacion = null;
                        string codigom = null;
                        string categoria = null;
                        string lecturaact = null;
                        string fecha = null;
                        string taxNumber = null;
                        string lastReading = null;
                        string difference = null;
                        string note = null;

                        if (line != "" && line.Contains('|'))
                        {
                            var arrLine = line.Split('|');
                            if (arrLine.Count() == 14) // Text File = 14
                            {
                                ubicacion = arrLine[0] + arrLine[1] + arrLine[2] + arrLine[3];
                                codigom = arrLine[4];
                                categoria = arrLine[5];
                                lecturaact = arrLine[8];
                                fecha = arrLine[9];

                                DataRow dr = dtMeasuredWater.NewRow();
                                dr["CATEGORIA"] = !string.IsNullOrEmpty(categoria) ? categoria.Trim() : null;
                                dr["TaxNumber"] = !string.IsNullOrEmpty(taxNumber) ? taxNumber.Trim() : null;
                                dr["UBICACION"] = !string.IsNullOrEmpty(ubicacion) ? ubicacion.Trim() : null;
                                dr["CODIGOM"] = !string.IsNullOrEmpty(codigom) ? codigom.Trim() : null;
                                dr["LECTURAACT"] = !string.IsNullOrEmpty(lecturaact) ? lecturaact.Trim() : null;
                                dr["LastReading"] = !string.IsNullOrEmpty(lastReading) ? lastReading.Trim() : null;
                                dr["Difference"] = !string.IsNullOrEmpty(difference) ? difference.Trim() : null;
                                dr["FECHA"] = !string.IsNullOrEmpty(fecha) ? fecha.Trim() : null;
                                dr["Note"] = !string.IsNullOrEmpty(note) ? note.Trim() : null;

                                dtMeasuredWater.Rows.Add(dr);
                            }
                        }
                    }

                    if (dtMeasuredWater.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtMeasuredWater);
                        DataTable distinctDataTable = view.ToTable(
                                true, "CATEGORIA", "TaxNumber", "UBICACION", "CODIGOM",
                                "LECTURAACT", "LastReading", "Difference", "FECHA", "Note");

                        distinctDataTable.Columns.Add("IsValid");
                        distinctDataTable.Columns.Add("RowNo");

                        int rowNo = 1;
                        foreach (DataRow dr in distinctDataTable.Rows)
                        {
                            if (isValidate)
                                dr["IsValid"] = true;
                            dr["RowNo"] = rowNo++;
                        }
                        model.MeasuredWaterFilingList = distinctDataTable;

                        model.IsValidData = true;
                    }
                    else
                        model.IsValidData = false;
                }
            }
            return model;
        }
        public List<ImportViewAccountModel> GetAccountListFromImportDatatTable(DataTable table, AccountMappingViewModel mappModel)
        {
            try
            {
                var list = new List<ImportViewAccountModel>(table.Rows.Count);

                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {
                        DateTime? date = null;
                        if (row[mappModel.InitialDate] != null && row[mappModel.InitialDate].ToString() != "")
                        {
                            date = Convert.ToDateTime(row[mappModel.InitialDate]);
                        }

                        if (ArtSolutions.MUN.Web.Helpers.UserSession.Current.CountryID != 52)
                        {
                            var obj = new ImportViewAccountModel()
                            {
                                GroupCode = row[mappModel.GroupCode].ToString(),
                                FirstName = row[mappModel.FirstName].ToString(),
                                LastName = row[mappModel.LastName].ToString(),
                                Salutation = row[mappModel.Salutation].ToString(),
                                AccountType = row[mappModel.AccountType].ToString(),
                                Name = row[mappModel.Name].ToString(),
                                DisplayName = row[mappModel.DisplayName].ToString(),
                                BusinessDescription = row[mappModel.BusinessDescription].ToString(),
                                Email = row[mappModel.Email].ToString(),
                                Website = row[mappModel.Website].ToString(),
                                WorkPhone = row[mappModel.WorkPhone].ToString(),
                                MobilePhone = row[mappModel.MobilePhone].ToString(),
                                Currency = row[mappModel.Currency].ToString(),
                                IDType = row[mappModel.IDType].ToString(),
                                TaxNumber = row[mappModel.TaxNumber].ToString(),
                                StateNumber = row[mappModel.StateNumber].ToString(),
                                TreasuryNumber = row[mappModel.TreasuryNumber].ToString(),
                                InitialDate = date,
                                NAICSCode = row[mappModel.NAICSCode].ToString(),
                                BusinessType = row[mappModel.BusinessType].ToString(),
                                Zone = row[mappModel.Zone].ToString(),
                                ARPEDescription = row[mappModel.ARPEDescription].ToString(),
                                PostalAddressLine1 = row[mappModel.PostalAddressLine1].ToString(),
                                PostalAddressLine2 = row[mappModel.PostalAddressLine2].ToString(),
                                PostalCity = row[mappModel.PostalCity].ToString(),
                                PostalTown = row[mappModel.PostalTown].ToString(),
                                PostalState = row[mappModel.PostalState].ToString(),
                                PostalCountry = row[mappModel.PostalCountry].ToString(),
                                PostalZipCode = row[mappModel.PostalZipCode].ToString(),
                                OfficeAddressLine1 = row[mappModel.OfficeAddressLine1].ToString(),
                                OfficeAddressLine2 = row[mappModel.OfficeAddressLine2].ToString(),
                                OfficeCity = row[mappModel.OfficeCity].ToString(),
                                OfficeTown = row[mappModel.OfficeTown].ToString(),
                                OfficeState = row[mappModel.OfficeState].ToString(),
                                OfficeCountry = row[mappModel.OfficeCountry].ToString(),
                                OfficeZipCode = row[mappModel.OfficeZipCode].ToString(),
                                ContactName = row[mappModel.ContactName].ToString(),
                                ContactLastName = row[mappModel.ContactLastName].ToString(),
                                ContactEmail = row[mappModel.ContactEmail].ToString(),
                                ContactPhone = row[mappModel.ContactPhone].ToString(),
                                ContactPosition = row[mappModel.ContactPosition].ToString(),
                                ReferenceID = row[mappModel.ReferenceID].ToString()
                            };
                            list.Add(obj);
                        }
                        else
                        {
                            var obj = new ImportViewAccountModel()
                            {
                                GroupCode = row[mappModel.GroupCode].ToString(),
                                FirstName = row[mappModel.FirstName].ToString(),
                                LastName = row[mappModel.LastName].ToString(),
                                Salutation = row[mappModel.Salutation].ToString(),
                                AccountType = row[mappModel.AccountType].ToString(),
                                Name = row[mappModel.Name].ToString(),
                                DisplayName = row[mappModel.DisplayName].ToString(),
                                BusinessDescription = row[mappModel.BusinessDescription].ToString(),
                                Email = row[mappModel.Email].ToString(),
                                Website = row[mappModel.Website].ToString(),
                                WorkPhone = row[mappModel.WorkPhone].ToString(),
                                MobilePhone = row[mappModel.MobilePhone].ToString(),
                                Currency = row[mappModel.Currency].ToString(),
                                IDType = row[mappModel.IDType].ToString(),
                                TaxNumber = row[mappModel.TaxNumber].ToString(),
                                //StateNumber = row[mappModel.StateNumber].ToString(),
                                //TreasuryNumber = row[mappModel.TreasuryNumber].ToString(),
                                InitialDate = date,
                                //NAICSCode = row[mappModel.NAICSCode].ToString(),
                                //BusinessType = row[mappModel.BusinessType].ToString(),
                                //Zone = row[mappModel.Zone].ToString(),
                                //ARPEDescription = row[mappModel.ARPEDescription].ToString(),
                                PostalAddressLine1 = row[mappModel.PostalAddressLine1].ToString(),
                                PostalAddressLine2 = row[mappModel.PostalAddressLine2].ToString(),
                                PostalCity = row[mappModel.PostalCity].ToString(),
                                PostalTown = row[mappModel.PostalTown].ToString(),
                                PostalState = row[mappModel.PostalState].ToString(),
                                PostalCountry = row[mappModel.PostalCountry].ToString(),
                                PostalZipCode = row[mappModel.PostalZipCode].ToString(),
                                OfficeAddressLine1 = row[mappModel.OfficeAddressLine1].ToString(),
                                OfficeAddressLine2 = row[mappModel.OfficeAddressLine2].ToString(),
                                OfficeCity = row[mappModel.OfficeCity].ToString(),
                                OfficeTown = row[mappModel.OfficeTown].ToString(),
                                OfficeState = row[mappModel.OfficeState].ToString(),
                                OfficeCountry = row[mappModel.OfficeCountry].ToString(),
                                OfficeZipCode = row[mappModel.OfficeZipCode].ToString(),
                                ContactName = row[mappModel.ContactName].ToString(),
                                ContactLastName = row[mappModel.ContactLastName].ToString(),
                                ContactEmail = row[mappModel.ContactEmail].ToString(),
                                ContactPhone = row[mappModel.ContactPhone].ToString(),
                                ContactPosition = row[mappModel.ContactPosition].ToString(),
                                ReferenceID = row[mappModel.ReferenceID].ToString()
                            };
                            list.Add(obj);
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ImportViewAccountServiceFilingModel> GetAccountServiceFilingListFromImportDatatTable(DataTable table, AccountServiceFilingMappingViewModel mappModel)
        {
            try
            {
                var list = new List<ImportViewAccountServiceFilingModel>(table.Rows.Count);

                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {

                        var obj = new ImportViewAccountServiceFilingModel()
                        {
                            GroupCode = row[mappModel.GroupCode].ToString(),
                            Year = row[mappModel.Year].ToString(),
                            Month = row[mappModel.Month].ToString(),
                            ServiceCode = row[mappModel.ServiceCode].ToString(),
                            ServiceName = row[mappModel.ServiceName].ToString(),
                            CustomField1 = row[mappModel.CustomField1].ToString(),
                            LastReading = row[mappModel.LastReading].ToString(),
                            CurrentReading = row[mappModel.CurrentReading].ToString(),
                            Difference = row[mappModel.Difference].ToString(),
                            TaxNumber = row[mappModel.TaxNumber].ToString()
                        };
                        list.Add(obj);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ImportViewAccountPropertyDetailWithErrorModel GetAccountPropertyListFromImportDatatTable(DataTable table, AccountPropertyMappingViewModel mappModel)
        {
            try
            {
                var list = new List<ImportViewAccountPropertyModel>(table.Rows.Count);
                List<ValidAccountPropertyStatement> ValidAccountPropertyStatementList = new List<ValidAccountPropertyStatement>();

                foreach (DataRow row in table.Rows)
                {
                    decimal? TerrainArea = null;
                    decimal? Percentage = null;
                    int? CurrentAge = null;
                    decimal? BuildingArea = null;

                    if (row != null)
                    {
                        if (!string.IsNullOrEmpty(mappModel.TerrainArea) && row[mappModel.TerrainArea] != null && row[mappModel.TerrainArea].ToString() != "")
                        {
                            try
                            {
                                TerrainArea = Convert.ToDecimal(row[mappModel.TerrainArea].ToString());
                            }
                            catch (Exception)
                            {
                                TerrainArea = 0;
                                var Validmodel = new ValidAccountPropertyStatement();
                                Validmodel.GroupCode = row[mappModel.GroupCode].ToString();
                                Validmodel.Reason = Resources.AccountProperty.AllowOnlyNumericInTerrainAreaMsg;
                                ValidAccountPropertyStatementList.Add(Validmodel);
                            }
                        }

                        if (!string.IsNullOrEmpty(mappModel.Percentage) && row[mappModel.Percentage] != null && row[mappModel.Percentage].ToString() != "")
                        {
                            try
                            {
                                Percentage = Convert.ToDecimal(row[mappModel.Percentage].ToString());
                            }
                            catch (Exception)
                            {
                                Percentage = 0;
                                var Validmodel = new ValidAccountPropertyStatement();
                                Validmodel.GroupCode = row[mappModel.GroupCode].ToString();
                                Validmodel.Reason = Resources.AccountProperty.AllowOnlyNumericInPercentageMsg;
                                ValidAccountPropertyStatementList.Add(Validmodel);
                            }
                        }

                        if (!string.IsNullOrEmpty(mappModel.CurrentAge) && row[mappModel.CurrentAge] != null && row[mappModel.CurrentAge].ToString() != "")
                        {
                            try
                            {
                                CurrentAge = Convert.ToInt32(row[mappModel.CurrentAge].ToString());
                            }
                            catch (Exception)
                            {
                                CurrentAge = 0;
                                var Validmodel = new ValidAccountPropertyStatement();
                                Validmodel.GroupCode = row[mappModel.GroupCode].ToString();
                                Validmodel.Reason = Resources.AccountProperty.AllowOnlyNumericInCurrentAgeMsg;
                                ValidAccountPropertyStatementList.Add(Validmodel);
                            }
                        }

                        if (!string.IsNullOrEmpty(mappModel.BuildingArea) && row[mappModel.BuildingArea] != null && row[mappModel.BuildingArea].ToString() != "")
                        {
                            try
                            {
                                BuildingArea = Convert.ToDecimal(row[mappModel.BuildingArea].ToString());
                            }
                            catch (Exception)
                            {
                                BuildingArea = 0;
                                var Validmodel = new ValidAccountPropertyStatement();
                                Validmodel.GroupCode = row[mappModel.GroupCode].ToString();
                                Validmodel.Reason = Resources.AccountProperty.AllowOnlyNumericInBuildingAreaMsg;
                                ValidAccountPropertyStatementList.Add(Validmodel);
                            }
                        }

                        var obj = new ImportViewAccountPropertyModel()
                        {
                            GroupCode = row[mappModel.GroupCode].ToString(),
                            RightNumber = row[mappModel.RightNumber].ToString(),
                            Duplicate = row[mappModel.Duplicate].ToString(),
                            Horizontal = row[mappModel.Horizontal].ToString(),
                            RightCode = row[mappModel.RightCode].ToString(),
                            TaxNumber = row[mappModel.TaxNumber].ToString(),
                            TerrainArea = TerrainArea,
                            Percentage = Percentage,
                            MaterialType = row[mappModel.MaterialType].ToString(),
                            ConstructionType = row[mappModel.ConstructionType].ToString(),
                            CurrentAge = CurrentAge,
                            BuildingArea = BuildingArea
                        };
                        list.Add(obj);
                    }
                }
                ValidAccountPropertyStatementList = (from c in ValidAccountPropertyStatementList
                                                     group c by new
                                                     {
                                                         c.GroupCode,
                                                         c.Reason,
                                                     } into gml
                                                     select new ValidAccountPropertyStatement()
                                                     {
                                                         GroupCode = gml.Key.GroupCode,
                                                         Reason = gml.Key.Reason,
                                                     }).ToList();

                ImportViewAccountPropertyDetailWithErrorModel _model = new ImportViewAccountPropertyDetailWithErrorModel();
                _model.ImportList = list;
                _model.ErrorList = ValidAccountPropertyStatementList;

                return _model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ImportViewBankPaymentsModel> GetBankPaymentsListFromImportDatatTable(DataTable table, BankPaymentsMappingViewModel mappModel)
        {
            try
            {
                var list = new List<ImportViewBankPaymentsModel>(table.Rows.Count);

                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {
                        var obj = new ImportViewBankPaymentsModel()
                        {
                            ImportID = Convert.ToInt32(row[mappModel.ImportID]),
                            PERIODOREC = row[mappModel.PERIODOREC].ToString(),
                            Contract = row[mappModel.Contract].ToString(),
                            TaxNumber = row[mappModel.TaxNumber].ToString(),
                            PaymentDate = row[mappModel.PaymentDate].ToString(),
                            Amount = row[mappModel.Amount].ToString(),
                            ReceiptNumber = row[mappModel.ReceiptNumber].ToString()
                        };
                        list.Add(obj);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ValidImportMeasuredWaterFilingFixFileModel> GetMeasuredWaterFilingListFromValidateDatatTable(DataTable table, MeasuredWaterFilingMappingViewModel mappModel)
        {
            try
            {
                var list = new List<ValidImportMeasuredWaterFilingFixFileModel>(table.Rows.Count);

                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {
                        var obj = new ValidImportMeasuredWaterFilingFixFileModel()
                        {
                            RowNo = Convert.ToInt32(row[mappModel.RowNo]),                        
                            UBICACION = row[mappModel.UBICACION].ToString(),
                            CODIGOM = row[mappModel.CODIGOM].ToString(),
                            CATEGORIA = row[mappModel.CATEGORIA].ToString(),
                            LECTURAACT = row[mappModel.LECTURAACT].ToString(),
                            FECHA = row[mappModel.FECHA].ToString(),
                            TaxNumber = row[mappModel.TaxNumber].ToString(),
                            LastReading = row[mappModel.LastReading].ToString(),
                            Difference = row[mappModel.Difference].ToString(),
                            Note = row[mappModel.Note].ToString(),
                            IsValid = row[mappModel.IsValid].ToString()
                        };
                        list.Add(obj);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ImportViewMeasuredWaterFilingModel> GetMeasuredWaterFilingListFromImportDatatTable(DataTable table, MeasuredWaterFilingMappingViewModel mappModel, bool isForFixFile)
        {
            try
            {
                var list = new List<ImportViewMeasuredWaterFilingModel>(table.Rows.Count);

                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {
                        var obj = new ImportViewMeasuredWaterFilingModel()
                        {
                            RowNo = Convert.ToInt32(row[mappModel.RowNo]),
                            Year = string.IsNullOrEmpty(mappModel.Year) ? null : row[mappModel.Year].ToString(),
                            Month = string.IsNullOrEmpty(mappModel.Month) ? null : row[mappModel.Month].ToString(),
                            UBICACION = row[mappModel.UBICACION].ToString(),
                            CODIGOM = row[mappModel.CODIGOM].ToString(),
                            CATEGORIA = row[mappModel.CATEGORIA].ToString(),
                            LECTURAACT = row[mappModel.LECTURAACT].ToString(),
                            FECHA = row[mappModel.FECHA].ToString(),
                            TaxNumber = row[mappModel.TaxNumber].ToString(),
                            LastReading = row[mappModel.LastReading].ToString(),
                            Difference = row[mappModel.Difference].ToString(),
                            IsValid = isForFixFile ? null : row[mappModel.IsValid].ToString()
                            //IsEditable = row[mappModel.IsEditable].ToString()
                        };
                        list.Add(obj);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool CheckDataTableEmptyRow(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                var isEmpty = row.ItemArray.All(c => string.IsNullOrEmpty(c.ToString()));
                if (!isEmpty)
                    return false;
            }
            return true;
        }
    }
}
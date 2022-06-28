using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class ImportBankPaymentsModel
    {
        #region public properties        
        public static string[] ContractColumn = new string[] { "Contract" };
        public static string[] TaxNumberColumn = new string[] { "TaxNumber", "Tax Number" };
        public static string[] PERIODORECColumn = new string[] { "PERIODOREC" };
        public static string[] PaymentDateColumn = new string[] { "PaymentDate", "Payment Date" };
        public static string[] AmountColumn = new string[] { "Amount" };
        public static string[] ReceiptNumberColumn = new string[] { "ReceiptNumber", "Receipt Number" };

        public const string S_ImportBankPaymentsData = "dtBankPaymentsSession";
        #endregion

        #region public methods

        public DataTable ImportBankPaymentsValid(ValidBankPaymentsModel ImportViewBankPayments)
        {
            var url = "api/Payment/ValidateImportBankPayments";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewBankPayments);
            List<ValidBankPaymentsStatement> model1 = new List<ValidBankPaymentsStatement>();
            return objRestSharpHandler.RestRequest<DataTable>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
        }

        public int ImportBankPaymentsInsert(ValidBankPaymentsModel ImportViewBankPayments)
        {
            if (ImportViewBankPayments.ImportList != null && ImportViewBankPayments.ImportList.Rows.Count > 0)
            {
                ImportViewBankPayments.Contract = ImportViewBankPayments.ImportList.Rows[0][1].ToString();
            }

            var url = "api/Payment/InsertImportBankPayments";
            var objRestSharpHandler = new RestSharpHandler();
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(ImportViewBankPayments);

            var result = objRestSharpHandler.RestRequest<int>(APISelector.Municipality, true, url, "POST", null, lstObjParameter);
            return result;
        }

        #endregion
    }
    public class ImportViewBankPaymentsModel
    {
        #region public properties
        public int ImportID { get; set; }
        public string Contract { get; set; }
        public string TaxNumber { get; set; }
        public string PERIODOREC { get; set; }
        public string PaymentDate { get; set; }
        public string Amount { get; set; }
        public string ReceiptNumber { get; set; }
        #endregion
    }
    public class ImportBankPaymentsFieldModel
    {
        #region public properties

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(ArtSolutions.MUN.Web.Resources.Global))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
        public string FileName { get; set; }
        public int type { get; set; }
        public int PaymentOptionID { get; set; }
        public IEnumerable<SelectListItem> PaymentOptions { get; set; }
        #endregion
    }
    public class ValidBankPaymentsStatement
    {
        #region public properties
        public int ImportID { get; set; }
        public string PERIODOREC { get; set; }
        public string Contract { get; set; }
        public string TaxNumber { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
        public int? ServiceID { get; set; }
        public string Segrega { get; set; }

        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int? PaymentID { get; set; }
        public decimal? Payment { get; set; }
        #endregion
    }
    public class BankPaymentsMappingViewModel
    {
        #region public properties
        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public int ImportID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Contract { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string TaxNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string PERIODOREC { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string PaymentDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string Amount { get; set; }
        public string ReceiptNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Resources.Global))]
        public string FileName { get; set; }
        public IEnumerable<SelectListItem> HeaderColumnList { get; set; }
        public IEnumerable<SelectListItem> DateFormatList { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentOptionID { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }

        #endregion

        #region Constructor
        public BankPaymentsMappingViewModel()
        {
            HeaderColumnList = new List<SelectListItem>();
        }
        public BankPaymentsMappingViewModel(string fileName, BankPaymentReadImportFileModel model)
        {
            this.Note = model.Note;
            this.Date = model.Date;
            this.PaymentOptionID = model.PaymentOptionID;
            this.TotalLinesInFileReceived = model.TotalLinesInFileReceived;
            this.TotalLinesWithPayments = model.TotalLinesWithPayments;
            this.TotalCommission = model.TotalCommission;
            this.TotalAmount = model.TotalAmount;
            this.FileName = fileName;
            this.HeaderColumnList = new SelectList(model.BankPaymentList.Columns.Cast<DataColumn>()
                      .Select(x => new { Name = x.ColumnName }).ToList(), "Name", "Name");

            if (this.HeaderColumnList != null && this.HeaderColumnList.Any())
            {
                this.Contract = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.ContractColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.ContractColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.TaxNumber = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                           this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.TaxNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                           : "";

                this.PERIODOREC = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.PERIODORECColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.PERIODORECColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.PaymentDate = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.PaymentDateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                          this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.PaymentDateColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                          : "";

                this.Amount = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.AmountColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                 this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.AmountColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                 : "";
                this.ReceiptNumber = this.HeaderColumnList.Any(a => ImportBankPaymentsModel.ReceiptNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())) ?
                                this.HeaderColumnList.FirstOrDefault(a => ImportBankPaymentsModel.ReceiptNumberColumn.Any(b => b.Trim().ToLower() == a.Value.Trim().ToLower())).Value
                                : "";
            }
        }
        #endregion
    }
    public class InvalidBankPaymentsModel
    {
        #region public properties
        public bool Valid { get; set; }
        public string FileName { get; set; }
        public DataTable ValidBankPaymentsStatement { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentOptionID { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsValid { get; set; }
        #endregion
    }
    public class ValidBankPaymentsModel
    {
        #region public properties
        public int CompanyID { get; set; }
        public bool IsValid { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DataTable ImportList { get; set; }
        public string Note { get; set; }
        public string FileName { get; set; }
        public string Contract { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentOptionID { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }
        #endregion

        #region Constructor
        public ValidBankPaymentsModel()
        { }
        public ValidBankPaymentsModel(DataTable importDataTable, BankPaymentsMappingViewModel model)
        {
            this.CreatedDate = this.ModifiedDate = Common.CurrentDateTime;
            this.CreatedUser = this.ModifiedUser = UserSession.Current.Id;
            this.ImportList = importDataTable;
            this.Note = model.Note;
            this.Date = model.Date;
            this.FileName = model.FileName;
            this.PaymentOptionID = model.PaymentOptionID;
            this.TotalLinesInFileReceived = model.TotalLinesInFileReceived;
            this.TotalLinesWithPayments = model.TotalLinesWithPayments;
            this.TotalCommission = model.TotalCommission;
            this.TotalAmount = model.TotalAmount;
        }
        #endregion
    }
    public class FinishViewBankPaymentsModel
    {
        #region public properties
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentOptionID { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<string> HeaderColumnList { get; set; } = new List<string>();
        public DataTable ImportList { get; set; }

        #endregion

        #region Constructor
        public FinishViewBankPaymentsModel()
        { }

        public FinishViewBankPaymentsModel(DataTable importDataTable, BankPaymentsMappingViewModel model)
        {
            this.HeaderColumnList = importDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.Trim()).ToList();
            this.ImportList = importDataTable;
            this.Note = model.Note;
            this.Date = model.Date;
            this.PaymentOptionID = model.PaymentOptionID;
            this.TotalLinesInFileReceived = model.TotalLinesInFileReceived;
            this.TotalLinesWithPayments = model.TotalLinesWithPayments;
            this.TotalCommission = model.TotalCommission;
            this.TotalAmount = model.TotalAmount;
        }
        #endregion
    }
    public class BankPaymentReadImportFileModel
    {
        public DataTable BankPaymentList { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public int? PaymentOptionID { get; set; }
        public string FileName { get; set; }
        public int? TotalLinesInFileReceived { get; set; }
        public int? TotalLinesWithPayments { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsValidFile { get; set; }
    }
}
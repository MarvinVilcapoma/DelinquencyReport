using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class Payment
    {
        #region Payment
        public PaymentListModel GetWithPaging(int companyID, int? accountID, int? iD, int? year, string filterText, bool? isVoid, string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                PaymentListModel model = new PaymentListModel();
                model.PaymentList = context.MUNCOLPaymentGetWithPaging(companyID, accountID, iD, year, filterText, isVoid, language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public List<MUNCOLPaymentGetForClosingEntry_Result> GetForClosingEntry(int companyID, int? accountID, int? iD, Guid? cashierID, DateTime? ClosingDate, string language, bool? OnlyPostedReceipts, int? PaymentOptionID)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {

                return context.MUNCOLPaymentGetForClosingEntry(companyID, accountID, iD, cashierID, ClosingDate, language, OnlyPostedReceipts, PaymentOptionID).ToList();
            }
        }
        public List<MUNCOLPaymentGetAsObject_Result> GetAsObject(int companyID, int? accountID, int? iD, string filterText, bool? isVoid, string language, bool? isPost)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNCOLPaymentGetAsObject(companyID, accountID, iD, filterText, isVoid, language, isPost).ToList();
            }
        }
        public PaymentPrintModel GetPaymentPlanPrint(int iD, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                PaymentPrintModel model = new PaymentPrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLPaymentPlanPrint");
                model.Company = ds.Tables[0].ToList<PaymentCompanyModel>().FirstOrDefault() ?? new PaymentCompanyModel();
                model.Account = ds.Tables[1].ToList<PaymentAccountModel>().FirstOrDefault() ?? new PaymentAccountModel();
                model.Payment = ds.Tables[2].ToList<PaymentModel>().FirstOrDefault() ?? new PaymentModel();
                model.PaymentPlanDetailList = ds.Tables[3].ToList<AccountPaymentPlanDetailModel>() ?? new List<AccountPaymentPlanDetailModel>();
                model.PaymentOptionList = ds.Tables[4].ToList<PaymentOptionModel>() ?? new List<PaymentOptionModel>();
                model.PrintTemplate = ds.Tables[5].ToList<PrintTemplateModel>().FirstOrDefault() ?? new PrintTemplateModel();
                model.CreditNote = ds.Tables[6].ToList<CreditNoteModel>() ?? new List<CreditNoteModel>();
                model.DebitNote = ds.Tables[7].ToList<DebitNoteModel>() ?? new List<DebitNoteModel>();
                return model;
            }
        }
        public PaymentPrintModel GetPrint(int iD, int? serviceTypeID, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                PaymentPrintModel model = new PaymentPrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},
                    new SqlParameter { ParameterName = "@ServiceTypeID",Value= serviceTypeID  > 0 ? serviceTypeID : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLPaymentPrint");
                model.Company = ds.Tables[0].ToList<PaymentCompanyModel>().FirstOrDefault() ?? new PaymentCompanyModel();
                model.Account = ds.Tables[1].ToList<PaymentAccountModel>().FirstOrDefault() ?? new PaymentAccountModel();
                model.Payment = ds.Tables[2].ToList<PaymentModel>().FirstOrDefault() ?? new PaymentModel();
                //if (serviceTypeID > 0)
                if (model.Payment.PrintTemplateID != 6 && model.Payment.PrintTemplateID != 10 && model.Payment.PrintTemplateID!=11)
                {
                    model.AccountServiceCollectionDetail = ds.Tables[3].ToList<AccountServiceCollectionDetailModel>() ?? new List<AccountServiceCollectionDetailModel>();
                }
                else
                {
                    model.InvoiceDetail = ds.Tables[3].ToList<InvoiceDetailModel>() ?? new List<InvoiceDetailModel>();
                }

                model.PaymentOptionList = ds.Tables[4].ToList<PaymentOptionModel>() ?? new List<PaymentOptionModel>();
                model.PrintTemplate = ds.Tables[5].ToList<PrintTemplateModel>().FirstOrDefault() ?? new PrintTemplateModel();
                model.CreditNote = ds.Tables[6].ToList<CreditNoteModel>() ?? new List<CreditNoteModel>();
                model.DebitNote = ds.Tables[7].ToList<DebitNoteModel>() ?? new List<DebitNoteModel>();
                return model;
            }
        }
        public int InsertByInvoice(PaymentModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter paymentId = new ObjectParameter("PaymentID", typeof(int));
                context.MUNCOLPaymentInsertByInvoice(model.CompanyID, model.AccountID, model.Date, model.InvoiceID, model.NetPayment, model.Notes, model.AttachmentID,
                    model.PaymentOptionJSON, model.Number, model.NumberPrefix, model.IsManual, model.IsOfficialReceipt, model.CreatedUserID,
                    model.CreatedDate, model.Language, model.ApplyCreditAmount, paymentId);
                return int.Parse(paymentId.Value.ToString());
            }
        }
        public int InsertByOtherService(PaymentModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                ObjectParameter paymentId = new ObjectParameter("PaymentID", typeof(int));
                context.MUNCOLPaymentInsertByOtherService(model.CompanyID, model.AccountID, model.Date, model.Notes, model.NetPayment, model.AttachmentID,
                    model.PaymentOptionJSON, model.InvoiceDetailJson, model.Number, model.NumberPrefix, model.IsManual, model.IsOfficialReceipt, model.CreatedUserID,
                    model.CreatedDate, model.Language, model.ApplyCreditAmount, paymentId);
                return int.Parse(paymentId.Value.ToString());
            }
        }
        public int InsertByService(PaymentModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@PaymentID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID},
                    new SqlParameter { ParameterName = "@AccountID",Value=model.AccountID},
                    new SqlParameter { ParameterName = "@ServiceTypeID",Value = model.ServiceTypeID},
                    new SqlParameter { ParameterName = "@Date",Value=model.Date},
                    new SqlParameter { ParameterName = "@TotalPayment",Value=model.TotalPayment},
                    new SqlParameter { ParameterName = "@TotalDiscount",Value= model.TotalDiscount},
                    new SqlParameter { ParameterName = "@NetPayment",Value= model.NetPayment},
                    new SqlParameter { ParameterName = "@Notes",Value= string.IsNullOrEmpty(model.Notes) ? (object)DBNull.Value : model.Notes},
                    new SqlParameter { ParameterName = "@AttachmentID",Value=model.AttachmentID > 0 ? model.AttachmentID : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID},
                    new SqlParameter { ParameterName = "@CreatedDate",Value=model.CreatedDate},
                    new SqlParameter { ParameterName = "@Language",Value=model.Language},
                    new SqlParameter { ParameterName = "@PaymentOptionJSON",Value=model.PaymentOptionJSON},
                    new SqlParameter { ParameterName = "@Number",Value=!string.IsNullOrEmpty(model.Number) ? model.Number : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@NumberPrefix",Value= !string.IsNullOrEmpty(model.NumberPrefix) ? model.NumberPrefix : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@IsManual",Value=model.IsManual},
                    new SqlParameter { ParameterName = "@IsOfficialReceipt",Value=model.IsOfficialReceipt},
                    new SqlParameter { ParameterName = "@ApplyCreditAmount",Value=model.ApplyCreditAmount},
                    new SqlParameter { ParameterName = "@MUNCOLPaymentDetailUTD",Value=model.PaymentDetailList.ToDataTable(),SqlDbType = SqlDbType.Structured,TypeName="MUNCOLPaymentDetailUTD"},
                    new SqlParameter { ParameterName = "@ReferenceNumber",Value=DBNull.Value },
                    new SqlParameter { ParameterName = "@IsIVAApply",Value = model.IVAPayment },
                    new SqlParameter { ParameterName = "@ApplybyAmnesty",Value = model.ApplybyAmnesty },
                    new SqlParameter { ParameterName = "@AmnestyAmount",Value = model.AmnestyAmount },
                    outputparam
                };
                context.ExecuteSqlProcedure(sqlparameters, "MUNCOLPaymentInsertByService");
                return (int)outputparam.Value;
            }
        }
        public int InsertByPaymentPlan(PaymentModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                SqlParameter outputparam = new SqlParameter { ParameterName = "@PaymentID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID},
                    new SqlParameter { ParameterName = "@AccountID",Value=model.AccountID},
                    new SqlParameter { ParameterName = "@Date",Value=model.Date},
                    new SqlParameter { ParameterName = "@TotalPayment",Value=model.TotalPayment},
                    new SqlParameter { ParameterName = "@TotalDiscount",Value= model.TotalDiscount},
                    new SqlParameter { ParameterName = "@NetPayment",Value= model.NetPayment},
                    new SqlParameter { ParameterName = "@Notes",Value= string.IsNullOrEmpty(model.Notes) ? (object)DBNull.Value : model.Notes},
                    new SqlParameter { ParameterName = "@AttachmentID",Value=model.AttachmentID > 0 ? model.AttachmentID : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID},
                    new SqlParameter { ParameterName = "@CreatedDate",Value=model.CreatedDate},
                    new SqlParameter { ParameterName = "@Language",Value=model.Language},
                    new SqlParameter { ParameterName = "@PaymentOptionJSON",Value=model.PaymentOptionJSON},
                    new SqlParameter { ParameterName = "@Number",Value=!string.IsNullOrEmpty(model.Number) ? model.Number : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@NumberPrefix",Value= !string.IsNullOrEmpty(model.NumberPrefix) ? model.NumberPrefix : (object)DBNull.Value },
                    new SqlParameter { ParameterName = "@IsManual",Value=model.IsManual},
                    new SqlParameter { ParameterName = "@IsOfficialReceipt",Value=model.IsOfficialReceipt},
                    new SqlParameter { ParameterName = "@ApplyCreditAmount",Value=model.ApplyCreditAmount},
                    new SqlParameter { ParameterName = "@IsRemoveInterest",Value=model.IsRemoveInterest},
                    new SqlParameter { ParameterName = "@MUNCOLPaymentDetailUTD",Value=model.PaymentDetailList.ToDataTable(),SqlDbType = SqlDbType.Structured,TypeName="MUNCOLPaymentDetailUTD"},
                    outputparam
                };
                context.ExecuteSqlProcedure(sqlparameters, "MUNCOLPaymentInsertByPaymentPlan");
                return (int)outputparam.Value;
            }
        }
        public int InsertByDebitNote(PaymentModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.Database.CommandTimeout = 3000;
                ObjectParameter paymentId = new ObjectParameter("PaymentID", typeof(int));
                context.MUNCOLPaymentInsertByDebitNote(model.CompanyID, model.AccountID, model.Date, model.Notes, model.NetPayment, model.AttachmentID,
                    model.PaymentOptionJSON, model.DebitNotesJson, model.Number, model.NumberPrefix, model.IsManual, model.IsOfficialReceipt, model.CreatedUserID,
                    model.CreatedDate, model.Language, model.ApplyCreditAmount, paymentId);
                return int.Parse(paymentId.Value.ToString());
            }
        }
        public bool Void(PaymentModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNCOLPaymentVoid(model.ID, model.CompanyID, model.VoidReason, model.RowVersion, model.ModifiedUserID, model.ModifiedDate);
                return true;
            }
        }
        public bool PaymentPlanVoid(PaymentModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNCOLPaymentPlanVoid(model.ID, model.CompanyID, model.VoidReason, model.RowVersion, model.ModifiedUserID, model.ModifiedDate);
                return true;
            }
        }
        public decimal GetAvailableCreditBalance(int AccountID, int CompanyID)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNCOLPaymentGetAvailableCreditBalance(CompanyID, AccountID).FirstOrDefault().Value;
            }
        }
        #endregion

        #region Import Bank Payments
        public DataTable ImportBankPaymentsValidation(int companyID, string language, BankPaymentsImportModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };

                SqlParameter[] sqlparameters = {
                         new SqlParameter { ParameterName = "@CompanyID", Value=companyID,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@IsValid", Value=model.IsValid,SqlDbType = SqlDbType.Bit}
                        ,new SqlParameter { ParameterName = "@IsSave", Value=false,SqlDbType = SqlDbType.Bit}
                        ,new SqlParameter {ParameterName = "@MUNIMPBankPayments",Value=model.ImportList,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPBankPaymentsImport"}
                        ,outParam,
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPValidateBankPaymentsImport"); //, "MUNIMPBankPaymentsImport");
                return ds.Tables[0];
            }
        }
        public int ImportBankPaymentsInsert(int companyID, string language, BankPaymentsImportModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                model.Note = model.Note.Replace("\r\n", "</br>");
                int output = 1;
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                SqlParameter[] sqlparameters = {
                         new SqlParameter { ParameterName = "@CompanyID", Value=companyID,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@Note",Value= (string.IsNullOrEmpty(model.Note) ? "" : model.Note),SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@Date", Value=model.Date != null ? model.Date.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@PaymentOptionID", Value=model.PaymentOptionID > 0 ? model.PaymentOptionID.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@FileName", Value=(string.IsNullOrEmpty(model.FileName) ? "" : model.FileName),SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@PaymentContract", Value=(string.IsNullOrEmpty(model.Contract) ? "" : model.Contract),SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@TotalLinesInFileReceived", Value=model.TotalLinesInFileReceived > 0 ? model.TotalLinesInFileReceived.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@TotalLinesWithPayments", Value=model.TotalLinesWithPayments > 0 ? model.TotalLinesWithPayments.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@TotalCommission", Value=model.TotalCommission > 0 ? model.TotalCommission.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.Decimal}
                        ,new SqlParameter { ParameterName = "@TotalAmount", Value=model.TotalAmount > 0 ? model.TotalAmount.Value :  (object)DBNull.Value,SqlDbType = SqlDbType.Decimal}
                        ,new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@ModifiedUser",Value=model.ModifiedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@ModifiedDate", Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@MUNIMPBankPayments",Value=model.ImportList,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPBankPaymentsImport"}
                        ,outParam
                };
                context.ExecuteSqlProcedureWithOutputParam(sqlparameters, "MUNIMPBankPaymentsImport");
                output = (int)outParam.Value;
                return output;
            }
        }
        #endregion
    }
}
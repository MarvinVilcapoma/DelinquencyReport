using ArtSolutions.MUN.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountService
    {
        private static readonly System.Object lockObject = new System.Object();

        #region Account Service 
        public AccountServiceList Get(int companyID, string language, string filterText, string serviceTypeIDs, int? accountId, int? accountPropertyID, string serviceIDs, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountServiceList list = new AccountServiceList();

                list.AccountServiceModelList = context.MUNSERAccountServiceGetWithPaging(companyID, language, filterText, serviceTypeIDs, accountId, accountPropertyID, serviceIDs, pageIndex, pageSize, TotalRecord, sortColumn, sortOrder).ToList();
                list.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return list;
            }
        }
        public List<MUNSERAccountServiceGet_Result> Get(int companyId, int? accountServiceID, string language, string filterText, int? accountId, int? fiscalYearID, bool? isLock)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceGet(companyId, accountServiceID, language, filterText, accountId, fiscalYearID, isLock).ToList();
            }
        }
        public List<MUNSERAccountServiceGetForTransfer_Result> GetAllAccountService(int companyId, string language, int accountId, int accountPropertyId, int transferTypeID, int TransferID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceGetForTransfer(companyId, language, accountId, accountPropertyId, transferTypeID, TransferID).ToList();
            }
        }

        public List<MUNSERAccountServiceGetForTimbre_Result> GetAccountServiceForTimbre(int companyId, string language, int accountId, int fiscalYearId, int serviceID, int? accountPropertyID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceGetForTimbre(companyId, language, accountId, fiscalYearId, serviceID, accountPropertyID).ToList();
            }
        }
        public List<MUNSERAccountServiceGetNotFiled_Result> GetNotFiled(int companyId, int accountId, int fiscalYearID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceGetNotFiled(companyId, accountId, fiscalYearID, language).ToList();
            }
        }
        public MUNSERAccountServiceUpdate_Result Update(AccountServiceStatusModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceUpdate(model.CompanyID, model.ID, model.IssueDate, model.IsIssued, model.IssuedBy, model.IsPrint, model.PrintDate, model.IsLock, model.IsVoid, model.IsActive, model.Notes, model.ModifiedUserID, model.ModifiedDate, model.RowVersion).FirstOrDefault();
            }
        }

        public void Delete(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountServiceDelete(model.ID, model.ReasonForDeleted, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
            }
        }

        public void DeleteNote(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountServiceNoteDelete(model.ID, model.ReasonForDeleted, model.IsAccountServiceNote, model.ModifiedUserID, model.ModifiedDate);
            }
        }
        public int UpdateForCustomField(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceUpdateForCustomField(model.CompanyID, model.ID, model.AccountID, model.ServiceID,
                model.Year, model.CustomField1, model.CustomField2, model.CustomField3, model.CustomField4, model.CustomField5, model.CustomDateField,
                model.CustomField1NewValue, model.CustomField2NewValue, model.CustomField3NewValue, model.CustomField4NewValue, model.CustomField5NewValue,
                model.CustomDateFieldNewValue, model.ModifiedUserID, model.ModifiedDate, model.CustomField1UpdateDate, model.RowVersion);
            }
        }
        public int UpdateForRight(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceUpdateForRight(model.CompanyID, model.ID, model.AccountPropertyID
                    , model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
            }
        }
        public int UpdateForLicenseNumber(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceUpdateForLicenseNumber(model.CompanyID, model.ID, model.LicenseNumber
                    , model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
            }
        }
        public int Insert(AccountServiceModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter AccountServiceID = new ObjectParameter("AccountServiceID", typeof(int));
                context.Database.CommandTimeout = 3000;
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNSERAccountServiceInsert(model.CompanyID, model.ServiceID, model.Year, model.AccountID, model.ServiceExceptionID, model.ServiceExceptionValue, model.FilingDueDate, model.PaymentDueDate, model.CustomField1, model.CustomField2, model.CustomField3, model.CustomField4, model.CustomField5, model.CustomDateField, model.AccountPropertyID, model.ServiceStartPeriod, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate, AccountServiceID, false, model.LicenseAccountServiceID, null, model.PreviousMeasure, model.CreatedBy);
                return (int)AccountServiceID.Value;
            }
        }
        public List<MUNSERAccountServiceGetAsObject_Result> GetAsObject(int companyId, int? accountServiceID, string language, string filterText, int? licenceGroupId, int? ivuServiceId, int? accountId, int? fiscalYearID, bool showAll, bool? isLock, int? id, bool? isPost)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceGetAsObject(companyId, accountServiceID, language, filterText, licenceGroupId, ivuServiceId, accountId, fiscalYearID, showAll, isLock, id, isPost).ToList();
            }
        }
        #endregion

        #region Notes
        public List<MUNSERAccountServiceNoteGet_Result> NoteGet(int companyID, string language, int accountServiceID, int? id)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceNoteGet(companyID, language, id, accountServiceID).ToList();
            }
        }
        public int NoteInsert(AccountServiceNoteModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceNoteInsert(model.CompanyID, model.AccountServiceID, model.Note, model.CreatedBy, model.CreatedUserID, model.CreatedDate);
            }
        }
        #endregion

        #region Issue and Re-Print
        public void Issue(int companyID, int id, DateTime issueDate, Guid issuedBy, Guid modifiedUserID, DateTime modifiedDate, byte[] original_rowVersion)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountServiceIssue(companyID, id, issueDate, issuedBy, modifiedUserID, modifiedDate, original_rowVersion);
            }
        }
        public AccountServicePrintModel GetPrint(int companyId, string language, int? accountServiceID, int? printTemplateId)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                AccountServicePrintModel model = new AccountServicePrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value= companyId},
                    new SqlParameter { ParameterName = "@Language",Value=language},
                    new SqlParameter { ParameterName = "@AccountServiceID",Value=  accountServiceID > 0 ? accountServiceID:(object)DBNull.Value},
                    new SqlParameter { ParameterName = "@PrintTemplateId",Value= printTemplateId > 0 ? printTemplateId:(object)DBNull.Value}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERAccountServiceGetPrint");
                model.Company = ds.Tables[0].ToList<CompanyModel>().FirstOrDefault() ?? new CompanyModel();
                model.AccountService = ds.Tables[1].ToList<AccountServiceModel>().FirstOrDefault() ?? new AccountServiceModel();
                model.AccountAddress = ds.Tables[2].ToList<AccountAddressForPrint>().FirstOrDefault() ?? new AccountAddressForPrint();
                model.PrintTemplate = ds.Tables[3].ToList<PrintTemplateModel>().SingleOrDefault() ?? new PrintTemplateModel();
                model.AccountBusiness = ds.Tables[4].ToList<AccountBusinessModel>().SingleOrDefault() ?? new AccountBusinessModel();
                model.AccountServiceCollection = ds.Tables[5].ToList<AccountServiceCollectionPrintModel>().SingleOrDefault() ?? new AccountServiceCollectionPrintModel();
                model.AccountServiceDebtList = ds.Tables[6].ToList<AccountServiceCollectionPrintModel>() ?? new List<AccountServiceCollectionPrintModel>();
                return model;
            }
        }
        public void Print(int companyID, int id, DateTime printDate, Guid modifiedUserID, DateTime modifiedDate, byte[] original_rowVersion)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountServicePrint(companyID, id, printDate, modifiedUserID, modifiedDate, original_rowVersion);
            }
        }
        #endregion

        #region Lock
        public void Lock(int companyID, int id, bool isLock, int lockReasonTableValue, int lockActionTableValue, string lockComment, string createdBy, Guid modifiedUserID, DateTime modifiedDate, byte[] original_rowVersion)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                //Pending createdBy parameter
                context.MUNSERAccountServiceLock(companyID, id, isLock, lockReasonTableValue, lockActionTableValue, lockComment, modifiedUserID, modifiedDate, createdBy, original_rowVersion);
            }
        }
        #endregion

        #region Adjustment
        public void ProcessAdjustment(AccountServiceAdjustmentModel model, int companyId, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountServiceAdjustmentsBulkInsert(companyId, model.AccountServiceId, model.AccountServiceCollectionDetailId, model.ServiceCollectionRuleId, model.Amount, model.Reason, language, model.CreatedUserID, model.CreatedDate, false);
            }
        }
        public List<MUNSERAccountServiceAdjustmentsGet_Result> AdjustmentsGet(int companyID, int accountserviceID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceAdjustmentsGet(companyID, accountserviceID, language).ToList();

            }
        }
        public void DeleteAdjustment(AccountServiceAdjustmentModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                context.MUNSERAccountServiceAdjustmentDelete(model.ID, model.CreatedUserID, model.CreatedDate);
            }
        }
        #endregion

        #region Exemption
        public void ProcessExemption(AccountServiceExemptionModel model, int companyId, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                context.MUNSERAccountServiceExemptionBulkInsert(companyId, model.AccountServiceId, model.AccountServiceCollectionDetailId, model.Amount, model.Reason, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void ExemptionUpdate(AccountServiceExemptionModel model, int companyId, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                context.MUNSERAccountServiceExemptionUpdate(companyId, model.ID, model.AccountServiceId, model.AccountServiceCollectionDetailId, model.Amount, model.Reason, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteExemption(AccountServiceExemptionModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                context.MUNSERAccountServiceExemptionDelete(model.ID, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteAllExemption(AccountServiceExemptionModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 5000;
                context.MUNSERAccountServiceExemptionDeleteAll(model.AccountServiceId, model.CreatedUserID, model.CreatedDate);
            }
        }

        public List<MUNSERAccountServiceExemptionGet_Result> ExemptionGet(int companyID, int accountserviceID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                return context.MUNSERAccountServiceExemptionGet(companyID, accountserviceID, language).ToList();

            }
        }

        public List<MUNSERAccountServiceExemptionHistoryGet_Result> ExemptionHistoryGet(int accountserviceID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                return context.MUNSERAccountServiceExemptionHistoryGet(accountserviceID, language).ToList();

            }
        }
        #endregion

        #region Correction
        public void ProcessCorrection(AccountServiceCorrectiontModel model, int companyId, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                context.MUNSERAccountServiceCorrection(companyId, model.AccountServiceId, model.FillingDate, model.Area, model.TerrainValue, model.MovementTypeID, model.TotalValue, model.ExemptionPeriod, model.ExemptionAmount, model.PendingPeriod, model.AdjustmentAmount, model.Reason, language, model.PropertyTaxJson, model.CreatedUserID, model.CreatedDate);
            }
        }
        #endregion

        #region Extension
        public MUNSERAccountServiceExtensionGet_Result ExtensionGet(int companyID, int accountserviceID, int ID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceExtensionGet(companyID, accountserviceID, ID).FirstOrDefault();
            }
        }
        public int ExtensionInsert(AccountServiceExtensionModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter outparam = new ObjectParameter("ID", typeof(int));
                context.MUNSERAccountServiceExtensionInsert(model.CompanyID, model.AccountServiceID, model.StartDate, model.EndDate, model.Months
                    , model.GrossVolume, model.ExemptionAmount, model.Total, model.CreditAmount, model.ImageID == 0 ? null : model.ImageID, model.IsActive, model.IsDeleted, model.CreatedUserID, model.CreatedDate
                    , model.ModifiedUserID, model.ModifiedDate, outparam);
                return int.Parse(outparam.Value.ToString());
            }
        }
        #endregion

        #region Service Group
        public List<MUNSERServiceTypeGroupGet_Result> ServiceTypeGroupGet(int companyId, string language, int? id)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERServiceTypeGroupGet(companyId, language, id).ToList();
            }
        }
        #endregion

        #region Service Type
        public List<MUNSERServiceTypeGet_Result> ServiceTypeGet(int companyId, string language, int groupId, int? id)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERServiceTypeGet(companyId, language, groupId, id).ToList();
            }
        }
        public List<MUNSERAccountServiceTypeGetNotPaid_Result> ServiceTypeGetNotPaid(int? accountID, bool? checkActivePaymentPlan, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                return context.MUNSERAccountServiceTypeGetNotPaid(accountID, checkActivePaymentPlan, companyID, language).ToList();
            }
        }
        public List<MUNREPJournalDetailInvoiceItemGet_Result> ServiceTypeGetByNoFilingType(int companyId, string language, bool isNoFilingType)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNREPJournalDetailInvoiceItemGet(companyId, language, isNoFilingType).ToList();
            }
        }
        #endregion

        #region Account Type
        public List<MUNAccountTypeGet_Result> AccountTypeGet(Guid ApplicationID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountTypeGet(ApplicationID, language).ToList();
            }
        }
        #endregion

        #region Fiscal Year
        public List<MUNSERFiscalYearGet_Result> FiscalYearGet()
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERFiscalYearGet().ToList();
            }
        }
        public List<MUNSERFiscalYearGetByService_Result> FiscalYearGetByService(int serviceId)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERFiscalYearGetByService(serviceId).ToList();
            }
        }
        public List<MUNSERFiscalYearGetByServiceNotInAccount_Result> FiscalYearGetByServiceNotInAccount(int serviceID, int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERFiscalYearGetByServiceNotInAccount(serviceID, accountID).ToList();
            }
        }
        public List<MUNSERFiscalYearGetByAccount_Result> FiscalYearGetByAccount(int accountID, int companyID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERFiscalYearGetByAccount(accountID, companyID).ToList();
            }
        }
        public List<MUNSERFiscalYearGetByAccountNotFiled_Result> FiscalYearGetByAccountNotFiled(int accountID, int companyID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERFiscalYearGetByAccountNotFiled(accountID, companyID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection - PROPERTY TAX
        public List<MUNSERAccountServiceCollectionPropertyTaxDetailGetNotPaid_Result> CollectionPropertyTaxDetailGetNotPaid(int AccountPropertyID, int serviceTypeID, int AccountPropertyRightID, DateTime? paymentDate, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionPropertyTaxDetailGetNotPaid(AccountPropertyID, serviceTypeID, AccountPropertyRightID, paymentDate, companyID, language).ToList();
            }
        }
        public MUNSERFillingPropertyTaxInsert_Result CollectionFillingPropertyTaxInsert(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingPropertyTaxInsert(companyID, model.AccountServiceCollectionID, model.PropertyAccountID, model.PropertyNumber, model.RigthNumber, model.Area, model.TerrainValue, model.TotalValue, model.MovementTypeID, model.Notes, model.IsFromPortal, model.FillingBy,
                     model.IsActive, model.IsDeleted, model.FillingDate, model.ExemptionAmount, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID,
                     model.ModifiedDate, model.RowVersion, model.CommaSeperatedImageIDs, model.PropertyTaxJson, language, model.ApplyToAllYear, false).FirstOrDefault();
            }
        }
        public MUNSERFillingPropertyTaxUpdate_Result CollectionFillingPropertyTaxUpdate(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingPropertyTaxUpdate(companyID, model.AccountServiceCollectionID, model.PropertyAccountID, model.PropertyNumber, model.RigthNumber, model.Area, model.TerrainValue, model.TotalValue, model.MovementTypeID, model.Notes, model.IsFromPortal, model.FillingBy, model.ExemptionAmount,
                    model.IsActive, model.IsDeleted, model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID,
                    model.ModifiedDate, model.RowVersion, model.CommaSeperatedImageIDs, model.PropertyTaxJson, language, model.ApplyToAllYear, false).FirstOrDefault();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingPropertyTaxGet_Result> CollectionFillingPropertyTaxGet(int companyID, int accountServiceCollectionID, int? fillingPropertyTaxID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingPropertyTaxGet(companyID, accountServiceCollectionID, fillingPropertyTaxID).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingPropertyTaxImagesGet_Result> CollectionFillingPropertyTaxImagesGet(int fillingID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingPropertyTaxImagesGet(fillingID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection - BUSINESS LICENSE
        public MUNSERFillingLicenseInsert_Result CollectionFillingLicenseInsert(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 30000;
                return context.MUNSERFillingLicenseInsert(companyID, model.AccountServiceCollectionID, model.GrossVolume,
                    model.ExemptionAmount, model.PercentageValue, model.Total, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public MUNSERFillingLicenseUpdate_Result CollectionFillingLicenseUpdate(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingLicenseUpdate(model.ID, companyID, model.AccountServiceCollectionID, model.GrossVolume,
                    model.ExemptionAmount, model.PercentageValue, model.Total, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingLicenseGet_Result> CollectionFillingLicenseGet(int companyID, int accountServiceCollectionID, int? fillingLicenseID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingLicenseGet(companyID, accountServiceCollectionID, fillingLicenseID).ToList();
            }
        }

        public List<MUNSERAccountServiceCollectionAutoFillingGet_Result> CollectionAutoFillingGet(int companyID, int accountServiceCollectionID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionAutoFillingGet(companyID, accountServiceCollectionID).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingLicenseImagesGet_Result> CollectionFillingLicenseImagesGet(int fillingID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingLicenseImagesGet(fillingID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection - TAX IVU
        public MUNSERFillingTaxInsert_Result CollectionFillingTaxInsert(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERFillingTaxInsert(companyID, model.AccountServiceCollectionID, model.FormIVUTreasury
                    , model.PurchasesImportsResale
                    , model.ImportsUse
                    , model.UseofInventory
                    , model.TotalUseSubjectIVU
                    , model.TaxableFurnitureSale
                    , model.TaxableServicesSale
                    , model.TaxableAdmissions
                    , model.TaxableItemsReturns
                    , model.TotalTaxableSales
                    , model.ExemptFurnitureSale
                    , model.ExemptServicesSale
                    , model.ExemptAdmissions
                    , model.ExemptReturns
                    , model.TotalExemptSales
                    , model.CreditSaleProperty
                    , model.CreditUncollectibleAccounts
                    , model.TaxCreditPaid, model.Total, model.IsFromPortal,
                    model.FillingBy, model.IsActive, model.IsDeleted, model.FillingDate, model.CreatedUserID, model.CreatedDate,
                    model.ModifiedUserID, model.ModifiedDate, model.RowVersion, model.CommaSeperatedImageIDs, language,
                    model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public MUNSERFillingTaxUpdate_Result CollectionFillingTaxUpdate(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERFillingTaxUpdate(model.ID, companyID, model.AccountServiceCollectionID, model.FormIVUTreasury
                    , model.PurchasesImportsResale
                    , model.ImportsUse
                    , model.UseofInventory
                    , model.TotalUseSubjectIVU
                    , model.TaxableFurnitureSale
                    , model.TaxableServicesSale
                    , model.TaxableAdmissions
                    , model.TaxableItemsReturns
                    , model.TotalTaxableSales
                    , model.ExemptFurnitureSale
                    , model.ExemptServicesSale
                    , model.ExemptAdmissions
                    , model.ExemptReturns
                    , model.TotalExemptSales
                    , model.CreditSaleProperty
                    , model.CreditUncollectibleAccounts
                    , model.TaxCreditPaid, model.Total, model.IsFromPortal,
                    model.FillingBy, model.IsActive, model.IsDeleted, model.FillingDate, model.CreatedUserID, model.CreatedDate,
                    model.ModifiedUserID, model.ModifiedDate, model.RowVersion, model.CommaSeperatedImageIDs, language,
                    model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingTaxGet_Result> CollectionFillingTaxGet(int companyID, int accountServiceCollectionID, int? fillingTaxID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingTaxGet(companyID, accountServiceCollectionID, fillingTaxID).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingTaxImagesGet_Result> CollectionFillingTaxImagesGet(int fillingID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingTaxImagesGet(fillingID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection - UNIT
        public MUNSERFillingUnitInsert_Result CollectionFillingUnitInsert(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingUnitInsert(companyID, model.AccountServiceCollectionID, model.Value,
                    model.UnitCost, model.Total, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public MUNSERFillingUnitUpdate_Result CollectionFillingUnitUpdate(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingUnitUpdate(model.ID, companyID, model.AccountServiceCollectionID, model.Value,
                    model.UnitCost, model.Total, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }
        public List<MUNSERAccountServiceCollectionFillingUnitGet_Result> CollectionFillingUnitGet(int companyID, int accountServiceCollectionID, int? fillingUnitID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingUnitGet(companyID, accountServiceCollectionID, fillingUnitID).ToList();
            }
        }
        public decimal CollectionFillingUnitRuleGet(int companyID, int accountServiceCollectionID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceFillingRuleGet(companyID, accountServiceCollectionID).FirstOrDefault().Value;
            }
        }
        public List<MUNSERAccountServiceCollectionFillingUnitImagesGet_Result> CollectionFillingUnitImagesGet(int fillingID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingUnitImagesGet(fillingID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection - MEASURED
        public MUNSERFillingMeasuredWaterInsert_Result CollectionFillingMeasuredWaterInsert(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingMeasuredWaterInsert(companyID, model.AccountServiceCollectionID, model.PreviousMeasure,
                    model.ActualMeasure, model.WaterConsumption, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }

        public List<string> CollectionFillingMeasuredWaterExport(int? accountID, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERAccountServiceFilingExportMeasuredWater(companyID, language, accountID).ToList();
            }
        }
        public MUNSERFillingMeasuredWaterUpdate_Result CollectionFillingMeasuredWaterUpdate(AccountServiceCollectionFillingModel model, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERFillingMeasuredWaterUpdate(model.ID, companyID, model.AccountServiceCollectionID, model.PreviousMeasure,
                    model.ActualMeasure, model.WaterConsumption, model.IsFromPortal, model.FillingBy, model.IsActive, model.IsDeleted,
                    model.FillingDate, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                    model.RowVersion, model.CommaSeperatedImageIDs, language, model.ApplyToAllYear).FirstOrDefault();
            }
        }

        public void DeleteFillingMeasuredWater(AccountServiceCollectionFillingModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERFillingMeasuredWaterDelete(model.CompanyID, model.AccountServiceCollectionID, model.Notes, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteFillingUnit(AccountServiceCollectionFillingModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERFillingUnitDelete(model.CompanyID, model.AccountServiceCollectionID, model.Notes, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteFillingPropertyTax(AccountServiceCollectionFillingModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                //context.MUNSERFillingUnitDelete(model.CompanyID, model.AccountServiceCollectionID, model.Notes, language, model.CreatedUserID, model.CreatedDate);
                context.MUNSERFillingPropertyTaxDelete(model.CompanyID, model.AccountServiceCollectionID, model.Notes, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteFillingLicense(AccountServiceCollectionFillingModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERFillingLicenseDelete(model.CompanyID, model.AccountServiceCollectionID, model.Notes, language, model.CreatedUserID, model.CreatedDate);
            }
        }

        public void DeleteFillingAutoFiling(int companyID, string language, int accountServiceCollectionID, string notes, Guid createdUserID, DateTime createdDate)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERFillingAutoFillingDelete(companyID, accountServiceCollectionID, notes, language, createdUserID, createdDate);
            }
        }
        public List<MUNSERAccountServiceCollectionFillingMeasuredWaterGet_Result> CollectionFillingMeasuredWaterGet(int companyID, int accountServiceCollectionID, int? fillingMeasuredWaterID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingMeasuredWaterGet(companyID, accountServiceCollectionID, fillingMeasuredWaterID).ToList();
            }
        }
        public decimal CollectionFillingPreviousMeasuredWaterGet(int companyID, int accountServiceCollectionID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingPreviousMeasuredWaterGet(companyID, accountServiceCollectionID).FirstOrDefault() ?? 0;
            }
        }
        public List<MUNSERAccountServiceCollectionFillingMeasuredWaterImagesGet_Result> CollectionFillingMeasuredWaterImagesGet(int fillingID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionFillingMeasuredWaterImagesGet(fillingID).ToList();
            }
        }
        #endregion

        #region AccountServiceCollection
        public List<MUNSERAccountServiceCollectionDetailGet_Result> CollectionDetailGet(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, string language, string filterText)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDetailGet(companyID, accountServiceID, accountServiceCollectionDetailID, language, filterText).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionDetailGetNotFiled_Result> CollectionDetailGetNotFiled(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDetailGetNotFiled(companyID, accountServiceID, accountServiceCollectionDetailID, language).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionDetailGetNotPaid_Result> CollectionDetailGetNotPaid(int accountID, DateTime? paymentDate, bool IsIvaApply, bool applybyAmnesty, int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDetailGetNotPaid(accountID, paymentDate, companyID, language, IsIvaApply, applybyAmnesty).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionDetailSummaryGetNotPaid_Result> CollectionDetailSummaryGetNotPaid(int companyID, string language, int accountID, string collectiondetailIDs, bool forEdit, bool applybyAmnesty)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                return context.MUNSERAccountServiceCollectionDetailSummaryGetNotPaid(companyID, language, accountID, collectiondetailIDs, forEdit, applybyAmnesty).ToList();
            }
        }
        public List<MUNSERAccountServiceFillingGet_Result> FillingGet(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, string language, string filterText)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 300;
                return context.MUNSERAccountServiceFillingGet(companyID, accountServiceID, accountServiceCollectionDetailID, language, filterText).ToList();
            }
        }
        public List<MUNSERAccountServiceDiscountGet_Result> CollectionDiscountGet(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {

                return context.MUNSERAccountServiceDiscountGet(companyID, accountServiceID, accountServiceCollectionDetailID, language).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionPaymentGet_Result> PaymentHistoryGet(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, string language, string filterText)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionPaymentGet(companyID, accountServiceID, accountServiceCollectionDetailID, language, filterText).ToList();
            }
        }
        public List<MUNSERAccountServiceCollectionDebtGet_Result> CollectionDebtGet(int companyID, int accountServiceID, int? accountServiceCollectionDetailID, bool? onlyAdjustment, string language, string filterText)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDebtGet(companyID, accountServiceID, accountServiceCollectionDetailID, onlyAdjustment, language, filterText).ToList();
            }
        }
        public int CollectionDetailUpdate(int accountServiceCollectionDetailID, int companyID, decimal? amountSubjectToTax, decimal? principal, decimal? penalties, decimal? charges, decimal? interest, decimal? tax, decimal? discount, decimal? discountPercentage, decimal? balance, decimal? paidAmount, bool? isPayed, bool? isActive, bool? isDeleted, Guid modifiedUserID, DateTime modifiedDate, byte[] rowVersion)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDetailUpdate(accountServiceCollectionDetailID, companyID, amountSubjectToTax, principal, penalties, charges, interest, tax, discount, discountPercentage, balance, paidAmount, isPayed, isActive, isDeleted, modifiedUserID, modifiedDate, rowVersion);
            }
        }
        public List<MUNSERAccountServiceCollectionDetailGetSummary_Result> CollectionDetailGetSummary(int companyID, string language, string licenseNumber, int? accountID, string filterText, int? accountserviceID, bool? isServiceTypeGroupIsTax)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServiceCollectionDetailGetSummary(companyID, language, licenseNumber, accountID, filterText, accountserviceID, isServiceTypeGroupIsTax).ToList();
            }
        }
        #endregion

        #region Import Account Service Filing
        public List<ValidImportAccountServiceFilingModel> ImportAccountServiceFilingValidation(AccountServiceFilingImportModel model, string language)
        {
            List<ValidImportAccountServiceFilingModel> validData = new List<ValidImportAccountServiceFilingModel>();
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                DataTable importTable = model.ImportList.ToDataTable();
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };

                SqlParameter[] sqlparameters = {
                        new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                        new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar},
                        new SqlParameter { ParameterName = "@IsValidate",Value=model.IsValidate,SqlDbType = SqlDbType.Int},
                        new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier},
                        new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                        new SqlParameter { ParameterName = "@ModifiedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier },
                        new SqlParameter { ParameterName = "@ModifiedDate",Value= model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                        new SqlParameter { ParameterName = "@CreatedBy",Value= model.CreatedBy,SqlDbType = SqlDbType.VarChar},
                        new SqlParameter { ParameterName = "@MUNIMPAccountServiceFillings",Value=importTable,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountServiceFilingImport"}
                        ,outParam,
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPAccountServiceFilingImport");
                validData = ds.Tables[0].ToList<ValidImportAccountServiceFilingModel>();
                return validData;
            }
        }
        public int ImportAccountServiceFilingInsert(AccountServiceFilingImportModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                DataTable dtImport = model.ImportList.ToDataTable();
                int output = 1;
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                SqlParameter[] sqlparameters = {
                        new SqlParameter {  ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@IsValidate", Value=model.IsValidate,SqlDbType = SqlDbType.Bit}
                        ,new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@ModifiedUser",Value=model.ModifiedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@ModifiedDate", Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@CreatedBy",Value= model.CreatedBy,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@MUNIMPAccountServiceFillings",Value=dtImport,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountServiceFilingImport"}
                        ,outParam
                };
                context.ExecuteSqlProcedureWithOutputParam(sqlparameters, "MUNIMPAccountServiceFilingImport");
                output = (int)outParam.Value;
                return output;
            }
        }
        #endregion

        #region Import Measured Water Filing    
        public DataTable ValidateImportMeasuredWaterFiling(MeasuredWaterFilingImportModel model, string language)
        {

            DataTable dt = new DataTable();

            SqlParameter[] sqlparameters = null; SqlParameter sqlOutParam = null;

            DataTable dtImportMeasuredWater = model.ImportList.ToDataTable();

            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 5000000;
                sqlOutParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                sqlparameters = new SqlParameter[]{
                                                new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Locale",Value=language,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@PeriodYear", Value=model.PeriodYear,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodMonth", Value=model.PeriodMonth,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNIMPMeasuredWaterFiling",Value=dtImportMeasuredWater,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPMeasuredWaterFilingImport"}
                                                ,sqlOutParam
                    };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPValidateMeasuredWaterFiling");
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                        dt = ds.Tables[0];
                }
            }
            return dt;
        }
        public int ImportMeasuredWaterFilingInsert(int companyID, string language, MeasuredWaterFilingImportModel model)
        {
            SqlParameter[] sqlparameters = null; SqlParameter sqlOutParam = null;
            int output = 1;

            lock (lockObject)
            {
                using (CollectionDataModelContainer context = new CollectionDataModelContainer())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = int.MaxValue;

                    sqlOutParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                    sqlparameters = new SqlParameter[]{
                                    new SqlParameter { ParameterName = "@CompanyID", Value=companyID,SqlDbType = SqlDbType.Int}
                                   ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                                   ,new SqlParameter { ParameterName = "@PeriodYear", Value=model.PeriodYear,SqlDbType = SqlDbType.Int}
                                   ,new SqlParameter { ParameterName = "@PeriodMonth", Value=model.PeriodMonth,SqlDbType = SqlDbType.Int}
                                   ,new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                                   ,new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                                   ,new SqlParameter { ParameterName = "@ModifiedUser",Value=model.ModifiedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                                   ,new SqlParameter { ParameterName = "@ModifiedDate", Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime}
                                   ,new SqlParameter { ParameterName = "@CreatedBy", Value=model.CreatedBy,SqlDbType = SqlDbType.VarChar}
                                   ,sqlOutParam

                    };
                    context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPAccountServiceFilingMeasuredWaterImport");
                    output = (int)sqlOutParam.Value;
                }
            }
            return output;
        }
        #endregion

        #region Validate Import Measured Water Filing  
        public MeasuredWaterFilingImportModel GetValidateMeasuredWaterFilingForFixFile(int companyID, int? periodYear, int? periodMonth, bool isGenerateFile,bool isInconsistencies, bool isHighConsumption, string filterText)
        {
            MeasuredWaterFilingImportModel model = new MeasuredWaterFilingImportModel();
            DataTable dt = new DataTable();
            SqlParameter[] sqlparameters = null;

            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 5000000;
                sqlparameters = new SqlParameter[]{
                                                new SqlParameter { ParameterName = "@CompanyID",Value=companyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodYear",Value=periodYear != null ? periodYear : (object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodMonth",Value=periodMonth != null ? periodMonth : (object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsGenerateFile",Value=isGenerateFile,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@IsInconsistencies",Value=isInconsistencies,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@IsHighConsumption",Value=isHighConsumption,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@FilterText",Value=filterText != null ? filterText : (object)DBNull.Value,SqlDbType = SqlDbType.VarChar}
                    };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNValidateMeasuredWaterFilingForFixFileGet");
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                        model.FileDataList = ds.Tables[0];
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        model.ProcessDate = Convert.ToDateTime(ds.Tables[1].Rows[0][0]);
                        model.PeriodYear = Convert.ToInt32(ds.Tables[1].Rows[0][1]);
                        model.PeriodMonth = Convert.ToInt32(ds.Tables[1].Rows[0][2]);
                    }
                }
            }
            return model;
        }
        public DataTable InsertValidateMeasuredWaterFilingForFixFile(MeasuredWaterFilingImportModel model, string language)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlparameters = null; SqlParameter sqlOutParam = null;

            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 5000000;
                sqlOutParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                sqlparameters = new SqlParameter[]{
                                                new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Language",Value=language,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@PeriodYear", Value=model.PeriodYear,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodMonth", Value=model.PeriodMonth,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNIMPMeasuredWaterFiling",Value=model.FileDataList,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPMeasuredWaterFilingValidate"}
                                            };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNValidateMeasuredWaterFilingForFixFileInsert");
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                        dt = ds.Tables[0];
                }
            }
            return dt;
        }
        public DataTable UpdateValidateMeasuredWaterFilingForFixFile(MeasuredWaterFilingImportModel model, string language)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlparameters = null; SqlParameter sqlOutParam = null;

            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 5000000;
                sqlOutParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                sqlparameters = new SqlParameter[]{
                                                new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@Language",Value=language,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@PeriodYear", Value=model.PeriodYear,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodMonth", Value=model.PeriodMonth,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNIMPMeasuredWaterFiling",Value=model.FileDataList,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPMeasuredWaterFilingValidate"}
                                            };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNValidateMeasuredWaterFilingForFixFileUpdate");
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                        dt = ds.Tables[0];
                }
            }
            return dt;
        }
        public ValidateMeasuredWaterAccountServiceFilingModel GetAccountServiceForValidateMeasuredWaterFiling(MeasuredWaterFilingImportModel model)
        {
            ValidateMeasuredWaterAccountServiceFilingModel validData = new ValidateMeasuredWaterAccountServiceFilingModel();
            SqlParameter[] sqlparameters = null;

            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 5000000;
                sqlparameters = new SqlParameter[]{
                                                new SqlParameter { ParameterName = "@CompanyID",Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodYear", Value=model.PeriodYear,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@PeriodMonth", Value=model.PeriodMonth,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNIMPMeasuredWaterFiling",Value=model.FileDataList,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPMeasuredWaterFilingValidate"}
                    };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERAccountServiceGetForValidateMeasuredWaterFiling");
                if (ds != null)
                {
                    if (ds.Tables[0] != null)
                        validData.AccountServiceList = ds.Tables[0];
                    if (ds.Tables[1] != null)
                        validData.ValidDetailList = ds.Tables[1];
                    if (ds.Tables[2] != null)
                        validData.AccountServiceNotExistList = ds.Tables[2];
                }
                return validData;
            }
        }
        #endregion

        #region Migration
        public bool ImportAccountServiceAutoCreationAndAutoFilingServices(ImportAccountServiceAutoFilingModel model)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportAutoCreation(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.Year, model.Segrega, model.CURRENT_YEAR_PERIODS_DEBT, model.Language, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServicePropertyTax(ImportAccountServiceFilingModel model, string AccountServiceFilingDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPropertyTax(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, AccountServiceFilingDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServicePropertyTaxOwnerSinglePropertyPreviousYear(ImportAccountServicePropertyForPreviousYearModel model)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPropertyTaxForPreviousYearWithOwnerBysingleProperty(model.FincaID, model.TaxNumber, model.ServiceCode, model.Year, model.AmountSubjectToTax, model.Principal, model.Exemption, model.ExemptionPeriod, model.Payment, model.PaymentPeriod, model.FillingDate, model.FillingeReference, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServicePropertyTaxOwnerMultiplePropertyPreviousYear(ImportAccountServicePropertyForPreviousYearModel model)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPropertyTaxForPreviousYearWithOwnerByMultipleProperty(model.FincaID, model.TaxNumber, model.ServiceCode, model.Year, model.AmountSubjectToTax, model.Principal, model.Exemption, model.ExemptionPeriod, model.FillingDate, model.FillingeReference, Result);
                return (bool)Result.Value;
            }
        }

        public bool ImportAccountServiceAutoCreationPreviousYear(ImportAccountServiceAutoCreationPreviousYearModel model)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportAutoCreationForPreviousYear(model.FincaID, model.TaxNumber, model.ServiceCode, model.Year, model.Segrega, model.PreviousYearPeriod, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServiceUnitPreviousYear(ImportAccountServiceUnitPreviousYearModel model)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportUnitForPreviousYear(model.FincaID, model.TaxNumber, model.ServiceCode, model.Year, model.Segrega, model.PreviousYearPeriod, model.AmountSubjectToTax, Result);
                return (bool)Result.Value;
            }
        }


        public bool ImportAccountServiceLicense(ImportAccountServiceLicenseFilingModel model, string AccountServiceFilingDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                //context.MUNIMPAccountServiceImportLicense(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.CustomField1, model.CustomField2, model.CustomField3,model.CustomDateField, AccountServiceFilingDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServiceMeasureWater(ImportAccountServiceMeasureWaterFilingModel model, string AccountServiceFilingDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportMeasureWater(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.CustomField1, AccountServiceFilingDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServiceMeasureWaterManual(ImportAccountServiceMeasureWaterFilingModel model, string AccountServiceFilingDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportMeasureWaterForManual(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.CustomField1, AccountServiceFilingDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServiceUnit(ImportAccountServiceUnitFilingModel model, string AccountServiceFilingDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportUnit(model.CompanyID, model.FincaID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.Segrega, model.CURRENT_YEAR_PERIODS_DEBT, AccountServiceFilingDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }

        public bool ImportAccountServicePayment(ImportAccountServiceFilingModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPayment(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServicePropertyTaxPayment(ImportAccountServiceFilingPaymentModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPropertyTaxPayment(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }

        public bool ImportAccountServicePropertyTaxPaymentForSelectedYear(ImportAccountServiceFilingPaymentForSelectedYearModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPropertyTaxPaymentForSelectedYear(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServicePayment(ImportAccountServiceFilingPaymentModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportPayment(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }
        public bool ImportAccountServiceAutoFilingPayment(ImportAccountServiceFilingPaymentModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportAutoFilingPayment(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }

        public bool ImportAccountServiceAutoFilingPaymentForSelectedYear(ImportAccountServiceFilingPaymentForSelectedYearModel model, string NumberPrefix, string AccountServicePaymentDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNIMPAccountServiceImportAutoFilingPaymentForSelectedYearWithPaymentData(model.CompanyID, model.TaxNumber, model.ServiceCode, model.GroupID, model.ServiceTypeID, model.FilingTypeID, NumberPrefix, AccountServicePaymentDetailJson, model.Language, model.CreatedUser, model.CreatedDate, model.ModifiedUser, model.ModifiedDate, Result);
                return (bool)Result.Value;
            }
        }

        public bool ImportAccountServicePaymentPlan(ImportAccountServicePaymentPlanModel model, string InvoiceDetailJson)
        {

            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                //context.MUNIMPAccountServiceImportPaymentPlan(model.CompanyID, model.TaxNumber, model.Date, model.Reference, model.Notes, model.TermsAndConditions, 0, model.TotalEmiPeriod, model.PaidEmiPeriod, model.RemainingEmiPeriod, InvoiceDetailJson, Result);
                return (bool)Result.Value;
            }
        }
        #endregion
    }
}

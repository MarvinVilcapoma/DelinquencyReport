using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class Service
    {
        public int? IsServiceExist(string serviceName, int CompanyId, string languageId, int? id)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceExist(serviceName, CompanyId, languageId, id).FirstOrDefault();
            }
        }
        public List<MUNSERServiceGet_Result> Get(int companyId, string language, int? serviceTypeId, int? groupId, int? id, bool isGetByEffectiveDate, string filingTypeID, int? AccountTypeID = (int?)null)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceGet(companyId, language, serviceTypeId, groupId, id, isGetByEffectiveDate, filingTypeID, AccountTypeID).ToList();
            }
        }

        public List<MUNSERServiceListGet_Result> Get(int companyId, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceListGet(companyId, language).ToList();
            }
        }
        public List<MUNSERServiceGetForFACTU_Result> GetForFACTU(int companyId, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceGetForFACTU(companyId, language).ToList();
            }
        }
        public List<MUNSERServiceGetForCobros_Result> GetForCobros(int companyId, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceGetForCobros(companyId, language).ToList();
            }
        }
        
        public ServiceList GetByPaging(int? serviceID, int companyID, string language, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder, Nullable<int> groupID, Nullable<int> serviceTypeID)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                ServiceList serviceList = new ServiceList();

                serviceList.ServiceModelList = context.MUNSERServiceGetWithPaging(serviceID, companyID, language, filterText, pageIndex, pageSize, totalRecord, sortColumn, sortOrder, groupID, serviceTypeID).ToList();
                serviceList.TotalRecord = Convert.ToInt32(totalRecord.Value);
                return serviceList;
            }
        }
        public int Insert(ServiceModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                DataTable ruleTable = model.ServiceCollectionRuleList.ToDataTable();
                DataTable ruleDetailTable = model.ServiceCollectionRuleDetailList.ToDataTable();
                DataTable calculationDateTable = model.ServiceCalculationDateList.ToDataTable();

                SqlParameter outputparam = new SqlParameter { ParameterName = "@ServiceID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@ServiceTypeID",Value=model.ServiceTypeID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@GroupID", Value=model.GroupID, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@ServiceName",Value=model.Name.Trim(),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Locale",Value=model.Locale,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Description",Value=model.Description == null ? (object)DBNull.Value : model.Description,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Code", Value=model.Code == null ? string.Empty : model.Code,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@FilingTypeTableID", Value=model.FilingTypeTableID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@FilingTypeID", Value=model.FilingTypeID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@UseFixedFillingDueDate",Value=model.UseFixedFillingDueDate== null ? (object)DBNull.Value : model.UseFixedFillingDueDate,SqlDbType = SqlDbType.Bit },
                                                 new SqlParameter { ParameterName = "@FillingDueDays",Value= model.FillingDueDays == null ? (object)DBNull.Value : model.FillingDueDays,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@UseFixedPaymentDueDate", Value=model.UseFixedPaymentDueDate== null ? (object)DBNull.Value : model.UseFixedPaymentDueDate,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@PaymentDueDays", Value=model.PaymentDueDays== null ? (object)DBNull.Value : model.PaymentDueDays, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@UseFixedPaymentGracePeriod",Value=model.UseFixedPaymentGracePeriod == null ? (object)DBNull.Value : model.UseFixedPaymentGracePeriod,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@PaymentGracePeriod", Value=model.PaymentGracePeriod == null ? (object)DBNull.Value : model.PaymentGracePeriod, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@UseFixedDiscountDate",Value=model.UseFixedDiscountDate == null ? (object)DBNull.Value : model.UseFixedDiscountDate,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@DiscountDueDays", Value=model.DiscountDueDays == null ? (object)DBNull.Value : model.DiscountDueDays, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@FrequencyID",Value= model.FrequencyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@StartDate",Value=model.StartDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EndDate",Value= model.EndDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EffectiveFrom", Value=model.EffectiveFrom,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EffectiveTo", Value=model.EffectiveTo, SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@Sort",Value=model.Sort == null ? 0 : model.Sort,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@IsPublic",Value= model.IsPublic,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsDeleted",Value=model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CollectionTemplateID",Value=model.CollectionTemplateID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PrintTemplateID",Value=model.PrintTemplateID == null ? (object)DBNull.Value : model.PrintTemplateID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceExceptionJSON",Value= model.ServiceExceptionListJSON,SqlDbType = SqlDbType.NVarChar},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCalculationDateUTD",Value=calculationDateTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCalculationDateUTD"},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCollectionRuleUTD",Value=ruleTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCollectionRuleUTD"},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCollectionRuleDetailUTD",Value=ruleDetailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCollectionRuleDetailUTD"},
                                                 new SqlParameter { ParameterName = "@CustomField1", Value=model.CustomField1 == null ? string.Empty : model.CustomField1,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomField2", Value=model.CustomField2 == null ? string.Empty : model.CustomField2,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomField3", Value=model.CustomField3 == null ? string.Empty : model.CustomField3,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomDateField",Value=model.CustomDateField == null ? string.Empty : model.CustomDateField ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField1", Value=model.AllowDuplicateCustomField1,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField2", Value=model.AllowDuplicateCustomField2,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField3", Value=model.AllowDuplicateCustomField3,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomDateField", Value=model.AllowDuplicateCustomDateField,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@TotalValueLabel",Value=model.TotalValueLabel == null ? string.Empty : model.TotalValueLabel ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AutoCreation", Value=model.AutoCreation == null ? (object)DBNull.Value : model.AutoCreation.Value,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@MultipleServicesPerYear", Value=model.MultipleServicesPerYear == null ? (object)DBNull.Value : model.MultipleServicesPerYear.Value,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AccountTypeIDs", Value=string.IsNullOrEmpty(model.AccountTypeIDs) ? (object)DBNull.Value : model.AccountTypeIDs,SqlDbType = SqlDbType.NVarChar},
                                                 new SqlParameter { ParameterName = "@FilingFormID", Value=model.FilingFormID==null ? (object)DBNull.Value : model.FilingFormID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@FilingDueType", Value=model.FilingDueType==null ? (object)DBNull.Value : model.FilingDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PaymentDueType", Value=model.PaymentDueType==null ? (object)DBNull.Value : model.PaymentDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@DiscountDueType", Value=model.DiscountDueType==null ? (object)DBNull.Value : model.DiscountDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PaymentGraceType", Value=model.PaymentGraceType==null ? (object)DBNull.Value : model.PaymentGraceType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CustomField4", Value=model.CustomField4 == null ? string.Empty : model.CustomField4,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField4", Value=model.AllowDuplicateCustomField4,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CustomField5", Value=model.CustomField5 == null ? string.Empty : model.CustomField5,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField5", Value=model.AllowDuplicateCustomField5,SqlDbType = SqlDbType.Bit},
                                                 outputparam,
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSERServiceInsert");
                return (int)outputparam.Value; // returns serviceId                               
            }
        }
        public int Update(ServiceModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                DataTable ruleTable = model.ServiceCollectionRuleList.ToDataTable();
                DataTable ruleDetailTable = model.ServiceCollectionRuleDetailList.ToDataTable();
                DataTable calculationDateTable = model.ServiceCalculationDateList.ToDataTable();
                context.Database.CommandTimeout = 90000;
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@ID", Value = model.ID, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@ServiceTypeID",Value=model.ServiceTypeID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@GroupID", Value=model.GroupID, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@ServiceName",Value=model.Name.Trim(),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Locale",Value=model.Locale,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Description",Value=model.Description == null ? (object)DBNull.Value : model.Description,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Code", Value=model.Code == null ? string.Empty : model.Code,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@FilingTypeTableID", Value=model.FilingTypeTableID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@FilingTypeID", Value=model.FilingTypeID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@UseFixedFillingDueDate",Value=model.UseFixedFillingDueDate== null ? (object)DBNull.Value : model.UseFixedFillingDueDate,SqlDbType = SqlDbType.Bit },
                                                 new SqlParameter { ParameterName = "@FillingDueDays",Value= model.FillingDueDays == null ? (object)DBNull.Value : model.FillingDueDays,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@UseFixedPaymentDueDate", Value=model.UseFixedPaymentDueDate== null ? (object)DBNull.Value : model.UseFixedPaymentDueDate,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@PaymentDueDays", Value=model.PaymentDueDays== null ? (object)DBNull.Value : model.PaymentDueDays, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@UseFixedPaymentGracePeriod",Value=model.UseFixedPaymentGracePeriod == null ? (object)DBNull.Value : model.UseFixedPaymentGracePeriod,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@PaymentGracePeriod", Value=model.PaymentGracePeriod == null ? (object)DBNull.Value : model.PaymentGracePeriod, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@UseFixedDiscountDate",Value=model.UseFixedDiscountDate == null ? (object)DBNull.Value : model.UseFixedDiscountDate,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@DiscountDueDays", Value=model.DiscountDueDays == null ? (object)DBNull.Value : model.DiscountDueDays, SqlDbType = SqlDbType.Int },
                                                 new SqlParameter { ParameterName = "@FrequencyID",Value= model.FrequencyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@StartDate",Value=model.StartDate == DateTime.MinValue ? (object)DBNull.Value : model.StartDate ,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EndDate",Value= model.EndDate  == DateTime.MinValue ? (object)DBNull.Value : model.EndDate ,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EffectiveFrom", Value=model.EffectiveFrom == DateTime.MinValue ? (object)DBNull.Value : model.EffectiveFrom ,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@EffectiveTo", Value=model.EffectiveTo == DateTime.MinValue ? (object)DBNull.Value :model.EffectiveTo , SqlDbType = SqlDbType.DateTime },
                                                 new SqlParameter { ParameterName = "@Sort",Value=model.Sort == null ? 0 : model.Sort,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@IsPublic",Value= model.IsPublic,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsDeleted",Value=model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsLocked",Value=model.IsLocked,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsTestMode",Value=model.IsTestMode,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CollectionTemplateID",Value=model.CollectionTemplateID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PrintTemplateID",Value=model.PrintTemplateID == null ? (object)DBNull.Value : model.PrintTemplateID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@Original_RowVersion",Value= model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceExceptionJSON",Value= model.ServiceExceptionListJSON,SqlDbType = SqlDbType.NVarChar},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCalculationDateUTD",Value=calculationDateTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCalculationDateUTD"},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCollectionRuleUTD",Value=ruleTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCollectionRuleUTD"},
                                                 new SqlParameter { ParameterName = "@MUNSERServiceCollectionRuleDetailUTD",Value=ruleDetailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNSERServiceCollectionRuleDetailUTD"},
                                                 new SqlParameter { ParameterName = "@CustomField1", Value=model.CustomField1 == null ? string.Empty : model.CustomField1,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomField2", Value=model.CustomField2 == null ? string.Empty : model.CustomField2,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomField3", Value=model.CustomField3 == null ? string.Empty : model.CustomField3,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CustomDateField",Value=model.CustomDateField == null ? string.Empty : model.CustomDateField ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField1", Value=model.AllowDuplicateCustomField1,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField2", Value=model.AllowDuplicateCustomField2,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField3", Value=model.AllowDuplicateCustomField3,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomDateField", Value=model.AllowDuplicateCustomDateField,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@TotalValueLabel",Value=model.TotalValueLabel == null ? string.Empty : model.TotalValueLabel ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AutoCreation", Value=model.AutoCreation == null ? (object)DBNull.Value : model.AutoCreation.Value,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@MultipleServicesPerYear", Value=model.MultipleServicesPerYear == null ? (object)DBNull.Value : model.MultipleServicesPerYear.Value,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AccountTypeIDs", Value=string.IsNullOrEmpty(model.AccountTypeIDs) ? (object)DBNull.Value : model.AccountTypeIDs,SqlDbType = SqlDbType.NVarChar},
                                                 new SqlParameter { ParameterName = "@FilingFormID", Value=model.FilingFormID==null ? (object)DBNull.Value : model.FilingFormID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@FilingDueType", Value=model.FilingDueType==null ? (object)DBNull.Value : model.FilingDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PaymentDueType", Value=model.PaymentDueType==null ? (object)DBNull.Value : model.PaymentDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@DiscountDueType", Value=model.DiscountDueType==null ? (object)DBNull.Value : model.DiscountDueType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@PaymentGraceType", Value=model.PaymentGraceType==null ? (object)DBNull.Value : model.PaymentGraceType,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CustomField4", Value=model.CustomField4 == null ? string.Empty : model.CustomField4,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField4", Value=model.AllowDuplicateCustomField4,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@CustomField5", Value=model.CustomField5 == null ? string.Empty : model.CustomField5,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@AllowDuplicateCustomField5", Value=model.AllowDuplicateCustomField5,SqlDbType = SqlDbType.Bit},
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNSERServiceUpdate");
                return model.ID;
            }
        }
        public int UpdateServiceFinanceItemId(ServiceModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceUpdateFinanceItemId(model.ID, model.CompanyID, model.FINItemID, model.ModifiedUserID, model.ModifiedDate, model.RowVersion).FirstOrDefault().Value;
            }
        }
        public List<MUNSERServiceGetByCollectionField_Result> GetByCollectionField(int CompanyID, int CollectionFieldID, string Language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceGetByCollectionField(CompanyID, CollectionFieldID, Language).ToList();
            }
        }
        public List<MUNGrantListGet_Result> GetGrantList(int companyId, string language)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNGrantListGet(companyId, language).ToList();
            }
        }
    }
}

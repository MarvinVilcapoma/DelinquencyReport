using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class Account
    {
        #region Account
        public AccountList Get(int companyID, string language, bool? status, int accountTypeID, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;

                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountList accountList = new AccountList();

                accountList.AccountModelList = context.MUNAccountGetWithPaging(companyID, language, status, accountTypeID, filterText, pageIndex, pageSize, TotalRecord, sortColumn, sortOrder).ToList();
                accountList.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return accountList;
            }
        }
        public List<MUNAccountGet_Result> Get(int companyID, string accountID, int? accountType, string displayName, string taxNumber, string phoneNumber, string filterText, bool? isActive)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                return context.MUNAccountGet(companyID, accountID, accountType, displayName, taxNumber, phoneNumber, filterText, isActive).ToList();
            }
        }
        public AccountListForSearch GetForSearch(int companyID, string accountTypeIDs, int? accountID, string searchText, bool? isActive, int pageIndex, int pageSize)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                AccountListForSearch accountList = new AccountListForSearch();
                accountList.AccountList = context.MUNAccountGetForSearch(companyID, accountTypeIDs, accountID, searchText, isActive, pageIndex, pageSize, TotalRecord).ToList();
                accountList.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return accountList;
            }
        }
        public bool? IsSponsorByAccount(int companyID, int accountID, int? accountType)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountGetIsSponsor(companyID, accountID, accountType).FirstOrDefault();
            }
        }
        public List<MUNAccountGetForSupportTable_Result> AccountGetForSupportTable(int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountGetForSupportTable(accountID).ToList();
            }
        }
        public string AccountGetForExist(int companyID, string taxNumber, string treasuryNumber, string stateNumber, int? ID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter result = new ObjectParameter("Result", typeof(string));
                result.Value = context.MUNAccountGetForExist(companyID, taxNumber, treasuryNumber, stateNumber, ID, result).FirstOrDefault();
                return Convert.ToString(result.Value);
            }
        }
        public int AddressesGetForExist(int ID, int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountAddressesGetForExist(ID, accountID).FirstOrDefault().Value;
            }
        }
        public int ContactsGetForExist(int ID, int accountID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountContactsGetForExist(ID, accountID).FirstOrDefault().Value;
            }
        }
        public List<MUNAccountGetByRegistrationInformation_Result> GetByRegistrationInformation(string TaxNumber, string TreasuryNumber, string StateNumber, int? ID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountGetByRegistrationInformation(TaxNumber, TreasuryNumber, StateNumber, ID).ToList();
            }
        }
        public AccountIndividualModel IndividualGet(string locale, int accountID, bool isEditAction)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@Locale",Value= locale},
                    new SqlParameter { ParameterName = "@AccountID",Value= accountID},
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountIndividualGet");
                AccountIndividualModel model = ds.Tables[0].ToList<AccountIndividualModel>().FirstOrDefault() ?? new AccountIndividualModel();
                if (model.CurrencyID != 7)
                {
                    if (isEditAction)
                    {
                        model.TaxNumber = model.UnMaskedTaxNumber;
                    }
                    else
                    {
                        model.TaxNumber = model.MaskedTaxNumber;
                    }
                }
                model.UnMaskedTaxNumber = null;
                model.MaskedTaxNumber = null;
                return model;
            }
        }
        public List<AccountBusinessModel> BusinessGet(int? accountID, int? parentID, string language, bool isEditAction, string filterText)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@AccountID",Value= accountID != null ? accountID : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@ISSubAccount",Value= parentID != null ? parentID : (object)DBNull.Value},
                    new SqlParameter { ParameterName = "@Language",Value= language},
                    new SqlParameter { ParameterName = "@FilterText",Value= (string.IsNullOrEmpty(filterText) ? "" : filterText),SqlDbType = SqlDbType.VarChar}

                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountBusinessGet");
                List<AccountBusinessModel> model = ds.Tables[0].ToList<AccountBusinessModel>() ?? new List<AccountBusinessModel>();
                if (model.Count > 0 && model[0].CurrencyID != 7)
                {
                    if (isEditAction)
                    {
                        model.ForEach(x => { x.TaxNumber = x.UnMaskedTaxNumber; x.MaskedTaxNumber = null; x.UnMaskedTaxNumber = null; });
                    }
                    else
                    {
                        model.ForEach(x => { x.TaxNumber = x.MaskedTaxNumber; x.MaskedTaxNumber = null; x.UnMaskedTaxNumber = null; });
                    }
                }
                return model;
            }
        }
        public MUNAccountUpdate_Result Update(MUNAccountUpdate_Result model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNAccountUpdate(model.ID, model.RegisterNumber, model.AccountTypeID, model.IsBusiness, model.ParentID, model.DisplayName, model.PrintCheckAs, model.BusinessDescription, model.CurrencyID, model.CurrencyISOCode, model.Notes, model.Website, model.TaxNumber, model.TreasuryNumber, model.StateNumber, model.InitialDate, model.IsActive, model.ModifiedUserID, model.ModifiedDate, model.RowVersion, model.IsSponsor).FirstOrDefault();
            }
        }
        public Tuple<int, string> IndividualInsert(AccountIndividualModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable emailTable = model.EmailList.ToDataTable();
                DataTable phoneTable = model.PhoneList.ToDataTable();
                DataTable addressTable = model.AddressList.ToDataTable();
                DataTable contactTable = model.ContactList.ToDataTable();
                DataTable filesTable = model.FileList.ToDataTable();

                SqlParameter outputparam = new SqlParameter { ParameterName = "@AccountID", Value = 0, SqlDbType = SqlDbType.Int, Size = 128, Direction = System.Data.ParameterDirection.Output };
                SqlParameter outputparam1 = new SqlParameter { ParameterName = "@RegisterNumber", Size = 128, Value = string.Empty, SqlDbType = SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Output };

                SqlParameter[] sqlparameters = {
                                                 //new SqlParameter { ParameterName = "@RegisterNumber", Value=string.IsNullOrEmpty(model.RegisterNumber)?(object)DBNull.Value:model.RegisterNumber ,SqlDbType = SqlDbType.NVarChar},
                                                outputparam1,
                                                outputparam,
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@AccountTypeID",Value=model.AccountTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsBusiness",Value= model.IsBusiness,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ParentID", Value= model.ParentID == null || model.ParentID == 0 ? (object)DBNull.Value : model.ParentID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@DisplayName", Value=model.DisplayName, SqlDbType = SqlDbType.VarChar },
                                                new SqlParameter { ParameterName = "@PrintCheckAs", Value=model.PrintCheckAs,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BusinessDescription",Value= (string.IsNullOrEmpty(model.BusinessDescription) ? "" : model.BusinessDescription),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CurrencyID",Value= model.CurrencyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CurrencyISOCode", Value=model.CurrencyISOCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Notes", Value=(string.IsNullOrEmpty(model.Notes) ? "" : model.Notes ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Website",Value=(string.IsNullOrEmpty(model.Website) ? "" : model.Website ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TaxNumber",Value=model.TaxNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TreasuryNumber",Value= model.TreasuryNumber == null ? (object)DBNull.Value : model.TreasuryNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@StateNumber",Value= model.StateNumber == null ? (object)DBNull.Value : model.StateNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@InitialDate",Value= model.InitialDate,SqlDbType = SqlDbType.Date},
                                                new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@SalutationID", Value= model.SalutationID > 0 ? model.SalutationID.Value :  (object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SalutationTableID", Value= model.SalutationTableID > 0 ? model.SalutationTableID.Value :  (object)DBNull.Value , SqlDbType = SqlDbType.Int },
                                                new SqlParameter { ParameterName = "@FirstName", Value=model.FirstName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@MiddleName",Value=(string.IsNullOrEmpty(model.MiddleName) ? "" : model.MiddleName),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@LastName",Value=model.LastName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@SecondLastName",Value= (string.IsNullOrEmpty(model.SecondLastName) ? "" : model.SecondLastName),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@SuffixID",Value= model.SuffixID == 0 ? (object)DBNull.Value : model.SuffixID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SuffixTableID",Value= model.SuffixTableID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsSponsor",Value=model.IsSponsor,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ExemptFromIVA",Value=model.ExemptFromIVA,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ReferenceID",Value=string.IsNullOrEmpty(model.ReferenceID)?(object)DBNull.Value:model.ReferenceID,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BanacioIdentifica",Value=string.IsNullOrEmpty(model.BanacioIdentifica)?(object)DBNull.Value:model.BanacioIdentifica,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@IDTypeID",Value= model.IDTypeID == null ? (object)DBNull.Value : model.IDTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IDTypeIDTableID",Value= model.IDTypeIDTableID == null ? (object)DBNull.Value : model.IDTypeIDTableID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNAccountEmailsUTD",Value=emailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountEmailsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountPhonesUTD",Value=phoneTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountPhonesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountAddressesUTD",Value=addressTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountAddressesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountContactsUTD",Value=contactTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountContactsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountFilesUTD",Value=filesTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountFilesUTD"}
                                            };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountIndividualInsert");
                //context.ExecuteSqlProcedure(sqlparameters, "MUNAccountIndividualInsert");          
                return new Tuple<int, string>(Convert.ToInt32(sqlparameters[1].Value), Convert.ToString(sqlparameters[0].Value));
            }
        }
        public Tuple<int, string> BusinessInsert(AccountBusinessModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable emailTable = model.EmailList.ToDataTable();
                DataTable phoneTable = model.PhoneList.ToDataTable();
                DataTable addressTable = model.AddressList.ToDataTable();
                DataTable contactTable = model.ContactList.ToDataTable();
                DataTable filesTable = model.FileList.ToDataTable();

                SqlParameter outputparam = new SqlParameter { ParameterName = "@AccountID", Value = 0, SqlDbType = SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
                SqlParameter outputparam1 = new SqlParameter { ParameterName = "@RegisterNumber", Value = string.Empty, Size = 128, SqlDbType = SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Output };

                SqlParameter[] sqlparameters = {
                                                //new SqlParameter { ParameterName = "@RegisterNumber", Value=string.IsNullOrEmpty(model.RegisterNumber)?(object)DBNull.Value:model.RegisterNumber ,SqlDbType = SqlDbType.NVarChar},
                                                outputparam1,
                                                outputparam,
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@AccountTypeID",Value=model.AccountTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsBusiness",Value= model.IsBusiness,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ParentID", Value= model.ParentID == null || model.ParentID == 0 ? (object)DBNull.Value : model.ParentID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@DisplayName", Value=model.DisplayName, SqlDbType = SqlDbType.VarChar },
                                                new SqlParameter { ParameterName = "@PrintCheckAs", Value=model.PrintCheckAs,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BusinessDescription",Value= (string.IsNullOrEmpty(model.BusinessDescription) ? "" : model.BusinessDescription),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CurrencyID",Value= model.CurrencyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CurrencyISOCode", Value=model.CurrencyISOCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Notes", Value=(string.IsNullOrEmpty(model.Notes) ? "" : model.Notes ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Website",Value=(string.IsNullOrEmpty(model.Website) ? "" : model.Website ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TaxNumber",Value=model.TaxNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TreasuryNumber",Value=string.IsNullOrEmpty(model.TreasuryNumber)?(object)DBNull.Value:model.TreasuryNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@StateNumber",Value=string.IsNullOrEmpty(model.StateNumber)?(object)DBNull.Value: model.StateNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@InitialDate",Value= model.InitialDate,SqlDbType = SqlDbType.Date},
                                                new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@LegalName", Value=model.LegalName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@DBAName",Value=(string.IsNullOrEmpty(model.DBAName) ? "" : model.DBAName ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@NAICSCodeID", Value=model.NAICSCodeID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@NAICSCodeIDTable",Value=model.NAICSCodeIDTable??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BusinessTypeID",Value=model.BusinessTypeID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BusinessTypeTableID",Value= model.BusinessTypeTableID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ZoneID",Value=model.ZoneID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ZoneTableID",Value= model.ZoneTableID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ARPEDescription",Value=string.IsNullOrEmpty(model.ARPEDescription)?"":model.ARPEDescription,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@IsSponsor",Value=model.IsSponsor,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ExemptFromIVA",Value=model.ExemptFromIVA,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ReferenceID",Value=string.IsNullOrEmpty(model.ReferenceID)?(object)DBNull.Value:model.ReferenceID,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BanacioIdentifica",Value=string.IsNullOrEmpty(model.BanacioIdentifica)?(object)DBNull.Value:model.BanacioIdentifica,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@MUNAccountEmailsUTD",Value=emailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountEmailsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountPhonesUTD",Value=phoneTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountPhonesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountAddressesUTD",Value=addressTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountAddressesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountContactsUTD",Value=contactTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountContactsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountFilesUTD",Value=filesTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountFilesUTD"}
                                            };
                //context.ExecuteSqlProcedure(sqlparameters, "MUNAccountBusinessInsert");
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountBusinessInsert");
                return new Tuple<int, string>(Convert.ToInt32(sqlparameters[1].Value), Convert.ToString(sqlparameters[0].Value));
            }
        }
        public int IndividualUpdate(AccountIndividualModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable emailTable = model.EmailList.ToDataTable();
                DataTable phoneTable = model.PhoneList.ToDataTable();
                DataTable addressTable = model.AddressList.ToDataTable();
                DataTable contactTable = model.ContactList.ToDataTable();
                DataTable filesTable = model.FileList.ToDataTable();

                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@AccountID", Value=model.AccountID ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegisterNumber", Value=string.IsNullOrEmpty(model.RegisterNumber)?(object)DBNull.Value:model.RegisterNumber ,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@AccountTypeID",Value=model.AccountTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsBusiness",Value= model.IsBusiness,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ParentID", Value= model.ParentID == null || model.ParentID == 0 ? (object)DBNull.Value : model.ParentID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@DisplayName", Value=model.DisplayName, SqlDbType = SqlDbType.VarChar },
                                                new SqlParameter { ParameterName = "@PrintCheckAs", Value=model.PrintCheckAs,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BusinessDescription",Value= (string.IsNullOrEmpty(model.BusinessDescription) ? "" : model.BusinessDescription),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CurrencyID",Value= model.CurrencyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CurrencyISOCode", Value=model.CurrencyISOCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Notes", Value=(string.IsNullOrEmpty(model.Notes) ? "" : model.Notes ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Website",Value=(string.IsNullOrEmpty(model.Website) ? "" : model.Website ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TaxNumber",Value=model.TaxNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TreasuryNumber",Value= model.TreasuryNumber == null ? (object)DBNull.Value : model.TreasuryNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@StateNumber",Value= model.StateNumber == null ? (object)DBNull.Value : model.StateNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@InitialDate",Value= model.InitialDate,SqlDbType = SqlDbType.Date},
                                                new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@SalutationID", Value= model.SalutationID.HasValue && model.SalutationID.Value > 0 ? model.SalutationID.Value :  (object)DBNull.Value ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SalutationTableID", Value= model.SalutationTableID.HasValue && model.SalutationTableID.Value > 0 ? model.SalutationTableID.Value :  (object)DBNull.Value , SqlDbType = SqlDbType.Int },
                                                new SqlParameter { ParameterName = "@FirstName", Value=model.FirstName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@MiddleName",Value=(string.IsNullOrEmpty(model.MiddleName) ? "" : model.MiddleName),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@LastName",Value=model.LastName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@SecondLastName",Value= (string.IsNullOrEmpty(model.SecondLastName) ? "" : model.SecondLastName),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@SuffixID",Value= model.SuffixID == 0 ? (object)DBNull.Value : model.SuffixID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@SuffixTableID",Value= model.SuffixTableID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@InactiveByUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@InactiveDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@Original_RowVersion", Value=model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                                new SqlParameter { ParameterName = "@IsSponsor",Value=model.IsSponsor,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ExemptFromIVA",Value=model.ExemptFromIVA,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ReferenceID",Value=string.IsNullOrEmpty(model.ReferenceID)?(object)DBNull.Value:model.ReferenceID,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BanacioIdentifica",Value=string.IsNullOrEmpty(model.BanacioIdentifica)?(object)DBNull.Value:model.BanacioIdentifica,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@IDTypeID",Value= model.IDTypeID == null ? (object)DBNull.Value : model.IDTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IDTypeIDTableID",Value= model.IDTypeIDTableID == null ? (object)DBNull.Value : model.IDTypeIDTableID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNAccountEmailsUTD",Value=emailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountEmailsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountPhonesUTD",Value=phoneTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountPhonesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountAddressesUTD",Value=addressTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountAddressesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountContactsUTD",Value=contactTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountContactsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountFilesUTD",Value=filesTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountFilesUTD"}
                                            };
                var retval = context.ExecuteSqlProcedure(sqlparameters, "MUNAccountIndividualUpdate");
                return retval;
            }
        }
        public int BusinessUpdate(AccountBusinessModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable emailTable = model.EmailList.ToDataTable();
                DataTable phoneTable = model.PhoneList.ToDataTable();
                DataTable addressTable = model.AddressList.ToDataTable();
                DataTable contactTable = model.ContactList.ToDataTable();
                DataTable filesTable = model.FileList.ToDataTable();

                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@AccountID", Value=model.AccountID ,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@RegisterNumber", Value=string.IsNullOrEmpty(model.RegisterNumber)?(object)DBNull.Value:model.RegisterNumber ,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@AccountTypeID",Value=model.AccountTypeID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@IsBusiness",Value= model.IsBusiness,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ParentID", Value= model.ParentID == null || model.ParentID == 0 ? (object)DBNull.Value : model.ParentID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@DisplayName", Value=model.DisplayName, SqlDbType = SqlDbType.VarChar },
                                                new SqlParameter { ParameterName = "@PrintCheckAs", Value=model.PrintCheckAs,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BusinessDescription",Value= (string.IsNullOrEmpty(model.BusinessDescription) ? "" : model.BusinessDescription),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@CurrencyID",Value= model.CurrencyID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@CurrencyISOCode", Value=model.CurrencyISOCode,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Notes", Value=(string.IsNullOrEmpty(model.Notes) ? "" : model.Notes ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@Website",Value=(string.IsNullOrEmpty(model.Website) ? "" : model.Website ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TaxNumber",Value=model.TaxNumber,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@TreasuryNumber",Value= model.TreasuryNumber??(object)DBNull.Value,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@StateNumber",Value=model.StateNumber??(object)DBNull.Value,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@InitialDate",Value= model.InitialDate,SqlDbType = SqlDbType.Date},
                                                new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@CreatedUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@LegalName", Value=model.LegalName,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@DBAName",Value=(string.IsNullOrEmpty(model.DBAName) ? "" : model.DBAName ),SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@NAICSCodeID", Value=model.NAICSCodeID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@NAICSCodeIDTable",Value=model.NAICSCodeIDTable??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BusinessTypeID",Value=model.BusinessTypeID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@BusinessTypeTableID",Value= model.BusinessTypeTableID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ZoneID",Value=model.ZoneID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ZoneTableID",Value= model.ZoneTableID??(object)DBNull.Value,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@ARPEDescription",Value= string.IsNullOrEmpty(model.ARPEDescription)?"":model.ARPEDescription,SqlDbType = SqlDbType.NVarChar},
                                                new SqlParameter { ParameterName = "@InactiveByUserID",Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                new SqlParameter { ParameterName = "@InactiveDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                new SqlParameter { ParameterName = "@Original_RowVersion", Value=model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                                new SqlParameter { ParameterName = "@IsSponsor",Value=model.IsSponsor,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ExemptFromIVA",Value=model.ExemptFromIVA,SqlDbType = SqlDbType.Bit},
                                                new SqlParameter { ParameterName = "@ReferenceID",Value=string.IsNullOrEmpty(model.ReferenceID)?(object)DBNull.Value:model.ReferenceID,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@BanacioIdentifica",Value=string.IsNullOrEmpty(model.BanacioIdentifica)?(object)DBNull.Value:model.BanacioIdentifica,SqlDbType = SqlDbType.VarChar},
                                                new SqlParameter { ParameterName = "@MUNAccountEmailsUTD",Value=emailTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountEmailsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountPhonesUTD",Value=phoneTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountPhonesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountAddressesUTD",Value=addressTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountAddressesUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountContactsUTD",Value=contactTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountContactsUTD"},
                                                new SqlParameter { ParameterName = "@MUNAccountFilesUTD",Value=filesTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountFilesUTD"}
                                            };
                var retval = context.ExecuteSqlProcedure(sqlparameters, "MUNAccountBusinessUpdate");
                return retval;
            }
        }
        #endregion

        #region Account Support Tables
        public List<MUNAccountAddressesGet_Result> AddressGet(int? addressID, int accountID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountAddressesGet(addressID, accountID, language).ToList();
            }
        }
        public List<MUNAccountContactsGet_Result> ContactGet(int? contactID, int? accountID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountContactsGet(contactID, accountID, language).ToList();
            }
        }
        public List<MUNAccountEmailsGet_Result> EmailGet(int? id, int accountID, int? emailTypeID, int? emailTypeTableID, string language)
        {
            using (AccountDataModelContainer contex = new AccountDataModelContainer())
            {
                return contex.MUNAccountEmailsGet(id, accountID, emailTypeID, emailTypeTableID, language).ToList();
            }
        }
        public List<MUNAccountFilesGet_Result> FileGet(int? fileID, int accountID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountFilesGet(fileID, accountID, language).ToList();
            }
        }
        public List<MUNAccountPhonesGet_Result> PhoneGet(int? id, int accountID, int? phoneTypeID, int? phoneTypeTableID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNAccountPhonesGet(id, accountID, phoneTypeID, phoneTypeTableID, language).ToList();
            }
        }
        public List<MUNCompanyCurrenciesGet_Result> CompanyCurrencyGet(int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCompanyCurrenciesGet(companyID, language).ToList();
            }
        }
        public List<MUNCountryGet_Result> CountryGet(string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCountryGet(language).ToList();
            }
        }
        public List<MUNCountryStateGet_Result> StateGetByCountryID(int countryID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCountryStateGet(countryID, language).ToList();
            }
        }
        public List<MUNCountryStateCityGet_Result> CityGetByCountryIdAndStateId(int countryID, int stateID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCountryStateCityGet(countryID, stateID, language).ToList();
            }
        }
        public List<MUNCountryStateCityTownGet_Result> TownGetByCountryIdStateIdAndCityId(int countryID, int stateID, int cityID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCountryStateCityTownGet(countryID, stateID, cityID, language).ToList();
            }
        }
        public List<MUNSUPSupportTableValueGet_Result> SupportTableValueGet(int companyID, int tableID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSUPSupportTableValueGet(companyID, tableID, language).ToList();
            }
        }
        public List<MUNCurrencyGet_Result> CurrencyGet(string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNCurrencyGet(language).ToList();
            }
        }
        public int AccountSupportTableInsert(int accountID, List<AccountSupportTable> accSupportTablelist)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable accSupportTable = accSupportTablelist.ToDataTable();
                SqlParameter[] sqlparameters = {
                                                new SqlParameter { ParameterName = "@accountID", Value=accountID,SqlDbType = SqlDbType.Int},
                                                new SqlParameter { ParameterName = "@MUNAccountSupportTableUTD",Value=accSupportTable,SqlDbType = SqlDbType.Structured,TypeName="MUNAccountSupportTableUTD"}
                                            };
                var retval = context.ExecuteSqlProcedure(sqlparameters, "MUNAccountSupportTableInsert");
                return retval;
            }
        }
        #endregion

        #region Import Account
        public List<ValidImportAccountModel> ImportAccountValidation(AccountImportModel model, string language)
        {
            List<ValidImportAccountModel> validData = new List<ValidImportAccountModel>();
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
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
                        new SqlParameter {ParameterName = "@MUNIMPAccounts",Value=importTable,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountImport"}
                        ,outParam,
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNIMPAccountImport");
                validData = ds.Tables[0].ToList<ValidImportAccountModel>();
                return validData;
            }
        }
        public int ImportAccountInsert(AccountImportModel model, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                DataTable dtImport = model.ImportList.ToDataTable();
                int output = 1;
                var outParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = 0 };
                SqlParameter[] sqlparameters = {
                        new SqlParameter { ParameterName = "@CompanyID", Value=model.CompanyID,SqlDbType = SqlDbType.Int}
                        ,new SqlParameter { ParameterName = "@Locale", Value=language,SqlDbType = SqlDbType.VarChar}
                        ,new SqlParameter { ParameterName = "@IsValidate", Value=model.IsValidate,SqlDbType = SqlDbType.Bit}
                        ,new SqlParameter { ParameterName = "@CreatedUser",Value=model.CreatedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@ModifiedUser",Value=model.ModifiedUser,SqlDbType = SqlDbType.UniqueIdentifier}
                        ,new SqlParameter { ParameterName = "@ModifiedDate", Value=model.ModifiedDate,SqlDbType = SqlDbType.DateTime}
                        ,new SqlParameter { ParameterName = "@MUNIMPAccounts",Value=dtImport,SqlDbType = SqlDbType.Structured,TypeName="MUNIMPAccountImport"}
                        ,outParam
                };
                context.ExecuteSqlProcedureWithOutputParam(sqlparameters, "MUNIMPAccountImport");
                output = (int)outParam.Value;
                return output;
            }
        }
        #endregion

        #region Account As People
        public AccountAsPeopleModel GetAsPeople(int companyID, int accountType, string accountID, string filterText, Nullable<bool> isActive, int pageIndex, int pageSize)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                AccountAsPeopleModel _objAccountAsPeople = new AccountAsPeopleModel();

                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                _objAccountAsPeople.PeopleList = context.MUNAccountGetAsPeople(companyID, accountType, accountID, filterText, isActive, pageIndex, pageSize, totalRecord).ToList();
                _objAccountAsPeople.TotalRecord = _objAccountAsPeople.PeopleList.Count > 0 ? Convert.ToInt32(totalRecord.Value) : 0;
                return _objAccountAsPeople;
            }
        }

        #endregion

        #region Account in Judicial Collection
        public void UpdateForJudicialCollection(AccountForJudicialCollectionModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNAccountUpdateForJudicialCollection(model.CompanyID, model.ID, model.IsInJudicialCollection, model.ModifiedUserID, model.ModifiedDate, model.CreatedBy, model.RowVersion);
            }
        }
        #endregion

        #region Export
        public AccountListExport GetAccountListExport(int companyID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                AccountListExport model = new AccountListExport();
                model.ExportAccountList = context.MUNAccountExport(companyID, language).ToList();
                return model;
            }
        }
        #endregion
    }
}

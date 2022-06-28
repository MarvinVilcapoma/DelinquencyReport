using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.CompanyModels
{
    public class Company
    {
        #region Company
        public int Registration(CompanyModel model)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                List<CompanyParameterModel> companyParameterList = new List<CompanyParameterModel>();
                model.CompanyParameter.ID = model.ID;
                companyParameterList.Add(model.CompanyParameter);
                System.Data.DataTable companyParameterTable = companyParameterList.ToDataTable();

                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@ID", Value=model.ID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Name", Value=model.Name,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@LegalName",Value=model.LegalName,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@WorkPhone",Value=(model.WorkPhone == null ? (object)DBNull.Value : model.WorkPhone),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@URL", Value= (model.URL == null ? (object)DBNull.Value : model.URL),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Email", Value=(model.Email == null ? (object)DBNull.Value : model.Email), SqlDbType = SqlDbType.VarChar },
                                                 new SqlParameter { ParameterName = "@LogoID", Value=(model.LogoID == null ? (object)DBNull.Value : model.LogoID),SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CountryID",Value=model.CountryID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CompanySize",Value=(model.CompanySize == null ? (object)DBNull.Value : model.CompanySize),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsAgreedTerms", Value=model.IsAgreedTerms,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AgreeTermsOnDate", Value=model.AgreeTermsOnDate, SqlDbType = SqlDbType.SmallDateTime},
                                                 new SqlParameter { ParameterName = "@TermsDetails", Value=model.TermsDetails,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsSystem",Value=model.IsSystem,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsDeleted",Value=model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@Sort",Value=(model.Sort == null ? (object)DBNull.Value : model.Sort),SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Modules",Value=model.Modules,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CurrencyID",Value=model.CurrencyID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Code",Value=model.Code,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CreatedUserID", Value=model.CreatedUserID,SqlDbType = SqlDbType.UniqueIdentifier},
                                                 new SqlParameter { ParameterName = "@CreatedDate", Value=model.CreatedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@MUNCompanyParameterUTD",Value=companyParameterTable,SqlDbType = SqlDbType.Structured,TypeName="MUNCompanyParameterUTD"}
                                               };
                return context.ExecuteSqlProcedure(sqlparameters, "MUNCompanyRegistration");
            }
        }
        public CompanyModel Update(CompanyModel model)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {

                model.CompanyParameter.ID = model.ID;
                List<CompanyParameterModel> companyParameterList = new List<CompanyParameterModel>();
                companyParameterList.Add(model.CompanyParameter);
                System.Data.DataTable companyParameterTable = companyParameterList.ToDataTable();
                SqlParameter[] sqlparameters = {
                                                 new SqlParameter { ParameterName = "@ID", Value=model.ID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Name", Value=model.Name,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@LegalName",Value=model.LegalName,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@WorkPhone",Value=(model.WorkPhone == null ? (object)DBNull.Value : model.WorkPhone),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@URL", Value= (model.URL == null ? (object)DBNull.Value : model.URL),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Email", Value=(model.Email == null ? (object)DBNull.Value : model.Email), SqlDbType = SqlDbType.VarChar },
                                                 new SqlParameter { ParameterName = "@LogoID", Value=(model.LogoID == null ? (object)DBNull.Value : model.LogoID),SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CountryID",Value=model.CountryID,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CompanySize",Value=(model.CompanySize == null ? (object)DBNull.Value : model.CompanySize),SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsAgreedTerms", Value=model.IsAgreedTerms,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@AgreeTermsOnDate", Value=model.AgreeTermsOnDate, SqlDbType = SqlDbType.SmallDateTime},
                                                 new SqlParameter { ParameterName = "@TermsDetails", Value=model.TermsDetails,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@IsSystem",Value=model.IsSystem,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsDeleted",Value=model.IsDeleted,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@IsActive",Value=model.IsActive,SqlDbType = SqlDbType.Bit},
                                                 new SqlParameter { ParameterName = "@Sort",Value=(model.Sort == null ? (object)DBNull.Value : model.Sort),SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@Code",Value=model.Code,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@ModifiedUserID",Value=model.ModifiedUserID,SqlDbType = SqlDbType.UniqueIdentifier },
                                                 new SqlParameter { ParameterName = "@ModifiedDate",Value= model.ModifiedDate,SqlDbType = SqlDbType.DateTime},
                                                 new SqlParameter { ParameterName = "@Original_RowVersion",Value= model.RowVersion,SqlDbType = SqlDbType.Timestamp},
                                                 new SqlParameter { ParameterName = "@MUNCompanyParameterUTD",Value=companyParameterTable,SqlDbType = SqlDbType.Structured,TypeName="MUNCompanyParameterUTD"},
                                                 new SqlParameter { ParameterName = "@Address1", Value=model.Address1 ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@Address2", Value=model.Address2 ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@City", Value=model.City ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@CountryStateID", Value=model.CountryStateID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@CompnayCountryID", Value=model.CompnayCountryID ,SqlDbType = SqlDbType.Int},
                                                 new SqlParameter { ParameterName = "@ZipPostalCode", Value=model.ZipPostalCode ,SqlDbType = SqlDbType.VarChar},
                                                 new SqlParameter { ParameterName = "@MUNAccountAddressesID", Value=(model.MUNAccountAddressesID == null ? (object)DBNull.Value : model.MUNAccountAddressesID),SqlDbType = SqlDbType.Int}
                                               };
                context.ExecuteSqlProcedure(sqlparameters, "MUNCompanyUpdate");
                return model;
            }

        }
        public MUNCompanyGet_Result Get(int companyID)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyGet(companyID).FirstOrDefault();
            }
        }
        public List<MUNCompanyGet_Result> Get()
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyGet(0).ToList();
            }
        }
        public int? IsCompanyCodeExist(int CompanyID, string Code)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyCodeExist(CompanyID, Code).FirstOrDefault();
            }
        }
        #endregion

        #region Company Modules
        public List<MUNCompanyModulesGet_Result> CompanyModulesGet(int companyID, Guid? moduleID)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyModulesGet(companyID, moduleID).ToList();
            }
        }
        #endregion

        #region Company Address
        public MUNCompanyAddressesGet_Result CompanyAddressGet(int companyID, string locale)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyAddressesGet(companyID, locale).FirstOrDefault();
            }
        }
        #endregion
    }
}

using ArtSolutions.MUN.Core.AccountModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.Collections
{
    public class Invoice
    {
        public InvoiceListModel GetWithPaging(int companyID, int? accountID, int? iD, string filterText,string language, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecord", typeof(Int32));
                InvoiceListModel model = new InvoiceListModel();
                model.InvoiceList = context.MUNCOLInvoiceGetWithPaging(companyID, accountID, iD, filterText,language, pageIndex, pageSize, sortColumn, sortOrder, TotalRecord).ToList();
                model.TotalRecord = Convert.ToInt32(TotalRecord.Value);
                return model;
            }
        }
        public List<MUNCOLInvoiceGet_Result> Get(int companyID, int? accountID, int? iD , bool? onlyPendingInvoice , bool? onlyPostedInvoice , string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNCOLInvoiceGet(companyID, accountID, iD,onlyPendingInvoice,onlyPostedInvoice, language).ToList();
            }
        }
        public List<MUNCOLInvoiceGetAsObject_Result> GetAsObject(int companyID, int? accountID, int? iD, string filterText, bool? isVoid, bool? isPost, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {              
                return context.MUNCOLInvoiceGetAsObject(companyID, accountID, iD, filterText, isVoid, isPost, language).ToList();               
            }
        }
        public List<MUNCOLInvoiceDetailGet_Result> DetailGet(int companyID,int? iD, int? invoiceID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                return context.MUNCOLInvoiceDetailGet(companyID,iD, invoiceID, language).ToList();
            }
        }       
        public InvoicePrintModel GetPrint(int iD, int companyID, string language)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                InvoicePrintModel model = new InvoicePrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= iD},                    
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNCOLInvoicePrint");
                model.Company = ds.Tables[0].ToList<InvoiceCompanyModel>().FirstOrDefault() ?? new InvoiceCompanyModel();
                model.Account = ds.Tables[1].ToList<InvoiceAccountModel>().FirstOrDefault() ?? new InvoiceAccountModel();
                model.Invoice = ds.Tables[2].ToList<InvoiceModel>().FirstOrDefault() ?? new InvoiceModel();
                model.ServiceTypeID = int.Parse(ds.Tables[3].Rows[0][0].ToString());
                if (model.ServiceTypeID > 0)
                    model.AccountServiceCollectionDetailList = ds.Tables[4].ToList<AccountServiceCollectionDetailModel>() ?? new List<AccountServiceCollectionDetailModel>();
                else
                    model.InvoiceDetailList = ds.Tables[4].ToList<InvoiceDetailModel>().ToList() ?? new List<InvoiceDetailModel>();
                model.PrintTemplate = ds.Tables[5].ToList<PrintTemplateModel>().FirstOrDefault() ?? new PrintTemplateModel();
                return model;
            }
        }       
        public MUNCOLInvoiceManualInsert_Result ManualInsert(InvoiceModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNCOLInvoiceManualInsert(model.CompanyID, model.AccountID, model.Date, model.Reference, model.Notes, model.TermsAndConditions,  model.Total, model.CreatedUserID, model.CreatedDate, model.Language,model.InvoiceDetailJson).FirstOrDefault();
            }
        }
        public int Update(InvoiceModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                context.MUNCOLInvoiceUpdate(model.ID, model.CompanyID, model.AccountID, model.Date, model.Reference, model.Notes, model.TermsAndConditions,  model.Total, model.ModifiedUserID, model.ModifiedDate, model.Language, model.RowVersion,model.InvoiceDetailJson);
                return model.ID;
            }
        }        
        public bool Void(InvoiceModel model)
        {
            using (CollectionDataModelContainer context = new CollectionDataModelContainer())
            {                
                context.MUNCOLInvoiceVoid(model.ID, model.CompanyID, model.VoidReason, model.RowVersion, model.ModifiedUserID, model.ModifiedDate);
                return true;
            }
        }
    }
}

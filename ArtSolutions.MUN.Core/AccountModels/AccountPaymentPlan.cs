using ArtSolutions.MUN.Core.ReportModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class AccountPaymentPlan
    {
        #region Account Payment Plan
        public ListAccountPaymentPlanModel GetByPaging(int companyID, string language, string filterText, int? accountID, int? accountPropertyID, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            ListAccountPaymentPlanModel model = new ListAccountPaymentPlanModel();
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;

                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(int));
                model.AccountPaymentPlanList = context.MUNSERAccountPaymentPlanGetWithPaging(companyID, language, filterText, accountID, accountPropertyID, pageIndex, pageSize, totalRecord, sortColumn, sortOrder).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
            }
            return model;
        }
        public List<MUNSERAccountPaymentPlanGet_Result> Get(int companyID, int? accountID, int? id, bool? isActive, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountPaymentPlanGet(companyID, accountID, id, isActive, language).ToList();
            }
        }
        public List<MUNSERAccountPaymentPlanGetNotPaid_Result> GetNotPaid(int companyID, int? accountID, int? id, DateTime? date, bool? isActive, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountPaymentPlanGetNotPaid(companyID, accountID, id, date, isActive, language).ToList();
            }
        }
        public bool IsAccountHavePaymentPlan(int companyID, int accountID, string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter Result = new ObjectParameter("Result", typeof(bool));
                context.MUNSERAccountPaymentPlanForAccount(companyID, accountID, language, Result);
                return Convert.ToBoolean(Result.Value);
            }
        }
        public int Insert(AccountPaymentPlanModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                ObjectParameter accountPaymentPlanID = new ObjectParameter("AccountPaymentPlanID", typeof(Int32));
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                accountPaymentPlanID.Value = context.MUNSERAccountPaymentPlanInsert
                    (
                        model.CompanyID, model.AccountID, model.ServicePaymentPlanID, model.StartDate, model.EndDate,
                        model.InstalmentPercentage, model.InterestPercentage, model.LateInterestPercentage, model.Months,
                        model.IsEditable, model.InstalmentAmount, model.MonthlyAmount, model.MonthlyInterest,
                        model.TotalInterest, model.TotalPayment, model.TotalDebt, model.ApplybyAmnesty,
                        //model.AmnestyAmount,
                        model.IsActive, model.InactiveDate, model.InactiveReason, model.IsDeleted,
                        model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate,
                        model.CreatedBy, model.AccountPaymentPlanDetailJson, model.AccountServiceCollectionDetailIDs,
                        accountPaymentPlanID
                      );
                return Convert.ToInt32(accountPaymentPlanID.Value);
            }
        }
        public int Update(AccountPaymentPlanModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERAccountPaymentPlanUpdate(model.ID, model.CompanyID, model.ServicePaymentPlanID,
                model.StartDate, model.EndDate, model.InstalmentPercentage, model.InterestPercentage, model.LateInterestPercentage,
                model.Months, model.InstalmentAmount, model.MonthlyAmount, model.MonthlyInterest, model.TotalInterest,
                model.TotalPayment, model.TotalDebt, model.ApplybyAmnesty,
                //model.AmnestyAmount,
                model.ModifiedUserID, model.ModifiedDate, model.AccountPaymentPlanDetailJson
                //, model.AccountServiceCollectionDetailIDs
                );
            }
        }
        public void Delete(AccountPaymentPlanModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.MUNSERAccountPaymentPlanDelete(model.ID, model.ReasonForDeleted, model.ModifiedUserID, model.ModifiedDate, model.RowVersion);
            }
        }
        public int UpdateStatus(AccountPaymentPlanModel model)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERAccountPaymentPlanUpdateStatus(model.CompanyID, model.ID, model.IsActive, model.ModifiedUserID, model.ModifiedDate, model.RowVersion).FirstOrDefault().Value;
            }
        }
        #endregion

        #region Account Payment Plan Detail
        public List<MUNSERAccountPaymentPlanDetailGet_Result> GetAccountPaymentPlanDetail(int companyID, int? id, string accountPaymentPlanIDs, bool? isActive, bool? isPaid, string language, bool? isRemoveInterest)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountPaymentPlanDetailGet(companyID, id, accountPaymentPlanIDs, isActive, isPaid, isRemoveInterest, language).ToList();
            }
        }
        #endregion

        #region Account Service Payment Plan
        public List<MUNSERAccountServicePaymentPlanGet_Result> GetAccountServicePaymentPlan(int companyID, string language, int? accountID, int? accountPaymentPlanID, int? accountServiceID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServicePaymentPlanGet(companyID, language, accountID, accountPaymentPlanID, accountServiceID).ToList();
            }
        }
        public List<MUNSERAccountServicePaymentPlanSummaryGet_Result> GetAccountServicePaymentPlanSummary(int companyID, string language, int accountPaymentPlanID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                return context.MUNSERAccountServicePaymentPlanSummaryGet(companyID, language, accountPaymentPlanID).ToList();
            }
        }
        #endregion

        #region Print
        public AccountPaymentPlanPrintModel Print(int companyID, string language, int accountID, int AccountPaymentPlanID)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                AccountPaymentPlanPrintModel model = new AccountPaymentPlanPrintModel();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@CompanyID",Value=companyID},
                    new SqlParameter { ParameterName = "@Locale",Value=language},
                    new SqlParameter { ParameterName = "@AccountID",Value=accountID},
                    new SqlParameter { ParameterName = "@AccountPaymentPlanID",Value=AccountPaymentPlanID},
                };

                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNSERAccountPaymentPlanGetForPrint");
                model.AccountModel = ds.Tables[0].ToList<ReportAccountModel>().FirstOrDefault() ?? new ReportAccountModel();
                model.AccountPaymentPlanModel = ds.Tables[1].ToList<AccountPaymentPlanModel>().FirstOrDefault() ?? new AccountPaymentPlanModel();
                model.AccountPaymentPlanList = ds.Tables[2].ToList<AccountPaymentPlanPrintDetailModel>() ?? new List<AccountPaymentPlanPrintDetailModel>();
                return model;
            }
        }
        #endregion

        #region Export
        public PaymentPlanExportList GetExportpaymentPlan(int companyID, string language)
        {
            PaymentPlanExportList model = new PaymentPlanExportList();
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                context.Database.CommandTimeout = 3000;
                model.ExportPaymentPlanList = context.MUNSERREPPaymentPlanExport(language, companyID).ToList();
            }
            return model;
        }
        #endregion
    }
}

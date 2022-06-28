using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class PaymentPlan
    {
        public PaymentPlanListModel GetByPaging(int companyID, string language, string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            PaymentPlanListModel model = new PaymentPlanListModel();
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                ObjectParameter totalRecord = new ObjectParameter("TotalRecord", typeof(int));
                model.PaymentPlanList = context.MUNSERPaymentPlanGetWithPaging(companyID, language, filterText, pageIndex, pageSize, totalRecord, sortColumn, sortOrder).ToList();
                model.TotalRecord = Convert.ToInt32(totalRecord.Value);
            }
            return model;
        }
        public List<MUNSERPaymentPlanGet_Result> Get(int companyID, string language, int? id, DateTime? date, bool? isActive)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERPaymentPlanGet(companyID, language, id, date, isActive).ToList();
            }
        }
        public int Insert(PaymentPlanModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                ObjectParameter paymentPlanID = new ObjectParameter("PaymentPlanID", typeof(Int32));
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                paymentPlanID.Value = context.MUNSERPaymentPlanInsert(model.CompanyID, model.Locale, model.Name, model.EffectiveFrom, model.EffectiveTo, model.InstalmentPercentage, model.InterestPercentage, model.LateInterestPercentage, model.Months, model.IsEditable, model.IsActive, model.IsDeleted, model.CreatedUserID, model.CreatedDate, model.ModifiedUserID, model.ModifiedDate, paymentPlanID);
                return Convert.ToInt32(paymentPlanID.Value);
            }
        }
        public int Update(PaymentPlanModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERPaymentPlanUpdate(model.ID, model.CompanyID, model.Locale, model.Name, model.EffectiveFrom, model.EffectiveTo, model.InstalmentPercentage, model.InterestPercentage, model.LateInterestPercentage, model.Months, model.IsEditable, model.IsActive, model.IsDeleted, model.ModifiedUserID, model.ModifiedDate, model.RowVersion).FirstOrDefault().Value;
            }
        }
        public int UpdateStatus(PaymentPlanModel model)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                return context.MUNSERPaymentPlanUpdateStatus(model.CompanyID, model.ID, model.IsActive, model.ModifiedUserID, model.ModifiedDate, model.RowVersion).FirstOrDefault().Value;
            }
        }
    }
}
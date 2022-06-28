using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core.AccountModels
{
    public class CreditHistoryDetailModel
    {
        
            public int ID { get; set; }
            public DateTime Date { get; set; }
            public string AccountDisplayName { get; set; }
            public string Number { get; set; }
            public decimal CreditAmount { get; set; }
            public string Reason { get; set; }
            public string ReceiptNumber { get; set; }
            public int ServiceTypeID { get; set; }
            public string PaymentPlanName { get; set; }
            public int PaymentID { get; set; }
            public bool IsVoid { get; set; }
            public decimal Amount { get; set; }
            public decimal AvailableAmount { get; set; }

        public CreditHistoryDetail Get(int ID,string language)
        {
            using (AccountDataModelContainer context = new AccountDataModelContainer())
            {
                CreditHistoryDetail model = new CreditHistoryDetail();
                SqlParameter[] sqlparameters = {
                    new SqlParameter { ParameterName = "@ID",Value= ID},
                    new SqlParameter { ParameterName = "@Language",Value=language}
                };
                DataSet ds = context.ExecuteSqlProcedureDataSet(sqlparameters, "MUNAccountCreditHistoryDetailGet");
                model.CreditHistory = ds.Tables[0].ToList<CreditHistoryDetailModel>().FirstOrDefault() ?? new CreditHistoryDetailModel();
                model.CreditHistoryDetailList = ds.Tables[1].ToList<CreditHistoryDetailModel>() ?? new List<CreditHistoryDetailModel>();
                return model;
            }
        }
    }

    public class CreditHistoryDetail
    {
        public CreditHistoryDetailModel CreditHistory { get; set; }
        public List<CreditHistoryDetailModel> CreditHistoryDetailList { get; set; }
    }
}

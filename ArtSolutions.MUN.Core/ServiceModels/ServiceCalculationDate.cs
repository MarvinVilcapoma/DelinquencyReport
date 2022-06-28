using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Core.ServiceModels
{
    public class ServiceCalculationDate
    {
        #region Public Property
        public int ID { get; set; }
        public int FiscalYearID { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }
        public DateTime FillingDueDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public Nullable<System.DateTime> PaymentGraceDate { get; set; }
        public Nullable<System.DateTime> DiscountDate { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public int SequenceID { get; set; }
        #endregion

        public List<MUNSERServiceCalculationDateGet_Result> Get(int? id, int? fiscalYearID)
        {
            using (ServiceDataModelContainer context = new ServiceDataModelContainer())
            {
                return context.MUNSERServiceCalculationDateGet(id, fiscalYearID).ToList();
            }
        }
    }
}

using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class PaymentPlanListModel
    {
        #region Public Properties
        public List<PaymentPlanModel> PaymentPlanList { get; set; }
        public int TotalRecord { get; set; }
        #endregion
    }
}
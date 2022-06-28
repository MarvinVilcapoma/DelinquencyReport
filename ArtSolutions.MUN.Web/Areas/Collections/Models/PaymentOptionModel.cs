using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PaymentOptionModel
    {
        #region Public Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public int PaymentID { get; set; }
        public int PaymentOptionTable17 { get; set; }
        public int PaymentOptionTableValue { get; set; }
        public string Notes { get; set; }
        public decimal? Amount { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public IEnumerable<SelectListItem> PaymentOptions { get; set; }
        #endregion
    }
}
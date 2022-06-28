using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class SalesCashierModel
    {
        #region Public Properties
        public Guid UserID { get; set; }        
        public string UserName { get; set; }
        #endregion
    }

    public class SalesCashier
    {
        #region Public Methods
        public List<SalesCashierModel> Get()
        {
            return new RestSharpHandler().RestRequest<List<SalesCashierModel>>(APISelector.Municipality, true, "api/Payment/SalesCashierGet", "GET", null, null);
        }
        #endregion
    }
}
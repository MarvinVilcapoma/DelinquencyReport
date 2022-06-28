using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class PaymentPlanList
    {
        #region public methods
        public PaymentPlanListModel GetByPaging(string filterText, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair> {
                new NameValuePair { Name = "filterText", Value = filterText},
                new NameValuePair { Name = "pageIndex", Value = pageIndex},
                new NameValuePair { Name = "pageSize", Value = pageSize},
                new NameValuePair { Name = "sortColumn", Value = sortColumn},
                new NameValuePair { Name = "sortOrder", Value = sortOrder}
            };
            PaymentPlanListModel list = new RestSharpHandler().RestRequest<PaymentPlanListModel>(APISelector.Municipality, true, "api/PaymentPlan/GetByPaging", "GET", lstParameter);
            list.PaymentPlanList.ForEach(a =>
            {
                a.InstalmentPercentageValue = (a.InstalmentPercentage / 100).ToString("P");
                a.InterestPercentageValue = (a.InterestPercentage / 100).ToString("P");
                a.LateInterestPercentageValue = (a.LateInterestPercentage / 100).ToString("P");
            });
            return list;
        }
        #endregion
    }
}
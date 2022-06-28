using System.Collections.Generic;
using ArtSolutions.MUN.Web.Helpers;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class DashboardModel
    {
        public List<CaseModel> CasesGetBalanceByStatus()
        {
            return new RestSharpHandler().RestRequest<List<CaseModel>>(APISelector.Municipality, true, "api/case/CasesGetBalanceByStatus", "GET", null);
        }
        public List<CaseModel> CasesGetCountByStatus()
        {
            return new RestSharpHandler().RestRequest<List<CaseModel>>(APISelector.Municipality, true, "api/case/CasesGetCountByStatus", "GET", null);
        }
    }
}
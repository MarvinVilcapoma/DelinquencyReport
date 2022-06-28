using System.Collections.Generic;
using System;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseList
    {
        public List<CaseModel> CaseModels { get; set; }
        public int TotalRecord { get; set; }
        public CaseList()
        {
            CaseModels = new List<CaseModel>();
        }
    }
    public class AccountView
    {
        public string MunicipalityName { get; set; }
        public int MunicipalityID { get; set; }
        public string DestinataryName { get; set; }
        public DateTime Date { get; set; }
        public string MunicipalityCity { get; set; }
    }
}
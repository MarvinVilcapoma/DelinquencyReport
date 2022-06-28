using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolutions.MUN.Web.Areas.Companies.Models
{
    public class CompanyParameterModel
    {
        #region public properties
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [Range(0, 5, ErrorMessageResourceName = "NoOfDecimalPointsRangeMessage", ErrorMessageResourceType = typeof(Company))]
        public int Precision { get; set; }
        public bool UseLeapYear { get; set; }
        public bool UseAccountReceivableJournal { get; set; }
        #endregion

        #region public methods
        public CompanyParameterModel Get()
        {
            return new RestSharpHandler().RestRequest<CompanyParameterModel>(APISelector.Municipality, true, "api/Company/GetCompanyParamter", "GET", null);
        }
        #endregion
    }
}
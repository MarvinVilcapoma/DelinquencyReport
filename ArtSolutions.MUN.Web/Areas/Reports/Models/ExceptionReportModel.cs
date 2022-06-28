using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class ExceptionReportModel
    {
        #region public properties     
        public DateTime? Date { get; set; }

        public List<ExceptionList> ExceptionList { get; set; }
        public ReportCompanyModel ReportCompanyModel { get; set; }
        public int TotalRecord { get; set; }
        #endregion

        #region Constructor
        public ExceptionReportModel()
        {
            this.ExceptionList = new List<ExceptionList>();
        }
        #endregion
    }
    public class ExceptionList
    {
        #region public properties
        public int ID { get; set; }
        public System.Guid UserToken { get; set; }
        public System.DateTime ExceptionDateTime { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        #endregion

        #region custom properties 
        public string FormattedUserToken
        {
            get
            {
                UserProfileViewModel model = new UserProfile().GetUserByUserIDs(this.UserToken.ToString()).FirstOrDefault();
                return model == null ? "-" : (string.IsNullOrEmpty(model.FullName) ? model.Email : model.FullName);
            }
        }        
        #endregion
    }
}
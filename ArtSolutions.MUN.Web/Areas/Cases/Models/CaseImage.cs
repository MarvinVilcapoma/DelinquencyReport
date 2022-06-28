using System;
using System.Collections.Generic;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseImage
    {
        #region "Properties"
        public int SrNo { get; set; }
        public int CaseID { get; set; }
        public int ImageID { get; set; }
        public Guid CreatedUserID { get; set; } = UserSession.Current.Id;
        public DateTime CreatedDate { get; set; } = Common.CurrentDateTime;
        public string FileName { get; set; }
        public string ByUser { get; set; }
        public string Date { get; set; }
        public string OldFileName { get; set; }
        #endregion

        #region "Public Methods"
        public int CaseImagesInsert(CaseModel model)
        {
            List<Object> parms = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/CaseImagesInsert", "POST", null, parms);
        }
        public CaseImageList CaseImageGetByPaging(CaseModel customSearch, JQDTParams jqdtParams)
        {
            CaseImageList caseImageList = new CaseImageList();

            customSearch.SortColumnName = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;

            customSearch.CurrentIndex = (jqdtParams.Start + 1);

            customSearch.PageSize = jqdtParams.Length;

            customSearch.Sort = jqdtParams.Order[0].Dir.ToString();

            caseImageList = CaseImageGetByPaging(customSearch);

            if (caseImageList.CaseImages.Any())
            {
                caseImageList.CaseImages.ToList().ForEach(a =>
                {
                    a.Date = a.CreatedDate.ToString("g");
                    a.ByUser = new UserProfile().UserProfileGet(new UserProfileViewModel {
                        UserID = a.CreatedUserID
                    }).FullName;
                });
            }
            return caseImageList;
        }
        #endregion

        #region "Private Methods"
        private CaseImageList CaseImageGetByPaging(CaseModel model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "caseid", Value = model.CaseID },
                new NameValuePair { Name = "currentIndex", Value = model.CurrentIndex },
                new NameValuePair { Name = "pageSize", Value = model.PageSize },
                new NameValuePair { Name = "sortColumn", Value = model.SortColumnName },
                new NameValuePair { Name = "sortDirection", Value = model.Sort }
            };
            return new RestSharpHandler().RestRequest<CaseImageList>(APISelector.Municipality, true, "api/case/caseimagesget", "GET", lstParameter);
        }
        #endregion
    }
    public class CaseImageList
    {
        public List<CaseImage> CaseImages { get; set; } = new List<CaseImage>();
        public int TotalRecord { get; set; } = 0;
    }
}
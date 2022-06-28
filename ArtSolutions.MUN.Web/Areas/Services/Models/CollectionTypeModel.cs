using ArtSolutions.MUN.Web.Helpers;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class CollectionTypeModel
    {
        #region public properties
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        #endregion

        #region public methods
        public List<CollectionTypeModel> Get()
        {
            return new RestSharpHandler().RestRequest<List<CollectionTypeModel>>(APISelector.Municipality, true, "api/Service/CollectionTypeGet", "GET", null);
        }
        #endregion
    }
}
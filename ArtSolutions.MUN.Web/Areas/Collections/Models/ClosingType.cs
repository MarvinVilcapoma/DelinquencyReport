using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class ClosingTypeModel
    {
        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        #endregion
    }
    public class ClosingType
    {
        #region public methods
        public List<ClosingTypeModel> Get()
        {
            List<ClosingTypeModel> list = new List<ClosingTypeModel>();
            list.Add(new ClosingTypeModel() { ID = 1, Name = "Default" });
            return list;
        }
        #endregion
    }
}
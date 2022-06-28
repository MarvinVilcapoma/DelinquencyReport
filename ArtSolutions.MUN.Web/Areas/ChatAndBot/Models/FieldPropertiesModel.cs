using System.Collections.Generic;

namespace ArtSolutions.MUN.Web.Areas.ChatAndBot.Models
{
    public class FieldPropertiesModel
    {
        #region Property Section
        public string type { get; set; }
        public bool required { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string placeholder { get; set; }
        public string className { get; set; }
        public string name { get; set; }
        public bool multiple { get; set; }
        public string value { get; set; }
        public OptionsModel[] values { get; set; }
        public string subtype { get; set; }
        public int maxlength { get; set; }
        public int rows { get; set; }
        public bool inline { get; set; }
        public string other { get; set; }
        public bool toggle { get; set; }
        public string style { get; set; }
        public string FieldID { get; set; }
        public string ColumnID { get; set; }
        #endregion
    }

    public class OptionsModel
    {
        #region Property Section
        public string label { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        #endregion
    }

    public class SRFieldFormModel
    {
        #region Property Section
        public List<FieldPropertiesModel> lstFields { get; set; }
        public int? FormID { get; set; }
        public string FormTitle { get; set; }
        public bool IsMultiColumn { get; set; }
        public string JsonSchema { get; set; }
        public string JsonResult { get; set; }
        public List<FieldPropertiesModel> lstFirstColumn { get; set; }
        public List<FieldPropertiesModel> lstSecondColumn { get; set; }
        #endregion
    }
}
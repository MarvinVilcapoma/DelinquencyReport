using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class PrintLetterModel
    {
        #region "Properties"
        public int PrintTemplateID { get; set; } = 0;
        public List<SelectListItemViewModel> Templates { get; set; } = new List<SelectListItemViewModel>();
        public List<SelectListItemViewModel> OutputFormats { get; set; } = new List<SelectListItemViewModel> {
             new SelectListItemViewModel { ID=-1, Name = Resources.Global.DropDownSelectMessage  },
             new SelectListItemViewModel { ID=1, Name= Resources.Global.Word},
             new SelectListItemViewModel { ID=2, Name= Resources.Global.PDF }
        };
        public int OutputFormat { get; set; }
        public string Comments { get; set; }
        public string Date { get; set; }
        public Destinatary Destinatary { get; set; } = new Destinatary();
        public List<SelectListItemViewModel> DataSources { get; set; } = new List<SelectListItemViewModel>();
        public List<CaseModel> CaseModel { get; set; } = new List<CaseModel>();
        public int DataSourceID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public enum OutputFormatType
        {
            Word = 1,
            PDF = 2
        }
        public List<PrintLetterModel> PrintLetterModels { get; set; } = new List<PrintLetterModel>();
        public PrintLetterModel()
        {

        }
        public PrintLetterModel(string _date, string _comments, string _name, string _position, string _department)
        {
            Date = _date;
            Comments = _comments;
            Name = _name;
            Position = _position;
            Department = _department;
        }
        #endregion  "Properties"

        #region "Public Methods"
        public PrintLetterModel PrintLetter(string caseids, int status, int workflowId = 0)
        {
            PrintLetterModel PrintLetterModel = new PrintLetterModel();

            PrintLetterModel.CaseModel = new CaseModel().MasterCasesGet(-1, -1, 0, caseids);

            PrintLetterModel.Templates = new PrintTemplates.Models.PrintTemplate().Get(new PrintTemplates.Models.PrintTemplate
            {
                StatusID = status,
                WorkflowID = workflowId
            }).Select(a => new SelectListItemViewModel
            {
                ID = a.ID,
                Name = a.TemplateName
            }).ToList();

            PrintLetterModel.Templates.Insert(0, new SelectListItemViewModel
            {
                ID = -1,
                Name = Resources.Global.DropDownSelectMessage
            });

            return PrintLetterModel;
        }
        #endregion
    }
    public class Destinatary
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public Destinatary()
        {

        }
        public Destinatary(string _name, string _position, string _department)
        {
            Name = _name;
            Position = _position;
            Department = _department;
        }
    }
}
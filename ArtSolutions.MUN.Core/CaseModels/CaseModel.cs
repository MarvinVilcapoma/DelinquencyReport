using System;
using System.Collections.Generic;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class CaseModel
    {
        public int CompanyID { get; set; }
        public string Language { get; set; }
        public bool IsActie { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string Version { get; set; } = "1";
        public string FilterText { get; set; } = null;
        public int CaseID { get; set; }
        public string BussinessName { get; set; }
        public int PriorityID { get; set; }
        public int Weight { get; set; }
        public Guid AsignedTo { get; set; } = Guid.Empty;
        public string KeyCode { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string Sort { get; set; }
        public int WorkflowID { get; set; }
        public bool IsPublic { get; set; } = true;
        public int StatusID { get; set; }
        public int ReasonID { get; set; }
        public int AccountID { get; set; }
        public string Name { get; set; }
        public string Refrence { get; set; }
        public string Note { get; set; }
        public Guid OwnerID { get; set; } = Guid.Empty;
        public Guid CreatedUserID { get; set; } = Guid.Empty;
        public DateTime? CreatedDate { get; set; }
        public int DocumentType { get; set; } = 24;
        public string AccountServices { get; set; }
        public int FormID { get; set; }
        public List<WorkflowHistoryLog> WorkflowHistoryLogs { get; set; }
        public string CaseIDs { get; set; }
        public Guid ModifiedUserID { get; set; } = Guid.Empty;
        public DateTime? ModifiedDate { get; set; }
        public string StatusIDs { get; set; }
        public int ImageID { get; set; }
        public List<CaseImage> CaseImages { get; set; } = new List<CaseImage>();
        //public List<CaseModel> CaseModels { get; set; } = new List<CaseModel>();
    }
    public class CaseList
    {
        public CaseList()
        {
            CaseModels = new List<MUNITAXMasterCasesGetWithPaging_Result>();
        }
        public List<MUNITAXMasterCasesGetWithPaging_Result> CaseModels { get; set; }
        public int TotalRecord { get; set; }
    }
    public class WorkflowHistoryLog
    {
        public int CompanyID { get; set; }
        public int CaseID { get; set; }
        public int StatuIdSource { get; set; }
        public int StatuIdTarget { get; set; }
        public int WorkflowID { get; set; }
        public int WorkflowVersionID { get; set; } = 1;
        public int ReasonID { get; set; }
        public string Comment { get; set; }
        public Guid AssignToID { get; set; }
    }
    public class CaseImage
    {
        public int CaseID { get; set; }
        public int ImageID { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CaseImagesList
    {
        public List<MUNITAXCaseImagesGetWithPaging_Result> CaseImages { get; set; }
        public int TotalRecord { get; set; } = 0;
    }
}

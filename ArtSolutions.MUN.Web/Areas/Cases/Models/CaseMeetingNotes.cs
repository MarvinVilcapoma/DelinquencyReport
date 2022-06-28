using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using ArtSolutions.MUN.Web.Models;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Cases.Models
{
    public class CaseMeetingNotes
    {
        #region "Properties"
        public CaseMeetingNotes()
        {
            MeetingTypes = new List<SelectListItemViewModel>();
        }
        public int ID { get; set; }
        public int CaseID { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string Language { get; set; }
        public int MeetingTypeID { get; set; }
        public string Notes { get; set; }
        public string Date { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingType { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<SelectListItemViewModel> MeetingTypes { get; set; }
        public Guid ModifiedUserID { get; set; } = UserSession.Current.Id;
        public DateTime ModifiedDate { get; set; } = Common.CurrentDateTime;
        #endregion "Properties"

        #region "Public Methods"
        public List<SelectListItemViewModel> MeetingTypeGet()
        {
            return new RestSharpHandler().RestRequest<List<SelectListItemViewModel>>(APISelector.Municipality, true, "api/case/MeetingTypeGet", "GET", null);
        }
        public int Insert(CaseMeetingNotes model)
        {
            model.CreatedDate = Common.CurrentDateTime;
            model.CreatedUserID = UserSession.Current.Id;
            List<Object> parms = new List<object>();
            parms.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/MeetingNotesInsert", "POST", null, parms);
        }
        public CaseMeetingNotes Get(CaseMeetingNotes model)
        {
            CaseMeetingNotes caseMeetingNote = new CaseMeetingNotes();
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "id", Value = model.ID },
                new NameValuePair { Name = "caseID", Value = model.CaseID }
            };
            var caseMeetingNotesList = new RestSharpHandler().RestRequest<CaseMeetingNotesList>(APISelector.Municipality, true, "api/case/CaseMeetingNotesGet", "GET", lstParameter);
            if (caseMeetingNotesList.CaseMeetingList != null && caseMeetingNotesList.CaseMeetingList.Any())
            {
                caseMeetingNote.CaseID = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().CaseID;
                caseMeetingNote.MeetingDate = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().MeetingDate;
                caseMeetingNote.ID = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().ID;
                caseMeetingNote.MeetingTypeID = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().MeetingTypeID;
                caseMeetingNote.Notes = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().Notes;
                caseMeetingNote.MeetingType = caseMeetingNotesList.CaseMeetingList.FirstOrDefault().MeetingType;
            }
            return caseMeetingNote;
        }
        public int Update(CaseMeetingNotes model)
        {
            List<Object> parms = new List<object>
            {
                model
            };
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/case/MeetingNotesUpdate", "POST", null, parms);
        }
        #endregion "Public Methods"
    }
    public class CaseMeetingNotesList
    {
        #region "Properties"
        public List<CaseMeetingNotes> CaseMeetingList { get; set; } = new List<CaseMeetingNotes>();
        public int TotalRecords { get; set; }
        #endregion "Properties"

        #region "Public Methods"
        public CaseMeetingNotesList GetByPaging(JQDTParams jqdtParams, CaseMeetingNotes model)
        {
            CaseMeetingNotesList caseMeetingNotesList = new CaseMeetingNotesList();

            model.SortColumn = jqdtParams.Columns[jqdtParams.Order[0].Column].Name;

            model.CurrentIndex = (jqdtParams.Start + 1);

            model.PageSize = Common.PageSize;

            model.SortDirection = jqdtParams.Order[0].Dir.ToString();

            caseMeetingNotesList = GetByPaging(model);

            if (caseMeetingNotesList.CaseMeetingList != null && caseMeetingNotesList.CaseMeetingList.Any())
            {
                caseMeetingNotesList.CaseMeetingList.ForEach(a => a.Date = Convert.ToDateTime(a.Date).ToString("g"));
            }

            return caseMeetingNotesList;
        }
        
        #endregion

        #region "Private Methods"
        private CaseMeetingNotesList GetByPaging(CaseMeetingNotes model)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "caseID", Value = model.CaseID },
                new NameValuePair { Name = "currentIndex", Value = model.CurrentIndex },
                new NameValuePair { Name = "pageSize", Value = model.PageSize },
                new NameValuePair { Name = "sortColumn", Value = model.SortColumn },
                new NameValuePair { Name = "sortDirection", Value = model.SortDirection }
            };
            return new RestSharpHandler().RestRequest<CaseMeetingNotesList>(APISelector.Municipality, true, "api/case/CaseMeetingNotesGetByPaging", "GET", lstParameter);
        }

        #endregion
    }
}
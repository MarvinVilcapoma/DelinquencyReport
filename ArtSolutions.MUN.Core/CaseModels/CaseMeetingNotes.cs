using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ArtSolutions.MUN.Core.CaseModels
{
    public class CaseMeetingNotes
    {
        public int ID { get; set; }
        public int CaseID { get; set; }
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string Language { get; set; }
        public int MeetingTypeID { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public string MeetingType { get; set; }
        public Guid CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CompanyId { get; set; }
        public DateTime MeetingDate { get; set; }
        public CaseMeetingNotesList GetWithPaging(CaseMeetingNotes model)
        {
            CaseMeetingNotesList caseMeetingNotesList = new CaseMeetingNotesList();
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecords", typeof(int));

                var meetinglist = context.MUNITAXCaseMeetingNotesGetWithPaging(model.CaseID, model.CurrentIndex, model.PageSize, TotalRecord, model.SortColumn,
                    model.SortDirection, model.Language, model.CompanyId).ToList();

                caseMeetingNotesList.CaseMeetingList = meetinglist.Select(a => new CaseMeetingNotes
                {
                    CaseID = model.CaseID,
                    ID = a.ID,
                    MeetingTypeID = a.MeetingTypeID,
                    MeetingType = a.MeetingType,
                    Notes = a.Notes,
                    Date = a.Date
                }).ToList();

                caseMeetingNotesList.TotalRecords = Convert.ToInt32(TotalRecord.Value);
            }
            return caseMeetingNotesList;
        }
        public List<MUNITAXMeetingTypeGet_Result> MeetingTypeGet(CaseMeetingNotes model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXMeetingTypeGet(model.Language).ToList();
            }
        }
        public int Insert(CaseMeetingNotes model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCaseMeetingNotesInsert(model.CaseID, model.MeetingTypeID, model.Date, model.Notes,
                    model.CreatedUserID, model.CreatedDate);
            }
        }
        public int Update(CaseMeetingNotes model)
        {
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                return context.MUNITAXCaseMeetingNotesUpdate(model.ID, model.CaseID, model.MeetingTypeID, model.Date, model.Notes,
                    model.ModifiedUserID, model.ModifiedDate, model.CompanyId);
            }
        }
        public CaseMeetingNotesList Get(CaseMeetingNotes model)
        {
            CaseMeetingNotesList caseMeetingNotesList = new CaseMeetingNotesList();
            using (CaseDataModelContainer context = new CaseDataModelContainer())
            {
                ObjectParameter TotalRecord = new ObjectParameter("TotalRecords", typeof(int));

                var meetinglist = context.MUNITAXCaseMeetingNotesGet(model.CaseID, model.Language, model.CompanyId, model.ID).ToList();

                caseMeetingNotesList.CaseMeetingList = meetinglist.Select(a => new CaseMeetingNotes
                {
                    CaseID = model.CaseID,
                    ID = a.ID,
                    MeetingTypeID = a.MeetingTypeID,
                    MeetingType = a.MeetingType,
                    Notes = a.Notes,
                    MeetingDate = a.Date
                }).ToList();
            }
            return caseMeetingNotesList;
        }
    }
    public class CaseMeetingNotesList
    {
        public CaseMeetingNotesList()
        {
            CaseMeetingList = new List<CaseMeetingNotes>();
        }
        public List<CaseMeetingNotes> CaseMeetingList { get; set; }
        public int TotalRecords { get; set; }
    }
}

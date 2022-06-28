using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Collections.Models
{
    public class PostingProcess
    {
        #region Public methods  
        public int Insert(string JournalID, string DocuemntTypeDetail, DateTime AsOfDate, int workflowID, int workflowStatusID,int? PaymentType=null)
        {
            if (!string.IsNullOrWhiteSpace(JournalID))
            {
                JournalID = JournalID.Trim(',');
                JournalID = JournalID.Replace(",,", ",");
            }
            PostingProcessJournal model = new PostingProcessJournal();
            model.JournalID = JournalID;
            model.DocuemntTypeDetail = DocuemntTypeDetail;
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.AsOfDate = AsOfDate;
            model.WorkflowID = workflowID;
            model.WorkflowVerionID = 1;
            model.WorkflowStatusID = workflowStatusID;
            model.PaymentType = PaymentType;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PostingProcess/Insert", "POST", null, lstObjParameter);
        }

        public List<JournalListForPosting> Get(int ID)
        {
            List<JournalListForPosting> list = new List<JournalListForPosting>();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            list = new RestSharpHandler().RestRequest<List<JournalListForPosting>>(APISelector.Municipality, true, "api/PostingProcess/GET", "GET", lstParameter, null);
            list.ForEach(x => { x.Status = x.IsVoid > 0 ? Resources.Global.Void : x.IsPost > 0 ? Resources.Global.Posted : Resources.Global.Draft; });
            return list;
        }

        public JournalListForPosting LatInsertedGet(int PaymentType)
        {
            JournalListForPosting list = new JournalListForPosting();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "PaymentType", Value = PaymentType });
            list = new RestSharpHandler().RestRequest<JournalListForPosting>(APISelector.Municipality, true, "api/PostingProcess/LatInsertedGet", "GET", lstParameter, null);
            return list;
        }

        public List<NewPostingProcessDetails> GetWithGroupSum(int ID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = ID });
            var list = new RestSharpHandler().RestRequest<List<NewPostingProcessDetails>>(APISelector.Municipality, true, "api/PostingProcess/GetWithGroupSum", "GET", lstParameter, null);
            return list;
        }

        public PostingProcessListModel GetWithPaging(JQDTParams jqdtParams, string filterText, string DocumentTypeID)
        {
            DateTime outDate;
            decimal outDecimal;
            PostingProcessListModel list = new PostingProcessListModel();
            //Filter for Void/Post Status
            bool? isvoid = null;
            if (Decimal.TryParse(filterText, out outDecimal))
            {
                try
                {                    
                    filterText= Convert.ToDecimal(filterText).ToString(CultureInfo.InvariantCulture);
                }
                catch { }
            }
            if (DateTime.TryParse(filterText, out outDate))
            {
                try
                {
                    filterText = outDate.ToString("d", CultureInfo.InvariantCulture);
                }
                catch { }
            }
            if (filterText.Trim().ToLower() == Resources.Global.Void.ToLower())
            {
                isvoid = true;
                filterText = null;
            }
            else if (filterText.Trim().ToLower() == Resources.Global.Posted.ToLower())
            {
                isvoid = false;
                filterText = null;
            }
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "ID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "FilterText", Value = filterText });
            lstParameter.Add(new NameValuePair { Name = "IsVoid", Value = isvoid });
            lstParameter.Add(new NameValuePair { Name = "DocumentTypeID", Value = DocumentTypeID });
            lstParameter.Add(new NameValuePair { Name = "PageIndex", Value = jqdtParams.Start });
            lstParameter.Add(new NameValuePair { Name = "PageSize", Value = jqdtParams.Length });
            lstParameter.Add(new NameValuePair { Name = "SortColumn", Value = jqdtParams.Columns[jqdtParams.Order[0].Column].Name });
            lstParameter.Add(new NameValuePair { Name = "SortOrder", Value = Convert.ToString(jqdtParams.Order[0].Dir) });
            list = new RestSharpHandler().RestRequest<PostingProcessListModel>(APISelector.Municipality, true, "api/PostingProcess/GetWithPaging", "GET", lstParameter, null);
            return list;
        }

        public List<JournalListForPosting> GetJournalListForPosting(DateTime AsOfDate, string DocumentTypeID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "StartPeriodDate", Value = AsOfDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "EndPeriodDate", Value = AsOfDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "DocumentTypeID", Value = DocumentTypeID });
            List<JournalListForPosting> model = new RestSharpHandler().RestRequest<List<JournalListForPosting>>(APISelector.Municipality, true, "api/PostingProcess/JournalDetailForPostingProcess", "GET", lstParameter);
            model.ForEach(d => { d.Status = d.IsVoid > 0 ? Resources.Global.Voided : d.IsPost > 0 ? Resources.Global.Posted : Resources.Global.Draft; });
            return model;
        }
        
        public int Update(int ID, int WorkFlowStatusID, bool Ispost)
        {
            PostingProcessModel model = new PostingProcessModel();
            model.ID = ID;
            model.WorkflowStatusID = WorkFlowStatusID;

            if (Ispost)
            {
                List<JournalListForPosting> journalList = new PostingProcess().Get(ID);
                List<int> journalIDList = journalList.Select(d => d.ID).Distinct().ToList();

                //Create list of JournalEntries for Finance
                List<FINJournalModel> FINJournalList = new List<FINJournalModel>();
                journalIDList.ForEach(k =>
                {
                    var journal = journalList.FirstOrDefault(d => d.ID == k);
                    if (journal != null)
                    {
                        int index = 1;
                        FINJournalModel FINJournal = new FINJournalModel
                        {
                            ID = journal.ID,
                            CompanyID = journal.CompanyID,
                            DocumentType = journal.DocumentTypeID,
                            Date = journal.Date,
                            FiscalYearID = journal.FiscalYearID,
                            PeriodID = journal.FiscalYearPeriodID,
                            Memo = journal.Memo,
                            IsPost = journal.IsPost == 1,
                            IsVoid = journal.IsVoid == 1,
                            Amount = journal.Amount,
                            CurrencyCode = journal.CurrencyCode,
                            CurrencyID = journal.CurrencyID,
                            ExchangeRate = journal.ExchangeRate ?? 1,
                            ApplicationSource = journal.ApplicationSource,
                            CreatedUserID = UserSession.Current.Id,
                            CreatedDate = DateTime.Now,
                            ModifiedUserID = UserSession.Current.Id,
                            ModifiedDate = DateTime.Now,
                            BankAccountID = journal.BankID,
                            Number = journal.ReceiptNumber,
                            AccountID = journal.ReferenceAccountID,
                            DisplayName = journal.ReferenceAccountName,
                            FinJournalID = journal.FINJournalID,
                            AccountList = journalList.Where(d => d.ID == journal.ID).ToList().GroupBy(d => new { d.AccountID, d.GrantID, d.ReferenceID, d.ReferenceNumber, d.ReferenceAccountID, d.ReferenceAccountName }).Select(d => new FINJournalDetail
                            {
                                JounalLine = index++,
                                AccountID = d.Key.AccountID,
                                GrantID = d.Key.GrantID,
                                ReferenceID = d.Key.ReferenceID,
                                ReferenceNumber = d.Key.ReferenceNumber,
                                ReferenceCustomerVendorEmployeID = d.Key.ReferenceAccountID.ToString(),
                                ReferenceCustomerVendorEmployeName = d.Key.ReferenceAccountName,
                                Debit = d.Sum(j => j.Debit),
                                Credit = d.Sum(j => j.Credit)
                            }).ToList()
                        };
                        FINJournalList.Add(FINJournal);
                    }
                });

                bool UpdateFinJournalStaus = true, AllJouranlSynsWithFIN = true;
                string Message = "";

                //Make insert in Finance
                FINJournalList.ForEach(d =>
                {
                    try
                    {
                        d.FinJournalID = new PostingProcess().JournalIntegration(d);
                    }
                    catch (Exception ex)
                    {
                        AllJouranlSynsWithFIN = false;
                        Message += String.Format("{0} : {1}\n", d.ID, ex.Message);
                        d.IntegrationError = ex.Message;
                    }
                });
                string FINJournalListJSON = JsonConvert.SerializeObject(FINJournalList.Select(d => new { JournalID = d.ID, FINJournalID = d.FinJournalID, IntegrationError = d.IntegrationError, FINJournalEntrySequence = d.FINJournalEntrySequence }).ToList());
                //Update Finance Journal ID 
                UpdateFinJournalStaus = UpdateFINJournalID(FINJournalListJSON);

                if (UpdateFinJournalStaus && AllJouranlSynsWithFIN)
                {
                    //Update Posting process record
                    return Update(model);
                }
            }
            else
            {
                return Update(model);
            }
            return 2;
        }

        public NewPostingProcessModel NewPostingProcessBulkJournalInsert(NewPostingProcessModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<NewPostingProcessModel>(APISelector.Municipality, true, "api/PostingProcess/NewPostingProcessBulkJournalInsert", "POST", null, lstObjParameter);
        }
        #endregion

        #region Private methods  
        private int JournalIntegration(FINJournalModel model)
        {
            model.CreatedUserID = UserSession.Current.Id;
            model.CreatedDate = Common.CurrentDateTime;
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/Finance/JournalMunicipalityInsert", "POST", null, lstObjParameter);
        }

        private bool UpdateFINJournalID(string FINJournalListJSON)
        {
            JournalSyns model = new JournalSyns();
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            model.FINJournalListJSON = FINJournalListJSON;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/PostingProcess/UpdateFINJournalID", "POST", null, lstObjParameter);
        }

        private int Update(PostingProcessModel model)
        {
            model.ModifiedUserID = UserSession.Current.Id;
            model.ModifiedDate = Common.CurrentDateTime;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(model);
            return new RestSharpHandler().RestRequest<int>(APISelector.Municipality, true, "api/PostingProcess/Update", "POST", null, lstObjParameter);
        }
        #endregion
    }
}
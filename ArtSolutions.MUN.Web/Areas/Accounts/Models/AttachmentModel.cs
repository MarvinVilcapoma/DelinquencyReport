using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AttachmentModel
    {
        #region public properties       
        public int ID { get; set; }
        public int SequenceID { get; set; }
        public int AccountID { get; set; }
        public int FileID { get; set; }
        public Nullable<int> FileTypeID { get; set; }
        public Nullable<int> FileTypeTableID { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        #endregion

        #region public methods    
        public List<AttachmentModel> Get(int accountID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "fileID", Value = null });
            lstParameter.Add(new NameValuePair { Name = "accountID", Value = accountID });
            return new RestSharpHandler().RestRequest<List<AttachmentModel>>(APISelector.Municipality, true, "api/Account/FileGet", "GET", lstParameter);
        }

        public AttachmentModel GetTypes()
        {
            List<SupportValueModel> _SupportValueModel = new AccountModel().GetSupportValues(10);
            this.TypeList = HMTLHelperExtensions.CreateSelectList(_SupportValueModel, "ID", "Name");
            return this;
        }
        #endregion
    }
}
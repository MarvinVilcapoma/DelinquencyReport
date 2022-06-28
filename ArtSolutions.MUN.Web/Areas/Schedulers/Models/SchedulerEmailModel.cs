using ArtSolutions.MUN.Web.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Schedulers.Models
{
    public class SchedulerEmailModel
    {
        #region public properties       
        public int ID { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredValidationMsg", ErrorMessageResourceType = typeof(Global))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "ValidEmailAddressMsg", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedUserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid ModifiedUserID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string ReasonForDeleted { get; set; }
        #endregion
    }
    public class SchedulerEmailList
    {
        #region Properties
        public IEnumerable<SelectListItem> ServiceSearch { get; set; }
        public List<SchedulerEmailModel> SchedulerEmailModelList { get; set; }
        public Int32 TotalRecord { get; set; }
        #endregion
    }
}
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountPropertyConstructionDetail
    {
        #region Methods
        public AccountPropertyConstructionDetailModel Get()
        {
            AccountPropertyConstructionDetailModel model = new AccountPropertyConstructionDetailModel();
            AccountProperty AccountProperty = new AccountProperty();
            model.MaterialTypeList = AccountProperty.GetSupportValues(28).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.ConstructionTypeList = AccountProperty.GetSupportValues(29).OrderBy(x => x.ID).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.StatusList = AccountProperty.GetSupportValues(30).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.StructureList = AccountProperty.GetSupportValues(31).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.RoofList = AccountProperty.GetSupportValues(32).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.CeilingList = AccountProperty.GetSupportValues(33).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.FloorsList = AccountProperty.GetSupportValues(34).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.InternalWallsList = AccountProperty.GetSupportValues(36).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            model.ExternalWallsList = AccountProperty.GetSupportValues(37).Select(d => new SelectListItem { Text = d.Name, Value = d.ID.ToString() }).ToList();
            return model;
        }
        #endregion
    }
}
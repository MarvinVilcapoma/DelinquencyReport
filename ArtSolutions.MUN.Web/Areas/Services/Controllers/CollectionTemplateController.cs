using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Controllers
{
    [Filters.IsCompanyConfigured]
    public class CollectionTemplateController : BaseController
    {
        bool status = false;
        string msg = "";

        #region List Collection Template
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                ServiceCollectionTemplateListModel list = new ServiceCollectionTemplateModel().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.CollectionTemplateList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region New/Edit Collection Template
        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                ServiceCollectionTemplateModel model = new ServiceCollectionTemplateModel();
                model.Get();
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            try
            {
                ServiceCollectionTemplateModel model = new ServiceCollectionTemplateModel().Edit(ID);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Save(ServiceCollectionTemplateModel model, int actionType)
        {
            try
            {
                //Exclude Removed List
                if (model.ServiceCollectionTemplateDetailList != null)
                    model.ServiceCollectionTemplateDetailList = model.ServiceCollectionTemplateDetailList.Where(x => x.IsRemoved == false).ToList();
                //Validate each items
                var isvalid = model.ValidateItems(model.ServiceCollectionTemplateDetailList, model.YearList, model.CollectionTypeList, true, true, true);
                //Add-Edit
                int result = model.ID == 0 ? model.Insert(model) : model.Update(model);
                if (result > 0)
                {
                    status = true;
                    TempData["SuccessMsg"] = ArtSolutions.MUN.Web.Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, actionType = actionType }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Collection Template
        [HttpGet]
        public ActionResult View(int ID)
        {
            try
            {
                ServiceCollectionTemplateModel model = new ServiceCollectionTemplateModel().Edit(ID);
                model.EnableViewMode = true;
                return View("Edit", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public JsonResult AddDetailRow(ServiceCollectionTemplateModel model)
        {
            try
            {
                ModelState.Clear();
                model = model.AddDetail(int.Parse(Request["yearId"]));
                string htmlData = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/CollectionTemplate/_PanelList.cshtml", model);
                return Json(new { status = true, data = htmlData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGrantAccounts(int grantId, int idx)
        {
            FinGrantAccountModel model = new FinGrantAccountModel();
            try
            {
                model = new FinGrantAccountModel().Get(grantId, EnumUtility.FINAccountType.AR);
                ViewBag.Id = "ServiceCollectionTemplateDetailList_" + idx + "__ReceivableAccountID";
                ViewBag.Name = "ServiceCollectionTemplateDetailList[" + idx + "].ReceivableAccountID";
                ViewBag.Class = "ddlaccountReceivable";
                string accountReceivable = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/CollectionTemplate/_AccountOptionSelectList.cshtml", model.ReceivableAccountList);
                ViewBag.Id = "ServiceCollectionTemplateDetailList_" + idx + "__RevenueAccountID";
                ViewBag.Name = "ServiceCollectionTemplateDetailList[" + idx + "].RevenueAccountID";
                ViewBag.Class = "ddlaccountRevenue";
                string accountRevenue = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/CollectionTemplate/_AccountOptionSelectList.cshtml", model.RevenueAccountList);
                ViewBag.Id = "ServiceCollectionTemplateDetailList_" + idx + "__CreditAccountID";
                ViewBag.Name = "ServiceCollectionTemplateDetailList[" + idx + "].CreditAccountID";
                ViewBag.Class = "ddlaccountCredit";
                string accountCredit = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/CollectionTemplate/_AccountOptionSelectList.cshtml", model.RevenueAccountList);

                ViewBag.Id = "ServiceCollectionTemplateDetailList_" + idx + "__CheckbookID";
                ViewBag.Name = "ServiceCollectionTemplateDetailList[" + idx + "].CheckbookID";
                ViewBag.Class = "ddlCheckbook";
                string Checkbook = HMTLHelperExtensions.RenderPartialToString(this, "~/Areas/Services/Views/CollectionTemplate/_HiddenFormFieldList.cshtml", model.CheckbookAccountList);


                return Json(new { status = true, accountreceivable = accountReceivable, accountrevenue = accountRevenue, accountcredit = accountCredit, checkbook = Checkbook }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                //if(model.CheckbookAccountList.Count()<=0)
                if ((model.CheckbookAccountList == null && model.CheckbookAccountList.Count() <= 0) || (model.CheckbookAccountList != null && model.CheckbookAccountList.Count() <= 0))
                    Message = ArtSolutions.MUN.Web.Resources.ServiceCollectionTemplate.NotAvailableCheckbook;
                return Json(new { status = false, message = Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
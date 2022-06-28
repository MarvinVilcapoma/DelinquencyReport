using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using ArtSolutions.MUN.Web.Resources;
using System;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Services.Controllers
{
    public class PaymentPlanController : BaseController
    {
        #region List Service Payment Plan
        public ActionResult List()
        {
            return View("~/Areas/Services/Views/PaymentPlan/List.cshtml");
        }

        [HttpPost]
        public JsonResult List(JQDTParams jqdtParams, string filterText = null)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                PaymentPlanListModel list = new PaymentPlanList().GetByPaging(filterText, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir));
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.PaymentPlanList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        #endregion

        #region New/Edit Collection Template
        public JsonResult ActiveDeactive(bool isActive, int id)
        {
            bool status = false;
            string msg = string.Empty;

            try
            {
                int ID = new Services.Models.PaymentPlan().UpdateStatus(isActive, id);
                if (ID > 0)
                {
                    status = true;
                    msg = isActive ? Global.ActiveMessage : Global.DeActiveMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            PaymentPlanModel model = new PaymentPlanModel();
            try
            {
                model = new Services.Models.PaymentPlan().Get();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Services/Views/PaymentPlan/Add.cshtml", model);
        }

        [HttpPost]
        public ActionResult Add(PaymentPlanModel model, int actionType)
        {
            bool status = false;
            string msg = string.Empty;

            try
            {
                if (model.EffectiveTo <= model.EffectiveFrom)
                {
                    status = false;
                    msg = Resources.PaymentPlan.EffectiveToShouldBeGreaterThanEffectiveFromMsg;
                }
                else
                {
                    int result = new Services.Models.PaymentPlan().Insert(model);
                    if (result > 0)
                    {
                        status = true;
                        TempData["SuccessMsg"] = Global.SavedSuccessfullyMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, actionType = actionType }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int? paymentPlanID, int actionType)
        {
            if (paymentPlanID == null)
                return RedirectToAction("NotFoundError", "Home", new { area = "" });

            PaymentPlanModel model = new PaymentPlanModel();
            try
            {
                ViewBag.ActionType = actionType;
                model = new Services.Models.PaymentPlan().Get(paymentPlanID);
                if (model == null || model.ID == 0)
                {
                    TempData["ErrorMsg"] = Resources.PaymentPlan.InvalidPaymentPlan;
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Services/Views/PaymentPlan/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(PaymentPlanModel model)
        {
            bool status = false;
            string msg = string.Empty;

            try
            {
                if (model.EffectiveTo <= model.EffectiveFrom)
                {
                    status = false;
                    msg = Resources.PaymentPlan.EffectiveToShouldBeGreaterThanEffectiveFromMsg;
                }
                else
                {
                    int result = new Services.Models.PaymentPlan().Update(model);
                    status = true;
                    TempData["SuccessMsg"] = Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg, actionType = 1 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
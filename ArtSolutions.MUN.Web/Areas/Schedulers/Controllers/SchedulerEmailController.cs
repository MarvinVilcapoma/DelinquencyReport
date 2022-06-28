using ArtSolutions.MUN.Web.Areas.Accounts.Models;
using ArtSolutions.MUN.Web.Areas.Schedulers.Models;
using ArtSolutions.MUN.Web.Areas.Services.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Schedulers.Controllers
{
    [Filters.IsCompanyConfigured]
    public class SchedulerEmailController : BaseController
    {
        bool status = false;
        string msg = string.Empty;
        string htmlstring = string.Empty;

        #region List Service
        public ActionResult List()
        {
            SchedulerEmailList list = new SchedulerEmailList();
            try
            {
                list.SchedulerEmailModelList = new List<SchedulerEmailModel>();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return View("~/Areas/Schedulers/Views/SchedulerEmail/List.cshtml", list);
        }

        [HttpPost]
        public ActionResult Get(JQDTParams jqdtParams, string filterText)
        {
            JsonResult jResult = new JsonResult();
            DateTime outDate;

            try
            {
                if (DateTime.TryParse(filterText, out outDate))
                {
                    try
                    {
                        filterText = Convert.ToDateTime(filterText).ToString("d", CultureInfo.InvariantCulture);
                    }
                    catch { }
                }

                SchedulerEmailList list = new SchedulerEmail().Get(null, filterText, jqdtParams.Start, jqdtParams.Length, jqdtParams.Columns[jqdtParams.Order[0].Column].Name, Convert.ToString(jqdtParams.Order[0].Dir));
                
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.SchedulerEmailModelList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        
        #endregion

        #region New/Edit Scheduler Email 
        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                SchedulerEmailModel Model = new SchedulerEmailModel();
                return View("~/Areas/Schedulers/Views/SchedulerEmail/Add.cshtml", Model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Add(SchedulerEmailModel model, int actionType)
        {
            try
            {
                int result = new SchedulerEmail().Insert(model);
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

        [HttpGet]
        public ActionResult Edit(int schedulerEmailID)
        {
            try
            {
                SchedulerEmailModel model = new SchedulerEmail().Edit(schedulerEmailID);
                if (model.ID == 0)
                {
                    TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.InvalidScheduleEmailNumber;
                    return RedirectToAction("List");
                }
                return View("~/Areas/Schedulers/Views/SchedulerEmail/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Edit(SchedulerEmailModel model)
        {
            try
            {
               
                int result = new SchedulerEmail().Update(model);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActiveDeactive(bool isActive, int id)
        {
            try
            {
                if (id > 0)
                {
                    int ID = new SchedulerEmail().UpdateStatus(isActive, id);
                    if (ID > 0)
                        status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
                TempData["ErrorMsg"] = ex.Message;
            }
            return Json(new { status = status, message = TempData["ErrorMsg"] }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id, string Reason, bool? isWindowReload = null)
        {
            try
            {
                new SchedulerEmail().Delete(id, Reason);
                status = true;
                msg = ArtSolutions.MUN.Web.Resources.Global.DeleteSchedulerEmailSuccMsg;

                if (isWindowReload == true)
                    TempData["SuccessMsg"] = msg;
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
                TempData["ErrorMsg"] = msg;
            }
            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
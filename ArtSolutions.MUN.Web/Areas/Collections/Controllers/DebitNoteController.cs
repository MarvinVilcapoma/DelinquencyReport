using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class DebitNoteController : BaseController
    {
        public ActionResult Index()
        {
            return View();
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
                DebitNoteListModel list = new DebitNote().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.DebitNoteList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult View(int? ID, bool? IsDH = false)
        {
            if (ID == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            DebitNotePrintModel model = new DebitNotePrintModel();
            try
            {
                ViewBag.IsViewMode = true;
                if (!IsDH.Value)
                {   //DebitNote
                    model = new DebitNote().GetPrint(ID.Value);
                    if (model.DebitNote == null || model.DebitNote.ID == 0)
                    {
                        TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("~/Areas/Collections/Views/DebitNote/View.cshtml", model);
                    }
                }
                else
                {   //DebitHistory
                    Areas.Accounts.Models.DebitHistoryDetailModel debitHistoryModel = new Areas.Accounts.Models.DebitHistoryDetailModel();
                    debitHistoryModel = new Areas.Accounts.Models.DebitHistoryDetailModel().DebitHistoryDetailGet(ID.Value);
                    if (debitHistoryModel.DebitHistory == null || debitHistoryModel.DebitHistory.ID == 0)
                    {
                        TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("~/Areas/Accounts/Views/Account/DebitHistoryDetail.cshtml", debitHistoryModel);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(int ID, int WorkFlowStatusID)
        {
            bool Status = false;
            string Message = "";
            if (ID <= 0 || WorkFlowStatusID <= 0)
            {
                Message = Resources.Global.NoDataMessage;
            }

            try
            {

                int Retval = new DebitNote().Update(ID, WorkFlowStatusID);
                if (Retval == 0)
                {
                    Message = Resources.Global.InvalidAction;
                }
                else if (Retval == 1)
                {
                    Status = true;
                    Message = Resources.Global.SavedSuccessfullyMessage;

                }
                else
                {
                    Message = Resources.Global.ValInternalError;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(new { status = Status, message = Message }, JsonRequestBehavior.AllowGet);
        }
    }
}
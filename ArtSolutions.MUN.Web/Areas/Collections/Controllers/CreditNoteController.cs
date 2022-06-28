using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Controllers;
using ArtSolutions.MUN.Web.Models;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Collections.Controllers
{
    public class CreditNoteController : BaseController
    {
        #region Credit Note
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
                CreditNoteListModel list = new CreditNote().GetWithPaging(jqdtParams, filterText);
                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = list.TotalRecord, recordsTotal = list.TotalRecord, data = list.CreditNoteList });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        public ActionResult View(int? ID, bool? IsCH=false)
        {
            if (ID == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }

            CreditNotePrintModel model = new CreditNotePrintModel();
            try
            {
                ViewBag.IsViewMode = true;
                if (!IsCH.Value)
                {   //CreditNote
                    model = new CreditNote().GetPrint(ID.Value);
                    if (model.CreditNote == null || model.CreditNote.ID == 0)
                    {
                        TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("~/Areas/Collections/Views/CreditNote/View.cshtml", model);
                    }
                }
                else
                {   //CreditHistory
                    Accounts.Models.CreditHistoryDetailModel CredHistoryModel = new Accounts.Models.CreditHistoryDetailModel();
                    CredHistoryModel = new Accounts.Models.CreditHistoryModel().GetCreditHistoryDetail(ID.Value);
                    if (CredHistoryModel.CreditHistory == null || CredHistoryModel.CreditHistory.ID == 0)
                    {
                        TempData["ErrorMsg"] = ArtSolutions.MUN.Web.Resources.Global.NoDataMessage;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("~/Areas/Accounts/Views/Account/CreditHistoryDetail.cshtml", CredHistoryModel);
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

                int Retval = new CreditNote().Update(ID, WorkFlowStatusID);
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
        #endregion
    }
}
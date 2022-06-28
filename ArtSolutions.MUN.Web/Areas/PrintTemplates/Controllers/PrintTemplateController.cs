using System;
using System.Web;
using System.Web.Mvc;
using ArtSolutions.MUN.Web.Areas.PrintTemplates.Models;
using ArtSolutions.MUN.Web.Models;
using System.IO;
using ArtSolutions.MUN.Web.Controllers;

namespace ArtSolutions.MUN.Web.Areas.PrintTemplates.Controllers
{
    public class PrintTemplateController : BaseController
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public ActionResult List(JQDTParams jqdtParams, PrintTemplate model)
        {
            JsonResult jResult = new JsonResult();
            try
            {
                var printTemplateList = new PrintTemplate().GetByPaging(jqdtParams, model);

                jResult = Json(new { draw = jqdtParams.Draw, recordsFiltered = printTemplateList.TotalRecord, recordsTotal = printTemplateList.TotalRecord, data = printTemplateList.PrintTemplates });
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return jResult;
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View(new PrintTemplate().Add());
        }
        [HttpPost]
        public ActionResult Add(PrintTemplate model)
        {
            ResponceMessage responceMessage = new ResponceMessage();
            JsonResult jResult = new JsonResult();
            try
            {
                responceMessage.Status = new PrintTemplate().Save(model).ID > 0;
                if (responceMessage.Status)
                {
                    responceMessage.Message = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            jResult = Json(responceMessage, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        public ActionResult UploadFileTemporary()
        {
            var model = new PrintTemplate().UploadFileTemporary(Request.Files[0]);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }
            return View(new PrintTemplate().Edit(id.Value));
        }
        [HttpPost]
        public ActionResult Edit(PrintTemplate model)
        {
            ResponceMessage responceMessage = new ResponceMessage();
            JsonResult jResult = new JsonResult();
            try
            {
                responceMessage.Status = new PrintTemplate().Edit(model).ID > 0;
                if (responceMessage.Status)
                {
                    responceMessage.Message = Resources.Global.SavedSuccessfullyMessage;
                }
            }
            catch (Exception ex)
            {
                responceMessage.Status = false;
                responceMessage.Message = ex.Message;
            }
            jResult = Json(responceMessage, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        [HttpGet]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundError", "Home", new { area = "" });
            }
            return View(new PrintTemplate().View(id.Value));
        }
       
    }

}
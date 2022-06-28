using ArtSolutions.MUN.Web.Helpers;
using ArtSolutions.MUN.Web.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Controllers
{
    public class FileController : Controller
    {
        public JsonResult UploadFile()
        {
            int imageID = 0;
            try
            {
                if (Request.Form["imageID"] != null)
                    imageID = Convert.ToInt32(Request.Form["imageID"]);

                var request = new RestRequest("", Method.POST);

                foreach (string fileName in Request.Files)
                {
                    request.AddHeader("Content-Type", "multipart/form-data");
                    HttpPostedFileBase file = Request.Files[fileName];

                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    request.AddParameter("Id", imageID);
                    request.AddParameter("Authorization", UserSession.Current.Token, ParameterType.HttpHeader);
                    request.AddParameter("Company", UserSession.Current.CompanyID, ParameterType.HttpHeader);
                    request.AddParameter("Language", UserSession.Current.Language, ParameterType.HttpHeader);
                    request.AddParameter("IsListedInFolders", false);
                    request.AddParameter("IsPublicImage", false);

                    request.AddFile("file", data, file.FileName, file.ContentType);

                    var restUrl = Common.URLServicePath + "api/file/UploadFile";
                    var client = new RestClient(restUrl);
                    IRestResponse<FileModel> response = client.Execute<FileModel>(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var badRequestMessage = JsonConvert.DeserializeObject<APIMessage>(response.Content);
                        if (badRequestMessage != null)
                            throw new Exception(badRequestMessage.Message);
                        else
                            throw new Exception(response.Content);
                    }                    
                    return Json(new {  id = response.Data.ID }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { id = 0  , message = ArtSolutions.MUN.Web.Resources.Global.GeneralErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {               
                return Json(new { id = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetFileData(int id)
        {
            byte[] data;

            var restUrl = Common.URLServicePath + "api/file/DownloadFile";
            var client = new RestClient(restUrl);
            var request = new RestRequest("", Method.GET);

            request.AddParameter("Authorization", UserSession.Current.Token, ParameterType.HttpHeader);
            request.AddParameter("Company", UserSession.Current.CompanyID, ParameterType.HttpHeader);
            request.AddParameter("Id", id);

            IRestResponse response = client.Execute(request);
            data = JsonConvert.DeserializeObject<byte[]>(response.Content);
            return Encoding.ASCII.GetString(data);
        }

        public ActionResult DownloadFile(int id)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>
            {
                new NameValuePair { Name = "Id", Value = id }
            };
            FileModel model = new RestSharpHandler().RestRequest<FileModel>(APISelector.Municipality, true, "api/file/DownloadFile", "GET", lstParameter);
            return File(model.Data, System.Net.Mime.MediaTypeNames.Application.Octet, (model.FileName.IndexOf(".") != -1 ? model.FileName : (model.FileName + "" + model.FileExtension)));
        }
    }
}
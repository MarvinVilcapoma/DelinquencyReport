using ArtSolutions.MUN.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Controllers
{
    public class FileController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";

        [HttpPost]
        [Route("api/file/UploadFile")]
        public IHttpActionResult UploadFile()
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                //Get Headers from Request
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                //Validation
                //ArtSolutions.Security.Core.User.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Core.Actions.New);
                //Call
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    // Get file content as byte array
                    MemoryStream ms = new MemoryStream();

                    httpRequest.Files[0].InputStream.CopyTo(ms);
                    byte[] fileContent = ms.ToArray();

                    FileModel model = new FileModel();
                    model.ID = Convert.ToInt32(httpRequest.Form["Id"]);
                    model.CompanyID = companyID;
                    model.Data = fileContent;
                    model.Length = httpRequest.Files[0].ContentLength;
                    model.ContentType = httpRequest.Files[0].ContentType;
                    model.FileName = httpRequest.Files[0].FileName;
                    model.FileExtension = Path.GetExtension(httpRequest.Files[0].FileName);
                    model.IsListedInFolders = Convert.ToBoolean(httpRequest.Form["IsListedInFolders"]);
                    model.IsPublicImage = Convert.ToBoolean(httpRequest.Form["IsPublicImage"]);

                    if (Convert.ToInt32(httpRequest.Form["FolderID"]) > 0)
                        model.FolderID = Convert.ToInt32(httpRequest.Form["FolderID"]);

                    ArtSolutions.MUN.Core.FileEngine file = new Core.FileEngine();
                    int Id = 0;
                    if (model.ID == 0)
                        Id = file.Insert(model);
                    else
                        Id = file.Update(model);

                    model.Data = null;
                    model.ID = Id;
                    return Ok(model);
                }
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.FileEngine.InsertUpdate", language, ex));
            }
        }

        [HttpGet]
        [Route("api/file/DownloadFile")]
        public IHttpActionResult DownloadFile(int id)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                //Get Headers from Request
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                //Validation
                //ArtSolutions.Security.Core.User.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Core.Actions.View);
                //Call
                ArtSolutions.MUN.Core.FileEngine file = new Core.FileEngine();
                return Ok(file.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.FileEngine.Get", language, ex));
            }
        }

        [HttpGet]
        [Route("api/file/GetFile")]
        public HttpResponseMessage GetFile(int id)
        {
            try
            {
                //Call
                ArtSolutions.MUN.Core.FileEngine file = new Core.FileEngine();
                MUNImagesGet_Result model = file.Get(id);

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(model.Data);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.ContentType);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/file/insert")]
        [HttpPost]
        public IHttpActionResult Insert(FileModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                //Get Headers from Request
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                FileEngine file = new FileEngine();
                model.CompanyID = companyID;
                model.ID = file.Insert(model);
                model.Data = null;
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.FileEngine.Insert", language, ex));
            }
        }
        [Route("api/file/Update")]
        [HttpPost]
        public IHttpActionResult Update(FileModel model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                //Get Headers from Request
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
                language = Request.Headers.GetValues("Language").First();
                FileEngine file = new FileEngine();
                model.CompanyID = companyID;
                file.Update(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ArtSolutions.ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.FileEngine.Update", language, ex));
            }
        }
    }
}

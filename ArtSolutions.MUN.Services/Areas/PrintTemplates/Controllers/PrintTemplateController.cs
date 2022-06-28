using ArtSolutions.MUN.Core.CaseModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.PrintTemplates.Controllers
{
    [RoutePrefix("api/printtemplate")]
    public class PrintTemplateController : ApiController
    {
        const string _featureID = "00000000-0009-0007-0001-000000000000";
        [Route("getbypaging")]
        [HttpGet]
        public IHttpActionResult Get(string filterText, int currentIndex, int pageSize, string sortColumn, string sortDirection)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                var model = new PrintTemplate().GetWithPaging(new PrintTemplate
                {
                    CompanyID = companyID,
                    FilterText = filterText,
                    CurrentIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = sortDirection,
                    Language = language
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintTemplate.GetWithPaging", language, ex));
            }
        }

        [Route("relatedstepsget")]
        [HttpGet]
        public IHttpActionResult RelatedStepsGet(string statusids)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                var model = new PrintTemplate().RelatedStepsGet(new CaseModel
                {
                    CompanyID = companyID,
                    Language = language,
                    StatusIDs = statusids
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintTemplate.ReletedStepsGet", language, ex));
            }
        }

        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert(PrintTemplate model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.New);

                var result = new PrintTemplate().Insert(new PrintTemplate
                {
                    CompanyID = companyID,
                    WorkFlowID = model.WorkFlowID,
                    Code = model.Code,
                    FileID = model.FileID,
                    TemplateName = model.TemplateName,
                    Description = model.Description,
                    StatusID = model.StatusID,
                    CreatedUserID = model.CreatedUserID,
                    CreatedDate = model.CreatedDate,
                    DataSourceID = model.DataSourceID
                });
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.New.ToString(), result.ToString(), model.TemplateName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintTemplate.Insert", language, ex));
            }
        }
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int statusid, int id = 0, int workflowid = 0)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);
                var model = new PrintTemplate().Get(new PrintTemplate
                {
                    CompanyID = companyID,
                    Language = language,
                    StatusID = statusid,
                    ID = id,
                    WorkFlowID = workflowid
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintTemplate.Get", language, ex));
            }
        }

        [HttpGet]
        [Route("datasourceget")]
        public IHttpActionResult DataSourceGet(int dataSourceID)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyId = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyId, Guid.Parse(_featureID), Actions.View);

                var returnVal = new PrintCenter().DataSourceGet(dataSourceID);

                return Ok(returnVal);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintCenter.DataSourceGet", language, ex));
            }
        }

        [Route("Update")]
        [HttpPost]
        public IHttpActionResult Update(PrintTemplate model)
        {
            Guid token = Guid.Empty;
            string language = "en";
            try
            {
                token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
                language = Request.Headers.GetValues("Language").First().ToString();
                int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.Edit);

                var result = new PrintTemplate().Update(new PrintTemplate
                {
                    ID = model.ID,
                    Code = model.Code,
                    TemplateName = model.TemplateName,
                    Description = model.Description,
                    WorkFlowID = model.WorkFlowID,
                    StatusID = model.StatusID,
                    DataSourceID = model.DataSourceID,
                    Language = language,
                    ModifiedUserID = model.ModifiedUserID,
                    ModifiedDate = model.ModifiedDate,
                    CompanyID = companyID
                });
                Common.InsertActivity(token, companyID, _featureID.ToString(), Actions.Edit.ToString(), model.ID.ToString(), model.TemplateName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.CaseModels.PrintTemplate.Update", language, ex));
            }
        }
    }
}

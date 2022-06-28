using ArtSolutions.MUN.Core;
using ArtSolutions.MUN.Core.WorkflowModels;
using ArtSolutions.MUN.Services.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace ArtSolutions.MUN.Services.Areas.Workflows.Controllers
{
    [RoutePrefix("api/WorkflowForm")]
    public class WorkflowFormController : ApiController
    {
        const string _featureID = "00000000-0011-0001-0001-000000000000";
        [HttpGet]
        [Route("GetByPaging")]
        public IHttpActionResult Get(int currentIndex, int pageSize, string sortColumn,
           int sortDirection, string filterText)
        {
            Guid token = Guid.Parse(Request.Headers.GetValues("Authorization").First());
            int companyID = Convert.ToInt32(Request.Headers.GetValues("Company").First());
            string language = Request.Headers.GetValues("Language").First().ToString();
            try
            {

                Common.ValidateUserPermission(token, companyID, Guid.Parse(_featureID), Actions.View);

                var result = new WorkflowFormModel().Get(new WorkflowFormViewModel
                {
                    Language = language,
                    CompanyID = companyID,
                    PageIndex = currentIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    Sortby = (CommonViewModel.SortDirection)sortDirection,
                    FilterText = filterText
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionHandler.Exception.Log(Guid.Parse(_featureID), token, "ArtSolutions.MUN.Core.WorkFlowModels.WorkflowFormModel.Get,", language, ex));
            }
        }
    }
}

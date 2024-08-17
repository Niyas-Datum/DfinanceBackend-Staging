using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Swashbuckle.AspNetCore.Annotations;


namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class DocTypeController : BaseController
    {
        private readonly IDocumentTypeService _obj;
        public DocTypeController(IDocumentTypeService obj)
        {
            _obj = obj;
        }
        [HttpGet(ApiRoutes.DocType.fill)]
        public IActionResult FillDocTypeMaster()
        {
            try
            {
                var view = _obj.FillDocTypeMaster();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.DocType.fillById)]
        public IActionResult FillById(int id)
        {
            try
            {
                var view = _obj.FillById(id);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.DocType.save)]
        [SwaggerOperation(Summary = "PageId=108")]
        public IActionResult SaveDocType(int pageId, DocumentTypeDto docTypeDto)
        {
            try
            {
                var view = _obj.SaveDocType(pageId, docTypeDto);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.DocType.delete)]
        public IActionResult DeleteDocType(int pageId, int Id)
        {
            try
            {
                var view = _obj.DeleteDocType(pageId, Id);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

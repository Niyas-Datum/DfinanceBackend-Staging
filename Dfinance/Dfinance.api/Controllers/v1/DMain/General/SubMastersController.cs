using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class SubMastersController : BaseController
    {
        private readonly ISubMastersService _subMastersService;
        public SubMastersController(ISubMastersService subMastersService)
        {
            _subMastersService = subMastersService;
        }

        [HttpGet(ApiRoutes.SubMasters.KeyDropDown)]
        [SwaggerOperation(Summary = "Fills the KeyDropDown")]
        public IActionResult KeyDropDown()
        {
            try
            {
                var result = _subMastersService.KeyDropDown();
                return Ok(result); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.SubMasters.FillMaster)]
        [SwaggerOperation(Summary = "FillMaster")]
        public IActionResult FillMaster(string Key)
        {
            try
            {
                var result = _subMastersService.FillMaster(Key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.SubMasters.FillSubMasterById)]
        [SwaggerOperation(Summary = "Filled by using Id")]
        public IActionResult FillSubMasterById(int? Id)
        {
            try
            {
                var result = _subMastersService.FillSubMasterById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.SubMasters.Save)]
        [SwaggerOperation(Summary = "Save SubMasters")]
        public IActionResult SaveSubMasters(SubMasterDto submasterDto, int PageId)
        {
            try
            {
                var result = _subMastersService.SaveSubMasters(submasterDto,PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.SubMasters.Update)]
        [SwaggerOperation(Summary = "Update SubMasters")]
        public IActionResult UpdateSubMasters(SubMasterDto submasterDto, int PageId)
        {
            try
            {
                var result = _subMastersService.UpdateSubMasters(submasterDto,PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.SubMasters.Delete)]
        [SwaggerOperation(Summary = "Delete SubMasters")]
        public IActionResult DeleteCounter(int Id, int PageId)
        {
            try
            {
                var result = _subMastersService.DeleteCounter(Id, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

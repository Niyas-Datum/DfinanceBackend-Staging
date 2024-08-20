using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [Authorize]
    [ApiController]
    public class SizeMasterController : BaseController
    {
        private readonly ISizeMasterService _sizeMaster;
        public SizeMasterController(ISizeMasterService sizeMaster)
        {
            _sizeMaster = sizeMaster;
        }
        [HttpGet(InvRoute.SizeMaster.fill)]
        public IActionResult FillMaster()
        {
            try
            {
                var result = _sizeMaster.FillMaster();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.SizeMaster.fillById)]
        public IActionResult FillById(int id)
        {
            try
            {
                var result = _sizeMaster.FillById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.SizeMaster.save)]
        [SwaggerOperation(Summary = "PageId=267")]
        public IActionResult Save(SizeMasterDto dto, int pageId)
        {
            try
            {
                var result = _sizeMaster.SaveSizeMaster(dto, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.SizeMaster.delete)]
        [SwaggerOperation(Summary = "PageId=267")]
        public IActionResult DeleteSizeMaster(int id, int pageId)
        {
            var result = _sizeMaster.DeleteSizeMaster(id, pageId);
            return Ok(result);
        }
    }
}

using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto.Inventory;
using Dfinance.Application.Inventory.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AreaMasterController : BaseController
    {
        private readonly IAreaMasterService _service;
        public AreaMasterController(IAreaMasterService service)
        {
            _service = service;
        }

        [HttpPost(ApiRoutes.AreaMaster.SaveAreaMaster)]
        public IActionResult SaveAreaMaster(MaAreaDto maAreaDto)
        {
            try
            {
                var result = _service.SaveAreaMaster(maAreaDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(ApiRoutes.AreaMaster.UpdateAreaMaster)]
        public IActionResult UpdateAreaMaster(MaAreaDto maAreaDto,int Id)
        {
            try
            {
                var result = _service.UpdateAreaMaster(maAreaDto, Id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.AreaMaster.DeleteAreaMaster)]
        public IActionResult DeleteAreaMaster(int id)
        {
            try
            {
                var result = _service.DeleteAreaMaster(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.AreaMaster.GetAreaMaster)]
        public IActionResult GetAreaMaster() 
        {
            try
            {
                var res = _service.FillAreaMaster();
                return Ok(res);
            }
            catch 
            {
                return BadRequest(); 
            }
        }

        [HttpGet(ApiRoutes.AreaMaster.GetAreaMasterById)]
        public IActionResult GetAreaMasterById(int id)
        {
            try
            {
                var res = _service.FillAreaMasterById(id);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

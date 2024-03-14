using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes;

using Dfinance.Warehouse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
namespace Dfinance.api.Controllers.v1.DMain.WareHouse
{
    [ApiController]
    [Authorize]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        [HttpGet(InvRoute.WareHouse.DropdownLocationTypes)]
        public IActionResult WarehouseDropdown()
        {
            try
            {
                var data = _warehouseService.WarehouseDropdown();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetAll)]
        public IActionResult WarehouseFillMaster()
        {
            try
            {
                var data = _warehouseService.WarehouseFillMaster();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetBWFill)]
        public IActionResult BranchwarehouseFill()
        {
            try
            {
                var data = _warehouseService.BranchWiseWarehouseFill();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetById)]
        [AllowAnonymous]
        public IActionResult WarehouseFillById(int id)
        {
            try
            {
                var data = _warehouseService.WarehouseFillById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.Save)]
        public IActionResult AddWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                var data = _warehouseService.Save(warehouseDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.WareHouse.Update)]
        public IActionResult UpdateWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                var data = _warehouseService.Save(warehouseDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.WareHouse.Delete)]
        public IActionResult DeleteWarehouse(int Id)
        {
            try
            {
                var data = _warehouseService.Delete(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

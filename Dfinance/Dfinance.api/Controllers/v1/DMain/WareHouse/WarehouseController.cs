using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory;
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
        private readonly IStockReportService _stockReportService;
        public WarehouseController(IWarehouseService warehouseService,IStockReportService stockReportService)
        {
            _warehouseService = warehouseService;
            _stockReportService = stockReportService;
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
        [HttpGet(InvRoute.WareHouse.StockLoadData)]
        public IActionResult StockRegistrationLoadData()
        {
            try
            {
                var data = _stockReportService.GetLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.StockRegistration)]
        public IActionResult StockRegistrationReport(StockRegistration stockRegistration)
        {
            try
            {
                var data = _stockReportService.FillItemReports(stockRegistration);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.StockItemLoadData)]
        public IActionResult StockItemRegistrationLoadData()
        {
            try
            {
                var data = _stockReportService.GetItemStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.StockItemRegistration)]
        public IActionResult StockItemRegistrationReport(ItemStockRegisterRpt itemStockRegister)
        {
            try
            {
                var data = _stockReportService.FillStockItemRegister(itemStockRegister);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

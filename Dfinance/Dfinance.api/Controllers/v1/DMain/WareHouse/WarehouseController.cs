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
        //ItemDetails
        [HttpGet(InvRoute.WareHouse.WarehouseStkLoadData)]
        public IActionResult GetWarehouseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetWarehouseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.WarehouseStkReg)]
        public IActionResult FillWarehouseStockDetails(WarehouseStockRegRpt warehouseStockReg)
        {
            try
            {
                var data = _stockReportService.FillWarehouseStockDetails(warehouseStockReg);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //StockRegisterCommoditywise
        [HttpGet(InvRoute.WareHouse.CommodityStkLoadData)]
        public IActionResult GetCommodityStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetCommudityLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.CommodityStkReg)]
        public IActionResult StockRegisterCommoditywise(CommodityStockRegRpt commodityStockReg)
        {
            try
            {
                var data = _stockReportService.FillStockRegisterCommoditywise(commodityStockReg);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //StockReceiptIssue
        [HttpGet(InvRoute.WareHouse.StockIssueLoadData)]
        public IActionResult GetReceiptIssueLoadData()
        {
            try
            {
                var data = _stockReportService.GetStockReceiptLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.StockIssueReceipt)]
        public IActionResult StockReceiptIssue(StockReceiptIssueRpt stockReceiptIssue)
        {
            try
            {
                var data = _stockReportService.FillStockReceiptIssue(stockReceiptIssue);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //UnitwiseStockRpt
        [HttpGet(InvRoute.WareHouse.UnitwiseStockLoadData)]
        public IActionResult GetUnitwiseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetUnitwiseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.UnitwiseStockRpt)]
        public IActionResult FillUnitwiseStock(UnitwiseStock unitwiseStock)
        {
            try
            {
                var data = _stockReportService.FillUnitwiseStock(unitwiseStock);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //ItemsCataloguewiseStockRpt
        [HttpGet(InvRoute.WareHouse.ItemwiseStockLoadData)]
        public IActionResult GetItemwiseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetItemwiseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.ItemwiseStockRpt)]
        public IActionResult FillItemwiseStock(ItemsCatalogue itemsCatalogue)
        {
            try
            {
                var data = _stockReportService.FillItemwiseStock(itemsCatalogue);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //monthwise Stock Rpt
        [HttpGet(InvRoute.WareHouse.MonthwiseStockLoadData)]
        public IActionResult GetMonthwiseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetMonthwiseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.MonthwiseStockRpt)]
        public IActionResult FillMonthwiseStock(MonthwiseStockRpt monthwiseStockRpt)
        {
            try
            {
                var data = _stockReportService.FillMonthwiseStock(monthwiseStockRpt);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Warehousehwise Stock Rpt
        [HttpGet(InvRoute.WareHouse.WhwiseStockLoadData)]
        public IActionResult GetWarehousewiseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetWarehousewiseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.WhwiseStockRpt)]
        public IActionResult FillWarehousewiseStock(string item,int branchId)
        {
            try
            {
                var data = _stockReportService.FillWarehousewiseStock(item,branchId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Batchwise Stock Rpt
        [HttpGet(InvRoute.WareHouse.BatchwiseStockLoadData)]
        public IActionResult GetBatchwiseStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetBatchwiseStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.BatchwiseStockRpt)]
        public IActionResult FillBatchwiseStock(BatchwiseStockRpt batchwiseStock)
        {
            try
            {
                var data = _stockReportService.FillBatchwiseStock(batchwiseStock);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Itemwise Register Rpt
        [HttpGet(InvRoute.WareHouse.ItemwiseRegLoadData)]
        public IActionResult GetItemwiseRegStockLoadData()
        {
            try
            {
                var data = _stockReportService.GetItemwiseRegStockLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.ItemwiseRegStockRpt)]
        public IActionResult FillItemwiseRegStock(ItemwiseRegRpt itemwiseStock)
        {
            try
            {
                var data = _stockReportService.FillItemwiseRegStock(itemwiseStock);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Item Stock Rpt
        [HttpGet(InvRoute.WareHouse.ItemStockLoadData)]
        public IActionResult GetItemStockRptLoadData()
        {
            try
            {
                var data = _stockReportService.GetItemStockRptLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.ItemStockRpt)]
        public IActionResult FillItemStock(ItemStockRpt itemStock)
        {
            try
            {
                var data = _stockReportService.FillItemStockRpt(itemStock);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetLocationsRpt)]
        public IActionResult GetLocations(int branchId)
        {
            try
            {
                var data = _stockReportService.FillVoucherLocations(branchId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //ItemMovementAnalysis Rpt
        [HttpGet(InvRoute.WareHouse.ItemMovAnlyLoadData)]
        public IActionResult GetItemMovementAnalysisLoadData()
        {
            try
            {
                var data = _stockReportService.GetItemMovementAnalysisLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.ItemMovAnlyRpt)]
        public IActionResult FillItemMovementAnalysis(ItemMovementAnalysis itemMovement)
        {
            try
            {
                var data = _stockReportService.FillItemMovementAnalysis(itemMovement);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetWHWiseInvLoadData)]
        public IActionResult GetWarehousewiseInventoryLoadData()
        {
            try
            {
                var data = _stockReportService.GetWarehousewiseInventoryLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.FillWHWiseInv)]
        public IActionResult FillWarehousewiseInventory(WarehouseWiseInventoryRpt warehouseWiseInventory)
        {
            try
            {
                var data = _stockReportService.FillWarehousewiseInventory(warehouseWiseInventory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.WareHouse.GetUnitStkLD)]
        public IActionResult GetUnitwiseStkLoadData()
        {
            try
            {
                var data = _stockReportService.GetUnitwiseStkLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.WareHouse.FillUnitWiseStk)]
        public IActionResult FillUnitwiseStk(int itemId)
        {
            try
            {
                var data = _stockReportService.FillUnitwiseStk(itemId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

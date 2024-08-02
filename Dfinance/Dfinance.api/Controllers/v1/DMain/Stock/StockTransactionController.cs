using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes;
using Dfinance.WareHouse.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Stock
{
    [Authorize]
    [ApiController]
    public class StockTransactionController : BaseController
    {
        private readonly IStockTransactionService _stockTrans;
        private readonly IPhyOpenStockService _physicalStock;
        private readonly IStockRtnAdjustService _stockRtnAdjust;
        public StockTransactionController(IStockTransactionService stockTrans, IPhyOpenStockService physicalStock, IStockRtnAdjustService stockRtnAdjust)
        {
            _stockTrans = stockTrans;
            _physicalStock = physicalStock;
            _stockRtnAdjust = stockRtnAdjust;
        }
        [HttpGet(InvRoute.StockTransfer.fillDamageWh)]
        [SwaggerOperation(Summary = "fills the ToWarehouse for Inventory Write Off(always Damage)")]
        public IActionResult FillDamageWH()
        {
            try
            {
                var result = _stockTrans.FillDamageWH();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.StockTransfer.Save)]
        [SwaggerOperation(Summary = "StockTransfer PageId=229 VoucherId=86, Stock_Issue PageId=248 VoucherId=95,Stock_Receipt PageId=247 VoucherId=96 ,Stock_Request PageId=534 VoucherId=163, Inventory_Write_Off PageId=287 VoucherId=108")]
        public IActionResult Save(StockTransactionDto stockDto, int voucherId, int pageId)
        {
            try
            {
                var result = _stockTrans.SaveStockTrans(stockDto, voucherId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.StockTransfer.Update)]
        [SwaggerOperation(Summary = "StockTransfer PageId=229 VoucherId=86, Stock_Issue PageId=248 VoucherId=95,Stock_Receipt PageId=247 VoucherId=96,Stock_Request PageId=534 VoucherId=163, Inventory_Write_Off PageId=287 VoucherId=108 ")]
        public IActionResult Update(StockTransactionDto stockDto, int voucherId, int pageId)
        {
            try
            {
                var result = _stockTrans.UpdateStockTrans(stockDto, voucherId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.PhyOpenStock.Save)]
        public IActionResult SavePhyOpenStock(PhyOpenStockDto phyOpenStockDto, int pageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.SavePhyOpenStock(phyOpenStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PhyOpenStock.Update)]
        public IActionResult UpdatePhyOpenStock(PhyOpenStockDto phyOpenStockDto, int pageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.UpdatePhyOpenStock(phyOpenStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.StockReturnAndAdjustment.Save)]
        [SwaggerOperation(Summary = "RtnPageId=237, RtnVoucherId=90, AdjustPageId=24, AdjustVoucherId=66")]
        public IActionResult SaveStockRtnAdjust(StockRtnAdjustDto stockRtnAdjustDto, int voucherId, int pageId)
        {
            try
            {
                var result = _stockRtnAdjust.SaveStockRtnAdjust(stockRtnAdjustDto, voucherId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.StockReturnAndAdjustment.Update)]
        [SwaggerOperation(Summary = "RtnPageId=237, RtnVoucherId=90, AdjustPageId=24, AdjustVoucherId=66")]
        public IActionResult UpdateStockRtnAdjust(StockRtnAdjustDto stockRtnAdjustDto, int voucherId, int pageId)
        {
            try
            {
                var result = _stockRtnAdjust.UpdateStockRtnAdjust(stockRtnAdjustDto, voucherId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.StockReturnAndAdjustment.Delete)]
        [SwaggerOperation(Summary = "transId=5175")]
        public IActionResult DeleteStockTransaction(int transId, int pageId, string reason)
        {
            try
            {
                var result = _stockRtnAdjust.DeleteTransactions(transId, pageId, reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.StockReturnAndAdjustment.Cancel)]
        [SwaggerOperation(Summary = "transId=5175")]
        public IActionResult CancelStockTransaction(int transId, int pageId, string reason)
        {
            try
            {
                var result = _stockRtnAdjust.CancelTransaction(transId, pageId, reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

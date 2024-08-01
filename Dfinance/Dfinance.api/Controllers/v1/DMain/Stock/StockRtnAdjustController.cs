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
    public class StockRtnAdjustController : BaseController
    {
        private readonly IStockRtnAdjustService _stockRtnAdjust;
        public StockRtnAdjustController(IStockRtnAdjustService stockRtnAdjust)
        {
            _stockRtnAdjust = stockRtnAdjust;   
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
        public IActionResult DeleteStockTransaction(int transId, int pageId,string reason)
        {
            try
            {
                var result = _stockRtnAdjust.DeleteTransactions(transId, pageId,reason);
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

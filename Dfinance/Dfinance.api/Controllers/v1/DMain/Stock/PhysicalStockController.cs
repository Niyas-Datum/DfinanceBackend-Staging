using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Routes;
using Dfinance.Stock.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Stock
{
    [ApiController]
    [Authorize]
    public class PhysicalStockController : Controller
    {
        private readonly IPhysicalStockService _physicalStock ;
        public PhysicalStockController(IPhysicalStockService physicalStock)
        {
            _physicalStock = physicalStock ;
        }
        [HttpPost(InvRoute.PhysicalStock.Save)]
        public IActionResult SavePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.SavePhysicalStock(physicalStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PhysicalStock.Update)]
        public IActionResult UpdatePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.UpdatePhysicalStock(physicalStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.PhysicalStock.Delete)]
        public IActionResult DeletePhystock(int TransId, int pageId, string Msg)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.DeletePhystock(TransId, pageId, Msg);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PhysicalStock.Cancel)]
        public IActionResult CancelPhysicalStock(int transId, string reason)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _physicalStock.CancelPhysicalStock(transId, reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

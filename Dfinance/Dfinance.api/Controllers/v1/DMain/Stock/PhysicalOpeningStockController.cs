using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes;
using Dfinance.WareHouse.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Stock
{
    [ApiController]
    [Authorize]
    public class PhysicalOpeningStockController : Controller
    {
        private readonly IPhyOpenStockService _physicalStock ;
        public PhysicalOpeningStockController(IPhyOpenStockService physicalStock)
        {
            _physicalStock = physicalStock ;
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
                var result = _physicalStock.SavePhyOpenStock(phyOpenStockDto, pageId,voucherId);
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
     
    }
}

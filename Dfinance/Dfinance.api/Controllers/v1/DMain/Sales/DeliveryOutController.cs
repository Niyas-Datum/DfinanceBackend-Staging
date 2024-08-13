using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOutController : BaseController
    {
        private readonly IDeliveryOutService _deliveryOutService;
        public DeliveryOutController(IDeliveryOutService deliveryOutService)
        {
            _deliveryOutService = deliveryOutService;
        }
        [HttpPost(InvRoute.DeliveryOut.SaveDeliveryOut)]

        public IActionResult SaveDeliveryOut([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _deliveryOutService.SaveDeliveryOut(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.DeliveryOut.UpdateDeliveryOut)]
        public IActionResult UpdateDeliveryOut([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _deliveryOutService.UpdateDeliveryOut(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
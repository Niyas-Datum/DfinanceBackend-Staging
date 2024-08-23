using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class PurchaseWithoutTaxController : BaseController
    {
        private readonly IPurchaseWithoutTaxService _obj;
        public PurchaseWithoutTaxController(IPurchaseWithoutTaxService obj)
        {
            _obj = obj;
        }
        [HttpGet(InvRoute.PurchaseWithoutTax.getData)]
        [SwaggerOperation(Summary = "Type dropdown, PriceCategory popup, Account popup,VoucherId=17")]
        public IActionResult GetData(int voucherId)
        {
            try
            {
                var data = _obj.GetData(voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.PurchaseWithoutTax.save)]
        [SwaggerOperation(Summary = "PageId=295,VoucherId=17")]
        public IActionResult SavePurchaseWithoutTax(InventoryTransactionDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.SavePurchaseWithoutTax(dto,pageId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseWithoutTax.update)]
        [SwaggerOperation(Summary = "PageId=295,VoucherId=17")]
        public IActionResult UpdatePurchaseWithoutTax(InventoryTransactionDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.UpdatePurchaseWithoutTax(dto, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

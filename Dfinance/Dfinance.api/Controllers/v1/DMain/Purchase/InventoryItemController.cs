using Dfinance.api.Framework;
using Dfinance.Inventory.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [ApiController]
    [Authorize]
    public class InventoryItemController : BaseController
    {
        private readonly IInventoryItemService _transItemsService;

        public InventoryItemController(IInventoryItemService transItemsService)
        {
             _transItemsService= transItemsService;
        }

        [HttpGet(InvRoute.InventoryItem.ItemTransData)]
        public IActionResult GetItemData(int itemId, int partyId, int voucherId)
        {
            try
            {
                var result = _transItemsService.GetItemData(itemId,partyId,voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
               
        [HttpGet(InvRoute.InventoryItem.DeleteItems)]
        public IActionResult DeleteTransItems(int transItemId)
        {
            try
            {
                var result = _transItemsService.DeleteInvTransItem(transItemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryItem.stockItems)]
        public IActionResult StockItemPopup()
        {
            try
            {
                var result = _transItemsService.StockItemPopup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

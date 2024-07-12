using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    public class PurchaseReturnController : BaseController
    {
        private readonly IPurchaseReturnService _purchaseRtnService;
        public PurchaseReturnController(IPurchaseReturnService purchaseRtnService)
        {
            _purchaseRtnService = purchaseRtnService;                
        }

        [HttpPost(InvRoute.PurchaseReturn.SavePurchaseRtn)]
        public IActionResult SavePurchaseRtn([FromBody]InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId)
        {
            try
            {
                var result = _purchaseRtnService.SavePurchaseRtn(purchaseRtnDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(InvRoute.PurchaseReturn.UpdatePurchaseRtn)]
        public IActionResult UpdatePurchaseRtn([FromBody] InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId)
        {
            try
            {
                var result = _purchaseRtnService.UpdatePurchaseRtn(purchaseRtnDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(InvRoute.PurchaseReturn.DeletePurchaseRtn)]
        public IActionResult DeletePurchaseRtn(int TransId, int pageId,bool isHigherApproval)
        {
            try
            {
                var result = _purchaseRtnService.DeletePurchaseRtn(TransId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseReturn.CancelPurchaseRtn)]
        public IActionResult CancelPurchaseRtn(int TransId, int pageId,bool isHigherApproval,string reason)
        {
            try
            {
                var result = _purchaseRtnService.CancelPurchaseRtn(TransId, pageId,reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

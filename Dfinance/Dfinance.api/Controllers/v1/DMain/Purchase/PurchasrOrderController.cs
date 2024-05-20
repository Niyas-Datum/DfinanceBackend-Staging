using Dfinance.api.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Shared.Routes;
using Dfinance.DataModels.Dto.Inventory.Purchase;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class PurchasrOrderController : BaseController
    {
        private readonly IPurchaseOrderService _purchaseOrderservice;
        public PurchasrOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderservice = purchaseOrderService;
        }
        [HttpPost(InvRoute.PurchaseOrder.SavePO)]
        public IActionResult SavePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseOrderservice.SavePurchaseOrder(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseOrder.UpdatePO)]
        public IActionResult UpdatePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseOrderservice.UpdatePurchaseOrder(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.PurchaseOrder.DeletePO)]
        public IActionResult DeletePurchaseOrder(int TransId, int PageId)
        {
            try
            {
                var data = _purchaseOrderservice.DeletePurchaseOrder( TransId,PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

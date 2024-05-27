using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class PurchaseRequestController : BaseController
    {
        private readonly IPurchaseRequestService _purchaseRequest;
        public PurchaseRequestController(IPurchaseRequestService purchaseRequest)
        {
            _purchaseRequest = purchaseRequest;
        }
        [HttpPost(InvRoute.PurchaseRequest.SavePuReq)]
        public IActionResult SavePurchaseRequest(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseRequest.SavePurchaseRequest(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseRequest.UpdatePuReq)]
        public IActionResult UpdatePurchaseRequest(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseRequest.UpdatePurchaseRequest(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.PurchaseRequest.DeletePuReq)]
        public IActionResult DeletePurchaseRequest(int PageId, int transId)
        {
            try
            {
                var data = _purchaseRequest.DeletePurchaseRequest(transId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

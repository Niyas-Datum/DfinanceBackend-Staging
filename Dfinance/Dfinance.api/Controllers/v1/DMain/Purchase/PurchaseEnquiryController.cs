using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class PurchaseEnquiryController : BaseController
    {
        private readonly IPurchaseEnquiryService _purchaseEnqservice;
        public PurchaseEnquiryController(IPurchaseEnquiryService purchaseEnqservice)
        {
            _purchaseEnqservice = purchaseEnqservice;
        }

        [HttpPost(InvRoute.PurchaseEnquiry.SavePUE)]
        public IActionResult PurchaseEnqSave(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId)
        {
            try
            {
                var result = _purchaseEnqservice.SavePurchaseEnquiry(purchaseEnqDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(InvRoute.PurchaseEnquiry.UpdatePUE)]
        public IActionResult PurchaseEnqUpdate(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId)
        {
            try
            {
                var result = _purchaseEnqservice.UpdatePurchaseEnquiry(purchaseEnqDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(InvRoute.PurchaseEnquiry.DeletePUE)]

        public IActionResult DeletePurchaseEnq(int TransId,int pageId)
        {
            try
            {
                var result = _purchaseEnqservice.DeletePurchaseEnq(TransId,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
        [HttpPatch(InvRoute.PurchaseEnquiry.CancelPUE)]

        public IActionResult CancelPurchaseEnq(int TransId,int pageId,string reason)
        {
            try
            {
                var result = _purchaseEnqservice.CancelPurchaseEnq(TransId,pageId,reason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

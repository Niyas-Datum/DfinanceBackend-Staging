using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorization.Authorize]
    [ApiController]
    public class PurchaseQuotationController : BaseController
    {
        private readonly IPurchaseQuotationService _purchaseQuotation;
        public PurchaseQuotationController(IPurchaseQuotationService purchaseQuotation)
        {
            _purchaseQuotation = purchaseQuotation;
        }
        [HttpPost(InvRoute.PurchaseQuotaion.SavePuQut)]
        public IActionResult SavePurchaseQuotation(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseQuotation.SavePurchaseQtn(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseQuotaion.UpdatePuQut)]
        public IActionResult UpdatePurchaseQuotation(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _purchaseQuotation.UpdatePurchaseQuotation(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.PurchaseQuotaion.DeletePuQut)]
        public IActionResult DeletePurchaseQuotation(int TransId, int PageId)
        {
            try
            {
                var data = _purchaseQuotation.DeletePurchaseQuotation(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PurchaseQuotaion.CancelpuQut)]
        public IActionResult CancelPurchaseQuotation(int TransId, int PageId,string reason)
        {
            try
            {
                var data = _purchaseQuotation.CancelPurchaseQuotation(TransId, PageId,reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

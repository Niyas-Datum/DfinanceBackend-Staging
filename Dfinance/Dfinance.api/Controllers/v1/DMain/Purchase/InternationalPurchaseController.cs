using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class InternationalPurchaseController : BaseController
    {
        private readonly IInternationalPurchaseService _internationalPurchase;
        public InternationalPurchaseController(IInternationalPurchaseService internationalPurchase)
        {
            _internationalPurchase = internationalPurchase;
        }
        [HttpGet(InvRoute.InternationalPurchase.GetData)]
        public IActionResult GetData(int pageId, int voucherId)
        {
            try
            {
                var data = _internationalPurchase.GetData(pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InternationalPurchase.fillitems)]
        public IActionResult FillTransItems(int partyId, int PageID, int locId, int voucherId)
        {
            try
            {
                var data = _internationalPurchase.FillTransItems(partyId, PageID, locId, voucherId).Data;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InternationalPurchase.FillInpurchase)]

        public IActionResult FillInPurchase(int pageid, bool post)
        {
            try
            {
                var data = _internationalPurchase.FillInPurchase(pageid, post);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRoute.InternationalPurchase.FillInpurchasebyid)]
        public IActionResult FillInPurschaseById(int TransId, int PageId)
        {
            try
            {
                var data = _internationalPurchase.FillInPurchaseById(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.InternationalPurchase.SaveInpurchase)]
        public IActionResult Save([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _internationalPurchase.SaveInPurchase(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.InternationalPurchase.UpdateInpurchase)]
        public IActionResult Update([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _internationalPurchase.UpdateInPurchase(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.InternationalPurchase.DelInpurchase)]
        public IActionResult Delete(int transId, int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _internationalPurchase.DeleteInPurchase(transId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

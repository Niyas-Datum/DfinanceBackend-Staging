using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;

using Dfinance.Shared.Routes;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class PurchaseController : BaseController
    {
        private readonly IPurchaseService _purchaseservice;
        public PurchaseController(IPurchaseService purchase)
        {
            _purchaseservice = purchase;
        }

        [HttpGet(InvRoute.Purchase.GetData)]
        public IActionResult GetData(int pageId, int voucherId)
        {
            try
            {
                var data = _purchaseservice.GetData(pageId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(InvRoute.Purchase.fillitems)]
        public IActionResult FillTransItems(int partyId, int PageID, int locId,int voucherId)
        {
            try
            {
                var data = _purchaseservice.FillTransItems(partyId, PageID, locId, voucherId).Data;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Purchase.Fillpurchase)]

        public IActionResult FillPurschase(int pageid, bool post)
        {
            try
            {
                var data = _purchaseservice.FillPurchase(pageid, post);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Purchase.Fillpurchasebyid)]
        
        public IActionResult FillPurschaseById(int TransId, int PageId)
        {
            try
            {
                var data = _purchaseservice.FillPurchaseById(TransId,PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(InvRoute.Purchase.Savepurchase)]
        public IActionResult Save([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _purchaseservice.SavePurchase(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Purchase.Updatepurchase)]
        public IActionResult Update([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _purchaseservice.UpdatePurchase(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.Purchase.Delpurchase)]
        public IActionResult Delete(int transId,int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _purchaseservice.DeletePurchase(transId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

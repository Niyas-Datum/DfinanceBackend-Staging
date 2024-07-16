using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReturnController : BaseController
    {
        private readonly ISalesReturnService _salesReturnService;
        public SalesReturnController(ISalesReturnService salesReturn)
        {
            _salesReturnService = salesReturn;
        }
        [HttpPost(InvRoute.SalesReturn.SaveSalesReturn)]

        public IActionResult Save([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesReturnService.SaveSalesReturn(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.SalesReturn.UpdateSalesReturn)]
        public IActionResult Update([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesReturnService.UpdateSalesReturn(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.SalesReturn.DelSalesReturn)]
        public IActionResult Delete(int transId,int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesReturnService.DeleteSalesReturn(transId,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.SalesReturn.CancelsalesRtn)]
        public IActionResult Cancel(int transId,int pageId,string reason)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesReturnService.CancelSalesReturn(transId,pageId,reason);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesEnquiryController : BaseController
    {
        private readonly ISalesEnquiryService _salesEnquiryService;
        public SalesEnquiryController(ISalesEnquiryService salesEnquiryService) 
        { 
            _salesEnquiryService=salesEnquiryService;   
        } 
        [HttpPost(InvRoute.SalesEnquiry.SaveSalesEnquiry)]

        public IActionResult SaveSalesOrder([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesEnquiryService.SaveSalesEnquiry(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.SalesEnquiry.UpdateSalesEnquiry)]
        public IActionResult UpdatSalesOrder([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesEnquiryService.UpdateSalesEnquiry(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

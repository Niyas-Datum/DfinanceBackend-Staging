using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
  
        public class SalesQuotationController : BaseController
        {
            private readonly ISalesQuotationService _salesQuotationService;
            public SalesQuotationController(ISalesQuotationService salesQuotationService)
            {
            _salesQuotationService = salesQuotationService;
            }
            [HttpPost(InvRoute.SalesQuotation.SaveSalesQuotation)]

            public IActionResult SaveSalesOrder([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    object result = _salesQuotationService.SaveSalesQuotation(salesDto, PageId, voucherId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [HttpPatch(InvRoute.SalesQuotation.UpdateSalesQuotation)]
            public IActionResult UpdatSalesOrder([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    object result = _salesQuotationService.UpdateSalesQuotation(purchaseDto, PageId, voucherId);
                    return Ok(result);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
        }
    }


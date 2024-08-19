using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class SalesEstimateController : BaseController
        {
            private readonly ISalesEstimateService _salesEstimate;
            public SalesEstimateController(ISalesEstimateService salesEstimate)
            {
                _salesEstimate = salesEstimate;
            }
            [HttpPost(InvRoute.SalesEstimate.SaveSalesEstimate)]

            public IActionResult SaveSalesEstimate([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    object result = _salesEstimate.SaveSalesEstimate(salesDto, PageId, voucherId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [HttpPatch(InvRoute.SalesEstimate.UpdateSalesEstimate)]
            public IActionResult UpdatSalesOrder([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    object result = _salesEstimate.UpdateSalesEstimate(purchaseDto, PageId, voucherId);
                    return Ok(result);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
        }
    }

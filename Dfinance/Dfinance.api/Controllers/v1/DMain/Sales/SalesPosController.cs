using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Shared.Routes;
using Dfinance.Sales.Service.Interface;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Authorize]
    [ApiController]
    public class SalesPosController : BaseController
    {
        private readonly ISalesPosService _salesPosService;
        public SalesPosController(ISalesPosService salesPos)
        {
            _salesPosService = salesPos;
        }
        [HttpPost(InvRoute.SalesPos.SaveSalesPos)]

        public IActionResult Save([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesPosService.SaveSalesPos(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

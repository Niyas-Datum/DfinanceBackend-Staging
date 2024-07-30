using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes;
using Dfinance.WareHouse.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Stock
{
    [Authorize]
    [ApiController]
    public class OpeningStockController : BaseController
    {
        private readonly IOpeningStockService _openingStockService;
        public OpeningStockController(IOpeningStockService openingStockService)
        {
            _openingStockService = openingStockService;   
        }
        [HttpPost(InvRoute.OpeningStock.Save)]
        [SwaggerOperation(Summary = "PageId=135, VoucherId=38")]
        public IActionResult SaveOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId)
        {            
            try
            {
                var result = _openingStockService.SaveOpeningStock(openingStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.OpeningStock.Update)]
        [SwaggerOperation(Summary = "PageId=135, VoucherId=38")]
        public IActionResult UpdateOpeningStock(OpeningStockDto openingStockDto, int pageId, int voucherId)
        {
            try
            {
                var result = _openingStockService.UpdateOpeningStock(openingStockDto, pageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

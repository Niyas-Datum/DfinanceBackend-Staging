using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Authorize]
    [ApiController]
    public class SalesB2BController : BaseController
    {
        private readonly ISalesB2BService _obj;
        public SalesB2BController(ISalesB2BService obj)
        {
            _obj = obj;
        }
        [HttpPost(InvRoute.Sales.saveSalesB2B)]
        [SwaggerOperation(Summary = "PageId=226, VoucherId=129")]
        public IActionResult SaveSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.SaveSalesB2B(dto, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Sales.updateSalesB2B)]
        [SwaggerOperation(Summary = "PageId=226, VoucherId=129")]
        public IActionResult UpdateSalesB2B(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.UpdateSalesB2B(dto, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

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
    public class SalesB2CController : BaseController
    {
        private readonly ISalesB2CService _obj;
        public SalesB2CController(ISalesB2CService obj)
        {
            _obj = obj;
        }
        [HttpPost(InvRoute.Sales.saveSalesB2C)]
        [SwaggerOperation(Summary = "PageId=256, VoucherId=130")]
        public IActionResult SaveSalesB2C(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.SaveSalesB2C(dto, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Sales.updateSalesB2C)]
        [SwaggerOperation(Summary = "PageId=256, VoucherId=130")]
        public IActionResult UpdateSalesB2C(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            try
            {
                var data = _obj.UpdateSalesB2C(dto, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

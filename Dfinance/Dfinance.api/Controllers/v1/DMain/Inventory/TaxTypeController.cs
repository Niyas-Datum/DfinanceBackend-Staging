using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Inventory;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class TaxTypeController : BaseController
    {
        private readonly ITaxTypeService _taxTypeService;
        public TaxTypeController(TaxTypeService taxTypeService)
        {
            _taxTypeService = taxTypeService;
        }

        [HttpPost(ApiRoutes.TaxType.SaveTaxType)]
        public IActionResult SaveCategory(TaxTypeDto taxTypeDto)
        {
            try
            {
                var result = _taxTypeService.SaveTaxType(taxTypeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiRoutes.TaxType.DeleteTaxType)]
        public IActionResult DeleteTaxType(int Id)
        {
            try
            {
                var result = _taxTypeService.DeleteTaxType(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

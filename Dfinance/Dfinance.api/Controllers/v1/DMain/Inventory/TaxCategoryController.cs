using Dfinance.api.Framework;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.Application.Services.Inventory;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class TaxCategoryController : BaseController
    {
        private readonly ITaxCategoryService _taxCategoryService;
        public TaxCategoryController(ITaxCategoryService taxCategoryService)
        {
            _taxCategoryService = taxCategoryService;
        }

        [HttpPost(ApiRoutes.TaxCat.SaveTaxCat)]
        public IActionResult SaveTaxCategory(TaxCategoryDto taxCategoryDto)
        {
            try
            {
                var result = _taxCategoryService.SaveTaxCategory(taxCategoryDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiRoutes.TaxCat.DeleteTaxCat)]
        public IActionResult DeleteTaxType(int Id)
        {
            try
            {
                var result = _taxCategoryService.DeleteTaxCat(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

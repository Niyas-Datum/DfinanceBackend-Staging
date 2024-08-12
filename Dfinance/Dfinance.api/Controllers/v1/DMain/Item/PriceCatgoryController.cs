using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [Authorize]
    [ApiController]
    public class PriceCatgoryController : BaseController
    {
        private readonly IPriceCategoryService _priceCategory;
        public PriceCatgoryController(IPriceCategoryService priceCategory)
        {
            _priceCategory = priceCategory;
        }
        [HttpGet(InvRoute.PriceCatgory.fillMaster)]
        public IActionResult FillMaster()
        {
            try
            {
                var result = _priceCategory.FillMaster();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.PriceCatgory.FillById)]
        public IActionResult FillPriceCategoryById(int Id)
        {
            try
            {
                var result = _priceCategory.FillPriceCategoryById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.PriceCatgory.save)]
        [SwaggerOperation(Summary = "PageId=244")]
        public IActionResult SavePriceCategory(PriceCategoryDto priceCategory, int PageId)
        {
            try
            {
                var result = _priceCategory.SavePriceCategory(priceCategory,PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.PriceCatgory.update)]
        public IActionResult UpdatePriceCategory(PriceCategoryDto priceCategory, int PageId)
        {
            try
            {
                var result = _priceCategory.UpdatePriceCategory(priceCategory, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.PriceCatgory.delete)]
        public IActionResult DeletePriceCategory(int Id, int PageId)
        {
            try
            {
                var result = _priceCategory.DeletePriceCategory(Id, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

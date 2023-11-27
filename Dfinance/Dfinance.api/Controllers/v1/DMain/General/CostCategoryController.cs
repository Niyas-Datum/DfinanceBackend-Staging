using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto.General;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CostCategoryController : BaseController
    {
        private readonly ICostCategoryService _costCategory;
        public CostCategoryController(ICostCategoryService costCategory)
        {
                _costCategory = costCategory;
        }

        [HttpGet(ApiRoutes.CostCategory.FillCostCategory)]
        public IActionResult FillCostCategory()
        {
            try
            {
               var result=_costCategory.FillCostCategory();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.CostCategory.FillCostCategoryById)]
        public IActionResult FillCostCategoryById(int Id)
        {
            try
            {
                var result = _costCategory.FillCostCategoryById(Id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiRoutes.CostCategory.SaveCostCategory)]
        public IActionResult SaveCostCategory(CostCategoryDto costCategoryDto)
        {
            try
            {
                var result = _costCategory.SaveCostCategory(costCategoryDto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(ApiRoutes.CostCategory.UpdateCostCategory)]
        public IActionResult UpdateCostCategory(CostCategoryDto costCategoryDto, int Id)
        {
            try
            {
                var result = _costCategory.UpdateCostCategory(costCategoryDto, Id);
                return Ok(result);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.CostCategory.DeleteCostCategory)]
        public IActionResult DeleteCostCategory(int Id)
        {
            try
            {
                var result = _costCategory.DeleteCostCategory(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.CostCategory.DropDown)]
        public IActionResult FillCostCategoryDropDown()
        {
            try
            {
                var categories = _costCategory.FillCostCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

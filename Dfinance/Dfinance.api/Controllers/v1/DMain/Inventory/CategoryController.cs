using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Inventory.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.Shared.Routes.v1;
using Dfinance.Application.Dto;
using Dfinance.Application.Dto.Inventory;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost(ApiRoutes.Category.SaveCategory)]
        public IActionResult SaveCategory(CategoryDto categoryDto)
        {
            try
            {
                var result = _categoryService.SaveCategory(categoryDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(ApiRoutes.Category.UpdateCategory)]
        public IActionResult UpdateCategory(CategoryDto categoryDto,int Id)
        {
            try
            {
                var result = _categoryService.UpdateCategory(categoryDto, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.Category.DeleteCategory)]
        public IActionResult DeleteCategory(int Id)
        {
            try
            {
                var result = _categoryService.DeleteCategory(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Category.FillCategory)]
        public IActionResult FillCategory()
        {
            try
            {
                var result= _categoryService.FillCategory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Category.FillCategoryById)]
        public IActionResult FillCategoryById(int Id)
        {
            try
            {
                var result = _categoryService.FillCategoryById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

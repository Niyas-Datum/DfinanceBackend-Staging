using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Microsoft.AspNetCore.Mvc;
using Dfinance.Shared.Routes.v1;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.Application.Services.Inventory;
using Dfinance.DataModels.Dto.Inventory;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
   
    [ApiController]
    [Authorize]
    public class CategoryTypeController : BaseController
    {
        private readonly ICategoryTypeService _categoryTypeService;
        public CategoryTypeController(ICategoryTypeService categoryTypeService)
        {
            _categoryTypeService = categoryTypeService;
        }

        /// <summary>
        /// @windows: -Inventory/masters 
		/// @Form:  CategoryType                    
        /// </summary>
        ///  <returns>Category Details</returns>
        /******************* Fill All CategoryType  ******************************/
        [HttpGet(ApiRoutes.CategoryType.FillCategoryType)]
        public IActionResult FillCategoryType()
        {
            try
            {
                var result = _categoryTypeService.FillCategoryType();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters   
		/// @Form:  CategoryType                  
        /// </summary>
        /// <param name="Id">[Id]</param>
        /// <returns>Category Details</returns>
        /************************** Fill CategoryType By Id ******************************/
        [HttpGet(ApiRoutes.CategoryType.FillCategoryTypeById)]
        public IActionResult FillCategoryTypeById(int Id)
        {
            try
            {
                var result = _categoryTypeService.FillCategoryTypeById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters   
		/// @Form:  CategoryType                  
        /// </summary>
        /// <returns>Code</returns>
        /************************ Get Next Code **********************/
        [HttpGet(ApiRoutes.CategoryType.GetNextCode)]
        public IActionResult GetNextCode()
        {
            try
            {
                var result=_categoryTypeService.GetNextCode();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Inventory/masters    
		/// @Form:  CategoryType                 
        /// </summary>
        /// <param name="categoryTypeDto"> [code : , description]</param>       
        /********************* Save Category Type  ********************/
        [HttpPost(ApiRoutes.CategoryType.SaveCategoryType)]
        public IActionResult SaveCategoryType(CategoryTypeDto categoryTypeDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _categoryTypeService.SaveCategoryType(categoryTypeDto);
                    return Ok(result);
                }
                else
                    return BadRequest(ModelState);          
            
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters   
		/// @Form:  CategoryType                
        /// </summary>
        /// <param name="categoryTypeDto"> [code : , description]</param>
      
        /************************ Update Category Type ******************************/
        [HttpPatch(ApiRoutes.CategoryType.UpdateCategoryType)]
        public IActionResult UpdateCategoryType(CategoryTypeDto categoryTypeDto, int Id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = _categoryTypeService.UpdateCategoryType(categoryTypeDto, Id);
                    return Ok(result);
                }
                else 
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters    
	   /// @Form:  CategoryType              
        /// </summary>
        /// <param> [Id] </param>       
        /************************** Delete Category Type *****************************/
        [HttpDelete(ApiRoutes.CategoryType.DeleteCategoryType)]
        public IActionResult DeleteCategoryType(int Id)
        {
            try
            {
                var result = _categoryTypeService.DeleteCategoryType(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters
		/// @Form:  Category                
        /// </summary>        
        [HttpGet(ApiRoutes.CategoryType.GetCatType)]

        public IActionResult CategoryTypePopUp()
        {

            try
            {
                var result = _categoryTypeService.CategoryTypePopUp();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Microsoft.AspNetCore.Mvc;
using Dfinance.Shared.Routes.v1;
using Dfinance.Item.Services.Inventory.Interface;
using FluentValidation;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [ApiController]
    [Authorize]
    public class ItemController : BaseController
    {
        private readonly IItemMasterService _itemService;

        public ItemController(IItemMasterService itemService)
        {
            _itemService = itemService;
        }


        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>Item Details</returns>
        /********************* Fill All Items ******************/
        [HttpGet(ApiRoutes.ItemMaster.FillItem)]
        public IActionResult FillItemMaster(int Id)
        {
            try
            {
                var result = _itemService.FillItemMaster(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet(ApiRoutes.ItemMaster.FillItemById)]
        //public IActionResult FillItemByID(int Id,int BranchId)
        //{
        //    try
        //    {
        //        var result = _itemService.FillItemByID(Id, BranchId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>next itemcode</returns>
        [HttpGet(ApiRoutes.ItemMaster.GetNextCode)]//for getting next itemcode
        public IActionResult GetNextItemCode()
        {
            try
            {
                var result = _itemService.GetNextItemCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>dropdown for branchwise taxtype </returns>
        ///  
        [HttpGet(ApiRoutes.ItemMaster.TaxDropdown)]
        public IActionResult TaxDropDown()//for branchwise taxtype dropdown
        {
            try
            {
                var result = _itemService.TaxDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>popup parent item</returns>
        ///  
        [HttpGet(ApiRoutes.ItemMaster.ParentPopup)]
        public IActionResult ParentItemPopup()//for parent item popup
        {
            try
            {
                var result = _itemService.ParentItemPopup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.ItemMaster.SaveItem)]
        public IActionResult SaveItemMaster(ItemMasterDto itemDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _itemService.SaveItemMaster(itemDto);
                    return Ok(result);
                }

                var errorMessages = ModelState.Values
                                      .SelectMany(state => state.Errors.Select(error => error.ErrorMessage))
                                      .ToList();

                return BadRequest(errorMessages);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }



        [HttpPatch(ApiRoutes.ItemMaster.UpdateItem)]
        public IActionResult UpdateItemMaster(ItemMasterDto itemDto, int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _itemService.UpdateItemMaster(itemDto, Id);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.ItemMaster.DeletItem)]
        public IActionResult DeleteItem(int Id)
        {
            try
            {
                var result = _itemService.DeleteItem(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.ItemMaster.Barcode)]
        public IActionResult GenerateBarCode()
        {
            try
            {
                var result = _itemService.GenerateBarCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet(ApiRoutes.ItemMaster.History)]
        //public IActionResult FillItemHistory(int ItemId)
        //{
        //    try
        //    {
        //        var result = _itemService.FillItemHistory(ItemId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet(ApiRoutes.ItemMaster.CurrStock)]
        //public IActionResult GetCurrentStock(int ItemId)
        //{
        //    try
        //    {
        //        var result = _itemService.GetCurrentStock(ItemId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}

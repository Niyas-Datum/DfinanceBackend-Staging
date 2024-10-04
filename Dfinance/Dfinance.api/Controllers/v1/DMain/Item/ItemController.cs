using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [ApiController]
    [Authorize]
    public class ItemController : BaseController
    {
        private readonly IItemMasterService _itemService;
        private readonly ILogger<ItemMasterService> _logger;

        public ItemController(IItemMasterService itemService, ILogger<ItemMasterService> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }


        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>Item Details</returns>
        /********************* Fill All Items ******************/
        [HttpGet(ApiRoutes.ItemMaster.FillMaster)]
        public IActionResult FillItemMaster([FromQuery] int[]? catId, [FromQuery] int[]? brandId, string search = null, int pageNo = 0, int limit = 0)
        {
            try
            {
                var response = _itemService.FillItemMaster(catId, brandId, search, pageNo, limit);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>fill item details by id,itemunits,item history,stock</returns>
        [HttpGet(ApiRoutes.ItemMaster.FillById)]
        public IActionResult FillItemByID(int pageId, int Id, int BranchId = 0)
        {
            try
            {
                var result = _itemService.FillItemByID(pageId, Id, BranchId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }



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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>save item</returns>
        ///
        [HttpPost(ApiRoutes.ItemMaster.SaveItem)]
        public IActionResult SaveItemMaster([FromBody] ItemMasterDto itemDto, int pageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _itemService.SaveItemMaster(itemDto, pageId);
                    return Ok(result);
                }

                var errorMessages = ModelState.Values
                                      .SelectMany(state => state.Errors.Select(error => error.ErrorMessage))
                                      .ToList();

                return BadRequest(errorMessages);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>update item</returns>
        ///

        [HttpPatch(ApiRoutes.ItemMaster.UpdateItem)]
        public IActionResult UpdateItemMaster(ItemMasterDto itemDto, int Id, int pageId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _itemService.UpdateItemMaster(itemDto, Id, pageId);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Inventory/masters 
        /// @Form:  ItemMaster                    
        /// </summary>
        ///  <returns>delete item</returns>
        ///
        [HttpDelete(ApiRoutes.ItemMaster.DeletItem)]
        public IActionResult DeleteItem(int Id, int pageId)
        {
            try
            {
                var result = _itemService.DeleteItem(Id, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Item used in transactions so Cannot be deleted ");
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// inv=>Report=>ItemSearch
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.Itemsearch)]

        public IActionResult GetItemSearch(int? itemId, string? value, string? criteria)
        {
            try
            {
                var result = _itemService.GetItemSearch(itemId, value, criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// inv=>Report=>ItemRegister
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.ItemRegister)]

        public IActionResult GetItemRegister(int? branchId, int? warehouseId, bool less = false, DateTime? date = null)
        {
            try
            {
                var result = _itemService.GetItemRegister(branchId, warehouseId, less, date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// GetInventoryAgeing
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="BranchID"></param>
        /// <param name="OpeningBalance"></param>
        /// <param name="VoucherID"></param>
        /// <param name="UserID"></param>
        /// <param name="Nature"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.InventoryAgeing)]

        public IActionResult GetInventoryAgeing(int? AccountID, DateTime? FromDate, DateTime? ToDate, bool? OpeningBalance, int? VoucherID, string? Nature)
        {
            try
            {
                var result = _itemService.GetInventoryAgeing(AccountID, FromDate, ToDate, OpeningBalance, VoucherID, Nature);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// GetItemExpiryReport
        /// </summary>
        /// <param name="itemExpiryReportDto"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.ItemMaster.ItemExpiryReport)]
        [AllowAnonymous]
        public IActionResult GetItemExpiryReport(ItemExpiryReportDto itemExpiryReportDto)
        {
            try
            {
                var result = _itemService.GetItemExpiryReport(itemExpiryReportDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ViewBy"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Customer"></param>
        /// <param name="Detailed"></param>
        /// <param name="Item"></param>
        /// <param name="Salesman"></param>
        /// <param name="accountid"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.InventoryProfitSP)]

        public IActionResult GetInventoryProfitSP(string? ViewBy, DateTime StartDate, DateTime EndDate, int? Customer, bool? Detailed, int Item, string? Salesman, int? accountid)
        {
            try
            {
                var result = _itemService.GetInventoryProfitSP(ViewBy, StartDate, EndDate, Customer, Detailed, Item, Salesman, accountid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewby"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="warehouse"></param>
        /// <param name="customersupplier"></param>
        /// <param name="item"></param>
        /// <param name="unit"></param>
        /// <param name="barcode"></param>
        /// <param name="orgin"></param>
        /// <param name="brand"></param>
        /// <param name="commodity"></param>
        /// <param name="branch"></param>
        /// <param name="vouchertype"></param>
        /// <param name="serialno"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.ItemHistory)]

        public IActionResult GetItemHistory(string? viewby, DateTime startdate, DateTime enddate, int? warehouse, int? customersupplier, int? item, int? unit, string? barcode, int orgin, int? brand, int? commodity, int? branch, int? vouchertype, string? serialno)
        {
            try
            {
                var result = _itemService.GetItemHistory(viewby, startdate, enddate, warehouse, customersupplier, item, unit, barcode, orgin, brand, commodity, branch, vouchertype, serialno);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouse"></param>
        /// <param name="type"></param>
        /// <param name="commodity"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.ItemMaster.ROLReport)]

        public IActionResult GetROLReport(int? warehouse, int? type, int? commodity, int? item)
        {
            try
            {
                var result = _itemService.GetROLReport(warehouse, type, commodity, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.ItemMaster.GetQuotationStatusReport)]
        [AllowAnonymous]
        public IActionResult GetQuotationStatusReport(int? VoucherId, string? VoucherNo)
        {
            try
            {
                var result = _itemService.GetQuotationStatusReport(VoucherId, VoucherNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ItemMaster.GetQuotationComparisonReport)]
        [AllowAnonymous]
        public IActionResult GetQuotationComparisonView(DateTime DateFrom, DateTime DateUpto, int BranchID, string? TransactionNo, int? AccountID, int? ItemID, int? VoucherID)
        {
            try
            {
                var result = _itemService.GetQuotationComparisonView(DateFrom, DateUpto, BranchID, TransactionNo, AccountID, ItemID, VoucherID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ItemMaster.GetPartialDelivery)]
        [AllowAnonymous]
        public IActionResult GetPartialDelivery(DateTime DateFrom, DateTime DateUpto, int branchid, bool Detailed, int? customersupplier, int? item, int? voucher, string? Criteria)
        {
            try
            {
                var result = _itemService.GetPartialDelivery(DateFrom, DateUpto, branchid, Detailed, customersupplier, item, voucher, Criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet(ApiRoutes.ItemMaster.GetMonthlyInventorySummary)]
        [AllowAnonymous]
        public IActionResult GetMonthlyInventorySummary(DateTime? startdate, DateTime? enddate, int? accountid, int? voucherid, int? drcr, int? partycategoryid, int? categorytypeid, int? commodity, int? item)
        {
            try
            {
                var result = _itemService.GetMonthlyInventorySummary(startdate, enddate, accountid, voucherid, drcr, partycategoryid, categorytypeid, commodity, item); ;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet(ApiRoutes.ItemMaster.PriceCategoryRep)]
        public IActionResult PriceCategoryReport(int? ItemId)
        {
            try
            {
                var result = _itemService.PriceCategoryReport(ItemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //ItemPopup in PriceCategoryReport
        [HttpGet(ApiRoutes.ItemMaster.ItemPopup)]
        public IActionResult ItemPopUp()
        {
            try
            {
                var result = _itemService.ItemPopUp();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet(ApiRoutes.ItemMaster.priceCatandRate)]
        public IActionResult GetPriceCatAndRate(int ItemId, string Unit)
        {
            try
            {
                var result = _itemService.GetPriceCatAndRate(ItemId,Unit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet(ApiRoutes.ItemMaster.SearchPopup)]
        //public IActionResult ItemSearchPopUp()
        //{
        //    try
        //    {
        //        var result = _itemService.ItemSearchPopup();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}     



    }
}

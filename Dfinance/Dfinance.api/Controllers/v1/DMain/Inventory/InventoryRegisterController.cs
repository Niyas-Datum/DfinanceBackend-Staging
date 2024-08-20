using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Inventory.Reports;
using Dfinance.Inventory.Reports.Interface;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class InventoryRegisterController : BaseController
    {
        private readonly IInventoryRegister _inventoryregisterService;

        public InventoryRegisterController(IInventoryRegister InventoryRegisterService)
        {
            _inventoryregisterService = InventoryRegisterService;

        }
        [HttpGet(InvRoute.InventoryRegister.FillInventoryRegister)]
        public IActionResult FillInventoryRegister(
    DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID, int? VTypeID = null,
     int? AccountID = null, int? SalesManID = null, int? ItemID = null, int? BrandID = null, int? OriginID = null,
     int? ColorID = null, int? CommodityID = null, int? LocationID = null, string Manufacturer = "",
     string GroupBy = "", int? AreaID = null, string VoucherNo = "", int? pageId = null)
        {
            try
            {
                
                var result = _inventoryregisterService.FillInventoryRegister(
                      DateFrom,DateUpto,BranchID,BasicVTypeID,VTypeID,
      AccountID,SalesManID,ItemID,BrandID,OriginID,
      ColorID,CommodityID,LocationID,Manufacturer = "",
      GroupBy= "",AreaID,VoucherNo = "",pageId=null);

                
                return Ok(result);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryRegister.PopManufacturer)]
        public ActionResult PopManufacturer(int type)
{
            try
            {

                var result = _inventoryregisterService.PopManufacturer(type);


                return Ok(result);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }
    }
}











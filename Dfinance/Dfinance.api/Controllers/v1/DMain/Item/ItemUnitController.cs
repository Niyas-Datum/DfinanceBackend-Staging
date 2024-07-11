using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Migrations;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [Authorize]
    [ApiController]
    public class ItemUnitController : BaseController
    {
        private readonly IItemUnitsService _unitService;      
        private readonly IAuthService _authService;
        public ItemUnitController(IItemUnitsService unitService, IAuthService authService)
        {
            _unitService = unitService;
          
            _authService = authService;
        }
        [HttpPatch(ApiRoutes.ItemUnits.UpdateItemUnit)]
        public IActionResult UpdateItemUnits([FromBody]List<ItemUnitsDto> units,[FromQuery] int ItemId)
        {
            try
            {
                List<int>? branch = new List<int>();
                int b=_authService.GetBranchId().Value;
                branch.Add(b);
                var response = _unitService.UpdateItemUnits(units,ItemId,branch);
                return Ok(response);
            }
            catch (Exception ex)
            {              
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ItemUnits.FillItemUnits)]
        public IActionResult FillItemUnits(int ItemId, int BranchId)
        {
            try
            {               
                var response = _unitService.FillItemUnits(ItemId, BranchId);
                return Ok(response);
            }
            catch (Exception ex)
            {               
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

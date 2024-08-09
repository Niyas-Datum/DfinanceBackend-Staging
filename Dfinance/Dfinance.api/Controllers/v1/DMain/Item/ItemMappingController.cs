using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [Authorize]
    [ApiController]
    public class ItemMappingController : BaseController
    {
        private readonly IItemMappingService _itemMapping;
        public ItemMappingController(IItemMappingService itemMapping)
        {
            _itemMapping = itemMapping;
        }
        [HttpGet(InvRoute.ItemMapping.fillItems)]
        public IActionResult FillMasterItems()
        {
            try
            {
                var result = _itemMapping.FillMasterItems();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.ItemMapping.itemDetails)]
        public IActionResult FillItemDetails(int itemId)
        {
            try
            {
                var result = _itemMapping.FillItemDetails(itemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.ItemMapping.save)]
        [SwaggerOperation(Summary = "PageId=387")]
        public IActionResult SaveItemMapping(ItemMappingDto itemMappingDto, int pageId)
        {
            try
            {
                var result = _itemMapping.SaveItemMapping(itemMappingDto,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

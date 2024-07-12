using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class GoodsInTransitController : BaseController
    {
        private readonly IGoodsInTransitService _goodsInTransit;
        public GoodsInTransitController(IGoodsInTransitService goodsInTransit)
        {            
            _goodsInTransit = goodsInTransit;
        }
        [HttpPost(InvRoute.GoodsInTransit.SaveGIT)]
        public IActionResult SaveGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _goodsInTransit.SaveGoodsInTransit(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.GoodsInTransit.UpdateGIT)]
        public IActionResult UpdateGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _goodsInTransit.UpdateGoodsInTransit(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.GoodsInTransit.DeleteGIT)]
        public IActionResult DeleteGoodsInTransit(int TransId, int PageId)
        {
            try
            {
                var data = _goodsInTransit.DeleteGoodsInTransit(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.GoodsInTransit.CancelGit)]
        public IActionResult CancelGoodsInTransit(int TransId, int PageId,string reason)
        {
            try
            {
                var data = _goodsInTransit.CancelGoodsInTransit(TransId, PageId,  reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

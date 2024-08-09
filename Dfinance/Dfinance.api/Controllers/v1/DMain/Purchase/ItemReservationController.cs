using Dfinance.api.Framework;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dfinance.Shared.Routes.InvRoute;
using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Inventory;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class ItemReservationController : BaseController
    {
        private readonly IItemReservationService _itemReservation;
        public ItemReservationController(IItemReservationService itemReservation)
        {
            _itemReservation = itemReservation;
        }
        [HttpGet(InvRoute.ItemReserv.LoadDate)]
        public IActionResult GetLoadData()
        {
            try
            {
                var data = _itemReservation.GetLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.ItemReserv.Save)]
        public IActionResult SaveItemReserv(ItemReservationDto itemReservation,int pageId,int voucherId)
        {
            try
            {
                var data = _itemReservation.SaveItemReserv(itemReservation,pageId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.ItemReserv.Update)]
        public IActionResult UpdateItemReserv(ItemReservationDto itemReservation, int pageId, int voucherId)
        {
            try
            {
                var data = _itemReservation.UpdateItemReserv(itemReservation, pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.ItemReserv.FillMaster)]
        public IActionResult FillMaster(int pageId,int? TransactionId=null)
        {
            try
            {
                var data = _itemReservation.FillMaster(pageId,TransactionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.ItemReserv.FillById)]
        public IActionResult FillById(int TransactionId )
        {
            try
            {
                var data = _itemReservation.FillById(TransactionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

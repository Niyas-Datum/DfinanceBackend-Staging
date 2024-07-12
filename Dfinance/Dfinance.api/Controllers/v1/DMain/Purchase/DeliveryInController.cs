using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class DeliveryInController : BaseController
    {
        private readonly IDeliveryInService _deliveryIn;
        public DeliveryInController(IDeliveryInService deliveryIn)
        {
            _deliveryIn = deliveryIn;
        }
        [HttpPost(InvRoute.DeliveryIn.Save)]
        public IActionResult SaveDeliveryIn(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _deliveryIn.SaveDeliveyIn(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.DeliveryIn.Update)]
        public IActionResult UpdateDeliveryIn(InventoryTransactionDto invTranseDto, int PageId, int voucherId)
        {
            try
            {
                var data = _deliveryIn.UpdateDeliveryIn(invTranseDto, PageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.DeliveryIn.Delete)]
        public IActionResult DeleteDeliveryIn(int TransId, int PageId)
        {
            try
            {
                var data = _deliveryIn.DeleteDeliveryIn(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.DeliveryIn.Cancel)]
        public IActionResult CancelDeliveryIn(int TransId, int PageId,string reason)
        {
            try
            {
                var data = _deliveryIn.CancelDeliveryIn(TransId, PageId,reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class InventoryAdditionalsController : BaseController
    {
        private readonly IInventoryAdditional _additionals;
        public InventoryAdditionalsController(IInventoryAdditional additionals)
        {
            _additionals = additionals;   
        }
        [HttpGet(InvRoute.TransactionAdditionals.GetByTransactionId)]
        public IActionResult FillTransactionAdditionals(int transactionId)
        {
            try
            {
                var data = _additionals.FillTransactionAdditionals(transactionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.TransactionAdditionals.transpType)]
        public IActionResult GetTransPortationType()
        {
            try
            {
                var data = _additionals.GetTransPortationType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.TransactionAdditionals.salesArea)]
        public IActionResult GetSalesArea()
        {
            try
            {
                var data = _additionals.GetSalesArea(); 
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.TransactionAdditionals.vehicleNo)]
        public IActionResult PopupVechicleNo()
        {
            try
            {
                var data = _additionals.PopupVechicleNo();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.TransactionAdditionals.delLoc)]
        public IActionResult PopupDelivaryLocations(int salesManId)
        {
            try
            {
                var data = _additionals.PopupDelivaryLocations(salesManId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
